using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Animation;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Windows.Forms;

namespace GISDiary
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(this.axSceneControl_OnMouseWheel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string file2d = @"res\china.mxd";
            axMapControl1.LoadMxFile(file2d);
            axMapControl1.Extent = axMapControl1.FullExtent;
            string file3d = @"res\china3d\china3d.sxd";
            axSceneControl1.LoadSxFile(file3d);
            //openFileDialog1.Filter = "";
            //openFileDialog1.ShowDialog();
            //string filename = openFileDialog1.FileName;
            //if (axSceneControl1.CheckSxFile(filename))
            //    axSceneControl1.LoadSxFile(filename);
            //else
            //{
            //    IScene pScene = axSceneControl1.Scene;
            //    IMemoryBlobStream mbStream = new MemoryBlobStreamClass();
            //    IObjectStream objectStream = new ObjectStreamClass();
            //    mbStream.LoadFromFile(filename);
            //    IPersistStream pPersistStream = (ESRI.ArcGIS.esriSystem.IPersistStream)pScene;
            //    objectStream.Stream = mbStream;
            //    pPersistStream.Load(objectStream);
            //}
            //string sFilePath;
            //sFilePath = openFileDialog1.FileName;

            //ITin pTIN = new Tin3DPropertiesClass() as ITin;
            //ITinLayer pTINLyr = new TinLayerClass();
            //pTINLyr.Dataset = pTIN;
            ////pTINLyr.
            //axSceneControl1.Scene.AddLayer(pTINLyr);
            //axSceneControl1.Refresh();
            //axSceneControl1.Scene.AddLayer(@"D:\code\resource\3d.tif.lyr");
        }

        private IPointCollection ptCol;//给定点的集合
        private ISegmentCollection m_polyline;//待绘制多线

        private void axSceneControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ISceneControlEvents_OnMouseDownEvent e)
        {
             ICamera pCamera = this.axSceneControl1.Camera;
            IPoint point = pCamera.Target;
            IEnvelope pEnv = new EnvelopeClass();
            pEnv.XMax = point.X + 5;
            pEnv.XMin = point.X - 5;
            pEnv.YMax = point.Y + 5;
            pEnv.YMin = point.Y - 5;

            IRectangleElement pRectangleEle = new RectangleElementClass();
            IElement pEle = pRectangleEle as IElement;
            pEle.Geometry = pEnv;

            //设置线框的边线对象，包括颜色和线宽
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 238; 
            pColor.Green = 99;
            pColor.Blue =99;
            pColor.Transparency = 255;
            // 产生一个线符号对象 
            ILineSymbol pOutline = new SimpleLineSymbolClass();
            pOutline.Width = 2;
            pOutline.Color = pColor;

            // 设置颜色属性 
            pColor.Red = 238;
            pColor.Green = 99;
            pColor.Blue = 99;
            pColor.Transparency =0;

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

        private void RouteNavi()
        {
            axSceneControl1.Navigate = true;
        }


        private void axMapControl_2D_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
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
                pPtObs.X += (pPtObs.X - pPtTar.X) * scale;
                pPtObs.Y += (pPtObs.Y - pPtTar.Y) * scale;
                pPtObs.Z += (pPtObs.Z - pPtTar.Z) * scale;
                pCamera.Observer = pPtObs;
                axSceneControl1.SceneGraph.RefreshViewers();
            }
            catch
            {
            }
        }
      

        private void axSceneControl1_OnMouseUp(object sender, ESRI.ArcGIS.Controls.ISceneControlEvents_OnMouseUpEvent e)
        {
          
        }

        private void btn_flyPath_Click(object sender, EventArgs e)
        {
            //CreateAnimationFromPath();
            CreateAnimationFromPath1();
        }

        #region"Create Animation from Path"

        ///<summary> 由路径来创建一个Camera动画.这条路径有图层提供一条三维线要素</summary>
        /// 
        ///<param name="globe">IGlobe接口</param>
        ///<param name="layer">一个包含PolyLine的ILayer接口</param>
        ///<param name="featureID">包含路径的要素ID.Example: 123</param>
        ///  
        ///<remarks></remarks>
        ///

        //private IGlobe pGlobe;
        //private IScene scene;
        private ILayer pLayer;
        private int pFeatureID;


        /// <summary>

        /// 

        /// </summary>

        /// <param name="_pScene"></param>

        /// <param name="_pPolyline"></param>

        /// <param name="_pType"></param>

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
                        dataRow[i] = pFeature.get_Value(i).ToString();
                    }
                }
                dataTable.Rows.Add(dataRow);
                pFeature = pFeatureCursor.NextFeature();
            }

            // 释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            return dataTable;
        }



        public void tt(IScene pScene, IPolyline _pPolyline, int _pType, double _pDuration)

        {

            IScene _pScene = pScene;

            // 获取动画扩展对象

            ESRI.ArcGIS.Analyst3D.IBasicScene2 pBasicScene2 = (ESRI.ArcGIS.Analyst3D.IBasicScene2)_pScene; // Explicit Cast

            ESRI.ArcGIS.Animation.IAnimationExtension pAnimationExtension = pBasicScene2.AnimationExtension;



            //创建两个对象，一个用于导入路径，一个用于播放

            ESRI.ArcGIS.Animation.IAGAnimationUtils pAGAnimationUtils = new ESRI.ArcGIS.Animation.AGAnimationUtilsClass();



            ESRI.ArcGIS.Animation.IAGImportPathOptions pAGImportPathOptions = new ESRI.ArcGIS.Animation.AGImportPathOptionsClass();



            // 设置参数

            //参数设置不正确会出错，尤其是类型，对象等信息！

            pAGImportPathOptions.BasicMap = (ESRI.ArcGIS.Carto.IBasicMap)_pScene;

            pAGImportPathOptions.AnimationTracks = pAnimationExtension.AnimationTracks;

            pAGImportPathOptions.AnimationType = new AnimationTypeCameraClass();

            // pAGImportPathOptions.AnimationType = new AnimationTypeGlobeCameraClass();



            pAGImportPathOptions.LookaheadFactor = 1;

            pAGImportPathOptions.PutAngleCalculationMethods(esriPathAngleCalculation.esriAngleAddRelative,

                              esriPathAngleCalculation.esriAngleAddRelative,

                              esriPathAngleCalculation.esriAngleAddRelative);



            pAGImportPathOptions.AnimatedObject = _pScene.SceneGraph.ActiveViewer.Camera;//

            //pAGImportPathOptions.AnimatedObject = pScene.SceneGraph.ActiveViewer.Camera;

            pAGImportPathOptions.PathGeometry = _pPolyline;

            //都移动

            if (_pType == 1)

            {

                pAGImportPathOptions.ConversionType = ESRI.ArcGIS.Animation.esriFlyFromPathType.esriFlyFromPathObsAndTarget;

            }//观察者移动

            else if (_pType == 2)

            {

                pAGImportPathOptions.ConversionType = ESRI.ArcGIS.Animation.esriFlyFromPathType.esriFlyFromPathObserver;

            }

            else

            {

                pAGImportPathOptions.ConversionType = ESRI.ArcGIS.Animation.esriFlyFromPathType.esriFlyFromPathTarget;

            }




            pAGImportPathOptions.RollFactor = 0;

            pAGImportPathOptions.AnimationEnvironment = pAnimationExtension.AnimationEnvironment;

            pAGImportPathOptions.ReversePath = false;



            ESRI.ArcGIS.Animation.IAGAnimationContainer AGAnimationContainer = pAnimationExtension.AnimationTracks.AnimationObjectContainer;

            pAGAnimationUtils.CreateFlybyFromPath(AGAnimationContainer,pAGImportPathOptions);

            //该接口相当于播放的界面，可以自己做一个界面

            IAGAnimationPlayer pAGAplayer = pAGAnimationUtils as IAGAnimationPlayer;

            IAGAnimationEnvironment pAGAeviroment = new AGAnimationEnvironmentClass();

            pAGAeviroment.AnimationDuration = _pDuration;

            pAGAeviroment.PlayMode = esriAnimationPlayMode.esriAnimationPlayOnceForward;

            pAGAplayer.PlayAnimation(_pScene as IAGAnimationTracks, pAGAeviroment, null);

            string SaveFilePath = @"D:\code\resource\fly.avi";
            //if (System.IO.File.Exists(SaveFilePath))
            //{
            //    System.IO.File.Delete(SaveFilePath);
            //    agAnimationUtils.SaveAnimationFile(AGAnimationContainer, SaveFilePath, esriArcGISVersion.esriArcGISVersion10);
            //}
        }

        public IPointCollection getPointcollection(IPointCollection pPolycollect)

        {

            //IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();

            //IFeatureWorkspace pWS = (IFeatureWorkspace)pWSF.OpenFromFile(@"D:\Riva\poi3", 0);

            //IFeatureClass pFeatureclass = pWS.OpenFeatureClass("poi3.shp");

            //IFeatureCursor pCursor = pFeatureclass.Search(null, false);

            //while (pCursor != null)

            //{

            //    IFeature pFeature = pCursor.NextFeature();

            //    if (pFeature != null)

            //    {

            //        IGeometry pGeometry = pFeature.Shape;

            //        object objmiss = Type.Missing;

            //        IPoint pPoint = new PointClass();

            //        //pPoint.SpatialReference = _spatial;

            //        IFeatureClass pFeatureClass = GetFeatureClass(@"D:\Riva\poi3\poi3.shp");
            //        DataTable dataTable = GetAttributesTable(pFeatureClass);
            //        //dataGridView1.DataSource = dataTable;

            //        //从数据库获取该站点的经纬度坐标
            //        IPoint point = new PointClass();
            //        point.X = Convert.ToDouble(dataTable.Rows[0]["X"].ToString());//X经度
            //        point.Y = Convert.ToDouble(dataTable.Rows[0]["Y"].ToString());//Y纬度
                   



            //        //pPoint.X = Convert.ToDouble(pFeature.get_Value(2));

            //        //pPoint.Y = Convert.ToDouble(pFeature.get_Value(3));

            //        //pPoint.Z = Convert.ToDouble(pFeature.get_Value(4));

            //        pPolycollect.AddPoint(pPoint, ref objmiss, ref objmiss);

            //    }

            //    else

            //    {

            //        pCursor = null;

            //    }

            //}


            IPoint pPoint1 = new PointClass();
            IPoint pPoint2 = new PointClass();
            IPoint pPoint3 = new PointClass();

            pPoint1.X = Convert.ToDouble("340214.11");

            pPoint1.Y = Convert.ToDouble("3240912.11");

            pPoint1.Z = Convert.ToDouble("3.5");

            pPoint2.X = Convert.ToDouble("340414.11");

            pPoint2.Y = Convert.ToDouble("3240932.11");

            pPoint2.Z = Convert.ToDouble("2.5");


            pPoint3.X = Convert.ToDouble("340614.11");

            pPoint3.Y = Convert.ToDouble("3240952.11");

            pPoint3.Z = Convert.ToDouble("1.5");


            pPolycollect.AddPoint(pPoint1);
            pPolycollect.AddPoint(pPoint2);
            pPolycollect.AddPoint(pPoint3);

            if (pPolycollect.PointCount != 0)

                return pPolycollect;

            else

                return null;

        }

        public void CreateAnimationFromPath1()
        {
            ILayerFactoryHelper pLayerFactoryHelper = new LayerFactoryHelperClass();

            IFileName filename = new FileNameClass();

            filename.Path = @"D:\Riva\poi3\poi3.shp";

            IEnumLayer enumlayer = pLayerFactoryHelper.CreateLayersFromName(filename as IName);

            ILayer layer;

            enumlayer.Reset();

            layer = enumlayer.Next();

            while (layer != null)

            {

                axSceneControl1.Scene.AddLayer(layer, false);

                layer = enumlayer.Next();

                axSceneControl1.SceneGraph.RefreshViewers();

            }

            //axSceneControl1.LoadSxFile(@"D:\code\resource\china3d.sxd");     //这句话后面有解释     

            IPolyline pPolyline = new PolylineClass();



            //pPolyline.SpatialReference = _spatial;

            IPointCollection pPolycollect = pPolyline as IPointCollection;

            getPointcollection(pPolycollect);

            tt(axSceneControl1.Scene, pPolyline, 2, 100000);

        }

        public void CreateAnimationFromPath()
        {
            //ESRI.ArcGIS.GlobeCore.IGlobeDisplay globeDisplay = globe.GlobeDisplay;
            IScene globe = axSceneControl1.Scene;
            ESRI.ArcGIS.Analyst3D.IScene scene = axSceneControl1.Scene;

            //获取动画扩展的句柄
            // Get a handle to the animation extension
            ESRI.ArcGIS.Analyst3D.IBasicScene2 basicScene2 = (ESRI.ArcGIS.Analyst3D.IBasicScene2)scene; // Explicit Cast
            ESRI.ArcGIS.Animation.IAnimationExtension animationExtension = basicScene2.AnimationExtension;

            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();

            string path = @"D:\code\resource\PLine.shp";
            string pFolder = System.IO.Path.GetDirectoryName(path);
            string pFileName = System.IO.Path.GetFileName(path);

            //2打开shapeFile工作空间
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pFolder, 0);
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;

            //3、打开要素类
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pFileName);

            //4、创建要素图层
            IFeatureLayer pFLayer = new FeatureLayerClass();

            //5、关联图层和要素类
            pFLayer.FeatureClass = pFeatureClass;
            pFLayer.Name = pFeatureClass.AliasName;

            ESRI.ArcGIS.Carto.IFeatureLayer featureLayer = (ESRI.ArcGIS.Carto.IFeatureLayer)pFLayer; // Explicit Cast
            ESRI.ArcGIS.Geodatabase.IFeatureClass featureClass = featureLayer.FeatureClass;
            int featureID = 0;
            ESRI.ArcGIS.Geodatabase.IFeature feature = featureClass.GetFeature(featureID);

            // Get the geometry of the line feature

            

            ESRI.ArcGIS.Geometry.IGeometry geometry = feature.Shape;

            // Create AGAnimationUtils and AGImportPathOptions objects
            ESRI.ArcGIS.Animation.IAGAnimationUtils agAnimationUtils = new ESRI.ArcGIS.Animation.AGAnimationUtilsClass();
            ESRI.ArcGIS.Animation.IAGImportPathOptions agImportPathOptions = new ESRI.ArcGIS.Animation.AGImportPathOptionsClass();

            // Set properties for AGImportPathOptions
            agImportPathOptions.BasicMap = (ESRI.ArcGIS.Carto.IBasicMap)globe; // Explicit Cast
            agImportPathOptions.AnimationTracks = (ESRI.ArcGIS.Animation.IAGAnimationTracks)globe; // Explicit Cast
            agImportPathOptions.AnimationType = new ESRI.ArcGIS.GlobeCore.AnimationTypeGlobeCameraClass();
            agImportPathOptions.AnimatedObject = globe.SceneGraph.ActiveViewer.Camera;
            agImportPathOptions.PathGeometry = geometry;
            agImportPathOptions.ConversionType = ESRI.ArcGIS.Animation.esriFlyFromPathType.esriFlyFromPathObsAndTarget;
            agImportPathOptions.LookaheadFactor = 0.05;
            agImportPathOptions.RollFactor = 0;

            agImportPathOptions.AnimationEnvironment = animationExtension.AnimationEnvironment;
            ESRI.ArcGIS.Animation.IAGAnimationContainer AGAnimationContainer = animationExtension.AnimationTracks.AnimationObjectContainer;

            // Call "CreateFlybyFromPath"
            agAnimationUtils.CreateFlybyFromPath(AGAnimationContainer, agImportPathOptions);


            ////pGlobe = axGlobeControl1.Globe;
            ////pScene = axSceneControl1.Scene;
            ////IGlobeDisplay globeDisplay = pGlobe.GlobeDisplay;
            ////IGlobeDisplay globeDisplay = axGlobeControl1.Globe.GlobeDisplay;
            ////IScene scene = axSceneControl1.Scene.SceneGraph.ActiveViewer;
            //scene = axSceneControl1.Scene;

            //// 获取动画扩展
            //IBasicScene2 basicScene2 = scene as IBasicScene2;
            //IAnimationExtension animationExtension = basicScene2.AnimationExtension;

            ////获取路径

            //IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();

            //string path = @"D:\code\resource\PLine.shp";
            //string pFolder = System.IO.Path.GetDirectoryName(path);
            //string pFileName = System.IO.Path.GetFileName(path);

            ////2打开shapeFile工作空间
            //IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pFolder, 0);
            //IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;

            ////3、打开要素类
            //IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pFileName);

            ////4、创建要素图层
            //IFeatureLayer pFLayer = new FeatureLayerClass();

            ////5、关联图层和要素类
            //pFLayer.FeatureClass = pFeatureClass;
            //pFLayer.Name = pFeatureClass.AliasName;


            ////string ShpPath = @"D:\code\resource\PLine.shp";
            ////string ShpPath1 = @"D:\code\resource";
            ////IWorkspaceFactory pWsF = new ShapefileWorkspaceFactory();
            ////IFeatureWorkspace pFW = (IFeatureWorkspace)pWsF.OpenFromFile(ShpPath1, 0);
            //////pFullPaths[0].Substring(0,pFullPaths[0].LastIndexOf("\\"))=pFilePath，不包含文件名




            //////打开文件
            ////IFeatureClass pFeaC = pFW.OpenFeatureClass(ShpPath);
            ////IFeatureLayer pFeaL = new FeatureLayer();
            ////pFeaL.FeatureClass = pFeaC;
            ////pFeaL.Name = pFeaC.AliasName;





            ////string xjShpPath = @"D:\code\resource\PLine.shp";
            ////if (xjShpPath == null) return;
            ////string xjShpFolder = System.IO.Path.GetDirectoryName(xjShpPath);
            ////string xjShpFileName = System.IO.Path.GetFileName(xjShpPath);
            //////工作工厂+工作空间
            ////IWorkspaceFactory xjShpWsF = new ShapefileWorkspaceFactory();
            ////IFeatureWorkspace xjShpFWs = (IFeatureWorkspace)xjShpWsF.OpenFromFile(xjShpFolder, 0);
            //////新建矢量图层：要素+名称
            ////IWorkspace xjShpWs = xjShpWsF.OpenFromFile(xjShpFolder, 0);
            ////IFeatureClass xjShpFeatureClass = xjShpFWs.OpenFeatureClass(xjShpFileName);
            ////IFeatureLayer featureLayer = new FeatureLayer();
            ////featureLayer.FeatureClass = xjShpFeatureClass;
            ////featureLayer.Name = xjShpFeatureClass.AliasName;
            ////加载刷新
            ////this.axGlobeControl1.AddLayer(xjShpFeatureLayer);
            ////this.axSceneControl1.ActiveView.Refresh();
            ////this.axSceneControl1.AddLayer(xjShpFeatureLayer);
            ////this.axSceneControl1.ActiveView.Refresh();

            ////IFeatureLayer featureLayer = pLayer as IFeatureLayer;
            //IFeatureClass featureClass = pFLayer.FeatureClass;
            //pFeatureID = 0;
            //IFeature feature = featureClass.GetFeature(pFeatureID);
            //int a = featureClass.FeatureClassID;
            //IGeometry geometry = feature.Shape;

            ////创建AGAnimationUtils和AGImportPathOptions对象
            //ESRI.ArcGIS.Animation.IAGAnimationUtils agAnimationUtils = new AGAnimationUtilsClass();
            //ESRI.ArcGIS.Animation.IAGImportPathOptions agImportPathOptions = new AGImportPathOptionsClass();

            //// 设置AGImportPathOptions的属性
            //agImportPathOptions.BasicMap = (IBasicMap)scene;
            //agImportPathOptions.AnimationTracks = (IAGAnimationTracks)scene;
            //agImportPathOptions.AnimationType = new AnimationTypeGlobeCameraClass();
            //agImportPathOptions.AnimatedObject = scene.SceneGraph.ActiveViewer.Camera; //动画对象
            //agImportPathOptions.PathGeometry = geometry;                      //动画轨迹
            //agImportPathOptions.ConversionType = ESRI.ArcGIS.Animation.esriFlyFromPathType.esriFlyFromPathObsAndTarget;
            //agImportPathOptions.LookaheadFactor = 0.05;
            //agImportPathOptions.RollFactor = 0;

            //agImportPathOptions.AnimationEnvironment = animationExtension.AnimationEnvironment;
            //IAGAnimationContainer AGAnimationContainer = animationExtension.AnimationTracks.AnimationObjectContainer;

            ////创建并保存动画
            //agAnimationUtils.CreateFlybyFromPath(AGAnimationContainer, agImportPathOptions);
            //string SaveFilePath = @"D:\code\resource\fly.avi";
            //if (System.IO.File.Exists(SaveFilePath))
            //{
            //    System.IO.File.Delete(SaveFilePath);
            //    agAnimationUtils.SaveAnimationFile(AGAnimationContainer, SaveFilePath, esriArcGISVersion.esriArcGISVersion10);
            //}

        }
        #endregion

        private void axLicenseControl2_Enter(object sender, EventArgs e)
        {

        }
    }
}
