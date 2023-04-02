using Emgu.CV;

namespace Drinker.DataGrab
{
    internal struct FlasksData
    {
        public FlasksState FlasksState;
        public CurMax_Numbers CharHP;
        public CurMax_Numbers CharMP;

        public bool HP_isFinded;
        public bool MP_isFinded;
        public bool FS_isFinded;

        public FlasksData(FlasksState state, CurMax_Numbers charHP, CurMax_Numbers charMP, bool HP_isFinded = true, bool MP_isFinded = true, bool FS_isFinded = true)
        {
            FlasksState = state;
            CharHP = charHP;
            CharMP = charMP;

            this.HP_isFinded = HP_isFinded;
            this.MP_isFinded = MP_isFinded;
            this.FS_isFinded = FS_isFinded;
        }

        public override string ToString()
        {
            string charHP = HP_isFinded ? CharHP.ToString() : "NA";
            string charMP = MP_isFinded ? CharMP.ToString() : "NA";
            string charFS = FS_isFinded ? FlasksState.ToString() : "NA";
            return $" Life {charHP}, Mana: {charMP}, Flasks state: {charFS}";
        }
    }
    internal static class GrabFlasksData
    {
        internal static FlasksData GrabData(Mat screen)
        {
            ImagesSetup templates;
            if (screen.Height == 1050)
                templates = NumbersTemplates.ImagesFor1050;
            else
                templates = NumbersTemplates.ImagesFor1080;

            Mat hp_area = BotFW.BotFW.GetPartOfImage(screen, NumbersFinder.hp_rect);
            Mat mp_area = BotFW.BotFW.GetPartOfImage(screen, NumbersFinder.mp_rect);

            FlasksData flasksData = new FlasksData
            {
                HP_isFinded = NumbersFinder.TryFindNumbers(hp_area, templates, out CurMax_Numbers charHP),
                CharHP = charHP,

                MP_isFinded = NumbersFinder.TryFindNumbers(mp_area, templates, out CurMax_Numbers charMP),
                CharMP = charMP,

                FS_isFinded = FlaskStateFinder.TryFindFlaskState(screen, out FlasksState flasksState),
                FlasksState = flasksState
            };

            hp_area.Dispose();
            mp_area.Dispose();

            return flasksData;
        }
    }
}
