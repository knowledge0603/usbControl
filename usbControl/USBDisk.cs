using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Diagnostics;
using System.Windows.Forms;

namespace usbControl
{
    public class USBDisk
    {
        #region 获取U盘的盘符和序列号
        public static string GetUSB()
        {
            ManagementObjectSearcher SUSBDisk = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk where drivetype=2 and Size>943718400 ");//可移动磁盘且容量大于900MB
            ManagementObjectSearcher SUSBSN = new ManagementObjectSearcher("SELECT * FROM Win32_USBHub where Name ='USB 大容量存储设备' or Name='USB Mass Storage Device' ");//Win7和WinXP
            string USBDisk = ""; string USBSN = "";
            if (SUSBDisk.Get().Count == 0 || SUSBSN.Get().Count == 0)
            {
                return "null";
            }
            else if (SUSBDisk.Get().Count == 1 && SUSBSN.Get().Count == 1 )
            {
                foreach (ManagementObject moUSB in SUSBDisk.Get())
                {
                    USBDisk = moUSB["DeviceID"].ToString();
                }
                /*foreach (ManagementObject moUSB in SUSBSN.Get())
                {
                    string DId = moUSB["DeviceID"].ToString();
                    USBSN = DId.Substring(DId.LastIndexOf("\\") + 1);
                }*/
               // return USBDisk + "\\" + USBSN;
                return USBDisk ;
            }
            else
            {
                foreach (ManagementObject moUSB in SUSBDisk.Get())
                {
                    USBDisk = USBDisk + moUSB["DeviceID"].ToString() ;
                }
                return USBDisk;
            }
        }
        #endregion

        #region 格式化U盘
        public static bool FomatUSB(string USBSN,string fromatType)
        {
           // string USB = USBSN.Substring(0, 2);
            string strcmd = "format " + USBSN + ": /FS:"+ fromatType + " /Q  /Y";
            string strOutPut=  Execute(strcmd,3000, USBSN,"format");
            if (strOutPut.Length >= 60)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region 复制文件到U盘
        public static string  CopyUSB(string copyType,string path, string UsbName)
        {
            string strCopyCmd = "";
            if (copyType=="copy") {
                //copy c:\windows\*.* d:\
                //将C盘windows目录下所有文件拷贝到D盘根目录
                strCopyCmd = "copy " + path  +"  "+ UsbName + ":\\ ";
            }

            if (copyType == "xcopy")
            {
                //要拷贝子目录用xcopy命令加参数 / S
                //xcopy c:\windows\*.* d:\windows / s
                strCopyCmd = "xcopy " + path + "\\*.* " + UsbName + ":\\  /s ";
            }

            string strOutPut = ExecuteXcopy(strCopyCmd, 30000);

            return strOutPut;


        }
        #endregion


        #region 比较U盘文件
        public static bool CompUSB(string dir1, string dir2,string UsbName)
        {
            string strCopyCmd = "";
            strCopyCmd = "comp /f  " + dir1 +" "+ dir2 ;

            string strOutPut = ExecuteComp(strCopyCmd, 2000, UsbName);
            if (strOutPut.Length >= 60)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 执行DOS命令，等待命令执行的时间（单位：毫秒），如果设定为0，则无限等待，返回DOS命令的输出，如果发生异常，返回空字符串
        public static string Execute(string dosCommand, int milliseconds,string usbName,string cmdType)
        {
            string output = "";     //输出字符串
            if (dosCommand != null && dosCommand != "")
            {
                Process process = new Process();     //创建进程对象
                process.StartInfo.FileName = "cmd.exe";  //设定需要执行的命令
                process.StartInfo.UseShellExecute = false; //不使用系统外壳程序启动
                process.StartInfo.RedirectStandardInput = true; //不重定向输入
                process.StartInfo.RedirectStandardOutput = true; //重定向输出，而不是默认的显示在dos控制台
                process.StartInfo.RedirectStandardError = true;  //输出错误信息
                process.StartInfo.CreateNoWindow = true;  //不创建窗口
                process.StartInfo.Arguments = "/C  " + dosCommand;   //设定参数，其中的“/C”表示执行完命令后马上退出
                try
                {
                    if (process.Start())       //开始进程
                    {
                        //TimeSpan gcpBegin = Process.GetCurrentProcess().TotalProcessorTime;

                        TimeSpan gcpBegin = Process.GetCurrentProcess().TotalProcessorTime;
                       // MessageBox.Show(gcpBegin.ToString());
                        TimeSpan utime = Process.GetCurrentProcess().UserProcessorTime;
                        //MessageBox.Show(utime.ToString());
                        if (milliseconds == 0)
                            process.WaitForExit();     //这里无限等待进程结束
                        else
                            process.WaitForExit(milliseconds);  //这里等待进程结束，等待时间为指定的毫秒
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出结果。
                        if (process.ExitCode==0)
                        {
                            return "fileCopySuccess";
                        }
                    }
                }
                catch(Exception ex)
                {
                    if (cmdType=="format") {
                      //  MessageBox.Show(usbName + "盘格式化失败！");
                    }

                    if (cmdType == "copy")
                    {
                       // MessageBox.Show(usbName + "盘复制失败！");
                    }
                    output = ex.Message;
                }
                finally
                {
                    if (process != null)
                    {
                        process.Close();
                        process.Dispose();
                    }
                }
            }
            if (output.Length >= 60||output.Contains("复制"))
            {
                if (cmdType == "format")
                {
                  //  MessageBox.Show(usbName + "盘格式化成功！");
                }
                if (cmdType == "copy")
                {
                  //  MessageBox.Show(usbName + "盘复制成功！");
                }
            }
            else
            {
                if (cmdType == "format")
                {
                   // MessageBox.Show(usbName + "盘格式化失败！");
                }
                if (cmdType == "copy")
                {
                  //  MessageBox.Show(usbName + "盘复制失败！");
                }
            }
            return output;
        }
        #endregion


        #region 执行DOS命令，等待命令执行的时间（单位：毫秒），如果设定为0，则无限等待，返回DOS命令的输出，如果发生异常，返回空字符串
        public static string ExecuteComp(string dosCommand, int milliseconds, string usbName)
        {
            string output = "";     //输出字符串
            if (dosCommand != null && dosCommand != "")
            {
                Process process = new Process();     //创建进程对象
                process.StartInfo.FileName = "cmd.exe";  //设定需要执行的命令
                process.StartInfo.UseShellExecute = false; //不使用系统外壳程序启动
                process.StartInfo.RedirectStandardInput = true; //不重定向输入
                process.StartInfo.RedirectStandardOutput = true; //重定向输出，而不是默认的显示在dos控制台
                process.StartInfo.RedirectStandardError = true;  //输出错误信息
                process.StartInfo.CreateNoWindow = true;  //不创建窗口
                process.StartInfo.Arguments = "/C  " + dosCommand;   //设定参数，其中的“/C”表示执行完命令后马上退出
                try
                {
                    if (process.Start())       //开始进程
                    {
                        TimeSpan gcpBegin = Process.GetCurrentProcess().TotalProcessorTime;
                        MessageBox.Show(gcpBegin.ToString());
                        TimeSpan utime = Process.GetCurrentProcess().UserProcessorTime;
                        MessageBox.Show(utime.ToString());
                        if (milliseconds == 0)
                            process.WaitForExit();     //这里无限等待进程结束
                        else
                            process.WaitForExit(milliseconds);  //这里等待进程结束，等待时间为指定的毫秒
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出结果。
                    }
                }
                catch (Exception ex)
                {
                    
                    output = ex.Message;
                }
                finally
                {
                    if (process != null)
                    {
                        process.Close();
                        process.Dispose();
                    }
                }
            }
           
            else
            {
               
                   // MessageBox.Show(usbName + "盘复制失败！");
               
            }
            return output;
        }
        #endregion


        #region 执行DOS命令，等待命令执行的时间（单位：毫秒），如果设定为0，则无限等待，返回DOS命令的输出，如果发生异常，返回空字符串
        public static string ExecuteXcopy(string dosCommand, int milliseconds)
        {
            string output = "";     //输出字符串
            if (dosCommand != null && dosCommand != "")
            {
                Process process = new Process();     //创建进程对象
                process.StartInfo.FileName = "cmd.exe";  //设定需要执行的命令
                process.StartInfo.UseShellExecute = false; //不使用系统外壳程序启动
                process.StartInfo.RedirectStandardInput = true; //不重定向输入
                process.StartInfo.RedirectStandardOutput = true; //重定向输出，而不是默认的显示在dos控制台
                process.StartInfo.RedirectStandardError = true;  //输出错误信息
                process.StartInfo.CreateNoWindow = true;  //不创建窗口
                process.StartInfo.Arguments = "/C  " + dosCommand;   //设定参数，其中的“/C”表示执行完命令后马上退出
                try
                {
                    if (process.Start())       //开始进程
                    {
                        //TimeSpan gcpBegin = Process.GetCurrentProcess().TotalProcessorTime;
                        //TimeSpan gcpBegin = Process.GetCurrentProcess().TotalProcessorTime;
                        // MessageBox.Show(gcpBegin.ToString());
                        //TimeSpan utime = Process.GetCurrentProcess().UserProcessorTime;
                        //MessageBox.Show(utime.ToString());
                        if (milliseconds == 0)
                            process.WaitForExit();     //这里无限等待进程结束
                        else
                            process.WaitForExit(milliseconds);  //这里等待进程结束，等待时间为指定的毫秒
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出结果。
                        if (process.ExitCode == 0)
                        {
                            return "fileCopySuccess";
                        }
                        else {

                            return "fileCopyError";
                        }
                    }
                }
                catch (Exception ex)
                {
                    output = ex.Message;
                }
                finally
                {
                    if (process != null)
                    {
                        process.Close();
                        process.Dispose();
                    }
                }
            }
            return output;
        }
        #endregion
    }
}
