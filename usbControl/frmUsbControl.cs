using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace usbControl
{
    public partial class frmUsbControl : Form
    {
        #region 初始化
        public frmUsbControl()
        {
            InitializeComponent();

           
            Control();
           
           // timer1.Start();
        }
        #endregion

        #region 变量区
        static string filePath;
        static CheckBox[] cmd;
        string[] strArray;
        static TextBox[] txtBox;
        static ProgressBar[] processList;
        Button[] usbId;
        FlowLayoutPanel[] flowLayoutPanel1 = new FlowLayoutPanel[5];
        GroupBox groupBox1 = new GroupBox();
        string[] usbIdArray;
        int usbCount = 0;
        static System.Windows.Forms.Timer[] time;
        static string processType = "";
        static Dictionary<int, string> compDictionary = null;
        static Dictionary<int, string> usbDictionary = null;
        #endregion

        #region 软件 注册 使用次数 弹出 处理
        SoftReg softReg = new SoftReg();
        private void FormUsbControl_Load(object sender, EventArgs e)
        {
            try
            {
                //判断软件是否注册
                RegistryKey retkey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("mySoftWare").CreateSubKey("Register.INI");
                foreach (string strRNum in retkey.GetSubKeyNames())
                {
                    if (strRNum == softReg.GetRNum())
                    {
                        return;
                    }
                }
                MessageBox.Show("此软件尚未注册！您现在使用的是试用版，可以免费试用700次！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Int32 tLong;    //已使用次数
                try
                {
                    tLong = (Int32)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\mySoftWare", "UseTimes", 0);
                    MessageBox.Show("您已经使用了" + tLong + "次！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("欢迎使用本软件！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\mySoftWare", "UseTimes", 0, RegistryValueKind.DWord);
                }
                //判断是否可以继续试用
                tLong = (Int32)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\mySoftWare", "UseTimes", 0);
                if (tLong < 700)
                {
                    int tTimes = tLong + 1;
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\mySoftWare", "UseTimes", tTimes);
                }
                else {
                    DialogResult result = MessageBox.Show("试用次数已到！您是否需要注册？", "信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        FormRegister.state = false; //设置软件状态为不可用
                        btnReg_Click(sender, e);    //打开注册窗口
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请以管理员身份运行!");
                this.Close();
            }
        }
        private void btnReg_Click(object sender, EventArgs e)
        {
            FormRegister frmRegister = new FormRegister();
            frmRegister.ShowDialog();
        }
        #endregion

        #region  动态生成 盘符合 操作
        private void Control()
        {
            CheckForIllegalCrossThreadCalls = false;
            usbIdArray = new string[20] { "无", "无", "无", "无", "无", "无", "无", "无", "无", "无", "无", "无", "无", "无", "无", "无", "无", "无", "无", "无" };
            if (USBDisk.GetUSB() != "null")
            {
                usbIdArray = USBDisk.GetUSB().Substring(0, (USBDisk.GetUSB().Length - 1)).Split(':');
                usbCount = usbIdArray.Length;
                if (usbCount > 20) {
                    MessageBox.Show("usb盘数目超过20个！");
                    return;
                }
            }

            for (int n = 0; n < 5; n++)
            {
                if (flowLayoutPanel1[n] != null)
                {
                    flowLayoutPanel1[n].Dispose();
                }
            }

            for (int n = 0; n < 5; n++)
            {
                flowLayoutPanel1[n] = new FlowLayoutPanel();
            }
            for (int j = 0; j < 5; j++)
            {
                flowLayoutPanel1[j].Controls.Clear();
                flowLayoutPanel1[j].Size = new System.Drawing.Size(260, 320);
                flowLayoutPanel1[j].Location = new System.Drawing.Point(260 * j, 120);
                this.Controls.Add(flowLayoutPanel1[j]);
                flowLayoutPanel1[j].AutoScroll = true;
            }

            cmd = new CheckBox[usbCount];
            usbId = new Button[usbCount];
            processList = new ProgressBar[usbCount];
            txtBox = new TextBox[usbCount];
            strArray = new string[usbCount];


            for (int i = 0; i < usbCount; i++)
            /* cmd = new CheckBox[20];
             usbId = new Button[20];
             processList = new ProgressBar[20];
             txtBox = new TextBox[20];
              strArray = new string[20];
              for (int i = 0; i < 20; i++)*/
            {
                //盘位 按钮 
                usbId[i] = new Button();
                usbId[i].Text = usbIdArray[i];
                usbId[i].Size = new Size(50, 30);
                usbId[i].Top = 25;
                usbId[i].Left = 10;
                if (i != 0)
                {
                    usbId[i].Top = 25;
                    usbId[i].Top = usbId[i].Top + usbId[i].Height + 5;
                }
                usbId[i].Visible = true;
                if (i < 5) {
                    flowLayoutPanel1[0].Controls.Add(usbId[i]);
                }
                else if (5 <= i && i < 10)
                {
                    flowLayoutPanel1[1].Controls.Add(usbId[i]);
                }
                else if (10 <= i && i < 15)
                {
                    flowLayoutPanel1[2].Controls.Add(usbId[i]);
                }
                else if (15 <= i && i < 20)
                {
                    flowLayoutPanel1[3].Controls.Add(usbId[i]);
                }
                else if (20 <= i && i < 25)
                {
                    flowLayoutPanel1[4].Controls.Add(usbId[i]);
                }

                //进度条
                processList[i] = new ProgressBar();
                processList[i].Size = new Size(130, 30);
                processList[i].Top = 25;
                processList[i].Left = 10;
                if (i != 0)
                {
                    processList[i].Top = 25;
                    processList[i].Top = processList[i].Top + processList[i].Height + 5;
                }
                processList[i].Visible = true;

                if (i < 5)
                {
                    flowLayoutPanel1[0].Controls.Add(processList[i]);

                }
                else if (5 <= i && i < 10)
                {
                    flowLayoutPanel1[1].Controls.Add(processList[i]);
                }
                else if (10 <= i && i < 15)
                {
                    flowLayoutPanel1[2].Controls.Add(processList[i]);
                }
                else if (15 <= i && i < 20)
                {
                    flowLayoutPanel1[3].Controls.Add(processList[i]);
                }
                else if (20 <= i && i < 25)
                {
                    flowLayoutPanel1[4].Controls.Add(processList[i]);
                }

                //copy命令按钮
                cmd[i] = new CheckBox();
                cmd[i].Checked = true;
                cmd[i].Size = new Size(20, 30);
                cmd[i].Top = 25;
                cmd[i].Left = 10;
                cmd[i].Tag = usbIdArray[i];
                strArray[i] = usbIdArray[i] + "|" + i;
                if (i != 0)
                {
                    cmd[i].Top = 25;
                    cmd[i].Top = cmd[i].Top + cmd[i].Height + 5;
                }
                cmd[i].Visible = true;

                if (i < 5)
                {
                    flowLayoutPanel1[0].Controls.Add(cmd[i]);
                }
                else if (5 <= i && i < 10)
                {
                    flowLayoutPanel1[1].Controls.Add(cmd[i]);
                }
                else if (10 <= i && i < 15)
                {
                    flowLayoutPanel1[2].Controls.Add(cmd[i]);
                }
                else if (15 <= i && i < 20)
                {
                    flowLayoutPanel1[3].Controls.Add(cmd[i]);
                }
                else if (20 <= i && i < 25)
                {
                    flowLayoutPanel1[4].Controls.Add(cmd[i]);
                }
                //text box  文本提示框
                txtBox[i] = new TextBox();
                txtBox[i].Size = new Size(210, 30);
                txtBox[i].Top = 25;
                txtBox[i].Left = 10;
                txtBox[i].Tag = usbIdArray[i];
                txtBox[i].BackColor = Color.Snow;
                if (i != 0)
                {
                    txtBox[i].Top = 25;
                    txtBox[i].Top = cmd[i].Top + cmd[i].Height + 5;
                }
                txtBox[i].Visible = true;

                if (i < 5)
                {
                    flowLayoutPanel1[0].Controls.Add(txtBox[i]);
                }
                else if (5 <= i && i < 10)
                {
                    flowLayoutPanel1[1].Controls.Add(txtBox[i]);
                }
                else if (10 <= i && i < 15)
                {
                    flowLayoutPanel1[2].Controls.Add(txtBox[i]);
                }
                else if (15 <= i && i < 20)
                {
                    flowLayoutPanel1[3].Controls.Add(txtBox[i]);
                }
                else if (20 <= i && i < 25)
                {
                    flowLayoutPanel1[4].Controls.Add(txtBox[i]);
                }
            }
        }
        #endregion 

        #region  拷贝
        private void button2_Click(object sender, EventArgs e)
        {
            processType = "copy";
            btnRefresh.Enabled = false;
            filePath = textBoxFile.Text;
            if (textBoxFile.Text == "")
            {
                MessageBox.Show("请打开文件夹或选择文件！");
                return;
            }
            InitializeMyTimer();
            for (int k = 0; k < cmd.Length; k++)
            {
                if (cmd[k].Checked == true)
                {
                    copyProcess objCopyProcess = new copyProcess(cmd[k].Tag.ToString(), k, textBoxFile.Text);
                    ThreadStart startCopy = new ThreadStart(objCopyProcess.excuteXcopy);
                    Thread thredCopy = new Thread(startCopy);
                    thredCopy.Start();
                }
            }
        }
        #endregion

        #region 打开文件
        private void button3_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = fbd.SelectedPath;
            }
            textBoxFile.Text = path;
        }
        #endregion

        #region  格式化

        private void button1_Click(object sender, EventArgs e)
        {
                      
            btnRefresh.Enabled = false;
            for (int k = 0; k < cmd.Length; k++)
            {
                if (cmd[k].Checked == true)
                {
                    txtBox[k].Text = "";
                    txtBox[k].Text = "格式化中...";
                    USBDisk.FomatUSB(cmd[k].Tag.ToString(), comboBox2.Text);
                    txtBox[k].Text = "格式化成功！";
                }
            }
            btnRefresh.Enabled = true;
           
        }
        public void Process()
        {
            for (int k = 0; k < cmd.Length; k++)
            {
                for (int i = 0; i <= 100; i++)
                {
                    processList[k].Value = i;
                    Thread.Sleep(10);
                }
            }
        }
        #endregion

        #region 开始停止按钮
        private void button6_Click(object sender, EventArgs e)
        {
           
           
          
        }
        #endregion

        #region 进度条处理 文本框处理
        // Call this method from the constructor of the form.
        public static void InitializeMyTimer()
        {
            time = new System.Windows.Forms.Timer[cmd.Length];
            for (int k = 0; k < cmd.Length; k++)
            {
                if (cmd[k].Checked == true)
                {
                    time[k] = new System.Windows.Forms.Timer();
                    // Set the interval for the timer.
                    time[k].Interval = 10;
                    // Connect the Tick event of the timer to its event handler.
                    if (processType == "copy")
                    {
                        time[k].Tick += new EventHandler(IncreaseProgressBar);
                    }
                    if (processType == "comp")
                    {
                        time[k].Tick += new EventHandler(IncreasetTextBox);
                    }

                    // Start the timer.
                    time[k].Start();
                }
            }
        }

        public static void IncreaseProgressBar(object sender, EventArgs e)
        {
            for (int k = 0; k < cmd.Length; k++)
            {
                if (cmd[k].Checked == true) {
                    // Increment the value of the ProgressBar a value of one each time.
                    long temp = GetHardDiskSpace(cmd[k].Tag.ToString()) - GetHardDiskFreeSpace(cmd[k].Tag.ToString());
                    long temp1 = GetDirectoryLength(filePath);
                    double SumXDXS = Math.Round(Convert.ToDouble(temp) / Convert.ToDouble(temp1), 2);
                    if (Convert.ToInt32(SumXDXS * 100) > 100)
                    {
                        txtBox[k].Text = 100 + "% 完成";
                        time[k].Stop();
                        return;
                    }
                    else {
                        processList[k].Value = Convert.ToInt32(SumXDXS * 100);
                        // Display the textual value of the ProgressBar in the StatusBar control's first panel.
                        if (txtBox[k].Text != "文件复制异常")
                        {
                            txtBox[k].Text = processList[k].Value.ToString() + "% 完成";
                        }
                        // Determine if we have completed by comparing the value of the Value property to the Maximum value.
                        if (processList[k].Value == 100)
                        {
                            // Stop the timer.
                            time[k].Stop();
                        }
                    }
                    if (txtBox[k].Text == "文件复制异常")
                    {
                        time[k].Stop();
                        return;
                    }
                }
            }
        }


        public static void IncreasetTextBox(object sender, EventArgs e)
        {
            for (int k = 0; k < cmd.Length; k++)
            {
                if (cmd[k].Checked == true)
                {
                    // Director(filePath, k);
                    //txtBox[k].Text = ;

                     //txtBox[k].Text = "";
                      //txtBox[k].Text = diff(fsinfo.FullName, cmd[diskIndex].Tag.ToString(), fsinfo.FullName.Substring(filePath.Length, fsinfo.FullName.Length- filePath.Length));
                    //txtBox[k].Refresh();
                    // if (txtBox[diskIndex].Text.ToString().IndexOf("文件不同")!=-1) {
                    //     txtBox[diskIndex].BackColor = Color.YellowGreen;
                    //   time[diskIndex].Stop();
                    //   break;
                    // }
                }
            }
        }
        
        #endregion

        #region xcopy 处理
        public class copyProcess
        {
            private string usbName;
            private int index;
            private string filePath;

            public copyProcess(string usbName, int index, string filePath)
            {
                this.usbName = usbName;
                this.index = index;
                this.filePath = filePath;
            }
            public void excuteXcopy()
            {
                //string strOutPut = USBDisk.CopyUSB("xcopy", filePath, usbName);

                var objCopyTask = new Task<outPutCopy>(copyTask, new inPutCopy() { usbNameInput = usbName, filePathInput = filePath });
                objCopyTask.Start();
                if (objCopyTask.Result.OutPut.ToString() == "fileCopySuccess")
                {
                    processList[index].Value = 100;
                    // Display the textual value of the ProgressBar in the StatusBar control's first panel.
                    txtBox[index].Text = 100 + "% 完成";
                    // Determine if we have completed by comparing the value of the Value property to the Maximum value.
                    time[index].Stop();
                }
                if (objCopyTask.Result.OutPut.ToString() == "fileCopyError")
                {
                    txtBox[index].Text = "文件复制异常";
                    txtBox[index].BackColor =Color.Red;
                    // Determine if we have completed by comparing the value of the Value property to the Maximum value.
                    time[index].Stop();
                    //Thread.ResetAbort(thredCopy);
                }
            }
           
        }
        #endregion

        #region  文件夾 u 盤 容量取得处理
        /// 获取指定驱动器的空间总大小(单位为GB)  
                /// </summary>  
        /// <param name=”str_HardDiskName”>只需输入代表驱动器的字母即可 </param>  
        /// <returns> </returns>  

        public static long GetHardDiskSpace(string str_HardDiskName)
        {
            try
            {
                long totalSize = new long();
                str_HardDiskName = str_HardDiskName + ":\\";
                System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
                foreach (System.IO.DriveInfo drive in drives)
                {
                    if (drive.Name == str_HardDiskName)
                    {
                        totalSize = drive.TotalSize / (1024 * 1024);
                    }
                }
                return totalSize;
            } catch {

                return 0L;
            }
            
        }

        /// <summary>  
        /// 获取指定驱动器的剩余空间总大小(单位为GB)  
        /// </summary>  
        /// <param name=”str_HardDiskName”>只需输入代表驱动器的字母即可 </param>  
        /// <returns> </returns>  

        public static long GetHardDiskFreeSpace(string str_HardDiskName)
        {
            long freeSpace = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace / (1024 * 1024);
                }
            }
            return freeSpace;
        }
        /// <summary>
        /// 获取指定路径的大小
        /// </summary>
        /// <param name="dirPath">路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            long len = 0;
            //判断该路径是否存在（是否为文件夹）
            if (!Directory.Exists(dirPath))
            {
                //查询文件的大小
                len = FileSize(dirPath);
            }
            else
            {
                //定义一个DirectoryInfo对象
                DirectoryInfo di = new DirectoryInfo(dirPath);

                //通过GetFiles方法，获取di目录中的所有文件的大小
                foreach (FileInfo fi in di.GetFiles())
                {
                    len += fi.Length / (1024 * 1024);
                }
                //获取di中所有的文件夹，并存到一个新的对象数组中，以进行递归
                DirectoryInfo[] dis = di.GetDirectories();
                if (dis.Length > 0)
                {
                    for (int i = 0; i < dis.Length; i++)
                    {
                        len += GetDirectoryLength(dis[i].FullName);
                    }
                }
            }
            return len;
        }
        //所给路径中所对应的文件大小
        public static long FileSize(string filePath)
        {
            //定义一个FileInfo对象，是指与filePath所指向的文件相关联，以获取其大小
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length / (1024 * 1024);
        }
        #endregion

        #region 全选  反选
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox100.Text == "反选")
            {
                checkBox100.Text = "全选";
                if (cmd != null)
                {
                    for (int k = 0; k < cmd.Length; k++)
                    {
                        cmd[k].Checked = false;
                    }
                }

            }
            else {

                checkBox100.Text = "反选";
                if (cmd != null)
                {
                    for (int k = 0; k < cmd.Length; k++)
                    {
                        cmd[k].Checked = true;
                    }
                }
            }


        }
        #endregion

        #region  task 调用
        public class inPutCopy
        {
            private string usbName;
            private string filePath;
            public string usbNameInput { get => usbName; set => usbName = value; }
            public string filePathInput { get => filePath; set => filePath = value; }
        }

        public class outPutCopy
        {
            private string outPut;
            public string OutPut { get => outPut; set => outPut = value; }
        }


        //public static outPutCopy xcopyTask(object filePath,object usbName) {
        public static outPutCopy copyTask(object para)
        {
           
            return new outPutCopy()
            {
                OutPut = USBDisk.CopyUSB("xcopy", ((inPutCopy)para).filePathInput, ((inPutCopy)para).usbNameInput)
            };
          
        }
        #endregion

        #region 比对按钮
        //比对按钮
        private void button5_Click(object sender, EventArgs e)
        {
            filePath = textBoxFile.Text;
            // processType = "comp";
            // InitializeMyTimer();
            processType = "comp";
            btnRefresh.Enabled = false;
            filePath = textBoxFile.Text;
            if (textBoxFile.Text == "")
            {
                MessageBox.Show("请打开文件夹或选择文件！");
                return;
            }
            compDictionary =  addFileDictionary(filePath);
            InitializeMyTimer();
            for (int k = 0; k < cmd.Length; k++)
            {
                if (cmd[k].Checked == true)
                {
                    diffProcess objDiffProcess = new diffProcess(cmd[k].Tag.ToString(),  textBoxFile.Text,k);
                    ThreadStart startDiff = new ThreadStart(objDiffProcess.excuteDiff);
                    Thread thredDiff = new Thread(startDiff);
                    thredDiff.Start();
                }
            }
        }
        #endregion

        #region diff flord 处理
        public class diffProcess
        {
            private string usbName;
            private int index;
            private string filePathDisk;

            public diffProcess(string usbName,  string filePathDisk,int index)
            {
                this.usbName = usbName;
                this.filePathDisk = filePathDisk;
                this.index = index;
            }
            public void excuteDiff()
            {
                var objDiffTask = new Task<outPutDiff>(diffTask, new inPutDiff() { usbNameInput = usbName, usbDiskPathInput = filePathDisk, indexDiskInput = index });
                objDiffTask.Start();
               /* txtBox[index].Text = objDiffTask.Result.OutPut.ToString();
                txtBox[index].Refresh();
                if (objDiffTask.Result.OutPut.ToString().IndexOf("文件不同")!=-1)
                {
                    txtBox[index].Text = "文件存在差异";
                    txtBox[index].BackColor = Color.Red;
                    txtBox[index].Refresh();
                    time[index].Stop();
                }*/
            }
        }

        public class inPutDiff
        {
            private string usbName;
            private string filePath;
            private string usbDiskPath;
            private int indexDisk;
            public string usbNameInput { get => usbName; set => usbName = value; }
            public string filePathInput { get => filePath; set => filePath = value; }
            public int indexDiskInput { get => indexDisk; set => indexDisk = value; }
            public string usbDiskPathInput { get => usbDiskPath; set => usbDiskPath = value; }
        }

        public class outPutDiff
        {
            private string outPut;
            public string OutPut { get => outPut; set => outPut = value; }
        }


        //public static outPutCopy xcopyTask(object filePath,object usbName) {
        public static outPutDiff diffTask(object para)
        {
            return new outPutDiff()
            {
                OutPut = diff(  ((inPutDiff)para).usbNameInput, ((inPutDiff)para).indexDiskInput)
            };
        }
        public static string diff(string diskName,int index)
        {
            string returnStr = "";
            DirectoryInfo directoryInfo = null;
            bool fileCompFlg = false;
            if (!diskName.Contains(":\\"))
            {
               directoryInfo = new DirectoryInfo(diskName + ":\\");
            }
            else
            {
                directoryInfo = new DirectoryInfo(diskName );
            }
            FileSystemInfo[] fsinfos = directoryInfo.GetFileSystemInfos();
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                //System Volume Information 不参与比较

                // if (fsinfo.FullName.ToString().Contains("System Volume Information"))
                // {
                //   continue;
                // }
                if (fsinfo is DirectoryInfo && fileCompFlg)     //判断是否为文件夹  
                {
                    diff(fsinfo.FullName, index);//递归调用  
                }
                else
                {
                    //查找隐藏文件不比对
                    string[] hiddenfiles = Directory.GetFiles(fsinfo.FullName, "*.*", SearchOption.AllDirectories);
                    foreach (var item in hiddenfiles)
                    {
                        if ((new FileInfo(item).Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)   //必须进行与运算，因为默认文件是“Hidden”+归档（二进制11）。而Hidden是10.因此与运算才可以判断
                        {
                            continue;
                        }
                    }
                    string filePathUsb = fsinfo.FullName;
                    fileCompFlg = isDiff( @filePathUsb);
                    if (fileCompFlg)
                    {
                        returnStr = filePathUsb + "文件相同";
                        txtBox[index].Text = filePathUsb + "文件相同";
                        txtBox[index].BackColor = Color.YellowGreen;
                        txtBox[index].Refresh();
                    }
                    else
                    {
                        txtBox[index].Text = filePathUsb + "文件不同"; 
                        txtBox[index].BackColor = Color.Red;
                        txtBox[index].Refresh();
                        returnStr = filePathUsb + "文件不同";
                        break;
                    }
                    if (intKeyDic != intUsbKeyDic)
                    {
                        txtBox[index].Text = "文件数量不同";
                        txtBox[index].BackColor = Color.Red;
                        txtBox[index].Refresh();
                        returnStr = "文件数量不同";
                    }
                }
            }
            return returnStr;
        }
        //usb dictionary key 
       static  int intUsbKeyDic = 0;
        public static bool isDiff( string filePathNameDisk)
        {
            bool returnComp = false;
            //创建一个哈希算法对象 
            using (HashAlgorithm hash = HashAlgorithm.Create())
            {
                try
                {
                    using (FileStream file1 = new FileStream(filePathNameDisk, FileMode.Open))
                    {
                        byte[] hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组 
                        string str1 = BitConverter.ToString(hashByte1);//将字节数组装换为字符串 
                        if (compDictionary.ContainsValue(str1))
                        {
                            usbDictionary.Add(intUsbKeyDic, str1);
                            intUsbKeyDic++;
                            returnComp = true;
                        }
                        else
                        {
                            returnComp = false;
                        }
                       
                    }
                }
                catch
                {
                    returnComp = false;
                }
                return returnComp;
                }
        }

        static int intKeyDic = 0;
        public static Dictionary<int, string> addFileDictionary(string uiSelectFilePath)
        {
            Dictionary<int, string> myDictionary = new Dictionary<int, string>();
            DirectoryInfo directoryInfo = new DirectoryInfo(uiSelectFilePath);
            FileSystemInfo[] fsinfos = directoryInfo.GetFileSystemInfos();
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo is DirectoryInfo )     //判断是否为文件夹  
                {
                    addFileDictionary(fsinfo.FullName);//递归调用  
                }
                else
                {
                    //创建一个哈希算法对象 
                    using (HashAlgorithm hash = HashAlgorithm.Create())
                    {
                        using (FileStream file1 = new FileStream(fsinfo.FullName, FileMode.Open))
                        {
                            byte[] hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组 
                            string strhashByte = BitConverter.ToString(hashByte1);//将字节数组装换为字符串 
                            myDictionary.Add(intKeyDic, strhashByte);
                            intKeyDic++;
                        }
                    }
            }
            }
            return myDictionary;
        }
        #endregion
    }
}
