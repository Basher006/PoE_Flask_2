using BotFW.lib;
using BotFW.lib.keyhook;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;
using static BotFW.lib.ImgLoader;

namespace BotFW
{
    public static class BotFW
    {
        #region =====cv2=====
        public static Size defloatKsize_for_blur = cv2.defloatKsize_for_blur;
        public static Size defloatKernel_for_erode = cv2.defloatKernel_for_erode;
        public static cv2.imgFindRes Img_find_s(Mat screen, Mat templ, float tr, bool giveOnlyMax = true, Emgu.CV.CvEnum.TemplateMatchingType method = Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed)
        {
            if (giveOnlyMax)
                return cv2.img_find_max_s(screen, templ, tr, 2, method);
            else
                return cv2.img_find_all_s(screen, templ, tr, method);
        }
        public static Point[] Img_find(Mat screen, Mat templ, float tr, bool giveOnlyMax = true, bool prtMax = false, Emgu.CV.CvEnum.TemplateMatchingType method = Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed)
        {
            if (giveOnlyMax)
                return cv2.img_find_max(screen, templ, tr, prtMax, 2, method);
            else
                return cv2.img_find_all(screen, templ, tr, prtMax, method);
        }
        public static Mat AppplyBlur(Mat img, Size ksize, double sigmaX = 1.0, double sigmaY = 1.0)
        {
            return cv2.AppplyBlur(img, ksize, sigmaX, sigmaY);
        }
        public static Mat ApplyErode(Mat img, Size Kernel, int iterations = 1)
        {
            return cv2.ApplyErode(img, Kernel, iterations);
        }
        public static Mat ApplyContrastBrightness(Mat img, float contrast = 1.0f, int brightness = 0)
        {
            return cv2.ApplyContrastBrightness(img, contrast, brightness);
        }
        public static Mat ApplyMask(Mat img, MCvScalar lower, MCvScalar upper, bool convert_to_hsv = false)
        {
            return cv2.ApplyMask(img, lower, upper, convert_to_hsv);
        }
        public static Mat ConvertToGray(Mat img)
        {
            return cv2.ConvertToGray(img);
        }
        public static void ConvertToGray(Mat img, Mat outImage)
        {
            cv2.ConvertToGray(img, outImage);
        }
        public static Mat ConvertToColor(Mat img)
        {
            return cv2.ConvertToColor(img);
        }
        public static Mat ConvertToHSV(Mat img)
        {
            return cv2.ConvertToHSV(img);
        }
        public static Bitmap Convert_Mat_To_Bitmap(Mat img)
        {
            return cv2.Convert_Mat_To_Bitmap(img);
        }
        public static Mat Convert_Bitmap_To_Mat(Bitmap img)
        {
            return cv2.Convert_Bitmap_To_Mat(img);
        }
        public static Mat Convert_to_conturs(Mat img, int ksize = 3, double skale = 1.0, double delta = 0.0)
        {
            return cv2.Convert_to_conturs(img, ksize, skale, delta);
        }
        public static List<Rectangle> FindAreaConturs(Mat img, int areaTrMin, int areaTrMax, int periTrMin, int periTRMax,
            RetrType mode = RetrType.External, ChainApproxMethod method = ChainApproxMethod.ChainApproxNone)
        {
            return cv2.FindAreaConturs(img, areaTrMin, areaTrMax, periTrMin, periTRMax, mode, method);
        }
        public static List<Rectangle> FindAreaConturs(Mat img, Mat draw_img, MCvScalar color, int areaTrMin, int areaTrMax, int periTrMin, int periTRMax,
            RetrType mode = RetrType.External, ChainApproxMethod method = ChainApproxMethod.ChainApproxNone)
        {
            return cv2.FindAreaConturs(img, draw_img, color, areaTrMin, areaTrMax, periTrMin, periTRMax, mode, method);
        }
        #endregion

        #region =====game=====
        public static RECT GetStandartOffsetedRect(RECT rc)
        {
            return GameThings.GetStandartOffsetedRect(rc);
        }
        public static void CutForStandartOffset(Bitmap bitmap, Bitmap outBitmap)
        {
            GameThings.CutForStandartOffset(bitmap, outBitmap);
        }
        public static RECT GetGameRect(IntPtr Hwnd)
        {
            return GameThings.GetGameRect(Hwnd);
        }
        public static RECT GetGameRect(string wName)
        {
            return GameThings.GetGameRect(wName);
        }
        public static IntPtr WinGetHandle(string wName)
        {
            return GameThings.WinGetHandle(wName);
        }
        public static void GetScreen(IntPtr hWnd, Bitmap bitmap)
        {
            GameThings.GetBitmapFromHWID_2(hWnd, bitmap);
        }
        public static void GetScreen(IntPtr Hwnd, RECT rc, Bitmap bitmap)
        {
            GameThings.GetScreen(Hwnd, rc, bitmap);
        }
        public static void GetScreen(RECT rc, Bitmap OutBitmap)
        {
            GameThings.GetScreen(rc, OutBitmap);
        }
        //public static Bitmap GetScreen(RECT rc)
        //{
        //    return GameThings.GetScreen(rc);
        //}
        public static void GetPartOfImage(Bitmap bitmap, RECT rc, Bitmap outBitmap)
        {
            GameThings.GetPartOfImage(bitmap, rc, outBitmap);
        }
        public static Mat GetPartOfImage(Mat img, RECT rc)
        {
            return GameThings.GetPartOfImage(img, rc);
        }
        public static bool GameWindowIsActive(string wName)
        {
            return GameThings.GameWindowIsActive(wName);
        }
        #endregion

        #region =====keyboard=====
        public static Dictionary<string, ushort> rawInptCodes = KeyboardInputEmulation.rawInptCodes;
        public static void SendKey(string key, int sleepTime = 50, bool prt = false, bool debug = false)
        { // только прописные!
            KeyboardInputEmulation.sendKey(key, sleepTime, prt, debug);
        }
        public static void PressKey(string key, bool prt = false, bool debug = false)
        {
            KeyboardInputEmulation.PressKey(key, prt, debug); ;
        }
        public static void RealeseKey(string key, bool prt = false, bool debug = false)
        {
            KeyboardInputEmulation.RealeseKey(key, prt, debug);
        }
        public static void SendKey(string keyCode, string wName, int sleepTime_ms = 50, string mode = "PostMessage", bool setForeground = false)
        {
            KeyboardInputEmulation.sendKey(keyCode, wName, sleepTime_ms, mode, setForeground);
        }
        public static void PressKey(string keyCode, string wName, string mode = "PostMessage", bool setForeground = false)
        {
            KeyboardInputEmulation.PressKey(keyCode, wName, mode, setForeground);
        }
        public static void RealeseKey(string keyCode, string wName, string mode = "PostMessage", bool setForeground = false)
        {
            KeyboardInputEmulation.RealeseKey(keyCode, wName, mode, setForeground);
        }
        #endregion

        #region =====mouse=====
        public static void MouseClik(string key = "l", int sleepTime_ms = 50, bool prt = false, bool debug = false)
        {
            MouseInput.MouseClik(key, sleepTime_ms, prt, debug);
        }
        public static void MousePressKey(string key = "l", bool prt = false, bool debug = false)
        {// "L", "R", "M"
            MouseInput.MousePressKey(key, prt, debug);
        }
        public static void MouseRealeseKey(string key = "l", bool prt = false, bool debug = false)
        {// "L/l", "R/r", "M/m"
            MouseInput.MouseRealeseKey(key, prt, debug);
        }
        public static void MouseMove(int x, int y, bool isGlobalCoord = true, bool prt = false, bool debug = false)
        {
            MouseInput.Move(x, y, isGlobalCoord, prt, debug);
        }
        public static void Wheel(int ticks, bool prt = false, bool debug = false)
        {
            MouseInput.Wheel(ticks, prt, debug);
        }
        #endregion

        #region =====hook=====
        public static void AddHook(Key key, Action callbak, bool suppres = false)
        {
            AddHook_i.AddHook(key, callbak, suppres);
        }
        public static void AddHook(List<Key> key, Action callbak, bool suppres = false)
        {
            AddHook_i.AddHook(key, callbak, suppres);
        }
        #endregion
        #region====defs====
        public static Dictionary<string, Mat> LoadImgs(string path = "imgs\\")
        {
            return ImgLoader.LoadImgs(path);
        }
        public static Dictionary<string, ImgSetup> GetImgSetup()
        {
            return ImgLoader.GetImgSetup();
        }
        private static double ConvertRadiansToDegrees(double Radians)
        {
            double degrees = ( 180 /  Math.PI ) * Radians;
            return degrees;
        }
        private static double ConvertDegreesToRadians(double degrees)
        {
            double radians = ( Math.PI / 180) * degrees;
            return radians;
        }
        public static Point Vector_2_xy(VectorCord crd)
        {
            double vector = crd.deg;
            int leng = crd.leng;
            double angBC;
            string flag;

            int x,y;

            if (vector > 270) 
            {
                angBC = vector - 270;
                flag = "-x";
            }
            else if (vector > 180)
            {
                angBC = vector - 180; // *
                angBC = 91 - angBC;
                flag = "-xy";
            }
            else if (vector > 90)
            {
                angBC = vector - 90;
                flag = "-y";
            }
            else
            {
                angBC = 91 - vector;
                flag = "";
            }

            x = (int)Math.Round(leng * Math.Cos(ConvertDegreesToRadians(angBC)));
            y = (int)Math.Round(leng * Math.Cos(ConvertDegreesToRadians(180 - (90 + angBC))));

            switch (flag)
            {
                case "-x":
                    x *= -1;
                    break;
                case "-xy":
                    x *= -1;
                    y *= -1;
                    break;
                case "-y":
                    y *= -1;
                    break;

                default:
                    break;
            }
            return new Point(x, y);
        }
        public static VectorCord xy_2_vector(Point point)
        {
            double a, b, c, XL_ang, vecor_out, x, y;

            x = point.X; y = point.Y;
            if (x == 0)
                x = 0.01;
            if (y == 0)
                y = 0.01;
            a = Math.Abs(y);
            c = Math.Abs(x);
            b = Math.Pow(Math.Pow(x, 2) + Math.Pow(y, 2), 0.5f);

            XL_ang = Math.Acos( ( Math.Pow(a, 2) - Math.Pow(b, 2) - Math.Pow(c, 2) ) / ( -2 * b * c) );

            vecor_out = ConvertRadiansToDegrees( XL_ang );

            if (y < 0 && x > 0)
                vecor_out += 90;
            else if (y < 0 && x < 0)
                vecor_out = 91 - vecor_out + 180;
            else if (y > 0 && x < 0)
                vecor_out += 270;
            else
                vecor_out = 91 - vecor_out;

            return new VectorCord { deg = vecor_out, leng = (int)Math.Round(b) };
        }

        #endregion
        #region====structs====
        public static RECT monitor_1 = new RECT()
        {
            X = 0,
            Y = 0,
            Width = 1920,
            Height = 1080
        };
        public static RECT monitor_2 = new RECT()
        {
            X = 1920,
            Y = 0,
            Width = 1920,
            Height = 1080
        };
        public struct VectorCord
        {
            public double deg;
            public int leng;
        }
        #endregion
    }
}
