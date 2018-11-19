using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeviceTest.ConfigForms;

namespace DeviceTest.ConfigForms
{
    public enum ControllerIndex 
    { 
        System      = 0,
        Device      = 1,
        Print       = 2,
        
    }
    public partial class FormConfig : Form
    {
        private FormSystemConfig formSystemConfig;
        private FormDeviceControllerConfig formDeviceControllerConfig;
        private FormPrintConfig formPrintConfig;
        /*private FormInputConfig formInputConfig;
        private FormOutputConfig formOutputConfig;
        private FormModbusConfig formModbusConfig;*/
        public FormConfig()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            Boolean result = false;
            if (lv_controller.FocusedItem != null)
            {
                switch ((ControllerIndex)lv_controller.FocusedItem.Index)
                {
                    case ControllerIndex.System:
                        result = this.formSystemConfig.SaveConfig();
                        break;
                    case ControllerIndex.Device:
                        result = this.formDeviceControllerConfig.SaveConfig();
                        break;
                   case ControllerIndex.Print:
                        result = this.formPrintConfig.SaveConfig();
                        break;
                   /*  case ControllerIndex.Sample:
                        result = this.formSampleConfig.SaveConfig();
                        break;
                    case ControllerIndex.Service:
                        result = this.formDetectService.SaveConfig();
                        break;
                    case ControllerIndex.Modbus:
                        result = this.formModbusConfig.SaveConfig();
                        break;*/
                    default:
                        break;
                }
            }
            if (result)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        private void ShowConfigForm(Form configForm, Control parentControl)
        {
            parentControl.Controls.Clear();
            configForm.MdiParent = this;
            configForm.WindowState = FormWindowState.Maximized;
            configForm.FormBorderStyle = FormBorderStyle.None;
            configForm.Parent = this.pnl_formContainer;
            configForm.Show();
        }

        private void FormConfig_Shown(object sender, EventArgs e)
        {
            this.formSystemConfig = new FormSystemConfig();
            this.formDeviceControllerConfig = new FormDeviceControllerConfig();
            this.formPrintConfig = new FormPrintConfig();
            /* this.formInputConfig = new FormInputConfig();
            this.formOutputConfig = new FormOutputConfig();
            this.formModbusConfig = new FormModbusConfig();*/
            this.ShowConfigForm(this.formSystemConfig, pnl_formContainer);
        }

        private void lv_controller_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv_controller.FocusedItem != null)
            {
                switch ((ControllerIndex)lv_controller.FocusedItem.Index)
                {
                    case ControllerIndex.System:
                        this.ShowConfigForm(this.formSystemConfig, pnl_formContainer);
                        break;
                    case ControllerIndex.Device:
                        this.ShowConfigForm(this.formDeviceControllerConfig, pnl_formContainer);
                        break;
                    case ControllerIndex.Print:
                        this.ShowConfigForm(this.formPrintConfig, pnl_formContainer);
                        break;
                   /* case ControllerIndex.Input:
                        this.ShowConfigForm(this.formInputConfig, pnl_formContainer);
                        break;
                    case ControllerIndex.Output:
                        this.ShowConfigForm(this.formOutputConfig, pnl_formContainer);
                        break;
                    case ControllerIndex.Modbus:
                        this.ShowConfigForm(this.formModbusConfig, pnl_formContainer);
                        break;*/
                    default:
                        this.ShowConfigForm(this.formSystemConfig, pnl_formContainer);
                        break;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
