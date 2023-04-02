using Emgu.CV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotFW.lib
{
    public static class ImgLoader
    {
        public struct ImgSetup
        {
            public string name;
            public float threashold;
            public bool getOnlyMax;
            public RECT rect;
        }
        public static Dictionary<string, Mat> LoadImgs(string path = "imgs\\")
        {
            Dictionary<string, Mat> outputData = new Dictionary<string, Mat>();

            string[] allfiles = Directory.GetFiles(path);
            foreach (string filename in allfiles)
            {
                //CvInvoke.Imread(filename);
                //Console.WriteLine(filename);
                outputData[filename.Replace(path, "").Replace(".png", "")] = new Mat(filename, Emgu.CV.CvEnum.ImreadModes.Grayscale);
            }
            return outputData;
        }
        public static Dictionary<string, ImgSetup> GetImgSetup()
        {
            Dictionary<string, ImgSetup> outputData = new Dictionary<string, ImgSetup>();
            ImgSetup[] defloatImgSetup = GetDefloatImgSetup();
            for (int i = 0; i < defloatImgSetup.Length; i++)
            {
                outputData[defloatImgSetup[i].name] = defloatImgSetup[i];
            }

            return outputData;
        }
        private static ImgSetup[] GetDefloatImgSetup()
        {
            return new ImgSetup[]
            // kruk =       0.99
            // poklevka =   0.6732, 0.6228
            // oslablo =    0.6414
            // uspeh =      0.6726, 0.6107
            // povelo =     0.5418, 0.5416
            // uveliche =   0.5189, 0.5188
            // ne tak =     0.5710
            // rano =       ??
                { //kruk, oslablo, poklevka, povelo v storonu, uspeh, uvelichelos, ne tak, rano
                    new ImgSetup
                    {
                        name = "kruk",
                        threashold = 0.98f,
                        getOnlyMax = true,
                    },
                    new ImgSetup
                    {
                        name = "oslablo",
                        threashold = 0.7f, //59
                        getOnlyMax = true,
                    },
                    new ImgSetup
                    {
                        name = "poklevka",
                        threashold = 0.6f,
                        getOnlyMax = true,
                    },
                    new ImgSetup
                    {
                        name = "povelo v storonu",
                        threashold = 0.53f,
                        getOnlyMax = true,
                    },
                    new ImgSetup
                    {
                        name = "uspeh",
                        threashold = 0.6f,
                        getOnlyMax = true,
                    },
                    new ImgSetup
                    {
                        name = "uvelichelos",
                        threashold = 0.7f, //0.5
                        getOnlyMax = true,
                    },
                    new ImgSetup
                    {
                        name = "ne tak",
                        threashold = 0.5f,
                        getOnlyMax = true,
                    },
                    new ImgSetup
                    {
                        name = "rano",
                        threashold = 0.5f,
                        getOnlyMax = true,
                    },
                    new ImgSetup
                    {
                        name = "tashit",
                        threashold = 0.6f,
                        getOnlyMax = true,
                    },

                };
        }
    }
}
