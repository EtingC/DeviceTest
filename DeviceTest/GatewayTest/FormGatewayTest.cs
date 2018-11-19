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
using DeviceTest.Center;
using JK.Channels.Base;
using JK.Libs.Utils;

namespace DeviceTest.GatewayTest
{
    public partial class FormGatewayTest : Form
    {
        public FormGatewayTest()
        {
            InitializeComponent();
        }


        private void FormGatewayTest_Load(object sender, EventArgs e)
        {
            Program.TestGatewayController.AttachObserver(this.Update);
        }

        private void RefreshGatewayListview()
        {
            this.listViewGateway.Items.Clear();
            foreach (Gateway gateway in Program.TestGatewayController.gatewayItems)
            {
                ListViewItem newItem = new ListViewItem();
                newItem.Text = gateway.deviceId;
                newItem.SubItems.Add(Program.TestGatewayController.GetGatewayType(gateway.deviceType));
                newItem.SubItems.Add(gateway.softwareVersion);
                newItem.SubItems.Add(gateway.resultStatus);
                newItem.SubItems.Add(gateway.model);
                newItem.SubItems.Add(gateway.address);
                newItem.SubItems.Add(gateway.port.ToString());
                newItem.Tag = gateway;
                this.listViewGateway.Items.Add(newItem);
            }       
        }


        private void RefreshGatewayByAddress(Gateway gateway)
        {
            foreach (ListViewItem item in this.listViewGateway.Items)
            {
                if (item.Tag.Equals(gateway))
                {
                    item.SubItems[1].Text = Program.TestGatewayController.GetGatewayType(gateway.deviceType) +"["+ Program.TestGatewayController.GetGatewayModel(gateway.deviceType)+"]";
                    item.SubItems[2].Text = gateway.softwareVersion;
                    item.SubItems[3].Text = gateway.resultStatus;
                    item.SubItems[4].Text = gateway.model;
                    item.SubItems[5].Text = gateway.address;
                    item.SubItems[6].Text = gateway.port.ToString();
                    break;
                }
            }              
        }

        private void RefreshGatewayTypeListCombox()
        {
            this.comboBoxGatewayTypeList.Items.Clear();
            foreach (GatewayNode node in Program.TestGatewayController.gatewayNodes.rows)
            {
                this.comboBoxGatewayTypeList.Items.Add(node.name);
            }
            if (this.comboBoxGatewayTypeList.Items.Count > 0)
            {
                this.comboBoxGatewayTypeList.SelectedIndex = 0; 
            }
            
        }
        private void toolStripTextBoxAddress_Click(object sender, EventArgs e)
        {
            this.toolStripTextBoxAddress.Text = "";
        }


        private void AddNewGatewayItem()
        {
            string address = this.toolStripTextBoxAddress.Text.Trim();
            if (address.Length > 0)
            {
                foreach (ListViewItem item in this.listViewGateway.Items)
                {
                    if (item.Text.Contains(address))
                    {
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                ListViewItem newItem = new ListViewItem();
                newItem.Text = address;
                this.listViewGateway.Items.Add(newItem);
            }
            this.toolStripTextBoxAddress.Clear();
        }

        private int GetActiveGatewayTypeIndex()
        {
            if (this.comboBoxGatewayTypeList.SelectedIndex > 0)
            {
                return Program.TestGatewayController.gatewayNodes.rows[this.comboBoxGatewayTypeList.SelectedIndex].index;
            }
            return -1;
        }

        private void toolStripTextBoxAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.AddNewGatewayItem();
            }
        }

        private void toolStripButtonConfirm_Click(object sender, EventArgs e)
        {
            this.AddNewGatewayItem();
        }

        private void FormGatewayTest_Activated(object sender, EventArgs e)
        {

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
                case GatewayController.GATEWAY_CONTROLLER_EVENT:
                    {
                        HttpRequestType httpRequestType = (HttpRequestType)Enum.Parse(typeof(HttpRequestType), flag);
                        switch (httpRequestType)
                        {
                            case HttpRequestType.RefreshGatewayNodes:                         
                                {
                                    this.RefreshGatewayTypeListCombox();
                                    break;
                                }
                            case HttpRequestType.PostGatewayBindData:
                                {
                                    this.RefreshGatewayListview();
                                    break;
                                }
                        }
                        break;
                    }
                case GatewayController.GATEWAY_CONTROLLER_SCAN_EVENT:
                    {
                        ScanOperate operate = (ScanOperate)Enum.Parse(typeof(ScanOperate), flag);
                        switch (operate)
                        {
                            case ScanOperate.ScanResponse:
                                {
                                    Gateway gateway = (Gateway)result;
                                    if (gateway != null)
                                    {
                                        this.RefreshGatewayByAddress(gateway);
                                    }
                                    break;
                                }
                        }

                        break;                 
                    }
            }

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Program.TestGatewayController.RefreshGatewayNodes();
        }

        private void buttonBind_Click(object sender, EventArgs e)
        {
            if((this.listViewGateway.Items.Count>0) && (this.comboBoxGatewayTypeList.Items.Count>0))
            {
                if (Utils.MessageBoxQuestion("确定把列表中的标识码绑定成[" + this.comboBoxGatewayTypeList.Text + "]？"))
                { 
                    Program.TestGatewayController.gatewayItems.Clear();
                    foreach (ListViewItem item in this.listViewGateway.Items)
                    {
                        Program.TestGatewayController.AppendGatewayItem(item.Text);
                    }
                    int type = this.GetActiveGatewayTypeIndex();
                    Program.TestGatewayController.PostGatewayBind(type.ToString());
                }
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item in this.listViewGateway.SelectedItems)
            {
                listViewGateway.Items.Remove(item);
                Program.TestGatewayController.Remove(item.Text);
            }
        }

        private void toolStripButtonScan_Click(object sender, EventArgs e)
        {
            if (this.toolStripButtonScan.Checked)
            {
                Program.TestGatewayController.StopScan();
                this.toolStripButtonScan.Text = "启动本地扫描";
            }
            else
            {
                Program.TestGatewayController.StartScan();
                this.toolStripButtonScan.Text = "停止本地扫描";
            }
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            this.listViewGateway.Items.Clear();
            Program.TestGatewayController.gatewayItems.Clear();
        }
    }
}
