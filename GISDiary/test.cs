using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISDiary
{
    public partial class test : Skin_DevExpress
    {
        public test()
        {

            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();

        }

        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);
        private void skinPictureBox1_Click(object sender, EventArgs e)
        {
            string file2d = @"res\china\china_1.mxd";
            axMapControl1.LoadMxFile(file2d);
        }
    }
}
