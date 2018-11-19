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
using DeviceTest.Base;
using System.IO;
using DeviceTest.Manager;
using Newtonsoft.Json;


namespace DeviceTest.TestForms
{
    public partial class FormImportDevice : Form
    {
        private GenerateResult label { get; set; }
        public FormImportDevice()
        {
            InitializeComponent();
            this.label = new GenerateResult();
        }

        private void PrintLabel()
        {
            string fileUrl = this.textBoxLabelPath.Text;
            if (fileUrl == "")
            {
                Utils.MessageBoxError("请选择标签模板文件！");
                return;
            }
            if (!File.Exists(fileUrl))
            {
                Utils.MessageBoxError("标签模板文件不存在，请重新选择！");
                return;                
            }

            string value = this.textBoxImportText.Text;
            if (value == "")
            {
                return;
            }
            value = value.Trim();

            this.label.FileURL = fileUrl;

            this.label.Items.Clear();

            KeyValueItem item =new KeyValueItem("SN",value);
            this.label.Items.Add(item);
            item = new KeyValueItem("MAC", value);
            this.label.Items.Add(item);

            this.label.ErrorCode = 0;
            string labelContext = JsonConvert.SerializeObject(this.label);

            Program.TestManager.labelController.PrintLabelContext(labelContext);
            this.textBoxImportText.Clear();
            Program.TestManager.labelController.SavePrintLogFile(value);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
              this.PrintLabel();
        }

        private void AddNewDevice()
        {
            DeviceItem device =  Program.TestManager.deviceController.AddNewDeviceByImportText(this.textBoxImportText.Text);
            this.textBoxImportText.Clear();
            if (device!=null  &&  this.checkBoxAutoPrintLabel.Checked)
            {
                Program.TestManager.deviceController.PrintDeviceLabel(device);
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.checkBoxAutoPrintLabel.Checked)
                {
                    this.PrintLabel();
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxImportText_MouseClick(object sender, MouseEventArgs e)
        {
            this.textBoxImportText.Clear();
        }

        private void checkBoxAutoPrintLabel_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonSelectLabel_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            { 
                this.textBoxLabelPath.Text = this.openFileDialog.FileName;
                
            
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.textBoxImportText.Clear();
        }
    }
}
