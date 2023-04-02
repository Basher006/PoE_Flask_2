using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using Emgu.CV;

namespace BotFW.lib
{
    public struct RECT
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public RECT(int x, int y, int w, int h)
        {
            X = x; 
            Y = y;
            Width = w;
            Height = h;
        }

        public override string ToString() => $"X: {X}, Y: {Y}, Width: {Width}, Height: {Height}.";
    }
    public class GameThings
    {
        static public RECT standartOffset = new RECT { X = -8, Y = -8, Width = -16, Height = -16 };
        public static bool GameWindowIsActive(string wName)
        {
            string cirrentTowWindow = ActiveWindowTitle();
            return cirrentTowWindow == wName;
        }
        public static RECT GetGameRect(IntPtr Hwnd)
        {
            GetWindowRect(Hwnd, out RECT rc);
            rc.Width -= rc.X; rc.Height -= rc.Y;

            if (rc.Width != 1920)
            {
                rc.Width -= 16;
                rc.X += 8;
            }
            if (rc.Height != 1080)
            {
                rc.Height -= 16;
                rc.Y += 8;
            }

            return rc;
        }
        public static RECT GetGameRect(string wName)
        {
            IntPtr Hwnd = WinGetHandle(wName);
            return GetGameRect(Hwnd);
        }
        public static RECT GetStandartOffsetedRect(RECT rc)
        {
            RECT outRECT = new RECT { X = rc.X, Y = rc.Y, Height = rc.Height - 16, Width = rc.Width - 16 };
            return outRECT;
        }
        public static void GetScreen(IntPtr Hwnd, RECT rc, Bitmap bitmap)
        {
            GetBitmapFromHWID(Hwnd, rc, ref bitmap);
        }
        public static void GetScreen(RECT rc, Bitmap bitmap)
        {
            GetBitmapFromRect(rc, bitmap);
        }
        public static void CutForStandartOffset(Bitmap bitmap, Bitmap outBitmap)
        {
            using (var g = Graphics.FromImage(outBitmap))
            {
                g.DrawImage(bitmap, 0, 0, new Rectangle { X = 8, Y = 8, Width = bitmap.Width - 16, Height = bitmap.Height - 16 }, GraphicsUnit.Pixel);
            }
        }
        public static void GetPartOfImage(Bitmap bitmap, RECT rc, Bitmap outBitmap)
        {
            using (var g = Graphics.FromImage(outBitmap))
            {
                g.DrawImage(bitmap, 0, 0, new Rectangle { X = rc.X, Y = rc.Y, Width = rc.Width, Height = rc.Height }, GraphicsUnit.Pixel);
            }
        }
        public static Mat GetPartOfImage(Mat img, RECT rc)
        {
            Rectangle newrect = new Rectangle { X = rc.X, Y = rc.Y, Height = rc.Height, Width = rc.Width };
            try
            {
                return new Mat(img, newrect);
            }
            catch (Exception)
            {
                Console.WriteLine("GetPartOfImage ERROR!");
                return new Mat(rc.Width, rc.Height, img.Depth, img.NumberOfChannels);
            }
        }
        public static IntPtr WinGetHandle(string wName) //битмап по частичному имени окна
        {
            foreach (Process pList in Process.GetProcesses())
                if (pList.MainWindowTitle.Contains(wName))
                    return pList.MainWindowHandle;
            return IntPtr.Zero;
        }
        private static void GetBitmapFromRect(RECT rc, Bitmap bitmap) // HERE!
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(rc.X, rc.Y, 0, 0, bitmap.Size);
            }
        }
        #region DLLCall

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT Rect);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hwnd, StringBuilder ss, int count);
        [DllImport("user32.dll")]
        static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);
        #endregion
        public static void GetBitmapFromHWID_2(IntPtr hWnd, Bitmap bitmap)
        {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    var hdcBitmap = graphics.GetHdc();
                    PrintWindow(hWnd, hdcBitmap, 0);
                    graphics.ReleaseHdc(hdcBitmap);
                }
                //bitmap.Save("screen11.png", ImageFormat.Png);
        }
        private static void GetBitmapFromHWID(IntPtr hWnd, RECT rc, ref Bitmap bitmap) //получаем бит мап из хендла
        {
            rc.X = 0;
            //GetClientRect(hWnd, out rc); //получаем его размеры
            IntPtr hWndDc = GetDC(hWnd); //получаем рабочий стол
            IntPtr hMemDc = CreateCompatibleDC(hWndDc); //получаем полный рабочий стол
            IntPtr hBitmap = CreateCompatibleBitmap(hWndDc, (rc.Width - rc.X), (rc.Height - rc.Y));

            IntPtr oldBitmap = SelectObject(hMemDc, hBitmap);

            SelectObject(hMemDc, hBitmap); //тут опять та самая магия с гдип

            BitBlt(hMemDc, 0, 0, (rc.Width - rc.X), (rc.Height - rc.Y), hWndDc, 0, 0, TernaryRasterOperations.SRCCOPY);
            bitmap = Image.FromHbitmap(hBitmap); //и ещё немножко магии

            //bitmap.Save("screen2.png");

            DeleteObject(hBitmap);
            ReleaseDC(hWnd, hWndDc);
            ReleaseDC(IntPtr.Zero, hMemDc);

            SelectObject(hMemDc, oldBitmap);
            DeleteDC(hMemDc);
        }
        //private static void GetBitmapFromHWID(IntPtr hWnd, RECT rc, out Bitmap bitmap) //получаем бит мап из хендла
        //{
        //    rc.X = 0;
        //    //GetClientRect(hWnd, out rc); //получаем его размеры
        //    IntPtr hWndDc = GetDC(hWnd); //получаем рабочий стол
        //    IntPtr hMemDc = CreateCompatibleDC(hWndDc); //получаем полный рабочий стол
        //    IntPtr hBitmap = CreateCompatibleBitmap(hWndDc, (rc.Width - rc.X), (rc.Height - rc.Y));

        //    IntPtr oldBitmap = SelectObject(hMemDc, hBitmap);

        //    SelectObject(hMemDc, hBitmap); //тут опять та самая магия с гдип

        //    BitBlt(hMemDc, 0, 0, (rc.Width - rc.X), (rc.Height - rc.Y), hWndDc, 0, 0, TernaryRasterOperations.SRCCOPY);
        //    bitmap = Image.FromHbitmap(hBitmap); //и ещё немножко магии

        //    //bitmap.Save("screen2.png");

        //    DeleteObject(hBitmap);
        //    ReleaseDC(hWnd, hWndDc);
        //    ReleaseDC(IntPtr.Zero, hMemDc);

        //    SelectObject(hMemDc, oldBitmap);
        //    DeleteDC(hMemDc);
        //}
        private static string ActiveWindowTitle()
        {
            //Create the variable
            const int nChar = 256;
            StringBuilder ss = new StringBuilder(nChar);

            //Run GetForeGroundWindows and get active window informations
            //assign them into handle pointer variable
            IntPtr handle = IntPtr.Zero;
            handle = GetForegroundWindow();

            if (GetWindowText(handle, ss, nChar) > 0) return ss.ToString();
            else return "";
        }
        public enum TernaryRasterOperations : uint //тут какая то жижа с модом, не трогай
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062
        }
    }
}
