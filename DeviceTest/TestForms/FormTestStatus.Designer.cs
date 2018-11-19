namespace DeviceTest.TestForms
{
    partial class FormTestStatus
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTestStatus));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBoxStartToClear = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.labelTestMessage = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBoxLabelData = new System.Windows.Forms.RichTextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.buttonTestPass = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.buttonTestFail = new System.Windows.Forms.Button();
            this.buttonPrintLabel = new System.Windows.Forms.Button();
            this.buttonLabelPreview = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelLabelResult = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(650, 77);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 180);
            this.panel1.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxMessage);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(431, 137);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "消息";
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.BackColor = System.Drawing.Color.White;
            this.richTextBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMessage.ForeColor = System.Drawing.Color.Black;
            this.richTextBoxMessage.Location = new System.Drawing.Point(3, 24);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.Size = new System.Drawing.Size(425, 110);
            this.richTextBoxMessage.TabIndex = 6;
            this.richTextBoxMessage.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBoxStartToClear);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 137);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(431, 43);
            this.panel2.TabIndex = 4;
            // 
            // checkBoxStartToClear
            // 
            this.checkBoxStartToClear.AutoSize = true;
            this.checkBoxStartToClear.Checked = true;
            this.checkBoxStartToClear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStartToClear.Location = new System.Drawing.Point(22, 12);
            this.checkBoxStartToClear.Name = "checkBoxStartToClear";
            this.checkBoxStartToClear.Size = new System.Drawing.Size(178, 22);
            this.checkBoxStartToClear.TabIndex = 1;
            this.checkBoxStartToClear.Text = "开始测试自动清空";
            this.checkBoxStartToClear.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(318, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "清空";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.SystemColors.Info;
            this.panel.Controls.Add(this.labelTestMessage);
            this.panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1090, 71);
            this.panel.TabIndex = 6;
            // 
            // labelTestMessage
            // 
            this.labelTestMessage.AutoSize = true;
            this.labelTestMessage.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTestMessage.Location = new System.Drawing.Point(15, 17);
            this.labelTestMessage.Name = "labelTestMessage";
            this.labelTestMessage.Size = new System.Drawing.Size(46, 42);
            this.labelTestMessage.TabIndex = 0;
            this.labelTestMessage.Text = "--";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.richTextBoxLabelData);
            this.groupBox2.Location = new System.Drawing.Point(22, 487);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 0);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标签";
            this.groupBox2.Visible = false;
            // 
            // richTextBoxLabelData
            // 
            this.richTextBoxLabelData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxLabelData.BackColor = System.Drawing.Color.WhiteSmoke;
            this.richTextBoxLabelData.ForeColor = System.Drawing.SystemColors.WindowText;
            this.richTextBoxLabelData.Location = new System.Drawing.Point(6, 27);
            this.richTextBoxLabelData.Name = "richTextBoxLabelData";
            this.richTextBoxLabelData.ReadOnly = true;
            this.richTextBoxLabelData.Size = new System.Drawing.Size(309, 26);
            this.richTextBoxLabelData.TabIndex = 0;
            this.richTextBoxLabelData.Text = "";
            this.richTextBoxLabelData.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.buttonTestPass);
            this.groupBox6.Controls.Add(this.buttonTestFail);
            this.groupBox6.Location = new System.Drawing.Point(6, 77);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(195, 175);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "测试结果";
            // 
            // buttonTestPass
            // 
            this.buttonTestPass.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonTestPass.ImageIndex = 1;
            this.buttonTestPass.ImageList = this.imageList;
            this.buttonTestPass.Location = new System.Drawing.Point(17, 27);
            this.buttonTestPass.Name = "buttonTestPass";
            this.buttonTestPass.Size = new System.Drawing.Size(157, 52);
            this.buttonTestPass.TabIndex = 3;
            this.buttonTestPass.Text = "通过";
            this.buttonTestPass.UseVisualStyleBackColor = true;
            this.buttonTestPass.Click += new System.EventHandler(this.buttonTestPass_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "document_print_preview.ico");
            this.imageList.Images.SetKeyName(1, "gtk_apply.ico");
            this.imageList.Images.SetKeyName(2, "gtk_cancel.ico");
            this.imageList.Images.SetKeyName(3, "printer.ico");
            // 
            // buttonTestFail
            // 
            this.buttonTestFail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonTestFail.ImageIndex = 2;
            this.buttonTestFail.ImageList = this.imageList;
            this.buttonTestFail.Location = new System.Drawing.Point(16, 97);
            this.buttonTestFail.Name = "buttonTestFail";
            this.buttonTestFail.Size = new System.Drawing.Size(157, 51);
            this.buttonTestFail.TabIndex = 2;
            this.buttonTestFail.Text = "失败";
            this.buttonTestFail.UseVisualStyleBackColor = true;
            this.buttonTestFail.Click += new System.EventHandler(this.buttonTestFail_Click);
            // 
            // buttonPrintLabel
            // 
            this.buttonPrintLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPrintLabel.ImageIndex = 3;
            this.buttonPrintLabel.ImageList = this.imageList;
            this.buttonPrintLabel.Location = new System.Drawing.Point(235, 97);
            this.buttonPrintLabel.Name = "buttonPrintLabel";
            this.buttonPrintLabel.Size = new System.Drawing.Size(157, 52);
            this.buttonPrintLabel.TabIndex = 9;
            this.buttonPrintLabel.Text = "打印标签";
            this.buttonPrintLabel.UseVisualStyleBackColor = true;
            this.buttonPrintLabel.Click += new System.EventHandler(this.buttonPrintLabel_Click);
            // 
            // buttonLabelPreview
            // 
            this.buttonLabelPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonLabelPreview.ImageIndex = 0;
            this.buttonLabelPreview.ImageList = this.imageList;
            this.buttonLabelPreview.Location = new System.Drawing.Point(71, 98);
            this.buttonLabelPreview.Name = "buttonLabelPreview";
            this.buttonLabelPreview.Size = new System.Drawing.Size(158, 51);
            this.buttonLabelPreview.TabIndex = 10;
            this.buttonLabelPreview.Text = "标签预览";
            this.buttonLabelPreview.UseVisualStyleBackColor = true;
            this.buttonLabelPreview.Click += new System.EventHandler(this.buttonLabelPreview_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonPrintLabel);
            this.groupBox3.Controls.Add(this.buttonLabelPreview);
            this.groupBox3.Controls.Add(this.labelLabelResult);
            this.groupBox3.Location = new System.Drawing.Point(220, 77);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(411, 172);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "标签打印";
            // 
            // labelLabelResult
            // 
            this.labelLabelResult.AutoSize = true;
            this.labelLabelResult.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelLabelResult.Location = new System.Drawing.Point(19, 34);
            this.labelLabelResult.Name = "labelLabelResult";
            this.labelLabelResult.Size = new System.Drawing.Size(30, 28);
            this.labelLabelResult.TabIndex = 1;
            this.labelLabelResult.Text = "--";
            // 
            // FormTestStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 255);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.panel1);
            this.Name = "FormTestStatus";
            this.Text = "FormTestStatus";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBoxStartToClear;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label labelTestMessage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox richTextBoxLabelData;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button buttonTestPass;
        private System.Windows.Forms.Button buttonTestFail;
        private System.Windows.Forms.Button buttonPrintLabel;
        private System.Windows.Forms.Button buttonLabelPreview;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelLabelResult;
        private System.Windows.Forms.ImageList imageList;

    }
}