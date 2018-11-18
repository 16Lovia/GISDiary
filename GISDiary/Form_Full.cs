using CCWin;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISDiary
{
    public partial class Form_Full : Skin_DevExpress
    {
        public Form_Full()
        {
            
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);

            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(this.axSceneControl_OnMouseWheel);
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
  

      
        private void Form_Full_Load(object sender, EventArgs e)
        {
           string file2d = @"res\china\china_1.mxd";
            //string file2d = @"res\usa.mxd";
            axMapControl1.LoadMxFile(file2d);
            axMapControl1.Extent = axMapControl1.FullExtent;
            string file3d = @"res\china3d\china3d.sxd";            
             axSceneControl1.LoadSxFile(file3d);

            //string file2d_1 = @"res\china\china_1.mxd";
            //axMapControl2.LoadMxFile(file2d_1);
            ////axMapControl2.Extent = axMapControl1.FullExtent;
        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 1)
            {
                //记录鼠标点击的点
                IPoint pNewPoint = new PointClass();
                pNewPoint.PutCoords(e.mapX, e.mapY);
            
            }
            //this.axMapControl1.Extent = this.axMapControl1.TrackRectangle();
            else if (e.button == 2)//右键
                this.axMapControl1.Pan();
        }

        private void axMapControl1_OnExtentUpdated(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnExtentUpdatedEvent e)
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

        private void axMapControl2_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
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
        }

        private void axMapControl2_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
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

        private void axMapControl1_OnMapReplaced(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMapReplacedEvent e)
        {
            //获取鹰眼图层
            this.axMapControl2.AddLayer(this.GetOverviewLayer(this.axMapControl1.Map));
            //设置 MapControl 显示范围至数据的全局范围
            this.axMapControl2.Extent = this.axMapControl1.FullExtent;
            // 刷新鹰眼控件地图
            this.axMapControl2.Refresh();
        }

        private void axMapControl1_OnFullExtentUpdated(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnFullExtentUpdatedEvent e)
        {
            //获取鹰眼图层
            this.axMapControl2.AddLayer(this.GetOverviewLayer(this.axMapControl1.Map));
            // 设置 MapControl 显示范围至数据的全局范围
            this.axMapControl2.Extent = this.axMapControl1.FullExtent;
            // 刷新鹰眼控件地图
            this.axMapControl2.Refresh();
        }

        private void axMapControl1_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {


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

        private void axMapControl1_OnDoubleClick(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnDoubleClickEvent e)
        {

            //设置三维视角
            IActiveView pActiveView = axMapControl1.ActiveView;
            //IActiveView pActiveView1 = this.axMapControl1 as IActiveView;   //获取当前二维活动区域               
            //IEnvelope pEnv = pActiveView1 as IEnvelope;      //将此二位区域的Extent 保存在Envelope中
            IActiveView pActiveView1 = this.axMapControl1.Map as IActiveView;   //获取当前二维活动区域               
            IEnvelope pEnv = pActiveView1.Extent as IEnvelope;      //将此二位区域的Extent 保存在Envelope中
            IPoint point = new PointClass();        //将此区域的中心点保存起来
            point= GetGeo(pActiveView,(pEnv.XMax + pEnv.XMin) / 2, (pEnv.YMax + pEnv.YMin) / 2);  //取得视角中心点X坐标

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
            pCamera.Inclination = 15;       //设置三维场景视角，也就是高度角，视线与地面所成的角度
            pCamera.Azimuth = 180;          //设置三维场景方位角，视线与向北的方向所成的角度
            axSceneControl1.SceneGraph.RefreshViewers();        //刷新地图，（很多时候，看不到效果，都是你没有刷新）

            System.Timers.Timer t = new System.Timers.Timer();//500ms空隙
            t.Elapsed += new System.Timers.ElapsedEventHandler(sceneRotate);//调用函数
            t.AutoReset = false;//是否循环调用
            t.Enabled = true;//是否调用
            t.Start();
        }

        private void sceneRotate(object sender, System.Timers.ElapsedEventArgs e)
        {
            ICamera pCamera = this.axSceneControl1.Camera;      //取得三维活动区域的Camara      ，就像你照相一样的视角，它有Taget（目标点）和Observer（观察点）两个属性需要设置    
            IPoint ptObserver = new PointClass();
            ptObserver = pCamera.Observer;
            for (double i = 0; i < 0.1;)
            {
                 i += 0.001;
                ptObserver.X += i;
                //ptObserver.Y += i;

                pCamera.Observer = ptObserver;
                axSceneControl1.SceneGraph.RefreshViewers();        //刷新地图，（很多时候，看不到效果，都是你没有刷新）
                System.Threading.Thread.Sleep(30);//停顿2秒钟  
            }
        }
    }
}
