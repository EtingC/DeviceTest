namespace DeviceTest.ConfigDevice
{
    partial class FormDeviceSNDetail
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxDeviceInfo = new System.Windows.Forms.TextBox();
            this.group = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerEndDT = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStartDT = new System.Windows.Forms.DateTimePicker();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBoxDetail = new System.Windows.Forms.GroupBox();
            this.listViewDeviceCodes = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.group.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBoxDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.group);
            this.panel1.Controls.Add(this.buttonExport);
            this.panel1.Controls.Add(this.buttonQuery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1530, 122);
            this.panel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxDeviceInfo);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(442, 90);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "产品信息";
            // 
            // textBoxDeviceInfo
            // 
            this.textBoxDeviceInfo.BackColor = System.Drawing.SystemColors.Menu;
            this.textBoxDeviceInfo.Location = new System.Drawing.Point(27, 38);
            this.textBoxDeviceInfo.Name = "textBoxDeviceInfo";
            this.textBoxDeviceInfo.ReadOnly = true;
            this.textBoxDeviceInfo.Size = new System.Drawing.Size(401, 28);
            this.textBoxDeviceInfo.TabIndex = 0;
            // 
            // group
            // 
            this.group.Controls.Add(this.label2);
            this.group.Controls.Add(this.label1);
            this.group.Controls.Add(this.dateTimePickerEndDT);
            this.group.Controls.Add(this.dateTimePickerStartDT);
            this.group.Location = new System.Drawing.Point(460, 12);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(596, 90);
            this.group.TabIndex = 6;
            this.group.TabStop = false;
            this.group.Text = "日期区间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(307, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "结束日期";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "开始日期";
            // 
            // dateTimePickerEndDT
            // 
            this.dateTimePickerEndDT.Location = new System.Drawing.Point(393, 34);
            this.dateTimePickerEndDT.Name = "dateTimePickerEndDT";
            this.dateTimePickerEndDT.Size = new System.Drawing.Size(181, 28);
            this.dateTimePickerEndDT.TabIndex = 5;
            // 
            // dateTimePickerStartDT
            // 
            this.dateTimePickerStartDT.Location = new System.Drawing.Point(112, 34);
            this.dateTimePickerStartDT.Name = "dateTimePickerStartDT";
            this.dateTimePickerStartDT.Size = new System.Drawing.Size(181, 28);
            this.dateTimePickerStartDT.TabIndex = 4;
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(1242, 31);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(146, 62);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "导出白名单...";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(1077, 31);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(148, 62);
            this.buttonQuery.TabIndex = 4;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 836);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1530, 77);
            this.panel2.TabIndex = 2;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(1391, 17);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(110, 38);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBoxDetail);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 122);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1530, 714);
            this.panel3.TabIndex = 3;
            // 
            // groupBoxDetail
            // 
            this.groupBoxDetail.Controls.Add(this.listViewDeviceCodes);
            this.groupBoxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDetail.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDetail.Name = "groupBoxDetail";
            this.groupBoxDetail.Size = new System.Drawing.Size(1530, 714);
            this.groupBoxDetail.TabIndex = 2;
            this.groupBoxDetail.TabStop = false;
            this.groupBoxDetail.Text = "SN信息列表";
            // 
            // listViewDeviceCodes
            // 
            this.listViewDeviceCodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader5,
            this.columnHeader4,
            this.columnHeader6,
            this.columnHeader7});
            this.listViewDeviceCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDeviceCodes.FullRowSelect = true;
            this.listViewDeviceCodes.GridLines = true;
            this.listViewDeviceCodes.Location = new System.Drawing.Point(3, 24);
            this.listViewDeviceCodes.Name = "listViewDeviceCodes";
            this.listViewDeviceCodes.Size = new System.Drawing.Size(1524, 687);
            this.listViewDeviceCodes.TabIndex = 3;
            this.listViewDeviceCodes.UseCompatibleStateImageBehavior = false;
            this.listViewDeviceCodes.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "物料代码";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "SN（DeviceName）";
            this.columnHeader3.Width = 252;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "ProductKey";
            this.columnHeader5.Width = 208;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "标识";
            this.columnHeader4.Width = 227;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Secret";
            this.columnHeader6.Width = 211;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "状态";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "文本文件|*.txt";
            this.saveFileDialog.RestoreDirectory = true;
            // 
            // FormDeviceSNDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1530, 913);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FormDeviceSNDetail";
            this.Text = "SN详细";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDeviceSNDetail_FormClosed);
            this.Load += new System.EventHandler(this.FormDeviceSNDetail_Load);
            this.Shown += new System.EventHandler(this.FormDeviceSNDetail_Shown);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.group.ResumeLayout(false);
            this.group.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBoxDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.GroupBox group;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDT;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartDT;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBoxDetail;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listViewDeviceCodes;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TextBox textBoxDeviceInfo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}