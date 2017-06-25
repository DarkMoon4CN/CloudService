using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Core.Hibernate;
using Bsr.Cloud.Model.Entities;
using Bsr.Cloud.Core;
using Bsr.Cloud.BLogic.BLL;
using System.Collections;
using Bsr.Cloud.Model;

namespace Bsr.Cloud.BLogic
{
    public class ChannelServer
    {
       #region  构参
        private static readonly object _object = new object();
        private static ChannelServer instance;
        private ChannelServer() { }
        public static ChannelServer GetInstance()
        {
            if (instance == null)
            {
                lock (_object)
                {
                    if (instance == null)
                    {
                        instance = new ChannelServer();
                    }
                }
            }
            return instance;
        }
        #endregion
        INHFactory nhFactory = NHFactory.Instance;
        static private ILogger myLog = new Logger<ChannelServer>();

        #region 查询某个设备下通道
        public IList<Channel> SelectChannelByDeviceId(Device devcie)
        {
            IList<Channel> ChannelFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    ChannelFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Channel AS c WHERE c.DeviceId=?")
                        .SetInt32(0, devcie.DeviceId).List<Channel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 44", e, myLog);
            }
            return ChannelFlag;
        }
        #endregion

        #region 添加通道信息
        public int InsertChannel(Channel channel)
        {
            int ChannelId=0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    ChannelId =(int)sessionFactory.Save(channel);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 65", e, myLog);
            }

            return ChannelId;

        }
        #endregion

        #region 更新通道
        public void UpdateChannel(Channel channel)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Update(channel);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 88", e, myLog);
            }
        }
        #endregion 更新通道

        #region 查询多个设备下通道
        /// <summary>
        /// 查询多个设备下通道
        /// </summary>
        /// <param name="deviceList">设备集合</param>
        /// <returns></returns>
        public IList<Channel> SelectChannelByDeviceIdList(List<int> deviceIdList)
        {
            IList<Channel> ChannelFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    ChannelFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Channel AS c WHERE c.DeviceId IN (:dList)")
                        .SetParameterList("dList", deviceIdList).List<Channel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 114", e, myLog);
            }

            return ChannelFlag;

        }
        #endregion

        #region 更新通道分组
        public void UpdateChannelByResourceGroupId(Channel channel,ResourceGroup resourceGroup)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    //sessionFactory.Session.GetISession()
                    //    .CreateQuery(" UPDATE Channel AS c SET c.ResourceGroupId=?  WHERE c.ResourceGroupId=?")
                    //    .SetInt32(0, channel.ResourceGroupId)
                    //    .SetInt32(1, resourceGroup.ResourceGroupId);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 140", e, myLog);
            }
        }
        #endregion 更新通道

        #region 查询通道属于哪个分组
        /// <summary>
        /// 查询通道属于哪个分组
        /// </summary>
        /// <param name="channel">ResourceGroupId</param>
        /// <returns></returns>
        public IList<Channel> SelectChannelByResourceGroupId(ResourceGroup resourceGroup)
        {
            IList<Channel> ChannelFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    ChannelFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Channel AS c WHERE c.ResourceGroupId =?")
                        .SetInt32(0, resourceGroup.ResourceGroupId).List<Channel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 166", e, myLog);
            }
            return ChannelFlag;
        }
        #endregion

        #region 更新通道的组信息
        /// <summary>
        /// 更新通道信息
        /// </summary>
        /// <param name="channel">ResourceGroupId</param>
        /// <returns></returns>
        public void UpdateChannelByResourceGroupId(Channel channel)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Update(channel);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 193", e, myLog);
            }
        }
        #endregion

        #region 以通道Id查询通道信息
        public IList<Channel> SelectChannelByChannelId(Channel channel)
        {
            IList<Channel> channelFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    channelFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Channel AS c WHERE c.ChannelId=?")
                        .SetInt32(0, channel.ChannelId).List<Channel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 213", e, myLog);
            }

            return channelFlag;

        }
        #endregion

        #region 查询多个组下通道
        /// <summary>
        /// 查询多个组下通道
        /// </summary>
        /// <param name="resourceGroupIdList">组集合</param>
        /// <returns></returns>
        public IList<Channel> SelectChannelByResourceGroupIdList(List<int> resourceGroupIdList)
        {
            IList<Channel> ChannelFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    ChannelFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Channel AS c WHERE c.ResourceGroupId IN (:rList)")
                        .SetParameterList("rList", resourceGroupIdList).List<Channel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 245", e, myLog);
            }

            return ChannelFlag;

        }
        #endregion

        #region  删除通道以通道Id
        public void DeleteChannel(Channel channel)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Session.GetISession()
                        .Delete(channel);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 266", e, myLog);
            }
        }
        #endregion 删除通道以设备ID

        #region 自定义批量删除
        /// <summary>
        /// 自定义sql做删除语法
        /// </summary>
        /// <param name="device">device.DeviceId</param>
        public void DeleteChannelByDeviceId(Device device)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM Channel ";
                    mySqlStr += string.Format(" WHERE DeviceId =({0}) ", device.DeviceId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 291", e, myLog);
            }

        }
        #endregion

        #region 查询多个设备下通道分页 -主用户
        /// <summary>
        /// 查询多个设备下通道分页 -主用户
        /// </summary>
        /// <param name="deviceIdList">设备Id集合</param>    
        /// <param name="keyWord">关键字</param>
        /// <returns></returns>
        public IList<Channel> SelectChannelByDeviceIdListPage(string deviceIdList, int startCount, int requestCount, string keyWord)
        {
            IList<Channel> GroupChannelResponseFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr =string.Empty;
                    mySqlStr = " SELECT ch.ChannelId,ch.ChannelName,ch.ChannelNumber,ch.DeviceId,ch.IsEnable,ch.ImagePath,ch.BPServerChannelId ";
                    mySqlStr += " FROM channel as ch ";
                    mySqlStr +=string.Format(" WHERE ch.DeviceId IN({0}) ",deviceIdList);
                    mySqlStr +=string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    mySqlStr +=string.Format(" ORDER BY ch.ChannelId DESC LIMIT {0},{1} ",startCount,requestCount);
                    GroupChannelResponseFlag = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr)
                                       .AddEntity(typeof(Channel))
                                         .List<Channel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 322", e, myLog);
            }

            return GroupChannelResponseFlag;

        }
        #endregion

        #region 查询多个设备下通道分页总数 -主用户
        /// <summary>
        /// 查询多个设备下通道分页总数 -主用户
        /// </summary>
        /// <returns>总条数</returns>
        public int SelectChannelByDeviceIdListPageCount(string deviceIdList, string keyWord)
        {
            object count = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr  = " SELECT COUNT(*) ";
                    mySqlStr += " FROM channel as ch ";
                    mySqlStr += string.Format(" WHERE ch.DeviceId IN({0}) ", deviceIdList);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    count = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr)
                                       .UniqueResult();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 357", e, myLog);
            }

            return Convert.ToInt32(count);

        }
        #endregion

        #region 自定义多表条件查询 以device.CustomerId,channel.ChannelId
        /// <summary>
        /// 自定义多表条件查询 以CustomerId,DevcieId,ChannelId,Channel表中的IsEnble
        /// </summary>
        public IList SelectChannelAndDeviceBySome(Device device,Channel channel)
        {
            IList result = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr  = " SELECT d.CustomerId,d.DeviceId,c.channelId,c.IsEnable FROM device as d, channel as c  ";
                    mySqlStr += " WHERE d.DeviceId=c.DeviceId    ";
                    mySqlStr += string.Format(" AND d.CustomerId=({0}) ", device.CustomerId);
                    mySqlStr += string.Format(" AND c.ChannelId =({0}) ", channel.ChannelId);
                    result    = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).List();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 388", e, myLog);
            }
            return result;
        }
        #endregion

        #region 查询多个设备下通道分页-子用户
        /// <summary>
        /// 查询多个设备下通道分页
        /// </summary>
        /// <param name="deviceIdList">设备Id集合</param>    
        /// <param name="keyWord">关键字</param>
        /// <returns></returns>
        public IList<Channel> SelectSubChannelByDeviceIdListPage(int subCustomerId,string deviceIdList, int startCount, int requestCount, string keyWord)
        {
            IList<Channel> channelFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT  DISTINCT ch.ChannelId,ch.ChannelName,ch.ChannelNumber,ch.DeviceId,ch.IsEnable,ch.ImagePath,ch.BPServerChannelId ";
                    mySqlStr += " FROM channel as ch ,permission as pm ";
                    mySqlStr += " WHERE pm.NodeId=ch.ChannelId ";
                    mySqlStr += " AND pm.NodeType=2 ";
                    mySqlStr += " AND pm.IsEnable=1 ";
                    mySqlStr += string.Format(" AND pm.CustomerId =({0}) ", subCustomerId);
                    mySqlStr += string.Format(" AND ch.DeviceId IN({0}) ", deviceIdList);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    mySqlStr += string.Format(" ORDER BY ch.ChannelId DESC LIMIT {0},{1} ", startCount, requestCount);
                    channelFlag = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr)
                                       .AddEntity(typeof(Channel))
                                         .List<Channel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 422", e, myLog);
            }
            return channelFlag;
        }
        #endregion

        #region 查询多个设备下通道分页总数-子用户
        /// <summary>
        /// 查询多个设备下通道分页总数-子用户
        /// </summary>
        /// <param name="deviceIdList">设备Id集合</param>    
        /// <param name="keyWord">关键字</param>
        /// <returns></returns>
        public int SelectSubChannelByDeviceIdListPageCount(int subCustomerId, string deviceIdList,string keyWord)
        {
            object count=0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = "  SELECT COUNT(DISTINCT ch.ChannelId) ";
                    mySqlStr += " FROM channel as ch ,permission as pm ";
                    mySqlStr += " WHERE pm.NodeId=ch.ChannelId ";
                    mySqlStr += " AND pm.NodeType=2 ";
                    mySqlStr += " AND pm.IsEnable=1 ";
                    mySqlStr += string.Format(" AND pm.CustomerId IN({0}) ", subCustomerId);
                    mySqlStr += string.Format(" AND ch.DeviceId IN({0}) ", deviceIdList);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    count = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr)
                                       .UniqueResult();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 460", e, myLog);
            }
            return Convert.ToInt32(count);
        }
        #endregion

        
        
    }
}
