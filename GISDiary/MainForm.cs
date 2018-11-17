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
    public partial class MainForm : Form
    {
        public MainForm()
        {

            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
           
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

        private void showGrave()
        {

            //十字丝定位放大

            //子窗体展示
            Form_showGrave fn = new Form_showGrave();
            fn.MdiParent = this;
            fn.StartPosition = FormStartPosition.CenterScreen;
            fn.Show();
            SetParent((int)fn.Handle, (int)this.Handle);


        }

        private void btn_show_Click(object sender, EventArgs e)
        {
            showGrave();
        }

        private void btn_Form1_Click(object sender, EventArgs e)
        {

            showForm();
        }
        private void showForm()
        {

            //十字丝定位放大

            //子窗体展示
            


        }

        private void button1_Click(object sender, EventArgs e)
        {

            FormGlobe f3 = new FormGlobe();
            f3.MdiParent = this;
            f3.StartPosition = FormStartPosition.CenterScreen;
            f3.Show();
            SetParent((int)f3.Handle, (int)this.Handle);
        }

    }
}
