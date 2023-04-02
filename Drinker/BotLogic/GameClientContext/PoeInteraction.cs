using BotFW.lib;


namespace Drinker.BotLogic.GameClientContext
{
    internal static class PoeInteraction
    {
        public const string GAME_CLIENT_NAME = "Path of Exile";

        public static RECT GetGameRect()
        {
            return BotFW.BotFW.GetGameRect(GAME_CLIENT_NAME);
        }

        public static bool GameWindowIsActive()
        {
            return BotFW.BotFW.GameWindowIsActive(GAME_CLIENT_NAME);
        }
    }
}
