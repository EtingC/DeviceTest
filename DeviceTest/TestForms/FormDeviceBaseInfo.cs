using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeviceTest.Base;
using DeviceTest.Manager;
using JK.Program.Pattern;

namespace DeviceTest.TestForms
{
    public partial class FormDeviceBaseInfo : Form
    {
        private DeviceItem activeDevice{get;set;}
        public FormDeviceBaseInfo()
        {
            InitializeComponent();
            this.activeDevice = null;
            this.InitializeViewer();
            
        }

        public void UpdateActiveDevice(DeviceItem device)
        {
            this.activeDevice = device;
        }
        private void AddNewItem(string name, string value)
        {
            ListViewItem item = new ListViewItem();
            item.Text = name;
            item.SubItems.Add(value);
            this.listViewDevice.Items.Add(item);
        }

        private ListViewGroup AddNewGroup(string name)
        {
            ListViewGroup group = new ListViewGroup();
            group.Header = name;
            this.listViewDevice.Groups.Add(group);
            return group;
        }

        public void RefreshDeviceBaseInforListView(DeviceItem device)
        {
            this.listViewDevice.Items[0].SubItems[1].Text = device.model;
            this.listViewDevice.Items[1].SubItems[1].Text = device.name;
            this.listViewDevice.Items[2].SubItems[1].Text = device.modelId;
            this.listViewDevice.Items[3].SubItems[1].Text = device.deviceId;
            this.listViewDevice.Items[4].SubItems[1].Text = device.hardwareVersion;
            this.listViewDevice.Items[5].SubItems[1].Text = device.softwareVersion;
            this.listViewDevice.Items[6].SubItems[1].Text = device.mcuBinVersion;
            this.listViewDevice.Items[7].SubItems[1].Text = device.onlineText;
            this.listViewDevice.Items[8].SubItems[1].Text = device.registedText;
            this.listViewDevice.Items[9].SubItems[1].Text = DateTime.Now.ToString();
        }


        private void InitializeViewer()
        {
            this.listViewDevice.Items.Clear();
            this.AddNewItem("型号","");
            this.AddNewItem("名称", "");
            this.AddNewItem("ModelId","");
            this.AddNewItem("MAC","");
            this.AddNewItem("硬件版本","");
            this.AddNewItem("软件版本","");
            this.AddNewItem("MCU固件版本","");
            this.AddNewItem("在线","");
            this.AddNewItem("注册","");
            this.AddNewItem("最后刷新时间","");

            ListViewGroup group = this.AddNewGroup("基本");
            for (int i = 0; i < 7; i++)
            {
                this.listViewDevice.Items[i].Group = group;
            }
            group =  this.AddNewGroup("状态");
            for (int i = 7; i < 10; i++)
            {
                this.listViewDevice.Items[i].Group = group;
            }
        }

        private void FormDeviceBaseInfo_Load(object sender, EventArgs e)
        {
            
        }


        public void Update(int notifyEvent, string flag, object result, string message, object sender)
        {
            SubjectObserver.FormInvoke update = new SubjectObserver.FormInvoke(this.UpdateInfo);
            try
            {
                this.Invoke(update, notifyEvent, flag, result, message, sender);
            }
            catch (System.InvalidOperationException )
            {
            }
            catch (System.ComponentModel.InvalidAsynchronousStateException)
            {

            }
        }

        private void UpdateInfo(int Event, string flag, object result, string message, object sender)
        {
            switch (Event)
            {
                case DeviceController.REFRESH_DEVICE_EVENT:
                    {
                        DeviceOperateEvent deviceOperate = (DeviceOperateEvent)Enum.Parse(typeof(DeviceOperateEvent), flag);
                        switch (deviceOperate)
                        {
                            case DeviceOperateEvent.ListUpdate:
                            case DeviceOperateEvent.Refresh:
                                {
                                    if (this.activeDevice != null && ((DeviceItem)result).deviceId == this.activeDevice.deviceId)
                                    {
                                        this.RefreshDeviceBaseInforListView((DeviceItem)result);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
            }
        }

    }
}
