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
using CCWin;
using ESRI.ArcGIS.Analyst3D;

namespace GISDiary
{
    public partial class Form_difficulty : Skin_DevExpress
    {
        //  private string tiffPath = "D:\\users\\lenovo\\documents\\visual studio 2015\\Projects\\Teamwork\\Teamwork\\raster\\last.tif";
        public Form_difficulty()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(this.axSceneControl_OnMouseWheel);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            string file2d = @"res\china\china_1.mxd";
            //string file2d = @"res\usa.mxd";
            axMapControl1.LoadMxFile(file2d);
            string file2d_2 = @"res\china\china_1.mxd";
            //string file2d = @"res\usa.mxd";
            axMapControl2.LoadMxFile(file2d_2);
            //axMapControl1.Extent = axMapControl1.FullExtent;
             string file3d = @"res\china3d\china3d.sxd";
             axSceneControl1.LoadSxFile(file3d);
        }
        private void buttton_tiff_Click(object sender, EventArgs e)
        {
            axMapControl1.ClearLayers();
            string tiffPath = @"res\china3d\L07\10.tif"; //D://users//lenovo//documents//visual studio 2015//Projects//Teamwork//Teamwork//tiff文件
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
            if (e.button == 1)
            {
                //记录鼠标点击的点
                IPoint pNewPoint = new PointClass();
                pNewPoint.PutCoords(e.mapX, e.mapY);

                //设置三维视角
                IActiveView pActiveView = axMapControl1.ActiveView;
                //IActiveView pActiveView1 = this.axMapControl1 as IActiveView;   //获取当前二维活动区域               
                //IEnvelope pEnv = pActiveView1 as IEnvelope;      //将此二位区域的Extent 保存在Envelope中
                IActiveView pActiveView1 = this.axMapControl1.Map as IActiveView;   //获取当前二维活动区域               
                IEnvelope pEnv = pActiveView1.Extent as IEnvelope;      //将此二位区域的Extent 保存在Envelope中
                IPoint point = new PointClass();        //将此区域的中心点保存起来
                point = GetGeo(pActiveView, (pEnv.XMax + pEnv.XMin) / 2, (pEnv.YMax + pEnv.YMin) / 2);  //取得视角中心点X坐标

                ICamera pCamera = this.axSceneControl1.Camera;      //取得三维活动区域的Camara      ，就像你照相一样的视角，它有Taget（目标点）和Observer（观察点）两个属性需要设置    
                IPoint ptTaget = new PointClass();      //创建一个目标点
                ptTaget = point;        //视觉区域中心点作为目标点
                ptTaget.Z = 0;         //设置目标点高度，这里设为 0米

                IPoint ptObserver = new PointClass();   //创建观察点 的X，Y，Z
                ptObserver.X = point.X;     //设置观察点坐标的X坐标
                ptObserver.Y = point.Y + 2;     //设置观察点坐标的Y坐标（这里加90米，是在南北方向上加了90米，当然这个数字可以自己定，意思就是将观察点和目标点有一定的偏差，从南向北观察
                double height = 10;     //计算观察点合适的高度，这里用三目运算符实现的，效果稍微好一些，当然可以自己拟定
                ptObserver.Z = height;              //设置观察点坐标的Y坐标

                // ptObserver = GetProject(pActiveView1, pCamera.Observer.X, pCamera.Observer.Y);
                pCamera.Target = ptTaget;       //赋予目标点
                pCamera.Observer = ptObserver;      //将上面设置的观察点赋予camera的观察点
                pCamera.Inclination = 20;       //设置三维场景视角，也就是高度角，视线与地面所成的角度
                pCamera.Azimuth = 180;          //设置三维场景方位角，视线与向北的方向所成的角度
                axSceneControl1.SceneGraph.RefreshViewers();        //刷新地图，（很多时候，看不到效果，都是你没有刷新）

                System.Timers.Timer t = new System.Timers.Timer();//500ms空隙
                t.Elapsed += new System.Timers.ElapsedEventHandler(sceneRotate);//调用函数
                t.AutoReset = false;//是否循环调用
                t.Enabled = true;//是否调用
                t.Start();
            }
            //this.axMapControl1.Extent = this.axMapControl1.TrackRectangle();
            else if (e.button == 2)//右键
            { this.axMapControl1.Pan();

            }
            // label1.Text = " 当前坐标 X = " + e.mapX.ToString() + " Y = " + e.mapY.ToString() + " " + this.axMapControl1.MapUnits.ToString().Substring(4);
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
                    //axTOCControl1.Update();
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
            IFeatureClass pFeatureClass = GetFeatureClass("E://lcy//GISDiary//GISDiary//bin//Debug//res//china//墓穴地shp//grave.shp");
            DataTable dataTable = GetAttributesTable(pFeatureClass);
            //dataGridView1.DataSource = dataTable;

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

        //private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        //{
        //    if (strUnion == false)
        //        return;
        //    repGeoMap();
        //}

        //private void axMapControl1_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        //{
        //    if (strUnion == false)
        //        return;
        //    CopyAndWriteMap();
        //}

        //private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        //{
        //    //TbGeoMap();
        //    if (strUnion == false)
        //        return;
        //    CopyAndWriteMap();
        //}

        private void button_save_Click(object sender, EventArgs e)
        {
            IActiveView docActiveView = axPageLayoutControl1.ActiveView;
            IExport docExport = new ExportJPEGClass();
            IPrintAndExport docPrintExport = new PrintAndExportClass();
            int iOutputResolution = 300;
            //设置输出文件名
            docExport.ExportFileName = @"res\盗墓难度专题图.JPG";
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
            IFeatureClass featureClass = GetFeatureClass("E://lcy//GISDiary//GISDiary//bin//Debug//res//china//墓穴地shp//grave.shp");
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
            pFromColor.Red =255;//天蓝色 124 252 0255 246 143 139 134 78
            pFromColor.Green = 246;
            pFromColor.Blue = 143;
            IRgbColor pToColor = new RgbColorClass();
            pToColor.Red = 139;//草坪绿
            pToColor.Green = 134;
            pToColor.Blue = 78;

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
            IFeatureClass featureClass = GetFeatureClass("E://lcy//GISDiary//GISDiary//bin//Debug//res//china//墓穴地shp//grave.shp");
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

        private void toolStripButton1_Click(object sender, EventArgs e)
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

        private void toolStripButton2_Click(object sender, EventArgs e)
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
                    //axTOCControl1.Update();
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
                strUnion = true;
                CopyAndWriteMap();

            }
            catch
            {
                MessageBox.Show("请输入有效图层!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
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
                Text = "我的盗墓日记"
            };
            element = pTextElement as ESRI.ArcGIS.Carto.IElement;
            element.Geometry = envelope;
            graphicsContainer.AddElement(element, 0);
            axPageLayoutControl1.Refresh();

            IActiveView docActiveView = axPageLayoutControl1.ActiveView;
            IExport docExport = new ExportJPEGClass();
            IPrintAndExport docPrintExport = new PrintAndExportClass();
            int iOutputResolution = 300;
            //设置输出文件名
            docExport.ExportFileName = @"res\我的盗墓日记.JPG";
            //输出当前视图到输出文件
            docPrintExport.Export(docActiveView, docExport, iOutputResolution, true, null);
        }

        private void 盗墓笔记ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 艰险指数ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 记下小本本ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 趟山海ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.Map.ClearLayers();
            string file2d = @"res\china\last_mxd\last.mxd";
            axMapControl1.LoadMxFile(file2d);
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
            IFeatureClass featureClass = GetFeatureClass("E://lcy//GISDiary//GISDiary//bin//Debug//res//china//墓穴地shp//grave.shp");
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
            pFromColor.Red = 0;//天蓝色 124 252 0     0 92 230         190 232 255
            pFromColor.Green = 92;
            pFromColor.Blue = 230;
            IRgbColor pToColor = new RgbColorClass();
            pToColor.Red = 190;//草坪绿
            pToColor.Green = 232;
            pToColor.Blue = 255;

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

        private void 遍古今ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //axMapControl1.Map.ClearLayers();
            //string file2d = @"res\china\last_mxd\last.mxd";
           // axMapControl1.LoadMxFile(file2d);
            object obj = null;
            ILayer pLayer = new FeatureLayerClass();
            IFeatureClass featureClass = GetFeatureClass("E://lcy//GISDiary//GISDiary//bin//Debug//res//china//墓穴地shp//grave.shp");
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
            pFromColor.Red = 190;//天蓝色 124 252 0     0 92 230         190 232 255
            pFromColor.Green = 232;
            pFromColor.Blue = 255;
            IRgbColor pToColor = new RgbColorClass();
            pToColor.Red = 0;//草坪绿
            pToColor.Green = 92;
            pToColor.Blue = 230;


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

        private void 盗墓笔记ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //axMapControl1.Map.ClearLayers();
            string file2d = @"res\china\china.mxd";
            axMapControl1.LoadMxFile(file2d);
            //IMapDocument xjMxdMapDocument = new MapDocumentClass();
            //OpenFileDialog xjMxdOpenFileDialog = new OpenFileDialog();
            //xjMxdOpenFileDialog.Filter = "地图文档(*.mxd)|*.mxd";


            //if (xjMxdOpenFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    string xjmxdFilePath = xjMxdOpenFileDialog.FileName;
            //    if (axMapControl1.CheckMxFile(xjmxdFilePath))
            //    {
            //        axMapControl1.Map.ClearLayers();
            //        axMapControl1.LoadMxFile(xjmxdFilePath);
            //    }
            //}
            axMapControl1.ActiveView.Refresh();
        }

        private void 艰险指数ToolStripMenuItem_Click_1(object sender, EventArgs e)
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
                pRgbColor1.Red = 144;
                pRgbColor1.Blue = 238;
                pRgbColor1.Green = 144; 
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
                    //axTOCControl1.Update();
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

        private void 记下小本本ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            strUnion = true;
            CopyAndWriteMap();
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

            //IActiveView docActiveView = axPageLayoutControl1.ActiveView;
            //IExport docExport = new ExportJPEGClass();
            //IPrintAndExport docPrintExport = new PrintAndExportClass();
            //int iOutputResolution = 300;
            ////设置输出文件名
            //docExport.ExportFileName = @"res\盗墓难度专题图.JPG";
            ////输出当前视图到输出文件
            //docPrintExport.Export(docActiveView, docExport, iOutputResolution, true, null);
            //刷新axPageLayoutControl1的内容

            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            //创建鹰眼中线框       
            IEnvelope pEnv = (IEnvelope)e.newEnvelope;
            IRectangleElement pRectangleEle = new RectangleElementClass();
            IElement pEle = pRectangleEle as IElement;
            pEle.Geometry = pEnv;

            //设置线框的边线对象，包括颜色和线宽
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 255;
            // 产生一个线符号对象 
            ILineSymbol pOutline = new SimpleLineSymbolClass();
            pOutline.Width = 2;
            pOutline.Color = pColor;

            // 设置颜色属性 
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 0;

            // 设置线框填充符号的属性 
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutline;
            IFillShapeElement pFillShapeEle = pEle as IFillShapeElement;
            pFillShapeEle.Symbol = pFillSymbol;

            // 得到鹰眼视图中的图形元素容器
            IGraphicsContainer pGra = axMapControl2.Map as IGraphicsContainer;
            IActiveView pAv = pGra as IActiveView;
            // 在绘制前，清除 axMapControl2 中的任何图形元素 
            pGra.DeleteAllElements();
            // 鹰眼视图中添加线框
            pGra.AddElement((IElement)pFillShapeEle, 0);
            // 刷新鹰眼
            pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void axMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            // 按下鼠标左键移动矩形框 
            if (e.button == 1)
            {
                IPoint pPoint = new PointClass();
                pPoint.PutCoords(e.mapX, e.mapY);
                IEnvelope pEnvelope = this.axMapControl1.Extent;
                pEnvelope.CenterAt(pPoint);
                this.axMapControl1.Extent = pEnvelope;
                this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
            // 按下鼠标右键绘制矩形框 
            else if (e.button == 2)
            {
                IEnvelope pEnvelop = this.axMapControl2.TrackRectangle();
                this.axMapControl1.Extent = pEnvelop;
                this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
            else if (e.button == 4)
            {
                this.axMapControl2.Pan();
            }
        }

        private void axMapControl2_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            // 如果不是左键按下就直接返回 
            if (e.button != 1) return;
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(e.mapX, e.mapY);
            this.axMapControl1.CenterAt(pPoint);
            this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        private ILayer GetOverviewLayer(IMap map)
        {
            //获取主视图的第一个图层
            ILayer pLayer = map.get_Layer(0);
            //遍历其他图层，并比较视图范围的宽度，返回宽度最大的图层
            ILayer pTempLayer = null;
            for (int i = 1; i < map.LayerCount; i++)
            {
                pTempLayer = map.get_Layer(i);
                if (pLayer.AreaOfInterest.Width < pTempLayer.AreaOfInterest.Width)
                    pLayer = pTempLayer;
            }
            return pLayer;
        }

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
          //  //获取鹰眼图层
          //  this.axMapControl2.AddLayer(this.GetOverviewLayer(this.axMapControl1.Map));
          //  //设置 MapControl 显示范围至数据的全局范围
          ////  this.axMapControl2.Extent = this.axMapControl1.FullExtent;
          //  // 刷新鹰眼控件地图
          //  this.axMapControl2.Refresh();
        }

        private void axMapControl1_OnFullExtentUpdated(object sender, IMapControlEvents2_OnFullExtentUpdatedEvent e)
        {
           // //获取鹰眼图层
           // this.axMapControl2.AddLayer(this.GetOverviewLayer(this.axMapControl1.Map));
           // // 设置 MapControl 显示范围至数据的全局范围
           //// this.axMapControl2.Extent = this.axMapControl1.FullExtent;
           // // 刷新鹰眼控件地图
           // this.axMapControl2.Refresh();
        }

        // 将平面坐标转换为经纬度。
        private IPoint GetGeo(IActiveView pActiveView, double x, double y)
        {
            try
            {
                IMap pMap = pActiveView.FocusMap;
                IPoint pt = new PointClass();
                ISpatialReferenceFactory pfactory = new SpatialReferenceEnvironmentClass();
                ISpatialReference flatref = pMap.SpatialReference;
                ISpatialReference earthref = pfactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_Beijing1954);
                pt.PutCoords(x, y);

                IGeometry geo = (IGeometry)pt;
                geo.SpatialReference = flatref;
                geo.Project(earthref);
                double xx = pt.X;
                return pt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        // 将经纬度点转换为平面坐标。
        private IPoint GetProject(IActiveView pActiveView, double x, double y)
        {
            try
            {
                IMap pMap = pActiveView.FocusMap;
                IPoint pt = new PointClass();
                ISpatialReferenceFactory pfactory = new SpatialReferenceEnvironmentClass();
                ISpatialReference flatref = pMap.SpatialReference;
                ISpatialReference earthref = pfactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_Beijing1954);
                pt.PutCoords(x, y);
                IGeometry geo = (IGeometry)pt;
                geo.SpatialReference = earthref;
                geo.Project(flatref);
                return pt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;

            }
        }


        private void sceneRotate(object sender, System.Timers.ElapsedEventArgs e)
        {
            ICamera pCamera = this.axSceneControl1.Camera;      //取得三维活动区域的Camara      ，就像你照相一样的视角，它有Taget（目标点）和Observer（观察点）两个属性需要设置    
            IPoint ptObserver = new PointClass();
            ptObserver = pCamera.Observer;
            for (double i = 0; i < 0.1;)
            {
                i += 0.0005;
                ptObserver.X += i;
                ptObserver.Y += i;

                pCamera.Observer = ptObserver;
                axSceneControl1.SceneGraph.RefreshViewers();        //刷新地图，（很多时候，看不到效果，都是你没有刷新）
                System.Threading.Thread.Sleep(30);//停顿2秒钟  
            }
        }

        private void axMapControl1_OnMouseDown_1(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
           
        }
        private void axSceneControl_OnMouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                System.Drawing.Point pSceLoc = axSceneControl1.PointToScreen(this.axSceneControl1.Location);
                System.Drawing.Point Pt = this.PointToScreen(e.Location);
                if (Pt.X < pSceLoc.X || Pt.X > pSceLoc.X + axSceneControl1.Width || Pt.Y < pSceLoc.Y || Pt.Y > pSceLoc.Y + axSceneControl1.Height)
                {
                    return;
                }
                double scale = 0.2;
                if (e.Delta < 0) scale = -0.2;
                ICamera pCamera = axSceneControl1.Camera;
                IPoint pPtObs = pCamera.Observer;
                IPoint pPtTar = pCamera.Target;

                IPoint point;
                //point= pCamera.Target;
                point = pCamera.Observer;
                IEnvelope pEnv = new EnvelopeClass();
                pEnv.XMax = point.X + 5;
                pEnv.XMin = point.X - 5;
                pEnv.YMax = point.Y + 5;
                pEnv.YMin = point.Y - 5;

                pPtObs.X += (pPtObs.X - pPtTar.X) * scale;
                pPtObs.Y += (pPtObs.Y - pPtTar.Y) * scale;
                pPtObs.Z += (pPtObs.Z - pPtTar.Z) * scale;
                pCamera.Observer = pPtObs;
                axSceneControl1.SceneGraph.RefreshViewers();

                IRectangleElement pRectangleEle = new RectangleElementClass();
                IElement pEle = pRectangleEle as IElement;
                pEle.Geometry = pEnv;

                //设置线框的边线对象，包括颜色和线宽
                IRgbColor pColor = new RgbColorClass();
                pColor.Red = 238;
                pColor.Green = 99;
                pColor.Blue = 99;
                pColor.Transparency = 255;
                // 产生一个线符号对象 
                ILineSymbol pOutline = new SimpleLineSymbolClass();
                pOutline.Width = 2;
                pOutline.Color = pColor;

                // 设置颜色属性 
                pColor.Red = 238;
                pColor.Green = 99;
                pColor.Blue = 99;
                pColor.Transparency = 0;

                // 设置线框填充符号的属性 
                IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
                pFillSymbol.Color = pColor;
                pFillSymbol.Outline = pOutline;
                IFillShapeElement pFillShapeEle = pEle as IFillShapeElement;
                pFillShapeEle.Symbol = pFillSymbol;

                // 得到鹰眼视图中的图形元素容器
                IGraphicsContainer pGra = axMapControl1.Map as IGraphicsContainer;
                IActiveView pAv = pGra as IActiveView;
                // 在绘制前，清除 axMapControl1 中的任何图形元素 
                pGra.DeleteAllElements();
                // 鹰眼视图中添加线框
                pGra.AddElement((IElement)pFillShapeEle, 0);
                // 刷新鹰眼
                pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch
            {
            }
        }

        private void axMapControl1_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
     
        }
    }
}