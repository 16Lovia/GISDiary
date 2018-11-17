using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
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
    public partial class FormRoute : Form
    {
        public FormRoute()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            string file = @"res\china.mxd";
            axMapControl1.LoadMxFile(file);
        }

        private void btn_Dijskta_Click(object sender, EventArgs e)
        {
            startDijsktra();
        }


        /// <summary>
        /// Dijskta
        /// </summary>

        struct VertexType
        {
            public string VexNo;        //顶点编号
            public string VexName;        //顶点名称
            public string otherInfo;     //顶点其他信息     
            public IPoint SpatialPoi;    //空间坐标

        };                               //顶点类型

        struct MGraph                    //图的定义
        {
            public int[,] edges;       //邻接矩阵
            public int n, e;             //顶点数,弧数
            public VertexType[] vexs; //存放顶点信息
        };                               //图的邻接矩阵类型

        private const int INF = 32767;    //INF表示∞
        private const int VerNum = 6;    //顶点个数
        private static MGraph g;//节点对应“图”数据
        private static int startPoiID;//起点ID
        private static int costMode;//花费因子标记 0-距离 1-时间

        static void Ppath(int[] path, int i, int v)  //前向递归查找路径上的顶点
        {
            int k;
            k = path[i];
            if (k == v)
            {
                return; }   //找到了起点则返回
            Ppath(path, k, v);    //找顶点k的前一个顶点v

            Console.Write("{0},", k);    //输出路径上的终点
                                        
        }

        static void Dispath(int[] dist, int[] path, int[] s, int n, int v)//结果输出
        {
            int i;
            for (i = 0; i < n; i++)
            {
                if(i != v)
                {
                    if (s[i] == 1)
                    {
                        Console.Write(" 从{0}到{1}的最短路径长度为:{2}\t路径为:", v, i, dist[i]);
                        Console.Write("{0},", v);    //输出路径上的起点
                        Ppath(path, i, v);    //输出路径上的中间点
                        Console.WriteLine("{0}", i);    //输出路径上的终点
                    }
                    else
                        Console.WriteLine("从{0}到{1}不存在路径\n", v, i);

                }
            }
        }

        /// <summary>
        /// Dijkstra核心功能函数
        /// </summary>
        /// <param name="g"> 图</param>
        /// <param name="v">起点</param>
        /// 
        static void Dijkstra(MGraph g, int v)//核心功能函数
        {
            int[] dist = new int[VerNum];//从原点v到其他的各定点当前的最短路径长度（实时更新）
            int[] path = new int[VerNum];//path[i]表示从原点v到定点i之间最短路径的前驱节点，path[v] = v
            int[] s = new int[VerNum];   //选定的顶点的集合
            int mindis, i, j, u;
            u = 0;
            for (i = 0; i < g.n; i++)
            {
                dist[i] = g.edges[v, i];       //距离初始化
                s[i] = 0;                        //s[]置空  0表示i不在s集合中
                if (g.edges[v, i] < INF)        //路径初始化
                    path[i] = v;
                else
                    path[i] = -1;
            }
            s[v] = 1;                  //源点编号v放入s中
            path[v] = v;
            for (i = 0; i < g.n; i++)                //循环直到所有顶点的最短路径都求出
            {
                mindis = INF;                    //mindis置最小长度初值
                for (j = 0; j < g.n; j++)         //选取不在s中且具有最小距离的顶点u
                    if (s[j] == 0 && dist[j] < mindis)
                    {
                        u = j;
                        mindis = dist[j];
                    }
                s[u] = 1;                       //顶点u加入s中
                for (j = 0; j < g.n; j++)         //修改不在s中的顶点的距离
                    if (s[j] == 0)
                        if (g.edges[u, j] < INF && dist[u] + g.edges[u, j] < dist[j])
                        {
                            dist[j] = dist[u] + g.edges[u, j];
                            path[j] = u;
                        }
            }
            Dispath(dist, path, s, g.n, v);      //输出最短路径
                                                
        }

        static void initdata()
        {
           
            g.n = VerNum; g.e = 2 * VerNum;//特殊图：全连通
            g.edges = new int[VerNum, VerNum];
            g.vexs = new VertexType[VerNum];
            int[,] Distance_p = new int[VerNum, VerNum] {
            {0, 5,3,7,4,1},
            {5, 0, 2,2, 4,1},
            {3, 2, 0,2,7,3},
            {7,2,2,0,3,6},
            {4,4,7,3,0,6},
            {1,1,3,6,6,0},

            };//基于距离权重设定

            int[,] Time_p = new int[VerNum, VerNum] {
            {0, 1,3,7,4,1},
            {1, 0, 4,2, 4,3},
            {3, 4, 0,2,5,3},
            {7,2,2,0,3,6},
            {4,4,5,3,0,2},
            {1,3,3,6,2,0},

            };//基于时间权重设定
            if(costMode==0)
            {
                for (int i = 0; i < g.n; i++)        //建立图的图的邻接矩阵
                {
                    for (int j = 0; j < g.n; j++)
                    {
                        g.edges[i, j] = Distance_p[i, j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < g.n; i++)        //建立图的图的邻接矩阵
                {
                    for (int j = 0; j < g.n; j++)
                    {
                        g.edges[i, j] = Time_p[i, j];
                    }
                }
            }
        
          //打开poi的shp文件，获取节点X\Y\Z     
            IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
            IFeatureWorkspace pWS = (IFeatureWorkspace)pWSF.OpenFromFile(@"res\poi5_pm", 0);
            IFeatureClass pFeatureclass = pWS.OpenFeatureClass("poi5_pm.shp");
            IFeatureCursor pCursor = pFeatureclass.Search(null, false);
            IPolyline pPolyline = new PolylineClass();
            IPointCollection pPolycollect = pPolyline as IPointCollection;
            while (pCursor != null)
            {
                IFeature pFeature = pCursor.NextFeature();
                if (pFeature != null)
                {
                    IGeometry pGeometry = pFeature.Shape;
                    object objmiss = Type.Missing;
                    IPoint pPoint = new PointClass();
                    pPoint.X = Convert.ToDouble(pFeature.get_Value(2));
                    pPoint.Y = Convert.ToDouble(pFeature.get_Value(3));
                    pPoint.Z = Convert.ToDouble(pFeature.get_Value(4));
                    pPolycollect.AddPoint(pPoint, ref objmiss, ref objmiss);
                }
                else
                {
                    pCursor = null;
                }

            }
         
            initialVexInfo(g);//初始化节点信息



        }

        static void initialVexInfo(MGraph g)
        {

            //打开poi的shp文件，获取节点X\Y\Z     
            IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
            IFeatureWorkspace pWS = (IFeatureWorkspace)pWSF.OpenFromFile(@"res\poi5_pm", 0);
            IFeatureClass pFeatureclass = pWS.OpenFeatureClass("poi5_pm.shp");
            IFeatureCursor pCursor = pFeatureclass.Search(null, false);
            IPolyline pPolyline = new PolylineClass();
            IPointCollection pPolycollect = pPolyline as IPointCollection;
            int i = 0;//节点ID
            while (pCursor != null)
            {
                IFeature pFeature = pCursor.NextFeature();
                if (pFeature != null)
                {
                    IGeometry pGeometry = pFeature.Shape;
                    object objmiss = Type.Missing;
                    IPoint pPoint = new PointClass();
                    pPoint.X = Convert.ToDouble(pFeature.get_Value(2));
                    pPoint.Y = Convert.ToDouble(pFeature.get_Value(3));
                    pPoint.Z = Convert.ToDouble(pFeature.get_Value(4));
                    g.vexs[i].SpatialPoi = pPoint;
                    i++;
                }
                else
                {
                    pCursor = null;
                }

            }

            g.vexs[0].VexNo = "1";
            g.vexs[0].VexName = "七星鲁王宫";
                     
            g.vexs[1].VexNo = "2";
            g.vexs[1].VexName = "西沙海底墓";
           
            g.vexs[2].VexNo = "3";
            g.vexs[2].VexName = "秦岭神树";
            
            g.vexs[3].VexNo = "4";
            g.vexs[3].VexName = "云顶天宫";
            
            g.vexs[4].VexNo = "5";
            g.vexs[4].VexName = "西王母宫";
          
            g.vexs[5].VexNo = "6";
            g.vexs[5].VexName = "张家古楼";
           


        }

        private void startDijsktra()//Dijkstra主函数
        {

            initdata();
            Console.WriteLine("最小生成树构成:");
            startPoiID = 2;
            Dijkstra(g, startPoiID);

            //Console.ReadKey();
            Console.Read();
        }

        private void SelStartPoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            int StartPoiIndex = SelStartPoi.SelectedIndex;

            switch (StartPoiIndex)
            {
                case 1://七星鲁王宫
                    startPoiID = 0;

                    break;
                case 2://西沙海底墓

                    startPoiID = 1;
                    break;
                case 3://秦岭神树

                    startPoiID = 2;

                    break;

                case 4://云顶天宫

                    startPoiID = 3;
                    break;
                case 5://西王母宫

                    startPoiID = 4;
                    break;
                case 6://张家古楼

                    startPoiID = 5;
                    break;
                default:

                    break;

            }

            checkTime.Enabled = true;
            checkDistance.Enabled = true;


        }

        private void checkDistance_CheckedChanged(object sender, EventArgs e)
        {
            costMode = 0;
            checkTime.Checked = false;
            btn_Dijskta.Visible = true;

        }

        private void checkTime_CheckedChanged(object sender, EventArgs e)
        {
            costMode = 1;
            checkDistance.Checked = false;
            btn_Dijskta.Visible = true;
        }
    }
}
