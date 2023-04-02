using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FlaskSetup
{
    public static class Program
    {
        public static Form1 form1;// = new Form1();
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        public static void Main()

            // TODO
            // обновить скрин
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form1 = new Form1();
            //Application.Run(form1);
            form1.ShowDialog();
        }
    }
}
