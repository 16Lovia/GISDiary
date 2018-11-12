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
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.axGlobeControl1 = new ESRI.ArcGIS.Controls.AxGlobeControl();
            this.btn_globe = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axGlobeControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(13, 72);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 1;
            // 
            // axGlobeControl1
            // 
            this.axGlobeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axGlobeControl1.Location = new System.Drawing.Point(0, 0);
            this.axGlobeControl1.Name = "axGlobeControl1";
            this.axGlobeControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axGlobeControl1.OcxState")));
            this.axGlobeControl1.Size = new System.Drawing.Size(1060, 556);
            this.axGlobeControl1.TabIndex = 3;
        
            // 
            // btn_globe
            // 
            this.btn_globe.Location = new System.Drawing.Point(0, 0);
            this.btn_globe.Name = "btn_globe";
            this.btn_globe.Size = new System.Drawing.Size(88, 46);
            this.btn_globe.TabIndex = 4;
            this.btn_globe.Text = "Globe";
            this.btn_globe.UseVisualStyleBackColor = true;
            this.btn_globe.Click += new System.EventHandler(this.btn_globe_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 556);
            this.Controls.Add(this.btn_globe);
            this.Controls.Add(this.axGlobeControl1);
            this.Controls.Add(this.axLicenseControl1);
            this.Name = "Form3";
            this.Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axGlobeControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private ESRI.ArcGIS.Controls.AxGlobeControl axGlobeControl1;
        private System.Windows.Forms.Button btn_globe;
    }
}