using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISDiary
{
    public partial class Form_Full : Form
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
        private void Form_Full_Load(object sender, EventArgs e)
        {
            string file2d = @"res\china\china.mxd";
            axMapControl1.LoadMxFile(file2d);
            axMapControl1.Extent = axMapControl1.FullExtent;
            //string file3d = @"res\china3d\china3d.sxd";
           // axSceneControl1.LoadSxFile(file3d);
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

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            

        }
        private Thread videothread;
        private void axWindowsMediaPlayer1_StatusChange(object sender, EventArgs e)
        {
            //判断视频是否已停止播放  
            if ((int)axWindowsMediaPlayer1.playState == 1)
            {
                //停顿2秒钟再重新播放  
                //System.Threading.Thread.Sleep(200);
                //重新播放  
                //windowsMediaPlay.Ctlcontrols.play();
                this.videothread = new Thread(new ThreadStart(this.VideoClose)); //另开线程安全改变控件可见性
                this.videothread.Start();
            }
            else if ((int)axWindowsMediaPlayer1.playState == 3)
            {
                axWindowsMediaPlayer1.fullScreen = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //System.Timers.Timer t = new System.Timers.Timer(10000);//10000ms空隙
            axWindowsMediaPlayer1.URL = @"res\3d.mp4";//连接视频
                                                      // t.Elapsed += new System.Timers.ElapsedEventHandler(Load3D);//调用函数
                                                      // t.AutoReset = false;//是否循环调用
                                                      // t.Enabled= true;//是否调用
        }
    }
}
