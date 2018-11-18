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
            //总体展示
            Form_Full f2 = new Form_Full();
            f2.MdiParent = this;
            f2.StartPosition = FormStartPosition.CenterScreen;
            f2.Show();
            SetParent((int)f2.Handle, (int)this.Handle);
            //axWindowsMediaPlayer1.URL = @"res\3d.mp4";//连接视频
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //难度展示
            Form_difficulty f3 = new Form_difficulty();
            f3.MdiParent = this;
            f3.StartPosition = FormStartPosition.CenterScreen;
            f3.Show();
            SetParent((int)f3.Handle, (int)this.Handle);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            //墓穴展示
            Form_showGrave f4 = new Form_showGrave();
            f4.MdiParent = this;
            f4.StartPosition = FormStartPosition.CenterScreen;
            f4.Show();
            SetParent((int)f4.Handle, (int)this.Handle);
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
    }
}
