using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotFW.lib
{
    public class Timer
    {
        public long tm;
        public Timer() 
        {
            this.tm = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        public void Upd()
        {
            tm = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        public bool Chek(int tr_ms, long time = -1)
        {
            if (time == -1)
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            //Console.WriteLine($"time - tm: {time - tm}, tr: {tr_ms}, result: {(time - tm) > tr_ms}");
            return (time - tm) > tr_ms;
        }
    }
}
