using BotFW.lib;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FlaskSetup
{
    public struct FlaskSettings
    {
        public string ActivationType { get; set; }
        public int ActivatePercent { get; set; }
        public float MinCD { get; set; }
        public string Group { get; set; }
        public string HotKey { get; set; }

        public override string ToString()
        {
            return $"Тип: {ActivationType}, Процент: {ActivatePercent}, МинКД: {MinCD}, Группа: {Group}";
        }
    }
    public struct GUIElements_FlaskSetUp
    {
        public int FlaskNumber;
        public ComboBox ActivarionType;
        public NumericUpDown ActivatePercentText;
        public TrackBar ActivatePercentBar;
        public NumericUpDown MinCD;
        public ComboBox HotKey;
        public ComboBox Group;

        public GroupBoxEx FlaskBox;

    }
    public partial class Form1 : Form
    {
        public delegate List<int> GetGroupList();
        public delegate void GroupSetupChange();
        public event GroupSetupChange OnGroupSetupChange;
        public GetGroupList GetGroup1List;
        public GetGroupList GetGroup2List;

        public delegate void DataNeedUpdate();
        public event DataNeedUpdate OnDataNeedUpdate;

        public GUIElements_FlaskSetUp[] guiFlasksElenents;
        public List<int> guiFlasksElenents_group1 = new List<int>();
        public List<int> guiFlasksElenents_group2 = new List<int>();



        private delegate void paintBorderlessGroupBox(object sender, PaintEventArgs p);

        private static readonly string FlaskScreenPath = "FlaskSetupScreen\\FlaskSetupScreen.png";



        private Color[] groupColors = new Color[] { Color.WhiteSmoke, Color.CornflowerBlue, Color.Orange }; //Color.PaleGoldenrod

        public Form1()
        {
            InitializeComponent();
            SetGUIElements_FlaskSetUp();
            SetFlaskGroups(); 
            SetDefloutOnDropBoxes();
            FlasksSetupData LoadedData = SaveLoadData.TryLoadData();
            LoadDataToForm(LoadedData);

            Closing += new CancelEventHandler(MainWindow_Closing);

            GetGroup1List = getGroup1;
            GetGroup2List = getGroup2;

            StartPosition = FormStartPosition.Manual;
            Left = Properties.Settings.Default.WinPos.Left;
            Top = Properties.Settings.Default.WinPos.Top;
            Width = Properties.Settings.Default.WinPos.Width;
            Height = Properties.Settings.Default.WinPos.Height;

            Location = new Point(Properties.Settings.Default.WinPos.Left, Properties.Settings.Default.WinPos.Top);

            LoadFlaskScreen();
        }

        private void SetFlaskGroups()
        {
            guiFlasksElenents_group1.Clear();
            guiFlasksElenents_group2.Clear();
            foreach (var flaskElement in guiFlasksElenents)
            {
                if (flaskElement.Group.Text == FlasksMetrics.GroupDropBoxValues[(int)FlasksMetrics.groupDB.Group1] & !guiFlasksElenents_group1.Contains(flaskElement.FlaskNumber))
                    guiFlasksElenents_group1.Add(flaskElement.FlaskNumber);

                if (flaskElement.Group.Text == FlasksMetrics.GroupDropBoxValues[(int)FlasksMetrics.groupDB.Group2] & !guiFlasksElenents_group2.Contains(flaskElement.FlaskNumber))
                    guiFlasksElenents_group2.Add(flaskElement.FlaskNumber);
            }
            OnGroupSetupChange?.Invoke();
        }

        private void SetGUIElements_FlaskSetUp()
        {
            guiFlasksElenents = new GUIElements_FlaskSetUp[FlasksMetrics.FLASKS_COUNT]
            {
                new GUIElements_FlaskSetUp
                {
                    FlaskNumber = 0,
                    ActivarionType = f1_activate,
                    ActivatePercentText = f1_percent,
                    ActivatePercentBar = f1_percentTrackBar,
                    MinCD = f1_minCD,
                    Group = f1_group,
                    HotKey = f1_Keys,

                    FlaskBox = f1_groupBox
                },
                new GUIElements_FlaskSetUp
                {
                    FlaskNumber = 1,
                    ActivarionType = f2_activate,
                    ActivatePercentText = f2_percent,
                    ActivatePercentBar = f2_percentTrackBar,
                    MinCD = f2_minCD,
                    Group = f2_group,
                    HotKey = f2_Keys,

                    FlaskBox = f2_groupBox
                },
                new GUIElements_FlaskSetUp
                {
                    FlaskNumber = 2,
                    ActivarionType = f3_activate,
                    ActivatePercentText = f3_percent,
                    ActivatePercentBar = f3_percentTrackBar,
                    MinCD = f3_minCD,
                    Group = f3_group,
                    HotKey = f3_Keys,

                    FlaskBox = f3_groupBox
                },
                new GUIElements_FlaskSetUp
                {
                    FlaskNumber = 3,
                    ActivarionType = f4_activate,
                    ActivatePercentText = f4_percent,
                    ActivatePercentBar = f4_percentTrackBar,
                    MinCD = f4_minCD,
                    Group = f4_group,
                    HotKey = f4_Keys,

                    FlaskBox = f4_groupBox
                },
                new GUIElements_FlaskSetUp
                {
                    FlaskNumber = 4,
                    ActivarionType = f5_activate,
                    ActivatePercentText = f5_percent,
                    ActivatePercentBar = f5_percentTrackBar,
                    MinCD = f5_minCD,
                    Group = f5_group,
                    HotKey = f5_Keys,

                    FlaskBox = f5_groupBox
                }
            };
        }

        private void LoadDataToForm(FlasksSetupData data)
        {
            var data_a = data.ToArray();

            for (int i = 0; i < FlasksMetrics.FLASKS_COUNT; i++)
            {
                guiFlasksElenents[i].ActivarionType.Text = data_a[i].ActivationType;
                guiFlasksElenents[i].ActivatePercentText.Text = data_a[i].ActivatePercent.ToString();
                guiFlasksElenents[i].ActivatePercentBar.Value = data_a[i].ActivatePercent;
                guiFlasksElenents[i].MinCD.Text = data_a[i].MinCD.ToString();
                guiFlasksElenents[i].HotKey.Text = data_a[i].HotKey; // !!!
                int index = Array.IndexOf(FlasksMetrics.GroupDropBoxValues, data_a[i].Group);
                guiFlasksElenents[i].Group.SelectedIndex = index;

                ChangeGroupColor(guiFlasksElenents[i].Group);
            }
            
        }

        private void SetDefloutOnDropBoxes()
        {
            foreach (var item in guiFlasksElenents)
            {
                item.ActivarionType.Items.AddRange(FlasksMetrics.ActivatorDropBoxValues);
                item.ActivarionType.SelectedIndex = 0;
                item.HotKey.SelectedIndex = 0;

                item.Group.Items.AddRange(FlasksMetrics.GroupDropBoxValues);
                item.Group.DrawItem += DrawDropBoxItem;
                item.Group.DrawMode = DrawMode.OwnerDrawFixed;

                item.Group.SelectedIndex = 0;

                ChangeGroupColor(item.Group);
            }
        }

        private void DrawDropBoxItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;
            using (Brush br = new SolidBrush(groupColors[e.Index]))
            {
                e.Graphics.FillRectangle(br, e.Bounds);
                e.Graphics.DrawString(FlasksMetrics.GroupDropBoxValues[e.Index], e.Font, Brushes.Black, e.Bounds);
            }
        }

        private void setGroupColor(object sender, EventArgs e)
        {
            var sender_DB = (ComboBox)sender;
            ChangeGroupColor(sender_DB);
        }

        private void ChangeGroupColor(ComboBox sender_DB)
        {
            int group_index;
            foreach (var item in guiFlasksElenents)
            {
                if (item.Group == sender_DB)
                {
                    group_index = item.Group.SelectedIndex;
                    if (group_index >= 0 && group_index <= groupColors.Length && item.ActivarionType.Text != FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.Not])
                    {
                        item.FlaskBox.BorderColor = groupColors[group_index];
                    }
                }
            }
            SetFlaskGroups();
        }

        private void activatePercentTrackBar_ValueChange(object sender, EventArgs e)
        {
            var sender_tb = (TrackBar)sender;
            foreach (var item in guiFlasksElenents)
            {
                if (item.ActivatePercentBar == sender_tb)
                    item.ActivatePercentText.Text = item.ActivatePercentBar.Value.ToString();
            }
        }

        private void activateDropBox_ValueChange(object sender, EventArgs e)
        {
            var sender_DB = (ComboBox)sender;
            foreach (var item in guiFlasksElenents)
            {
                if (item.ActivarionType == sender_DB)
                {
                    SetEnableFlagsOnElements(item);
                }
            }
        }

        private void SetEnableFlagsOnElements(GUIElements_FlaskSetUp flaskElement)
        {
            if (flaskElement.ActivarionType.Text == FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.HP] ||
                flaskElement.ActivarionType.Text == FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.MP] ||
                flaskElement.ActivarionType.Text == FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.ES])
            {
                flaskElement.ActivatePercentText.Enabled = true;
                flaskElement.ActivatePercentBar.Enabled = true;
                flaskElement.MinCD.Enabled = true;
                flaskElement.Group.Enabled = true;

                flaskElement.FlaskBox.BackColor = Color.White;
                int index = Array.IndexOf(FlasksMetrics.GroupDropBoxValues, flaskElement.Group.Text);
                if (index >= 0)
                    flaskElement.FlaskBox.BorderColor = groupColors[index];

            }
            else if (flaskElement.ActivarionType.Text == FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.CD])
            {
                flaskElement.ActivatePercentText.Enabled = false;
                flaskElement.ActivatePercentBar.Enabled = false;
                flaskElement.MinCD.Enabled = true;
                flaskElement.Group.Enabled = true;
                flaskElement.FlaskBox.BackColor = Color.White;
                int index = Array.IndexOf(FlasksMetrics.GroupDropBoxValues, flaskElement.Group.Text);
                if (index >= 0)
                    flaskElement.FlaskBox.BorderColor = groupColors[index];
            }
            else if (flaskElement.ActivarionType.Text == FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.Not])
            {
                flaskElement.ActivatePercentText.Enabled = false;
                flaskElement.ActivatePercentBar.Enabled = false;
                flaskElement.MinCD.Enabled = false;
                flaskElement.Group.Enabled = false;

                flaskElement.FlaskBox.BackColor = Color.Gainsboro;
                flaskElement.FlaskBox.BorderColor = Color.Gainsboro;
            }
            else
            {
                flaskElement.ActivatePercentText.Enabled = false;
                flaskElement.ActivatePercentBar.Enabled = false;
            }
        }

        private void activatePercentText_ValueChange(object sender, EventArgs e)
        {
            var sender_TB = (NumericUpDown)sender;
            foreach (var item in guiFlasksElenents)
            {
                if (item.ActivatePercentText == sender_TB)
                {
                    int validNumber = Validate_activatePercentText_Input(item.ActivatePercentText.Text);
                    item.ActivatePercentText.Text = validNumber.ToString();
                    item.ActivatePercentBar.Value = validNumber;
                }
            }
        }

        private int Validate_activatePercentText_Input(string input)
        {
            int min_val = 0;
            int max_val = 100;
            if (int.TryParse(input, out int input_i))
            {
                if (input_i >= min_val && input_i <= max_val)
                    return input_i;
                else if (input_i >= max_val)
                    return Validate_activatePercentText_Input(input.Substring(0, input.Length - 1));
            }

            return 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            FlaskSettings[] saveData = new FlaskSettings[FlasksMetrics.FLASKS_COUNT];

            for (int i = 0; i < FlasksMetrics.FLASKS_COUNT; i++)
            {
                saveData[i].ActivationType = guiFlasksElenents[i].ActivarionType.Text;
                if (int.TryParse(guiFlasksElenents[i].ActivatePercentText.Text, out int i_result))
                    saveData[i].ActivatePercent = i_result;
                else
                    guiFlasksElenents[i].ActivatePercentText.Text = "0";
                string text = guiFlasksElenents[i].MinCD.Text.Replace(".", ",");
                if (float.TryParse(text, out float f_result))
                    saveData[i].MinCD = f_result;
                else
                    guiFlasksElenents[i].MinCD.Text = "0,5";


                saveData[i].HotKey = guiFlasksElenents[i].HotKey.Text;
                saveData[i].Group = guiFlasksElenents[i].Group.Text;
            }
            SaveLoadData.SaveData(saveData);
            OnDataNeedUpdate?.Invoke();
            OnGroupSetupChange?.Invoke();

            ActiveForm.Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }
        private void MainWindow_Closing(object sender, EventArgs e)
        {
            Properties.Settings.Default.WinPos = new Rectangle(Left, Top, Width, Height);
            Properties.Settings.Default.Save();
        }

        private List<int> getGroup1()
        {
            return guiFlasksElenents_group1;
        }

        private List<int> getGroup2()
        {
            return guiFlasksElenents_group2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OnGroupSetupChange?.Invoke();
        }

        private void FlaskScreenUpdate()
        {
            RECT gameRECT = BotFW.BotFW.GetGameRect("Path of Exile");
            //if (gameRECT.Width == 1920 && gameRECT.Height >= 1050)
            //{
            RECT flaskRECT;
            Bitmap newscreen;
            if (gameRECT.Height == 1050)
            {
                flaskRECT = new RECT(gameRECT.X + 292, 947, 217, 99);
                    newscreen = new Bitmap(flaskRECT.Width, flaskRECT.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }
            else if (gameRECT.Height == 1080)
            {
                flaskRECT = new RECT(gameRECT.X + 306, 973, 230, 105);
                newscreen = new Bitmap(flaskRECT.Width, flaskRECT.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }
            else
            {
                flaskRECT = new RECT(gameRECT.X + 273, gameRECT.Y + 889, 203, 92);
                newscreen = new Bitmap(flaskRECT.Width, flaskRECT.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }

            BotFW.BotFW.GetScreen(flaskRECT, newscreen);
            Mat assdf = newscreen.ToMat();
            flasksScreenIMG.Image.Dispose();
            assdf.Save("FlaskSetupScreen\\FlaskSetupScreen.png");
                LoadFlaskScreen();
            //}
        }

        private void LoadFlaskScreen()
        {
            Bitmap flaskScreen = new Bitmap(FlaskScreenPath);
            this.flasksScreenIMG.Image = flaskScreen;
        }

        private void GetScreen_Button_Click(object sender, EventArgs e)
        {
            FlaskScreenUpdate();
        }
    }
}
