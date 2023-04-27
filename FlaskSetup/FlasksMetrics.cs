using System.Windows.Input;

namespace FlaskSetup
{
    public struct FlaskHotkey
    {
        public Key HotKey { get; set; }
        public string HotKeyName { get; set; }

        public FlaskHotkey(Key hotKey, string hotKeyName)
        {
            HotKey = hotKey;
            HotKeyName = hotKeyName;
        }
        public override string ToString()
        {
            return HotKeyName;
        }
    }

    public static class FlasksMetrics
    {
        public const int FLASKS_COUNT = 5;
        public const int MINIMUM_STATE_PERCENT_WHEN_USE_FLASK = 5;

        public static readonly string[] ActivatorDropBoxValues = new string[] { "Нет", "HP", "Мана", "ES", "По Кд", "1 раз" };
        public static readonly string[] GroupDropBoxValues = new string[] { "Нет", "Group1", "Group2" };

        public static readonly FlaskHotkey[] AlowedHotkeys = { 
            new FlaskHotkey(Key.D1, "1"), new FlaskHotkey(Key.D2, "2"), new FlaskHotkey(Key.D3, "3"), new FlaskHotkey(Key.D4, "4"), new FlaskHotkey(Key.D5, "5"),
            new FlaskHotkey(Key.D6, "6"), new FlaskHotkey(Key.D7, "7"), new FlaskHotkey(Key.D8, "8"), new FlaskHotkey(Key.D9, "9"), new FlaskHotkey(Key.D0, "0"),

            new FlaskHotkey(Key.NumPad1, "NumPad 1"), new FlaskHotkey(Key.NumPad2, "NumPad 2"), new FlaskHotkey(Key.NumPad3, "NumPad 3"), new FlaskHotkey(Key.NumPad4, "NumPad 4"), new FlaskHotkey(Key.NumPad5, "NumPad 5"),
            new FlaskHotkey(Key.NumPad6, "NumPad 6"), new FlaskHotkey(Key.NumPad7, "NumPad 7"), new FlaskHotkey(Key.NumPad8, "NumPad 8"), new FlaskHotkey(Key.NumPad9, "NumPad 9"), new FlaskHotkey(Key.NumPad0, "NumPad 0"),
        };
        public static readonly string[] AlowedHotkeys_names = SetNamesForHotkeys(AlowedHotkeys);



        public static bool TryGetHotKeyByName(string name, out FlaskHotkey result)
        {
            foreach (var hk in AlowedHotkeys)
            {
                if (hk.HotKeyName == name)
                {
                    result = hk;
                    return true;
                }
                    
            }
            result = new FlaskHotkey();
            return false;
        }
        private static string[] SetNamesForHotkeys(FlaskHotkey[] alowedHotkeys)
        {
            string[] hotkeysNames = new string[alowedHotkeys.Length];

            for (int i = 0; i < alowedHotkeys.Length; i++)
            {
                hotkeysNames[i] = alowedHotkeys[i].ToString();
            }
            
            return hotkeysNames;
        }

        public enum ActivationType
        {//{ "Нет", "HP", "Мана", "ES", "По Кд", "1 раз" };
            Not = 0,
            HP = 1,
            MP = 2,
            ES = 3,
            CD = 4,
            One = 5,
        }
        public enum groupDB
        {
            Not = 0,
            Group1 = 1,
            Group2 = 2,
        }
    }
}
