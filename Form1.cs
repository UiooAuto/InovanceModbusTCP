using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        InovanceModbusTCPTool plc1;
        InovanceModbusTCPTool plc2;
        Thread readThread1;
        Thread readThread2;
        Thread readThread3;

        Work work;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        #region 多线程测试

        public void G1()
        {
            while (true)
            {
                //Thread.Sleep(10);
                Show1("T1-" + work.GetA());                
                //Thread.Sleep(10);
            }
        }

        public void G2()
        {
            while (true)
            {
                //Thread.Sleep(10);
                Show1("T2-" + work.GetA());
                //Thread.Sleep(10);
            }
        }

        public void G3()
        {
            while (true)
            {
                //Thread.Sleep(10);
                Show1("T3-" + work.GetA());
                //Thread.Sleep(10);
            }
        }

        #endregion

        public void Show1(string str)
        {
            listBox1.Items.Add(DateTime.Now.ToString("HH:mm:ss.fff") + "- " + str);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }
        
        public void Show2(string str)
        {
            listBox2.Items.Add(DateTime.Now.ToString("HH:mm:ss.fff") + "- " + str);
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
        }

        private void connect_Click(object sender, EventArgs e)
        {
            if (connect.Text == "连接")
            {
                plc1 = new InovanceModbusTCPTool(tb_ip.Text, int.Parse(tb_port.Text), byte.Parse(tb_SlaveNum.Text));
                plc2 = new InovanceModbusTCPTool(tb_ip.Text, int.Parse(tb_port.Text), byte.Parse(tb_SlaveNum.Text));
                bool v1 = plc1.Connect();
                //bool v2 = plc2.Connect();
                if (!(v1 /*& v2*/))
                {
                    Show1("连接失败");
                    return;
                }
                connect.Text = "断开";
                Show1("连接");
            }
            else
            {
                plc1.CloseConnect();
                plc2.CloseConnect();
                connect.Text = "连接";
                Show1("断开");
            }
        }

        private void btn_ReadBool_Click(object sender, EventArgs e)
        {
            int num;
            if (int.TryParse(tb_ReadBoolLength.Text, out num))
            {
                ReadResult<bool[]> readResult = plc1.ReadBoolean(tb_ReadBoolAddress.Text, (UInt16)num);
                if (readResult.isSuccess)
                {
                    Show1(JsonConvert.SerializeObject(readResult.result));
                }
                else
                {
                    Show1("读取失败");
                }
            }
            else
            {
                ReadResult<bool> readResult = plc1.ReadBoolean(tb_ReadBoolAddress.Text);
                if (readResult.isSuccess)
                {
                    Show1(readResult.result.ToString());
                }
                else
                {
                    Show1("读取失败");
                }
            }
        }

        private void btn_ReadWord_Click(object sender, EventArgs e)
        {
            int num;
            if (int.TryParse(tb_ReadWordLength.Text, out num))
            {
                if (cb_ThreadReadOpen.Checked)
                {
                    readThread1 = new Thread(ReadThread1);
                    readThread1.IsBackground = true;
                    readThread1.Start();

                    readThread2 = new Thread(ReadThread11);
                    readThread2.IsBackground = true;
                    readThread2.Start();
                    return;
                }

                if (cb_IsNotUWord.Checked)
                {
                    ReadResult<Int16[]> readResult = plc1.Read16(tb_ReadWordAddress.Text, (UInt16)num);

                    if (readResult.isSuccess)
                    {
                        Show1(JsonConvert.SerializeObject(readResult.result));
                    }
                    else
                    {
                        Show1("读取失败");
                    }
                }
                else
                {
                    ReadResult<UInt16[]> readResult = plc1.ReadU16(tb_ReadWordAddress.Text, (UInt16)num);

                    if (readResult.isSuccess)
                    {
                        Show1(JsonConvert.SerializeObject(readResult.result));
                    }
                    else
                    {
                        Show1("读取失败");
                    }
                }                
            }
            else
            {
                if (cb_IsNotUWord.Checked)
                {
                    ReadResult<Int16> readResult = plc1.Read16(tb_ReadWordAddress.Text);
                    if (readResult.isSuccess)
                    {
                        Show1(readResult.result.ToString());
                    }
                    else
                    {
                        Show1("读取失败");
                    }
                }
                else
                {
                    ReadResult<UInt16> readResult = plc1.ReadU16(tb_ReadWordAddress.Text);
                    if (readResult.isSuccess)
                    {
                        Show1(readResult.result.ToString());
                    }
                    else
                    {
                        Show1("读取失败");
                    }
                }
            }
        }

        private void ReadThread1()
        {
            while (true)
            {
                ReadResult<Int16[]> readResult = plc1.Read16(tb_ReadWordAddress.Text, (UInt16)int.Parse(tb_ReadWordLength.Text));
                if (readResult.isSuccess)
                {
                    /*if (null == readResult.result || 0 == readResult.result.Length)
                    {
                        ;
                    }*/
                    string v = JsonConvert.SerializeObject(readResult.result);
                    if (v == "[]")
                    {
                        Thread.Sleep(1);
                    }
                    Show1(v);
                }
                else
                {
                    Show1("读取失败");
                }
            }
        }
        
        private void ReadThread11()
        {
            while (true)
            {
                ReadResult<Int16[]> readResult = plc1.Read16(tb_ReadWordAddress.Text, (UInt16)int.Parse(tb_ReadWordLength.Text));
                if (readResult.isSuccess)
                {
                    /*if (null == readResult.result || 0 == readResult.result.Length)
                    {
                        Thread.Sleep(1);
                    }*/
                    string v = JsonConvert.SerializeObject(readResult.result);
                    if (v == "[]")
                    {
                        Thread.Sleep(1);
                    }
                    Show2(v);
                }
                else
                {
                    Show2("读取失败");
                }
            }
        }
        private void ReadThread2()
        {
            while (true)
            {
                ReadResult<int[]> readResult = plc1.Read32(tb_ReadDWordAddress.Text, (UInt16)int.Parse(tb_ReadDWordLength.Text));
                if (readResult.isSuccess)
                {
                    /*if (null == readResult.result || 0 == readResult.result.Length)
                    {
                        Thread.Sleep(1);
                    }*/
                    string v = JsonConvert.SerializeObject(readResult.result);
                    if (v == "[]")
                    {
                        Thread.Sleep(1);
                    }
                    Show1(v);
                }
                else
                {
                    Show1("读取失败");
                }
            }
        }

        private void btn_WriteBool_Click(object sender, EventArgs e)
        {
            bool v;
            if (tb_WriteBoolValue.Text.Contains('，'))
            {
                MessageBox.Show("请使用英文逗号分隔");
                return;
            }
            if (tb_WriteBoolValue.Text.Contains(','))
            {
                string[] strings = tb_WriteBoolValue.Text.Split(',');
                bool[] bools = new bool[strings.Length];
                for (int i = 0; i < strings.Length; i++)
                {
                    bools[i] = StringToBool(strings[i]);
                }
                v = plc1.Write(tb_WriteBoolAddress.Text, bools);
            }
            else
            {
                v = plc1.Write(tb_WriteBoolAddress.Text, StringToBool(tb_WriteBoolValue.Text));
            }
            if (v)
            {
                Show1("写入成功");
            }
            else
            {
                Show1("写入失败");
            }
        }

        private void btn_WriteWord_Click(object sender, EventArgs e)
        {
            bool v;

            if (tb_WriteBoolValue.Text.Contains('，'))
            {
                MessageBox.Show("请使用英文逗号分隔");
                return;
            }
            if (tb_WriteWordValue.Text.Contains(','))
            {
                string[] strings = tb_WriteWordValue.Text.Split(',');

                if (cb_IsNotUWord.Checked)
                {
                    Int16[] int16s = new Int16[strings.Length];
                    for (int i = 0; i < strings.Length; i++)
                    {
                        int16s[i] = Int16.Parse(strings[i]);
                    }
                    Show1("开始写入");
                    v = plc1.Write(tb_WriteWordAddress.Text, int16s);
                }
                else
                {
                    UInt16[] uint16s = new UInt16[strings.Length];
                    for (int i = 0; i < strings.Length; i++)
                    {
                        uint16s[i] = UInt16.Parse(strings[i]);
                    }
                    Show1("开始写入");
                    v = plc1.Write(tb_WriteWordAddress.Text, uint16s);
                }

            }
            else
            {
                if (cb_IsNotUWord.Checked)
                {
                    Show1("开始写入");
                    v = plc1.Write(tb_WriteWordAddress.Text, Int16.Parse(tb_WriteWordValue.Text));

                }
                else
                {
                    Show1("开始写入");
                    v = plc1.Write(tb_WriteWordAddress.Text, UInt16.Parse(tb_WriteWordValue.Text));
                }
            }
            if (v)
            {
                Show1("写入成功");
            }
            else
            {
                Show1("写入失败");
            }
        }

        public bool StringToBool(string value)
        {
            if (value == "1")
            {
                return true;
            }else if (value == "true")
            {
                return true;
            }
            else if(value == "0")
            {
                return false;
            }else if(value == "false")
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        private void btn_ReadDWord_Click(object sender, EventArgs e)
        {
            int num;
            if (int.TryParse(tb_ReadDWordLength.Text, out num))
            {
                if (cb_ThreadReadOpen.Checked)
                {
                    readThread1 = new Thread(ReadThread2);
                    readThread1.IsBackground = true;
                    readThread1.Start();
                    return;
                }
                if (cb_IsNotUDWord.Checked)
                {
                    ReadResult<int[]> readResult = plc1.Read32(tb_ReadDWordAddress.Text, (UInt16)num);
                    if (readResult.isSuccess)
                    {
                        Show1(JsonConvert.SerializeObject(readResult.result));
                    }
                    else
                    {
                        Show1("读取失败");
                    }
                }
                else
                {
                    ReadResult<uint[]> readResult = plc1.ReadU32(tb_ReadDWordAddress.Text, (UInt16)num);
                    if (readResult.isSuccess)
                    {
                        Show1(JsonConvert.SerializeObject(readResult.result));
                    }
                    else
                    {
                        Show1("读取失败");
                    }
                }
            }
            else
            {
                ReadResult<int> readResult = plc1.Read32(tb_ReadDWordAddress.Text);
                if (readResult.isSuccess)
                {
                    Show1(readResult.result.ToString());
                }
                else
                {
                    Show1("读取失败");
                }
            }
        }

        private void btn_WriteDWord_Click(object sender, EventArgs e)
        {
            bool v;

            if (tb_WriteBoolValue.Text.Contains('，'))
            {
                MessageBox.Show("请使用英文逗号分隔");
                return;
            }

            if (tb_WriteDWordValue.Text.Contains(','))
            {
                string[] strings = tb_WriteDWordValue.Text.Split(',');

                if (cb_IsNotUDWord.Checked)
                {
                    int[] ints = new int[strings.Length];
                    for (int i = 0; i < strings.Length; i++)
                    {
                        ints[i] = int.Parse(strings[i]);
                    }
                    Show1("开始写入");
                    v = plc1.Write(tb_WriteDWordAddress.Text, ints);

                    if (v)
                    {
                        Show1("写入成功");
                    }
                    else
                    {
                        Show1("写入失败");
                    }
                }
                else
                {
                    uint[] uints = new uint[strings.Length];
                    for (int i = 0; i < strings.Length; i++)
                    {
                        uints[i] = uint.Parse(strings[i]);
                    }
                    Show1("开始写入");
                    v = plc1.Write(tb_WriteDWordAddress.Text, uints);

                    if (v)
                    {
                        Show1("写入成功");
                    }
                    else
                    {
                        Show1("写入失败");
                    }
                }
            }
            else
            {
                if (cb_IsNotUDWord.Checked)
                {
                    Show1("开始写入");
                    v = plc1.Write(tb_WriteDWordAddress.Text, int.Parse(tb_WriteDWordValue.Text));

                    if (v)
                    {
                        Show1("写入成功");
                    }
                    else
                    {
                        Show1("写入失败");
                    }
                }
                else
                {
                    Show1("开始写入");
                    v = plc1.Write(tb_WriteDWordAddress.Text, uint.Parse(tb_WriteDWordValue.Text));

                    if (v)
                    {
                        Show1("写入成功");
                    }
                    else
                    {
                        Show1("写入失败");
                    }
                }
            }
        }

        private void btn_StopThreadRead_Click(object sender, EventArgs e)
        {
            if (readThread1 != null)
            {
                readThread1.Abort();
            }

            if (readThread2 != null)
            {
                readThread2.Abort();
            }
        }

        private void btn_CheckConnect_Click(object sender, EventArgs e)
        {
            /*bool v = plc.CheckConnect();
            if (v)
            {
                Show("连接正常");
            }
            else
            {
                Show("连接中断");
            }*/
        }
    }
}
