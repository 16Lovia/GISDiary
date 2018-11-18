using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.IO;


using ESRI.ArcGIS.SystemUI;
using CCWin;

namespace GISDiary
{
    public partial class FormRoute : Skin_DevExpress
    {

        /// <summary>
        /// //此demo云顶天宫以为出发点！！！
        /// </summary>
        public FormRoute()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            //PathRouteTem = new int[VerNum, VerNum] ;
            //for(int i =0; i< VerNum; i++)
            //{
            //    for (int j = 0; j < VerNum; j++)
            //    {
            //        PathRouteTem[i, j] = -1;
            //    }
            //}
          
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            
            string path1 = @"res\RouteShp";
            string path2 = @"CHN_adm1.shp";
            loadShp(path1,path2);//底图加载          
            path2 = @"POI.shp";
            loadShp(path1, path2);//图标加载


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
        private static int[,] PathRouteTem;//存储路径数组
        public static int[] h;//中间节点个数
        public static IGeometry[] geometry;//路径几何信息
        public static int[] s;//记录节点间是否可达
        public static ILineSymbol[] pLineSymbol;//路径绘制符号
        public static ISegmentCollection[] m_polyline;//路径线集
        public static string shpPath;//搜索shp文件路径
        public static string startPointname;//起点名
        public static string stringvalue;//所点击路径图层编号
        public static string endPointname;//终点名
        public static string cost;//所选路径总花费



        /// <summary>
        /// creatPolylineShp()\ GetIFeatureClass()\SaveShpToFile() 线图层存为shp（未使用）
        /// </summary>
        public void creatPolylineShp()
        {

            //分别生成shp


            string FeatureType = "Polyline";
            //打开一个要素类
            IFeatureClass pFeatureClass = GetIFeatureClass(FeatureType);
            IFeature pFeature = pFeatureClass.CreateFeature();
            // pFeature.Shape = geometry[0] as IPolyline;
            //IPolyline aTempPolyline = new PolylineClass();
            //IGeometryCollection aTempGeometryCollection = aTempPolyline as IGeometryCollection;
            //aTempGeometryCollection.AddGeometry(geometry[0]);                  
            //pFeature.Shape = aTempGeometryCollection as IPolyline;
            pFeature.Shape =(IGeometry) m_polyline[0];
            pFeature.Store();

            string path = @"D:\code\test.shp";
            if (path != "")
            {
                string ExportFileShortName = System.IO.Path.GetFileNameWithoutExtension(path);
                string ExportFilePath = System.IO.Path.GetDirectoryName(path);
                SaveShpToFile(pFeatureClass, ExportFilePath, ExportFileShortName);
            }
            else
            {
                MessageBox.Show("无效目录！");
            }
        }

        public IFeatureClass GetIFeatureClass(string FeatureType)
        {
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspace pWorkSpace = pWorkspaceFactory.OpenFromFile(@"res\poi_shp_jw", 0);
            //初始化要素工作空间
            IFeatureWorkspace pFeatWorkSpace = pWorkSpace as IFeatureWorkspace;
            //IFeatureClass pFeatureClass = pFeatWorkSpace.OpenFeatureClass(FeatureType);
            IFeatureClass pFeatureClass = pFeatWorkSpace.OpenFeatureClass("poi.shp");
            
            return pFeatureClass;
        }

        public void SaveShpToFile(IFeatureClass pFeatureClass, string ExportFilePath, string ExportFileShortName)
        {
            //设置导出要素类的参数
            IFeatureClassName pOutFeatureClassName = new FeatureClassNameClass();
            IDataset pOutDataset = (IDataset)pFeatureClass;
            pOutFeatureClassName = (IFeatureClassName)pOutDataset.FullName;
            //创建一个输出shp文件的工作空间
            IWorkspaceFactory pShpWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            IWorkspaceName pInWorkspaceName = new WorkspaceNameClass();
            pInWorkspaceName = pShpWorkspaceFactory.Create(ExportFilePath, ExportFileShortName, null, 0);

            //创建一个要素类
            IFeatureClassName pInFeatureClassName = new FeatureClassNameClass();
            IDatasetName pInDatasetClassName;
            pInDatasetClassName = (IDatasetName)pInFeatureClassName;
            pInDatasetClassName.Name = ExportFileShortName;//作为输出参数
            pInDatasetClassName.WorkspaceName = pInWorkspaceName;
            IFeatureDataConverter pShpToClsConverter = new FeatureDataConverterClass();
            pShpToClsConverter.ConvertFeatureClass(pOutFeatureClassName, null, null, pInFeatureClassName, null, null, "", 1000, 0);
            MessageBox.Show("导出成功", "系统提示");
        }


        /// <summary>
        /// 线图层存为shp（未使用）方法2
        /// </summary>
        /// <param name="apFeatureClass"></param>
        public void ExportFeatureClassToShp(IFeatureClass apFeatureClass)
        {
            if (apFeatureClass == null)
            {
                MessageBox.Show("请选择", "系统提示");
                return;
            }
            //调用保存文件函数
            SaveFileDialog sa = new SaveFileDialog();
            sa.Filter = "SHP文件(.shp)|*.shp";
            sa.ShowDialog();
            sa.CreatePrompt = true;

            string ExportShapeFileName = sa.FileName;
            // string StrFilter = "SHP文件(.shp)|*.shp";
            // string ExportShapeFileName = SaveFileDialog(StrFilter);
            if (ExportShapeFileName == "")
                return;

            string ExportFileShortName = System.IO.Path.GetFileNameWithoutExtension(ExportShapeFileName);
            string ExportFilePath = System.IO.Path.GetDirectoryName(ExportShapeFileName);
            shpPath = ExportFilePath + "\\" + ExportFileShortName + "\\" + ExportFileShortName + ".shp";
            //设置导出要素类的参数

            IFeatureClassName pOutFeatureClassName = new FeatureClassNameClass();
            IDataset pOutDataset = (IDataset)apFeatureClass;
            pOutFeatureClassName = (IFeatureClassName)pOutDataset.FullName;

            //创建一个输出shp文件的工作空间
            IWorkspaceFactory pShpWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            IWorkspaceName pInWorkspaceName = new WorkspaceNameClass();
            pInWorkspaceName = pShpWorkspaceFactory.Create(ExportFilePath, ExportFileShortName, null, 0);

            //创建一个要素集合
            IFeatureDatasetName pInFeatureDatasetName = null;
            //创建一个要素类
            IFeatureClassName pInFeatureClassName = new FeatureClassNameClass();
            IDatasetName pInDatasetClassName;
            pInDatasetClassName = (IDatasetName)pInFeatureClassName;
            pInDatasetClassName.Name = ExportFileShortName;//作为输出参数
            pInDatasetClassName.WorkspaceName = pInWorkspaceName;

            //通过FIELDCHECKER检查字段的合法性，为输出SHP获得字段集合
            long iCounter;
            IFields pOutFields, pInFields;
            IFieldChecker pFieldChecker;
            IField pGeoField;
            IEnumFieldError pEnumFieldError = null;
            pInFields = apFeatureClass.Fields;
            pFieldChecker = new FieldChecker();
            pFieldChecker.Validate(pInFields, out pEnumFieldError, out pOutFields);

            //通过循环查找几何字段
            pGeoField = null;
            for (iCounter = 0; iCounter < pOutFields.FieldCount; iCounter++)
            {
                if (pOutFields.get_Field((int)iCounter).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    pGeoField = pOutFields.get_Field((int)iCounter);
                    break;
                }
            }

            //得到几何字段的几何定义
            IGeometryDef pOutGeometryDef;
            IGeometryDefEdit pOutGeometryDefEdit;
            pOutGeometryDef = pGeoField.GeometryDef;

            //设置几何字段的空间参考和网格
            pOutGeometryDefEdit = (IGeometryDefEdit)pOutGeometryDef;
            pOutGeometryDefEdit.GridCount_2 = 1;
            pOutGeometryDefEdit.set_GridSize(0, 1500000);

            try

            {
                //开始导入
                IFeatureDataConverter pShpToClsConverter = new FeatureDataConverterClass();
                pShpToClsConverter.ConvertFeatureClass(pOutFeatureClassName, null, pInFeatureDatasetName, pInFeatureClassName, pOutGeometryDef, pOutFields, "", 1000, 0);
                //MessageBox.Show("导出成功", "系统提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("the following exception occurred:" + ex.ToString());
            }
        }

        //线图层存为shp（未使用）方法3

        static void PointToLineShp()
        {

            //打开poi的shp文件，设置属性（order)  
            IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
            IFeatureWorkspace pWS = (IFeatureWorkspace)pWSF.OpenFromFile(@"res\poi_shp_jw", 0);
            IFeatureClass pFeatureclass = pWS.OpenFeatureClass("poi.shp");
            IFeatureCursor pCursor = pFeatureclass.Search(null, false);
            IPolyline pPolyline = new PolylineClass();
            IPointCollection pPolycollect = pPolyline as IPointCollection;



            IDataset dataset = (IDataset)pFeatureclass;//属性表可编辑状态
            IWorkspace workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();

            for (int d = 0; d < VerNum; d++)
            {
                if (d != startPoiID)
                {

                    //逐条设置order，生成shp
                    int ss = 0;//节点id
                    int e = 0;
                    while (pCursor != null)
                    {
                        IFeature pFeature = pCursor.NextFeature();
                        IFields pFields = pFeature.Fields;
                        // IField pField = pFeature.Fields;
                        IClass pClass = pFeatureclass as IClass;

                        if (pFeature != null)
                        {

                            for (e = 0; e < h[ss] + 2; e++)
                            {
                                if (PathRouteTem[d, e] == ss) return;

                            }
                            IGeometry pGeometry = pFeature.Shape;
                            object objmiss = Type.Missing;
                            pFeature.set_Value(10, PathRouteTem[d, e]);
                            ss++;

                        }
                        else
                        {
                            pCursor = null;
                        }

                    }




                }

            }



        }

        /// <summary>
        /// 加载shp文件（多次调用）
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        public void loadShp(string path1,string path2)
        {

            //打开poi的shp文件，获取节点X\Y\Z     
            IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
            IFeatureWorkspace pWS = (IFeatureWorkspace)pWSF.OpenFromFile(path1, 0);//依据经纬度数据画线
            IFeatureClass pFeatureclass = pWS.OpenFeatureClass(path2);                  
            IFeatureLayer pFLayer = new FeatureLayerClass(); 
            pFLayer.FeatureClass = pFeatureclass;
            pFLayer.Name = pFeatureclass.AliasName; 
            ILayer pLayer = pFLayer as ILayer;
            IMap pMap = axMapControl1.Map;
            pMap.AddLayer(pLayer); 
            axMapControl1.ActiveView.Refresh();


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
            s = new int[VerNum];   //选定的顶点的集合
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

        //PathRouteTem bug
        static void Ppath(int[] path, int i, int v)  //前向递归查找路径上的顶点
        {

            int k;
            k = path[i];
            if (k == v)
            {
                return;
            }   //找到了起点则返回
            Ppath(path, k, v);    //找顶点k的前一个顶点

            Console.Write("{0},", k);    //输出路径上的终点
            ++h[i];//迭代次数
            PathRouteTem[i, h[i]] = k;
            int yyy = PathRouteTem[i, h[i]];




        }

        static void Dispath(int[] dist, int[] path, int[] s, int n, int v)//结果输出
        {

            PathRouteTem = new int[VerNum, VerNum];
            h = new int[VerNum];
            int i;
            for (i = 0; i < n; i++)
            {
                if (i != v)
                {
                    if (s[i] == 1)
                    {
                        Console.Write(" 从{0}到{1}的最短路径长度为:{2}\t路径为:", v, i, dist[i]);
                        Console.Write("{0},", v);    //输出路径上的起点
                        PathRouteTem[i, 0] = v;
                        h[i] = 0;
                        Ppath(path, i, v);    //输出路径上的中间点
                        Console.WriteLine("{0}", i);    //输出路径上的终点
                        PathRouteTem[i, h[i] + 1] = i;
                        int fff = h[i] + 2;
                        for (int rrr = 0; rrr < fff; rrr++)
                        {
                            int ddd = PathRouteTem[i, rrr];

                        }

                    }
                    else
                        Console.WriteLine("从{0}到{1}不存在路径\n", v, i);


                }
            }
            //PathRoute = PathRouteTem;


        }

        

        static void drawPath(int[] s, int n, int v)
        {


            geometry = new IGeometry[VerNum];
            pLineSymbol = new ILineSymbol[VerNum];
            m_polyline = new ISegmentCollection[VerNum];
            int i;
            //ISegmentCollection m_polyline = null;
            for (i = 0; i < n; i++)
            {
                if (i != v)
                {


                    if (s[i] == 1)
                    {
                        m_polyline[i] = null;

                        //路径绘制


                        int pointNum = h[i] + 2;
                        IPoint pt = null;
                        IPointCollection ptCol = new MultipointClass();
                        for (int pP = 0; pP < pointNum; pP++)
                        {
                            //读取路径上每个点
                            pt = g.vexs[PathRouteTem[i, pP]].SpatialPoi;

                            if (pt == null) return;
                            ptCol.AddPoint(pt);
                        }

                        int tt = ptCol.PointCount;



                        IRgbColor pRgbColor = new RgbColorClass();
                        pRgbColor.Blue = 255 - i * 51;
                        pRgbColor.Green = 0 + i * 51;
                        pRgbColor.Red = 0 + i * 51;//不同路径颜色设置

                        //ISimpleLine3DSymbol pSimpleLine3DSymbol = new SimpleLine3DSymbolClass();
                        //pSimpleLine3DSymbol.Style = esriSimple3DLineStyle.esriS3DLSTube;
                        //pLineSymbol = pSimpleLine3DSymbol as ILineSymbol;  
                        pLineSymbol[i] = new SimpleLineSymbolClass();
                        pLineSymbol[i].Color = pRgbColor;
                        pLineSymbol[i].Width = 3;

                        //产生线段对象 line  
                        //ISegmentCollection m_polyline = null;
                        ILine pLine = new LineClass();
                        IPoint fromPt = null;//线起点
                        IPoint toPt = null;//线终点
                        for (int pP = 0; pP < pointNum - 1; pP++)
                        {
                            fromPt = ptCol.get_Point(pP);
                            toPt = ptCol.get_Point(pP + 1);
                            pLine.PutCoords(fromPt, toPt);
                            object Missing1 = Type.Missing;
                            object Missing2 = Type.Missing;
                            ISegment pSegment = pLine as ISegment;
                            if (m_polyline[i] == null)
                                m_polyline[i] = new PolylineClass();
                            m_polyline[i].AddSegment(pSegment, ref Missing1, ref Missing2); //将线段对象添加到多义线对象polyline  
                        }


                        int tttt = m_polyline[i].SegmentCount;
                        geometry[i] = m_polyline[i] as PolylineClass;
                        esriGeometryType aaa = geometry[i].GeometryType;


                    }



                }
            }
            //geometry[0] = (IGeometry)m_polyline;

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
        
          ////打开poi的shp文件，获取节点X\Y\Z     
          //  IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
          //  IFeatureWorkspace pWS = (IFeatureWorkspace)pWSF.OpenFromFile(@"res\poi5_pm", 0);
          //  IFeatureClass pFeatureclass = pWS.OpenFeatureClass("poi5_pm.shp");
          //  IFeatureCursor pCursor = pFeatureclass.Search(null, false);
          //  IPolyline pPolyline = new PolylineClass();
          //  IPointCollection pPolycollect = pPolyline as IPointCollection;
          //  while (pCursor != null)
          //  {
          //      IFeature pFeature = pCursor.NextFeature();
          //      if (pFeature != null)
          //      {
          //          IGeometry pGeometry = pFeature.Shape;
          //          object objmiss = Type.Missing;
          //          IPoint pPoint = new PointClass();
          //          pPoint.X = Convert.ToDouble(pFeature.get_Value(2));
          //          pPoint.Y = Convert.ToDouble(pFeature.get_Value(3));
          //          pPoint.Z = Convert.ToDouble(pFeature.get_Value(4));
          //          pPolycollect.AddPoint(pPoint, ref objmiss, ref objmiss);
          //      }
          //      else
          //      {
          //          pCursor = null;
          //      }

          //  }
         
            initialVexInfo(g);//初始化节点信息



        }

        static void initialVexInfo(MGraph g)
        {

            //打开poi的shp文件，获取节点X\Y\Z     
            IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
            IFeatureWorkspace pWS = (IFeatureWorkspace)pWSF.OpenFromFile(@"res\ppp_jw", 0);//依据经纬度数据画线
            IFeatureClass pFeatureclass = pWS.OpenFeatureClass("poi.shp");
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
                    pPoint.Z = Convert.ToDouble(pFeature.get_Value(9));
                    g.vexs[i].SpatialPoi = pPoint;
                    i++;
                }
                else
                {
                    pCursor = null;
                }

            }

            g.vexs[0].VexNo = "0";
            g.vexs[0].VexName = "七星鲁王宫";
                     
            g.vexs[1].VexNo = "1";
            g.vexs[1].VexName = "西沙海底墓";
           
            g.vexs[2].VexNo = "2";
            g.vexs[2].VexName = "秦岭神树";
            
            g.vexs[3].VexNo = "3";
            g.vexs[3].VexName = "云顶天宫";
            
            g.vexs[4].VexNo = "4";
            g.vexs[4].VexName = "西王母宫";
          
            g.vexs[5].VexNo = "5";
            g.vexs[5].VexName = "张家古楼";
           


        }

        private void startDijsktra()//Dijkstra算法入口
        {

            initdata();
            Console.WriteLine("最小生成树构成:");
            Dijkstra(g, startPoiID);

            //Console.ReadKey();
            Console.Read();
        }

        private void SelStartPoi_SelectedIndexChanged(object sender, EventArgs e)
        {

            
            int StartPoiIndex = SelStartPoi.SelectedIndex;
            switch (StartPoiIndex)
            {
                case 0://七星鲁王宫
                    startPoiID = 0;
                    startPointname = "七星鲁王宫";
                    
                    break;
                case 1://西沙海底墓

                    startPoiID = 1;
                    startPointname = "西沙海底墓";
                    break;
                case 2://秦岭神树

                    startPoiID = 2;
                    startPointname = "秦岭神树";
                    break;

                case 3://云顶天宫

                    startPoiID = 3;
                    startPointname = "云顶天宫";
                    break;
                case 4://西王母宫

                    startPoiID = 4;
                    startPointname = "西王母宫";
                    break;
                case 5://张家古楼

                    startPoiID = 5;
                    startPointname = "张家古楼";
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
            //btn_Dijskta.Enabled = true;
            lab_cost.Text = "路程";



        }

        private void checkTime_CheckedChanged(object sender, EventArgs e)
        {
            costMode = 1;
            checkDistance.Checked = false;
            //btn_Dijskta.Enabled = true;
            lab_cost.Text = "时间";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            startDijsktra();
            drawPath(s, g.n, startPoiID);
            string path_1 = @"res\RouteShp";
            string path_2 = @"LineAll.shp";
            loadShp(path_1,path_2);
            panel2.Visible = true;

            ////更新到Graphics窗口  
            //IGraphicsContainer pGCon = axMapControl1.ActiveView.GraphicsContainer;
            //IElement pElement = new LineElementClass();
            //ILineElement pLineElement = pElement as ILineElement;
            //pGCon.DeleteAllElements();

            //int tttttt = geometry.Count();
            //for (int i = 0; i < VerNum; i++)
            //{
            //    if (i != startPoiID)
            //    {

            //        pElement.Geometry = geometry[i];                    
            //        pLineElement.Symbol = pLineSymbol[i];
            //        pGCon.AddElement(pElement, 0);
            //        //axMapControl1.ActiveView.Extent = pElement.Geometry.Envelope;

            //        axMapControl1.Update();
            //        axMapControl1.Refresh();
            //        Thread.Sleep(500);

            //        //axMapControl1

            //    }

            //}
            //axMapControl1.Update();
            //axMapControl1.Refresh();
            //Thread.Sleep(500);

            ////pElement.Geometry = geometry[0];
            ////ILineElement pLineElement = pElement as ILineElement;
            ////pLineElement.Symbol = pLineSymbol[0];

            ////pGCon.AddElement(pElement, 0);
            //////axMapControl1.ActiveView.Extent = pElement.Geometry.Envelope;
            ////axMapControl1.Refresh();




        }


        //鼠标点击-空间查询
        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {

            //1、高亮

            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;

            IActiveView pActiveView;
            IPoint pPoint;
            double length;
            //获取视图范围
            pActiveView = this.axMapControl1.ActiveView;
            //获取鼠标点击屏幕坐标
            pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            //屏幕距离转换为地图距离
            length = ConvertPixelToMapUnits(pActiveView, 2);

            ITopologicalOperator pTopoOperator;
            IGeometry pGeoBuffer;
            //根据缓冲半径生成空间过滤器
            pTopoOperator = pPoint as ITopologicalOperator;
            pGeoBuffer = pTopoOperator.Buffer(length);

            ILayer iLayer;
            //iLayer = Tool.GetLayerByName(axMapControl1.get_Layer(0).Name, axMapControl1);
            iLayer = axMapControl1.get_Layer(2);//地图图层编号!!!
            IFeatureLayer iFeatureLayer = (IFeatureLayer)iLayer;
            iFeatureLayer.Selectable = false;//地图不可选择

            IMap pMap = axMapControl1.Map;
            //IGeometry pGeometry = axMapControl1.TrackRectangle();   //获取几何图框范围
            ISelectionEnvironment pSelectionEnv = new SelectionEnvironment(); //新建选择环境
                                                                              /*选择图层*/
            IRgbColor pColor = new RgbColor();
            pColor.Red = 200; pColor.Green = 155; pColor.Blue = 180; //调整高亮显示的颜色
            pSelectionEnv.DefaultColor = pColor;     //设置高亮显示的颜色
            pMap.SelectByShape(pGeoBuffer, pSelectionEnv, false);  //选择图形SelectByShape方法
                                                                   //高亮后获得ID
                                                                   /*获取ID*/
            ISelection selection = pMap.FeatureSelection;
            /*获取ID*/
            IEnumFeatureSetup enumFeatureSetup = selection as IEnumFeatureSetup; //这里很必要
            /*获取ID*/
            enumFeatureSetup.AllFields = true; //这里很必要
            IEnumFeature enumFeature = enumFeatureSetup as IEnumFeature;
            enumFeature.Reset();
            //feature赋值
            IFeature feature = enumFeature.Next();

            //IFeatureClass pFeatureClass = pFeature.Class as IFeatureClass;

            while (feature != null)
            {
                stringvalue = feature.get_Value(0).ToString();//就可以得到任意字段的值了 
                feature = enumFeature.Next();
            }
            axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
           // Thread.Sleep(2000);



            //2、省份查询


            this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;

            //记录查询到的要素名称
            string strNames = "";
            //查询的字段名称
            string strFieldName = "NL_NAME_1";
            //点查询
            //读取shp线
            string strFielPath = @"res\RouteShp\" + stringvalue + ".shp";//D:\\NewShp\\line1.shp 
            FileInfo fileInfo = new FileInfo(strFielPath);
            string fileDirectoryName = fileInfo.DirectoryName;
            string fileName = fileInfo.Name;
            //IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactoryClass();
            IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspace workspace = workspaceFactory.OpenFromFile(fileDirectoryName, 0);
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);
            IFeatureCursor featureCursor = featureClass.Search(null, false);//访问要素类的一系列要素 对要素类进行查询返回的一个游标（即指向搜素结果集的一个指针）
            IFeature feature_q = featureCursor.NextFeature();//将游标移动到结果集下一个要素并返回当前要素，这里将返回结果赋值给了pFeature 
            while (feature_q != null)
            {
                IGeometry geoMetry = feature_q.Shape;//得到的每个数据
                feature_q = featureCursor.NextFeature();
                //提示框
                strNames = strNames + QuerySpatial(this.axMapControl1, geoMetry, strFieldName) ;
                //提示框显示提示
                this.lab_info.Text = strNames;
            }

            //3、获取所选路径终点名、花费
            switch (stringvalue)
            {
                case "0"://七星鲁王宫
                    endPointname = "七星鲁王宫";
                    cost ="5542公里" ;
                    break;
                case "1"://西沙海底墓

                    endPointname = "西沙海底墓";
                    cost ="3284公里" ;
                    break;
                case "2"://秦岭神树

                    endPointname = "秦岭神树";
                    cost ="1867公里" ;
                    break;

                case "3"://西王母宫

                    endPointname = "西王母宫";
                    cost = "3433公里";
                    break;
                case "4"://张家鼓楼

                    endPointname = "张家鼓楼";
                    cost = "7528公里";
                    break;
                
                default:

                    break;

            }

           
            lab_start.Text = startPointname;
            lab_end.Text = endPointname;
            lab_costAll.Text = cost ;
        }

        private void skinComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            int StartPoiIndex = SelStartPoi.SelectedIndex;
            switch (StartPoiIndex)
            {
                case 0://七星鲁王宫
                    startPoiID = 0;
                    startPointname = "七星鲁王宫";

                    break;
                case 1://西沙海底墓

                    startPoiID = 1;
                    startPointname = "西沙海底墓";
                    break;
                case 2://秦岭神树

                    startPoiID = 2;
                    startPointname = "秦岭神树";
                    break;

                case 3://云顶天宫

                    startPoiID = 3;
                    startPointname = "云顶天宫";
                    break;
                case 4://西王母宫

                    startPoiID = 4;
                    startPointname = "西王母宫";
                    break;
                case 5://张家古楼

                    startPoiID = 5;
                    startPointname = "张家古楼";
                    break;
                default:

                    break;

            }

            checkTime.Enabled = true;
            checkDistance.Enabled = true;

        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            // 设置显示样式
            skinToolTip1.AutoPopDelay = 5000;//提示信息的可见时间
            skinToolTip1.InitialDelay = 500;//事件触发多久后出现提示
            skinToolTip1.ReshowDelay = 500;//指针从一个控件移向另一个控件时，经过多久才会显示下一个提示框
            skinToolTip1.ShowAlways = true;//是否显示提示框
            skinToolTip1.SetToolTip(label3, "点击地图上的线进行查询");//设置提示按钮和提示内容
        }
    }
}
