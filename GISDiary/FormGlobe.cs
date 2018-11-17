using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Animation;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GlobeCore;
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
    public partial class FormGlobe : Form
    {
        public FormGlobe()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            //string file3d = @"res\china3d\china3d.sxd";
            //axSceneControl1.LoadSxFile(file3d);
            LoadGlobe();
            a = 0;
        }

        //加载Globe

        private  void LoadGlobe()
        {

            //ClearGlobe();
            string GlobeFileName = @"res\globe1.3dd";
            if (GlobeFileName==null)
            {
                return;
            }

            axGlobeControl1.Load3dFile(GlobeFileName);

        }


        #region"Create Animation from Path"

        ///<summary> 由路径来创建一个Camera动画.这条路径有图层提供一条三维线要素</summary>
        /// 
        ///<param name="globe">IGlobe接口</param>
        ///<param name="layer">一个包含PolyLine的ILayer接口</param>
        ///<param name="featureID">包含路径的要素ID.Example: 123</param>
        ///  
        ///<remarks></remarks>
        public void CreateAnimationFromPath(string path,string SaveFilePath)
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

          
            if (System.IO.File.Exists(SaveFilePath))
            {
                System.IO.File.Delete(SaveFilePath);
                agAnimationUtils.SaveAnimationFile(AGAnimationContainer, SaveFilePath, esriArcGISVersion.esriArcGISVersion10);
            }

        }


        //public void CreateAnimationFromPath_scene()
        //{
        //    //IGlobe pGlobe = axGlobeControl1.Globe;           
        //    //IGlobeDisplay globeDisplay = pGlobe.GlobeDisplay;
        //    //IScene scene = globeDisplay.Scene;
        //    IScene pScene = axSceneControl1.Scene;
        //    ISceneGraph sceneGraph = pScene.SceneGraph;
        //    IScene scene = sceneGraph.Scene;

        //    // 获取动画扩展
        //    IBasicScene2 basicScene2 = scene as IBasicScene2;
        //    IAnimationExtension animationExtension = basicScene2.AnimationExtension;

        //    //获取路径

        //    //读取shp文件

        //    IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();

        //    string path = @"D:\code\GISDiary\GISDiary\bin\Debug\res\poi5line_lei\poi5line_lei\poi5line_lei.shp";
        //    string pFolder = System.IO.Path.GetDirectoryName(path);
        //    string pFileName = System.IO.Path.GetFileName(path);

        //    //2打开shapeFile工作空间
        //    IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pFolder, 0);
        //    IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;

        //    //3、打开要素类
        //    IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pFileName);

        //    //4、创建要素图层
        //    IFeatureLayer pFLayer = new FeatureLayerClass();

        //    //5、关联图层和要素类
        //    pFLayer.FeatureClass = pFeatureClass;
        //    pFLayer.Name = pFeatureClass.AliasName;
        //    int b = pFeatureClass.ObjectClassID;

        //    ILayer pLayer = pFLayer;
        //    IFeatureLayer featureLayer = pLayer as IFeatureLayer;
        //    IFeatureClass featureClass = featureLayer.FeatureClass;
        //    //int pFeatureID = pFeatureClass.FeatureClassID;
        //    int pFeatureID = 0;
        //    IFeature feature = featureClass.GetFeature(pFeatureID);
        //    IGeometry geometry = feature.Shape;

        //    //创建AGAnimationUtils和AGImportPathOptions对象
        //    ESRI.ArcGIS.Animation.IAGAnimationUtils agAnimationUtils = new AGAnimationUtilsClass();
        //    ESRI.ArcGIS.Animation.IAGImportPathOptions agImportPathOptions = new AGImportPathOptionsClass();

        //    // 设置AGImportPathOptions的属性
        //    agImportPathOptions.BasicMap = (IBasicMap)pScene;
        //    agImportPathOptions.AnimationTracks = (IAGAnimationTracks)pScene;
        //    agImportPathOptions.AnimationType = new AnimationTypeGlobeCameraClass();
        //    agImportPathOptions.AnimatedObject = pScene.SceneGraph.ActiveViewer.Camera; //动画对象
        //    agImportPathOptions.PathGeometry = geometry;                      //动画轨迹
        //    agImportPathOptions.ConversionType = ESRI.ArcGIS.Animation.esriFlyFromPathType.esriFlyFromPathObsAndTarget;
        //    agImportPathOptions.LookaheadFactor = 0.05;
        //    agImportPathOptions.RollFactor = 0;

        //    agImportPathOptions.AnimationEnvironment = animationExtension.AnimationEnvironment;
        //    IAGAnimationContainer AGAnimationContainer = animationExtension.AnimationTracks.AnimationObjectContainer;

        //    //创建
        //    agAnimationUtils.CreateFlybyFromPath(AGAnimationContainer, agImportPathOptions);

        //    //播放

        //    //获取AGAnimationEnvironment对象
        //    IBasicScene2 basicscene = pScene as IBasicScene2;
        //    IAnimationExtension animationEx = basicscene.AnimationExtension;
        //    IAGAnimationEnvironment agAnimationEnv;
        //    agAnimationEnv = animationEx.AnimationEnvironment;
        //    agAnimationEnv.AnimationDuration = Convert.ToDouble("30");//持续时间
        //    agAnimationEnv.PlayType = esriAnimationPlayType.esriAnimationPlayTypeDuration; //播放模式

        //    agAnimationEnv.PlayMode = esriAnimationPlayMode.esriAnimationPlayOnceForward;

        //    agAnimationEnv.PlayMode = esriAnimationPlayMode.esriAnimationPlayOnceReverse;
        //    //agAnimationEnv.PlayMode = esriAnimationPlayMode.esriAnimationPlayLoopForward;
        //    //agAnimationEnv.PlayMode = esriAnimationPlayMode.esriAnimationPlayLoopReverse;


        //    IAGAnimationPlayer agAnimationPlayer = agAnimationUtils as IAGAnimationPlayer;

        //    agAnimationPlayer.PlayAnimation(pScene as IAGAnimationTracks, agAnimationEnv, null);




        //    //保存

        //    string SaveFilePath = @"res\flyScene";
        //    if (System.IO.File.Exists(SaveFilePath))
        //    {
        //        System.IO.File.Delete(SaveFilePath);
        //        agAnimationUtils.SaveAnimationFile(AGAnimationContainer, SaveFilePath, esriArcGISVersion.esriArcGISVersion10);
        //    }

        //}

        #endregion

        #region"Stop Animation and Show Grave Information"

          


        public void StopAnimationShowInfor(int AniID)
        {

            //camara设定

            IActiveView pActiveView1 = this.axGlobeControl1.Globe as IActiveView;   //获取当前二维活动区域               
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

            ICamera pCamera = (ICamera)this.axGlobeControl1.GlobeCamera;      //取得三维活动区域的Camara，就像你照相一样的视角，它有Taget（目标点）和Observer（观察点）两个属性需要设置    
            pCamera.Target = ptTaget;       //赋予目标点
            pCamera.Observer = ptObserver;      //将上面设置的观察点赋予camera的观察点
            pCamera.Inclination = 90;       //设置三维场景视角，也就是高度角，视线与地面所成的角度
            pCamera.Azimuth = 180;          //设置三维场景方位角，视线与向北的方向所成的角度
            axGlobeControl1.GlobeDisplay.RefreshViewers();        //刷新地图，（很多时候，看不到效果，都是你没有刷新）


            //资料卡弹出
        }
        #endregion

        private int a;//button点击次数、当前动画播放序号

        private void btn_globe_Click(object sender, EventArgs e)
        {
            string pathShp1 = @"res\Line_5s\poi5line1_lei.shp";
            string pathShp2 = @"res\Line_5s\poi5line2_lei.shp";
            string pathShp3 = @"res\Line_5s\poi5line3_lei.shp";
            string pathShp4 = @"res\Line_5s\poi5line4_lei.shp";
            string pathShp5 = @"res\Line_5s\poi5line5_lei.shp";

            string SaveFilePath1 = @"res\flyGlobe1";
            string SaveFilePath2 = @"res\flyGlobe2";
            string SaveFilePath3 = @"res\flyGlobe3";
            string SaveFilePath4 = @"res\flyGlobe4";
            string SaveFilePath5 = @"res\flyGlobe5";

            a++;
            switch (a)
            {
                case 1:
                    CreateAnimationFromPath(pathShp1, SaveFilePath1);
                    StopAnimationShowInfor(1);

                    break;
                case 2:
                    CreateAnimationFromPath(pathShp2, SaveFilePath2);
                    StopAnimationShowInfor(2);

                    break;
                case 3:
                    CreateAnimationFromPath(pathShp3, SaveFilePath3);
                    StopAnimationShowInfor(3);

                    break;
                case 4:
                    CreateAnimationFromPath(pathShp4, SaveFilePath4);
                    StopAnimationShowInfor(4);
                    break;
                case 5:
                    CreateAnimationFromPath(pathShp5, SaveFilePath5);
                    StopAnimationShowInfor(5);

                    break;
                default:
                    break;

            }
          
        
          
          
        }

     
        //private void btn_Scene_Click(object sender, EventArgs e)
        //{
        //    CreateAnimationFromPath_scene();
        //}
    }
}
