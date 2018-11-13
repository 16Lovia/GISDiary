namespace GISDiary
{
    partial class Form5
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.SelStartPoi = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Dijskta = new System.Windows.Forms.Button();
            this.checkDistance = new System.Windows.Forms.CheckBox();
            this.checkTime = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(0, 0);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(1398, 542);
            this.axMapControl1.TabIndex = 0;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(3, 20);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkTime);
            this.panel1.Controls.Add(this.checkDistance);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.SelStartPoi);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_Dijskta);
            this.panel1.Controls.Add(this.axLicenseControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1172, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 542);
            this.panel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "选择花费因子";
            // 
            // SelStartPoi
            // 
            this.SelStartPoi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelStartPoi.FormattingEnabled = true;
            this.SelStartPoi.Items.AddRange(new object[] {
            "七星鲁王宫-山东瓜子庙（临沂蒙山）",
            "秦岭神树-秦始皇陵",
            "西沙海底墓-西沙碗礁",
            "西王母宫-青海柴达木盆地",
            "云顶天宫-吉林长白山三圣山",
            "张家古楼-广西省上思县南屏乡巴乃村"});
            this.SelStartPoi.Location = new System.Drawing.Point(7, 109);
            this.SelStartPoi.Name = "SelStartPoi";
            this.SelStartPoi.Size = new System.Drawing.Size(153, 26);
            this.SelStartPoi.TabIndex = 3;
            this.SelStartPoi.SelectedIndexChanged += new System.EventHandler(this.SelStartPoi_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "选择起点（墓穴名）";
            // 
            // btn_Dijskta
            // 
            this.btn_Dijskta.Location = new System.Drawing.Point(33, 251);
            this.btn_Dijskta.Name = "btn_Dijskta";
            this.btn_Dijskta.Size = new System.Drawing.Size(116, 40);
            this.btn_Dijskta.TabIndex = 0;
            this.btn_Dijskta.Text = "开始寻路";
            this.btn_Dijskta.UseVisualStyleBackColor = true;
            this.btn_Dijskta.Visible = false;
            this.btn_Dijskta.VisibleChanged += new System.EventHandler(this.btn_Dijskta_Click);
            this.btn_Dijskta.Click += new System.EventHandler(this.btn_Dijskta_Click);
            // 
            // checkDistance
            // 
            this.checkDistance.AutoSize = true;
            this.checkDistance.Enabled = false;
            this.checkDistance.Location = new System.Drawing.Point(7, 205);
            this.checkDistance.Name = "checkDistance";
            this.checkDistance.Size = new System.Drawing.Size(70, 22);
            this.checkDistance.TabIndex = 5;
            this.checkDistance.Text = "路程";
            this.checkDistance.UseVisualStyleBackColor = true;
            this.checkDistance.CheckedChanged += new System.EventHandler(this.checkDistance_CheckedChanged);
            // 
            // checkTime
            // 
            this.checkTime.AutoSize = true;
            this.checkTime.Enabled = false;
            this.checkTime.Location = new System.Drawing.Point(126, 205);
            this.checkTime.Name = "checkTime";
            this.checkTime.Size = new System.Drawing.Size(70, 22);
            this.checkTime.TabIndex = 6;
            this.checkTime.Text = "时间";
            this.checkTime.UseVisualStyleBackColor = true;
            this.checkTime.CheckedChanged += new System.EventHandler(this.checkTime_CheckedChanged);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1398, 542);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.axMapControl1);
            this.Name = "Form5";
            this.Text = "Form5";
            this.Load += new System.EventHandler(this.Form5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_Dijskta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox SelStartPoi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkTime;
        private System.Windows.Forms.CheckBox checkDistance;
    }
}