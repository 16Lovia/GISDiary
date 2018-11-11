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
            CreateAnimationFromPath();
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
        private IScene scene;
        private ILayer pLayer;
        private int pFeatureID;

        public void CreateAnimationFromPath()
        {

            //pGlobe = axGlobeControl1.Globe;
            //pScene = axSceneControl1.Scene;
            //IGlobeDisplay globeDisplay = pGlobe.GlobeDisplay;
            //IGlobeDisplay globeDisplay = axGlobeControl1.Globe.GlobeDisplay;
            //IScene scene = axSceneControl1.Scene.SceneGraph.ActiveViewer;
            scene = axSceneControl1.Scene;

            // 获取动画扩展
            IBasicScene2 basicScene2 = scene as IBasicScene2;
            IAnimationExtension animationExtension = basicScene2.AnimationExtension;

            //获取路径

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

          
            //string ShpPath = @"D:\code\resource\PLine.shp";
            //string ShpPath1 = @"D:\code\resource";
            //IWorkspaceFactory pWsF = new ShapefileWorkspaceFactory();
            //IFeatureWorkspace pFW = (IFeatureWorkspace)pWsF.OpenFromFile(ShpPath1, 0);
            ////pFullPaths[0].Substring(0,pFullPaths[0].LastIndexOf("\\"))=pFilePath，不包含文件名


            

            ////打开文件
            //IFeatureClass pFeaC = pFW.OpenFeatureClass(ShpPath);
            //IFeatureLayer pFeaL = new FeatureLayer();
            //pFeaL.FeatureClass = pFeaC;
            //pFeaL.Name = pFeaC.AliasName;
           

            


            //string xjShpPath = @"D:\code\resource\PLine.shp";
            //if (xjShpPath == null) return;
            //string xjShpFolder = System.IO.Path.GetDirectoryName(xjShpPath);
            //string xjShpFileName = System.IO.Path.GetFileName(xjShpPath);
            ////工作工厂+工作空间
            //IWorkspaceFactory xjShpWsF = new ShapefileWorkspaceFactory();
            //IFeatureWorkspace xjShpFWs = (IFeatureWorkspace)xjShpWsF.OpenFromFile(xjShpFolder, 0);
            ////新建矢量图层：要素+名称
            //IWorkspace xjShpWs = xjShpWsF.OpenFromFile(xjShpFolder, 0);
            //IFeatureClass xjShpFeatureClass = xjShpFWs.OpenFeatureClass(xjShpFileName);
            //IFeatureLayer featureLayer = new FeatureLayer();
            //featureLayer.FeatureClass = xjShpFeatureClass;
            //featureLayer.Name = xjShpFeatureClass.AliasName;
            //加载刷新
            //this.axGlobeControl1.AddLayer(xjShpFeatureLayer);
            //this.axSceneControl1.ActiveView.Refresh();
            //this.axSceneControl1.AddLayer(xjShpFeatureLayer);
            //this.axSceneControl1.ActiveView.Refresh();

            //IFeatureLayer featureLayer = pLayer as IFeatureLayer;
            IFeatureClass featureClass = pFLayer.FeatureClass;
            pFeatureID = 0;
            IFeature feature = featureClass.GetFeature(pFeatureID);
            int a = featureClass.FeatureClassID;
            IGeometry geometry = feature.Shape;

            //创建AGAnimationUtils和AGImportPathOptions对象
            ESRI.ArcGIS.Animation.IAGAnimationUtils agAnimationUtils = new AGAnimationUtilsClass();
            ESRI.ArcGIS.Animation.IAGImportPathOptions agImportPathOptions = new AGImportPathOptionsClass();

            // 设置AGImportPathOptions的属性
            agImportPathOptions.BasicMap = (IBasicMap)scene;
            agImportPathOptions.AnimationTracks = (IAGAnimationTracks)scene;
            agImportPathOptions.AnimationType = new AnimationTypeGlobeCameraClass();
            agImportPathOptions.AnimatedObject = scene.SceneGraph.ActiveViewer.Camera; //动画对象
            agImportPathOptions.PathGeometry = geometry;                      //动画轨迹
            agImportPathOptions.ConversionType = ESRI.ArcGIS.Animation.esriFlyFromPathType.esriFlyFromPathObsAndTarget;
            agImportPathOptions.LookaheadFactor = 0.05;
            agImportPathOptions.RollFactor = 0;

            agImportPathOptions.AnimationEnvironment = animationExtension.AnimationEnvironment;
            IAGAnimationContainer AGAnimationContainer = animationExtension.AnimationTracks.AnimationObjectContainer;

            //创建并保存动画
            agAnimationUtils.CreateFlybyFromPath(AGAnimationContainer, agImportPathOptions);
            string SaveFilePath = @"D:\code\resource\fly.avi";
            if (System.IO.File.Exists(SaveFilePath))
            {
                System.IO.File.Delete(SaveFilePath);
                agAnimationUtils.SaveAnimationFile(AGAnimationContainer, SaveFilePath, esriArcGISVersion.esriArcGISVersion10);
            }

        }
        #endregion

        private void axLicenseControl2_Enter(object sender, EventArgs e)
        {

        }
    }
}
