using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;

namespace BotFW.lib
{
    internal class MouseInput
    {
        private const int INPUT_SIZE = 40;
        private const int WHEEL_TICK = 120;

        private struct INPUT
        {
            public int type; // 0 - InputMouse, 1 - InputKeyboard, 2 - InputHardware
            public MouseInputData mkhi;
        }
        //private static INPUT mouseLeftDownInput, mouseRightDownInput, mouseLeftUpInput, mouseRightUpInput, mouseMoveInput, mouseWheelInput;

        #region mouse key press and up
        public static void MouseClik(string key = "l", int sleepTime_ms = 50, bool prt = false, bool debug = false)
        {
            if (debug)
            {
                Console.WriteLine($"Hold mouse: {key} for {sleepTime_ms}ms");
                return;
            }
            if (prt)
                Console.WriteLine($"Send mouse: {key}");
            MousePressKey(key);
            Thread.Sleep(sleepTime_ms);
            MouseRealeseKey(key);
        }
        public static void MousePressKey(string key = "l", bool prt = false, bool debug = false)
        {
            if (debug)
            {
                Console.WriteLine($"Press mouse: {key}");
                return;
            }
            if (prt)
                Console.WriteLine($"Press mouse: {key}");
            {// "L", "R", "M"
                if (key == "l" || key == "L")
                    LeftDown();
                else if (key == "r" || key == "R")
                    RightDown();
                else if (key == "m" || key == "M")
                    MiddleDown();
            }
        }
        public static void MouseRealeseKey(string key = "l", bool prt = false, bool debug = false)
        {
            if (debug)
            {
                Console.WriteLine($"Realese mouse: {key}");
                return;
            }
            if (prt)
                Console.WriteLine($"Realese mouse: {key}");
            if (key == "l" | key == "L")
                LeftUp();
            else if (key == "r" | key == "R")
                RightUp();
            else if (key == "m" | key == "M")
                MiddleUp();
        }
        public static void Wheel(int ticks, bool prt = false, bool debug = false)
        {
            if (debug)
            {
                Console.WriteLine($"Mouse Wheel for: {ticks} ticks");
                return;
            }
            if (prt)
                Console.WriteLine($"Mouse Wheel for: {ticks} ticks");
            INPUT tempINPUT = new INPUT
            {
                type = 0
            };
            tempINPUT.mkhi.dwFlags = MouseEventFlags.MOUSEEVENTF_WHEEL;
            tempINPUT.mkhi.mouseData = ticks * WHEEL_TICK;
            SendInput(1, ref tempINPUT, INPUT_SIZE);
        }
        private static void LeftDown()
        {
            INPUT tempINPUT = new INPUT
            {
                type = 0
            };
            tempINPUT.mkhi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref tempINPUT, INPUT_SIZE);
        }
        private static void LeftUp()
        {
            INPUT tempINPUT = new INPUT
            {
                type = 0
            };
            tempINPUT.mkhi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput(1, ref tempINPUT, INPUT_SIZE);
        }
        private static void RightDown()
        {
            INPUT tempINPUT = new INPUT
            {
                type = 0
            };
            tempINPUT.mkhi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
            SendInput(1, ref tempINPUT, INPUT_SIZE);
        }
        private static void RightUp()
        {
            INPUT tempINPUT = new INPUT
            {
                type = 0
            };
            tempINPUT.mkhi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTUP;
            SendInput(1, ref tempINPUT, INPUT_SIZE);
        }
        private static void MiddleDown()
        {
            INPUT tempINPUT = new INPUT
            {
                type = 0
            };
            tempINPUT.mkhi.dwFlags = MouseEventFlags.MOUSEEVENTF_MIDDLEDOWN;
            SendInput(1, ref tempINPUT, INPUT_SIZE);
        }
        private static void MiddleUp()
        {
            INPUT tempINPUT = new INPUT
            {
                type = 0
            };
            tempINPUT.mkhi.dwFlags = MouseEventFlags.MOUSEEVENTF_MIDDLEUP;
            SendInput(1, ref tempINPUT, INPUT_SIZE);
        }
        #endregion
        #region mouse move tings
        public static Point GetMousePos()
        {
            var gotPoint = GetCursorPos(out Point currentMousePoint);
            if (!gotPoint) { currentMousePoint = new Point(0, 0); }
            return currentMousePoint;
        }
        public static void Move(int x, int y, bool isGlobalCoord = true, bool prt = false, bool debug = false)
        {
            if (debug)
            {
                Console.WriteLine($"Mouse move to: X {x}, Y {y}");
                return;
            }
            if (prt)
                Console.WriteLine($"Mouse move to: X {x}, Y {y}");
            if (isGlobalCoord)
                MoveTo(x, y);
            else
            {
                Point mousePoint = GetMousePos();
                MoveTo(mousePoint.X + x, mousePoint.Y + y);
            }
        }
        private static void MoveTo(int x, int y)
        {
            INPUT tempINPUT = new INPUT
            {
                type = 0
            };
            tempINPUT.mkhi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            tempINPUT.mkhi.dx = (x + 1) * 65535 / GetSystemMetrics(SystemMetric.SM_CXSCREEN);
            tempINPUT.mkhi.dy = (y + 1) * 65535 / GetSystemMetrics(SystemMetric.SM_CYSCREEN);
            SendInput(1, ref tempINPUT, INPUT_SIZE);
        }
        #endregion

        #region hz ting
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out Point lpMousePoint);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(SystemMetric smIndex);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);
        private enum SystemMetric
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1
        }
        internal struct MouseInputData
        {
            public int dx;
            public int dy;
            public int mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [Flags]
        internal enum MouseEventFlags : uint
        {
            /// <summary>
            /// Movement occurred.
            /// </summary>
            MOUSEEVENTF_MOVE = 0x0001,

            /// <summary>
            /// The WM_MOUSEMOVE messages will not be coalesced.
            /// The default behavior is to coalesce WM_MOUSEMOVE messages.
            /// Windows XP/2000:  This value is not supported.
            /// </summary>
            MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,

            /// <summary>
            /// The left button was pressed.
            /// </summary>
            MOUSEEVENTF_LEFTDOWN = 0x0002,

            /// <summary>
            /// The left button was released.
            /// </summary>
            MOUSEEVENTF_LEFTUP = 0x0004,

            /// <summary>
            /// The right button was pressed.
            /// </summary>
            MOUSEEVENTF_RIGHTDOWN = 0x0008,

            /// <summary>
            /// The right button was released.
            /// </summary>
            MOUSEEVENTF_RIGHTUP = 0x0010,

            /// <summary>
            /// The middle button was pressed.
            /// </summary>
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,

            /// <summary>
            /// The middle button was released.
            /// </summary>
            MOUSEEVENTF_MIDDLEUP = 0x0040,

            /// <summary>
            /// Maps coordinates to the entire desktop. Must be used with MOUSEEVENTF_ABSOLUTE.
            /// </summary>
            MOUSEEVENTF_VIRTUALDESK = 0x4000,

            /// <summary>
            /// An X button was pressed.
            /// </summary>
            MOUSEEVENTF_XDOWN = 0x0080,

            /// <summary>
            /// An X button was released.
            /// </summary>
            MOUSEEVENTF_XUP = 0x0100,

            /// <summary>
            /// The wheel was moved, if the mouse has a wheel. The amount of movement is specified in mouseData.
            /// </summary>
            MOUSEEVENTF_WHEEL = 0x0800,

            /// <summary>
            /// The wheel was moved horizontally, if the mouse has a wheel.
            /// The amount of movement is specified in mouseData.
            /// Windows XP/2000:  This value is not supported.
            /// </summary>
            MOUSEEVENTF_HWHEEL = 0x01000,

            /// <summary>
            /// The dx and dy members contain normalized absolute coordinates.
            /// If the flag is not set, dxand dy contain relative data (the change in position since the last reported position).
            /// This flag can be set, or not set, regardless of what kind of mouse or other pointing device, if any, is connected to the system.
            /// For further information about relative mouse motion, see the following Remarks section.
            /// </summary>
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }
        #endregion
    }
}
