using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DeviceTest.Base;
using JK.Program.Pattern;
using JK.Channels.Base;
using JK.Channels.TCP;
using Newtonsoft.Json;
using DeviceTest.Manager;
using JK.Libs.Utils;
using DeviceTest.ConfigDevice;



namespace DeviceTest.TestForms
{
    public partial class FormDeviceTest : Form
    {
        private FormTestStatus formTestStatus { get; set; }
        private FormDevice formDevice { get; set; }
        private FormImportDevice formImportDevice { get; set; }
        private List<DeviceNode> projectNodeList { get; set; }
        public FormDeviceTest()
        {
            InitializeComponent();
            this.formTestStatus = new FormTestStatus();
            this.formDevice = new FormDevice();
            this.formImportDevice = new FormImportDevice();
            this.formDevice.AttachSubject(Program.TestManager);
            Utils.AddFormToContainer(this.formTestStatus, this.groupBoxTestStatus);
            this.projectNodeList = new List<DeviceNode>();
        }

        private void FormDeviceTest_Load(object sender, EventArgs e)
        {
            this.InitializeGatewayViewer();
            this.InitilizeBaseCombox();
            Program.TestManager.AttachObserver(this.Update);

            Program.TestManager.deviceController.GetGatewayAllAttribute();
            Program.TestManager.deviceController.GetDeviceList();

        }

        private void AddProjectMenuItem(Project project)
        {
            System.Windows.Forms.ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = project.name;
            item.Tag = project;
            item.Click += new System.EventHandler(this.ToolStripMenuItemProjectItem_Click);
            this.toolStripSplitButtonProject.DropDownItems.Add(item);       
        }

        private void ToolStripMenuItemProjectItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item!=null)
            {
                Project project = (Project)item.Tag;
                string projectFolder = StringResource.MainDataPath + project.name + "\\";
                if (Directory.Exists(projectFolder))
                {
                    System.Diagnostics.Process.Start("explorer.exe", projectFolder);
                }
            }
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

        private void ProcessTestMessage(TestType testType,TestOperate operate,string message)
        {
            switch (operate)
            {
                case TestOperate.Stop:
                    {
                        switch (testType)
                        {
                            case TestType.Auto:
                                {
                                    this.buttonAutoTest.Text = "开始自动测试";
                                    this.buttonManualTest.Enabled = true;
                                    break;
                                }
                            case TestType.Manual:
                                {
                                    this.buttonManualTest.Text = "开始手动测试";
                                    this.buttonAutoTest.Enabled = true;                                  
                                    break;
                                }
                        }                       
                        Program.TestManager.AddDeviceTestResultRecord();
                        break;

                    }
                case TestOperate.Start:
                    {
                        switch (testType)
                        {
                            case TestType.Auto:
                                {
                                    this.buttonAutoTest.Text = "停止自动测试";
                                    this.buttonManualTest.Enabled = false;
                                    break;
                                }
                            case TestType.Manual:
                                {
                                    this.buttonManualTest.Text = "停止手动测试";
                                    this.buttonAutoTest.Enabled = false;
                                    break;
                                }
                        }
                        break;
                    }
            }
        }

        private void UpdateInfo(int Event, string flag, object result, string message, object sender)
        {

            switch (Event)
            {
                case DeviceController.REFRESH_DEVICE_LIST_EVENT:
                    {
                        DeviceOperateEvent operate = (DeviceOperateEvent)Enum.Parse(typeof(DeviceOperateEvent), flag);
                        switch (operate)
                        {
                            case DeviceOperateEvent.ListAll:
                            case DeviceOperateEvent.ListAdd:
                            case DeviceOperateEvent.ListDelete:
                                {
                                    this.RefreshDeviceList();
                                    break;
                                }                         
                        }
                       break;
                    }
                case DeviceController.REFRESH_DEVICE_EVENT:
                    {
                        DeviceOperateEvent operate = (DeviceOperateEvent)Enum.Parse(typeof(DeviceOperateEvent), flag);
                        switch (operate)
                        {
                            case DeviceOperateEvent.ListAdd:
                                {
                                    this.AddListviewItem((DeviceItem)result);
                                    break;
                                }
                            case DeviceOperateEvent.ListUpdate:
                                {
                                    this.UpdateListviewItem((DeviceItem)result);
                                    this.RefreshlistViewRealDevicesSelectedItem();
                                    break;
                                }
                            case DeviceOperateEvent.ListDelete:
                                {
                                    this.RefreshDeviceList();
                                    break;
                                }


                        }
                        break;
                    }
                case DeviceController.REFRESH_GATEWAY_EVENT:
                    {
                        this.RefreshGatewayData();
                        break;
                    }
                case (int)HttpRequestType.RefreshDeviceNodes:
                    {
                        this.RefreshDeviceNodesCombox();
                        break;
                    }
                case TestController.TEST_CONTROLLER_MESSAGE_EVENT:
                    {
                        TestOperate step = (TestOperate)Enum.Parse(typeof(TestOperate), flag);
                        this.ProcessTestMessage((TestType)result, step, message);   
                        break; 
                    }
            }
        }

        private void UpdateListviewItem(DeviceItem device)
        {
            foreach (ListViewItem item in this.listViewRealDevices.Items)
            {
                if (item.SubItems[2].Text == device.deviceId)
                {
                    item.SubItems[1].Text = device.status;
                    item.SubItems[3].Text = device.onlineText;
                    item.SubItems[4].Text = device.registedText;
                    item.SubItems[5].Text = device.model;
                    item.SubItems[6].Text = device.name;
                    item.SubItems[7].Text = device.modelId;
                    item.SubItems[8].Text = device.softwareVersion;
                    item.SubItems[10].Text = device.printlabelSum.ToString();
                    item.SubItems[11].Text = DateTime.Now.ToString();
                    break;
                }
            }       
        }




        private void AddListviewItem(DeviceItem device)
        {
            ListViewItem item = new ListViewItem();
            item.Text = (this.listViewRealDevices.Items.Count + 1).ToString();
            item.SubItems.Add(device.status);
            item.SubItems.Add(device.deviceId);
            item.SubItems.Add(device.onlineText);
            item.SubItems.Add(device.registedText);
            item.SubItems.Add(device.model);
            item.SubItems.Add(device.name);
            item.SubItems.Add(device.modelId);
            item.SubItems.Add(device.softwareVersion);
            item.SubItems.Add(device.lastSoftwareVersion);
            item.SubItems.Add(device.printlabelSum.ToString());
            item.SubItems.Add(DateTime.Now.ToString());
            item.Tag = device;
            this.listViewRealDevices.Items.Add(item);
            //FIXME暂时先放这里
            Program.TestManager.deviceController.SaveHistoryDevicesItemsWithProject();
        }


        private void RefreshDeviceList()
        {
            this.listViewRealDevices.Items.Clear();
            foreach (DeviceItem device in Program.TestManager.deviceController.deviceList)
            {
                if (!device.deleted)
                {
                    this.AddListviewItem(device);
                }
            }            
        }

        private void RefreshDeviceNodesCombox()
        {
            this.comboBoxDeviceList.Items.Clear();
            this.projectNodeList.Clear();
            foreach (DeviceNode node in Program.TestManager.deviceController.deviceNodes.rows)
            {
                if (node.flag == Program.TestManager.projectService.activeProject.serialCode)
                {
                    string value = node.serialCode + "[" + node.name + "]";
                    this.projectNodeList.Add(node);
                    this.comboBoxDeviceList.Items.Add(value);                   
                }
            }
            this.comboBoxDeviceList.SelectedIndex = -1;
        }

        private void InitilizeBaseCombox()
        {
            foreach (Project project in Program.TestManager.projectService.projects)
            {
                this.comboBoxProjects.Items.Add(project.name);
                this.AddProjectMenuItem(project);              
            }
            
            if (this.comboBoxProjects.Items.Count > 0)
            {
                if (Program.TestManager.projectService.activeProjectIndex < this.comboBoxProjects.Items.Count)
                {
                    this.comboBoxProjects.SelectedIndex = Program.TestManager.projectService.activeProjectIndex;
                }
                else
                {
                    this.comboBoxProjects.SelectedIndex = 0;
                }
            }
            this.comboBoxProjects.SelectedIndexChanged += new System.EventHandler(this.comboBoxProjects_SelectedIndexChanged);

            if (Program.TestManager.projectService.activeProject != null)
            {
                this.groupBoxSelectDeviceType.Enabled = !(Program.TestManager.projectService.activeProject.serialCode == "None");
            }


            comboBoxDeviceColors.Items.Add("白色");
            comboBoxDeviceColors.Items.Add("金色");
            if (Program.TestManager.projectService.deviceColor - 1 < comboBoxDeviceColors.Items.Count)
            {
                comboBoxDeviceColors.SelectedIndex = Program.TestManager.projectService.deviceColor - 1;
            }
            else
            {
                comboBoxDeviceColors.SelectedIndex = 0;
            }
        }

        private void InitializeGatewayViewer()
        {
            ListViewItem item = new ListViewItem();
            item.Text = "连接";
            item.SubItems.Add(Program.TestGateway.onlineText);
            this.listViewGateway.Items.Add(item);
            item = new ListViewItem();
            item.Text = "入网";
            item.SubItems.Add(Program.TestGateway.permitJoiningText);           
            this.listViewGateway.Items.Add(item);
            item = new ListViewItem();
            item.Text = "硬件版本";
            item.SubItems.Add(Program.TestGateway.hardwareVersion);
            this.listViewGateway.Items.Add(item);
            item = new ListViewItem();
            item.Text = "软件版本";
            item.SubItems.Add(Program.TestGateway.softwareVersion);
            this.listViewGateway.Items.Add(item);

        }
        private void RefreshGatewayData()
        {
            this.listViewGateway.Items[0].SubItems[1].Text = Program.TestGateway.onlineText;
            this.listViewGateway.Items[1].SubItems[1].Text = Program.TestGateway.permitJoiningText;
            this.listViewGateway.Items[2].SubItems[1].Text = Program.TestGateway.hardwareVersion;
            this.listViewGateway.Items[3].SubItems[1].Text = Program.TestGateway.softwareVersion;
            if (Program.TestGateway.permitJoining)
            {
                this.buttonPermitJoining.Text = "禁止入网";
                this.buttonPermitJoining.ImageIndex = 1;
            }
            else
            {
                this.buttonPermitJoining.Text = "允许入网";
                this.buttonPermitJoining.ImageIndex = 0;
            }
        }

        private void listViewRealDevices_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

        }

        private void listViewRealDevices_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            DeviceItem item = (DeviceItem)e.Item.Tag;
            e.Item.UseItemStyleForSubItems = false;

            if (item.online)
            {
                e.Item.BackColor = Color.Gray;
            }
            else
            {
                e.Item.BackColor = Color.White;
            }
        }

        private void RefreshDevicePropertyListViewItem(DeviceItem device, Property property)
        {
            if (this.listViewPropertys.Tag.Equals(device))
            {
                foreach(ListViewItem item in listViewPropertys.Items)
                {
                    if (item.Tag.Equals(property))
                    {
                        item.SubItems[1].Text = property.value;
                        break;
                    }
                }
            }
        }
        private void RefreshDevicePropertysListView(DeviceItem device)
        {
            
            this.listViewPropertys.Items.Clear();
            if (device != null)
            {
                this.listViewPropertys.Tag = device;
                foreach(Property property in device.propertys)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = property.keyName;
                    item.SubItems.Add(property.valueText);
                    item.SubItems.Add(property.unit);
                    item.Tag = property;
                    this.listViewPropertys.Items.Add(item);
                }           
            }
        }

        private void RefreshlistViewRealDevicesSelectedItem()
        {
            if (this.listViewRealDevices.SelectedItems.Count == 1)
            {
                DeviceItem device = (DeviceItem)this.listViewRealDevices.SelectedItems[0].Tag;
                Program.TestManager.UpdateActiveDevice(device);
                Program.TestManager.projectService.GenerateLabelContent(device);              
                this.RefreshDevicePropertysListView(device);               
                this.textBoxDeviceId.Text = device.deviceId;
                this.buttonAutoTest.Enabled =  device.online;
                this.buttonManualTest.Enabled = device.online;            
            }        
        }



        private void listViewRealDevices_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.RefreshlistViewRealDevicesSelectedItem();
        }

        private void buttonAutoTest_Click(object sender, EventArgs e)
        {
            if (Program.TestManager.testControllerPool.autoTestController.active)
            {
                Program.TestManager.testControllerPool.autoTestController.StopTest();
            }
            else
            {
                if (this.listViewRealDevices.SelectedItems.Count == 1)
                {
                    DeviceItem device = (DeviceItem)this.listViewRealDevices.SelectedItems[0].Tag;
                    Program.TestManager.testControllerPool.UpdateTestDeviceList(device);
                    Program.TestManager.testControllerPool.autoTestController.UpdateTestDeviceList(device);
                    Program.TestManager.testControllerPool.autoTestController.StartTest();

                }
            }
           
        }



        private void buttonManualTest_Click(object sender, EventArgs e)
        {
            if (Program.TestManager.testControllerPool.manualTestController.active)
            {
                Program.TestManager.testControllerPool.manualTestController.StopTest();
            }
            else
            {
                if (this.listViewRealDevices.SelectedItems.Count == 1)
                {
                    DeviceItem device = (DeviceItem)this.listViewRealDevices.SelectedItems[0].Tag;
                    Program.TestManager.testControllerPool.UpdateTestDeviceList(device);
                    Program.TestManager.testControllerPool.manualTestController.UpdateTestDeviceList(device);
                    Program.TestManager.testControllerPool.manualTestController.StartTest();

                }
            }
        }

        private void buttonTestPass_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.TestManager.deviceController.deviceList.Clear();
            Program.TestManager.activeDevice = null;
            Program.TestManager.deviceController.NotifyListUpdate(null);
            Program.TestManager.projectService.activeProjectIndex = this.comboBoxProjects.SelectedIndex;
            Program.TestManager.projectService.SaveToFile();
            this.RefreshDeviceNodesCombox();
            if (Program.TestManager.projectService.activeProject != null)
            {
                this.groupBoxSelectDeviceType.Enabled = !(Program.TestManager.projectService.activeProject.serialCode == "None");
            }
        }

        private void comboBoxDeviceColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.TestManager.projectService.deviceColor = this.comboBoxDeviceColors.SelectedIndex + 1;
            Program.TestManager.projectService.SaveToFile();
        }


        private void listViewRealDevices_DoubleClick(object sender, EventArgs e)
        {
            this.ShowSelectedDevicePropertyForm();
        }


        private void toolStripButtonGetDeviceList_Click(object sender, EventArgs e)
        {
            Program.TestManager.deviceController.deviceList.Clear();
            Program.TestManager.activeDevice = null;
            Program.TestManager.deviceController.NotifyListUpdate(null);
            Program.TestManager.deviceController.GetDeviceList();
        }

        private void toolStripButtonClearDeviceList_Click(object sender, EventArgs e)
        {
            foreach (DeviceItem device in Program.TestManager.deviceController.deviceList)
            {
                if (!device.deleted)
                {
                    Program.TestManager.deviceController.DeleteDevice(device.deviceId);
                    device.deleted = true;
                }
            }
            Program.TestManager.activeDevice = null;
            
            Program.TestManager.deviceController.GetDeviceList();
            Program.TestManager.deviceController.NotifyListUpdate(null);
            
        }



        private void checkBoxAutoPermitJoining_CheckedChanged(object sender, EventArgs e)
        {
            Program.TestManager.deviceController.gateway.autoPermitJoining = this.checkBoxAutoPermitJoining.Checked;
            Program.TestManager.deviceController.SaveToFile();
        }

        private void buttonPermitJoining_Click(object sender, EventArgs e)
        {
            Program.TestManager.PermitJoining();
        }

        private void FormDeviceTest_Shown(object sender, EventArgs e)
        {
            this.checkBoxAutoPermitJoining.Checked = Program.TestManager.deviceController.gateway.autoPermitJoining;
            if (Program.TestManager.deviceController.gateway.autoPermitJoining)
            {
                Program.TestManager.PermitJoining();
            }
        }

        private void ShowSelectedDevicePropertyForm()
        {
            if (this.listViewRealDevices.SelectedItems.Count == 1)
            {
                DeviceItem device = (DeviceItem)this.listViewRealDevices.SelectedItems[0].Tag;
                this.formDevice.UpdateActiveDevice(device);
                this.formDevice.ShowDialog();
            }          
        }

        private void toolStripButtonDevice_Click(object sender, EventArgs e)
        {
            this.ShowSelectedDevicePropertyForm();
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (this.listViewRealDevices.SelectedItems.Count == 1)
            {
                DeviceItem device = (DeviceItem)this.listViewRealDevices.SelectedItems[0].Tag;
                Program.TestManager.deviceController.DeleteDeviceItem(device);
            }
            Program.TestManager.deviceController.GetDeviceList();
            Program.TestManager.deviceController.NotifyListUpdate(null);
        }

        private void buttonSupportDevice_Click(object sender, EventArgs e)
        {
            FormSupportDevices form = new FormSupportDevices();
            form.ShowDialog();
        }

        private void toolStripSplitButtonProject_ButtonClick(object sender, EventArgs e)
        {
            string projectFolder = StringResource.MainDataPath + Program.TestManager.projectService.activeProject.name + "\\";
            if (Directory.Exists(projectFolder))
            {
                System.Diagnostics.Process.Start("explorer.exe", projectFolder);
            }
        }

        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            try
            {
                this.formImportDevice.ShowDialog();
            }
            catch(System.ObjectDisposedException)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.projectNodeList.Count; i++)
            { 
                DeviceNode node = this.projectNodeList[i];
                if (node.serialCode == this.textBoxSerialCode.Text)
                {
                    this.comboBoxDeviceList.SelectedIndex = i;
                }                      
            }
        }

        private void comboBoxDeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxDeviceList.SelectedIndex!=-1)
            {
                Program.TestManager.projectService.activeProject.activeDeviceNode = this.projectNodeList[this.comboBoxDeviceList.SelectedIndex];
            }
            else
            {
                Program.TestManager.projectService.activeProject.activeDeviceNode = null;
            }
        }

        private void buttonProductKeyDetail_Click(object sender, EventArgs e)
        {

        }



    }
}
