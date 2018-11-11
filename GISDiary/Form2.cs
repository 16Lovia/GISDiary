using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Output;
using System.Threading;
using stdole;
using ESRI.ArcGIS.ADF.COMSupport;
using ESRI.ArcGIS.AnalysisTools;         //添加引用
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.SpatialAnalystTools;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.GeoAnalyst;

namespace GISDiary
{
    public partial class Form2 : Form
    {
        private string tiffPath = "D:\\users\\lenovo\\documents\\visual studio 2015\\Projects\\Teamwork\\Teamwork\\raster\\last.tif";
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
        }
        private void buttton_tiff_Click(object sender, EventArgs e)
        {
            axMapControl1.ClearLayers();
            //string tiffPath = "tiff文件/未命名.tiff";D://users//lenovo//documents//visual studio 2015//Projects//Teamwork//Teamwork//tiff文件
            if (tiffPath == "")
                return;

            int Index = tiffPath.LastIndexOf("\\");
            string fileName = tiffPath.Substring(Index + 1);
            string filePath = tiffPath.Substring(0, Index);

            IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactoryClass();//利用工厂对象去生成一个raster文件的工作空间
            IRasterWorkspace pRasterWorkspace = (IRasterWorkspace)pWorkspaceFactory.OpenFromFile(filePath, 0);//到指定路径下
            IRasterDataset pRasterDataset = (IRasterDataset)pRasterWorkspace.OpenRasterDataset(fileName);//利用要素集去接收对应的raster文件

            IRasterLayer pRasterLayer = new RasterLayerClass();//生成一个矢量图层对象
            pRasterLayer.CreateFromDataset(pRasterDataset);//利用矢量图层对象去创建对应的raster文件
            axMapControl1.Map.AddLayer(pRasterLayer);//添加对应的图层
            axMapControl1.ActiveView.Refresh();
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            label1.Text = " 当前坐标 X = " + e.mapX.ToString() + " Y = " + e.mapY.ToString() + " " + this.axMapControl1.MapUnits.ToString().Substring(4);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void button_thematic_Click(object sender, EventArgs e)
        {
            // IFeatureClass pFeatureClass = GetFeatureClass("D://a_gis工程设计实践课//shp//POI.shp");
            //DataTable dataTable = GetAttributesTable(pFeatureClass);
            try
            {
                //获取目标图层   IFeatureClass pFeatureClass = GetFeatureClass("D://a_gis工程设计实践课//shp//POI.shp");
                ILayer pLayer = new FeatureLayerClass();
                pLayer = axMapControl1.get_Layer(0);
                IGeoFeatureLayer pGeoFeatLyr = pLayer as IGeoFeatureLayer;//目标图层的feature*/

                /* IFeatureClass pFeatureClass = GetFeatureClass("D://a_gis工程设计实践课//shp//POI.shp");
                  //pLayer = new FeatureLayerClass();
                 ILayer pLayer = pFeatureClass as ILayer;
                 IGeoFeatureLayer pGeoFeatLyr = pLayer as IGeoFeatureLayer;//目标图层的feature*/


                ITable table;
                ICursor cursor;
                IDataStatistics dataStatistics;//用一个字段生成统计数据
                IStatisticsResults statisticsResult;//报告统计数据 //stdole.IFontDisp fontDisp;//定义字体
                                                    //设置点符号
                ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbol();
                pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;//设置点符号样式为方形
                IRgbColor pRgbColor1 = new RgbColor();
                pRgbColor1.Red = 127;
                pRgbColor1.Blue = 255;
                pRgbColor1.Green = 212;
                pMarkerSymbol.Color = pRgbColor1;
                table = pLayer as ITable;
                cursor = table.Search(null, true);
                dataStatistics = new DataStatisticsClass();
                dataStatistics.Cursor = cursor;
                dataStatistics.Field = "难度";//确定分级字段
                statisticsResult = dataStatistics.Statistics;
                if (statisticsResult != null)
                {
                    IFillSymbol fillSymbol = new SimpleFillSymbolClass();
                    fillSymbol.Color = pRgbColor1;
                    IProportionalSymbolRenderer proportionalSymbolRenderer = new ProportionalSymbolRendererClass();
                    //proportionalSymbolRenderer.ValueUnit = units;
                    proportionalSymbolRenderer.Field = "难度";
                    proportionalSymbolRenderer.FlanneryCompensation = false;//分级是不是在TOC中显示legend
                    proportionalSymbolRenderer.MinDataValue = statisticsResult.Minimum;//
                    proportionalSymbolRenderer.MaxDataValue = statisticsResult.Maximum;//
                    // proportionalSymbolRenderer.BackgroundSymbol = fillSymbol;

                    pMarkerSymbol.Size = 15;
                    proportionalSymbolRenderer.MinSymbol = pMarkerSymbol as ISymbol;
                    proportionalSymbolRenderer.LegendSymbolCount = 6;//要分成的级数
                    proportionalSymbolRenderer.CreateLegendSymbols();
                    pGeoFeatLyr.Renderer = proportionalSymbolRenderer as IFeatureRenderer;

                    //axMapControl1.Update();
                    //axMapControl1.Refresh();
                    axTOCControl1.Update();
                    //Thread.Sleep(2000); //停一秒

                    for (int i = 0; i < 4; i++)
                    {
                        pMarkerSymbol.Size = 3 * (i + 1);
                        proportionalSymbolRenderer.MinSymbol = pMarkerSymbol as ISymbol;
                        //proportionalSymbolRenderer.LegendSymbolCount = 6;//要分成的级数
                        proportionalSymbolRenderer.CreateLegendSymbols();
                        pGeoFeatLyr.Renderer = proportionalSymbolRenderer as IFeatureRenderer;

                        axMapControl1.Update();
                        axMapControl1.Refresh();
                        //axTOCControl1.Update();
                        Thread.Sleep(500); //停一秒


                    }


                    /*pMarkerSymbol.Size = 10;
                    proportionalSymbolRenderer.MinSymbol = pMarkerSymbol as ISymbol;
                    pGeoFeatLyr.Renderer = proportionalSymbolRenderer as IFeatureRenderer;
                    axMapControl1.Refresh();
                    axTOCControl1.Update();*/
                }


            }
            catch
            {
                MessageBox.Show("请输入有效图层!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void button_openmxd_Click(object sender, EventArgs e)
        {

            IMapDocument xjMxdMapDocument = new MapDocumentClass();
            OpenFileDialog xjMxdOpenFileDialog = new OpenFileDialog();
            xjMxdOpenFileDialog.Filter = "地图文档(*.mxd)|*.mxd";


            if (xjMxdOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string xjmxdFilePath = xjMxdOpenFileDialog.FileName;
                if (axMapControl1.CheckMxFile(xjmxdFilePath))
                {
                    axMapControl1.Map.ClearLayers();
                    axMapControl1.LoadMxFile(xjmxdFilePath);
                }
            }
            axMapControl1.ActiveView.Refresh();
        }
        private IGeometry getGeometry(IPointCollection Points)
        {
            IPointCollection iPointCollection = new PolygonClass();

            Ring ring = new RingClass();
            object missing = Type.Missing;

            ring.AddPointCollection(Points);

            IGeometryCollection pointPolygon = new PolygonClass();
            pointPolygon.AddGeometry(ring as IGeometry, ref missing, ref missing);
            IPolygon polyGonGeo = pointPolygon as IPolygon;
            //polyGonGeo.Close();
            polyGonGeo.SimplifyPreserveFromTo();
            return polyGonGeo as IGeometry;

        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
        }
        /// <summary>
        /// 获取要素类
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static IFeatureClass GetFeatureClass(string filePath)
        {
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspaceFactoryLockControl pWorkspaceFactoryLockControl = pWorkspaceFactory as IWorkspaceFactoryLockControl;
            if (pWorkspaceFactoryLockControl.SchemaLockingEnabled)
            {
                pWorkspaceFactoryLockControl.DisableSchemaLocking();
            }
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath), 0);
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(System.IO.Path.GetFileName(filePath));
            return pFeatureClass;
        }

        /// <summary>
        /// 获取要素属性表
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <returns></returns>
        private static DataTable GetAttributesTable(IFeatureClass pFeatureClass)
        {
            string geometryType = string.Empty;
            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
            {
                geometryType = "点";
            }
            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryMultipoint)
            {
                geometryType = "点集";
            }
            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
            {
                geometryType = "折线";
            }
            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                geometryType = "面";
            }

            // 字段集合
            IFields pFields = pFeatureClass.Fields;
            int fieldsCount = pFields.FieldCount;

            // 写入字段名
            DataTable dataTable = new DataTable();
            for (int i = 0; i < fieldsCount; i++)
            {
                dataTable.Columns.Add(pFields.get_Field(i).Name);
            }

            // 要素游标
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, true);
            IFeature pFeature = pFeatureCursor.NextFeature();
            if (pFeature == null)
            {
                return dataTable;
            }

            // 获取MZ值
            IMAware pMAware = pFeature.Shape as IMAware;
            IZAware pZAware = pFeature.Shape as IZAware;
            if (pMAware.MAware)
            {
                geometryType += " M";
            }
            if (pZAware.ZAware)
            {
                geometryType += "Z";
            }

            // 写入字段值
            while (pFeature != null)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int i = 0; i < fieldsCount; i++)
                {
                    if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        dataRow[i] = geometryType;
                    }
                    else
                    {
                       // dataRow[i] = pFeature.get_Value(i).ToString();
                    }
                }
                dataTable.Rows.Add(dataRow);
                pFeature = pFeatureCursor.NextFeature();
            }

            // 释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            return dataTable;
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_atrribute_Click(object sender, EventArgs e)
        {
            IFeatureClass pFeatureClass = GetFeatureClass("D://a_gis工程设计实践课//墓穴地shp//GravePoint.shp");
            DataTable dataTable = GetAttributesTable(pFeatureClass);
            dataGridView1.DataSource = dataTable;

            //从数据库获取该站点的经纬度坐标
            IPoint point = new PointClass();
            point.X = Convert.ToDouble(dataTable.Rows[0]["经度"].ToString());//X经度
            point.Y = Convert.ToDouble(dataTable.Rows[0]["纬度"].ToString());//Y纬度

            //label1.Text = dataTable.Rows[0].ItemArray[2];
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button_dress_Click(object sender, EventArgs e)
        {
            //获取axPageLayoutControl1的图形容器
            IGraphicsContainer graphicsContainer =
      axPageLayoutControl1.GraphicsContainer;
            //获取axPageLayoutControl1空间里面显示的地图图层
            IMapFrame mapFrame =
      (IMapFrame)graphicsContainer.FindFrame(axPageLayoutControl1.ActiveView.FocusMap);
            if (mapFrame == null) return;
            //--------------创建图例------------
            UID uID = new UIDClass();//创建UID作为该图例的唯一标识符，方便创建之后进行删除、移动等操作
            uID.Value = "esriCarto.Legend";
            IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(uID, null);
            if (mapSurroundFrame == null) return;
            if (mapSurroundFrame.MapSurround == null) return;
            mapSurroundFrame.MapSurround.Name = "图例";
            IEnvelope envelope = new EnvelopeClass();
            envelope.PutCoords(16, 2, 18, 7);//设置图例摆放位置（原点在axPageLayoutControl左下角）

            IElement element = (IElement)mapSurroundFrame;
            element.Geometry = envelope;
            //将图例转化为几何要素添加到axPageLayoutControl1,并刷新页面显示
            axPageLayoutControl1.AddElement(element, Type.Missing, Type.Missing,
      "Legend", 0);
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null,
            null);
            //-----------设置指北针--------
            IMapSurround pMapSurround;
            INorthArrow pNorthArrow;
            pNorthArrow = new MarkerNorthArrowClass();//创建指北针的实例
            pMapSurround = pNorthArrow;
            pMapSurround.Name = "NorthArrow";
            //定义UID
            UID uid = new UIDClass();
            uid.Value = "esriCarto.MarkerNorthArrow";
            //定义MapSurroundFrame对象
            IMapSurroundFrame pMapSurroundFrame = mapFrame.CreateSurroundFrame(uid, null);
            pMapSurroundFrame.MapSurround = pMapSurround;
            IElement pDeletElement = axPageLayoutControl1.FindElementByName("NorthArrow");//获取PageLayout中的图例元素
            if (pDeletElement != null)
            {
                graphicsContainer.DeleteElement(pDeletElement);  //如果已经存在指北针，删除已经存在的指北针
            }
            //定义Envelope设置Element摆放的位置
            IEnvelope pEnvelope = new EnvelopeClass();
            pEnvelope.PutCoords(16, 24, 21, 32);
            IElement pElement = pMapSurroundFrame as IElement;
            pElement.Geometry = pEnvelope;
            graphicsContainer.AddElement(pElement, 0);
            //刷新axPageLayoutControl1的内容
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            //-----------设置比例尺------------
            IActiveView pActiveView = axPageLayoutControl1.PageLayout as IActiveView;
            IMap pMap = pActiveView.FocusMap as IMap;
            IGraphicsContainer pGraphicsContainer = pActiveView as IGraphicsContainer;
            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame;
            //IMapSurround pMapSurround;
            //设置比例尺样式
            IScaleBar pScaleBar = new ScaleLineClass();
            pScaleBar.Units = esriUnits.esriKilometers;
            pScaleBar.Divisions = 4;
            pScaleBar.Subdivisions = 3;
            pScaleBar.DivisionsBeforeZero = 0;
            pScaleBar.UnitLabel = "km";
            pScaleBar.LabelPosition = esriVertPosEnum.esriBelow;
            pScaleBar.LabelGap = 3.6;
            pScaleBar.LabelFrequency = esriScaleBarFrequency.esriScaleBarDivisionsAndFirstMidpoint;
            pScaleBar.LabelPosition = esriVertPosEnum.esriBelow;
            ITextSymbol pTextsymbol = new TextSymbolClass();
            pTextsymbol.Size = 1;
            stdole.StdFont pFont = new stdole.StdFont();
            pFont.Size = 3;
            pFont.Name = "Arial";
            pTextsymbol.Font = pFont as stdole.IFontDisp;
            pTextsymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            pScaleBar.UnitLabelSymbol = pTextsymbol;
            pScaleBar.LabelSymbol = pTextsymbol;
            INumericFormat pNumericFormat = new NumericFormatClass();
            pNumericFormat.AlignmentWidth = 0;
            pNumericFormat.RoundingOption = esriRoundingOptionEnum.esriRoundNumberOfSignificantDigits;
            pNumericFormat.RoundingValue = 0;
            pNumericFormat.UseSeparator = true;
            pNumericFormat.ShowPlusSign = false;
            //定义UID
            pMapSurround = pScaleBar;
            pMapSurround.Name = "ScaleBar";
            // UID uid = new UIDClass();
            uid.Value = "esriCarto.ScaleLine";
            //定义MapSurroundFrame对象IMapSurroundFrame
            pMapSurroundFrame = pMapFrame.CreateSurroundFrame(uid, null);
            pMapSurroundFrame.MapSurround = pMapSurround;
            //定义Envelope设置Element摆放的位置
            //IEnvelope pEnvelope = new EnvelopeClass();
            pEnvelope.PutCoords(8, 2, 14, 4);//IElement 
            pElement = pMapSurroundFrame as IElement;
            pElement.Geometry = pEnvelope;//IElement 
            pDeletElement = axPageLayoutControl1.FindElementByName("ScaleBar");//获取PageLayout中的比例尺元素
            if (pDeletElement != null)
            {
                pGraphicsContainer.DeleteElement(pDeletElement);  //如果已经存在比例尺，删除已经存在的比例尺
            }
            pGraphicsContainer.AddElement(pElement, 0);
            //刷新axPageLayoutControl1的内容

            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            //---------------添加标题-------------
            //IGraphicsContainer graphicsContainer = axPageStation.PageLayout as IGraphicsContainer;
            //IEnvelope envelope = new EnvelopeClass();
            envelope.PutCoords(-14, 26, 35, 26);
            IRgbColor pColor = new RgbColorClass()
            {
                Red = 0,
                Blue = 0,
                Green = 0
            };
            pFont.Name = "宋体";
            pFont.Bold = true;
            ITextSymbol pTextSymbol = new TextSymbolClass()
            {
                Color = pColor,
                //Font = pFont,
                Size = 30
            };
            ITextElement pTextElement = new TextElementClass()
            {
                Symbol = pTextSymbol,
                ScaleText = true,
                Text = "盗墓难度专题图"
            };
            element = pTextElement as ESRI.ArcGIS.Carto.IElement;
            element.Geometry = envelope;
            graphicsContainer.AddElement(element, 0);
            axPageLayoutControl1.Refresh();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void axPageLayoutControl1_OnPageLayoutReplaced(object sender, IPageLayoutControlEvents_OnPageLayoutReplacedEvent e)
        {

        }
        public void CopyAndWriteMap()
        {
            IObjectCopy objectCopy = new ObjectCopyClass();
            object toCopyMap = axMapControl1.Map;
            object copiedMap = objectCopy.Copy(toCopyMap);// 把axMapControl1.Map定义为toCopyMap，然后复制到copiedMap中
            object toOverwriteMap = axPageLayoutControl1.ActiveView.FocusMap;
            objectCopy.Overwrite(copiedMap, ref toOverwriteMap);
            axPageLayoutControl1.ActiveView.Refresh();
        }
        public void repGeoMap()
        {
            IActiveView pActiveView = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
            IDisplayTransformation displayTransformation = pActiveView.ScreenDisplay.DisplayTransformation;

            displayTransformation.VisibleBounds = axMapControl1.Extent;
            axPageLayoutControl1.ActiveView.Refresh();
            CopyAndWriteMap();
        }
        bool strUnion = false;
        private void button_changeView_Click(object sender, EventArgs e)
        {
            strUnion = true;
            CopyAndWriteMap();
        }

        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            if (strUnion == false)
                return;
            repGeoMap();
        }

        private void axMapControl1_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            if (strUnion == false)
                return;
            CopyAndWriteMap();
        }

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            //TbGeoMap();
            if (strUnion == false)
                return;
            CopyAndWriteMap();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            IActiveView docActiveView = axPageLayoutControl1.ActiveView;
            IExport docExport = new ExportJPEGClass();
            IPrintAndExport docPrintExport = new PrintAndExportClass();
            int iOutputResolution = 300;
            //设置输出文件名
            docExport.ExportFileName = "D:\\a_gis工程设计实践课\\盗墓难度专题图.JPG";
            //输出当前视图到输出文件
            docPrintExport.Export(docActiveView, docExport, iOutputResolution, true, null);
        }

        private void button_kriging_Click(object sender, EventArgs e)
        {

            //  IFeatureLayer pfeaturelayer = (IFeatureLayer)this.axMapControl1.get_Layer(0);
            //IFeatureClass pfeatureclass = pfeaturelayer.FeatureClass;

            //1.将Shape文件读取成FeatureClass
            //2.根据FeatureClass生成IFeatureClassDescriptor 
            //3.创建IRasterRaduis 对象
            //设置Cell
            //4.插值并生成表面
            object obj = null;
            /* IFeatureLayer pfeaturelayer = (IFeatureLayer)this.axMapControl1.get_Layer(0);
             IFeatureClass featureClass = pfeaturelayer.FeatureClass;
             IGeoDataset geo = featureClass as IGeoDataset;*/

            ILayer pLayer = new FeatureLayerClass();
            IFeatureClass featureClass = GetFeatureClass("D://a_gis工程设计实践课//墓穴地shp//grave.shp");
            IGeoDataset geo = featureClass as IGeoDataset;

            object extend = geo.Extent;
            object o = null;
            IFeatureClassDescriptor feades = new FeatureClassDescriptorClass();
            feades.Create(featureClass, null, "area");
            IRasterRadius rasterrad = new RasterRadiusClass();
            rasterrad.SetVariable(12, ref obj);
            object dCell = 0.014800000;//可以根据不同的点图层进行设置
            IInterpolationOp interpla = new RasterInterpolationOpClass();
            IRasterAnalysisEnvironment rasanaenv = interpla as IRasterAnalysisEnvironment;
            rasanaenv.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, ref dCell);
            rasanaenv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, ref extend, ref o);
            IGeoDataset g_GeoDS_Raster = interpla.IDW((IGeoDataset)feades, 2, rasterrad, ref obj);
            IRaster pOutRsater = (IRaster)g_GeoDS_Raster;
            IRasterLayer pOutRasterlayer = new RasterLayerClass();
            pOutRasterlayer.CreateFromRaster(pOutRsater);
            pOutRasterlayer.Name = "面积大小";

            IRasterClassifyColorRampRenderer pRClassRend = new RasterClassifyColorRampRendererClass();
            IRasterRenderer pRRend = pRClassRend as IRasterRenderer;

            IRaster pRaster = pOutRasterlayer.Raster;
            IRasterBandCollection pRBandCol = pRaster as IRasterBandCollection;
            IRasterBand pRBand = pRBandCol.Item(0);
            if (pRBand.Histogram == null)
            {
                pRBand.ComputeStatsAndHist();
            }
            pRRend.Raster = pRaster;
            pRClassRend.ClassCount = 10;
            pRRend.Update();

            IRgbColor pFromColor = new RgbColorClass();
            pFromColor.Red = 124;//天蓝色 124 252 0
            pFromColor.Green = 252;
            pFromColor.Blue = 0;
            IRgbColor pToColor = new RgbColorClass();
            pToColor.Red = 135;//草坪绿
            pToColor.Green = 206;
            pToColor.Blue = 235;

            IAlgorithmicColorRamp colorRamp = new AlgorithmicColorRampClass();
            colorRamp.Size = 10;
            colorRamp.FromColor = pFromColor;
            colorRamp.ToColor = pToColor;
            bool createColorRamp;
            colorRamp.CreateRamp(out createColorRamp);

            IFillSymbol fillSymbol = new SimpleFillSymbolClass();
            for (int i = 0; i < pRClassRend.ClassCount; i++)
            {

                fillSymbol.Color = colorRamp.get_Color(i);
                pRClassRend.set_Symbol(i, fillSymbol as ISymbol);
                pRClassRend.set_Label(i, pRClassRend.get_Break(i).ToString("0.00"));
            }
            pOutRasterlayer.Renderer = pRRend;
            this.axMapControl1.AddLayer(pOutRasterlayer);

        }

        private void button_IDW_TIME_Click(object sender, EventArgs e)
        {
            object obj = null;
            ILayer pLayer = new FeatureLayerClass();
            IFeatureClass featureClass = GetFeatureClass("D://a_gis工程设计实践课//墓穴地shp//grave.shp");
            IGeoDataset geo = featureClass as IGeoDataset;
            object extend = geo.Extent;
            object o = null;
            IFeatureClassDescriptor feades = new FeatureClassDescriptorClass();
            feades.Create(featureClass, null, "time_inter");
            IRasterRadius rasterrad = new RasterRadiusClass();
            rasterrad.SetVariable(12, ref obj);
            object dCell = 0.014800000;//可以根据不同的点图层进行设置
            IInterpolationOp interpla = new RasterInterpolationOpClass();
            IRasterAnalysisEnvironment rasanaenv = interpla as IRasterAnalysisEnvironment;
            rasanaenv.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, ref dCell);
            rasanaenv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, ref extend, ref o);
            IGeoDataset g_GeoDS_Raster = interpla.IDW((IGeoDataset)feades, 2, rasterrad, ref obj);
            IRaster pOutRsater = (IRaster)g_GeoDS_Raster;
            IRasterLayer pOutRasterlayer = new RasterLayerClass();
            pOutRasterlayer.CreateFromRaster(pOutRsater);

            pOutRasterlayer.Name = "时间久远程度";

            IRasterClassifyColorRampRenderer pRClassRend = new RasterClassifyColorRampRendererClass();
            IRasterRenderer pRRend = pRClassRend as IRasterRenderer;

            IRaster pRaster = pOutRasterlayer.Raster;
            IRasterBandCollection pRBandCol = pRaster as IRasterBandCollection;
            IRasterBand pRBand = pRBandCol.Item(0);
            if (pRBand.Histogram == null)
            {
                pRBand.ComputeStatsAndHist();
            }
            pRRend.Raster = pRaster;
            pRClassRend.ClassCount = 10;
            pRRend.Update();

            IRgbColor pFromColor = new RgbColorClass();
            pFromColor.Red = 135;//天蓝色
            pFromColor.Green = 206;
            pFromColor.Blue = 235;
            IRgbColor pToColor = new RgbColorClass();
            pToColor.Red = 124;//草坪绿
            pToColor.Green = 252;
            pToColor.Blue = 0;

            IAlgorithmicColorRamp colorRamp = new AlgorithmicColorRampClass();
            colorRamp.Size = 10;
            colorRamp.FromColor = pFromColor;
            colorRamp.ToColor = pToColor;
            bool createColorRamp;
            colorRamp.CreateRamp(out createColorRamp);

            IFillSymbol fillSymbol = new SimpleFillSymbolClass();

            for (int i = 0; i < pRClassRend.ClassCount; i++)
            {

                fillSymbol.Color = colorRamp.get_Color(i);
                pRClassRend.set_Symbol(i, fillSymbol as ISymbol);
                pRClassRend.set_Label(i, pRClassRend.get_Break(i).ToString("0.00"));
            }
            pOutRasterlayer.Renderer = pRRend;

            this.axMapControl1.AddLayer(pOutRasterlayer);
        }
    }
}
