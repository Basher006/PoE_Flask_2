using BotFW.lib;
using Microsoft.Win32;
using System.IO;
using System.Linq;

namespace Drinker.BotLogic.GameClientContext
{
    internal static class PoeInteraction
    {
        public const string GAME_CLIENT_NAME = "Path of Exile";
        private static readonly string logFolder = "logs\\Client.txt";
        public static readonly string correctFileName = "Client.txt";

        public static readonly int[] ACCEPT_SCREEN_HEIGHT = new int[] { 1050, 1080 };
        public static readonly int[] ACCEPT_SCREEN_WIDTH = new int[] { 1920 };

        public static RECT GetGameRect()
        {
            var dfsv = BotFW.BotFW.GetGameRect(GAME_CLIENT_NAME);
            return dfsv;
        }

        public static bool GameWindowIsActive()
        {
            return BotFW.BotFW.GameWindowIsActive(GAME_CLIENT_NAME);
        }

        public static bool GameWindowIsValideResolution(RECT gameRECT)
        {
            return ACCEPT_SCREEN_HEIGHT.Contains(gameRECT.Height) && ACCEPT_SCREEN_WIDTH.Contains(gameRECT.Width);
        }


        public static bool TryGetPOELogFolder(out string logPath)
        {
            string path;
            logPath = "";
            if (TryGetPOEInstallPathFromRegistry(out path))
            {
                path += logFolder;
                if (IsValidePOELogFolder(path))
                {
                    logPath = path;
                    return true;
                }
            }
            return false;
        }

        private static bool TryGetPOEInstallPathFromRegistry(out string path)
        {
            path = "";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\GrindingGearGames\\Path of Exile"))
            {
                if (key != null)
                {
                    object o = key.GetValue("InstallLocation");
                    if (o != null)
                    {
                        path = (string)o;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsValidePOELogFolder(string path)
        {
            if (File.Exists(path))
                return true;

            return false;
        }
    }
}
