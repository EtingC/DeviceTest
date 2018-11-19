using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JK.Libs.Utils;

namespace DeviceTest.ConfigForms
{
    public partial class FormDeviceControllerConfig : Form
    {
        public FormDeviceControllerConfig()
        {
            InitializeComponent();
        }

        private void RefreshViewer()
        {
            this.textBoxServerIP.Text = Program.TestManager.deviceController.TCPClient.Host;
            this.textBoxServerPort.Text = Program.TestManager.deviceController.TCPClient.Port.ToString();
            this.textBoxComPort.Text = Program.TestManager.deviceController.serialChannel.Port;
            this.comboBoxBaudrate.SelectedIndex = this.comboBoxBaudrate.Items.IndexOf(Program.TestManager.deviceController.serialChannel.BaudRate.ToString());
        }

        private void FormDeviceControllerConfig_Load(object sender, EventArgs e)
        {
            this.RefreshViewer();
        }
        private Boolean SaveGatewayConnection()
        {
            int port = 0;
            if (!int.TryParse(this.textBoxServerPort.Text, out port) || (port < 0))
            {
                Utils.MessageBoxError("端口值错误!");
                return false;
            }
            Program.TestManager.deviceController.TCPClient.Host = this.textBoxServerIP.Text.Trim();
            Program.TestManager.deviceController.TCPClient.Port = port;
            Program.TestManager.deviceController.TCPClient.SaveToFile(StringResource.ConfigFileName);
            return true;
        }

        private Boolean SaveSerialPort()
        {
            int baudrate = 0;
            if (!int.TryParse(this.comboBoxBaudrate.Text, out baudrate))
            {
                Utils.MessageBoxError("波特率错误!");
                return false;
            }
            Program.TestManager.deviceController.serialChannel.Port = this.textBoxComPort.Text;
            Program.TestManager.deviceController.serialChannel.BaudRate = baudrate;
            Program.TestManager.deviceController.serialChannel.SaveToFile(StringResource.ConfigFileName);
            return true;
        
        }
        public Boolean SaveConfig()
        {
            return this.SaveGatewayConnection() & this.SaveSerialPort();
        }

        private void buttonSaveSerialPort_Click(object sender, EventArgs e)
        {
            if (!this.SaveSerialPort())
            {
                Utils.MessageBoxError("数据侦听串口设置保存失败");
            }
        }

        private void buttonSaveGateway_Click(object sender, EventArgs e)
        {
            if (!this.SaveGatewayConnection())
            {
                Utils.MessageBoxError("测试网关设置保存失败");
            }
        }


    }
}
