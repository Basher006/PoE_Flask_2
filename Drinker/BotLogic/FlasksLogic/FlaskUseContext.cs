using Drinker.DataGrab;
using FlaskSetup;
using System;
using Timer = BotFW.lib.Timer;

namespace Drinker.BotLogic
{
    internal class FlaskUseContext
    {
        public delegate void UpdateFlaskSetUp(FlaskSettings setUp);
        public delegate void UpdateFlaskSetUp_arr(FlasksSetupData setUp);
        public delegate void UpdDelegate(FlasksData data);
        public UpdDelegate Upd;
        public UpdateFlaskSetUp UpdFlaskSetUp;
        public UpdateFlaskSetUp UpdFlaskSetUp_arr;
        public int flaskNumber;

        public FlaskSettings setUp;


        private static readonly string activationType_HP = FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.HP];
        private static readonly string activationType_MP = FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.MP];
        private static readonly string activationType_ES = FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.ES];
        private static readonly string activationType_KD = FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.CD];
        private static readonly string activationType_One = FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.One];


        private Timer flaskTimer;
        private Timer oneUseTimer;
        private static readonly int oneUseTimer_tr = 1000; // one second


        public FlaskUseContext(FlaskSettings setUp, int flaskNumber) 
        { 
            this.setUp = setUp;
            this.flaskNumber = flaskNumber;
            UpdFlaskSetUp = new UpdateFlaskSetUp(UpdateSetUp);
            UpdFlaskSetUp_arr = new UpdateFlaskSetUp(UpdateSetUp);
            Upd = new UpdDelegate(Update);

            flaskTimer = new Timer();
            oneUseTimer = new Timer();
        }

        public void Update(FlasksData data) 
        {
            if (FlaskIsNeedUsed(data))
                UseFlask();
        }

        public bool FlaskChekTimer()
        {
            return flaskTimer.Chek((int)(setUp.MinCD * 1000f));

        }
        public bool FlaskIsNeedUsed(FlasksData data)
        {
            if (!flaskTimer.Chek((int) (setUp.MinCD * 1000f))) // if false > return, else > do
                return false;

            bool result = false;
            if (setUp.ActivationType == activationType_HP)
            {
                result = ChekHP(data);
            }
            else if (setUp.ActivationType == activationType_MP)
            {
                result = ChekMP(data);
            }
            else if (setUp.ActivationType == activationType_ES)
            {
                result = ChekES(data);
            }
            else if (setUp.ActivationType == activationType_KD)
            {
                result = ChekState(data);
            }
            else if (setUp.ActivationType == activationType_One)
            {
                result = ChekOneUse(data);
            }

            return result;
        }

        public void UseFlask()
        {
            SendKey();
            flaskTimer.Upd();
        }

        public void UpdateSetUp(FlaskSettings setUp) 
        {
            this.setUp = setUp;
        }
        public void UpdateSetUp(FlasksSetupData setUp)
        {
            var setUpArr = setUp.ToArray();
            UpdateSetUp(setUpArr[flaskNumber - 1]);
        }

        private bool ChekHP(FlasksData data)
        {
            int charHPpercent = CalcCharHP_percent(data.CharHP.Cur, data.CharHP.Max);
            if (data.HP_isFinded && charHPpercent <= setUp.ActivatePercent && ChekState(data))
            {
                return true;
            }
            return false;
        }

        private bool ChekMP(FlasksData data)
        {
            int charMPpercent = CalcCharHP_percent(data.CharMP.Cur, data.CharMP.Max);
            if (data.MP_isFinded && charMPpercent <= setUp.ActivatePercent && ChekState(data))
            {
                return true;
            }
            return false;
        }

        private bool ChekES(FlasksData data)
        {
            int charESpercent = CalcCharHP_percent(data.CharES.Cur, data.CharES.Max);
            bool ststs = ChekState(data);
            bool sfsdv = data.ES_isFinded && charESpercent <= setUp.ActivatePercent && ststs;
            if (sfsdv)
            {
                return true;
            }
            return false;
        }

        private bool ChekOneUse(FlasksData data)
        {
            var flaskState = data.FlasksState.ToArray()[flaskNumber-1];
            if (flaskState >= FlasksMetrics.MINIMUM_STATE_PERCENT_WHEN_USE_FLASK && data.HP_isFinded && data.FS_isFinded)
            {
                oneUseTimer.Upd();
                return false;
            }
            else if (oneUseTimer.Chek(oneUseTimer_tr))
            {
                oneUseTimer.Upd();
                return true;
            }
            return false;
        }

        private bool ChekState(FlasksData data)
        {
            var flaskState = data.FlasksState.ToArray()[flaskNumber - 1];
            if (flaskState <= FlasksMetrics.MINIMUM_STATE_PERCENT_WHEN_USE_FLASK && data.HP_isFinded && data.FS_isFinded)
            {
                return true;
            }
            return false;
        }

        private void SendKey()
        {
            if (FlasksMetrics.TryGetHotKeyByName(setUp.HotKey, out FlaskHotkey _))
                BotFW.BotFW.SendKey(setUp.HotKey, debug: Program.debug, prt:Program.prt);
            //Console.WriteLine($"Sended: {key}");
            else
                Console.WriteLine("HOTKEY ERROR!");
        }

        private int CalcCharHP_percent(int curHP, int maxHP)
        {
            float m = maxHP / (float)curHP;

            return (int)(100 / m);
        }
    }
}
