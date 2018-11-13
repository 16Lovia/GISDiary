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


namespace GISDiary
{
    public partial class Form0 : Form
    {
        public Form0()
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
            Form1 f1 = new Form1();
            f1.MdiParent = this;
            f1.StartPosition = FormStartPosition.CenterScreen;
            f1.Show();
            SetParent((int)f1.Handle, (int)this.Handle);



        }

        private void button2_Click(object sender, EventArgs e)
        {
            //总体展示
            MainForm f2 = new MainForm();
            f2.MdiParent = this;
            f2.StartPosition = FormStartPosition.CenterScreen;
            f2.Show();
            SetParent((int)f2.Handle, (int)this.Handle);


        }

        private void button3_Click(object sender, EventArgs e)
        {
            //难度展示
            Form3 f3= new Form3();
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
            Form5 f5 = new Form5();
            f5.MdiParent = this;
            f5.StartPosition = FormStartPosition.CenterScreen;
            f5.Show();
            SetParent((int)f5.Handle, (int)this.Handle);
        }
    }
}
