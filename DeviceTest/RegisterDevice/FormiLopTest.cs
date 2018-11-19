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


namespace DeviceTest.RegisterDevice
{
    public partial class FormiLopTest : Form
    {
        public FormDeviceRegister formDeviceRegister { get; set; }
        public FormiLopTest()
        {
            InitializeComponent();
            this.formDeviceRegister = new FormDeviceRegister();
        }

        private void FormiLopTest_Load(object sender, EventArgs e)
        {
            Utils.AddFormToContainer(this.formDeviceRegister, this.tabPageGPRS);
        }
    }
}
