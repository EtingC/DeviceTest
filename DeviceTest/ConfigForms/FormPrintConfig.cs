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
    public partial class FormPrintConfig : Form
    {
        public FormPrintConfig()
        {
            InitializeComponent();
        }

        private void RefreshViewer()
        {
            this.textBoxServerIP.Text = Program.TestManager.labelController.UDPClient.Host;
            this.textBoxServerPort.Text = Program.TestManager.labelController.UDPClient.Port.ToString();
        }

        public Boolean SaveConfig()
        {
            int port = 0;
            if (!int.TryParse(this.textBoxServerPort.Text, out port) || (port < 0))
            {
                Utils.MessageBoxError("端口值错误!");
                return false;
            }
            Program.TestManager.labelController.UDPClient.Host = this.textBoxServerIP.Text.Trim();
            Program.TestManager.labelController.UDPClient.Port = port;
            Program.TestManager.labelController.UDPClient.SaveToFile(StringResource.ConfigFileName);
            return true;
        }

        private void FormPrintConfig_Load(object sender, EventArgs e)
        {
            this.RefreshViewer();
        }
    }
}
