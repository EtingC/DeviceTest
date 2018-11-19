namespace DeviceTest.TestForms
{
    partial class FormDevicePropertys
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
            this.groupBoxPropertys = new System.Windows.Forms.GroupBox();
            this.listViewPropertys = new System.Windows.Forms.ListView();
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxPropertys.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPropertys
            // 
            this.groupBoxPropertys.Controls.Add(this.listViewPropertys);
            this.groupBoxPropertys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPropertys.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPropertys.Name = "groupBoxPropertys";
            this.groupBoxPropertys.Size = new System.Drawing.Size(722, 535);
            this.groupBoxPropertys.TabIndex = 1;
            this.groupBoxPropertys.TabStop = false;
            this.groupBoxPropertys.Text = "产品属性";
            // 
            // listViewPropertys
            // 
            this.listViewPropertys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader1,
            this.columnHeader16});
            this.listViewPropertys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPropertys.FullRowSelect = true;
            this.listViewPropertys.GridLines = true;
            this.listViewPropertys.Location = new System.Drawing.Point(3, 24);
            this.listViewPropertys.Name = "listViewPropertys";
            this.listViewPropertys.Size = new System.Drawing.Size(716, 508);
            this.listViewPropertys.TabIndex = 0;
            this.listViewPropertys.UseCompatibleStateImageBehavior = false;
            this.listViewPropertys.View = System.Windows.Forms.View.Details;
            this.listViewPropertys.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewPropertys_ItemSelectionChanged);
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "属性";
            this.columnHeader14.Width = 136;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "值";
            this.columnHeader15.Width = 212;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "单位";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "数值范围";
            this.columnHeader1.Width = 204;
            // 
            // FormDevicePropertys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 535);
            this.Controls.Add(this.groupBoxPropertys);
            this.Name = "FormDevicePropertys";
            this.Text = "FormDevicePropertys";
            this.groupBoxPropertys.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPropertys;
        private System.Windows.Forms.ListView listViewPropertys;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}