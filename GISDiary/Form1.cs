using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Animation;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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



        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

        private void axSceneControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ISceneControlEvents_OnMouseDownEvent e)//整体路径生成
        {

            //设置三维视角
            IActiveView pActiveView1 = this.axSceneControl1.Scene as IActiveView;   //获取当前二维活动区域               
            IEnvelope enve = pActiveView1.Extent as IEnvelope;      //将此二位区域的Extent 保存在Envelope中
            IPoint point = new PointClass();        //将此区域的中心点保存起来
            point.X = (enve.XMax + enve.XMin) / 2;  //取得视角中心点X坐标
            point.Y = (enve.YMax + enve.YMin) / 2;  //取得视角中心点Y坐标

            IPoint ptTaget = new PointClass();      //创建一个目标点
            ptTaget = point;        //视觉区域中心点作为目标点
            ptTaget.Z = 0;         //设置目标点高度，这里设为 0米

            IPoint ptObserver = new PointClass();   //创建观察点 的X，Y，Z
            ptObserver.X = point.X;     //设置观察点坐标的X坐标
            ptObserver.Y = point.Y + 90;     //设置观察点坐标的Y坐标（这里加90米，是在南北方向上加了90米，当然这个数字可以自己定，意思就是将观察点和目标点有一定的偏差，从南向北观察
            double height = (enve.Width < enve.Height) ? enve.Width : enve.Height;      //计算观察点合适的高度，这里用三目运算符实现的，效果稍微好一些，当然可以自己拟定
            ptObserver.Z = height;              //设置观察点坐标的Y坐标

            ICamera pCamera = this.axSceneControl1.Camera;      //取得三维活动区域的Camara      ，就像你照相一样的视角，它有Taget（目标点）和Observer（观察点）两个属性需要设置    
            pCamera.Target = ptTaget;       //赋予目标点
            pCamera.Observer = ptObserver;      //将上面设置的观察点赋予camera的观察点
            pCamera.Inclination = 90;       //设置三维场景视角，也就是高度角，视线与地面所成的角度
            pCamera.Azimuth = 180;          //设置三维场景方位角，视线与向北的方向所成的角度
            axSceneControl1.SceneGraph.RefreshViewers();        //刷新地图，（很多时候，看不到效果，都是你没有刷新）

            IPoint pt = null;
            ISceneGraph pSG = axSceneControl1.SceneGraph;
            ISceneViewer pSW = pSG.ActiveViewer;
            object a;
            object b;
            pSG.Locate(pSW, e.x, e.y, esriScenePickMode.esriScenePickAll, true, out pt, out a, out b);
            if (pt == null) return;
            if (ptCol == null)
                ptCol = new MultipointClass();
            ptCol.AddPoint(pt);
            int i = ptCol.PointCount;
            if (i < 1) return;

            IRgbColor pRgbColor = new RgbColorClass();
            pRgbColor.Blue = 255;
            pRgbColor.Green = 0;
            pRgbColor.Red = 0;
            ISimpleLine3DSymbol pSimpleLine3DSymbol = new SimpleLine3DSymbolClass();
            pSimpleLine3DSymbol.Style = esriSimple3DLineStyle.esriS3DLSTube;
            ILineSymbol pLineSymbol = pSimpleLine3DSymbol as ILineSymbol;
            pLineSymbol.Color = pRgbColor;
            pLineSymbol.Width = 0.5;
            //ILineElement pLineElement = new LineElementClass();  
            //pLineElement.Symbol = pLineSymbol;  

            //产生线段对象 line  
            ILine pLine = new LineClass();
            IPoint fromPt = ptCol.get_Point(i - 1);
            IPoint toPt = ptCol.get_Point(i - 2);
            pLine.PutCoords(fromPt, toPt);

            //将线段对象添加到多义线对象polyline  
            object Missing1 = Type.Missing;
            object Missing2 = Type.Missing;
            ISegment pSegment = pLine as ISegment;
            if (m_polyline == null)
                m_polyline = new PolylineClass();
            m_polyline.AddSegment(pSegment, ref Missing1, ref Missing2);
            int tttt = m_polyline.SegmentCount;

            //让Z值生效  
            IZAware Zaware = m_polyline as IZAware;
            Zaware.ZAware = true;

            IGeometry geometry = (IGeometry)m_polyline;

            //更新到Graphics窗口  
            IGraphicsContainer3D pGCon3D = axSceneControl1.Scene.BasicGraphicsLayer as IGraphicsContainer3D;
            IElement pElement = new LineElementClass();
            pElement.Geometry = geometry;

            ILineElement pLineElement = pElement as ILineElement;
            pLineElement.Symbol = pLineSymbol;

            pGCon3D.DeleteAllElements();
            pGCon3D.AddElement(pElement);
            axSceneControl1.Scene.SceneGraph.RefreshViewers();
        }

        private void RouteNavi()
        {
            axSceneControl1.Navigate = true;
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

    }
}
