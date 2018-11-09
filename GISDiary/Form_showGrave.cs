using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GISDiary
{
    public partial class Form_showGrave : Form
    {
        public Form_showGrave()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            


        }

        private void Form_showGrave_Load(object sender, EventArgs e)
        {
            //三维展示+真实图片

            //难度系数

            //墓的历史（文字）

            //物件展示（图片列）

        }

        
    }
}
