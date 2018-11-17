using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.NetworkAnalysis;
using ESRI.ArcGIS.Display;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.NetworkAnalyst;
using System.Collections;
using System.Timers;
using System.Threading;
using ESRI.ArcGIS.Analyst3D;

namespace GISDiary
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
          
            InitializeComponent();
          //  this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
           // this.skinEngine1.SkinFile = Application.StartupPath + "//EighteenColor2.ssk";
            
           // this.skinEngine1.SkinFile = Application.StartupPath + "//WaveColor2.ssk";
            //Sunisoft.IrisSkin.SkinEngine se = null;
            //se = new Sunisoft.IrisSkin.SkinEngine();
            //se.SkinAllForm = true;
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



                //pEnv.XMax = e.X + 5;
                //pEnv.XMin = e.X - 5;
                //pEnv.YMax = e.Y + 5;
                //pEnv.YMin = e.Y - 5;

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


        private void MainForm_Load(object sender, EventArgs e)
        {
           
        }

        private void VideoClose()
        {
            this.videoclose(false);
        }
        
        delegate void SetVisibleCore(bool videostate);
        private void videoclose(bool videostate)
        {
            if (this.axWindowsMediaPlayer1.InvokeRequired)
            {           
               
                SetVisibleCore v = new SetVisibleCore(videoclose);               
                this.Invoke(v, new object[] { videostate });
           
            }
            else
            {

                this.axWindowsMediaPlayer1.Visible = videostate;
            }
        }
        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)//鼠标选点
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
            
            //坐标显示
            //label1.Text = " 当前坐标 X = " + e.mapX.ToString() + " Y = " + e.mapY.ToString() + " " + this.axMapControl1.MapUnits.ToString().Substring(4);
        }

      

     

       


        //方法2
        private NetWork m_ipPathFinder;
        private Thread videothread;

        private void NetRoad()
        {
            // 备注：在调用该类时的次序

            //if (m_ipPathFinder == null)//打开几何网络工作空间
            //{
            //    m_ipPathFinder.m_ipMap = this.axMapControl1.ActiveView.FocusMap;
            //    ILayer ipLayer = m_ipPathFinder.m_ipMap.get_Layer(1);
            //    IFeatureLayer ipFeatureLayer = ipLayer as IFeatureLayer;
            //    IFeatureDataset ipFDB = ipFeatureLayer.FeatureClass.FeatureDataset;
            //    m_ipPathFinder.SetOrGetMap = m_ipPathFinder.m_ipMap;
            //    m_ipPathFinder.OpenFeatureDatasetNetwork(ipFDB);
            //}
            m_ipPathFinder.m_ipMap = this.axMapControl1.ActiveView.FocusMap;
            ILayer ipLayer = m_ipPathFinder.m_ipMap.get_Layer(0);
            IFeatureLayer ipFeatureLayer = ipLayer as IFeatureLayer;
            IFeatureDataset ipFDB = ipFeatureLayer.FeatureClass.FeatureDataset;
            m_ipPathFinder.SetOrGetMap = m_ipPathFinder.m_ipMap;
            m_ipPathFinder.OpenFeatureDatasetNetwork(ipFDB);
            //m_ipPathFinder.m_ipPoints = mPointCollection;
            m_ipPathFinder.SolvePath("length");//先解析路径

            IPolyline ipPolyResult = m_ipPathFinder.PathPolyLine();//最后返回最短路径

        }

        //-----------------------------------
        /// <summary>
        /// //墓穴展示
        /// </summary>
        /// <param name="hWndChild"></param>
        /// <param name="hWndNewParent"></param>
        /// <returns></returns>

        
        

        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);
        public static int longitude;//public类型的实例字段  /********************/
        public Form_showGrave fn = new Form_showGrave();

        //关闭子窗口
        private void CloseGrave(object sender, ElapsedEventArgs e)
        {
            //Form_showGrave fn = new Form_showGrave();
            fn.Close();
        }
        private void showGrave()
        {

            //十字丝定位放大

            //子窗体展示
            //Form_showGrave fn = new Form_showGrave();
            longitude = 117;/*****************/
            fn.MdiParent = this;
            fn.StartPosition = FormStartPosition.CenterScreen;
            fn.Show();
            SetParent((int)fn.Handle, (int)this.Handle);

            //一段时间后关闭
             //System.Timers.Timer t = new System.Timers.Timer(20);//10000ms空隙
             //t.Elapsed += new System.Timers.ElapsedEventHandler(CloseGrave);//调用函数
             //t.AutoReset = false;//是否循环调用
             //t.Enabled= true;//是否调用
        }
       
        private void btn_show_Click(object sender, EventArgs e)
        {
            showGrave();

            CheckForIllegalCrossThreadCalls = false;
            System.Timers.Timer t = new System.Timers.Timer(3000);//10000ms空隙
            t.Elapsed += new System.Timers.ElapsedEventHandler(CloseGrave);//调用函数
            t.AutoReset = false;//是否循环调用
            t.Enabled = true;//是否调用

        }

        private void btn_Form1_Click(object sender, EventArgs e)
        {

            showForm();
        }
        private void showForm()
        {


      


        }


        private void btn_video_Click(object sender, EventArgs e)
        {
            
            axWindowsMediaPlayer1.URL = @"res\3d.mp4";//连接视频
            //System.Timers.Timer t = new System.Timers.Timer(10000);//10000ms空隙
           // t.Elapsed += new System.Timers.ElapsedEventHandler(Load3D);//调用函数
           // t.AutoReset = false;//是否循环调用
           // t.Enabled= true;//是否调用
        
        }

        private void axWindowsMediaPlayer1_StatusChange(object sender, EventArgs e)
        {
            //判断视频是否已停止播放  
            if ((int)axWindowsMediaPlayer1.playState == 1)
            {      
               //重新播放  
                //windowsMediaPlay.Ctlcontrols.play();
                this.videothread = new Thread(new ThreadStart(this.VideoClose)); //另开线程安全改变控件可见性
                this.videothread.Start();

                System.Threading.Thread.Sleep(200);//停顿2秒钟  
                //子窗体展示
                Form_Full f1 = new Form_Full();
                f1.MdiParent = this;
                f1.StartPosition = FormStartPosition.CenterScreen;
                f1.Show();
                SetParent((int)f1.Handle, (int)this.Handle);
                //string file2d = @"res\china\china.mxd";
                //axMapControl1.LoadMxFile(file2d);
                //axMapControl1.Extent = axMapControl1.FullExtent;
                // string file3d = @"res\china3d\china3d.sxd";
                //axSceneControl1.LoadSxFile(file3d);

            }
            else if ((int)axWindowsMediaPlayer1.playState == 3)
            {
                axWindowsMediaPlayer1.fullScreen = true;
            }
        }

        private void axSceneControl1_OnSceneReplaced(object sender, ESRI.ArcGIS.Controls.ISceneControlEvents_OnSceneReplacedEvent e)
        {
  

        }

        private void axSceneControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ISceneControlEvents_OnMouseDownEvent e)
        {
            ICamera pCamera = this.axSceneControl1.Camera;
            IPoint point = pCamera.Target;
            IEnvelope pEnv = new EnvelopeClass();
            pEnv.XMax = point.X + 2;
            pEnv.XMin = point.X - 2;
            pEnv.YMax = point.Y + 2;
            pEnv.YMin = point.Y - 2;

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
            pOutline.Width = 1;
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

        private void button1_Click(object sender, EventArgs e)
        {
            //子窗体展示
            Form_difficluty f2 = new Form_difficluty();
            f2.MdiParent = this;
            f2.StartPosition = FormStartPosition.CenterScreen;
            f2.Show();
            SetParent((int)f2.Handle, (int)this.Handle);
        }

        private void axMapControl1_OnExtentUpdated(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnExtentUpdatedEvent e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //子窗体展示
            Form_Full f2 = new Form_Full();
            f2.MdiParent = this;
            f2.StartPosition = FormStartPosition.CenterScreen;
            f2.Show();
            SetParent((int)f2.Handle, (int)this.Handle);
        }
    }
}
