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
        InovanceModbusTCPTool plc = new InovanceModbusTCPTool("127.0.0.1", 502);
        Thread thread2;
        Thread thread3;
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
            if (button1.Text == "连接")
            {
                plc.Connect();
                button1.Text = "断开";
            }
            else
            {
                if (thread2.IsAlive)
                {
                    thread2.Abort();
                }
                if (thread3.IsAlive)
                {
                    thread3.Abort();
                }
                plc.CloseConnect();
                button1.Text = "连接";
            }
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            show(listBox2, "按下");
            bool v = plc.Write(1, "q101", new bool[] {true, false ,true, true, false, false, false, false, true, true, false, true});
            if (v)
            {
                show(listBox2, "写入成功");
            }
            else
            {
                show(listBox2, "写入失败");
            }
            /*ReadResult<UInt16[]> readResult = plc.ReadWordM(0, 100, 4);
            if (!readResult.success)
            {
                show(listBox1, "失败");
            }
            else
            {
                //MessageBox.Show(Newtonsoft.Json.JsonConvert.SerializeObject(readResult.data));
                show(listBox1, Newtonsoft.Json.JsonConvert.SerializeObject(readResult.data));
            }*/
            /*thread2 = new Thread(go2);
            thread2.IsBackground = true;
            thread2.Start();
            thread3 = new Thread(go3);
            thread3.IsBackground = true;
            thread3.Start();*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            show(listBox2, "按下");
            bool v = plc.Write(1, "m101", new UInt16[] { 0, 1, 258, 55536 });
            if (v)
            {
                show(listBox2, "写入成功");
            }
            else
            {
                show(listBox2, "写入失败");
            }
            /*ReadResult<bool> readResult = plc.ReadBooleanQ(1, 101);
            if (!readResult.success)
            {
                show(listBox2, "失败");
            }
            else
            {
                show(listBox2, readResult.data.ToString());
            }*/
        }

        public void go()
        {
            while (true)
            {
                show(listBox1, plc.ReadBooleanQ(1, 100).data.ToString()); 
            }
        }
        public void go3()
        {
            while (true)
            {
                //show(listBox2, "按下");
                show(listBox3, Newtonsoft.Json.JsonConvert.SerializeObject(plc.ReadWordM(0, 102, 4).data));
                Thread.Sleep(1000);
            }
        }
        public void go2()
        {
            while (true)
            {
                //show(listBox2, "按下");
                show(listBox1, Newtonsoft.Json.JsonConvert.SerializeObject(plc.ReadWordM(1, 100, 4).data));
                Thread.Sleep(1000);
            }
        }

        public void show(ListBox listbox, string str)
        {
            listbox.Items.Add(DateTime.Now.ToString("HH:mm:ss.fff") + "-" + str);
            listbox.SelectedIndex = listbox.Items.Count - 1;
        }

    }
}
