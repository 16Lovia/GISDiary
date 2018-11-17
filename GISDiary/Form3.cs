using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Animation;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GlobeCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;



namespace GISDiary
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
           // ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            //string file3d = @"res\china3d\china3d.sxd";
            //axSceneControl1.LoadSxFile(file3d);
        }

        #region"Create Animation from Path"

        ///<summary> 由路径来创建一个Camera动画.这条路径有图层提供一条三维线要素</summary>
        /// 
        ///<param name="globe">IGlobe接口</param>
        ///<param name="layer">一个包含PolyLine的ILayer接口</param>
        ///<param name="featureID">包含路径的要素ID.Example: 123</param>
        ///  
        ///<remarks></remarks>
        public void CreateAnimationFromPath()
        {
            IGlobe pGlobe = axGlobeControl1.Globe;
            IGlobeDisplay globeDisplay = pGlobe.GlobeDisplay;
            IScene scene = globeDisplay.Scene;

            // 获取动画扩展
            IBasicScene2 basicScene2 = scene as IBasicScene2;
            IAnimationExtension animationExtension = basicScene2.AnimationExtension;

            //获取路径

            //读取shp文件

            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();

            string path = @"C:\Users\lenovo\Desktop\poi5line_lei\poi5line_lei.shp";
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
            int b = pFeatureClass.ObjectClassID;

            ILayer pLayer = pFLayer;
            IFeatureLayer featureLayer = pLayer as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            //int pFeatureID = pFeatureClass.FeatureClassID;
            int pFeatureID = 0;
            IFeature feature = featureClass.GetFeature(pFeatureID);
            IGeometry geometry = feature.Shape;

            //创建AGAnimationUtils和AGImportPathOptions对象
            ESRI.ArcGIS.Animation.IAGAnimationUtils agAnimationUtils = new AGAnimationUtilsClass();
            ESRI.ArcGIS.Animation.IAGImportPathOptions agImportPathOptions = new AGImportPathOptionsClass();

            // 设置AGImportPathOptions的属性
            agImportPathOptions.BasicMap = (IBasicMap)pGlobe;
            agImportPathOptions.AnimationTracks = (IAGAnimationTracks)pGlobe;
            agImportPathOptions.AnimationType = new AnimationTypeGlobeCameraClass();
            agImportPathOptions.AnimatedObject = pGlobe.GlobeDisplay.ActiveViewer.Camera; //动画对象
            agImportPathOptions.PathGeometry = geometry;                      //动画轨迹
            agImportPathOptions.ConversionType = ESRI.ArcGIS.Animation.esriFlyFromPathType.esriFlyFromPathObsAndTarget;
            agImportPathOptions.LookaheadFactor = 0.05;
            agImportPathOptions.RollFactor = 0;

            agImportPathOptions.AnimationEnvironment = animationExtension.AnimationEnvironment;
            IAGAnimationContainer AGAnimationContainer = animationExtension.AnimationTracks.AnimationObjectContainer;

            //创建
            agAnimationUtils.CreateFlybyFromPath(AGAnimationContainer, agImportPathOptions);

            //播放

            //获取AGAnimationEnvironment对象
            IBasicScene2 basicscene = pGlobe as IBasicScene2;
            IAnimationExtension animationEx = basicscene.AnimationExtension;
            IAGAnimationEnvironment agAnimationEnv;
            agAnimationEnv = animationEx.AnimationEnvironment;
            agAnimationEnv.AnimationDuration = Convert.ToDouble("30");//持续时间
            agAnimationEnv.PlayType = esriAnimationPlayType.esriAnimationPlayTypeDuration; //播放模式

            agAnimationEnv.PlayMode = esriAnimationPlayMode.esriAnimationPlayOnceForward;

            agAnimationEnv.PlayMode = esriAnimationPlayMode.esriAnimationPlayOnceReverse;
            //agAnimationEnv.PlayMode = esriAnimationPlayMode.esriAnimationPlayLoopForward;
            //agAnimationEnv.PlayMode = esriAnimationPlayMode.esriAnimationPlayLoopReverse;


            IAGAnimationPlayer agAnimationPlayer = agAnimationUtils as IAGAnimationPlayer;

            agAnimationPlayer.PlayAnimation(pGlobe as IAGAnimationTracks, agAnimationEnv, null);




            //保存

            string SaveFilePath = @"res\flyGlobe";
            if (System.IO.File.Exists(SaveFilePath))
            {
                System.IO.File.Delete(SaveFilePath);
                agAnimationUtils.SaveAnimationFile(AGAnimationContainer, SaveFilePath, esriArcGISVersion.esriArcGISVersion10);
            }

        }


        #endregion

        private void btn_Globe_Click(object sender, EventArgs e)
        {
            string filePath = @"D:\a_gis工程设计实践课\scene世界地图\world.3dd";
            if (axGlobeControl1.Check3dFile(filePath))
            {
                axGlobeControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerHourglass;
                axGlobeControl1.Load3dFile(filePath);
                axGlobeControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
            else
            {
                MessageBox.Show(filePath + "不是有效的地图文档");
            }



            //定位墓穴点
            try
            {
                IScene pScene = axGlobeControl1.Globe.GlobeDisplay.Scene;
                IFeatureLayer pFlyr = null;

                string name = "云顶天宫";
                for (int i = 0; i < pScene.LayerCount; ++i)
                {
                    if (pScene.get_Layer(i).Name == name)//要定位的图层
                    {
                        pFlyr = pScene.get_Layer(i) as IFeatureLayer;
                        break;
                    }
                }
                IFeature pFteature = pFlyr.FeatureClass.GetFeature(0);//定位ObjectID=1384元素

                ESRI.ArcGIS.Analyst3D.ICamera camera = axGlobeControl1.Globe.GlobeDisplay.ActiveViewer.Camera;
                ESRI.ArcGIS.GlobeCore.IGlobeCamera globeCamera = (ESRI.ArcGIS.GlobeCore.IGlobeCamera)camera;

                IMap pMap = axGlobeControl1.Globe as IMap;
                pMap.SelectFeature(pFlyr, pFteature);

                ((IActiveView)axGlobeControl1.Globe).PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

                ZoomToGeometry(axGlobeControl1.Globe, pFteature.Shape, 3,name);
            }
            catch { }

            showGrave();


            //展示墓穴点
        }

        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);
        public static int longitude;
        private void showGrave()
        {

            //十字丝定位放大

            //子窗体展示
            longitude = 117;/*****************/
            testform fn = new testform();
            fn.MdiParent = this;
            fn.StartPosition = FormStartPosition.CenterScreen;
           
/*            //除此之外，也可以手动设置窗口显示的位置，即窗口坐标。首先必须把窗体的显示位置设置为手动。
            fn.StartPosition = FormStartPosition.Manual;
            //随后获取屏幕的分辨率，也就是显示器屏幕的大小。
            int xWidth = SystemInformation.PrimaryMonitorSize.Width;//获取显示器屏幕宽度
            int yHeight = SystemInformation.PrimaryMonitorSize.Height;//高度
            //然后定义窗口位置，以主窗体为例
            //MessageBox.Show(xWidth.ToString(), yHeight.ToString());          
           //fn.Location = new Point(xWidth / 2, yHeight / 2);//这里需要再减去窗体本身的宽度和高度的一半
           在窗体的属性location里设置
           */
            
            fn.Show();
            SetParent((int)fn.Handle, (int)this.Handle);

        }

        public void ZoomToGeometry(IGlobe pGlobe, IGeometry pGeo, double doubleDuration,string name)
        {
            pGeo = ProjectGeometryGeo(pGeo);
            IEnvelope pEnvelope = null;
            pEnvelope = pGeo.Envelope;
            ZoomToSelectedGlobeFeatures(pGlobe, pEnvelope,name);
        }

        public void ZoomToSelectedGlobeFeatures(ESRI.ArcGIS.GlobeCore.IGlobe globe, IEnvelope pEv,string name)
        {
            ESRI.ArcGIS.GlobeCore.IGlobeDisplay globeDisplay = globe.GlobeDisplay;
            ESRI.ArcGIS.Analyst3D.ISceneViewer sceneViewer = globeDisplay.ActiveViewer;
            ESRI.ArcGIS.Analyst3D.ICamera camera = sceneViewer.Camera;
            ESRI.ArcGIS.GlobeCore.IGlobeCamera globeCamera = (ESRI.ArcGIS.GlobeCore.IGlobeCamera)camera;
            ESRI.ArcGIS.Analyst3D.IScene scene = globeDisplay.Scene;


            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.SetEmpty();
            ESRI.ArcGIS.Geometry.IEnvelope layersExtentEnvelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            layersExtentEnvelope.SetEmpty();
            ESRI.ArcGIS.Geometry.IZAware ZAware = (ESRI.ArcGIS.Geometry.IZAware)envelope;
            ZAware.ZAware = (true);


            envelope.Union(pEv);
            IFeatureLayer pFlyr = null;
            for (int i = 0; i < scene.LayerCount; ++i)
            {
                if (scene.get_Layer(i).Name == name)
                {
                    pFlyr = scene.get_Layer(i) as IFeatureLayer;
                    break;
                }
            }
            ESRI.ArcGIS.Geodatabase.IGeoDataset geoDataset = (ESRI.ArcGIS.Geodatabase.IGeoDataset)pFlyr;
            if (geoDataset != null)
            {
                ESRI.ArcGIS.Geometry.IEnvelope layerExtent = geoDataset.Extent;
                layersExtentEnvelope.Union(layerExtent);
            }


            System.Double width = envelope.Width;
            System.Double height = envelope.Height;
            if (width == 0.0 && height == 0.0)
            {
                System.Double dim = 1.0;


                System.Boolean bEmpty = layersExtentEnvelope.IsEmpty;
                if (!bEmpty)
                {
                    System.Double layerWidth = layersExtentEnvelope.Width;
                    System.Double layerHeight = layersExtentEnvelope.Height;
                    System.Double layerDim = System.Math.Max(layerWidth, layerHeight) * 0.05;
                    if (layerDim > 0.0)
                        dim = System.Math.Min(1.0, layerDim);
                }


                System.Double xMin = envelope.XMin;
                System.Double yMin = envelope.YMin;


                ESRI.ArcGIS.Geometry.IPoint point = new ESRI.ArcGIS.Geometry.PointClass();
                point.X = xMin;
                point.Y = yMin;


                envelope.Width = dim * 0.8;
                envelope.Height = dim * 0.8;
                envelope.CenterAt(point);

            }
            else if (width == 0.0 || height == 0.0)
            {
                System.Double maxDim = System.Math.Max(width, height);
                envelope.Width = maxDim;
                envelope.Height = maxDim;
            }


            globeCamera.SetToZoomToExtents(envelope, globe, sceneViewer);
            sceneViewer.Redraw(true);
        }

        public static IGeometry ProjectGeometryGeo(IGeometry pGeo)
        {
            //如果是地理坐标系，则投影到投影坐标系
            if (pGeo.SpatialReference is IProjectedCoordinateSystem)
            {
                ISpatialReferenceFactory srFactory = new SpatialReferenceEnvironment();
                IGeographicCoordinateSystem pcs = srFactory.CreateGeographicCoordinateSystem(4326);     //投影到 Mercator 坐标系
                pGeo.Project(pcs);
            }
            return pGeo;
        }

        public static IGeometry ProjectGeometry(IGeometry pGeo)
        {
            //如果是地理坐标系，则投影到投影坐标系
            if (pGeo.SpatialReference is IGeographicCoordinateSystem)
            {
                ISpatialReferenceFactory srFactory = new SpatialReferenceEnvironment();
                IProjectedCoordinateSystem pcs = srFactory.CreateProjectedCoordinateSystem((int)esriSRProjCSType.esriSRProjCS_World_Mercator);      //投影到 Mercator 坐标系
                pGeo.Project(pcs);
            }
            return pGeo;
        }

        private void axGlobeControl1_OnMouseDown(object sender, IGlobeControlEvents_OnMouseDownEvent e)
        {
       
        }

    }
}


   
