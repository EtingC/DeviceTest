namespace DeviceTest.TestForms
{
    partial class FormImportDevice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportDevice));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.checkBoxAutoPrintLabel = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.textBoxImportText = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonSelectLabel = new System.Windows.Forms.Button();
            this.textBoxLabelPath = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonClear);
            this.groupBox1.Controls.Add(this.checkBoxAutoPrintLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonOk);
            this.groupBox1.Controls.Add(this.textBoxImportText);
            this.groupBox1.Location = new System.Drawing.Point(9, 114);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(932, 248);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入（回车表示输入结束）";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(830, 27);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(93, 44);
            this.buttonClear.TabIndex = 4;
            this.buttonClear.Text = "清空";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // checkBoxAutoPrintLabel
            // 
            this.checkBoxAutoPrintLabel.AutoSize = true;
            this.checkBoxAutoPrintLabel.Checked = true;
            this.checkBoxAutoPrintLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoPrintLabel.Location = new System.Drawing.Point(25, 198);
            this.checkBoxAutoPrintLabel.Name = "checkBoxAutoPrintLabel";
            this.checkBoxAutoPrintLabel.Size = new System.Drawing.Size(142, 22);
            this.checkBoxAutoPrintLabel.TabIndex = 3;
            this.checkBoxAutoPrintLabel.Text = "自动打印标签";
            this.checkBoxAutoPrintLabel.UseVisualStyleBackColor = true;
            this.checkBoxAutoPrintLabel.CheckedChanged += new System.EventHandler(this.checkBoxAutoPrintLabel_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(182, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "注：字符串以关键字[SN]输出到打印服务器";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(830, 77);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(96, 90);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "打印";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // textBoxImportText
            // 
            this.textBoxImportText.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxImportText.ForeColor = System.Drawing.SystemColors.Highlight;
            this.textBoxImportText.Location = new System.Drawing.Point(25, 27);
            this.textBoxImportText.Multiline = true;
            this.textBoxImportText.Name = "textBoxImportText";
            this.textBoxImportText.Size = new System.Drawing.Size(799, 140);
            this.textBoxImportText.TabIndex = 0;
            this.textBoxImportText.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBoxImportText_MouseClick);
            this.textBoxImportText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 373);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(953, 68);
            this.panel1.TabIndex = 1;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(839, 17);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(96, 33);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonSelectLabel);
            this.groupBox2.Controls.Add(this.textBoxLabelPath);
            this.groupBox2.Location = new System.Drawing.Point(12, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(926, 101);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择标签模板文件";
            // 
            // buttonSelectLabel
            // 
            this.buttonSelectLabel.Location = new System.Drawing.Point(824, 38);
            this.buttonSelectLabel.Name = "buttonSelectLabel";
            this.buttonSelectLabel.Size = new System.Drawing.Size(96, 35);
            this.buttonSelectLabel.TabIndex = 2;
            this.buttonSelectLabel.Text = "选择...";
            this.buttonSelectLabel.UseVisualStyleBackColor = true;
            this.buttonSelectLabel.Click += new System.EventHandler(this.buttonSelectLabel_Click);
            // 
            // textBoxLabelPath
            // 
            this.textBoxLabelPath.Location = new System.Drawing.Point(22, 42);
            this.textBoxLabelPath.Name = "textBoxLabelPath";
            this.textBoxLabelPath.Size = new System.Drawing.Size(799, 28);
            this.textBoxLabelPath.TabIndex = 0;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "标签模板|*.prn";
            this.openFileDialog.Title = "标签模板选择";
            // 
            // FormImportDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 441);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormImportDevice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标签打印";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxImportText;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonSelectLabel;
        private System.Windows.Forms.TextBox textBoxLabelPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxAutoPrintLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button buttonClear;
    }
}