using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InovanceModbusTCP
{
    internal class Work
    {
        public int a;

        public string GetA()
        {
            a++;
            //Thread.Sleep(20);
            for (int i = 0; i < 2048; i++)
            {
                for (int j = 0; j < 2048; j++)
                {
                    ;
                }
            }
            return a.ToString();
        }
    }
}
