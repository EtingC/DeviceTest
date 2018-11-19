using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; 
using JK.Channels.TCP;
using JK.Program.Pattern;
using DeviceTest.Manager;
using JK.Libs.Utils;
using DeviceTest.TestForms;
using DeviceTest.ConfigForms;
using System.Diagnostics;
using DeviceTest.GatewayTest;
using DeviceTest.RegisterDevice;

namespace DeviceTest
{
    public partial class FormMain : Form
    {
        private const int PAGE_DEVICE_TEST = 0;
        private const int PAGE_GATEWAY_TEST = 1;
        private const int PAGE_ILOP_TEST = 2;
        private const int PAGE_MESSAGE  = 3;

        private Boolean autoExit = false;

        private FormConfig formConfig;
        public FormDeviceTest formDeviceTest;
        public FormGatewayTest formGatewayTest;
        public FormiLopTest formiLopTest;
        private FormImportDevice formImportDevice { get; set; }
        public FormMain()
        {
            InitializeComponent();
            this.tableLayoutPanelMain.Dock = DockStyle.Fill;
            this.formImportDevice = new FormImportDevice();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Program.TestManager = new Manager.TestManager();
            Program.TestGateway = Program.TestManager.deviceController.gateway;
            Program.TestGatewayController = Program.TestManager.gatewayController;
            Program.TestManager.AttachObserver(this.Update);
            Program.TestManager.Initialize(StringResource.ConfigFileName);

            this.formConfig = new FormConfig();

            this.formDeviceTest = new FormDeviceTest();
            Utils.AddFormToContainer(this.formDeviceTest, this.tabPageDeviceTest);

            this.formGatewayTest = new FormGatewayTest();
            Utils.AddFormToContainer(this.formGatewayTest, this.tabPageGatewayTest);

            this.formiLopTest = new FormiLopTest();
            Utils.AddFormToContainer(this.formiLopTest, this.tabPageiLop);

        }



        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!autoExit)
            {
                if (MessageBox.Show("退出系统,是否继续？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    e.Cancel = false;
                    Program.TestManager.deviceController.SaveHistoryDevicesItemsWithProject();
                    this.timer.Enabled = false;
                    Program.TestManager.Dispose();
                }
                else
                {
                    e.Cancel = true;
                }
            }

        }

        private void toolStripStatusLabelGatewayStatus_Click(object sender, EventArgs e)
        {
            if (!Program.TestGateway.active)
            {
                Program.TestManager.deviceController.TCPClient.OpenSync();
            }
        }


        public void Update(int notifyEvent, string flag, object result, string message, object sender)
        {
            SubjectObserver.FormInvoke update = new SubjectObserver.FormInvoke(this.ShowStatus);
            try
            {
                this.Invoke(update, notifyEvent, flag, result, message, sender);
            }
            catch (System.InvalidOperationException)
            {
            }
            catch (System.ComponentModel.InvalidAsynchronousStateException)
            {

            }
        }

        private void RefreshHttpServerStatus()
        {
            if (Program.TestManager.httpController.active)
            {
                this.toolStripStatusLabelHttpServer.Image = PictureResource.ok;
            }
            else
            {
                 this.toolStripStatusLabelHttpServer.Image = PictureResource.error;
            }        
        }

        private void RefreshGatewayStatus()
        {
            if (Program.TestGateway.active)
            {
                this.toolStripStatusLabelGatewayStatus.Image = PictureResource.ok;
            }
            else
            {
                this.toolStripStatusLabelGatewayStatus.Image = PictureResource.error;
            }   
            this.toolStripStatusLabelGatewayStatus.ToolTipText =Program.TestGateway.detailMessage;
        }

        private void ShowStatus(int Event, string flag, object result, string message, object sender)
        {
            switch (Event)
            {
                case TCPClientChannel.TCP_CONTROL_EVENT:
                    {
                        this.RefreshGatewayStatus();
                        break;
                    }
                case (int)HttpRequestType.RefreshDeviceNodes:
                    {
                        this.RefreshHttpServerStatus(); 
                        break;  
                    }           
            }
            if (message!=null && message.Length > 0)
            {
                this.toolStripStatusLabelMessage.Text = message;
                this.richTextBoxMessage.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + message + "\n");
            }

        }

        private void toolStripStatusLabelHttpServer_Click(object sender, EventArgs e)
        {
            Program.TestManager.deviceController.RefreshDeviceNodes();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            this.timer.Enabled = true;
            if (Program.TestManager.InvalidWorkPath())
            {
                Utils.MessageBoxError("当前工作路径包含中文,请拷贝到无中文路径下使用！");
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Program.TestManager.OperateTask();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonAbout_Click(object sender, EventArgs e)
        {
            string PathName = StringResource.AboutFileName;
            if (File.Exists(PathName))
            {
                try
                {
                    Process p = Process.Start(PathName);
                    p.WaitForExit();//关键，等待外部程序退出后才能往下执行   
                }
                catch (System.ComponentModel.Win32Exception)
                {

                }
            }
        }

        private void ButtonConfig_Click(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            this.formConfig.ShowDialog();
            this.timer.Enabled = true;
            if (this.formConfig.DialogResult == DialogResult.OK)
            {
                Utils.MessageBoxWarning("系统设置已更改，请重启软件！");
                this.autoExit = true;
                this.Close();
            }
        }

        private void ShowUpgradeForm()
        {
            string PathName = StringResource.AutoUpgraderFileName;
            if (File.Exists(PathName))
            {
                try
                {
                    this.autoExit = true;
                    Process p = Process.Start(PathName);
                    this.Close();
                }
                catch (System.ComponentModel.Win32Exception)
                {

                }
            }       
        }

        private void toolStripButtonUpgrade_Click(object sender, EventArgs e)
        {
            if (Program.TestManager.upgradeController.CheckVersion())
            {
                this.ShowUpgradeForm();
            }
            else
            {
                Utils.MessageBoxInformation("无可用更新，当前已经是最新版本！");
            }
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tabControlMain.SelectedIndex)
            {
                case PAGE_GATEWAY_TEST:
                    {
                        Program.TestManager.workSpace = WorkSpace.Gateway;
                        Program.TestGatewayController.RefreshGatewayNodes();
                        Program.TestGatewayController.StartScan();
                        break;
                    }
                case PAGE_DEVICE_TEST:
                    {
                        Program.TestManager.workSpace = WorkSpace.Zigbee;
                        break;
                    }
                case PAGE_ILOP_TEST:
                    {
                        Program.TestManager.workSpace = WorkSpace.iLopGPRS;
                        break;
                    }
                default:
                    {

                        Program.TestGatewayController.StopScan();
                        break;                    
                    }
            }
        }

        private void timerCheckUpgrade_Tick(object sender, EventArgs e)
        {
            Program.TestManager.upgradeController.CheckToUpgrade();
            if (Program.TestManager.upgradeController.WaitTimeOut())
            {
                this.timerCheckUpgrade.Enabled = false; 
                Boolean result = Program.TestManager.upgradeController.CheckVersion();
                if (result)
                {
                    if (Utils.MessageBoxQuestion("有新的版本可以，是否进行升级?"))
                    {
                        this.ShowUpgradeForm();
                    }
                }
                        
            }
            
        }

        private void ToolStripMenuItemException_Click(object sender, EventArgs e)
        {
            string directory = StringResource.ExceptionDirectory;
            if (Directory.Exists(directory))
            {
                System.Diagnostics.Process.Start("explorer.exe", directory);
            }

        }

        private void ToolStripMenuItemPrintLabel_Click(object sender, EventArgs e)
        {
            try
            {
                this.formImportDevice.ShowDialog();
            }
            catch (System.ObjectDisposedException)
            {

            }
        }

    }
}
