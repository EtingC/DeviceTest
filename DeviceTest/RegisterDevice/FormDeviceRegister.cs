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
using JK.Libs.Utils;
using DeviceTest.Projects;
using DeviceTest.TestForms;

namespace DeviceTest.RegisterDevice
{
    public partial class FormDeviceRegister : Form
    {
        public FormDevicePropertys formDevicePropertys { get; set; }
        public FormDevice formDevice { get; set; }
        public FormDeviceRegister()
        {
            InitializeComponent();
            this.formDevicePropertys = new FormDevicePropertys();
            Utils.AddFormToContainer(this.formDevicePropertys,this. panelPropertys);
            Program.TestManager.deviceController.AttachObserver(this.formDevicePropertys.Update);

            this.formDevice = new FormDevice();
            this.formDevice.AttachSubject(Program.TestManager.deviceController);
            this.formDevice.UpdateActiveDevice(Program.TestManager.deviceController.iLopController.activeDevice);
        }

        private void AddListviewItem(ListView listView, string name)
        {
            ListViewItem item = new ListViewItem();
            item.Text = "" ;
            item.SubItems.Add(name);
            item.SubItems.Add("");
            item.ImageIndex = 0;
            listView.Items.Add(item);
        }

        private void InitializeListView()
        {
            this.listViewData.Items.Clear();
            AddListviewItem(this.listViewData, "Device Name");
            AddListviewItem(this.listViewData, "Product Key");           
            AddListviewItem(this.listViewData,"Secret");
            AddListviewItem(this.listViewData, "消息");
        }
        private void FormDeviceRegister_Load(object sender, EventArgs e)
        {
            this.InitializeListView();
            Program.TestManager.deviceController.AttachObserver(this.Update);
            this.formDevicePropertys.UpdateActiveDevice(Program.TestManager.deviceController.iLopController.activeDevice);
            this.formDevicePropertys.RefreshDevicePropertysListView(Program.TestManager.deviceController.iLopController.activeDevice);
        }

        private void AddNewDevice()
        {
            if (toolStripTextBoxProductKey.Text != "")
            {
                string productKey = toolStripTextBoxProductKey.Text;
                if (productKey.Contains("https://"))
                {
                    string[] list = productKey.Split('&');
                    foreach (string value in list)
                    {
                        if (value.Contains("sn="))
                        {
                            productKey = value.Substring(3);
                        }
                    }
                }

                Program.TestManager.deviceController.QueryRegistedDeviceData(productKey);
                this.toolStripTextBoxProductKey.Clear();
            }            
        }
        private void RefreshRegisterInfoViewer()
        {
            string message = "查询[" + Program.TestManager.deviceController.iLopController.productKey+ "]三元组信息";
            if (Program.TestManager.deviceController.iLopController.validProductKey)
            {
                this.listViewData.Items[0].SubItems[2].Text = Program.TestManager.deviceController.iLopController.deviceCodeItem.deviceName;
                this.listViewData.Items[1].SubItems[2].Text = Program.TestManager.deviceController.iLopController.deviceCodeItem.deviceId;
                this.listViewData.Items[2].SubItems[2].Text = Program.TestManager.deviceController.iLopController.deviceCodeItem.secret;
                this.listViewData.Items[3].SubItems[2].Text = message + "成功";
            }
            else
            {
                this.listViewData.Items[3].SubItems[2].Text = message + "失败";
            }
        }
        private void toolStripButtonConfirm_Click(object sender, EventArgs e)
        {
            this.AddNewDevice();
        }

        private void toolStripTextBoxProductKey_Click(object sender, EventArgs e)
        {
            this.toolStripTextBoxProductKey.Clear();
        }

        private void toolStripTextBoxProductKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.AddNewDevice();
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

        private void UpdateRegisterInfoViewer()
        { 

            foreach(ListViewItem item in this.listViewData.Items)
            {
                if (item.Index == 3)
                {
                    return;
                }
                if (Program.TestManager.deviceController.iLopController.KeyEquals(item.Index))
                {
                    item.ImageIndex = 1;
                }
                else
                {
                    item.ImageIndex = 2;
                }

            }
         
        }
        private void ShowStatus(int Event, string flag, object result, string message, object sender)
        {
            switch (Event)
            {
                case (int)HttpRequestType.QueryRegistedDeviceData:
                    {
                        this.RefreshRegisterInfoViewer();
                        break;
                    }
                case DeviceController.REFRESH_DEVICE_EVENT:
                    {
                        this.UpdateRegisterInfoViewer();
                        if (message.Length > 0)
                        {
                            this.richTextBoxMessage.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + message + "\n");
                        }
                        break;
                    }
                
            }
        }

        private void toolStripButtonSet_Click(object sender, EventArgs e)
        {
            if (Program.TestManager.deviceController.iLopController.validProductKey)
            {
                Program.TestManager.deviceController.iLopController.WriteProductKeys();      
            }
        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButtonQuery_Click(object sender, EventArgs e)
        {
            Program.TestManager.deviceController.iLopController.ReadProductKeys();
            Program.TestManager.deviceController.iLopController.ReadConnectStatus();
            Program.TestManager.deviceController.iLopController.ReadDeviceStatus();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.formDevice.UpdateActiveDevice(Program.TestManager.deviceController.iLopController.activeDevice);
            this.formDevice.ShowDialog();
        }

        private void toolStripButtonAutoQuery_Click(object sender, EventArgs e)
        {
            if (this.timer.Enabled)
            {
                this.timer.Enabled = false;
                this.toolStripButtonAutoQuery.Text = "自动";
            }
            else
            {
                this.timer.Enabled = true;
                this.toolStripButtonAutoQuery.Text = "停止";          
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Program.TestManager.deviceController.iLopController.ReadDeviceStatus();
        }

    }
}
