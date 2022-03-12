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

namespace InovanceModbusTCP
{
    public partial class Form1 : Form
    {
        Test t = new Test();
        InovanceModbusTCPTool plc = new InovanceModbusTCPTool("127.0.0.1", 502);
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            /*Thread thread = new Thread(go);
            thread.IsBackground = true;
            thread.Start();*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            plc.Connect();
            /*Thread thread = new Thread(go2);
            thread.IsBackground = true;
            thread.Start();*/
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ReadResult<bool> readResult = plc.ReadBoolean(0, 100);
            if (!readResult.success)
            {
                show(listBox1, "失败");
            }
            else
            {
                show(listBox1, readResult.data.ToString());
            }
        }

        public void go()
        {
            while (true)
            {
                show(listBox1, t.covn()); 
            }
        }
        public void go2()
        {
            /*while (true)
            {*/
            show(listBox2, "按下");
            show(listBox2, t.covn());
            /*}*/
        }

        public void show(ListBox listbox, string str)
        {

            listbox.Items.Add(DateTime.Now.ToString("HH:mm:ss.fff") + "-" + str);
        }
    }
}
