namespace GISDiary
{
    partial class Form_difficulty
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_difficulty));
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axPageLayoutControl1 = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.盗墓足迹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.盗墓笔记ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.艰险指数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.记下小本本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.我的盗墓之旅ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.趟山海ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.遍古今ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.axTOCControl1.Location = new System.Drawing.Point(0, 0);
            this.axTOCControl1.Margin = new System.Windows.Forms.Padding(2);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(176, 473);
            this.axTOCControl1.TabIndex = 1;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(176, 0);
            this.axMapControl1.Margin = new System.Windows.Forms.Padding(2);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(852, 473);
            this.axMapControl1.TabIndex = 2;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnViewRefreshed += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnViewRefreshedEventHandler(this.axMapControl1_OnViewRefreshed);
            this.axMapControl1.OnAfterScreenDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterScreenDrawEventHandler(this.axMapControl1_OnAfterScreenDraw);
            this.axMapControl1.OnMapReplaced += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMapReplacedEventHandler(this.axMapControl1_OnMapReplaced);
            // 
            // axPageLayoutControl1
            // 
            this.axPageLayoutControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.axPageLayoutControl1.Location = new System.Drawing.Point(697, 0);
            this.axPageLayoutControl1.Margin = new System.Windows.Forms.Padding(2);
            this.axPageLayoutControl1.Name = "axPageLayoutControl1";
            this.axPageLayoutControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl1.OcxState")));
            this.axPageLayoutControl1.Size = new System.Drawing.Size(331, 473);
            this.axPageLayoutControl1.TabIndex = 4;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(553, 469);
            this.axLicenseControl1.Margin = new System.Windows.Forms.Padding(2);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 5;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.盗墓足迹ToolStripMenuItem,
            this.我的盗墓之旅ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(176, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(521, 25);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 盗墓足迹ToolStripMenuItem
            // 
            this.盗墓足迹ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.盗墓笔记ToolStripMenuItem,
            this.艰险指数ToolStripMenuItem,
            this.记下小本本ToolStripMenuItem});
            this.盗墓足迹ToolStripMenuItem.Name = "盗墓足迹ToolStripMenuItem";
            this.盗墓足迹ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.盗墓足迹ToolStripMenuItem.Text = "盗墓足迹";
            // 
            // 盗墓笔记ToolStripMenuItem
            // 
            this.盗墓笔记ToolStripMenuItem.Name = "盗墓笔记ToolStripMenuItem";
            this.盗墓笔记ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.盗墓笔记ToolStripMenuItem.Text = "盗墓笔记";
            this.盗墓笔记ToolStripMenuItem.Click += new System.EventHandler(this.盗墓笔记ToolStripMenuItem_Click_1);
            // 
            // 艰险指数ToolStripMenuItem
            // 
            this.艰险指数ToolStripMenuItem.Name = "艰险指数ToolStripMenuItem";
            this.艰险指数ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.艰险指数ToolStripMenuItem.Text = "艰险指数";
            this.艰险指数ToolStripMenuItem.Click += new System.EventHandler(this.艰险指数ToolStripMenuItem_Click_1);
            // 
            // 记下小本本ToolStripMenuItem
            // 
            this.记下小本本ToolStripMenuItem.Name = "记下小本本ToolStripMenuItem";
            this.记下小本本ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.记下小本本ToolStripMenuItem.Text = "记下小本本";
            this.记下小本本ToolStripMenuItem.Click += new System.EventHandler(this.记下小本本ToolStripMenuItem_Click_1);
            // 
            // 我的盗墓之旅ToolStripMenuItem
            // 
            this.我的盗墓之旅ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.趟山海ToolStripMenuItem,
            this.遍古今ToolStripMenuItem});
            this.我的盗墓之旅ToolStripMenuItem.Name = "我的盗墓之旅ToolStripMenuItem";
            this.我的盗墓之旅ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.我的盗墓之旅ToolStripMenuItem.Text = "我的盗墓之旅";
            // 
            // 趟山海ToolStripMenuItem
            // 
            this.趟山海ToolStripMenuItem.Name = "趟山海ToolStripMenuItem";
            this.趟山海ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.趟山海ToolStripMenuItem.Text = "趟山海";
            this.趟山海ToolStripMenuItem.Click += new System.EventHandler(this.趟山海ToolStripMenuItem_Click);
            // 
            // 遍古今ToolStripMenuItem
            // 
            this.遍古今ToolStripMenuItem.Name = "遍古今ToolStripMenuItem";
            this.遍古今ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.遍古今ToolStripMenuItem.Text = "遍古今";
            this.遍古今ToolStripMenuItem.Click += new System.EventHandler(this.遍古今ToolStripMenuItem_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 473);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axPageLayoutControl1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form2";
            this.Text = "Form2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxPageLayoutControl axPageLayoutControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 盗墓足迹ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 艰险指数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 我的盗墓之旅ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 趟山海ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 遍古今ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 盗墓笔记ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 记下小本本ToolStripMenuItem;
    }
}