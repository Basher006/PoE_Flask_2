using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drinker.GUI
{
    internal static class GUIRuner
    {
        public static Form2 form2 = new Form2();
        public static Overlay overlayForm = new Overlay();
        public static void OverlayFormRun()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //form2 = new Form2();
            Application.Run(form2);
        }

        public static void MainFormRun()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //form2 = new Form2();
            Application.Run(overlayForm);
        }
    }
}
