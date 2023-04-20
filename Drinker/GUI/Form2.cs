using Drinker.BotLogic.GameClientContext;
using Drinker.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using static Drinker.GUI.Form2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Drinker.GUI
{
    public partial class Form2 : Form
    {
        public delegate void StartStopButtonChanger(bool run);
        public StartStopButtonChanger OnStartStopChange;
        public delegate void UpdateStatusBarChanger(int UPS, string charHP, string charMP);
        public UpdateStatusBarChanger OnUpdateStatusBar;
        public delegate void ChangeOverlay(bool checkBoxIsCheked);
        public ChangeOverlay OnChangeOverlay;
        public delegate bool GetChangeOverlay();
        public GetChangeOverlay getChangeOverlay;
        public delegate void PauseStartStopButtonChange(bool pause);
        public PauseStartStopButtonChange OnPauseChange;
        public delegate string FormGetPOELogFilePath();
        public FormGetPOELogFilePath getPOELogFilePath;
        public delegate void UsePausaChange(bool usePausa);
        public UsePausaChange OnUsePausaChange;
        //public delegate bool GetUsePausa();
        //public GetUsePausa OnUsePausaChange;

        public bool FormIsClosing = false;

        private static readonly string runButtonText_run = "Stop (F4)";
        private static readonly Color runButtonColor_run = Color.YellowGreen;
        private static readonly string runButtonText_stop = "Start (F4)";
        private static readonly Color runButtonColor_stop = Color.IndianRed;
        private static readonly string runButtonText_pause = "Pause(in ho) (F4)";
        private static readonly Color runButtonColor_pause = Color.RosyBrown;

        private static readonly string UPS_statusText_prefix = "UPS: ";
        private static readonly string charHP_statusText_prefix = "HP: ";
        private static readonly string charMP_statusText_prefix = "MP: ";

        private static readonly string noLogFolderPathText = "Указать путь к игре..";
        private static string poeLogFilePath;

        private static bool flaskOnPause = false;
        private static bool flaskOnRun = false;
        private static bool usePausa = false;
        public Form2()
        {
            InitializeComponent();

            OnStartStopChange = StartStopChange;
            //SetLenghts();
            OnUpdateStatusBar = UpdateStatusBar;


            Closing += new CancelEventHandler(MainWindow_Closing);

            StartPosition = FormStartPosition.Manual;
            Left = Settings.Default.WinPos.X;
            Top = Settings.Default.WinPos.Y;
            showOverlay_checkBox.Checked = Settings.Default.ShowOverlay;
            autoPayse_chekbox.Checked = Settings.Default.UsePausa;
            getChangeOverlay = get_checkBox1_Checked;
            //OnUsePausaChange = Form2GetUsePausa;
            OnPauseChange = PauseChange;
            getPOELogFilePath = GetPOELogFilePath;


            Settings.Default.LogFilePath = "";
            Settings.Default.Save();
            TryLoadLogFilePath();
            //OnChangeOverlay?.Invoke(showOverlay_checkBox.Checked);

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
            Settings.Default.WinPos = new Point(Left, Top);
            Settings.Default.ShowOverlay = showOverlay_checkBox.Checked;
            Settings.Default.UsePausa = autoPayse_chekbox.Checked;
            Settings.Default.Save();
        }

        private void PauseChange(bool pause)
        {
            flaskOnPause = pause;
            UpdateRunButton();
        }

        private void StartStopChange(bool run)
        {
            flaskOnRun = run;
            UpdateRunButton();
        }

        private void UpdateRunButton()
        {
            if (flaskOnRun)
            {
                if (flaskOnPause && usePausa)
                {
                    StartStop_button.Text = runButtonText_pause;
                    StartStop_button.BackColor = runButtonColor_pause;
                }
                else
                {
                    StartStop_button.Text = runButtonText_run;
                    StartStop_button.BackColor = runButtonColor_run;
                }
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            OnChangeOverlay?.Invoke(showOverlay_checkBox.Checked);
        }

        private bool get_checkBox1_Checked()
        {
            return showOverlay_checkBox.Checked;
        }

        private void POEPath_lable_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = string.IsNullOrEmpty(poeLogFilePath) ? "c:\\" : poeLogFilePath;
                openFileDialog.Filter = "Client.txt (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    poeLogFilePath = openFileDialog.FileName;
                    Console.WriteLine(poeLogFilePath);
                    POEPath_lable.Text = poeLogFilePath;
                    Settings.Default.LogFilePath = poeLogFilePath;
                    Settings.Default.Save();
                }
            }
        }

        private string GetPOELogFilePath()
        {
            return poeLogFilePath;
        }

        //private bool Form2GetUsePausa()
        //{
        //    return usePausa;
        //}

        private bool TryLoadLogFilePath()
        {
            string logFilePath = Settings.Default.LogFilePath;
            if (PoeInteraction.IsValidePOELogFolder(logFilePath))
            {
                poeLogFilePath = logFilePath;
                POEPath_lable.Text = logFilePath;
                return true;
            }
            else
            {
                if (PoeInteraction.TryGetPOELogFolder(out string logFilePath_fromReg))
                {
                    poeLogFilePath = logFilePath_fromReg;
                    POEPath_lable.Text = logFilePath_fromReg;
                    Settings.Default.LogFilePath = logFilePath_fromReg;
                    Settings.Default.Save();
                    return true;
                }
                else
                {
                    POEPath_lable.Text = noLogFolderPathText;
                    return false;
                }  
            }
        }

        private void autoPayse_chekbox_CheckedChanged(object sender, EventArgs e)
        {
            usePausa = autoPayse_chekbox.Checked;
            OnUsePausaChange?.Invoke(usePausa);
        }
    }
}
