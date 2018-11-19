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
using JK.Program.Pattern;
using DeviceTest.Manager;

namespace DeviceTest.TestForms
{
    public partial class FormDevicePropertys : Form
    {
        private DeviceItem activeDevice { get; set; }
        public FormDevicePropertys()
        {
            InitializeComponent();
            this.activeDevice = null;
            
        }

        public void UpdateActiveDevice(DeviceItem device)
        {
            this.activeDevice = device;
        }
        public void Update(int notifyEvent, string flag, object result, string message, object sender)
        {
            SubjectObserver.FormInvoke update = new SubjectObserver.FormInvoke(this.UpdateInfo);
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
                                        this.RefreshDevicePropertysListView((DeviceItem)result);
                                    }
                                    break;
                                }
                        }
                        break;
                    }                
            }
        }

        public void RefreshDevicePropertysListView(DeviceItem device)
        {
            this.listViewPropertys.Items.Clear();
            if (device != null)
            {
                this.listViewPropertys.Tag = device;
                foreach (Property property in device.propertys)
                {
                    if (property.operates.Contains(Operate.Event) || property.operates.Contains(Operate.Attribute))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = property.keyName;
                        item.SubItems.Add(property.valueText);                       
                        item.SubItems.Add(property.rangeText);
                        item.SubItems.Add(property.unit);
                        item.Tag = property;
                        this.listViewPropertys.Items.Add(item);
                    }
                
                }
            }
        }

        private void listViewPropertys_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (this.listViewPropertys.SelectedItems.Count == 1)
            {
                Property property = (Property)this.listViewPropertys.SelectedItems[0].Tag;
                Program.TestManager.Notify(DeviceController.REFRESH_DEVICE_EVENT, DeviceOperateEvent.Property.ToString(),property,"");
            }  
        }
    }
}
