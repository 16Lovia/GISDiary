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
          
        }
    }
}
