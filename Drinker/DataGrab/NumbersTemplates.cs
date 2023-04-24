using Emgu.CV;

namespace Drinker.DataGrab
{
    internal struct ImagesSetup
    {
        public Template[] NumbersIMGS;
        public Template SlashIMG;
    }
    internal struct Template
    {
        public string Name;
        public Mat Img;
        public float Tr;
        public bool GiveOnlyMax;
    }
    internal static class NumbersTemplates
    {
        internal static ImagesSetup ImagesFor1050;
        internal static ImagesSetup ImagesFor1080;
        internal static ImagesSetup ImagesFor983;

        private static readonly string numbers1050Path = "imgs\\Numbers\\1050\\";
        private static readonly string numbers1080Path = "imgs\\Numbers\\1080\\";
        private static readonly string numbers983Path = "imgs\\Numbers\\983\\";

        static NumbersTemplates()
        {
            ImagesFor1050 = LoadNumbers(numbers1050Path, GetDefloutSetupForTemplates1050());
            ImagesFor1080 = LoadNumbers(numbers1080Path, GetDefloutSetupForTemplates1080());
            ImagesFor983 = LoadNumbers(numbers983Path, GetDefloutSetupForTemplates983());
        }

        private static ImagesSetup LoadNumbers(string path, Template[] defloatTemplates)
        {
            ImagesSetup output = new ImagesSetup();
            int slashImage = 1;

            for (int i = 0; i < defloatTemplates.Length - slashImage; i++)
            {
                defloatTemplates[i].Img = new Mat($"{path}{i}.png", Emgu.CV.CvEnum.ImreadModes.Grayscale);
            }

            defloatTemplates[10].Img = new Mat($"{path}s.png", Emgu.CV.CvEnum.ImreadModes.Grayscale);

            output.NumbersIMGS = defloatTemplates;
            output.SlashIMG = defloatTemplates[10];

            return output;
        }

        private static Template[] GetDefloutSetupForTemplates1050()
        {
            Template[] templates = new Template[11];
            templates[0] = new Template() { Name = "0", GiveOnlyMax = false, Tr = 0.84f };
            templates[1] = new Template() { Name = "1", GiveOnlyMax = false, Tr = 0.84f };
            templates[2] = new Template() { Name = "2", GiveOnlyMax = false, Tr = 0.84f };
            templates[3] = new Template() { Name = "3", GiveOnlyMax = false, Tr = 0.84f };
            templates[4] = new Template() { Name = "4", GiveOnlyMax = false, Tr = 0.84f };
            templates[5] = new Template() { Name = "5", GiveOnlyMax = false, Tr = 0.84f };
            templates[6] = new Template() { Name = "6", GiveOnlyMax = false, Tr = 0.84f };
            templates[7] = new Template() { Name = "7", GiveOnlyMax = false, Tr = 0.84f };
            templates[8] = new Template() { Name = "8", GiveOnlyMax = false, Tr = 0.8f };
            templates[9] = new Template() { Name = "9", GiveOnlyMax = false, Tr = 0.8f };
            templates[10] = new Template() { Name = "s", GiveOnlyMax = false, Tr = 0.84f };

            return templates;
        }

        private static Template[] GetDefloutSetupForTemplates1080()
        {
            Template[] templates = new Template[11];
            templates[0] = new Template() { Name = "0", GiveOnlyMax = false, Tr = 0.84f };
            templates[1] = new Template() { Name = "1", GiveOnlyMax = false, Tr = 0.9f };
            templates[2] = new Template() { Name = "2", GiveOnlyMax = false, Tr = 0.84f };
            templates[3] = new Template() { Name = "3", GiveOnlyMax = false, Tr = 0.84f };
            templates[4] = new Template() { Name = "4", GiveOnlyMax = false, Tr = 0.8f };
            templates[5] = new Template() { Name = "5", GiveOnlyMax = false, Tr = 0.84f };
            templates[6] = new Template() { Name = "6", GiveOnlyMax = false, Tr = 0.84f };
            templates[7] = new Template() { Name = "7", GiveOnlyMax = false, Tr = 0.84f };
            templates[8] = new Template() { Name = "8", GiveOnlyMax = false, Tr = 0.84f };
            templates[9] = new Template() { Name = "9", GiveOnlyMax = false, Tr = 0.84f };
            templates[10] = new Template() { Name = "s", GiveOnlyMax = false, Tr = 0.9f };

            return templates;
        }

        private static Template[] GetDefloutSetupForTemplates983()
        {
            Template[] templates = new Template[11];
            templates[0] = new Template() { Name = "0", GiveOnlyMax = false, Tr = 0.84f };
            templates[1] = new Template() { Name = "1", GiveOnlyMax = false, Tr = 0.9f };
            templates[2] = new Template() { Name = "2", GiveOnlyMax = false, Tr = 0.84f };
            templates[3] = new Template() { Name = "3", GiveOnlyMax = false, Tr = 0.84f };
            templates[4] = new Template() { Name = "4", GiveOnlyMax = false, Tr = 0.8f };
            templates[5] = new Template() { Name = "5", GiveOnlyMax = false, Tr = 0.84f };
            templates[6] = new Template() { Name = "6", GiveOnlyMax = false, Tr = 0.84f };
            templates[7] = new Template() { Name = "7", GiveOnlyMax = false, Tr = 0.84f };
            templates[8] = new Template() { Name = "8", GiveOnlyMax = false, Tr = 0.84f };
            templates[9] = new Template() { Name = "9", GiveOnlyMax = false, Tr = 0.84f };
            templates[10] = new Template() { Name = "s", GiveOnlyMax = false, Tr = 0.88f };

            return templates;
        }
    }
}
