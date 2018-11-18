namespace GISDiary
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_2e = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button_IDW_TIME = new System.Windows.Forms.Button();
            this.button_kriging = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_dress = new System.Windows.Forms.Button();
            this.button_changeView = new System.Windows.Forms.Button();
            this.button_thematic = new System.Windows.Forms.Button();
            this.button_openmxd = new System.Windows.Forms.Button();
            this.button_atrribute = new System.Windows.Forms.Button();
            this.buttton_tiff = new System.Windows.Forms.Button();
            this.axPageLayoutControl1 = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(12, 0);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(331, 28);
            this.axToolbarControl1.TabIndex = 0;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Location = new System.Drawing.Point(2, 34);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(260, 409);
            this.axTOCControl1.TabIndex = 1;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(268, 34);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(331, 409);
            this.axMapControl1.TabIndex = 2;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnViewRefreshed += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnViewRefreshedEventHandler(this.axMapControl1_OnViewRefreshed);
            this.axMapControl1.OnAfterScreenDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterScreenDrawEventHandler(this.axMapControl1_OnAfterScreenDraw);
            this.axMapControl1.OnMapReplaced += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMapReplacedEventHandler(this.axMapControl1_OnMapReplaced);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_2e);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.button_IDW_TIME);
            this.panel1.Controls.Add(this.button_kriging);
            this.panel1.Controls.Add(this.button_save);
            this.panel1.Controls.Add(this.button_dress);
            this.panel1.Controls.Add(this.button_changeView);
            this.panel1.Controls.Add(this.button_thematic);
            this.panel1.Controls.Add(this.button_openmxd);
            this.panel1.Controls.Add(this.button_atrribute);
            this.panel1.Controls.Add(this.buttton_tiff);
            this.panel1.Location = new System.Drawing.Point(626, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(308, 545);
            this.panel1.TabIndex = 3;
            // 
            // btn_2e
            // 
            this.btn_2e.Location = new System.Drawing.Point(170, 313);
            this.btn_2e.Name = "btn_2e";
            this.btn_2e.Size = new System.Drawing.Size(111, 49);
            this.btn_2e.TabIndex = 10;
            this.btn_2e.Text = "寻找最可能的点";
            this.btn_2e.UseVisualStyleBackColor = true;
            this.btn_2e.Click += new System.EventHandler(this.btn_2e_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(19, 392);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(262, 150);
            this.dataGridView1.TabIndex = 9;
            // 
            // button_IDW_TIME
            // 
            this.button_IDW_TIME.Location = new System.Drawing.Point(19, 313);
            this.button_IDW_TIME.Name = "button_IDW_TIME";
            this.button_IDW_TIME.Size = new System.Drawing.Size(118, 49);
            this.button_IDW_TIME.TabIndex = 8;
            this.button_IDW_TIME.Text = "IDW_距今时间";
            this.button_IDW_TIME.UseVisualStyleBackColor = true;
            this.button_IDW_TIME.Click += new System.EventHandler(this.button_IDW_TIME_Click);
            // 
            // button_kriging
            // 
            this.button_kriging.Location = new System.Drawing.Point(170, 241);
            this.button_kriging.Name = "button_kriging";
            this.button_kriging.Size = new System.Drawing.Size(111, 50);
            this.button_kriging.TabIndex = 7;
            this.button_kriging.Text = "IDW_面积";
            this.button_kriging.UseVisualStyleBackColor = true;
            this.button_kriging.Click += new System.EventHandler(this.button_kriging_Click);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(19, 241);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(118, 50);
            this.button_save.TabIndex = 6;
            this.button_save.Text = "将布局视图保存为图片";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_dress
            // 
            this.button_dress.Location = new System.Drawing.Point(170, 173);
            this.button_dress.Name = "button_dress";
            this.button_dress.Size = new System.Drawing.Size(111, 49);
            this.button_dress.TabIndex = 5;
            this.button_dress.Text = "图幅整饰";
            this.button_dress.UseVisualStyleBackColor = true;
            this.button_dress.Click += new System.EventHandler(this.button_dress_Click);
            // 
            // button_changeView
            // 
            this.button_changeView.Location = new System.Drawing.Point(19, 173);
            this.button_changeView.Name = "button_changeView";
            this.button_changeView.Size = new System.Drawing.Size(118, 49);
            this.button_changeView.TabIndex = 4;
            this.button_changeView.Text = "切换到布局视图";
            this.button_changeView.UseVisualStyleBackColor = true;
            this.button_changeView.Click += new System.EventHandler(this.button_changeView_Click);
            // 
            // button_thematic
            // 
            this.button_thematic.Location = new System.Drawing.Point(170, 94);
            this.button_thematic.Name = "button_thematic";
            this.button_thematic.Size = new System.Drawing.Size(111, 49);
            this.button_thematic.TabIndex = 3;
            this.button_thematic.Text = "生成专题图";
            this.button_thematic.UseVisualStyleBackColor = true;
            this.button_thematic.Click += new System.EventHandler(this.button_thematic_Click);
            // 
            // button_openmxd
            // 
            this.button_openmxd.Location = new System.Drawing.Point(19, 94);
            this.button_openmxd.Name = "button_openmxd";
            this.button_openmxd.Size = new System.Drawing.Size(118, 49);
            this.button_openmxd.TabIndex = 2;
            this.button_openmxd.Text = "打开mxd文件";
            this.button_openmxd.UseVisualStyleBackColor = true;
            this.button_openmxd.Click += new System.EventHandler(this.button_openmxd_Click);
            // 
            // button_atrribute
            // 
            this.button_atrribute.Location = new System.Drawing.Point(170, 21);
            this.button_atrribute.Name = "button_atrribute";
            this.button_atrribute.Size = new System.Drawing.Size(111, 52);
            this.button_atrribute.TabIndex = 1;
            this.button_atrribute.Text = "打开要素的属性表";
            this.button_atrribute.UseVisualStyleBackColor = true;
            this.button_atrribute.Click += new System.EventHandler(this.button_atrribute_Click);
            // 
            // buttton_tiff
            // 
            this.buttton_tiff.Location = new System.Drawing.Point(19, 21);
            this.buttton_tiff.Name = "buttton_tiff";
            this.buttton_tiff.Size = new System.Drawing.Size(118, 51);
            this.buttton_tiff.TabIndex = 0;
            this.buttton_tiff.Text = "打开tiff文件";
            this.buttton_tiff.UseVisualStyleBackColor = true;
            this.buttton_tiff.Click += new System.EventHandler(this.buttton_tiff_Click);
            // 
            // axPageLayoutControl1
            // 
            this.axPageLayoutControl1.Location = new System.Drawing.Point(972, 34);
            this.axPageLayoutControl1.Name = "axPageLayoutControl1";
            this.axPageLayoutControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl1.OcxState")));
            this.axPageLayoutControl1.Size = new System.Drawing.Size(331, 481);
            this.axPageLayoutControl1.TabIndex = 4;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(553, 469);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 528);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "选中位置的坐标（米）：";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(1422, 151);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 25);
            this.numericUpDown1.TabIndex = 9;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1589, 591);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axPageLayoutControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.axToolbarControl1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_atrribute;
        private System.Windows.Forms.Button buttton_tiff;
        private System.Windows.Forms.Button button_thematic;
        private System.Windows.Forms.Button button_openmxd;
        private System.Windows.Forms.Button button_dress;
        private System.Windows.Forms.Button button_changeView;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_kriging;
        private System.Windows.Forms.Button button_IDW_TIME;
        private System.Windows.Forms.DataGridView dataGridView1;
        private ESRI.ArcGIS.Controls.AxPageLayoutControl axPageLayoutControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_2e;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}