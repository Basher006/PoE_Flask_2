using BotFW.lib;
using Emgu.CV;
using System;
using Drinker.DataGrab;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using Drinker.GUI;
using Drinker.BotLogic;
using System.Windows.Input;
using Drinker.BotLogic.GameClientContext;
using System.Windows.Forms;
using Drinker.Properties;

namespace Drinker
{
    struct CurMax_Numbers
    {
        public int Max;
        public int Cur;

        public override string ToString()
        {
            return $"{Cur}/{Max}";
        }
    }
    internal class Program
    {
        public static Thread Form1Thread;
        public static Thread Form2Thread;
        public static Thread OverlayThread;
        public static bool Run = false;
        public static bool Pause = false;


        private static RECT game_rect = new RECT();
        private static Bitmap screen_bm;
        private static Mat screen;
        private static bool gameWindowIsActive;
        private static bool gameWindowIsActive_old;
        private static bool gameWindowIsValidResalution;
        private static bool usePausa = Settings.Default.UsePausa;

        public static bool debug = false;
        public static bool prt = true;

        [STAThread]
        static void Main(string[] args)
        {
            string poeLogPAth;
            if (PoeInteraction.TryGetPOELogFolder(out poeLogPAth))
            {
                Console.WriteLine(poeLogPAth);
            }
            

            BotFW.BotFW.AddHook(Key.F4, StartStop, true);
            Form2ThreadStart();
            OverlayThreadStart();

            Thread.Sleep(200); // wait forms loaded (~60ms)

            bool chekBoxIsCheked = (bool)GUIRuner.form2.Invoke(GUIRuner.form2.getChangeOverlay);
            GUIRuner.form2.OnUsePausaChange += UpdateUsePausa;
            GUIRuner.overlayForm.Invoke(GUIRuner.overlayForm.changeColor, chekBoxIsCheked);
            GUIRuner.overlayForm.Invoke(GUIRuner.overlayForm.gameWindowActivChange, false);



            string poeLogFilePath = (string)GUIRuner.form2.Invoke(GUIRuner.form2.getPOELogFilePath);
            PoeLogReader.OnZoneWasChanged += OnPauseChange;
            PoeLogReader.InintChek(poeLogFilePath); 
            Pause = PoeLogReader.characterIsInPauseZone;


            UpdateScreenSize();
            if (gameWindowIsValidResalution)
                GUIRuner.overlayForm.Invoke(GUIRuner.overlayForm.poeRectChange, game_rect);
            else
                RiseMsgBoxWithGameClientRECTError(game_rect);


            FlasksData res;

            Stopwatch watch_stop;
            Stopwatch watch_run;
            long elapsedTime;
            int UPS;
            string charHP;
            string charMP;
            gameWindowIsActive = false;
            gameWindowIsActive_old = false;
            while (true)
            {
                if (gameWindowIsValidResalution)
                {
                    BotFW.BotFW.GetScreen(game_rect, screen_bm);
                    screen_bm.ToMat(screen);
                    res = GrabFlasksData.GrabData(screen);

                    gameWindowIsActive = PoeInteraction.GameWindowIsActive();
                    if (ChekGameWindowActivChange())
                    {
                        OnGameWindowActivChange();
                    }

                    watch_stop = Stopwatch.StartNew();
                    while (Run)
                    {
                        watch_run = Stopwatch.StartNew();

                        if (gameWindowIsValidResalution)
                        {
                            BotFW.BotFW.GetScreen(game_rect, screen_bm);
                            screen_bm.ToMat(screen);
                            //screen.Save("screen.png");

                            gameWindowIsActive = PoeInteraction.GameWindowIsActive();
                            if (ChekGameWindowActivChange())
                            {
                                OnGameWindowActivChange();
                            }

                            // TODO
                            // 1. Добавить возможность сохранять и загружать пресеты настроек. 
                            // 2. определение енергощита,
                            // 3. возможность настраивать использование скилов

                            res = GrabFlasksData.GrabData(screen);
                            PoeLogReader.Update();
                            if (usePausa)
                            {
                                Pause = PoeLogReader.characterIsInPauseZone;
                            }
                            else
                            {
                                Pause = false;
                            }
                            
                            if (gameWindowIsActive && !Pause)
                                FlasksUse.UseFlasks(res);

                            watch_run.Stop();

                            elapsedTime = watch_run.ElapsedMilliseconds;
                            UPS = (int)(1000 / elapsedTime);

                            charHP = res.HP_isFinded ? res.CharHP.ToString() : "NA";
                            charMP = res.MP_isFinded ? res.CharMP.ToString() : "NA";

                            GUIRuner.form2.Invoke(GUIRuner.form2.OnUpdateStatusBar, UPS, charHP, charMP);
                        }
                    }

                    Thread.Sleep(100);

                    elapsedTime = watch_stop.ElapsedMilliseconds;
                    if (elapsedTime > 0)
                        UPS = (int)(1000 / elapsedTime);
                    else
                        UPS = 0;
                    charHP = res.HP_isFinded ? res.CharHP.ToString() : "NA";
                    charMP = res.MP_isFinded ? res.CharMP.ToString() : "NA";

                    if (!GUIRuner.form2.FormIsClosing)
                        GUIRuner.form2?.Invoke(GUIRuner.form2.OnUpdateStatusBar, UPS, charHP, charMP);
                }
                else
                    Thread.Sleep(100);
            }
        }

        public static void Form1ThreadStart()
        {
            if (FlaskSetup.Program.form1 == null)
            {
                Form1Thread = new Thread(FlaskSetup.Program.Main);
                Form1Thread.SetApartmentState(ApartmentState.STA);
                Form1Thread.Start();

                while (FlaskSetup.Program.form1 == null)
                {
                    Thread.Sleep(10);
                }
                while (FlaskSetup.Program.form1.Text != "Настройка банок") // Fix This!!!!
                {
                    Thread.Sleep(10);
                }
                FlaskSetup.Program.form1.OnGroupSetupChange += FlasksGroupsManager.GroupSetUpUpdate;
                FlaskSetup.Program.form1.OnGroupSetupChange += FlasksUse.UpdateFlaskGroupes_forUpdate;
                FlaskSetup.Program.form1.OnDataNeedUpdate += FlasksGroupsManager.DataUpdate;

                Form1Thread.Join();
            }
            else
            {
                FlaskSetup.Program.form1.ShowDialog();
            }
        }

        public static void StartStop()
        {
            Console.WriteLine("старт стоп");
            Run = !Run;

            if (debug)
            {
                game_rect = new RECT(1920, 0, 1920, 1050); // 1050
                //screen_rect = new RECT(1920, 0, 1920, 1080); // 1080
            }
            else
            {
                UpdateScreenSize();
                if (!gameWindowIsValidResalution && Run)
                {
                    Run = false;
                    RiseMsgBoxWithGameClientRECTError(game_rect);
                }
                else if (Run)
                {
                    GUIRuner.overlayForm.Invoke(GUIRuner.overlayForm.poeRectChange, game_rect);
                }
                game_rect = PoeInteraction.GetGameRect();
            }

            GUIRuner.form2.Invoke(GUIRuner.form2.OnStartStopChange, Run);
            GUIRuner.overlayForm.Invoke(GUIRuner.overlayForm.textChange, Run);
        }

        public static void OverlayThreadStart()
        {
            OverlayThread = new Thread(GUIRuner.OverlayFormRun);
            OverlayThread.SetApartmentState(ApartmentState.STA);
            OverlayThread.Start();
        }

        public static void Form2ThreadStart() 
        {
            Form2Thread = new Thread(GUIRuner.MainFormRun);
            Form2Thread.SetApartmentState(ApartmentState.STA);
            Form2Thread.Start();
        }

        private static bool ChekGameWindowActivChange()
        {
            return gameWindowIsActive != gameWindowIsActive_old;
        }

        private static void OnGameWindowActivChange()
        {
            GUIRuner.overlayForm.Invoke(GUIRuner.overlayForm.gameWindowActivChange, gameWindowIsActive);
            gameWindowIsActive_old = gameWindowIsActive;
        }

        private static void OnPauseChange()
        {
            GUIRuner.form2.Invoke(GUIRuner.form2.OnPauseChange, PoeLogReader.characterIsInPauseZone);
            GUIRuner.overlayForm.Invoke(GUIRuner.overlayForm.onPauseChange, PoeLogReader.characterIsInPauseZone);
        }

        private static void RiseMsgBoxWithGameClientRECTError(RECT gameRECT)
        {
            string acceptScreenRess = "";
            foreach (var scr_res in PoeInteraction.ACCEPT_SCREEN_RES)
            {
                acceptScreenRess += $"{scr_res};\n";
            }

            MessageBox.Show($"Текущее разрешение клиента игры не потдерживается! ({gameRECT})\n\nУбедитесь что разрешение пое (размер скриншота через Alt+PrtSc):\n{acceptScreenRess}", "ERROR");
        }

        private static void UpdateScreensSize(RECT gameRECT)
        {
            screen_bm = new Bitmap(gameRECT.Width, gameRECT.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            screen = new Mat(gameRECT.Width, gameRECT.Height, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
        }

        private static void UpdateScreenSize()
        {
            game_rect = PoeInteraction.GetGameRect();
            if (PoeInteraction.GameWindowIsValideResolution(game_rect))
            {
                UpdateScreensSize(game_rect);
                gameWindowIsValidResalution = true;
            }
            else
                gameWindowIsValidResalution = false;
        }

        private static void UpdateUsePausa(bool useP)
        {
            usePausa = useP;
        }
    }
}
