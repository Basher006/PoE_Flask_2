using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BotFW
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BotFW.AddHook(Key.F4, StartStop, true);
        }
        public static void StartStop()
        {
            Console.WriteLine("123");
        }
    }
}
