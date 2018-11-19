namespace DeviceTest.TestForms
{
    partial class FormDeviceValues
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listViewDeviceValues = new System.Windows.Forms.ListView();
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(882, 518);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listViewDeviceValues);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(874, 486);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数值";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listViewDeviceValues
            // 
            this.listViewDeviceValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader1});
            this.listViewDeviceValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDeviceValues.FullRowSelect = true;
            this.listViewDeviceValues.GridLines = true;
            this.listViewDeviceValues.Location = new System.Drawing.Point(3, 3);
            this.listViewDeviceValues.Name = "listViewDeviceValues";
            this.listViewDeviceValues.Size = new System.Drawing.Size(868, 480);
            this.listViewDeviceValues.TabIndex = 2;
            this.listViewDeviceValues.UseCompatibleStateImageBehavior = false;
            this.listViewDeviceValues.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "时间";
            this.columnHeader14.Width = 168;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "--";
            this.columnHeader15.Width = 145;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "单位";
            this.columnHeader1.Width = 90;
            // 
            // FormDeviceValues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 518);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormDeviceValues";
            this.Text = "FormDeviceChart";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView listViewDeviceValues;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}