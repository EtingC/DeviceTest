using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JK.Program.Pattern;
using DeviceTest.Manager;
using DeviceTest.Base;
using JK.Libs.Utils;

namespace DeviceTest.ConfigDevice
{
    public partial class FormDeviceSNDetail : Form
    {
  
        public DeviceNode deviceNode;
        public FormDeviceSNDetail()
        {
            InitializeComponent();
        }


        public void updateQueryForm(DeviceNode deviceNode)
        {
            this.deviceNode = deviceNode;
            this.dateTimePickerStartDT.Value = DateTime.Now;
            this.dateTimePickerEndDT.Value = DateTime.Now;
            this.textBoxDeviceInfo.Text = this.deviceNode.name;
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

        private void ShowStatus(int Event, string flag, object result, string message, object sender)
        {
            switch (Event)
            {
                case (int)HttpRequestType.QuerySerialNumberListData:
                    {
                        this.RefreshSerialNumberViewer();
                        break;
                    }
            }
        }


        private void RefreshSerialNumberViewer()
        {
            try
            {
                this.listViewDeviceCodes.BeginUpdate();
                this.listViewDeviceCodes.Items.Clear();
                foreach (DeviceCodeItem code in Program.TestManager.deviceController.deviceCodeList.rows)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = (this.listViewDeviceCodes.Items.Count + 1).ToString();
                    item.SubItems.Add(code.remark);
                    item.SubItems.Add(code.deviceName);
                    item.SubItems.Add(code.deviceId);
                    item.SubItems.Add(code.identity);
                    item.SubItems.Add(code.secret);
                    item.SubItems.Add(code.activeText);
                    this.listViewDeviceCodes.Items.Add(item);
                }
                groupBoxDetail.Text = "SN信息列表(" + this.listViewDeviceCodes.Items.Count.ToString()+")";
            }
            finally
            {
                this.listViewDeviceCodes.EndUpdate();               
            }
           

        }

        private void FormDeviceSNDetail_Load(object sender, EventArgs e)
        {
            Program.TestManager.deviceController.AttachObserver(this.Update);
        }

        private void FormDeviceSNDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.TestManager.deviceController.DetachObserver(this.Update);
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            this.QueryOperate();
        }

        private void QueryOperate()
        {
            string startDT = this.dateTimePickerStartDT.Value.Date.ToString("yyyy-M-d");
            string endDT = this.dateTimePickerEndDT.Value.Date.ToString("yyyy-M-d");
            Program.TestManager.deviceController.QueryDeviceCodeList(this.deviceNode.productKey, true, startDT, endDT);        
        
        }

        private void FormDeviceSNDetail_Shown(object sender, EventArgs e)
        {
            this.QueryOperate();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (Program.TestManager.deviceController.deviceCodeList.total > 0)
            {
                string fileName = this.deviceNode.serialCode + "-(" + this.deviceNode.productKey + ")-"+ Program.TestManager.deviceController.deviceCodeList.total + ".txt";
                saveFileDialog.FileName = fileName;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Program.TestManager.deviceController.deviceCodeList.ExportToFile(saveFileDialog.FileName);
                    Utils.MessageBoxInformation("导出白名单成功！");
                }
            }
        }

    }
}
