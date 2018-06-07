using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace usbControl
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }
        public static bool state = true;  //软件是否为可用状态
        SoftReg softReg = new SoftReg();
        private void FormRegister_Load(object sender, EventArgs e)
        {
            this.txtHardware.Text = softReg.GetMNum();
        }        
        private void btnReg_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLicence.Text == softReg.GetRNum())
                {
                    MessageBox.Show("注册成功！重启软件后生效！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RegistryKey retkey = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("mySoftWare").CreateSubKey("Register.INI").CreateSubKey(txtLicence.Text);
                    retkey.SetValue("UserName", "Rsoft");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("注册码错误！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLicence.SelectAll();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (state == true)
            {
                this.Close();
            }
            else
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}