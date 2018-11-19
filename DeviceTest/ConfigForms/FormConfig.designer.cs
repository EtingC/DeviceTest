namespace DeviceTest.ConfigForms
{
    partial class FormConfig
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("系统配置", 1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("设备配置", 0);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("打印配置", 2);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfig));
            this.pnl_formContainer = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.lv_controller = new System.Windows.Forms.ListView();
            this.Imagelist_controller = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_formContainer
            // 
            this.pnl_formContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_formContainer.Location = new System.Drawing.Point(175, 0);
            this.pnl_formContainer.Margin = new System.Windows.Forms.Padding(4);
            this.pnl_formContainer.Name = "pnl_formContainer";
            this.pnl_formContainer.Size = new System.Drawing.Size(767, 795);
            this.pnl_formContainer.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btn_ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(175, 795);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(767, 68);
            this.panel1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(641, 20);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ok.Location = new System.Drawing.Point(512, 20);
            this.btn_ok.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(112, 34);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "确定";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // lv_controller
            // 
            this.lv_controller.Dock = System.Windows.Forms.DockStyle.Left;
            this.lv_controller.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lv_controller.LargeImageList = this.Imagelist_controller;
            this.lv_controller.Location = new System.Drawing.Point(0, 0);
            this.lv_controller.Margin = new System.Windows.Forms.Padding(4);
            this.lv_controller.Name = "lv_controller";
            this.lv_controller.Size = new System.Drawing.Size(175, 863);
            this.lv_controller.TabIndex = 3;
            this.lv_controller.UseCompatibleStateImageBehavior = false;
            this.lv_controller.SelectedIndexChanged += new System.EventHandler(this.lv_controller_SelectedIndexChanged);
            // 
            // Imagelist_controller
            // 
            this.Imagelist_controller.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Imagelist_controller.ImageStream")));
            this.Imagelist_controller.TransparentColor = System.Drawing.Color.Transparent;
            this.Imagelist_controller.Images.SetKeyName(0, "deviceConfig.png");
            this.Imagelist_controller.Images.SetKeyName(1, "systemConfig.png");
            this.Imagelist_controller.Images.SetKeyName(2, "printer.png");
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 863);
            this.Controls.Add(this.pnl_formContainer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lv_controller);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置选项";
            this.Shown += new System.EventHandler(this.FormConfig_Shown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_formContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.ListView lv_controller;
        private System.Windows.Forms.ImageList Imagelist_controller;
        private System.Windows.Forms.Button button1;
    }
}