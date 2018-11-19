using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeviceTest.Manager;
using DeviceTest.Center;
using System.Diagnostics;
using JK.Libs.Utils;

namespace DeviceTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// 
        public static TestManager TestManager;
        public static Gateway TestGateway;
        public static GatewayController TestGatewayController;
        [STAThread]
        static void Main()
        {
             Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
             //处理UI线程异常
             Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Utils.ApplicationThreadException);
             //处理非线程异常
             AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Utils.CurrentDomainUnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
