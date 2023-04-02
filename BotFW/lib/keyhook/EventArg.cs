using System;
using static BotFW.lib.keyhook.Hooking;

namespace BotFW.lib.keyhook
{
    public class KeyboardHookEventArgs : EventArgs
    {
        public LowLevelKeyboardInputEvent InputEvent
        {
            get; private set;
        }

        public KeyPressType KeyPressType
        {
            get; private set;
        }

        public KeyboardHookEventArgs(LowLevelKeyboardInputEvent inputEvent, KeyPressType keyPressType)
        {
            InputEvent = inputEvent;
            KeyPressType = keyPressType;
        }
    }
}
