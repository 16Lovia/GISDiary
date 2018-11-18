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
using System.Threading;
using CCWin;
using System.Timers;

namespace GISDiary
{
    public partial class FormStart : Skin_DevExpress
    {
        public FormStart()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 子窗体跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);

        private void button1_Click(object sender, EventArgs e)
        {
            //Globe动画
            FormGlobe f1 = new FormGlobe();
            f1.MdiParent = this;
            f1.StartPosition = FormStartPosition.CenterScreen;
            f1.Show();
            SetParent((int)f1.Handle, (int)this.Handle);



        }

        private void button2_Click(object sender, EventArgs e)
        {
            //总体展示
            Form_Full f2 = new Form_Full();
            f2.MdiParent = this;
            f2.StartPosition = FormStartPosition.CenterScreen;
            f2.Show();
            SetParent((int)f2.Handle, (int)this.Handle);
            //axWindowsMediaPlayer1.URL = @"res\3d.mp4";//连接视频

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //难度展示
            Form_difficulty f3 = new Form_difficulty();
            f3.MdiParent = this;
            f3.StartPosition = FormStartPosition.CenterScreen;
            f3.Show();
            SetParent((int)f3.Handle, (int)this.Handle);


        }

        private void button4_Click(object sender, EventArgs e)
        {
            //墓穴展示
            Form_showGrave f4 = new Form_showGrave();
            f4.MdiParent = this;
            f4.StartPosition = FormStartPosition.CenterScreen;
            f4.Show();
            SetParent((int)f4.Handle, (int)this.Handle);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //路径分析
            FormRoute f5 = new FormRoute();
            f5.MdiParent = this;
            f5.StartPosition = FormStartPosition.CenterScreen;
            f5.Show();
            SetParent((int)f5.Handle, (int)this.Handle);
        }
        private Thread videothread;

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
                Form_difficulty f3 = new Form_difficulty();
                f3.MdiParent = this;
                f3.StartPosition = FormStartPosition.CenterScreen;
                f3.Show();
                SetParent((int)f3.Handle, (int)this.Handle);
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

        private void button_test_Click(object sender, EventArgs e)
        {
            //test
            test f5 = new test();
            f5.MdiParent = this;
            f5.StartPosition = FormStartPosition.CenterScreen;
            f5.Show();
            SetParent((int)f5.Handle, (int)this.Handle);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //Globe动画
            FormGlobe f1 = new FormGlobe();
            f1.MdiParent = this;
            f1.StartPosition = FormStartPosition.CenterScreen;
            f1.Show();
            SetParent((int)f1.Handle, (int)this.Handle);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           // difficultyAll1.Visible = true;
            
            System.Timers.Timer t = new System.Timers.Timer(5000);//10000ms空隙
            longitude = 117;
            showGrave();
            //Thread.Sleep(3000);

            //CheckForIllegalCrossThreadCalls = false;
            //t = new System.Timers.Timer(3000);//10000ms空隙
            //t.Elapsed += new System.Timers.ElapsedEventHandler(CloseGrave);//调用函数
            //t.AutoReset = false;//是否循环调用
            //t.Enabled = true;//是否调用

            fn = new Form_showGrave();
            longitude = 111;
            showGrave();
            Thread.Sleep(1000);
            //CheckForIllegalCrossThreadCalls = false;
            ////   t = new System.Timers.Timer(5000);//10000ms空隙
            //t = new System.Timers.Timer(3000);//10000ms空隙
            //t.Elapsed += new System.Timers.ElapsedEventHandler(CloseGrave);//调用函数
            //t.AutoReset = false;//是否循环调用
            //t.Enabled = true;//是否调用

            fn = new Form_showGrave();
            longitude = 109;
            showGrave();
            Thread.Sleep(1000);
            //CheckForIllegalCrossThreadCalls = false;
            ////    t = new System.Timers.Timer(5000);//10000ms空隙
            //t = new System.Timers.Timer(8000);//10000ms空隙
            //t.Elapsed += new System.Timers.ElapsedEventHandler(CloseGrave);//调用函数
            //t.AutoReset = false;//是否循环调用
            //t.Enabled = true;//是否调用

            fn = new Form_showGrave();
            longitude = 128;
            showGrave();
            Thread.Sleep(1000);
            //CheckForIllegalCrossThreadCalls = false;
            ////   t = new System.Timers.Timer(5000);//10000ms空隙
            //t = new System.Timers.Timer(8000);//10000ms空隙
            //t.Elapsed += new System.Timers.ElapsedEventHandler(CloseGrave);//调用函数
            //t.AutoReset = false;//是否循环调用
            //t.Enabled = true;//是否调用

            fn = new Form_showGrave();
            longitude = 91;
            showGrave();
            Thread.Sleep(1000);
            //CheckForIllegalCrossThreadCalls = false;
            ////   t = new System.Timers.Timer(5000);//10000ms空隙
            //t = new System.Timers.Timer(8000);//10000ms空隙
            //t.Elapsed += new System.Timers.ElapsedEventHandler(CloseGrave);//调用函数
            //t.AutoReset = false;//是否循环调用
            //t.Enabled = true;//是否调用

            fn = new Form_showGrave();
            longitude = 107;
            showGrave();
            Thread.Sleep(1000);
            //CheckForIllegalCrossThreadCalls = false;
            //// t = new System.Timers.Timer(5000);//10000ms空隙
            //t = new System.Timers.Timer(8000);//10000ms空隙
            //t.Elapsed += new System.Timers.ElapsedEventHandler(CloseGrave);//调用函数
            //t.AutoReset = false;//是否循环调用
            //t.Enabled = true;//是否调用
           
        }

        public Form_showGrave fn = new Form_showGrave();
        public static int longitude;


        private void CloseGrave(object sender, ElapsedEventArgs e)
        {
            //Form_showGrave fn = new Form_showGrave();
            //fn.Close();
        }
        private void showGrave()
        {

            //十字丝定位放大
            //子窗体展示
            //longitude = 117;/*****************/
            //Form_showGrave fn = new Form_showGrave();
            fn.MdiParent = this;
            fn.StartPosition = FormStartPosition.CenterScreen;
            fn.Show();
            SetParent((int)fn.Handle, (int)this.Handle);
            // Thread.Sleep(10000);
            //资料卡停靠显示

            //      fn.Close();

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //难度展示
            // axWindowsMediaPlayer2.URL = @"res\3d.mp4";//连接视频
            Form_difficulty f3 = new Form_difficulty();
            f3.MdiParent = this;
            f3.StartPosition = FormStartPosition.CenterScreen;
            f3.Show();
            SetParent((int)f3.Handle, (int)this.Handle);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ////墓穴展示
            //Form_showGrave f4 = new Form_showGrave();
            //f4.MdiParent = this;
            //f4.StartPosition = FormStartPosition.CenterScreen;
            //f4.Show();
            //SetParent((int)f4.Handle, (int)this.Handle);

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //路径分析
            FormRoute f5 = new FormRoute();
            f5.MdiParent = this;
            f5.StartPosition = FormStartPosition.CenterScreen;
            f5.Show();
            SetParent((int)f5.Handle, (int)this.Handle);
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            // 设置显示样式
            skinToolTip1.AutoPopDelay = 5000;//提示信息的可见时间
            skinToolTip1.InitialDelay = 500;//事件触发多久后出现提示
            skinToolTip1.ReshowDelay = 500;//指针从一个控件移向另一个控件时，经过多久才会显示下一个提示框
            skinToolTip1.ShowAlways = true;//是否显示提示框
            skinToolTip1.SetToolTip(pictureBox1, "二三维路线展示&难度专题&预测");//设置提示按钮和提示内容
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            // 设置显示样式
            skinToolTip2.AutoPopDelay = 5000;//提示信息的可见时间
            skinToolTip2.InitialDelay = 500;//事件触发多久后出现提示
            skinToolTip2.ReshowDelay = 500;//指针从一个控件移向另一个控件时，经过多久才会显示下一个提示框
            skinToolTip2.ShowAlways = true;//是否显示提示框
            skinToolTip2.SetToolTip(pictureBox4, "路径分析");//设置提示按钮和提示内容
        }

        private void axWindowsMediaPlayer2_StatusChange(object sender, EventArgs e)
        {
            //判断视频是否已停止播放  
            if ((int)axWindowsMediaPlayer2.playState == 1)
            {
                //重新播放  
                //windowsMediaPlay.Ctlcontrols.play();
                this.videothread = new Thread(new ThreadStart(this.VideoClose)); //另开线程安全改变控件可见性
                this.videothread.Start();

                System.Threading.Thread.Sleep(200);//停顿2秒钟  
                                                   //子窗体展示
                Form_difficulty f3 = new Form_difficulty();
                f3.MdiParent = this;
                f3.StartPosition = FormStartPosition.CenterScreen;
                f3.Show();
                SetParent((int)f3.Handle, (int)this.Handle);
                //string file2d = @"res\china\china.mxd";
                //axMapControl1.LoadMxFile(file2d);
                //axMapControl1.Extent = axMapControl1.FullExtent;
                // string file3d = @"res\china3d\china3d.sxd";
                //axSceneControl1.LoadSxFile(file3d);
            }
            else if ((int)axWindowsMediaPlayer2.playState == 3)
            {
                axWindowsMediaPlayer2.fullScreen = true;
            }
        }

        private void FormStart_Load(object sender, EventArgs e)
        {
            
        }
    }
}
