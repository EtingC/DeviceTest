using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceTest.ConfigForms
{
    public partial class FormSystemConfig : Form
    {
        public FormSystemConfig()
        {
            InitializeComponent();
        }
        private void RefreshViewer()
        {
            this.textBoxServerIP.Text = Program.TestManager.httpController.baseUrl;
        }
        public Boolean SaveConfig()
        {
            Program.TestManager.httpController.baseUrl = this.textBoxServerIP.Text;
            Program.TestManager.SaveToFile();
            return true;
        }

        private void FormSystemConfig_Load(object sender, EventArgs e)
        {
            this.RefreshViewer();
        }
    }
}
