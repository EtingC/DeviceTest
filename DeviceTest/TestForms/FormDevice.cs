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
using DeviceTest.Manager;
using JK.Program.Pattern;

namespace DeviceTest.TestForms
{
    public partial class FormDevice : Form
    {
        private DeviceItem activeDevice { get; set; }
        private FormDeviceBaseInfo formDeviceBaseInfo { get;set; }
        private FormDevicePropertys formDevicePropertys { get; set; }
        private FormDeviceValues formDeviceValues { get; set; }
        private Subject subject { get;set;}
        public FormDevice()
        {
            InitializeComponent();
            this.formDeviceBaseInfo = new FormDeviceBaseInfo();
            this.formDevicePropertys = new FormDevicePropertys();
            this.formDeviceValues = new FormDeviceValues();

            Utils.AddFormToContainer(this.formDeviceBaseInfo, this.panelDeviceBase);
            Program.TestManager.AttachObserver(this.formDeviceBaseInfo.Update);
            Utils.AddFormToContainer(this.formDevicePropertys, this.panelDevicePropertys);
            Program.TestManager.AttachObserver(this.formDevicePropertys.Update);
            Utils.AddFormToContainer(this.formDeviceValues, this.groupBoxDeviceValues);
            Program.TestManager.AttachObserver(this.formDeviceValues.Update);
        }

        public void AttachSubject(Subject subject)
        {
            this.subject = subject;
            subject.AttachObserver(this.formDeviceBaseInfo.Update);
            subject.AttachObserver(this.formDevicePropertys.Update);
            subject.AttachObserver(this.formDeviceValues.Update);           
        }
        public void UpdateActiveDevice(DeviceItem device)
        {
            this.activeDevice = device;
            this.formDeviceBaseInfo.UpdateActiveDevice(device);
            this.formDeviceBaseInfo.RefreshDeviceBaseInforListView(device);
            this.formDevicePropertys.UpdateActiveDevice(device);
            this.formDevicePropertys.RefreshDevicePropertysListView(device);
            this.formDeviceValues.UpdateActiveDevice(device);
            this.formDeviceValues.RefreshDeviceValues(null);
        }

        private void FormDevice_Load(object sender, EventArgs e)
        {

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButtonQuery_Click(object sender, EventArgs e)
        {
            if (this.activeDevice != null)
            {
                Program.TestManager.deviceController.GetDeviceAllAttribute(this.activeDevice.deviceId);
            }
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            Program.TestManager.Notify(DeviceController.REFRESH_DEVICE_EVENT, DeviceOperateEvent.Refresh.ToString(), this.activeDevice, "");
        }

        private void FormDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.subject!=null)
            { 
                this.subject.DetachObserver(this.formDeviceBaseInfo.Update);
                this.subject.DetachObserver(this.formDevicePropertys.Update);
                this.subject.DetachObserver(this.formDeviceValues.Update);
            }
        }
    }
}
