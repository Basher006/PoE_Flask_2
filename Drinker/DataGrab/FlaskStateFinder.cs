using BotFW.lib;
using Emgu.CV;
using Emgu.CV.Structure;


namespace Drinker.DataGrab
{
    internal struct FlasksState
    {
        public int Slot_1, Slot_2, Slot_3, Slot_4, Slot_5;

        public FlasksState(int[] flasks_percent)
        {
            Slot_1 = flasks_percent[0];
            Slot_2 = flasks_percent[1];
            Slot_3 = flasks_percent[2];
            Slot_4 = flasks_percent[3];
            Slot_5 = flasks_percent[4];
        }
        public override string ToString()
        {
            return $"{Slot_1}, {Slot_2}, {Slot_3}, {Slot_4}, {Slot_5}";
        }
        public int[] ToArray()
        {
            return new int[] { Slot_1, Slot_2, Slot_3, Slot_4, Slot_5 };
        }
    }
    internal static class FlaskStateFinder
    {
        private const int FLASKS_COUNT = 5;

        private static readonly RECT flaskStateRECT_1050 = new RECT(290, 1042, 220, 1);
        private static readonly RECT flaskStateRECT_1080 = new RECT(306, 1073, 230, 1);
        private static readonly RECT flaskStateRECT_983 = new RECT(273, 977, 205, 1);

        private static readonly MCvScalar flaskState_lower = new MCvScalar(100, 100, 100);
        private static readonly MCvScalar flaskState_upper = new MCvScalar(255, 255, 255);

        private static readonly int eachFlaskWidth_1050 = 44;
        private static readonly int eachFlaskWidth_1080 = 46;
        private static readonly int eachFlaskWidth_983 = 41;

        private static readonly int maxFlaskStateLineWidth_1050 = 32;
        private static readonly int maxFlaskStateLineWidth_1080 = 34;
        private static readonly int maxFlaskStateLineWidth_983 = 31;



        internal static bool TryFindFlaskState(Mat screen, out FlasksState flasksState)
        {
            RECT flaskStateRECT;
            int eachFlaskWidth;
            int maxFlaskStateLineWidth;
            if (screen.Height == 1050)
            {
                eachFlaskWidth = eachFlaskWidth_1050;
                flaskStateRECT = flaskStateRECT_1050;
                maxFlaskStateLineWidth = maxFlaskStateLineWidth_1050;
            } 
            else if (screen.Height == 1080)
            {
                flaskStateRECT = flaskStateRECT_1080;
                eachFlaskWidth = eachFlaskWidth_1080;
                maxFlaskStateLineWidth = maxFlaskStateLineWidth_1080;
            }
            else
            {
                flaskStateRECT = flaskStateRECT_983;
                eachFlaskWidth = eachFlaskWidth_983;
                maxFlaskStateLineWidth = maxFlaskStateLineWidth_983;
            }

            var flaskStateArea_pixels = GetPixels(screen, flaskStateRECT);
            int imageLen = flaskStateArea_pixels.Width;

            int[] flaskState_result = GetAbsoluteFlasksState(imageLen, flaskStateArea_pixels, eachFlaskWidth);
            int[] result_percent = GetFlasksStatePercent(flaskState_result, maxFlaskStateLineWidth);
            
            flasksState = new FlasksState(result_percent);
            return true;
        }

        private static Image<Gray, byte> GetPixels(Mat screen, RECT flaskStateRECT)
        {
            Mat flaskStateArea = BotFW.BotFW.GetPartOfImage(screen, flaskStateRECT);
            Mat flaskStateArea_mask = BotFW.BotFW.ApplyMask(flaskStateArea, flaskState_lower, flaskState_upper);
            var pixels = flaskStateArea_mask.ToImage<Gray, byte>();

            flaskStateArea.Dispose();
            flaskStateArea_mask.Dispose();
            
            return pixels;
        }

        private static int[] GetAbsoluteFlasksState(int imageLen, Image<Gray, byte> flaskStateArea_pixels, int eachFlaskWidth)
        {
            int y = 0;
            int chanel = 0;
            int[] flaskState_result = new int[FLASKS_COUNT];
            for (int x = 0; x < imageLen; x++)
            {
                var pixel1 = flaskStateArea_pixels.Data[y, x, chanel];
                if (pixel1 != 0)
                {
                    int flask;
                    if (x == 0)
                        flask = 0;
                    else
                        flask = x / eachFlaskWidth;
                    flaskState_result[flask] += 1;
                }
            }
            flaskStateArea_pixels.Dispose();

            return flaskState_result;
        }

        private static int[] GetFlasksStatePercent(int[] flaskState_result, int maxFlaskStateLineWidth)
        {
            float fs_percent;
            int[] result_percent = new int[FLASKS_COUNT];
            for (int i = 0; i < FLASKS_COUNT; i++)
            {
                if (flaskState_result[i] != 0)
                    fs_percent = flaskState_result[i] / (float)maxFlaskStateLineWidth;
                else
                    fs_percent = 0;

                result_percent[i] = (int)(100 * fs_percent);
            }

            return result_percent;
        }
    }
}