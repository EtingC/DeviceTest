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
using DeviceTest.ConfigForms;


namespace DeviceTest.ConfigDevice
{
    public partial class FormSupportDevices : Form
    {
        public FormDeviceSNDetail formDeviceSNDetail { get; set; }
        public FormSupportDevices()
        {
            InitializeComponent();
            
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeCombox()
        {
            int index = 0;
            foreach (Project project in Program.TestManager.projectService.projects)
            {
                this.comboBoxProjects.Items.Add(project.name);

                if (project.serialCode == "AL-Z")
                {
                    this.comboBoxProjects.SelectedIndex = index;
                }
                index += 1;
            }
            if (this.comboBoxProjects.Items.Count > 0 && this.comboBoxProjects.SelectedIndex < 0)
            {
                this.comboBoxProjects.SelectedIndex = 0;
            }

        }

        private void RefreshProjectDeviceListViewer(Project project)
        {
            this.listViewDeviceNodes.BeginUpdate();
            this.listViewDeviceNodes.Items.Clear();
            foreach (DeviceNode node in Program.TestManager.deviceController.deviceNodes.rows)
            {
                if (node.flag == project.serialCode)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = node.serialCode;
                    item.SubItems.Add(node.name);
                    item.SubItems.Add(node.firmware.modelId);
                    item.SubItems.Add(node.productKey);
                    if (project.generateSNType == GenerateSNType.FromServer)
                    {
                        string value = "SN数量(有效/总数)：" + node.remainCount.ToString() + "/" + node.totalCount.ToString();
                        item.SubItems.Add(value);
                    }
                    else
                    {
                        item.SubItems.Add("");
                    }
                    item.Tag = node;
                    this.listViewDeviceNodes.Items.Add(item);
                }
            }
            this.listViewDeviceNodes.EndUpdate();
        }

        private void RefreshDeviceListviewer()
        {
            
            this.listViewDevices.Items.Clear();
            foreach (DeviceConfig device in Program.TestManager.deviceController.deviceConfigList.items)
            {
                ListViewItem item = new ListViewItem();
                item.Text = device.model;
                item.SubItems.Add(device.name);
                item.SubItems.Add(device.modelId);
                item.Tag = device;
                item.Group = this.listViewDevices.Groups[(int)device.type];
                this.listViewDevices.Items.Add(item);
            }
            this.groupBoxDevice.Text = "设备列表(" + Program.TestManager.deviceController.deviceConfigList.items.Count + ")";
        }

        private void FormSupportDevices_Load(object sender, EventArgs e)
        {
            this.formDeviceSNDetail = new FormDeviceSNDetail();
            this.RefreshDeviceListviewer();
            this.InitializeCombox();
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            Program.TestManager.deviceController.deviceConfigList.Initialize();
            this.RefreshDeviceListviewer();
        }



        private void toolStripTextBoxQuery_TextChanged(object sender, EventArgs e)
        {
            string value = this.toolStripTextBoxQuery.Text.Trim();
            foreach (ListViewItem item in this.listViewDevices.Items)
            {
                item.Selected = false;
                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                {
                    if (subItem.Text.Contains(value))
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }

        private void comboBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxProjects.SelectedIndex >= 0)
            {
                Project project = Program.TestManager.projectService.projects[this.comboBoxProjects.SelectedIndex];
                this.RefreshProjectDeviceListViewer(project);
            }
            
        }

        private void ShowDeviceSerialCodeDetail()
        {
            if (listViewDeviceNodes.SelectedItems.Count==1)
            {
                DeviceNode node = (DeviceNode)listViewDeviceNodes.SelectedItems[0].Tag;
                if (node != null)
                {
                    this.formDeviceSNDetail.updateQueryForm(node);
                    this.formDeviceSNDetail.ShowDialog();
                }
                
            }
        }

        private void listViewDeviceNodes_DoubleClick(object sender, EventArgs e)
        {
            this.ShowDeviceSerialCodeDetail();
        }

        private void listViewDeviceNodes_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listViewDeviceNodes.SelectedItems.Count == 1)
            {
                DeviceNode node = (DeviceNode)listViewDeviceNodes.SelectedItems[0].Tag;
                if (node != null)
                {
                    this.textBoxName.Text = node.name;
                }

            }
        }

        private void buttonDetail_Click(object sender, EventArgs e)
        {
            this.ShowDeviceSerialCodeDetail();
        }
    }
}
