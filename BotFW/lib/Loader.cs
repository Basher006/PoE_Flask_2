//using BotFW.imgs;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using static BotFW.lib.Loader;

namespace BotFW.lib
{
    public static class Loader
    {
        public struct Setup
        {
            public Dictionary<string, Mat> imgs;
            public Dictionary<string, ImgSetup> imgsSetup;
            public Dictionary<string, DataSetup> data;
            public Dictionary<string, Dictionary<string, string>> paths;
        }

        public struct ImgSetup
        {
            public string name;
            public float threashold;
            public bool getOnlyMax;
            public RECT rect;
        }
        public struct DataSetup
        {
            public string name;
            public RECT rect;
            public int skale;
            public int delta;
            public int ksize;
            public int[] lower;
            public int[] upper;
        }
        public struct Path
        {
            public string Name;
            public string[] comands;
        }

        public static Setup GetSetup()
        {
            return new Setup() 
            { 
                imgs = getImgs(),
                imgsSetup = GetImgSetup(),
                data = GetDataSetup(),
                //paths = GetDefloatPaths()
            };
        }
        private static Dictionary<string, Mat> getImgs()
        {
            Dictionary<string, Mat> outputData = new Dictionary<string, Mat>();

            string[] allfiles = Directory.GetFiles("imgs\\");
            foreach (string filename in allfiles)
            {
                //CvInvoke.Imread(filename);
                //Console.WriteLine(filename);
                outputData[filename.Replace("imgs\\", "").Replace(".png", "")] = new Mat(filename, Emgu.CV.CvEnum.ImreadModes.Grayscale);
            }
            return outputData;
        }
        private static Dictionary<string, ImgSetup> GetImgSetup()
        {
            Dictionary<string, ImgSetup> outputData = new Dictionary<string, ImgSetup>();
            ImgSetup[] defloatImgSetup = GetDefloatImgSetup();
            for (int i = 0; i < defloatImgSetup.Length; i++)
            {
                outputData[defloatImgSetup[i].name] = defloatImgSetup[i];
            }

            return outputData;
        }
        private static Dictionary<string, DataSetup> GetDataSetup() 
        {
            Dictionary<string, DataSetup> outputData = new Dictionary<string, DataSetup>();
            DataSetup[] defloatDataSetups = GetDefloatData();
            for (int i = 0; i < defloatDataSetups.Length; i++)
            {
                outputData[defloatDataSetups[i].name] = defloatDataSetups[i];
            }

            return outputData;
        }
        #region====defloat====
        private static ImgSetup[] GetDefloatImgSetup()
        {
            return new ImgSetup[]
                {
                    new ImgSetup
                    {
                        name = "boss",
                        threashold = 0.9f,
                        getOnlyMax = true,
                        rect = new RECT{X=622, Y=26, Width=55, Height=55},
                    },
                    new ImgSetup
                    {
                        name = "dead",
                        threashold = 0.99f,
                        getOnlyMax = true,
                        rect = new RECT{X=607, Y=777, Width=702, Height=92},
                    },
                    new ImgSetup
                    {
                        name = "loot",
                        threashold = 0.961f,
                        getOnlyMax = false,
                        rect = new RECT{X=100, Y=100, Width=1820, Height=790},
                    },
                    new ImgSetup
                    {
                        name = "portal",
                        threashold = 0.7f,
                        getOnlyMax = true,
                        rect = new RECT{X=41, Y=58, Width=1837, Height=812},
                    },
                    new ImgSetup
                    {
                        name = "wave_end",
                        threashold = 0.85f,
                        getOnlyMax = true,
                        rect = new RECT{X=845, Y=46, Width=232, Height=61},
                    },
                    new ImgSetup
                    {
                        name = "portal_iteract",
                        threashold = 0.92f,
                        getOnlyMax = true,
                        rect = new RECT{X=923, Y=775, Width=80, Height=80},
                    },
                    new ImgSetup
                    {
                        name = "start_interact",
                        threashold = 0.99f,
                        getOnlyMax = true,
                        rect = new RECT{X=885, Y=740, Width=150, Height=150},
                    },
                    new ImgSetup
                    {
                        name = "start_interact_2",
                        threashold = 0.99f,
                        getOnlyMax = true,
                        rect = new RECT{X=885, Y=740, Width=150, Height=150},
                    },
                    new ImgSetup
                    {
                        name = "chek_minione_LMB",
                        threashold = 0.99f,
                        getOnlyMax = true,
                        rect = new RECT{X=732, Y=880, Width=25, Height=14},
                    },
                    new ImgSetup
                    {
                        name = "chek_minione_Q",
                        threashold = 0.98f,
                        getOnlyMax = true,
                        rect = new RECT{X=818, Y=880, Width=25, Height=14},
                    },

                };
        }
        private static DataSetup[] GetDefloatData()
        {
            return new DataSetup[]
            {
                new DataSetup
                {
                    name = "boss_hp_offset",
                    rect = new RECT{X=223, Y=15, Width=474, Height=1}
                },
                new DataSetup
                {
                    name = "loot_data",
                    skale = 1,
                    delta = 0,
                    ksize = 7,
                    lower = new int[] { 0, 70, 82},
                    upper = new int[] { 180, 255, 255}
                }
            };
        }
        //private static Dictionary<string, Dictionary<string, string>> GetDefloatPaths()
        //{
        //    return new Dictionary<string, Dictionary<string, string>>
        //    {
        //        ["level_1"] = DefloatPaths.GetLevel_1(),
        //        ["level_2"] = DefloatPaths.GetLevel_2(),
        //        ["level_3"] = DefloatPaths.GetLevel_3(),
        //        ["level_4"] = DefloatPaths.GetLevel_4(),
        //        ["level_5"] = DefloatPaths.GetLevel_5(),
        //        ["level_6"] = DefloatPaths.GetLevel_6(),
        //        ["level_7"] = DefloatPaths.GetLevel_7(),
        //        ["level_8"] = DefloatPaths.GetLevel_8(),
        //        ["level_9"] = DefloatPaths.GetLevel_9(),
        //        ["level_10"] = DefloatPaths.GetLevel_10(),
        //    };
        //}
        #endregion
    }
}
