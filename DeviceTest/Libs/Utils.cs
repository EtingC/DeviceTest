using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;





namespace JK.Libs.Utils
{
   


        
    public static class Utils
    {

        public static void KillProcess(string processName)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程   
            try
            {
                foreach (Process thisproc in Process.GetProcessesByName(processName))
                {
                    //找到程序进程,kill之。
                    if (!thisproc.CloseMainWindow())
                    {
                        thisproc.Kill();
                    }
                }

            }
            catch (Exception )
            {
                //MessageBox.Show(Exc.Message);
            }
        }


        public static void StopProcess(string processName)
        {
            try
            {
                System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName(processName);
                foreach (System.Diagnostics.Process p in ps)
                {
                    p.Kill();
                }
            }
            catch (Exception )
            {
                //throw ex;
            }
        }

        /// <summary>
        /// 处理未捕获异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            SaveLog("-----------------------begin--------------------------");
            SaveLog("CurrentDomain_UnhandledException" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            SaveLog("IsTerminating : " + e.IsTerminating.ToString());
            SaveLog(e.ExceptionObject.ToString());
            SaveLog("-----------------------end----------------------------");
        }

        /// <summary>
        /// 处理UI主线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ApplicationThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            SaveLog("-----------------------begin--------------------------");
            SaveLog("Application_ThreadException" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            SaveLog("Application_ThreadException:" + e.Exception.Message);
            SaveLog(e.Exception.StackTrace);
            SaveLog("-----------------------end----------------------------");
        }

        public static void SaveLog(string log)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\exception\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = path + "exception.txt";

            //采用using关键字，会自动释放
            using (FileStream fs = new FileStream(filePath, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    sw.WriteLine(log);
                }
            }
        }


        public static Boolean ContainChinese(string input)
        {
            string pattern = "[\u4e00-\u9fbb]";
            return Regex.IsMatch(input, pattern);
        }


        /// <summary>
        /// 获取两个字符串之间的字符
        /// </summary>
        /// <returns></returns>
        public static Boolean GetValueAnd(string strStart, string strEnd, string text ,ref string matchValue)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            string regex = @"^.*" + strStart + "(?<content>.+?)" + strEnd + ".*$";
            Regex rgClass = new Regex(regex, RegexOptions.Singleline);
            Match match = rgClass.Match(text);
            if (match.Success)
            { 
                matchValue =match.Groups["content"].Value;
            }
            return match.Success;
        } 


        public static void MessageBoxError(string message)
        {
            if (message.Length > 0)
            {
                MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void MessageBoxWarning(string message)
        {
            if (message.Length > 0)
            {
                MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void MessageBoxInformation(string message)
        {
            if (message.Length > 0)
            {
                MessageBox.Show(message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static Boolean MessageBoxQuestion(string message)
        {
            if (message.Length > 0)
            {
                return MessageBox.Show(message, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
            }
            return false;
        }


        public static void InitializeTableLayoutPanel(TableLayoutPanel container ,int rowCount, int columnCount)
        {
            if (rowCount != 0 && columnCount != 0)
            {
                container.ColumnCount = columnCount;
                container.RowCount = rowCount;
                container.RowStyles.Clear();
                int percent = 100 / container.RowCount;
                for (int i = 0; i < container.RowCount; i++)
                {
                    container.RowStyles.Add(new RowStyle(SizeType.Percent, percent));
                }
                container.ColumnStyles.Clear();
                percent = 100 / container.ColumnCount;
                for (int i = 0; i < container.ColumnCount; i++)
                {
                    container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percent));
                }
                container.Dock = System.Windows.Forms.DockStyle.Fill;
                container.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic).SetValue(container, true, null);
            }
        
        }

        public static void AddFormToContainer(Form form , Control container)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.TopLevel = false;
            form.Dock = System.Windows.Forms.DockStyle.Fill;
            container.Controls.Add(form);
            form.Show();              
        }
            
        public static void AddFormToTableLayoutPanel(Form form ,TableLayoutPanel container ,int row,int column)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.TopLevel = false;
            form.Dock = System.Windows.Forms.DockStyle.Fill;
            container.Controls.Add(form, column, row);
            form.Show();           
        }
                // 摘要: 
        //将List转换为TXT文件
        public static void WriteListToFile(List<string> list, string filePathName)
        {
            //创建一个文件流，用以写入或者创建一个StreamWriter           
            FileStream fileStream = new FileStream(filePathName, FileMode.OpenOrCreate, FileAccess.Write);
            fileStream.SetLength(0);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.Flush();
            // 使用StreamWriter来往文件中写入内容 
            try
            {
                streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < list.Count; i++)
                {
                    streamWriter.WriteLine(list[i]);
                }
                //关闭此文件 
                streamWriter.Flush();
            }
            finally
            {
                streamWriter.Close();
                fileStream.Close();
            }
        }

        public static Boolean DoPython(string StartFileName, string StartFileArg,Boolean hidden)
        {
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = StartFileName;      // 命令  
            CmdProcess.StartInfo.Arguments = StartFileArg;      // 参数  

            CmdProcess.StartInfo.CreateNoWindow = hidden;        // 不创建新窗口  
            CmdProcess.StartInfo.UseShellExecute = false;
            return CmdProcess.Start();
        }

        /// <summary>
        /// 读取文件中所有字符
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string LoadFromFile(string filename)
        {
            string content = "";
            if (System.IO.File.Exists(filename))
            {
                StreamReader reader = new StreamReader(filename, Encoding.UTF8);
                content = reader.ReadToEnd();
                reader.Close();
            }
            return content;
        }

        public static void SaveToFile(string filename, string content)
        {
            List<string> contents = new List<string>();
            contents.Add(content);
            WriteListToFile(contents, filename);        
        }



         
        /// 该函数设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。 
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_SHOWNOMAL = 1;
        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, SW_SHOWNOMAL);//显示
            SetForegroundWindow(instance.MainWindowHandle);//当到最前端
        }
        public static Process RuningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] Processes = Process.GetProcessesByName(currentProcess.ProcessName);
            foreach (Process process in Processes)
            {
                if (process.Id != currentProcess.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == currentProcess.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }




        /// <summary>
        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public static string String2Unicode(string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }


        /// <summary>
        /// 日志部分
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        public static void WriteLogs(string fileName, string type, string content)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory + fileName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                if (File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + type + "-->" + content);
                    sw.Close();
                }
            }
        }

    }
}
