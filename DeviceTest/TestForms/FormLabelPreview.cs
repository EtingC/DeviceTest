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

namespace DeviceTest.TestForms
{
    public partial class FormLabelPreview : Form
    {
        public FormLabelPreview()
        {
            InitializeComponent();
            Program.TestManager.projectService.AttachObserver(this.Update);
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void RefreshLabelPreview()
        {
            this.listViewLabel.Items.Clear();
            if (Program.TestManager.projectService.result)
            {
                foreach (KeyValueItem item in Program.TestManager.projectService.generateResult.Items)
                {
                    ListViewItem newItem = new ListViewItem();
                    newItem.Text = item.Key;
                    newItem.SubItems.Add(item.Value);
                    this.listViewLabel.Items.Add(newItem);
                }
            }
        }

        private void UpdateInfo(int Event, string flag, object result, string message, object sender)
        {

            switch (Event)
            {
                case ProjectService.PROJECT_LABEL_GENERATE_EVENT:
                    {
                        this.RefreshLabelPreview();
                        break;
                    }
            }
        }

        private void FormLabelPreview_Shown(object sender, EventArgs e)
        {
            this.RefreshLabelPreview();
        }

        private void FormLabelPreview_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.TestManager.projectService.DetachObserver(this.Update);
        }

    }




}
