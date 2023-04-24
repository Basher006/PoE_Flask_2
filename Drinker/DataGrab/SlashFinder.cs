using Emgu.CV.Structure;
using Emgu.CV;
using static BotFW.lib.cv2;
using System.Drawing;

namespace Drinker.DataGrab
{
    internal static class SlashFinder
    {
        private static MCvScalar slashMask_lower = new MCvScalar(191, 209, 197);
        private static MCvScalar slashMask_upper = new MCvScalar(256, 256, 256);

        internal static Point Find_HP_or_MP_Slash(Mat screen, Template slashTempl)
        {
            imgFindRes res = GetSlashes_FromScreen(screen, slashTempl);
            Point point = GetTopMostSlash(res);

            return point;
        }

        internal static Point Find_ES_Slash(Mat screen, Template slashTempl)
        {
            imgFindRes res = GetSlashes_FromScreen(screen, slashTempl);
            Point point = GetBottomMostSlash(res);

            return point;
        }

        internal static bool EnergyShieldIsCoverLife(Mat lifeAreaScreen, Template slashTempl)
        {
            imgFindRes lifeRes = GetSlashes_FromScreen(lifeAreaScreen, slashTempl);
            return lifeRes.result.Length == 2;
        }

        private static Point GetTopMostSlash(imgFindRes FindedSlash)
        {
            int maxY_index = 0;
            for (int i = 0; i < FindedSlash.result.Length; i++)
            {
                if (FindedSlash.result[i].Y < maxY_index)
                    maxY_index = i;
            }
            return FindedSlash.result[maxY_index];
        }

        private static Point GetBottomMostSlash(imgFindRes FindedSlash)
        {
            int minY_index = 0;
            for (int i = 0; i < FindedSlash.result.Length; i++)
            {
                if (FindedSlash.result[i].Y > minY_index)
                    minY_index = i;
            }
            return FindedSlash.result[minY_index];
        }

        private static imgFindRes GetSlashes_FromScreen(Mat Screen, Template templ)
        {
            Mat mask = BotFW.BotFW.ApplyMask(Screen, slashMask_lower, slashMask_upper);
            //Screen.Save("screen.png");
            //mask.Save("templ.png");
            imgFindRes result = BotFW.BotFW.Img_find_s(mask, templ.Img, templ.Tr, giveOnlyMax: false);
            //mask.Dispose();
            return result;
        }
    }
}
