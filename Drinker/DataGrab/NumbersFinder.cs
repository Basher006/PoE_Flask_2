using System.Collections.Generic;
using System.Drawing;
using BotFW.lib;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Drinker.DataGrab
{
    internal struct FindedNumber
    {
        public int Number;
        public Point Pos;
    }
    internal static class NumbersFinder
    {
        internal static readonly RECT hp_rect = new RECT(60, 760, 240, 120);
        internal static readonly RECT mp_rect = new RECT(1719, 760, 200, 120);

        internal static readonly Point hp_offset = new Point(0, -190);
        internal static readonly Point mp_offset = new Point(200, -190);

        private static readonly Point numbersOffset = new Point(58, 17);
        private static readonly MCvScalar numbersMask_lower = new MCvScalar(191, 209, 197);
        private static readonly MCvScalar numbersMask_upper = new MCvScalar(256, 256, 256);

        internal static RECT Get_Hp_ScreenArea(int screen_W, int screen_H)
        {
            return new RECT(
                hp_offset.X,
                screen_H + hp_offset.Y - hp_rect.Height,
                hp_rect.Width,
                hp_rect.Height);
        }

        internal static RECT Get_Mp_ScreenArea(int screen_W, int screen_H)
        {
            return new RECT
                (screen_W - mp_offset.X,
                screen_H + mp_offset.Y - mp_rect.Height,
                mp_rect.Width,
                mp_rect.Height);
        }

        internal static bool TryFindNumbers(Mat screen, ImagesSetup templates, out CurMax_Numbers curMax_Numbers)
        {
            curMax_Numbers = new CurMax_Numbers();
            Point slash_location = SlashFinder.FindSlash(screen, templates.SlashIMG);
            if (slash_location.X == -1)
                return false;

            int cur, max;

            cur = GetNumbersLine(screen, slash_location, templates, false);
            max = GetNumbersLine(screen, slash_location, templates, true);

            curMax_Numbers.Cur = cur;
            curMax_Numbers.Max = max;
            return true;
        }

        private static int GetNumbersLine(Mat screen, Point slash_location, ImagesSetup templates, bool forMax)
        {
            var result_RECT = GetRECT_For_Numbers(slash_location, numbersOffset, forMax);
            Mat result_img_gray = BotFW.BotFW.ConvertToGray(screen);
            Mat result_img = BotFW.BotFW.GetPartOfImage(result_img_gray, result_RECT);


            var rawNumbers = GetAll_RawNumbers(result_img, templates.NumbersIMGS);
            var sortedNumbers = SortByX(rawNumbers);

            result_img_gray.Dispose();
            result_img.Dispose();
            return sortedNumbers;
        }

        private static List<FindedNumber> GetAll_RawNumbers(Mat img, Template[] numbers)
        {
            List<FindedNumber> numbersList = new List<FindedNumber>();
            for (int i = 0; i < numbers.Length; i++)
            {
                Mat mask = BotFW.BotFW.ApplyMask(img, numbersMask_lower, numbersMask_upper);
                var ress = BotFW.BotFW.Img_find_s(mask, numbers[i].Img, numbers[i].Tr, giveOnlyMax: false);
                mask.Dispose();
                foreach (var res in ress.result)
                {
                    if (res.X > -1)
                        numbersList.Add(new FindedNumber { Number = i, Pos = res });
                }
            }
            
            return numbersList;
        }

        private static int SortByX(List<FindedNumber> numbersList, string tempRes = "")
        {
            int minimumX = int.MaxValue;
            int minimumNumber = 0;
            FindedNumber minimumIndex = new FindedNumber();
            bool flag = false;
            foreach (var item in numbersList)
            {
                if (item.Pos.X < minimumX)
                {
                    minimumX = item.Pos.X;
                    minimumIndex = item;
                    minimumNumber = item.Number;
                    flag = true;
                }
            }
            tempRes += minimumNumber;
            if (flag)
                numbersList.Remove(minimumIndex);

            if (numbersList.Count > 0)
                return SortByX(numbersList, tempRes);
            else
                return int.Parse(tempRes);
        }

        private static RECT GetRECT_For_Numbers(Point slashLocation, Point numbersOffset, bool forMax)
        {
            int AdditionalOffsetFor_Y = -2;
            int AdditionalOffsetFor_X = 5;
            int x, y, w, h;

            if (forMax)
            {
                x = slashLocation.X + AdditionalOffsetFor_X;
                y = slashLocation.Y + AdditionalOffsetFor_Y;
            }
            else // for current
            {
                x = slashLocation.X - numbersOffset.X;
                y = slashLocation.Y + AdditionalOffsetFor_Y;
            }

            w = numbersOffset.X;
            h = numbersOffset.Y;

            return new RECT(x, y, w, h);
        }
    }
}
