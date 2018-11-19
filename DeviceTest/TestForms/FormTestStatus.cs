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

namespace DeviceTest.TestForms
{
    public partial class FormTestStatus : Form
    {
        private FormLabelPreview formLabelPreview { get; set; }
        private DeviceItem activeDevice {

            get {
                return Program.TestManager.activeDevice;
            }
        }
        public FormTestStatus()
        {
            InitializeComponent();
            
            
            Program.TestManager.AttachObserver(this.Update);
            Program.TestManager.projectService.AttachObserver(this.Update);
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
                
                case TestController.TEST_CONTROLLER_MESSAGE_EVENT:
                    {
                        TestOperate step = (TestOperate)Enum.Parse(typeof(TestOperate), flag);
                        this.ProcessTestMessage((TestType)result, step, message);
                        break;
                    }
                case ProjectService.PROJECT_LABEL_PRINT_FINISH_EVENT:
                    {
                        if (Program.TestManager.projectService.responseMessage.ErrorCode == (int)PrinterErrorCode.Ok)
                        {
                            this.labelLabelResult.ForeColor = Color.Black;
                        }
                        else
                        {
                            this.labelLabelResult.ForeColor = Color.Red;

                        }
                        this.labelLabelResult.Text = Program.TestManager.projectService.responseMessage.Message;

                        break;
                    }
                case ProjectService.PROJECT_LABEL_GENERATE_EVENT:
                    {
                        this.richTextBoxLabelData.Clear();
                        if (Program.TestManager.projectService.result)
                        { 
                            this.richTextBoxLabelData.ForeColor = Color.Black;
                            this.labelLabelResult.ForeColor = Color.Black;
                        }
                        else 
                        { 
                             this.richTextBoxLabelData.ForeColor = Color.Red;
                             this.labelLabelResult.ForeColor = Color.Red;
                             
                        }
                        this.labelLabelResult.Text = Program.TestManager.projectService.message;

                        if ((message != null) && (message.Length > 0))
                        {
                            message = Utils.Unicode2String(message);
                            this.richTextBoxLabelData.AppendText(message);
                        }
                        break;
                    }
            }
        }

        private void ProcessTestMessage(TestType testType, TestOperate operate, string message)
        {
            switch (operate)
            {
                case TestOperate.Start:
                    {
                        if (this.checkBoxStartToClear.Checked)
                        {
                            this.richTextBoxMessage.Clear();
                        }
                        break;
                    }
            }

            if (message.Length > 0)
            {
                this.labelTestMessage.Text = message;
                this.richTextBoxMessage.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + message + "\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.richTextBoxMessage.Clear();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxLabelEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.richTextBoxLabelData.ReadOnly)
            {
                this.richTextBoxLabelData.BackColor = Color.WhiteSmoke;
            }
            else
            {
                this.richTextBoxLabelData.BackColor = Color.White;
            }
            this.richTextBoxLabelData.Refresh();
        }

        private void buttonPrintLabel_Click(object sender, EventArgs e)
        {
            if (Program.TestManager.activeDevice != null) 
            {
                if (Program.TestManager.activeDevice.printlabelSum >= 1)
                {
                    string message = "设备[" + Program.TestManager.activeDevice.deviceId + "]已经打印标签" + Program.TestManager.activeDevice.printlabelSum.ToString() + "次，是否继续？";
                    if (MessageBox.Show(message, "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                Program.TestManager.AddDeviceTestResultRecord(this.activeDevice);
                Program.TestManager.PrintLabelContext();
                Program.TestManager.deviceController.NotifyListUpdate(this.activeDevice);

            }


        }

        private void buttonTestPass_Click(object sender, EventArgs e)
        {
            if(this.activeDevice!=null)
            {
                this.activeDevice.testResult = TestResult.Pass;
                Program.TestManager.AddDeviceTestResultRecord(this.activeDevice);
            }
        }

        private void buttonTestFail_Click(object sender, EventArgs e)
        {
            if (this.activeDevice != null)
            {
                this.activeDevice.testResult = TestResult.Fail;
                Program.TestManager.AddDeviceTestResultRecord(this.activeDevice);
            }
        }

        private void buttonLabelPreview_Click(object sender, EventArgs e)
        {
            this.formLabelPreview = new FormLabelPreview();
            this.formLabelPreview.Show();
        }
        

    }
}
