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
            this.SuspendLayout();
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(644, 64);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(75, 23);
            this.connect.TabIndex = 1;
            this.connect.Text = "连接";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // btn_ReadBool
            // 
            this.btn_ReadBool.Location = new System.Drawing.Point(644, 131);
            this.btn_ReadBool.Name = "btn_ReadBool";
            this.btn_ReadBool.Size = new System.Drawing.Size(75, 23);
            this.btn_ReadBool.TabIndex = 3;
            this.btn_ReadBool.Text = "读取bool";
            this.btn_ReadBool.UseVisualStyleBackColor = true;
            this.btn_ReadBool.Click += new System.EventHandler(this.btn_ReadBool_Click);
            // 
            // tb_ip
            // 
            this.tb_ip.Location = new System.Drawing.Point(121, 66);
            this.tb_ip.Name = "tb_ip";
            this.tb_ip.Size = new System.Drawing.Size(100, 21);
            this.tb_ip.TabIndex = 5;
            this.tb_ip.Text = "127.0.0.1";
            // 
            // tb_port
            // 
            this.tb_port.Location = new System.Drawing.Point(244, 66);
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(100, 21);
            this.tb_port.TabIndex = 6;
            this.tb_port.Text = "502";
            // 
            // tb_ReadBoolAddress
            // 
            this.tb_ReadBoolAddress.Location = new System.Drawing.Point(121, 131);
            this.tb_ReadBoolAddress.Name = "tb_ReadBoolAddress";
            this.tb_ReadBoolAddress.Size = new System.Drawing.Size(100, 21);
            this.tb_ReadBoolAddress.TabIndex = 7;
            this.tb_ReadBoolAddress.Text = "q100";
            // 
            // tb_ReadBoolLength
            // 
            this.tb_ReadBoolLength.Location = new System.Drawing.Point(244, 131);
            this.tb_ReadBoolLength.Name = "tb_ReadBoolLength";
            this.tb_ReadBoolLength.Size = new System.Drawing.Size(100, 21);
            this.tb_ReadBoolLength.TabIndex = 8;
            // 
            // tb_ReadWordAddress
            // 
            this.tb_ReadWordAddress.Location = new System.Drawing.Point(121, 176);
            this.tb_ReadWordAddress.Name = "tb_ReadWordAddress";
            this.tb_ReadWordAddress.Size = new System.Drawing.Size(100, 21);
            this.tb_ReadWordAddress.TabIndex = 9;
            this.tb_ReadWordAddress.Text = "m100";
            // 
            // tb_ReadWordLength
            // 
            this.tb_ReadWordLength.Location = new System.Drawing.Point(244, 176);
            this.tb_ReadWordLength.Name = "tb_ReadWordLength";
            this.tb_ReadWordLength.Size = new System.Drawing.Size(100, 21);
            this.tb_ReadWordLength.TabIndex = 10;
            // 
            // tb_WriteBoolAddress
            // 
            this.tb_WriteBoolAddress.Location = new System.Drawing.Point(121, 217);
            this.tb_WriteBoolAddress.Name = "tb_WriteBoolAddress";
            this.tb_WriteBoolAddress.Size = new System.Drawing.Size(100, 21);
            this.tb_WriteBoolAddress.TabIndex = 11;
            this.tb_WriteBoolAddress.Text = "q100";
            // 
            // tb_WriteBoolValue
            // 
            this.tb_WriteBoolValue.Location = new System.Drawing.Point(244, 217);
            this.tb_WriteBoolValue.Name = "tb_WriteBoolValue";
            this.tb_WriteBoolValue.Size = new System.Drawing.Size(345, 21);
            this.tb_WriteBoolValue.TabIndex = 12;
            // 
            // tb_WriteWordAddress
            // 
            this.tb_WriteWordAddress.Location = new System.Drawing.Point(121, 256);
            this.tb_WriteWordAddress.Name = "tb_WriteWordAddress";
            this.tb_WriteWordAddress.Size = new System.Drawing.Size(100, 21);
            this.tb_WriteWordAddress.TabIndex = 14;
            this.tb_WriteWordAddress.Text = "m100";
            // 
            // tb_WriteWordValue
            // 
            this.tb_WriteWordValue.Location = new System.Drawing.Point(244, 256);
            this.tb_WriteWordValue.Name = "tb_WriteWordValue";
            this.tb_WriteWordValue.Size = new System.Drawing.Size(345, 21);
            this.tb_WriteWordValue.TabIndex = 15;
            // 
            // btn_ReadWord
            // 
            this.btn_ReadWord.Location = new System.Drawing.Point(644, 174);
            this.btn_ReadWord.Name = "btn_ReadWord";
            this.btn_ReadWord.Size = new System.Drawing.Size(75, 23);
            this.btn_ReadWord.TabIndex = 17;
            this.btn_ReadWord.Text = "读取word";
            this.btn_ReadWord.UseVisualStyleBackColor = true;
            this.btn_ReadWord.Click += new System.EventHandler(this.btn_ReadWord_Click);
            // 
            // btn_WriteBool
            // 
            this.btn_WriteBool.Location = new System.Drawing.Point(644, 217);
            this.btn_WriteBool.Name = "btn_WriteBool";
            this.btn_WriteBool.Size = new System.Drawing.Size(75, 23);
            this.btn_WriteBool.TabIndex = 18;
            this.btn_WriteBool.Text = "写入bool";
            this.btn_WriteBool.UseVisualStyleBackColor = true;
            this.btn_WriteBool.Click += new System.EventHandler(this.btn_WriteBool_Click);
            // 
            // btn_WriteWord
            // 
            this.btn_WriteWord.Location = new System.Drawing.Point(644, 256);
            this.btn_WriteWord.Name = "btn_WriteWord";
            this.btn_WriteWord.Size = new System.Drawing.Size(75, 23);
            this.btn_WriteWord.TabIndex = 19;
            this.btn_WriteWord.Text = "写入word";
            this.btn_WriteWord.UseVisualStyleBackColor = true;
            this.btn_WriteWord.Click += new System.EventHandler(this.btn_WriteWord_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(121, 304);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(598, 88);
            this.listBox1.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
            this.Name = "Form1";
            this.Text = "Form1";
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
    }
}

