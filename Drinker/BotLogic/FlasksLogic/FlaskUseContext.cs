using Drinker.DataGrab;
using BotFW.lib;
using FlaskSetup;
using System;
using System.Windows.Input;
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


        private Timer flaskTimer;

        public FlaskSettings setUp;

        private static readonly string activationType_HP = FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.HP];
        private static readonly string activationType_MP = FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.MP];
        private static readonly string activationType_KD = FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.KD];


        public FlaskUseContext(FlaskSettings setUp, int flaskNumber) 
        { 
            this.setUp = setUp;
            this.flaskNumber = flaskNumber;
            UpdFlaskSetUp = new UpdateFlaskSetUp(UpdateSetUp);
            UpdFlaskSetUp_arr = new UpdateFlaskSetUp(UpdateSetUp);
            Upd = new UpdDelegate(Update);

            flaskTimer = new Timer();
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
            else if (setUp.ActivationType == activationType_KD)
            {
                result = ChekState(data);
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
            //Console.WriteLine($"flask {flaskNumber} has updated");
        }
        public void UpdateSetUp(FlasksSetupData setUp)
        {
            var setUpArr = setUp.ToArray();
            UpdateSetUp(setUpArr[flaskNumber - 1]);
            //this.setUp = setUpArr[flaskNumber-1];
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
            if (data.HP_isFinded && charMPpercent <= setUp.ActivatePercent && ChekState(data))
            {
                return true;
            }
            return false;
        }

        private bool ChekState(FlasksData data)
        {
            var flaskState = data.FlasksState.ToArray()[flaskNumber-1];
            if (flaskState <= FlasksMetrics.MINIMUM_STATE_PERCENT_WHEN_USE_FLASK)
                return true;
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
