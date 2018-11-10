namespace GISDiary
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axSceneControl1 = new ESRI.ArcGIS.Controls.AxSceneControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_flyPath = new System.Windows.Forms.Button();
            this.axGlobeControl1 = new ESRI.ArcGIS.Controls.AxGlobeControl();
            ((System.ComponentModel.ISupportInitialize)(this.axSceneControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axGlobeControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axSceneControl1
            // 
            this.axSceneControl1.Location = new System.Drawing.Point(0, 0);
            this.axSceneControl1.Name = "axSceneControl1";
            this.axSceneControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSceneControl1.OcxState")));
            this.axSceneControl1.Size = new System.Drawing.Size(542, 435);
            this.axSceneControl1.TabIndex = 0;
            this.axSceneControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ISceneControlEvents_Ax_OnMouseDownEventHandler(this.axSceneControl1_OnMouseDown);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(483, 0);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(80, 6);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(397, 28);
            this.axToolbarControl1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_flyPath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1221, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 616);
            this.panel1.TabIndex = 4;
            // 
            // btn_flyPath
            // 
            this.btn_flyPath.Location = new System.Drawing.Point(24, 55);
            this.btn_flyPath.Name = "btn_flyPath";
            this.btn_flyPath.Size = new System.Drawing.Size(120, 46);
            this.btn_flyPath.TabIndex = 0;
            this.btn_flyPath.Text = "沿路径飞行";
            this.btn_flyPath.UseVisualStyleBackColor = true;
            this.btn_flyPath.Click += new System.EventHandler(this.btn_flyPath_Click);
            // 
            // axGlobeControl1
            // 
            this.axGlobeControl1.Location = new System.Drawing.Point(559, 0);
            this.axGlobeControl1.Name = "axGlobeControl1";
            this.axGlobeControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axGlobeControl1.OcxState")));
            this.axGlobeControl1.Size = new System.Drawing.Size(541, 435);
            this.axGlobeControl1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1421, 616);
            this.Controls.Add(this.axGlobeControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axSceneControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axSceneControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axGlobeControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxSceneControl axSceneControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_flyPath;
        private ESRI.ArcGIS.Controls.AxGlobeControl axGlobeControl1;
    }
}