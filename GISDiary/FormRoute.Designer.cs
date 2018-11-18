using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GISDiary
{
    partial class FormRoute
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

        private double ConvertPixelToMapUnits(IActiveView activeView, double pixelUnits)
        {
            double realWorldDiaplayExtent;
            int pixelExtent;
            double sizeOfOnePixel;
            double mapUnits;

            //获取设备中视图显示宽度，即像素个数
            pixelExtent = activeView.ScreenDisplay.DisplayTransformation.get_DeviceFrame().right - activeView.ScreenDisplay.DisplayTransformation.get_DeviceFrame().left;
            //获取地图坐标系中地图显示范围
            realWorldDiaplayExtent = activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            //每个像素大小代表的实际距离
            sizeOfOnePixel = realWorldDiaplayExtent / pixelExtent;
            //地理距离
            mapUnits = pixelUnits * sizeOfOnePixel;

            return mapUnits;
        }
        /// <summary>
        /// 空间查询
        /// </summary>
        /// <param name="mapControl">MapControl</param>
        /// <param name="geometry">空间查询方式</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>查询得到的要素名称</returns>
        private string QuerySpatial(AxMapControl mapControl, IGeometry geometry, string fieldName)
        {
            //本例添加一个图层进行查询，多个图层时返回
            //if (mapControl.LayerCount > 1)
            //return null;

            //清除已有选择
            mapControl.Map.ClearSelection();

            //查询得到的要素名称
            string strNames = null;

            IFeatureLayer pFeatureLayer;
            IFeatureClass pFeatureClass;
            //获取图层和要素类，为空时返回
            pFeatureLayer = mapControl.Map.get_Layer(2) as IFeatureLayer;
            pFeatureClass = pFeatureLayer.FeatureClass;
            if (pFeatureClass == null)
                return null;

            //初始化空间过滤器
            ISpatialFilter pSpatialFilter;
            pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = geometry;
            //根据图层类型选择缓冲方式
            switch (pFeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    break;
            }
            //定义空间过滤器的空间字段
            pSpatialFilter.GeometryField = pFeatureClass.ShapeFieldName;

            IQueryFilter pQueryFilter;
            IFeatureCursor pFeatureCursor;
            IFeature pFeature;
            //利用要素过滤器查询要素
            pQueryFilter = pSpatialFilter as IQueryFilter;
            pFeatureCursor = pFeatureLayer.Search(pQueryFilter, true);
            pFeature = pFeatureCursor.NextFeature();

            int fieldIndex;
            while (pFeature != null)
            {
                //选择指定要素
                fieldIndex = pFeature.Fields.FindField(fieldName);
                //获取要素名称
                strNames = strNames + pFeature.get_Value(fieldIndex) + "\n\n";
                //高亮选中要素
                mapControl.Map.SelectFeature((ILayer)pFeatureLayer, pFeature);
                mapControl.ActiveView.Refresh();
                pFeature = pFeatureCursor.NextFeature();
            }

            return strNames;
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRoute));
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SelStartPoi = new CCWin.SkinControl.SkinComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lab_info = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lab_costAll = new System.Windows.Forms.Label();
            this.lab_cost = new System.Windows.Forms.Label();
            this.lab_end = new System.Windows.Forms.Label();
            this.lab_start = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkTime = new System.Windows.Forms.CheckBox();
            this.checkDistance = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.skinToolTip1 = new CCWin.SkinToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(4, 34);
            this.axMapControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(1029, 583);
            this.axMapControl1.TabIndex = 0;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(810, 51);
            this.axLicenseControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.SelStartPoi);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.checkTime);
            this.panel1.Controls.Add(this.checkDistance);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(857, 34);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(176, 583);
            this.panel1.TabIndex = 2;
            // 
            // SelStartPoi
            // 
            this.SelStartPoi.ArrowColor = System.Drawing.Color.White;
            this.SelStartPoi.BaseColor = System.Drawing.Color.Black;
            this.SelStartPoi.BorderColor = System.Drawing.Color.Black;
            this.SelStartPoi.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.SelStartPoi.FormattingEnabled = true;
            this.SelStartPoi.Items.AddRange(new object[] {
            "七星鲁王宫-山东瓜子庙（临沂蒙山）",
            "秦岭神树-秦始皇陵",
            "西沙海底墓-西沙碗礁",
            "西王母宫-青海柴达木盆地",
            "云顶天宫-吉林长白山三圣山",
            "张家古楼-广西省上思县南屏乡巴乃村"});
            this.SelStartPoi.Location = new System.Drawing.Point(30, 47);
            this.SelStartPoi.Name = "SelStartPoi";
            this.SelStartPoi.Size = new System.Drawing.Size(119, 24);
            this.SelStartPoi.TabIndex = 3;
            this.SelStartPoi.WaterText = "";
            this.SelStartPoi.SelectedIndexChanged += new System.EventHandler(this.skinComboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(47, 174);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 39);
            this.button1.TabIndex = 8;
            this.button1.Text = "开始寻路";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.lab_info);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.lab_end);
            this.panel2.Controls.Add(this.lab_start);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(2, 232);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(174, 389);
            this.panel2.TabIndex = 7;
            this.panel2.Visible = false;
            // 
            // lab_info
            // 
            this.lab_info.AutoSize = true;
            this.lab_info.ForeColor = System.Drawing.Color.White;
            this.lab_info.Location = new System.Drawing.Point(55, 173);
            this.lab_info.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lab_info.Name = "lab_info";
            this.lab_info.Size = new System.Drawing.Size(0, 16);
            this.lab_info.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(5, 140);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "途径省份";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lab_costAll);
            this.panel3.Controls.Add(this.lab_cost);
            this.panel3.Location = new System.Drawing.Point(2, 331);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(171, 59);
            this.panel3.TabIndex = 15;
            // 
            // lab_costAll
            // 
            this.lab_costAll.AutoSize = true;
            this.lab_costAll.Location = new System.Drawing.Point(83, 31);
            this.lab_costAll.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lab_costAll.Name = "lab_costAll";
            this.lab_costAll.Size = new System.Drawing.Size(0, 16);
            this.lab_costAll.TabIndex = 6;
            // 
            // lab_cost
            // 
            this.lab_cost.AutoSize = true;
            this.lab_cost.Location = new System.Drawing.Point(2, 31);
            this.lab_cost.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lab_cost.Name = "lab_cost";
            this.lab_cost.Size = new System.Drawing.Size(56, 16);
            this.lab_cost.TabIndex = 5;
            this.lab_cost.Text = "详细信息";
            // 
            // lab_end
            // 
            this.lab_end.AutoSize = true;
            this.lab_end.ForeColor = System.Drawing.Color.White;
            this.lab_end.Location = new System.Drawing.Point(87, 103);
            this.lab_end.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lab_end.Name = "lab_end";
            this.lab_end.Size = new System.Drawing.Size(0, 16);
            this.lab_end.TabIndex = 12;
            // 
            // lab_start
            // 
            this.lab_start.AutoSize = true;
            this.lab_start.ForeColor = System.Drawing.Color.White;
            this.lab_start.Location = new System.Drawing.Point(87, 68);
            this.lab_start.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lab_start.Name = "lab_start";
            this.lab_start.Size = new System.Drawing.Size(0, 16);
            this.lab_start.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(14, 103);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "终点";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 68);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "起点";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("汉仪星宇体简", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(50, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "路径信息";
            this.label3.MouseHover += new System.EventHandler(this.label3_MouseHover);
            // 
            // checkTime
            // 
            this.checkTime.AutoSize = true;
            this.checkTime.BackColor = System.Drawing.Color.Transparent;
            this.checkTime.Enabled = false;
            this.checkTime.ForeColor = System.Drawing.Color.White;
            this.checkTime.Location = new System.Drawing.Point(110, 146);
            this.checkTime.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.checkTime.Name = "checkTime";
            this.checkTime.Size = new System.Drawing.Size(51, 20);
            this.checkTime.TabIndex = 6;
            this.checkTime.Text = "时间";
            this.checkTime.UseVisualStyleBackColor = false;
            this.checkTime.CheckedChanged += new System.EventHandler(this.checkTime_CheckedChanged);
            // 
            // checkDistance
            // 
            this.checkDistance.AutoSize = true;
            this.checkDistance.BackColor = System.Drawing.Color.Transparent;
            this.checkDistance.Enabled = false;
            this.checkDistance.ForeColor = System.Drawing.Color.White;
            this.checkDistance.Location = new System.Drawing.Point(36, 146);
            this.checkDistance.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.checkDistance.Name = "checkDistance";
            this.checkDistance.Size = new System.Drawing.Size(51, 20);
            this.checkDistance.TabIndex = 5;
            this.checkDistance.Text = "路程";
            this.checkDistance.UseVisualStyleBackColor = false;
            this.checkDistance.CheckedChanged += new System.EventHandler(this.checkDistance_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("汉仪星宇体简", 8.999999F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(54, 114);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "选择花费因子";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(33, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "选择起点（墓穴名）";
            // 
            // skinToolTip1
            // 
            this.skinToolTip1.AutoPopDelay = 5000;
            this.skinToolTip1.BackColor2 = System.Drawing.Color.White;
            this.skinToolTip1.Border = System.Drawing.Color.White;
            this.skinToolTip1.InitialDelay = 500;
            this.skinToolTip1.OwnerDraw = true;
            this.skinToolTip1.ReshowDelay = 800;
            this.skinToolTip1.TipFore = System.Drawing.Color.Black;
            this.skinToolTip1.TitleFore = System.Drawing.Color.Gold;
            // 
            // FormRoute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GISDiary.Properties.Resources.展示;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CaptionFont = new System.Drawing.Font("汉仪星宇体简", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ClientSize = new System.Drawing.Size(1037, 621);
            this.CloseNormlBack = global::GISDiary.Properties.Resources.剑hei;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axLicenseControl1);
            this.Font = new System.Drawing.Font("汉仪星宇体简", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "FormRoute";
            this.Text = "盗墓笔记";
            this.Load += new System.EventHandler(this.Form5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkTime;
        private System.Windows.Forms.CheckBox checkDistance;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lab_cost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lab_end;
        private System.Windows.Forms.Label lab_start;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lab_info;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lab_costAll;
        private CCWin.SkinToolTip skinToolTip1;
        private CCWin.SkinControl.SkinComboBox SelStartPoi;
    }
}