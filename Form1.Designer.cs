namespace InovanceModbusTCP
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.connect = new System.Windows.Forms.Button();
            this.btn_ReadBool = new System.Windows.Forms.Button();
            this.tb_ip = new System.Windows.Forms.TextBox();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.tb_ReadBoolAddress = new System.Windows.Forms.TextBox();
            this.tb_ReadBoolLength = new System.Windows.Forms.TextBox();
            this.tb_ReadWordAddress = new System.Windows.Forms.TextBox();
            this.tb_ReadWordLength = new System.Windows.Forms.TextBox();
            this.tb_WriteBoolAddress = new System.Windows.Forms.TextBox();
            this.tb_WriteBoolValue = new System.Windows.Forms.TextBox();
            this.tb_WriteWordAddress = new System.Windows.Forms.TextBox();
            this.tb_WriteWordValue = new System.Windows.Forms.TextBox();
            this.btn_ReadWord = new System.Windows.Forms.Button();
            this.btn_WriteBool = new System.Windows.Forms.Button();
            this.btn_WriteWord = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn_WriteDWord = new System.Windows.Forms.Button();
            this.btn_ReadDWord = new System.Windows.Forms.Button();
            this.tb_WriteDWordValue = new System.Windows.Forms.TextBox();
            this.tb_WriteDWordAddress = new System.Windows.Forms.TextBox();
            this.tb_ReadDWordLength = new System.Windows.Forms.TextBox();
            this.tb_ReadDWordAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_SlaveNum = new System.Windows.Forms.TextBox();
            this.cb_IsNotUWord = new System.Windows.Forms.CheckBox();
            this.cb_IsNotUDWord = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(553, 39);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(93, 23);
            this.connect.TabIndex = 1;
            this.connect.Text = "连接";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // btn_ReadBool
            // 
            this.btn_ReadBool.Location = new System.Drawing.Point(553, 106);
            this.btn_ReadBool.Name = "btn_ReadBool";
            this.btn_ReadBool.Size = new System.Drawing.Size(93, 23);
            this.btn_ReadBool.TabIndex = 3;
            this.btn_ReadBool.Text = "读取bool";
            this.btn_ReadBool.UseVisualStyleBackColor = true;
            this.btn_ReadBool.Click += new System.EventHandler(this.btn_ReadBool_Click);
            // 
            // tb_ip
            // 
            this.tb_ip.Location = new System.Drawing.Point(30, 41);
            this.tb_ip.Name = "tb_ip";
            this.tb_ip.Size = new System.Drawing.Size(100, 21);
            this.tb_ip.TabIndex = 5;
            this.tb_ip.Text = "127.0.0.1";
            // 
            // tb_port
            // 
            this.tb_port.Location = new System.Drawing.Point(153, 41);
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(100, 21);
            this.tb_port.TabIndex = 6;
            this.tb_port.Text = "502";
            // 
            // tb_ReadBoolAddress
            // 
            this.tb_ReadBoolAddress.Location = new System.Drawing.Point(30, 106);
            this.tb_ReadBoolAddress.Name = "tb_ReadBoolAddress";
            this.tb_ReadBoolAddress.Size = new System.Drawing.Size(100, 21);
            this.tb_ReadBoolAddress.TabIndex = 7;
            this.tb_ReadBoolAddress.Text = "q100";
            // 
            // tb_ReadBoolLength
            // 
            this.tb_ReadBoolLength.Location = new System.Drawing.Point(153, 106);
            this.tb_ReadBoolLength.Name = "tb_ReadBoolLength";
            this.tb_ReadBoolLength.Size = new System.Drawing.Size(100, 21);
            this.tb_ReadBoolLength.TabIndex = 8;
            // 
            // tb_ReadWordAddress
            // 
            this.tb_ReadWordAddress.Location = new System.Drawing.Point(30, 196);
            this.tb_ReadWordAddress.Name = "tb_ReadWordAddress";
            this.tb_ReadWordAddress.Size = new System.Drawing.Size(100, 21);
            this.tb_ReadWordAddress.TabIndex = 9;
            this.tb_ReadWordAddress.Text = "m100";
            // 
            // tb_ReadWordLength
            // 
            this.tb_ReadWordLength.Location = new System.Drawing.Point(153, 196);
            this.tb_ReadWordLength.Name = "tb_ReadWordLength";
            this.tb_ReadWordLength.Size = new System.Drawing.Size(100, 21);
            this.tb_ReadWordLength.TabIndex = 10;
            // 
            // tb_WriteBoolAddress
            // 
            this.tb_WriteBoolAddress.Location = new System.Drawing.Point(30, 144);
            this.tb_WriteBoolAddress.Name = "tb_WriteBoolAddress";
            this.tb_WriteBoolAddress.Size = new System.Drawing.Size(100, 21);
            this.tb_WriteBoolAddress.TabIndex = 11;
            this.tb_WriteBoolAddress.Text = "q100";
            // 
            // tb_WriteBoolValue
            // 
            this.tb_WriteBoolValue.Location = new System.Drawing.Point(153, 144);
            this.tb_WriteBoolValue.Name = "tb_WriteBoolValue";
            this.tb_WriteBoolValue.Size = new System.Drawing.Size(312, 21);
            this.tb_WriteBoolValue.TabIndex = 12;
            // 
            // tb_WriteWordAddress
            // 
            this.tb_WriteWordAddress.Location = new System.Drawing.Point(30, 234);
            this.tb_WriteWordAddress.Name = "tb_WriteWordAddress";
            this.tb_WriteWordAddress.Size = new System.Drawing.Size(100, 21);
            this.tb_WriteWordAddress.TabIndex = 14;
            this.tb_WriteWordAddress.Text = "m100";
            // 
            // tb_WriteWordValue
            // 
            this.tb_WriteWordValue.Location = new System.Drawing.Point(153, 234);
            this.tb_WriteWordValue.Name = "tb_WriteWordValue";
            this.tb_WriteWordValue.Size = new System.Drawing.Size(312, 21);
            this.tb_WriteWordValue.TabIndex = 15;
            // 
            // btn_ReadWord
            // 
            this.btn_ReadWord.Location = new System.Drawing.Point(553, 196);
            this.btn_ReadWord.Name = "btn_ReadWord";
            this.btn_ReadWord.Size = new System.Drawing.Size(93, 23);
            this.btn_ReadWord.TabIndex = 17;
            this.btn_ReadWord.Text = "读取Word(16)";
            this.btn_ReadWord.UseVisualStyleBackColor = true;
            this.btn_ReadWord.Click += new System.EventHandler(this.btn_ReadWord_Click);
            // 
            // btn_WriteBool
            // 
            this.btn_WriteBool.Location = new System.Drawing.Point(553, 144);
            this.btn_WriteBool.Name = "btn_WriteBool";
            this.btn_WriteBool.Size = new System.Drawing.Size(93, 23);
            this.btn_WriteBool.TabIndex = 18;
            this.btn_WriteBool.Text = "写入bool";
            this.btn_WriteBool.UseVisualStyleBackColor = true;
            this.btn_WriteBool.Click += new System.EventHandler(this.btn_WriteBool_Click);
            // 
            // btn_WriteWord
            // 
            this.btn_WriteWord.Location = new System.Drawing.Point(553, 234);
            this.btn_WriteWord.Name = "btn_WriteWord";
            this.btn_WriteWord.Size = new System.Drawing.Size(93, 23);
            this.btn_WriteWord.TabIndex = 19;
            this.btn_WriteWord.Text = "写入Word(16)";
            this.btn_WriteWord.UseVisualStyleBackColor = true;
            this.btn_WriteWord.Click += new System.EventHandler(this.btn_WriteWord_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(30, 350);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(616, 88);
            this.listBox1.TabIndex = 20;
            // 
            // btn_WriteDWord
            // 
            this.btn_WriteDWord.Location = new System.Drawing.Point(553, 318);
            this.btn_WriteDWord.Name = "btn_WriteDWord";
            this.btn_WriteDWord.Size = new System.Drawing.Size(93, 23);
            this.btn_WriteDWord.TabIndex = 26;
            this.btn_WriteDWord.Text = "写入DWord(32)";
            this.btn_WriteDWord.UseVisualStyleBackColor = true;
            this.btn_WriteDWord.Click += new System.EventHandler(this.btn_WriteDWord_Click);
            // 
            // btn_ReadDWord
            // 
            this.btn_ReadDWord.Location = new System.Drawing.Point(553, 280);
            this.btn_ReadDWord.Name = "btn_ReadDWord";
            this.btn_ReadDWord.Size = new System.Drawing.Size(93, 23);
            this.btn_ReadDWord.TabIndex = 25;
            this.btn_ReadDWord.Text = "读取DWord(32)";
            this.btn_ReadDWord.UseVisualStyleBackColor = true;
            this.btn_ReadDWord.Click += new System.EventHandler(this.btn_ReadDWord_Click);
            // 
            // tb_WriteDWordValue
            // 
            this.tb_WriteDWordValue.Location = new System.Drawing.Point(153, 318);
            this.tb_WriteDWordValue.Name = "tb_WriteDWordValue";
            this.tb_WriteDWordValue.Size = new System.Drawing.Size(312, 21);
            this.tb_WriteDWordValue.TabIndex = 24;
            // 
            // tb_WriteDWordAddress
            // 
            this.tb_WriteDWordAddress.Location = new System.Drawing.Point(30, 318);
            this.tb_WriteDWordAddress.Name = "tb_WriteDWordAddress";
            this.tb_WriteDWordAddress.Size = new System.Drawing.Size(100, 21);
            this.tb_WriteDWordAddress.TabIndex = 23;
            this.tb_WriteDWordAddress.Text = "m100";
            // 
            // tb_ReadDWordLength
            // 
            this.tb_ReadDWordLength.Location = new System.Drawing.Point(153, 280);
            this.tb_ReadDWordLength.Name = "tb_ReadDWordLength";
            this.tb_ReadDWordLength.Size = new System.Drawing.Size(100, 21);
            this.tb_ReadDWordLength.TabIndex = 22;
            // 
            // tb_ReadDWordAddress
            // 
            this.tb_ReadDWordAddress.Location = new System.Drawing.Point(30, 280);
            this.tb_ReadDWordAddress.Name = "tb_ReadDWordAddress";
            this.tb_ReadDWordAddress.Size = new System.Drawing.Size(100, 21);
            this.tb_ReadDWordAddress.TabIndex = 21;
            this.tb_ReadDWordAddress.Text = "m100";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(30, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(618, 1);
            this.label1.TabIndex = 27;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(30, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(618, 1);
            this.label2.TabIndex = 28;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(30, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(618, 1);
            this.label3.TabIndex = 29;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 30;
            this.label4.Text = "IP";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(151, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 31;
            this.label5.Text = "端口";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(275, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 33;
            this.label6.Text = "从站号";
            // 
            // tb_SlaveNum
            // 
            this.tb_SlaveNum.Location = new System.Drawing.Point(277, 41);
            this.tb_SlaveNum.Name = "tb_SlaveNum";
            this.tb_SlaveNum.Size = new System.Drawing.Size(100, 21);
            this.tb_SlaveNum.TabIndex = 32;
            this.tb_SlaveNum.Text = "1";
            // 
            // cb_IsNotUWord
            // 
            this.cb_IsNotUWord.AutoSize = true;
            this.cb_IsNotUWord.Location = new System.Drawing.Point(277, 199);
            this.cb_IsNotUWord.Name = "cb_IsNotUWord";
            this.cb_IsNotUWord.Size = new System.Drawing.Size(60, 16);
            this.cb_IsNotUWord.TabIndex = 34;
            this.cb_IsNotUWord.Text = "有符号";
            this.cb_IsNotUWord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_IsNotUWord.UseVisualStyleBackColor = true;
            // 
            // cb_IsNotUDWord
            // 
            this.cb_IsNotUDWord.AutoSize = true;
            this.cb_IsNotUDWord.Location = new System.Drawing.Point(277, 283);
            this.cb_IsNotUDWord.Name = "cb_IsNotUDWord";
            this.cb_IsNotUDWord.Size = new System.Drawing.Size(60, 16);
            this.cb_IsNotUDWord.TabIndex = 35;
            this.cb_IsNotUDWord.Text = "有符号";
            this.cb_IsNotUDWord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_IsNotUDWord.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 450);
            this.Controls.Add(this.cb_IsNotUDWord);
            this.Controls.Add(this.cb_IsNotUWord);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_SlaveNum);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_WriteDWord);
            this.Controls.Add(this.btn_ReadDWord);
            this.Controls.Add(this.tb_WriteDWordValue);
            this.Controls.Add(this.tb_WriteDWordAddress);
            this.Controls.Add(this.tb_ReadDWordLength);
            this.Controls.Add(this.tb_ReadDWordAddress);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btn_WriteWord);
            this.Controls.Add(this.btn_WriteBool);
            this.Controls.Add(this.btn_ReadWord);
            this.Controls.Add(this.tb_WriteWordValue);
            this.Controls.Add(this.tb_WriteWordAddress);
            this.Controls.Add(this.tb_WriteBoolValue);
            this.Controls.Add(this.tb_WriteBoolAddress);
            this.Controls.Add(this.tb_ReadWordLength);
            this.Controls.Add(this.tb_ReadWordAddress);
            this.Controls.Add(this.tb_ReadBoolLength);
            this.Controls.Add(this.tb_ReadBoolAddress);
            this.Controls.Add(this.tb_port);
            this.Controls.Add(this.tb_ip);
            this.Controls.Add(this.btn_ReadBool);
            this.Controls.Add(this.connect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "汇川PLC ModbusTCP通信工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Button btn_ReadBool;
        private System.Windows.Forms.TextBox tb_ip;
        private System.Windows.Forms.TextBox tb_port;
        private System.Windows.Forms.TextBox tb_ReadBoolAddress;
        private System.Windows.Forms.TextBox tb_ReadBoolLength;
        private System.Windows.Forms.TextBox tb_ReadWordAddress;
        private System.Windows.Forms.TextBox tb_ReadWordLength;
        private System.Windows.Forms.TextBox tb_WriteBoolAddress;
        private System.Windows.Forms.TextBox tb_WriteBoolValue;
        private System.Windows.Forms.TextBox tb_WriteWordAddress;
        private System.Windows.Forms.TextBox tb_WriteWordValue;
        private System.Windows.Forms.Button btn_ReadWord;
        private System.Windows.Forms.Button btn_WriteBool;
        private System.Windows.Forms.Button btn_WriteWord;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_WriteDWord;
        private System.Windows.Forms.Button btn_ReadDWord;
        private System.Windows.Forms.TextBox tb_WriteDWordValue;
        private System.Windows.Forms.TextBox tb_WriteDWordAddress;
        private System.Windows.Forms.TextBox tb_ReadDWordLength;
        private System.Windows.Forms.TextBox tb_ReadDWordAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_SlaveNum;
        private System.Windows.Forms.CheckBox cb_IsNotUWord;
        private System.Windows.Forms.CheckBox cb_IsNotUDWord;
    }
}

