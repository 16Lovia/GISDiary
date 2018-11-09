using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 迪杰特斯拉最短路径
/// </summary>
namespace GISDiary
{
    class DjRoute
    {
        private const int MaxSize = 6;
        private const int INF = 32767;    //INF表示∞
        private const int MAXV = 4;    //最大顶点个数
        //结构体的成员定义里不能直接赋值，也就是等号后的应该移除，在你后面实例化整个结构体以后，
        //再对Study_Data[n].input=new double[50] 其他成员类似。顺便说下其实用class简单得多。

        struct VertexType
        {
            public string VexNo;        //顶点编号
            public string VexName;        //顶点名称
            public string otherInfo;     //顶点其他信息     
        };                               //顶点类型
        struct MGraph                    //图的定义
        {
            public int[,] edges;       //邻接矩阵
            public int n, e;             //顶点数,弧数
            public VertexType[] vexs; //存放顶点信息
        };                               //图的邻接矩阵类型

        static void Ppath(int[] path, int i, int v)  //前向递归查找路径上的顶点
        {
            int k;
            k = path[i];
            if (k == v) return;    //找到了起点则返回
            Ppath(path, k, v);    //找顶点k的前一个顶点v

            Console.Write("{0},", k);    //输出路径上的终点
                                         // Ppath(path, k, j);    //找顶点k的前一个顶点j
        }

        static void Ppath(MGraph g, int[] path, int i, int v)  //前向递归查找路径上的顶点
        {
            int k;
            k = path[i];
            if (k == v) return;    //找到了起点则返回
            Ppath(path, k, v);    //找顶点k的前一个顶点v

            Console.Write("{0},", g.vexs[k].VexName);    //输出路径上的终点
            // Ppath(path, k, j);    //找顶点k的前一个顶点j
        }

        static void Dispath(int[] dist, int[] path, int[] s, int n, int v)
        {
            int i;
            for (i = 0; i < n; i++)
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

        static void PutBothpath(MGraph g, int[] dist, int[] path, int[] s, int n, int v, int m)
        {
            int i;
            for (i = 0; i < n; i++)
            {
                if (s[i] == 1)
                {
                    //Console.Write(" 从{0}到{1}的最短路径长度为:{2}\t路径为:", v, i, dist[i]);
                    //Console.Write("{0},", v);    //输出路径上的起点
                    //Ppath(path, i, v);    //输出路径上的中间点
                    //Console.WriteLine("{0}", i);    //输出路径上的终点
                    if (i == m && dist[i] < INF)
                    {
                        Console.Write(" 从{0}到{1}的最短路径长度为:{2}\t路径为:", g.vexs[v].VexName, g.vexs[i].VexName, dist[i]);
                        Console.Write("{0},", g.vexs[v].VexName);    //输出路径上的起点
                        //Ppath(path, i, v);    //输出路径上的中间点
                        Ppath(g, path, i, v);
                        Console.WriteLine("{0}", g.vexs[i].VexName);    //输出路径上的终点
                    }
                }
                else
                    Console.WriteLine("从{0}到{1}不存在路径\n", v, i);
            }
        }

        static void Dijkstra(MGraph g, int v)
        {
            int[] dist = new int[MAXV];//从原点v到其他的各定点当前的最短路径长度
            int[] path = new int[MAXV];//path[i]表示从原点到定点i之间最短路径的前驱节点
            int[] s = new int[MAXV];   //选定的顶点的集合
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
            path[v] = 0;
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
                                                 //PutBothpath(g, dist, path, s, g.n, v, 3);
        }

        static void initdata()
        {
            int i, j;
            MGraph g;
            g.n = 4; g.e = 8;
            g.edges = new int[MAXV, MAXV];
            g.vexs = new VertexType[MAXV];
            //int [,] anArray = new int [2, 4] {{1,2,3,4},{5,6,7,8}};
            int[,] a = new int[MAXV, MAXV] {
            {0,  5,INF,7},
            {INF,0,  4,2},
            {3,  3,  0,2},
            {INF,INF,1,0}
            };

            for (i = 0; i < g.n; i++)        //建立图的图的邻接矩阵
            {
                for (j = 0; j < g.n; j++)
                {
                    g.edges[i, j] = a[i, j];///////////////////////////////////////////////
                }
            }
            Console.WriteLine("各顶点的最短路径:");
        }

        static void initialVexInfo(MGraph g)
        {
            g.vexs[0].VexNo = "0361";
            g.vexs[0].VexName = "西安";

            g.vexs[1].VexNo = "010";
            g.vexs[1].VexName = "北京";

            g.vexs[2].VexNo = "0681";
            g.vexs[2].VexName = "武汉";

            g.vexs[3].VexNo = "0571";
            g.vexs[3].VexName = "杭州";
        }
        static void Main(string[] args)
        {
            int i, j;
            MGraph g;
            g.n = 4; g.e = 8;
            g.edges = new int[MAXV, MAXV];
            g.vexs = new VertexType[MAXV];

            initialVexInfo(g);
            //int [,] anArray = new int [2, 4] {{1,2,3,4},{5,6,7,8}};
            int[,] a = new int[MAXV, MAXV] {
            {0, 5,INF,7},
            {5, 0,  4,1},
            {INF, 4, 0,INF},
            {7,1,INF,0}
            };//权重设定

            for (i = 0; i < g.n; i++)        //建立图的图的邻接矩阵
            {
                for (j = 0; j < g.n; j++)
                {
                    g.edges[i, j] = a[i, j];
                }
            }
            Console.WriteLine("最小生成树构成:");
            Dijkstra(g, 0);

            //Console.ReadKey();
            Console.Read();
        }
    }

}

