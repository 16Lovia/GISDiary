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



        private void showGrave()//资料卡展示
        {

            //十字丝定位放大

            //子窗体展示
            //longitude = 117;/*****************/
            Form_showGrave fn = new Form_showGrave();
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


        public void StopAnimationShowInfor(int AniID)
        {

            //camara设定



            //资料卡弹出

        }
        #endregion



        static string pathShp1 = @"res\Line_5s\poi5line1_lei.shp";
        static string pathShp2 = @"res\Line_5s\poi5line2_lei.shp";
        static string pathShp3 = @"res\Line_5s\poi5line3_lei.shp";
        static string pathShp4 = @"res\Line_5s\poi5line4_lei.shp";
        static string pathShp5 = @"res\Line_5s\poi5line5_lei.shp";

        static string SaveFilePath1 = @"res\flyGlobe1";
        static string SaveFilePath2 = @"res\flyGlobe2";
        static string SaveFilePath3 = @"res\flyGlobe3";
        static string SaveFilePath4 = @"res\flyGlobe4";
        static string SaveFilePath5 = @"res\flyGlobe5";
        static int a;//当前动画播放序号

        public void partToshow(int a)
        {
            ++a;
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

        private void btn_globe_Click(object sender, EventArgs e)
        {
            a = 0;
            for (int i = 0; i<5;i++)
            {
                partToshow(a);

            }
           
        }

     
        
    }
}
