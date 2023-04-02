﻿using BotFW.lib;
using Emgu.CV;
using System;
using Drinker.DataGrab;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using Drinker.GUI;
using Drinker.BotLogic;
using System.Windows.Input;
using System.Linq;
using Drinker.BotLogic.GameClientContext;

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
        public static readonly int[] ACCEPT_SCREEN_HEIGHT = new int[] { 1050, 1080 };
        public static readonly int[] ACCEPT_SCREEN_WIDTH = new int[] { 1920 };


        public static Thread Form1Thread;
        public static Thread Form2Thread;
        public static bool Run = false;


        private static RECT screen_rect = new RECT();
        private static Bitmap screen_bm;
        private static Mat screen;

        public static bool debug = false;
        public static bool prt = true;

        [STAThread]
        static void Main(string[] args)
        {


            BotFW.BotFW.AddHook(Key.F4, StartStop, true);
            Form2ThreadStart();

            while (screen_rect.Width <= 0 && screen_rect.Height <= 0)
            {
                if (debug)
                {
                    screen_rect = new RECT(1920, 0, 1920, 1050); // 1050
                                                                 //screen_rect = new RECT(1920, 0, 1920, 1080); // 1080
                }
                else
                {
                    screen_rect = PoeInteraction.GetGameRect();
                }
                Thread.Sleep(100);
            }

            
            screen_bm = new Bitmap(screen_rect.Width, screen_rect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            screen = new Mat(screen_rect.Width, screen_rect.Height, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            ChekScreenRectAndUpdate(screen_rect);

            FlasksData res;

            Stopwatch watch_stop;
            Stopwatch watch_run;
            long elapsedTime;
            int UPS;
            string charHP;
            string charMP;
            while (true)
            {
                ChekScreenRectAndUpdate(screen_rect);
                BotFW.BotFW.GetScreen(screen_rect, screen_bm);
                screen_bm.ToMat(screen);

                res = GrabFlasksData.GrabData(screen);

                watch_stop = Stopwatch.StartNew();
                while (Run)
                {
                    watch_run = Stopwatch.StartNew();

                    ChekScreenRectAndUpdate(screen_rect);
                    BotFW.BotFW.GetScreen(screen_rect, screen_bm);
                    screen_bm.ToMat(screen);
                    //screen.Save("screen.png");


                    // TODO
                    // 1. add group update
                    // 2. swith console write line to send for flask send method
                    // 3. poe client integration
                    // 4. perfactoring


                    res = GrabFlasksData.GrabData(screen);
                    if (PoeInteraction.GameWindowIsActive())
                        FlasksUse.UseFlasks(res);


                    watch_run.Stop();

                    elapsedTime = watch_run.ElapsedMilliseconds;
                    UPS = (int)(1000 / elapsedTime);
                    //Console.WriteLine($"UPS: {UPS}, Ms: {ElapsedTime}");

                    charHP = res.HP_isFinded ? res.CharHP.ToString() : "NA";
                    charMP = res.MP_isFinded ? res.CharMP.ToString() : "NA";

                    GuiStart.form2.Invoke(GuiStart.form2.OnUpdateStatusBar, UPS, charHP, charMP);
                }

                Thread.Sleep(100);

                elapsedTime = watch_stop.ElapsedMilliseconds;
                if (elapsedTime > 0)
                    UPS = (int)(1000 / elapsedTime);
                else
                    UPS = 0;
                charHP = res.HP_isFinded ? res.CharHP.ToString() : "NA";
                charMP = res.MP_isFinded ? res.CharMP.ToString() : "NA";

                if (!GuiStart.form2.FormIsClosing)
                    GuiStart.form2?.Invoke(GuiStart.form2.OnUpdateStatusBar, UPS, charHP, charMP); 
            }
        }

        public static void Form1ThreadStart()
        {
            if (FlaskSetup.Program.form1 == null)
            {
                Form1Thread = new Thread(FlaskSetup.Program.Main);
                Form1Thread.SetApartmentState(ApartmentState.STA);
                Form1Thread.Start();
                //Thread.Sleep(2000);
                while (FlaskSetup.Program.form1 == null)
                {
                    Thread.Sleep(10);
                }
                while (FlaskSetup.Program.form1.Text != "Настройка банок")
                {
                    Thread.Sleep(10);
                }
                //Console.WriteLine(Poe_Flask_2.Program.form1);
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
            if (screen_rect.Width <= 0 && screen_rect.Height <= 0)
                return;


            Console.WriteLine("старт стоп");
            Run = !Run;
            GuiStart.form2.Invoke(GuiStart.form2.OnStartStopChange, Run);

            if (debug)
            {
                screen_rect = new RECT(1920, 0, 1920, 1050); // 1050
                //screen_rect = new RECT(1920, 0, 1920, 1080); // 1080
            }
            else
            {
                screen_rect = PoeInteraction.GetGameRect();
            }
            if (screen_rect.Width <= 0 && screen_rect.Height <= 0)
            {
                Run = false;
                GuiStart.form2.Invoke(GuiStart.form2.OnStartStopChange, Run);
                return;
            }
                


            screen_bm = new Bitmap(screen_rect.Width, screen_rect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            screen = new Mat(screen_rect.Width, screen_rect.Height, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            ChekScreenRectAndUpdate(screen_rect);
        }

        public static void Form2ThreadStart() 
        {
            Form2Thread = new Thread(GuiStart.Run);
            Form2Thread.SetApartmentState(ApartmentState.STA);
            Form2Thread.Start();
        }

        private static void ChekScreenRectAndUpdate(RECT scr_rect)
        {
            if ((ACCEPT_SCREEN_HEIGHT.Contains(scr_rect.Height) || ACCEPT_SCREEN_WIDTH.Contains(scr_rect.Width)) &&
                (screen_bm.Width == scr_rect.Width || screen_bm.Height == scr_rect.Height) &&
                (screen.Width == scr_rect.Width || screen.Height == scr_rect.Height))
            {
                return;
            }
            else if ((ACCEPT_SCREEN_HEIGHT.Contains(scr_rect.Height) && ACCEPT_SCREEN_WIDTH.Contains(scr_rect.Width)) &&
                (screen_bm.Width != scr_rect.Width || screen_bm.Height != scr_rect.Height))
            {
                screen_bm = new Bitmap(scr_rect.Width, scr_rect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }
            else if ((ACCEPT_SCREEN_HEIGHT.Contains(scr_rect.Height) && ACCEPT_SCREEN_WIDTH.Contains(scr_rect.Width)) &&
                (screen.Width != scr_rect.Width || screen.Height != scr_rect.Height))
            {
                screen = new Mat(scr_rect.Width, scr_rect.Height, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            }
            else
            {
                while (!ACCEPT_SCREEN_HEIGHT.Contains(scr_rect.Height) || !ACCEPT_SCREEN_WIDTH.Contains(scr_rect.Width))
                {
                    scr_rect = PoeInteraction.GetGameRect();
                    Thread.Sleep(200);
                }
                screen_rect = scr_rect;
                ChekScreenRectAndUpdate(screen_rect);
            }

            

        }
    }
}