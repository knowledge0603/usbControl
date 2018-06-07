namespace usbControl
{
    partial class frmUsbControl
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBoxCopy = new System.Windows.Forms.GroupBox();
            this.btnCompFile = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox100 = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBoxCopy.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 23);
            this.button1.TabIndex = 0;
            this.button1.Tag = "1001";
            this.button1.Text = "格式化";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(17, 26);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Tag = "1001";
            this.button2.Text = "复制";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxFile
            // 
            this.textBoxFile.ForeColor = System.Drawing.Color.Red;
            this.textBoxFile.Location = new System.Drawing.Point(98, 26);
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.Size = new System.Drawing.Size(390, 21);
            this.textBoxFile.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(494, 25);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(36, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBoxCopy
            // 
            this.groupBoxCopy.Controls.Add(this.textBoxFile);
            this.groupBoxCopy.Controls.Add(this.button3);
            this.groupBoxCopy.Controls.Add(this.button2);
            this.groupBoxCopy.Controls.Add(this.btnCompFile);
            this.groupBoxCopy.Location = new System.Drawing.Point(384, 12);
            this.groupBoxCopy.Name = "groupBoxCopy";
            this.groupBoxCopy.Size = new System.Drawing.Size(619, 64);
            this.groupBoxCopy.TabIndex = 4;
            this.groupBoxCopy.TabStop = false;
            this.groupBoxCopy.Text = "文件";
            // 
            // btnCompFile
            // 
            this.btnCompFile.Location = new System.Drawing.Point(536, 25);
            this.btnCompFile.Name = "btnCompFile";
            this.btnCompFile.Size = new System.Drawing.Size(75, 22);
            this.btnCompFile.TabIndex = 6;
            this.btnCompFile.Text = "比对";
            this.btnCompFile.UseVisualStyleBackColor = true;
            this.btnCompFile.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(261, 21);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(79, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "清除卷";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(847, 461);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 45);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(928, 461);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 45);
            this.button9.TabIndex = 7;
            this.button9.Text = "退出";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(98, 460);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(737, 46);
            this.progressBar1.TabIndex = 10;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "EXFAT",
            "NTFS",
            "FAT",
            "FAT32",
            "CARD"});
            this.comboBox2.Location = new System.Drawing.Point(117, 24);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 11;
            this.comboBox2.Text = "EXFAT";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Location = new System.Drawing.Point(3, 446);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 2);
            this.panel1.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Location = new System.Drawing.Point(1, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1002, 2);
            this.panel2.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Location = new System.Drawing.Point(19, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(359, 64);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "U盘";
            // 
            // checkBox100
            // 
            this.checkBox100.AutoSize = true;
            this.checkBox100.Location = new System.Drawing.Point(19, 476);
            this.checkBox100.Name = "checkBox100";
            this.checkBox100.Size = new System.Drawing.Size(48, 16);
            this.checkBox100.TabIndex = 14;
            this.checkBox100.Text = "全选";
            this.checkBox100.UseVisualStyleBackColor = true;
            this.checkBox100.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // frmUsbControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 522);
            this.Controls.Add(this.checkBox100);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.groupBoxCopy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmUsbControl";
            this.Text = "u盘操作";
            this.Load += new System.EventHandler(this.FormUsbControl_Load);
            this.groupBoxCopy.ResumeLayout(false);
            this.groupBoxCopy.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBoxCopy;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnCompFile;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox100;
        private System.Windows.Forms.Timer timer1;
    }
}

