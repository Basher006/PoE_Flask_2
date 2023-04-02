using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;


namespace BotFW.lib
{
    internal class KeyboardInputEmulation
    {
        private const int WM_KEYDOWNH = 0x0100;
        private const int WM_KEYUP = 0x0101;

        #region send defs
        public static void sendKey(string key, int sleepTime = 50, bool prt = false, bool debug = false)
        {//only for raw input
            if (debug)
            {
                Console.WriteLine($"Hold: {key} for {sleepTime}ms (debug)");
                return;
            }
            if (prt)
                Console.WriteLine($"Send: {key}");
            PressKey(key);
            Thread.Sleep(sleepTime);
            RealeseKey(key);
        }
        public static void PressKey(string key, bool prt = false, bool debug = false)
        {//only for raw input
            if (debug)
            {
                Console.WriteLine($"Press: {key} (debug)");
                return;
            }
            if (prt)
                Console.WriteLine($"Press: {key}");
            var inputs_dw = new KeyboardInput[]
            {
                new KeyboardInput
                {
                    wScan = rawInptCodes[key],
                    dwFlags = (uint)(KeyEventF.KeyDown | KeyEventF.Scancode),
                    dwExtraInfo = GetMessageExtraInfo()
                },
            };
            SendKeyboardInput(inputs_dw);
        }
        public static void RealeseKey(string key, bool prt = false, bool debug = false)
        {//only for raw input
            if (debug)
            {
                Console.WriteLine($"Realese: {key} (debug)");
                return;
            }
            if (prt)
                Console.WriteLine($"Realese: {key}");
            var inputs_up = new KeyboardInput[]
            {
                new KeyboardInput
                {
                    wScan = rawInptCodes[key],
                    dwFlags = (uint)(KeyEventF.KeyUp | KeyEventF.Scancode),
                    dwExtraInfo = GetMessageExtraInfo()
                }
            };
            SendKeyboardInput(inputs_up);
        }
        public static void sendKey(string keyCode, string wName, int sleepTime_ms = 50, string mode = "PostMessage", bool setForeground = false, bool prt = false)
        {//only for post and send message
            foreach (Process proc in Process.GetProcesses())
                if (proc.MainWindowTitle.Contains(wName))
                {
                    if (setForeground)
                    {
                        SetForegroundWindow(proc.MainWindowHandle);
                        Thread.Sleep(50);
                    }
                    if (mode == "SendMessage")
                    {
                        SendMessage(proc.MainWindowHandle, WM_KEYDOWNH, postMessageCodes[keyCode], 0); // not work wint Media and Блокнот

                    }
                    else if (mode == "PostMessage")
                    {
                        PostMessage(proc.MainWindowHandle, WM_KEYDOWNH, postMessageCodes[keyCode], 0); // не работает с Блокнот, работает с Media
                        if (prt)
                            Console.WriteLine($"pressed: {keyCode} to {proc.MainWindowTitle} with {mode} mode.");
                    }

                    Thread.Sleep(sleepTime_ms);

                    if (mode == "SendMessage")
                    {
                        SendMessage(proc.MainWindowHandle, WM_KEYUP, postMessageCodes[keyCode], 0); // not work wint Media and Блокнот
                        if (prt)
                            Console.WriteLine($"realesed: {keyCode} to {proc.MainWindowTitle} with {mode} mode.");
                    }
                    else if (mode == "PostMessage")
                    {
                        PostMessage(proc.MainWindowHandle, WM_KEYUP, postMessageCodes[keyCode], 0); // не работает с Блокнот, работает с Media
                        if (prt)
                            Console.WriteLine($"realesed: {keyCode} to {proc.MainWindowTitle} with {mode} mode.");
                    }
                }
        }
        public static void PressKey(string keyCode, string wName, string mode = "PostMessage", bool setForeground = false)
        {
            foreach (Process proc in Process.GetProcesses())
                if (proc.MainWindowTitle.Contains(wName))
                {
                    if (setForeground)
                    {
                        SetForegroundWindow(proc.MainWindowHandle);
                        Thread.Sleep(50);
                    }
                    if (mode == "SendMessage")
                        SendMessage(proc.MainWindowHandle, WM_KEYDOWNH, postMessageCodes[keyCode], 0); // not work wint Media and Блокнот
                    else if (mode == "PostMessage")
                        PostMessage(proc.MainWindowHandle, WM_KEYDOWNH, postMessageCodes[keyCode], 0); // не работает с Блокнот, работает с Media
                }
        }
        public static void RealeseKey(string keyCode, string wName, string mode = "PostMessage", bool setForeground = false)
        {
            foreach (Process proc in Process.GetProcesses())
                if (proc.MainWindowTitle.Contains(wName))
                {
                    if (setForeground)
                    {
                        SetForegroundWindow(proc.MainWindowHandle);
                        Thread.Sleep(50);
                    }
                    if (mode == "SendMessage")
                        SendMessage(proc.MainWindowHandle, WM_KEYUP, postMessageCodes[keyCode], 0); // not work wint Media and Блокнот
                    else if (mode == "PostMessage")
                        PostMessage(proc.MainWindowHandle, WM_KEYUP, postMessageCodes[keyCode], 0); // не работает с Блокнот, работает с Media
                }
        }
        #endregion
        #region DLLCall
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
        [DllImport("user32.dll")]
        static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr point);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        #endregion
        #region structs
        public struct POINT
        {
            public int X;
            public int Y;
        }
        [Flags]
        private enum MouseEventF
        {
            Absolute = 0x8000,
            HWheel = 0x01000,
            Move = 0x0001,
            MoveNoCoalesce = 0x2000,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            VirtualDesk = 0x4000,
            Wheel = 0x0800,
            XDown = 0x0080,
            XUp = 0x0100
        }
        [Flags]
        private enum KeyEventF
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008
        }
        [Flags]
        private enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }
        private struct Input
        {
            public int type;
            public InputUnion u;
        }
        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public MouseInput mi;
            [FieldOffset(0)] public KeyboardInput ki;
            [FieldOffset(0)] public HardwareInput hi;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct HardwareInput
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardInput
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        #endregion
        #region key kodes
        public static Dictionary<string, int> postMessageCodes = new Dictionary<string, int>
        {
            {"Esc", 0x1b },
            {"f1", 0x70 },
            {"f2", 0x71 },
            {"f3", 0x72 },
            {"f4", 0x73 },
            {"f5", 0x74 },
            {"f6", 0x75 },
            {"f7", 0x76 },
            {"f8", 0x77 },
            {"f9", 0x78 },
            {"f10", 0x79 },
            {"f11", 0x7a },
            {"f12", 0x7b },
            {"~", 0xc0 },
            {"1", 0x31 },
            {"2", 0x32 },
            {"3", 0x33 },
            {"4", 0x34 },
            {"5", 0x35 },
            {"6", 0x36 },
            {"7", 0x37 },
            {"8", 0x38 },
            {"9", 0x39 },
            {"0", 0x30 },
            {"-", 0xBD },
            {"=", 0xBB },
            {"backspace", 0x08 },
            {"tab", 0x09 },
            {"q", 0x51 },
            {"w", 0x57 },
            {"e", 0x45 },
            {"r", 0x52 },
            {"t", 0x54 },
            {"y", 0x59 },
            {"u", 0x55 },
            {"i", 0x49 },
            {"o", 0x4f },
            {"p", 0x50 },
            {"[", 0xDB },
            {"]", 0xDD },
            {"enter", 0x0D },
            {"ctrl", 0x11 },
            {"capslock", 0x14 },
            {"a", 0x41 },
            {"s", 0x53 },
            {"d", 0x44 },
            {"f", 0x46 },
            {"g", 0x47 },
            {"h", 0x48 },
            {"j", 0x4a },
            {"k", 0x4b },
            {"l", 0x4c },
            {";", 0xba },
            {"\'", 0xDE },
            {"\\", 0xE2 },
            {"shift", 0x10 },
            {"lshift", 0xA0 },
            {"rshift", 0xA1 },
            {"z", 0x5a },
            {"x", 0x58 },
            {"c", 0x43 },
            {"v", 0x56 },
            {"b", 0x42 },
            {"n", 0x4e },
            {"m", 0x4d },
            {",", 0xBC },
            {".", 0xBE },
            {"/", 0xBF },
            {"alt", 0x12 },
            {"space", 0x20 },
            {"num1", 0x61 },
            {"num2", 0x62 },
            {"num3", 0x63 },
            {"num4", 0x64 },
            {"num5", 0x65 },
            {"num6", 0x66 },
            {"num7", 0x67 },
            {"num8", 0x68 },
            {"num9", 0x69 },
            {"num0", 0x60 },
            {"num*", 0x6A },
            {"num-", 0x6D },
            {"num+", 0x6B },
            {"numdel", 0x6C },
        };
        public static Dictionary<string, ushort> rawInptCodes = new Dictionary<string, ushort>
        {
            {"Esc", 0x01 },
            {"f1", 0x3b },
            {"f2", 0x3c },
            {"f3", 0x3d },
            {"f4", 0x3e },
            {"f5", 0x3f },
            {"f6", 0x40 },
            {"f7", 0x41 },
            {"f8", 0x42 },
            {"f9", 0x43 },
            {"f10", 0x44 },
            {"f11", 0x57 },
            {"f12", 0x58 },
            {"~", 0x29 },
            {"1", 0x02 },
            {"2", 0x03 },
            {"3", 0x04 },
            {"4", 0x05 },
            {"5", 0x06 },
            {"6", 0x07 },
            {"7", 0x08 },
            {"8", 0x09 },
            {"9", 0x0a },
            {"0", 0x0b },
            {"-", 0x0c },
            {"=", 0x0d },
            {"backspace", 0x0e },
            {"tab", 0x0f },
            {"q", 0x10 },
            {"w", 0x11 },
            {"e", 0x12 },
            {"r", 0x13 },
            {"t", 0x14 },
            {"y", 0x15 },
            {"u", 0x16 },
            {"i", 0x17 },
            {"o", 0x18 },
            {"p", 0x19 },
            {"[", 0x1a },
            {"]", 0x1b },
            {"enter", 0x1c },
            {"ctrl", 0x1d },
            {"capslock", 0x3a },
            {"a", 0x1e },
            {"s", 0x1f },
            {"d", 0x20 },
            {"f", 0x21 },
            {"g", 0x22 },
            {"h", 0x23 },
            {"j", 0x24 },
            {"k", 0x25 },
            {"l", 0x26 },
            {";", 0x27 },
            {"\'", 0x28 },
            {"\\", 0x2b },
            {"shift", 0x2a },
            {"lshift", 0x2a },
            {"rshift", 0x36 },
            {"z", 0x2c },
            {"x", 0x2d },
            {"c", 0x2e },
            {"v", 0x2f },
            {"b", 0x30 },
            {"n", 0x31 },
            {"m", 0x32 },
            {",", 0x33 },
            {".", 0x34 },
            {"/", 0x35 },
            {"alt", 0x38 },
            {"space", 0x39 },
            {"num1", 0x4f },
            {"num2", 0x50 },
            {"num3", 0x51 },
            {"num4", 0x4b },
            {"num5", 0x4c },
            {"num6", 0x4d },
            {"num7", 0x47 },
            {"num8", 0x48 },
            {"num9", 0x49 },
            {"num0", 0x52 },
            {"num*", 0x37 },
            {"num-", 0x4a },
            {"num+", 0x4e },
            {"numdel", 0x53 },
            {"up",  0x148 },
            {"left", 0x14B },
            {"right", 0x14D  },
            {"down", 0x150 },
        };
        #endregion
        #region private defs
        private static void SendKeyboardInput(KeyboardInput[] kbInputs)
        {
            Input[] inputs = new Input[kbInputs.Length];

            for (int i = 0; i < kbInputs.Length; i++)
            {
                inputs[i] = new Input
                {
                    type = (int)InputType.Keyboard,
                    u = new InputUnion
                    {
                        ki = kbInputs[i]
                    }
                };
            }

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
        }
        #endregion
    }
}