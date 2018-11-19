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
    public partial class FormDeviceValues : Form
    {
        private DeviceItem activeDevice { get; set; }
        private Property activeProperty { get; set; }
        public FormDeviceValues()
        {
            InitializeComponent();
            
        }

        public void UpdateActiveDevice(DeviceItem device)
        {
            this.activeDevice = device;
        }

        public void AddNewValueItem(PropertyValue value)
        { 
            ListViewItem item = new ListViewItem();
            item.Text  = value.dateTime.ToString();
            item.SubItems.Add(value.value);
            item.SubItems.Add(value.unit);
            this.listViewDeviceValues.Items.Add(item);
        }
        public void RefreshDeviceValuesListView(Property property)
        {
            this.listViewDeviceValues.Items.Clear();
            this.listViewDeviceValues.Columns[1].Text ="--";
            if (property != null)
            {         
                this.listViewDeviceValues.Columns[1].Text = property.keyName;
                foreach (PropertyValue value in property.values)
                {
                    this.AddNewValueItem(value);
                }
            }
        }



        public void RefreshDeviceValues(Property property)
        {
            RefreshDeviceValuesListView(property);        
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
                                   case DeviceOperateEvent.Property:
                                       {
                                           this.activeProperty = (Property)result;
                                           this.RefreshDeviceValues((Property)result);
                                           break;
                                       }
                                   case DeviceOperateEvent.ListUpdate:
                                       {
                                           if (this.activeDevice != null && ((DeviceItem)result).deviceId == this.activeDevice.deviceId)
                                           {
                                               this.RefreshDeviceValues(this.activeProperty);
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
