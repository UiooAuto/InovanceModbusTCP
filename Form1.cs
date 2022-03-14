using Newtonsoft.Json;
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
        InovanceModbusTCPTool plc;
        Thread thread2;
        Thread thread3;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }


        public void Show(string str)
        {
            listBox1.Items.Add(DateTime.Now.ToString("HH:mm:ss.fff") + "-" + str);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void connect_Click(object sender, EventArgs e)
        {
            if (connect.Text == "连接")
            {
                plc = new InovanceModbusTCPTool(tb_ip.Text, int.Parse(tb_port.Text));
                plc.Connect();
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
                ReadResult<bool[]> readResult = plc.ReadBoolean(1, tb_ReadBoolAddress.Text, (UInt16)num);
                if (readResult.success)
                {
                    Show(JsonConvert.SerializeObject(readResult.data));
                }
                else
                {
                    Show("读取失败");
                }
            }
            else
            {
                ReadResult<bool> readResult = plc.ReadBoolean(1, tb_ReadBoolAddress.Text);
                if (readResult.success)
                {
                    Show(readResult.data.ToString());
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
                ReadResult<UInt16[]> readResult = plc.ReadWord(1, tb_ReadWordAddress.Text, (UInt16)num);
                if (readResult.success)
                {
                    Show(JsonConvert.SerializeObject(readResult.data));
                }
                else
                {
                    Show("读取失败");
                }
            }
            else
            {
                ReadResult<UInt16> readResult = plc.ReadWord(1, tb_ReadWordAddress.Text);
                if (readResult.success)
                {
                    Show(readResult.data.ToString());
                }
                else
                {
                    Show("读取失败");
                }
            }
        }

        private void btn_WriteBool_Click(object sender, EventArgs e)
        {
            bool v;
            if (tb_WriteBoolValue.Text.Contains(','))
            {
                string[] strings = tb_WriteBoolValue.Text.Split(',');
                bool[] bools = new bool[strings.Length];
                for (int i = 0; i < strings.Length; i++)
                {
                    bools[i] = StringToBool(strings[i]);
                }
                v = plc.Write(1, tb_WriteBoolAddress.Text, bools);
            }
            else
            {
                v = plc.Write(1, tb_WriteBoolAddress.Text, StringToBool(tb_WriteBoolValue.Text));
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
            if (tb_WriteWordValue.Text.Contains(','))
            {
                string[] strings = tb_WriteWordValue.Text.Split(',');
                UInt16[] uint16s = new UInt16[strings.Length];
                for (int i = 0; i < strings.Length; i++)
                {
                    uint16s[i] = UInt16.Parse(strings[i]);
                }
                v = plc.Write(1, tb_WriteWordAddress.Text, uint16s);
            }
            else
            {
                v = plc.Write(1, tb_WriteWordAddress.Text, UInt16.Parse(tb_WriteWordValue.Text));
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
    }
}
