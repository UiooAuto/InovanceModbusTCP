using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InovanceModbusTCP
{
    public class Test
    {
        public static int num = 100;
        public string covn()
        {
            Thread thread = new Thread(rec);
            thread.IsBackground = true;
            send();
            thread.Start();
            thread.Join();
            return "over";
        }
        public bool send()
        {
            Thread.Sleep(2000);
            return true;
        }
        public void rec()
        {
            Thread.Sleep(3000);
        }
    }
}
