namespace DeviceTest
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonConfig = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButtonTool = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolStripMenuItemPrintLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemException = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonUpgrade = new System.Windows.Forms.ToolStripButton();
            this.ButtonAbout = new System.Windows.Forms.ToolStripButton();
            this.ButtonClose = new System.Windows.Forms.ToolStripButton();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageDeviceTest = new System.Windows.Forms.TabPage();
            this.tabPageGatewayTest = new System.Windows.Forms.TabPage();
            this.tabPageiLop = new System.Windows.Forms.TabPage();
            this.tabPageMessage = new System.Windows.Forms.TabPage();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelGatewayStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelHttpServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.timerCheckUpgrade = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageMessage.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tabControlMain, 0, 1);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(38, 12);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(677, 324);
            this.tableLayoutPanelMain.TabIndex = 4;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonConfig,
            this.toolStripSplitButtonTool,
            this.toolStripButtonUpgrade,
            this.ButtonAbout,
            this.ButtonClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(677, 31);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonConfig
            // 
            this.ButtonConfig.Image = ((System.Drawing.Image)(resources.GetObject("ButtonConfig.Image")));
            this.ButtonConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonConfig.Name = "ButtonConfig";
            this.ButtonConfig.Size = new System.Drawing.Size(74, 28);
            this.ButtonConfig.Text = "设置";
            this.ButtonConfig.Click += new System.EventHandler(this.ButtonConfig_Click);
            // 
            // toolStripSplitButtonTool
            // 
            this.toolStripSplitButtonTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemPrintLabel,
            this.toolStripMenuItem1,
            this.ToolStripMenuItemException});
            this.toolStripSplitButtonTool.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonTool.Image")));
            this.toolStripSplitButtonTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonTool.Name = "toolStripSplitButtonTool";
            this.toolStripSplitButtonTool.Size = new System.Drawing.Size(127, 28);
            this.toolStripSplitButtonTool.Text = "常用工具";
            // 
            // ToolStripMenuItemPrintLabel
            // 
            this.ToolStripMenuItemPrintLabel.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemPrintLabel.Image")));
            this.ToolStripMenuItemPrintLabel.Name = "ToolStripMenuItemPrintLabel";
            this.ToolStripMenuItemPrintLabel.Size = new System.Drawing.Size(210, 30);
            this.ToolStripMenuItemPrintLabel.Text = "标签打印";
            this.ToolStripMenuItemPrintLabel.Click += new System.EventHandler(this.ToolStripMenuItemPrintLabel_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(207, 6);
            // 
            // ToolStripMenuItemException
            // 
            this.ToolStripMenuItemException.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemException.Image")));
            this.ToolStripMenuItemException.Name = "ToolStripMenuItemException";
            this.ToolStripMenuItemException.Size = new System.Drawing.Size(210, 30);
            this.ToolStripMenuItemException.Text = "异常信息";
            this.ToolStripMenuItemException.Click += new System.EventHandler(this.ToolStripMenuItemException_Click);
            // 
            // toolStripButtonUpgrade
            // 
            this.toolStripButtonUpgrade.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUpgrade.Image")));
            this.toolStripButtonUpgrade.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUpgrade.Name = "toolStripButtonUpgrade";
            this.toolStripButtonUpgrade.Size = new System.Drawing.Size(74, 28);
            this.toolStripButtonUpgrade.Text = "更新";
            this.toolStripButtonUpgrade.Click += new System.EventHandler(this.toolStripButtonUpgrade_Click);
            // 
            // ButtonAbout
            // 
            this.ButtonAbout.Image = ((System.Drawing.Image)(resources.GetObject("ButtonAbout.Image")));
            this.ButtonAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAbout.Name = "ButtonAbout";
            this.ButtonAbout.Size = new System.Drawing.Size(74, 28);
            this.ButtonAbout.Text = "关于";
            this.ButtonAbout.Click += new System.EventHandler(this.ButtonAbout_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("ButtonClose.Image")));
            this.ButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(74, 28);
            this.ButtonClose.Text = "退出";
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageDeviceTest);
            this.tabControlMain.Controls.Add(this.tabPageGatewayTest);
            this.tabControlMain.Controls.Add(this.tabPageiLop);
            this.tabControlMain.Controls.Add(this.tabPageMessage);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(3, 58);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(671, 243);
            this.tabControlMain.TabIndex = 1;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageDeviceTest
            // 
            this.tabPageDeviceTest.Location = new System.Drawing.Point(4, 28);
            this.tabPageDeviceTest.Name = "tabPageDeviceTest";
            this.tabPageDeviceTest.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDeviceTest.Size = new System.Drawing.Size(663, 211);
            this.tabPageDeviceTest.TabIndex = 0;
            this.tabPageDeviceTest.Text = "终端设备";
            this.tabPageDeviceTest.UseVisualStyleBackColor = true;
            // 
            // tabPageGatewayTest
            // 
            this.tabPageGatewayTest.Location = new System.Drawing.Point(4, 28);
            this.tabPageGatewayTest.Name = "tabPageGatewayTest";
            this.tabPageGatewayTest.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGatewayTest.Size = new System.Drawing.Size(663, 211);
            this.tabPageGatewayTest.TabIndex = 2;
            this.tabPageGatewayTest.Text = "网关设备";
            this.tabPageGatewayTest.UseVisualStyleBackColor = true;
            // 
            // tabPageiLop
            // 
            this.tabPageiLop.Location = new System.Drawing.Point(4, 28);
            this.tabPageiLop.Name = "tabPageiLop";
            this.tabPageiLop.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageiLop.Size = new System.Drawing.Size(663, 211);
            this.tabPageiLop.TabIndex = 3;
            this.tabPageiLop.Text = "飞燕平台(单品)";
            this.tabPageiLop.UseVisualStyleBackColor = true;
            // 
            // tabPageMessage
            // 
            this.tabPageMessage.Controls.Add(this.richTextBoxMessage);
            this.tabPageMessage.Location = new System.Drawing.Point(4, 28);
            this.tabPageMessage.Name = "tabPageMessage";
            this.tabPageMessage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMessage.Size = new System.Drawing.Size(663, 211);
            this.tabPageMessage.TabIndex = 4;
            this.tabPageMessage.Text = "消息日志";
            this.tabPageMessage.UseVisualStyleBackColor = true;
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.BackColor = System.Drawing.SystemColors.InfoText;
            this.richTextBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMessage.ForeColor = System.Drawing.Color.Lime;
            this.richTextBoxMessage.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.Size = new System.Drawing.Size(657, 205);
            this.richTextBoxMessage.TabIndex = 2;
            this.richTextBoxMessage.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelGatewayStatus,
            this.toolStripStatusLabelHttpServer,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabelMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 455);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(850, 29);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelGatewayStatus
            // 
            this.toolStripStatusLabelGatewayStatus.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabelGatewayStatus.Image")));
            this.toolStripStatusLabelGatewayStatus.Name = "toolStripStatusLabelGatewayStatus";
            this.toolStripStatusLabelGatewayStatus.Size = new System.Drawing.Size(70, 24);
            this.toolStripStatusLabelGatewayStatus.Text = "网关";
            this.toolStripStatusLabelGatewayStatus.ToolTipText = "网关连接状态";
            this.toolStripStatusLabelGatewayStatus.Click += new System.EventHandler(this.toolStripStatusLabelGatewayStatus_Click);
            // 
            // toolStripStatusLabelHttpServer
            // 
            this.toolStripStatusLabelHttpServer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabelHttpServer.Image")));
            this.toolStripStatusLabelHttpServer.LinkColor = System.Drawing.Color.Black;
            this.toolStripStatusLabelHttpServer.Name = "toolStripStatusLabelHttpServer";
            this.toolStripStatusLabelHttpServer.Size = new System.Drawing.Size(70, 24);
            this.toolStripStatusLabelHttpServer.Text = "网络";
            this.toolStripStatusLabelHttpServer.ToolTipText = "云端服务器状态";
            this.toolStripStatusLabelHttpServer.Click += new System.EventHandler(this.toolStripStatusLabelHttpServer_Click);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(61, 24);
            this.toolStripStatusLabel3.Text = "  信息 ";
            // 
            // toolStripStatusLabelMessage
            // 
            this.toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            this.toolStripStatusLabelMessage.Size = new System.Drawing.Size(30, 24);
            this.toolStripStatusLabelMessage.Text = "    ";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // timerCheckUpgrade
            // 
            this.timerCheckUpgrade.Enabled = true;
            this.timerCheckUpgrade.Interval = 1000;
            this.timerCheckUpgrade.Tick += new System.EventHandler(this.timerCheckUpgrade_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 484);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "鸿雁智能终端调测工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageMessage.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonConfig;
        private System.Windows.Forms.ToolStripButton ButtonAbout;
        private System.Windows.Forms.ToolStripButton ButtonClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGatewayStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelHttpServer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageDeviceTest;
        private System.Windows.Forms.TabPage tabPageGatewayTest;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripButton toolStripButtonUpgrade;
        private System.Windows.Forms.TabPage tabPageiLop;
        private System.Windows.Forms.Timer timerCheckUpgrade;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonTool;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemException;
        private System.Windows.Forms.TabPage tabPageMessage;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPrintLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;

    }
}

