namespace GISDiary
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.axGlobeControl1 = new ESRI.ArcGIS.Controls.AxGlobeControl();
            this.btn_Globe = new System.Windows.Forms.Button();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            ((System.ComponentModel.ISupportInitialize)(this.axGlobeControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axGlobeControl1
            // 
            this.axGlobeControl1.Location = new System.Drawing.Point(0, 56);
            this.axGlobeControl1.Name = "axGlobeControl1";
            this.axGlobeControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axGlobeControl1.OcxState")));
            this.axGlobeControl1.Size = new System.Drawing.Size(774, 479);
            this.axGlobeControl1.TabIndex = 2;
            this.axGlobeControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IGlobeControlEvents_Ax_OnMouseDownEventHandler(this.axGlobeControl1_OnMouseDown);
            // 
            // btn_Globe
            // 
            this.btn_Globe.Location = new System.Drawing.Point(67, 6);
            this.btn_Globe.Name = "btn_Globe";
            this.btn_Globe.Size = new System.Drawing.Size(123, 26);
            this.btn_Globe.TabIndex = 3;
            this.btn_Globe.Text = "button1";
            this.btn_Globe.UseVisualStyleBackColor = true;
            this.btn_Globe.Click += new System.EventHandler(this.btn_Globe_Click);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(1014, 503);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 4;
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(250, 4);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(331, 28);
            this.axToolbarControl1.TabIndex = 5;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 536);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.btn_Globe);
            this.Controls.Add(this.axGlobeControl1);
            this.IsMdiContainer = true;
            this.Name = "Form3";
            this.Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)(this.axGlobeControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxGlobeControl axGlobeControl1;
        private System.Windows.Forms.Button btn_Globe;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
    }
}