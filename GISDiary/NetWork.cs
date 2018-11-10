using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.NetworkAnalysis;


namespace GISDiary
{
    /// <summary>
    /// 最短路径分析
    /// </summary>
    class NetWork
    {
        private IGeometricNetwork m_ipGeometricNetwork;
        public IMap m_ipMap;
        public IPointCollection m_ipPoints;
        private IPointToEID m_ipPointToEID;
        private double m_dblPathCost = 0;
        private IEnumNetEID m_ipEnumNetEID_Junctions;
        private IEnumNetEID m_ipEnumNetEID_Edges;
        public IPolyline m_ipPolyline;

        #region Public Function
        //返回和设置当前地图
        public IMap SetOrGetMap
        {
            set { m_ipMap = value; }
            get { return m_ipMap; }
        }
        //打开几何数据集的网络工作空间
        public void OpenFeatureDatasetNetwork(IFeatureDataset FeatureDataset)
        {
            CloseWorkspace();
            if (!InitializeNetworkAndMap(FeatureDataset))
                Console.WriteLine("打开network出错");
        }
        //输入点的集合
        public IPointCollection StopPoints
        {
            set { m_ipPoints = value; }
            get { return m_ipPoints; }
        }

        //路径成本
        public double PathCost
        {
            get { return m_dblPathCost; }
        }

        //返回路径的几何体
        public IPolyline PathPolyLine()
        {
            IEIDInfo ipEIDInfo;
            IGeometry ipGeometry;
            if (m_ipPolyline != null) return m_ipPolyline;

            m_ipPolyline = new PolylineClass();
            IGeometryCollection ipNewGeometryColl = m_ipPolyline as IGeometryCollection;

            ISpatialReference ipSpatialReference = m_ipMap.SpatialReference;
            IEIDHelper ipEIDHelper = new EIDHelperClass();
            ipEIDHelper.GeometricNetwork = m_ipGeometricNetwork;
            ipEIDHelper.OutputSpatialReference = ipSpatialReference;
            ipEIDHelper.ReturnGeometries = true;

            //IEnumEIDInfo ipEnumEIDInfo = ipEIDHelper.CreateEnumEIDInfo(m_ipEnumNetEID_Edges);
            //int count = ipEnumEIDInfo.Count;
            //ipEnumEIDInfo.Reset();
            //for (int i = 0; i < count; i++)
            //{
            //    ipEIDInfo = ipEnumEIDInfo.Next();
            //    ipGeometry = ipEIDInfo.Geometry;
            //    ipNewGeometryColl.AddGeometryCollection(ipGeometry as IGeometryCollection);
            //}
            return m_ipPolyline;
        }

        //解决路径
        public void SolvePath(string WeightName)
        {

            int intEdgeUserClassID;
            int intEdgeUserID;
            int intEdgeUserSubID;
            int intEdgeID;
            IPoint ipFoundEdgePoint;
            double dblEdgePercent;
            /*PutEdgeOrigins方法的第二个参数要求是IEdgeFlag类型的数组,
            * 在VB等其他语言的代码中，只需传人该类型数组的第一个元素即
            * 可，但C#中的机制有所不同，需要作出如下修改：使用
            * ITraceFlowSolverGEN替代ITraceFlowSolver
            */
            ITraceFlowSolverGEN ipTraceFlowSolver = new TraceFlowSolverClass() as ITraceFlowSolverGEN;
            INetSolver ipNetSolver = ipTraceFlowSolver as INetSolver;
            INetwork ipNetwork = m_ipGeometricNetwork.Network;
            ipNetSolver.SourceNetwork = ipNetwork;
            INetElements ipNetElements = ipNetwork as INetElements;
            int intCount = m_ipPoints.PointCount;
            //定义一个边线旗数组
            IEdgeFlag[] pEdgeFlagList = new EdgeFlagClass[intCount];
            for (int i = 0; i < intCount; i++)
            {

                INetFlag ipNetFlag = new EdgeFlagClass() as INetFlag;
                IPoint ipEdgePoint = m_ipPoints.get_Point(i);
                //查找输入点的最近的边线
                m_ipPointToEID.GetNearestEdge(ipEdgePoint, out intEdgeID, out ipFoundEdgePoint, out dblEdgePercent);
                ipNetElements.QueryIDs(intEdgeID, esriElementType.esriETEdge, out intEdgeUserClassID, out intEdgeUserID, out intEdgeUserSubID);
                ipNetFlag.UserClassID = intEdgeUserClassID;
                ipNetFlag.UserID = intEdgeUserID;
                ipNetFlag.UserSubID = intEdgeUserSubID;
                IEdgeFlag pTemp = (IEdgeFlag)(ipNetFlag as IEdgeFlag);
                pEdgeFlagList[i] = pTemp;
            }
            ipTraceFlowSolver.PutEdgeOrigins(ref pEdgeFlagList);
            INetSchema ipNetSchema = ipNetwork as INetSchema;
            INetWeight ipNetWeight = ipNetSchema.get_WeightByName(WeightName);

            INetSolverWeights ipNetSolverWeights = ipTraceFlowSolver as INetSolverWeights;
            ipNetSolverWeights.FromToEdgeWeight = ipNetWeight;//开始边线的权重
            ipNetSolverWeights.ToFromEdgeWeight = ipNetWeight;//终止边线的权重
            object[] vaRes = new object[intCount - 1];
            //通过findpath得到边线和交汇点的集合
            ipTraceFlowSolver.FindPath(esriFlowMethod.esriFMConnected,
            esriShortestPathObjFn.esriSPObjFnMinSum,
            out m_ipEnumNetEID_Junctions, out m_ipEnumNetEID_Edges, intCount - 1, ref vaRes);
            //计算元素成本
            m_dblPathCost = 0;
            for (int i = 0; i < vaRes.Length; i++)
            {

                double m_Va = (double)vaRes[i];
                m_dblPathCost = m_dblPathCost + m_Va;
            }
            m_ipPolyline = null;
        //    try
        //    {
        //        int intEdgeUserClassID;
        //        int intEdgeUserID;
        //        int intEdgeUserSubID;
        //        int intEdgeID;
        //        IPoint ipFoundEdgePoint;
        //        double dblEdgePercent;
        //        /*PutEdgeOrigins方法的第二个参数要求是IEdgeFlag类型的数组,
        //        * 在VB等其他语言的代码中，只需传人该类型数组的第一个元素即
        //        * 可，但C#中的机制有所不同，需要作出如下修改：使用
        //        * ITraceFlowSolverGEN替代ITraceFlowSolver
        //        */
        //        ITraceFlowSolverGEN ipTraceFlowSolver = new TraceFlowSolverClass() as ITraceFlowSolverGEN;
        //        INetSolver ipNetSolver = ipTraceFlowSolver as INetSolver;
        //        INetwork ipNetwork = m_ipGeometricNetwork.Network;
        //        ipNetSolver.SourceNetwork = ipNetwork;
        //        INetElements ipNetElements = ipNetwork as INetElements;
        //        int intCount = m_ipPoints.PointCount;
        //        //定义一个边线旗数组
        //        IEdgeFlag[] pEdgeFlagList = new EdgeFlagClass[intCount];
        //        for (int i = 0; i < intCount; i++)
        //        {

        //            INetFlag ipNetFlag = new EdgeFlagClass() as INetFlag;
        //            IPoint ipEdgePoint = m_ipPoints.get_Point(i);
        //            //查找输入点的最近的边线
        //            m_ipPointToEID.GetNearestEdge(ipEdgePoint, out intEdgeID, out ipFoundEdgePoint, out dblEdgePercent);
        //            ipNetElements.QueryIDs(intEdgeID, esriElementType.esriETEdge, out intEdgeUserClassID, out intEdgeUserID, out intEdgeUserSubID);
        //            ipNetFlag.UserClassID = intEdgeUserClassID;
        //            ipNetFlag.UserID = intEdgeUserID;
        //            ipNetFlag.UserSubID = intEdgeUserSubID;
        //            IEdgeFlag pTemp = (IEdgeFlag)(ipNetFlag as IEdgeFlag);
        //            pEdgeFlagList[i] = pTemp;
        //        }
        //        ipTraceFlowSolver.PutEdgeOrigins(ref pEdgeFlagList);
        //        INetSchema ipNetSchema = ipNetwork as INetSchema;
        //        INetWeight ipNetWeight = ipNetSchema.get_WeightByName(WeightName);

        //        INetSolverWeights ipNetSolverWeights = ipTraceFlowSolver as INetSolverWeights;
        //        ipNetSolverWeights.FromToEdgeWeight = ipNetWeight;//开始边线的权重
        //        ipNetSolverWeights.ToFromEdgeWeight = ipNetWeight;//终止边线的权重
        //        object[] vaRes = new object[intCount - 1];
        //        //通过findpath得到边线和交汇点的集合
        //        ipTraceFlowSolver.FindPath(esriFlowMethod.esriFMConnected,
        //        esriShortestPathObjFn.esriSPObjFnMinSum,
        //        out m_ipEnumNetEID_Junctions, out m_ipEnumNetEID_Edges, intCount - 1, ref vaRes);
        //        //计算元素成本
        //        m_dblPathCost = 0;
        //        for (int i = 0; i < vaRes.Length; i++)
        //        {
                   
        //            double m_Va =(double)vaRes[i];
        //            m_dblPathCost = m_dblPathCost + m_Va;
        //        }
        //        m_ipPolyline = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        }
        #endregion

        #region Private Function
        //初始化几何网络和地图
        private bool InitializeNetworkAndMap(IFeatureDataset FeatureDataset)
        {
            IFeatureClassContainer ipFeatureClassContainer;
            IFeatureClass ipFeatureClass;
            IGeoDataset ipGeoDataset;
            ILayer ipLayer;
            IFeatureLayer ipFeatureLayer;
            IEnvelope ipEnvelope, ipMaxEnvelope;
            double dblSearchTol;

            INetworkCollection ipNetworkCollection = FeatureDataset as INetworkCollection;
            int count = ipNetworkCollection.GeometricNetworkCount;
            //获取第一个几何网络工作空间
            m_ipGeometricNetwork = ipNetworkCollection.get_GeometricNetwork(0);
            INetwork ipNetwork = m_ipGeometricNetwork.Network;

            if (m_ipMap != null)
            {
                m_ipMap = new MapClass();
                ipFeatureClassContainer = m_ipGeometricNetwork as IFeatureClassContainer;
                count = ipFeatureClassContainer.ClassCount;
                for (int i = 0; i < count; i++)
                {
                    ipFeatureClass = ipFeatureClassContainer.get_Class(i);
                    ipFeatureLayer = new FeatureLayerClass();
                    ipFeatureLayer.FeatureClass = ipFeatureClass;
                    m_ipMap.AddLayer(ipFeatureLayer);
                }
            }
            count = m_ipMap.LayerCount;
            ipMaxEnvelope = new EnvelopeClass();
            for (int i = 0; i < count; i++)
            {
                ipLayer = m_ipMap.get_Layer(i);
                ipFeatureLayer = ipLayer as IFeatureLayer;
                ipGeoDataset = ipFeatureLayer as IGeoDataset;
                ipEnvelope = ipGeoDataset.Extent;
                ipMaxEnvelope.Union(ipEnvelope);
            }

            m_ipPointToEID = new PointToEIDClass();
            m_ipPointToEID.SourceMap = m_ipMap;
            m_ipPointToEID.GeometricNetwork = m_ipGeometricNetwork;

            double dblWidth = ipMaxEnvelope.Width;
            double dblHeight = ipMaxEnvelope.Height;

            if (dblWidth > dblHeight)
                dblSearchTol = dblWidth / 100;
            else
                dblSearchTol = dblHeight / 100;
            m_ipPointToEID.SnapTolerance = dblSearchTol;

            return true;

        }
        //关闭工作空间          
        private void CloseWorkspace()
        {
            m_ipGeometricNetwork = null;
            m_ipPoints = null;
            m_ipPointToEID = null;
            m_ipEnumNetEID_Junctions = null;
            m_ipEnumNetEID_Edges = null;
            m_ipPolyline = null;
        }

        #endregion

        static void Main(string[] args)
        {


          
        }

    }
}
