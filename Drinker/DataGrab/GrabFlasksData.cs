using Emgu.CV;

namespace Drinker.DataGrab
{
    internal struct FlasksData
    {
        public FlasksState FlasksState;
        public CurMax_Numbers CharHP;
        public CurMax_Numbers CharMP;
        public CurMax_Numbers CharES;

        public bool HP_isFinded;
        public bool MP_isFinded;
        public bool FS_isFinded;
        public bool ES_isFinded;

        public FlasksData(FlasksState state, CurMax_Numbers charHP, CurMax_Numbers charMP, CurMax_Numbers charES, bool HP_isFinded = true, bool MP_isFinded = true, bool FS_isFinded = true, bool ES_isFinded = true)
        {
            FlasksState = state;
            CharHP = charHP;
            CharMP = charMP;
            CharES = charES;

            this.HP_isFinded = HP_isFinded;
            this.MP_isFinded = MP_isFinded;
            this.FS_isFinded = FS_isFinded;
            this.ES_isFinded = ES_isFinded;
        }

        public override string ToString()
        {
            string charHP = HP_isFinded ? CharHP.ToString() : "NA";
            string charMP = MP_isFinded ? CharMP.ToString() : "NA";
            string charES = ES_isFinded ? CharES.ToString() : "NA";
            string charFS = FS_isFinded ? FlasksState.ToString() : "NA";
            return $" Life {charHP}, Mana: {charMP}, ES: {charES}, Flasks state: {charFS}";
        }
    }
    internal static class GrabFlasksData
    {
        internal static FlasksData GrabData(Mat screen)
        {
            ImagesSetup templates;
            if (screen.Height == 1050)
                templates = NumbersTemplates.ImagesFor1050;
            else if (screen.Height == 1080)
                templates = NumbersTemplates.ImagesFor1080;
            else 
                templates = NumbersTemplates.ImagesFor983;


            var hp_area_RECT = NumbersFinder.Get_Hp_ScreenArea(screen.Width, screen.Height);
            var mp_area_RECT = NumbersFinder.Get_Mp_ScreenArea(screen.Width, screen.Height);

            Mat hp_area = BotFW.BotFW.GetPartOfImage(screen, hp_area_RECT);
            Mat mp_area = BotFW.BotFW.GetPartOfImage(screen, mp_area_RECT);

            FlasksData flasksData = new FlasksData
            {
                HP_isFinded = NumbersFinder.TryFindNumbers(hp_area, templates, out CurMax_Numbers charHP),
                CharHP = charHP,

                MP_isFinded = NumbersFinder.TryFindNumbers(mp_area, templates, out CurMax_Numbers charMP),
                CharMP = charMP,

                ES_isFinded = NumbersFinder.TryFind_ES_Numbers(hp_area, mp_area, templates, out CurMax_Numbers charES),
                CharES = charES,

                FS_isFinded = FlaskStateFinder.TryFindFlaskState(screen, out FlasksState flasksState),
                FlasksState = flasksState
            };

            hp_area.Dispose();
            mp_area.Dispose();

            return flasksData;
        }
    }
}
