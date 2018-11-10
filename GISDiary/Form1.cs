using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
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
           // axMapControl1.Extent = axMapControl1.FullExtent;
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


        }

        private void RouteNavi()
        {
            axSceneControl1.Navigate = true;
        }

        private void axSceneControl1_OnSceneReplaced(object sender, ESRI.ArcGIS.Controls.ISceneControlEvents_OnSceneReplacedEvent e)
        {
           
           
        }

        private void axMapControl_2D_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            //得到接口
            IActiveView pActiveView = this.axSceneControl1.SceneViewer as IActiveView;
            //获得显示范围
            IEnvelope pEnvelope = (IEnvelope)pActiveView.Extent;

            IPoint ptTaget = new PointClass();      //创建一个目标点
            ptTaget.X =e.x;        
            ptTaget.Y = e.y;
            ptTaget.Z = 0;         //设置目标点高度，这里设为 0米
            ICamera pCamera = this.axSceneControl1.Camera;      //取得三维活动区域的Camara      ，就像你照相一样的视角，它有Taget（目标点）和Observer（观察点）两个属性需要设置    
            pCamera.Target = ptTaget;       //赋予目标点
            IPoint ptObserver = new PointClass();   //创建观察点 的X，Y，Z
            ptObserver.X = ptTaget.X;     //设置观察点坐标的X坐标
            ptObserver.Y = ptTaget.Y + 90;     //设置观察点坐标的Y坐标（这里加90米，是在南北方向上加了90米，当然这个数字可以自己定，意思就是将观察点和目标点有一定的偏差，从南向北观察
            double height = (pEnvelope.Width < pEnvelope.Height) ? pEnvelope.Width : pEnvelope.Height;      //计算观察点合适的高度，这里用三目运算符实现的，效果稍微好一些，当然可以自己拟定
            ptObserver.Z = height;              //设置观察点坐标的Y坐标

            pCamera.Observer = ptObserver;      //将上面设置的观察点赋予camera的观察点
            pCamera.Inclination = 90;       //设置三维场景视角，也就是高度角，视线与地面所成的角度
            pCamera.Azimuth = 180;          //设置三维场景方位角，视线与向北的方向所成的角度
            axSceneControl1.SceneGraph.RefreshViewers();        //刷新地图，（很多时候，看不到效果，都是你没有刷新）
        }

        private void axMapControl_2D_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            
        }
     
        private void axSceneControl1_OnMouseMove(object sender, ESRI.ArcGIS.Controls.ISceneControlEvents_OnMouseMoveEvent e)
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
        private void button1_Click(object sender, EventArgs e)
        {
            //得到接口
            IActiveView pActiveView = this.axMapControl1.Map as IActiveView;

            //获得显示范围
            IEnvelope pEnvelope = (IEnvelope)pActiveView.Extent;



            //刷新
            pEnvelope.Expand(0.9, 0.9, true);
            pActiveView.Extent = pEnvelope;
            pActiveView.Refresh();

            //三维
            //获得现场的相机
            ICamera pCamera = this.axSceneControl1.Camera as ICamera;

            //拓宽视野
            double dAngle;
            dAngle = pCamera.ViewFieldAngle;
            pCamera.ViewFieldAngle = dAngle * 0.9;

            //重绘现场
            ISceneViewer pSceneViewer = this.axSceneControl1.SceneGraph.ActiveViewer as ISceneViewer;
            pSceneViewer.Redraw(false);

        }

        private void axSceneControl1_OnMouseUp(object sender, ESRI.ArcGIS.Controls.ISceneControlEvents_OnMouseUpEvent e)
        {

            IActiveView pActiveView1 = this.axSceneControl1.Scene as IActiveView;
            IEnvelope enve = pActiveView1.Extent as IEnvelope;      //将此二位区域的Extent 保存在Envelope中
            IPoint point = new PointClass();
           // point.PutCoords(e.x, e.y);          
           point.X = (enve.XMax - enve.XMin) / 2;
          point.Y = (enve.YMax - enve.YMin) / 2;

            IEnvelope pEnv = new EnvelopeClass();
            pEnv.XMax = point.X + 50;
            pEnv.XMin = point.X - 50;
            pEnv.YMax = point.Y + 50;
            pEnv.YMin = point.Y - 50;

            //创建鹰眼中线框
            // IEnvelope pEnv = (IEnvelope)enve;
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
            IGraphicsContainer pGra = axMapControl1.Map as IGraphicsContainer;
            IActiveView pAv = pGra as IActiveView;
            // 在绘制前，清除 axMapControl1 中的任何图形元素 
            pGra.DeleteAllElements();
            // 鹰眼视图中添加线框
            pGra.AddElement((IElement)pFillShapeEle, 0);
            // 刷新鹰眼
            pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            //IPoint minPoint = new PointClass();
            //minPoint.X = enve.LowerLeft.X;
            //minPoint.Y = enve.LowerLeft.Y;

            //IPoint maxPoint = new PointClass();
            //maxPoint.X = enve.UpperRight.X;
            //maxPoint.Y = enve.UpperRight.Y;

            //IPoint minPoint_ProjectToXY = new PointClass();
            //IPoint maxPoint_ProjectToXY = new PointClass();



            ////设置平面Envelope
            //IEnvelope GeoEnvelope = new EnvelopeClass();
            //GeoEnvelope.LowerLeft = minPoint;
            //GeoEnvelope.UpperRight = maxPoint;
            ////设置平面extent
            //axMapControl1.CenterAt(point);
            ////axMapControl1.ActiveView.Extent = GeoEnvelope;
            //// axMapControl1.ActiveView.Activate(axMapControl1.hWnd);
            //axMapControl1.ActiveView.Refresh();


        }
    }
}
