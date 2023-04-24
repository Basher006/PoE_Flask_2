using BotFW.lib;
using Microsoft.Win32;
using System.IO;

namespace Drinker.BotLogic.GameClientContext
{

    public struct PoeResalution
    {
        public int WIDTH;
        public int HEIGHT;
        public string Dicription;

        public PoeResalution(int height, int width, string dicription)
        {
            WIDTH = width;
            HEIGHT = height;
            Dicription = dicription;
        }

        public override string ToString()
        {
            return Dicription;
        }
    }

    internal static class PoeInteraction
    {
        public const string GAME_CLIENT_NAME = "Path of Exile";
        public static readonly string correctFileName = "Client.txt";

        public static readonly PoeResalution[] ACCEPT_SCREEN_RES = { new PoeResalution(1050, 1920, "1920 x 1027"), new PoeResalution(1080, 1920, "1920 x 1080"), new PoeResalution(983, 1280, "1280 x 960") };


        private static readonly string logFolder = "logs\\Client.txt";
        private static readonly string registryKey_for_PoeInstallPath = "SOFTWARE\\GrindingGearGames\\Path of Exile";
        private static readonly string registryValue_for_PoeInstallPath = "InstallLocation";

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
            foreach (var scr_res in ACCEPT_SCREEN_RES)
            {
                if (gameRECT.Width == scr_res.WIDTH && gameRECT.Height == scr_res.HEIGHT)
                {
                    return true; 
                }
            }

            return false;
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
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKey_for_PoeInstallPath))
            {
                if (key != null)
                {
                    object o = key.GetValue(registryValue_for_PoeInstallPath);
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
