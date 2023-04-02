using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Drinker.GUI
{
    public partial class Form2 : Form
    {
        public delegate void StartStopButtonChager(bool run);
        public StartStopButtonChager OnStartStopChange;
        public delegate void UpdateStatusBarChanger(int UPS, string charHP, string charMP);
        public UpdateStatusBarChanger OnUpdateStatusBar;

        public bool FormIsClosing = false;

        private static readonly string runButtonText_run = "Stop (F4)";
        private static readonly Color runButtonColor_run = Color.YellowGreen;
        private static readonly string runButtonText_stop = "Start (F4)";
        private static readonly Color runButtonColor_stop = Color.IndianRed;

        private static readonly string UPS_statusText_prefix = "UPS: ";
        private static readonly string charHP_statusText_prefix = "HP: ";
        private static readonly string charMP_statusText_prefix = "MP: ";
        public Form2()
        {
            InitializeComponent();

            OnStartStopChange = StartStopChange;
            //SetLenghts();
            OnUpdateStatusBar = UpdateStatusBar;


            Closing += new CancelEventHandler(MainWindow_Closing);

            StartPosition = FormStartPosition.Manual;
            Left = Properties.Settings.Default.WinPos.X;
            Top = Properties.Settings.Default.WinPos.Y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Program.Form1Thread == null || Program.Form1Thread.ThreadState != ThreadState.Running)
            {
                Program.Form1ThreadStart();
                //Program.StartStop();
            }
        }

        private void MainWindow_Closing(object sender, EventArgs e)
        {
            Properties.Settings.Default.WinPos = new Point(Left, Top);
            Properties.Settings.Default.Save();
        }

        private void StartStopChange(bool run)
        {
            if (run)
            {
                StartStop_button.Text = runButtonText_run;
                StartStop_button.BackColor = runButtonColor_run;
            }
            else
            {
                StartStop_button.Text = runButtonText_stop;
                StartStop_button.BackColor = runButtonColor_stop;
            }
        }

        private void UpdateStatusBar(int UPS, string charHP, string charMP)
        {
            string UPS_statusText_temp = UPS_statusText_prefix + UPS.ToString();
            string charHP_statusText_temp = charHP_statusText_prefix + charHP;
            string charMP_statusText_temp = charMP_statusText_prefix + charMP;

            UPS_statusText.Text = UPS_statusText_temp;
            charHP_statusText.Text = charHP_statusText_temp;
            charMP_statusText.Text = charMP_statusText_temp;
        }

        private void StartStop_button_Click(object sender, EventArgs e)
        {
            Program.StartStop();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormIsClosing = true;
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
