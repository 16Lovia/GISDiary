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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRoute));
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lab_cost = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lab_end = new System.Windows.Forms.Label();
            this.lab_start = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkTime = new System.Windows.Forms.CheckBox();
            this.checkDistance = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SelStartPoi = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lab_info = new System.Windows.Forms.Label();
            this.lab_costAll = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(0, 0);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(1334, 699);
            this.axMapControl1.TabIndex = 0;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
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
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.checkTime);
            this.panel1.Controls.Add(this.checkDistance);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.SelStartPoi);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.axLicenseControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1108, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 699);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lab_costAll);
            this.panel3.Controls.Add(this.lab_cost);
            this.panel3.Location = new System.Drawing.Point(3, 372);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(220, 66);
            this.panel3.TabIndex = 15;
            // 
            // lab_cost
            // 
            this.lab_cost.AutoSize = true;
            this.lab_cost.Location = new System.Drawing.Point(3, 34);
            this.lab_cost.Name = "lab_cost";
            this.lab_cost.Size = new System.Drawing.Size(80, 18);
            this.lab_cost.TabIndex = 5;
            this.lab_cost.Text = "详细信息";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(46, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 43);
            this.button1.TabIndex = 8;
            this.button1.Text = "开始寻路";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lab_info);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.lab_end);
            this.panel2.Controls.Add(this.lab_start);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(3, 261);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(223, 438);
            this.panel2.TabIndex = 7;
            this.panel2.Visible = false;
            // 
            // lab_end
            // 
            this.lab_end.AutoSize = true;
            this.lab_end.Location = new System.Drawing.Point(112, 116);
            this.lab_end.Name = "lab_end";
            this.lab_end.Size = new System.Drawing.Size(0, 18);
            this.lab_end.TabIndex = 12;
            // 
            // lab_start
            // 
            this.lab_start.AutoSize = true;
            this.lab_start.Location = new System.Drawing.Point(112, 76);
            this.lab_start.Name = "lab_start";
            this.lab_start.Size = new System.Drawing.Size(0, 18);
            this.lab_start.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 18);
            this.label5.TabIndex = 2;
            this.label5.Text = "终点";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 1;
            this.label4.Text = "起点";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "路径信息（点击查询）";
            // 
            // checkTime
            // 
            this.checkTime.AutoSize = true;
            this.checkTime.Enabled = false;
            this.checkTime.Location = new System.Drawing.Point(130, 166);
            this.checkTime.Name = "checkTime";
            this.checkTime.Size = new System.Drawing.Size(70, 22);
            this.checkTime.TabIndex = 6;
            this.checkTime.Text = "时间";
            this.checkTime.UseVisualStyleBackColor = true;
            this.checkTime.CheckedChanged += new System.EventHandler(this.checkTime_CheckedChanged);
            // 
            // checkDistance
            // 
            this.checkDistance.AutoSize = true;
            this.checkDistance.Enabled = false;
            this.checkDistance.Location = new System.Drawing.Point(3, 166);
            this.checkDistance.Name = "checkDistance";
            this.checkDistance.Size = new System.Drawing.Size(70, 22);
            this.checkDistance.TabIndex = 5;
            this.checkDistance.Text = "路程";
            this.checkDistance.UseVisualStyleBackColor = true;
            this.checkDistance.CheckedChanged += new System.EventHandler(this.checkDistance_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 128);
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
            this.SelStartPoi.Location = new System.Drawing.Point(21, 69);
            this.SelStartPoi.Name = "SelStartPoi";
            this.SelStartPoi.Size = new System.Drawing.Size(153, 26);
            this.SelStartPoi.TabIndex = 3;
            this.SelStartPoi.SelectedIndexChanged += new System.EventHandler(this.SelStartPoi_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "选择起点（墓穴名）";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 18);
            this.label6.TabIndex = 16;
            this.label6.Text = "途径省份";
            // 
            // lab_info
            // 
            this.lab_info.AutoSize = true;
            this.lab_info.Location = new System.Drawing.Point(70, 195);
            this.lab_info.Name = "lab_info";
            this.lab_info.Size = new System.Drawing.Size(0, 18);
            this.lab_info.TabIndex = 17;
            // 
            // lab_costAll
            // 
            this.lab_costAll.AutoSize = true;
            this.lab_costAll.Location = new System.Drawing.Point(106, 34);
            this.lab_costAll.Name = "lab_costAll";
            this.lab_costAll.Size = new System.Drawing.Size(0, 18);
            this.lab_costAll.TabIndex = 6;
            // 
            // FormRoute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 699);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.axMapControl1);
            this.Name = "FormRoute";
            this.Text = "Form5";
            this.Load += new System.EventHandler(this.Form5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox SelStartPoi;
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
    }
}