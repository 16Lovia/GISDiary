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
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            

        }

        private void Form_showGrave_Load(object sender, EventArgs e)
        {
            //三维展示+真实图片

            //难度系数
            int longi = 0;
            //MainForm f1 = new MainForm();
            longi = MainForm.longitude;
            switch (longi)
            {
                case 117:
                    {
                        pictureBox1.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label16.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label15.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label14.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label13.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label17.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label1.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label22.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label21.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label20.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label19.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label18.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label2.Image = Image.FromFile("D:\\墓穴资料卡\\七星鲁王宫.png");
                        label22.Text = null;
                        label21.Text = null;
                        label20.Text = null;
                        label19.Text = null;
                        label18.Text = null;
                        label2.Text = null;
                        label22.Visible = true;
                        progressBar1.Visible = true;
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = 100;
                        for (int i = 0; i < 12; i++)
                        {
                            progressBar1.Value++;
                            Application.DoEvents();                          
                            this.label22.Text = Convert.ToString(progressBar1.Value);
                        }

                        label21.Visible = true;
                        progressBar2.Visible = true;
                        progressBar2.Minimum = 0;
                        progressBar2.Maximum = 100;
                        for (int i = 0; i < 33; i++)
                        {
                            progressBar2.Value++;
                            Application.DoEvents();
                            this.label21.Text = Convert.ToString(progressBar2.Value);
                        }
                        label20.Visible = true;
                        progressBar3.Visible = true;
                        progressBar3.Minimum = 0;
                        progressBar3.Maximum = 100;
                        for (int i = 0; i < 23; i++)
                        {
                            progressBar3.Value++;
                            Application.DoEvents();
                            this.label20.Text = Convert.ToString(progressBar3.Value);
                        }
                        label19.Visible = true;
                        progressBar4.Visible = true;
                        progressBar4.Minimum = 0;
                        progressBar4.Maximum = 100;
                        for (int i = 0; i < 44; i++)
                        {
                            progressBar4.Value++;
                            Application.DoEvents();
                            this.label19.Text = Convert.ToString(progressBar4.Value);
                        }
                        label18.Visible = true;
                        progressBar5.Visible = true;
                        progressBar5.Minimum = 0;
                        progressBar5.Maximum = 100;
                        for (int i = 0; i < 51; i++)
                        {
                            progressBar5.Value++;
                            Application.DoEvents();
                            this.label18.Text = Convert.ToString(progressBar5.Value);
                        }
                        label2.Visible = true;
                        progressBar6.Visible = true;
                        progressBar6.Minimum = 0;
                        progressBar6.Maximum = 100;
                        for (int i = 0; i < 33; i++)
                        {
                            progressBar6.Value++;
                            Application.DoEvents();
                            this.label2.Text = Convert.ToString(progressBar6.Value);
                        }
                        break;
                    }
                case 111:
                    {
                        pictureBox1.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label16.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label15.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label14.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label13.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label17.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label1.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label22.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label21.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label20.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label19.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label18.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");
                        label2.Image = Image.FromFile("D:\\墓穴资料卡\\西沙海底墓.png");

                        label22.Visible = true;
                        label22.Text = null;
                        label21.Text = null;
                        label20.Text = null;
                        label19.Text = null;
                        label18.Text = null;
                        label2.Text = null;
                        label22.Visible = true;
                        progressBar1.Visible = true;
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = 100;
                        for (int i = 0; i < 80; i++)
                        {
                            progressBar1.Value++;
                            Application.DoEvents();
                            this.label22.Text = Convert.ToString(progressBar1.Value);
                        }

                        label21.Visible = true;
                        progressBar2.Visible = true;
                        progressBar2.Minimum = 0;
                        progressBar2.Maximum = 100;
                        for (int i = 0; i < 91; i++)
                        {
                            progressBar2.Value++;
                            Application.DoEvents();
                            this.label21.Text = Convert.ToString(progressBar2.Value);
                        }
                        label20.Visible = true;
                        progressBar3.Visible = true;
                        progressBar3.Minimum = 0;
                        progressBar3.Maximum = 100;
                        for (int i = 0; i < 76; i++)
                        {
                            progressBar3.Value++;
                            Application.DoEvents();
                            this.label20.Text = Convert.ToString(progressBar3.Value);
                        }
                        label19.Visible = true;
                        progressBar4.Visible = true;
                        progressBar4.Minimum = 0;
                        progressBar4.Maximum = 100;
                        for (int i = 0; i < 84; i++)
                        {
                            progressBar4.Value++;
                            Application.DoEvents();
                            this.label19.Text = Convert.ToString(progressBar4.Value);
                        }
                        label18.Visible = true;
                        progressBar5.Visible = true;
                        progressBar5.Minimum = 0;
                        progressBar5.Maximum = 100;
                        for (int i = 0; i < 67; i++)
                        {
                            progressBar5.Value++;
                            Application.DoEvents();
                            this.label18.Text = Convert.ToString(progressBar5.Value);
                        }
                        label2.Visible = true;
                        progressBar6.Visible = true;
                        progressBar6.Minimum = 0;
                        progressBar6.Maximum = 100;
                        for (int i = 0; i < 80; i++)
                        {
                            progressBar6.Value++;
                            Application.DoEvents();
                            this.label2.Text = Convert.ToString(progressBar6.Value);
                        }
                        break;
                    }
                case 109:
                    {
                        pictureBox1.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label16.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label15.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label14.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label13.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label17.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label1.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label22.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label21.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label20.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label19.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label18.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label2.Image = Image.FromFile("D:\\墓穴资料卡\\秦岭神树.png");
                        label22.Text = null;
                        label21.Text = null;
                        label20.Text = null;
                        label19.Text = null;
                        label18.Text = null;
                        label2.Text = null;
                        label22.Visible = true;
                        progressBar1.Visible = true;
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = 100;
                        for (int i = 0; i < 52; i++)
                        {
                            progressBar1.Value++;
                            Application.DoEvents();
                            this.label22.Text = Convert.ToString(progressBar1.Value);
                        }

                        label21.Visible = true;
                        progressBar2.Visible = true;
                        progressBar2.Minimum = 0;
                        progressBar2.Maximum = 100;
                        for (int i = 0; i < 67; i++)
                        {
                            progressBar2.Value++;
                            Application.DoEvents();
                            this.label21.Text = Convert.ToString(progressBar2.Value);
                        }
                        label20.Visible = true;
                        progressBar3.Visible = true;
                        progressBar3.Minimum = 0;
                        progressBar3.Maximum = 100;
                        for (int i = 0; i < 59; i++)
                        {
                            progressBar3.Value++;
                            Application.DoEvents();
                            this.label20.Text = Convert.ToString(progressBar3.Value);
                        }
                        label19.Visible = true;
                        progressBar4.Visible = true;
                        progressBar4.Minimum = 0;
                        progressBar4.Maximum = 100;
                        for (int i = 0; i < 71; i++)
                        {
                            progressBar4.Value++;
                            Application.DoEvents();
                            this.label19.Text = Convert.ToString(progressBar4.Value);
                        }
                        label18.Visible = true;
                        progressBar5.Visible = true;
                        progressBar5.Minimum = 0;
                        progressBar5.Maximum = 100;
                        for (int i = 0; i < 65; i++)
                        {
                            progressBar5.Value++;
                            Application.DoEvents();
                            this.label18.Text = Convert.ToString(progressBar5.Value);
                        }
                        label2.Visible = true;
                        progressBar6.Visible = true;
                        progressBar6.Minimum = 0;
                        progressBar6.Maximum = 100;
                        for (int i = 0; i < 62; i++)
                        {
                            progressBar6.Value++;
                            Application.DoEvents();
                            this.label17.Text = Convert.ToString(progressBar6.Value);
                        }
                        break;
                    }
                case 128:
                    {
                        pictureBox1.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label16.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label15.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label14.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label13.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label17.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label1.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label22.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label21.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label20.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label19.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label18.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label2.Image = Image.FromFile("D:\\墓穴资料卡\\云顶天宫.png");
                        label22.Text = null;
                        label21.Text = null;
                        label20.Text = null;
                        label19.Text = null;
                        label18.Text = null;
                        label2.Text = null;
                        label22.Visible = true;
                        progressBar1.Visible = true;
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = 100;
                        for (int i = 0; i < 52; i++)
                        {
                            progressBar1.Value++;
                            Application.DoEvents();
                            this.label22.Text = Convert.ToString(progressBar1.Value);
                        }

                        label21.Visible = true;
                        progressBar2.Visible = true;
                        progressBar2.Minimum = 0;
                        progressBar2.Maximum = 100;
                        for (int i = 0; i < 67; i++)
                        {
                            progressBar2.Value++;
                            Application.DoEvents();
                            this.label21.Text = Convert.ToString(progressBar2.Value);
                        }
                        label20.Visible = true;
                        progressBar3.Visible = true;
                        progressBar3.Minimum = 0;
                        progressBar3.Maximum = 100;
                        for (int i = 0; i < 59; i++)
                        {
                            progressBar3.Value++;
                            Application.DoEvents();
                            this.label20.Text = Convert.ToString(progressBar3.Value);
                        }
                        label19.Visible = true;
                        progressBar4.Visible = true;
                        progressBar4.Minimum = 0;
                        progressBar4.Maximum = 100;
                        for (int i = 0; i < 71; i++)
                        {
                            progressBar4.Value++;
                            Application.DoEvents();
                            this.label19.Text = Convert.ToString(progressBar4.Value);
                        }
                        label18.Visible = true;
                        progressBar5.Visible = true;
                        progressBar5.Minimum = 0;
                        progressBar5.Maximum = 100;
                        for (int i = 0; i < 65; i++)
                        {
                            progressBar5.Value++;
                            Application.DoEvents();
                            this.label18.Text = Convert.ToString(progressBar5.Value);
                        }
                        label2.Visible = true;
                        progressBar6.Visible = true;
                        progressBar6.Minimum = 0;
                        progressBar6.Maximum = 100;
                        for (int i = 0; i < 69; i++)
                        {
                            progressBar6.Value++;
                            Application.DoEvents();
                            this.label2.Text = Convert.ToString(progressBar6.Value);
                        }
                        break;
                    }
                case 91:
                    {
                        pictureBox1.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label16.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label15.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label14.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label13.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label17.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label1.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label22.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label21.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label20.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label19.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label18.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");
                        label2.Image = Image.FromFile("D:\\墓穴资料卡\\西王母宫.png");

                        label22.Text = null;
                        label21.Text = null;
                        label20.Text = null;
                        label19.Text = null;
                        label18.Text = null;
                        label2.Text = null;
                        label22.Visible = true;
                        progressBar1.Visible = true;
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = 100;
                        for (int i = 0; i < 30; i++)
                        {
                            progressBar1.Value++;
                            Application.DoEvents();
                            this.label22.Text = Convert.ToString(progressBar1.Value);
                        }

                        label21.Visible = true;
                        progressBar2.Visible = true;
                        progressBar2.Minimum = 0;
                        progressBar2.Maximum = 100;
                        for (int i = 0; i < 42; i++)
                        {
                            progressBar2.Value++;
                            Application.DoEvents();
                            this.label21.Text = Convert.ToString(progressBar2.Value);
                        }
                        label20.Visible = true;
                        progressBar3.Visible = true;
                        progressBar3.Minimum = 0;
                        progressBar3.Maximum = 100;
                        for (int i = 0; i < 51; i++)
                        {
                            progressBar3.Value++;
                            Application.DoEvents();
                            this.label20.Text = Convert.ToString(progressBar3.Value);
                        }
                        label19.Visible = true;
                        progressBar4.Visible = true;
                        progressBar4.Minimum = 0;
                        progressBar4.Maximum = 100;
                        for (int i = 0; i < 62; i++)
                        {
                            progressBar4.Value++;
                            Application.DoEvents();
                            this.label19.Text = Convert.ToString(progressBar4.Value);
                        }
                        label18.Visible = true;
                        progressBar5.Visible = true;
                        progressBar5.Minimum = 0;
                        progressBar5.Maximum = 100;
                        for (int i = 0; i < 61; i++)
                        {
                            progressBar5.Value++;
                            Application.DoEvents();
                            this.label18.Text = Convert.ToString(progressBar5.Value);
                        }
                        label2.Visible = true;
                        progressBar6.Visible = true;
                        progressBar6.Minimum = 0;
                        progressBar6.Maximum = 100;
                        for (int i = 0; i < 49; i++)
                        {
                            progressBar6.Value++;
                            Application.DoEvents();
                            this.label2.Text = Convert.ToString(progressBar6.Value);
                        }
                        break;
                    }
                case 107:
                    {
                        pictureBox1.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label16.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label15.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label14.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label13.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label17.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label1.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label22.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label21.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label20.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label19.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label18.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");
                        label2.Image = Image.FromFile("D:\\墓穴资料卡\\张家古楼.png");

                        label22.Text = null;
                        label21.Text = null;
                        label20.Text = null;
                        label19.Text = null;
                        label18.Text = null;
                        label2.Text = null;
                        label22.Visible = true;
                        progressBar1.Visible = true;
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = 100;
                        for (int i = 0; i < 19; i++)
                        {
                            progressBar1.Value++;
                            Application.DoEvents();
                            this.label22.Text = Convert.ToString(progressBar1.Value);
                        }

                        label21.Visible = true;
                        progressBar2.Visible = true;
                        progressBar2.Minimum = 0;
                        progressBar2.Maximum = 100;
                        for (int i = 0; i < 15; i++)
                        {
                            progressBar2.Value++;
                            Application.DoEvents();
                            this.label21.Text = Convert.ToString(progressBar2.Value);
                        }
                        label20.Visible = true;
                        progressBar3.Visible = true;
                        progressBar3.Minimum = 0;
                        progressBar3.Maximum = 100;
                        for (int i = 0; i < 8; i++)
                        {
                            progressBar3.Value++;
                            Application.DoEvents();
                            this.label20.Text = Convert.ToString(progressBar3.Value);
                        }
                        label19.Visible = true;
                        progressBar4.Visible = true;
                        progressBar4.Minimum = 0;
                        progressBar4.Maximum = 100;
                        for (int i = 0; i < 43; i++)
                        {
                            progressBar4.Value++;
                            Application.DoEvents();
                            this.label19.Text = Convert.ToString(progressBar4.Value);
                        }
                        label18.Visible = true;
                        progressBar5.Visible = true;
                        progressBar5.Minimum = 0;
                        progressBar5.Maximum = 100;
                        for (int i = 0; i < 21; i++)
                        {
                            progressBar5.Value++;
                            Application.DoEvents();
                            this.label18.Text = Convert.ToString(progressBar5.Value);
                        }
                        label2.Visible = true;
                        progressBar6.Visible = true;
                        progressBar6.Minimum = 0;
                        progressBar6.Maximum = 100;
                        for (int i = 0; i < 22; i++)
                        {
                            progressBar6.Value++;
                            Application.DoEvents();
                            this.label2.Text = Convert.ToString(progressBar6.Value);
                        }
                        break;
                    }
            }

            //墓的历史（文字）

            //物件展示（图片列）



        }
        class MyProgressBar : ProgressBar //新建一个MyProgressBar类，它继承了ProgressBar的所有属性与方法
        {
            public MyProgressBar()
            {
                base.SetStyle(ControlStyles.UserPaint, true);//使控件可由用户自由重绘
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                SolidBrush brush = null;
                Rectangle bounds = new Rectangle(0, 0, base.Width, base.Height);
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 1, 1, bounds.Width - 2, bounds.Height - 2);//此处完成背景重绘，并且按照属性中的BackColor设置背景色
                bounds.Height -= 4;
                bounds.Width = ((int)(bounds.Width * (((double)base.Value) / ((double)base.Maximum)))) - 4;//是的进度条跟着ProgressBar.Value值变化
                brush = new SolidBrush(this.ForeColor);
                e.Graphics.FillRectangle(brush, 2, 2, bounds.Width, bounds.Height);//此处完成前景重绘，依旧按照Progressbar的属性设置前景色
            }
        }
    }
}
