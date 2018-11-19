namespace DeviceTest.RegisterDevice
{
    partial class FormiLopTest
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
            this.tabPageGPRS = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGPRS);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1142, 552);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageGPRS
            // 
            this.tabPageGPRS.Location = new System.Drawing.Point(4, 28);
            this.tabPageGPRS.Name = "tabPageGPRS";
            this.tabPageGPRS.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGPRS.Size = new System.Drawing.Size(1134, 520);
            this.tabPageGPRS.TabIndex = 1;
            this.tabPageGPRS.Text = "燃气报警器";
            this.tabPageGPRS.UseVisualStyleBackColor = true;
            // 
            // FormiLopTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 552);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormiLopTest";
            this.Text = "FormiLopTest";
            this.Load += new System.EventHandler(this.FormiLopTest_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGPRS;
    }
}