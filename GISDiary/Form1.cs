using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Animation;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
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

        private IGlobe pGlobe;
        private ILayer pLayer;
        //public void CreateAnimationFromPath()
        //{


        //    IGlobeDisplay globeDisplay = pGlobe.GlobeDisplay;
        //    IScene scene = globeDisplay.Scene;

        //    // 获取动画扩展
        //    IBasicScene2 basicScene2 = scene as IBasicScene2;
        //    IAnimationExtension animationExtension = basicScene2.AnimationExtension;

        //    //获取路径

        //    string xjShpPath = @"";
        //    string xjShpFolder = System.IO.Path.GetDirectoryName(xjShpPath);
        //    string xjShpFileName = System.IO.Path.GetFileName(xjShpPath);
        //    //工作工厂+工作空间
        //    IWorkspaceFactory xjShpWsF = new ShapefileWorkspaceFactory();
        //    IFeatureWorkspace xjShpFWs = (IFeatureWorkspace)xjShpWsF.OpenFromFile(xjShpFolder, 0);
        //    //新建矢量图层：要素+名称
        //    IWorkspace xjShpWs = xjShpWsF.OpenFromFile(xjShpFolder, 0);
        //    IFeatureClass xjShpFeatureClass = xjShpFWs.OpenFeatureClass(xjShpFileName);
        //    IFeatureLayer featureLayer = new FeatureLayer();
        //    featureLayer.FeatureClass = xjShpFeatureClass;
        //    featureLayer.Name = xjShpFeatureClass.AliasName;
        //    //加载刷新
        //    //this.axSceneControl1.AddLayer(xjShpFeatureLayer);
        //    //this.axSceneControl1.ActiveView.Refresh();
            
        //    //IFeatureLayer featureLayer = pLayer as IFeatureLayer;
        //    IFeatureClass featureClass = featureLayer.FeatureClass;
        //    IFeature feature = featureClass.GetFeature(pFeatureID);
        //    IGeometry geometry = feature.Shape;

        //    //创建AGAnimationUtils和AGImportPathOptions对象
        //    ESRI.ArcGIS.Animation.IAGAnimationUtils agAnimationUtils = new AGAnimationUtilsClass();
        //    ESRI.ArcGIS.Animation.IAGImportPathOptions agImportPathOptions = new AGImportPathOptionsClass();

        //    // 设置AGImportPathOptions的属性
        //    agImportPathOptions.BasicMap = (IBasicMap)pGlobe;
        //    agImportPathOptions.AnimationTracks = (IAGAnimationTracks)pGlobe;
        //    agImportPathOptions.AnimationType = new AnimationTypeGlobeCameraClass();
        //    agImportPathOptions.AnimatedObject = pGlobe.GlobeDisplay.ActiveViewer.Camera; //动画对象
        //    agImportPathOptions.PathGeometry = geometry;                      //动画轨迹
        //    agImportPathOptions.ConversionType = ESRI.ArcGIS.Animation.esriFlyFromPathType.esriFlyFromPathObsAndTarget;
        //    agImportPathOptions.LookaheadFactor = 0.05;
        //    agImportPathOptions.RollFactor = 0;

        //    agImportPathOptions.AnimationEnvironment = animationExtension.AnimationEnvironment;
        //    IAGAnimationContainer AGAnimationContainer = animationExtension.AnimationTracks.AnimationObjectContainer;

        //    //创建并保存动画
        //    agAnimationUtils.CreateFlybyFromPath(AGAnimationContainer, agImportPathOptions);
        //    string SaveFilePath = @"D:\code\resource\fly.avi";
        //    if (System.IO.File.Exists(SaveFilePath))
        //    {
        //        System.IO.File.Delete(SaveFilePath);
        //        agAnimationUtils.SaveAnimationFile(AGAnimationContainer, SaveFilePath, esriArcGISVersion.esriArcGISVersion10);
        //    }

        //}
        #endregion

    }
}
