using BotFW.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Drinker.GUI.Overlay;
using static System.Net.Mime.MediaTypeNames;

namespace Drinker.GUI
{

    public struct OverlayContext
    {
        public string Text_on;
        public string Text_off;
        public string Text_pause;
        public Brush Brush_on;
        public Brush Brush_off;
        public Brush Brush_paused;
        public bool OverlayOn;
        public bool GameWindowIsActive;
        public bool CharacterInPauseZone;
        public bool BotIsRun;

        public int PoeGameRECT_H;

    }

    public partial class Overlay : Form
    {
        public delegate void ChangeColor(bool chekbixState);
        public ChangeColor changeColor;
        public delegate void TextChange(bool run);
        public TextChange textChange;
        public delegate void PoeRECTChange(RECT poeWindowRECT);
        public PoeRECTChange poeRectChange;
        public delegate void GameWindowActivChange(bool gameWindowIsActive);
        public GameWindowActivChange gameWindowActivChange;
        public delegate void PauseChange(bool pause);
        public PauseChange onPauseChange;

        public OverlayContext OverlayRePaintData;


        private Point TextPositionFor_1050 = new Point(10, 1010);
        private Point TextPositionFor_1080 = new Point(10, 1040);
        private static readonly int DefloatPoeGameRECT_H = 1050;
        private static readonly int[] AcceptPoeGameRECTs = { 1050, 1080 };

        private static bool usePausa = false;

        public Overlay()
        {
            Thread.Sleep(1000);
            InitializeComponent();
            GUIRuner.form2.OnChangeOverlay += OnOverlayChange;
            changeColor = OnOverlayChange;
            textChange = label1_Paint_TextChange;
            poeRectChange = OnPoeRECTChange;
            gameWindowActivChange = OnGameClientActiveChange;
            onPauseChange = OnCharacterZoneChange;
            GUIRuner.form2.OnUsePausaChange += UpdateUsePausa;

            StartPosition = FormStartPosition.Manual;
            Left = 0;
            //Top = 0;

            OverlayRePaintData = new OverlayContext
            {
                OverlayOn = true,
                GameWindowIsActive = false,
                CharacterInPauseZone = false,
                Brush_on = Brushes.GreenYellow,
                Brush_off = Brushes.Red,
                Brush_paused = Brushes.Gray,
                Text_on = "On",
                Text_off = "Off",
                Text_pause = "Paused",

                PoeGameRECT_H = 1050
            };

            //OnOverlayChange(GUIRuner.form2.getChangeOverlay());
            //swith = false;
        }

        private void OnOverlayChange(bool checkBoxIsCheked)
        {
            OverlayRePaintData.OverlayOn = checkBoxIsCheked;
            RePaint();
        }

        private void OnGameClientActiveChange(bool gameWindowIsActive)
        {
            OverlayRePaintData.GameWindowIsActive = gameWindowIsActive;
            RePaint();
        }

        private void OnCharacterZoneChange(bool characterInPauseZone)
        {
            OverlayRePaintData.CharacterInPauseZone = characterInPauseZone;
            RePaint();
        }

        private void OnPoeRECTChange(RECT poeWindowRECT)
        {
            if (AcceptPoeGameRECTs.Contains(poeWindowRECT.Height))
            {
                OverlayRePaintData.PoeGameRECT_H = poeWindowRECT.Height;
                //TextPositionFor_1050.X = poeWindowRECT.X + 10;
                //TextPositionFor_1080.X = poeWindowRECT.X + 10;
            }
            else
            {
                OverlayRePaintData.PoeGameRECT_H = DefloatPoeGameRECT_H;
                //TextPositionFor_1050.X = 10;
                //TextPositionFor_1080.X = 10;
            }
            Left = poeWindowRECT.X;
            RePaint();
        }

        private void label1_Paint_TextChange(bool run)
        {
            OverlayRePaintData.BotIsRun = run;
            RePaint();
        }

        private void RePaint()
        {
            var g = label1.CreateGraphics();
            g.Clear(Color.White);

            if (!OverlayRePaintData.OverlayOn || !OverlayRePaintData.GameWindowIsActive)
            {
                return;
            }
            else
            {
                string text;
                Brush brush;

                if (!OverlayRePaintData.BotIsRun)
                {
                    if (OverlayRePaintData.CharacterInPauseZone && usePausa)
                    {
                        text = OverlayRePaintData.Text_pause;
                        brush = OverlayRePaintData.Brush_paused;
                    }
                    else
                    {
                        text = OverlayRePaintData.Text_off;
                        brush = OverlayRePaintData.Brush_off;
                    }
                }
                else
                {
                    text = OverlayRePaintData.Text_on;
                    brush = OverlayRePaintData.Brush_on;
                }
                
                Point drawPoint = OverlayRePaintData.PoeGameRECT_H == 1050 ? TextPositionFor_1050 : TextPositionFor_1080;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
                g.DrawString(text, new Font("Arial", 12), brush, drawPoint);
            }
        }

        private void UpdateUsePausa(bool useP)
        {
            usePausa = useP;
        }
    }
}
