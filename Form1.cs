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
        InovanceModbusTCPTool plc;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }


        public void Show(string str)
        {
            listBox1.Items.Add(DateTime.Now.ToString("HH:mm:ss.fff") + "- " + str);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void connect_Click(object sender, EventArgs e)
        {
            if (connect.Text == "连接")
            {
                plc = new InovanceModbusTCPTool(tb_ip.Text, int.Parse(tb_port.Text), byte.Parse(tb_SlaveNum.Text));
                bool v = plc.Connect();
                if (!v)
                {
                    Show("连接失败");
                    return;
                }
                connect.Text = "断开";
                Show("连接");
            }
            else
            {
                plc.CloseConnect();
                connect.Text = "连接";
                Show("断开");
            }
        }

        private void btn_ReadBool_Click(object sender, EventArgs e)
        {
            int num;
            if (int.TryParse(tb_ReadBoolLength.Text, out num))
            {
                ReadResult<bool[]> readResult = plc.ReadBoolean(tb_ReadBoolAddress.Text, (UInt16)num);
                if (readResult.isSuccess)
                {
                    Show(JsonConvert.SerializeObject(readResult.result));
                }
                else
                {
                    Show("读取失败");
                }
            }
            else
            {
                ReadResult<bool> readResult = plc.ReadBoolean(tb_ReadBoolAddress.Text);
                if (readResult.isSuccess)
                {
                    Show(readResult.result.ToString());
                }
                else
                {
                    Show("读取失败");
                }
            }
        }

        private void btn_ReadWord_Click(object sender, EventArgs e)
        {
            int num;
            if (int.TryParse(tb_ReadWordLength.Text, out num))
            {

                if (cb_IsNotUWord.Checked)
                {
                    ReadResult<Int16[]> readResult = plc.Read16(tb_ReadWordAddress.Text, (UInt16)num);

                    if (readResult.isSuccess)
                    {
                        Show(JsonConvert.SerializeObject(readResult.result));
                    }
                    else
                    {
                        Show("读取失败");
                    }
                }
                else
                {
                    ReadResult<UInt16[]> readResult = plc.ReadU16(tb_ReadWordAddress.Text, (UInt16)num);

                    if (readResult.isSuccess)
                    {
                        Show(JsonConvert.SerializeObject(readResult.result));
                    }
                    else
                    {
                        Show("读取失败");
                    }
                }                
            }
            else
            {
                if (cb_IsNotUWord.Checked)
                {
                    ReadResult<Int16> readResult = plc.Read16(tb_ReadWordAddress.Text);
                    if (readResult.isSuccess)
                    {
                        Show(readResult.result.ToString());
                    }
                    else
                    {
                        Show("读取失败");
                    }
                }
                else
                {
                    ReadResult<UInt16> readResult = plc.ReadU16(tb_ReadWordAddress.Text);
                    if (readResult.isSuccess)
                    {
                        Show(readResult.result.ToString());
                    }
                    else
                    {
                        Show("读取失败");
                    }
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
                v = plc.Write(tb_WriteBoolAddress.Text, bools);
            }
            else
            {
                v = plc.Write(tb_WriteBoolAddress.Text, StringToBool(tb_WriteBoolValue.Text));
            }
            if (v)
            {
                Show("写入成功");
            }
            else
            {
                Show("写入失败");
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
                    v = plc.Write(tb_WriteWordAddress.Text, int16s);
                }
                else
                {
                    UInt16[] uint16s = new UInt16[strings.Length];
                    for (int i = 0; i < strings.Length; i++)
                    {
                        uint16s[i] = UInt16.Parse(strings[i]);
                    }
                    v = plc.Write(tb_WriteWordAddress.Text, uint16s);
                }

            }
            else
            {
                if (cb_IsNotUWord.Checked)
                {
                    v = plc.Write(tb_WriteWordAddress.Text, Int16.Parse(tb_WriteWordValue.Text));

                }
                else
                {
                    v = plc.Write(tb_WriteWordAddress.Text, UInt16.Parse(tb_WriteWordValue.Text));
                }
            }
            if (v)
            {
                Show("写入成功");
            }
            else
            {
                Show("写入失败");
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
                if (cb_IsNotUDWord.Checked)
                {
                    ReadResult<int[]> readResult = plc.Read32(tb_ReadDWordAddress.Text, (UInt16)num);
                    if (readResult.isSuccess)
                    {
                        Show(JsonConvert.SerializeObject(readResult.result));
                    }
                    else
                    {
                        Show("读取失败");
                    }
                }
                else
                {
                    ReadResult<uint[]> readResult = plc.ReadU32(tb_ReadDWordAddress.Text, (UInt16)num);
                    if (readResult.isSuccess)
                    {
                        Show(JsonConvert.SerializeObject(readResult.result));
                    }
                    else
                    {
                        Show("读取失败");
                    }
                }
            }
            else
            {
                ReadResult<int> readResult = plc.Read32(tb_ReadDWordAddress.Text);
                if (readResult.isSuccess)
                {
                    Show(readResult.result.ToString());
                }
                else
                {
                    Show("读取失败");
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
                    v = plc.Write(tb_WriteDWordAddress.Text, ints);

                    if (v)
                    {
                        Show("写入成功");
                    }
                    else
                    {
                        Show("写入失败");
                    }
                }
                else
                {
                    uint[] uints = new uint[strings.Length];
                    for (int i = 0; i < strings.Length; i++)
                    {
                        uints[i] = uint.Parse(strings[i]);
                    }
                    v = plc.Write(tb_WriteDWordAddress.Text, uints);

                    if (v)
                    {
                        Show("写入成功");
                    }
                    else
                    {
                        Show("写入失败");
                    }
                }
            }
            else
            {
                if (cb_IsNotUDWord.Checked)
                {
                    v = plc.Write(tb_WriteDWordAddress.Text, int.Parse(tb_WriteDWordValue.Text));

                    if (v)
                    {
                        Show("写入成功");
                    }
                    else
                    {
                        Show("写入失败");
                    }
                }
                else
                {
                    v = plc.Write(tb_WriteDWordAddress.Text, uint.Parse(tb_WriteDWordValue.Text));

                    if (v)
                    {
                        Show("写入成功");
                    }
                    else
                    {
                        Show("写入失败");
                    }
                }
            }
        }
    }
}
