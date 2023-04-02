using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drinker.GUI
{
    internal static class GuiStart
    {
        public static Form2 form2 = new Form2();
        public static void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //form2 = new Form2();
            Application.Run(form2);
        }
    }
}
