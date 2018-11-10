using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FControlEve
{
    public partial class DifficultyEve: UserControl
    {
        public DifficultyEve()
        {
            InitializeComponent();
        }
        private void btnCount_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            for (int i = 0; i < 30; i++)
            {
                progressBar1.Value++;
                Application.DoEvents();
                this.label1.Text = Convert.ToString(progressBar1.Value);
            }

            label2.Visible = true;
            progressBar2.Visible = true;
            progressBar2.Minimum = 0;
            progressBar2.Maximum = 100;
            for (int i = 0; i < 50; i++)
            {
                progressBar2.Value++;
                Application.DoEvents();
                this.label2.Text = Convert.ToString(progressBar2.Value);
            }
            label3.Visible = true;
            progressBar3.Visible = true;
            progressBar3.Minimum = 0;
            progressBar3.Maximum = 100;
            for (int i = 0; i < 80; i++)
            {
                progressBar3.Value++;
                Application.DoEvents();
                this.label3.Text = Convert.ToString(progressBar3.Value);
            }
            label4.Visible = true;
            progressBar4.Visible = true;
            progressBar4.Minimum = 0;
            progressBar4.Maximum = 100;
            for (int i = 0; i < 45; i++)
            {
                progressBar4.Value++;
                Application.DoEvents();
                this.label4.Text = Convert.ToString(progressBar4.Value);
            }
            label5.Visible = true;
            progressBar5.Visible = true;
            progressBar5.Minimum = 0;
            progressBar5.Maximum = 100;
            for (int i = 0; i < 90; i++)
            {
                progressBar5.Value++;
                Application.DoEvents();
                this.label5.Text = Convert.ToString(progressBar5.Value);
            }
            label12.Visible = true;
            progressBar6.Visible = true;
            progressBar6.Minimum = 0;
            progressBar6.Maximum = 100;
            for (int i = 0; i < 90; i++)
            {
                progressBar6.Value++;
                Application.DoEvents();
                this.label12.Text = Convert.ToString(progressBar6.Value);
            }
        }
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
