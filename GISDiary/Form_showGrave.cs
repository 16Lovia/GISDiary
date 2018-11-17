using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.esriSystem;
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
            //ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            


        }

        private void Form_showGrave_Load(object sender, EventArgs e)
        {
            //三维展示+真实图片

            //难度系数

            //墓的历史（文字）

            //物件展示（图片列）
            openFileDialog1.Filter = "Scene文档(*.sxd)|*.sxd";
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            if (axSceneControl1.CheckSxFile(filename))
                axSceneControl1.LoadSxFile(filename);
            else
            {
                IScene pScene = axSceneControl1.Scene;
                IMemoryBlobStream mbStream = new MemoryBlobStreamClass();
                IObjectStream objectStream = new ObjectStreamClass();
                mbStream.LoadFromFile(filename);
                IPersistStream pPersistStream = (ESRI.ArcGIS.esriSystem.IPersistStream)pScene;
                objectStream.Stream = mbStream;
                pPersistStream.Load(objectStream);
                }


            }

        private void axSceneControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ISceneControlEvents_OnMouseDownEvent e)
        {

        }
    }
}
