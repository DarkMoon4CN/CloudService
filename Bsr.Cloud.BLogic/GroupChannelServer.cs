using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Core.Hibernate;
using Bsr.Cloud.Model.Entities;
using Bsr.Cloud.Core;
using Bsr.Cloud.Model;
using System.Collections;

namespace Bsr.Cloud.BLogic
{
    public class GroupChannelServer
    {
        #region  构参
        private static readonly object _object = new object();
        private static GroupChannelServer instance;
        private GroupChannelServer() { }
        public static GroupChannelServer GetInstance()
        {
            if (instance == null)
            {
                lock (_object)
                {
                    if (instance == null)
                    {
                        instance = new GroupChannelServer();
                    }
                }
            }
            return instance;
        }
        #endregion
        INHFactory nhFactory = NHFactory.Instance;
        static private ILogger myLog = new Logger<GroupChannel>();

        #region  查询某个分组 ResourceGroup.ResourceGroupId组下的通道
        public IList<GroupChannel> SelectGroupChannelByResourceGroupId(ResourceGroup resourceGroup)
        {
            IList<GroupChannel> GroupChannelFlag = null;
            try
            {
                using (var cust = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    cust.Session.BeginTransaction();
                    GroupChannelFlag = cust.Session.GetISession()
                        .CreateQuery(" FROM GroupChannel AS g WHERE g.resourceGroup.ResourceGroupId=? ")
                        .SetInt32(0, resourceGroup.ResourceGroupId).List<GroupChannel>();
                    cust.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 43", e, myLog);
            }

            return GroupChannelFlag;

        }
        #endregion

        #region   添加单条组与通道的关系 InertGroupChannel
        public int InertGroupChannel(GroupChannel groupChannel)
        {
            int GroupChannelId = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    GroupChannelId = (int)sessionFactory.Save(groupChannel);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 68", e, myLog);
            }
            return GroupChannelId;
        }
        #endregion 添加单条组与通道的关系

        #region  移除组与通道关系 DeleteGroupChannel
        public void DeleteGroupChannel(GroupChannel groupChannel)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Delete(groupChannel);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 88", e, myLog);
            }
        }
        #endregion 移除组与通道关系

        #region  通道分组关系表以通道Id Date:2014/12/15
        public IList<GroupChannel> SelectGroupChannelByChannelId(GroupChannel groupChannel)
        {
            IList<GroupChannel> GroupChannelFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    GroupChannelFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM GroupChannel AS g WHERE g.channel.ChannelId=? ")
                        .SetInt32(0, groupChannel.channel.ChannelId).List<GroupChannel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 108", e, myLog);
            }

            return GroupChannelFlag;

        }
        #endregion

        #region 更新通道分组关系表 Date:2014/12/15
        public void UpdateGroupChannel(GroupChannel groupChannel)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Update(groupChannel);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 132", e, myLog);
            }
        }
        #endregion 

        #region  查询某个分组下所有通道信息 Date:2014/12/15
        public IList<GroupChannel> SelectGroupChannelByResourceGroupIdList(List<int> resourceGroupIdList)
        {
            IList<GroupChannel> GroupChannelFlag = null;
            try
            {
                using (var resourceGroupSessionFactory = nhFactory.GetRepositoryFor<ResourceGroup>())
                using (var channelSessionFactory = nhFactory.GetRepositoryFor<Channel>())
                using (var groupChannelsessionFactory = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    groupChannelsessionFactory.Session.BeginTransaction();
                    GroupChannelFlag = groupChannelsessionFactory.Session.GetISession()
                        .CreateQuery(" FROM GroupChannel AS g WHERE g.resourceGroup.ResourceGroupId IN(:rList)")
                        .SetParameterList("rList", resourceGroupIdList).List<GroupChannel>();
                    groupChannelsessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 151", e, myLog);
            }

            return GroupChannelFlag;

        }
        #endregion

        #region  通道分组关系表以通道Id Date:2014/12/15
        public IList<GroupChannel> SelectGroupChannelByChannelIdAndResourceGroupId(GroupChannel groupChannel)
        {
            IList<GroupChannel> GroupChannelFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    GroupChannelFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM GroupChannel AS g WHERE g.channel.ChannelId=? AND g.resourceGroup.ResourceGroupId=?")
                        .SetInt32(0, groupChannel.channel.ChannelId)
                        .SetInt32(1,groupChannel.resourceGroup.ResourceGroupId)
                        .List<GroupChannel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 180", e, myLog);
            }

            return GroupChannelFlag;

        }
        #endregion

        #region  以用户Id和通道Id查询通道分组关系表
        public IList<GroupChannel> SelectGroupChannelByChannelIdAndCustomerId(GroupChannel groupChannel)
        {
            IList<GroupChannel> GroupChannelFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    GroupChannelFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM GroupChannel AS g WHERE g.channel.ChannelId=? AND g.CustomerId=?")
                        .SetInt32(0, groupChannel.channel.ChannelId)
                        .SetInt32(1, groupChannel.CustomerId)
                        .List<GroupChannel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 205", e, myLog);
            }

            return GroupChannelFlag;

        }
        #endregion



        #region  以用户Id和通道Id集合查询通道分组关系表
        /// <summary>
        /// 以用户Id和通道Id集合查询通道分组关系表
        /// </summary>
        /// <param name="customerId">用户Id</param>
        /// <param name="channelIdList">通道集合</param>
        /// <returns></returns>
        public IList<GroupChannel> SelectGroupChannelByChannelIdListAndCustomerId(int customerId,List<int> channelIdList)
        {
            IList<GroupChannel> GroupChannelFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    GroupChannelFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM GroupChannel AS g WHERE g.CustomerId=? AND g.channel.ChannelId IN(:cList) ")
                        .SetInt32(0, customerId)
                        .SetParameterList("cList", channelIdList)
                        .List<GroupChannel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 205", e, myLog);
            }

            return GroupChannelFlag;

        }
        #endregion


        #region 自定义批量删除 按 customerId
        /// <summary>
        /// NHibernate自定义sql做删除语法
        /// </summary>
        /// <param name="customer">customer.CustomerId</param>
        public void DeleteGroupChannelByCustomerId(Customer customer) 
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM groupchannel ";
                    mySqlStr += string.Format(" WHERE CustomerId =({0}) ", customer.CustomerId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 235", e, myLog);
            }

        }
        #endregion

        #region  查询分组下(包括子分组)的所有通道 分页 
        public IList SelectChannelByPageOrResourceGroupId
            (int startCount, int requestCount, string keyWord, string deviceIdList, string resourceGroupIdListStr)
        {
            IList result = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT ch.ChannelId,gc.ResourceGroupId,ch.ChannelName,ch.ChannelNumber,ch.DeviceId,ch.IsEnable,ch.ImagePath ";
                    mySqlStr += " FROM channel as ch ,groupchannel AS gc ";
                    mySqlStr += " WHERE gc.ChannelId=ch.ChannelId  ";
                    mySqlStr += string.Format(" AND ch.DeviceId IN({0}) ", deviceIdList);
                    mySqlStr += string.Format(" AND gc.ResourceGroupId IN({0}) ", resourceGroupIdListStr);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ",keyWord);
                    mySqlStr += string.Format(" ORDER BY ch.ChannelId LIMIT {0},{1} ", startCount, requestCount);
                    result = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).List();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 260", e, myLog);
            }
            return result;
        }
        #endregion

        #region  查询分组下(包括子分组)的所有通道 分页(总条数)
        public int SelectChannelByPageOrResourceGroupIdCount
            (string keyWord, string deviceIdList, string resourceGroupIdListStr)
        {
            object count = 0;
            try
            {
                using (var cust = nhFactory.GetRepositoryFor<Channel>())
                {
                    cust.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr  = " SELECT COUNT(*) ";
                    mySqlStr += " FROM channel as ch ,groupchannel AS gc ";
                    mySqlStr += " WHERE gc.ChannelId=ch.ChannelId  ";
                    mySqlStr += string.Format(" AND ch.DeviceId IN({0}) ", deviceIdList);
                    mySqlStr += string.Format(" AND gc.ResourceGroupId IN({0}) ", resourceGroupIdListStr);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    count = cust.Session.GetISession().CreateSQLQuery(mySqlStr).UniqueResult();
                    cust.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 291", e, myLog);
            }
            return Convert.ToInt32(count);
        }
        #endregion

        #region  查询分组下(包括子分组)的所有通道分页-子用户
        public IList SelectSubChannelByPageOrResourceGroupId
            (int subCustomerid, int startCount, int requestCount, string keyWord, string resourceGroupIdListStr)
        {
            IList result = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT DISTINCT ch.ChannelId,gc.ResourceGroupId,ch.ChannelName,ch.ChannelNumber,ch.DeviceId,ch.IsEnable,ch.ImagePath ";
                    mySqlStr += " FROM channel as ch,groupchannel AS gc,permission as pm";
                    mySqlStr += " WHERE gc.ChannelId=ch.ChannelId  AND pm.NodeId=gc.ChannelId ";
                    mySqlStr += " AND pm.NodeType=2 ";
                    mySqlStr += string.Format(" AND pm.CustomerId={0} ", subCustomerid);
                    mySqlStr += string.Format(" AND gc.ResourceGroupId IN({0}) ", resourceGroupIdListStr);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    mySqlStr += string.Format(" ORDER BY ch.ChannelId LIMIT {0},{1} ", startCount, requestCount);
                    result = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).List();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 320", e, myLog);
            }
            return result;
        }
        #endregion

        #region  查询分组下(包括子分组)的所有通道 分页(总条数)-子用户
        public int SelectSubChannelByPageOrResourceGroupIdCount
            (int subCustomerid, string keyWord, string resourceGroupIdListStr)
        {
            object count = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT COUNT(DISTINCT ch.ChannelId) AS count";
                    mySqlStr += " FROM channel as ch ,groupchannel AS gc,permission as pm";
                    mySqlStr += " WHERE gc.ChannelId=ch.ChannelId  AND pm.NodeId=gc.ChannelId ";
                    mySqlStr += " AND pm.NodeType=2 ";
                    mySqlStr += string.Format(" AND pm.CustomerId={0} ", subCustomerid);
                    mySqlStr += string.Format(" AND gc.ResourceGroupId IN({0}) ", resourceGroupIdListStr);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    count = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).UniqueResult();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 351", e, myLog);
            }
            return Convert.ToInt32(count);
        }
        #endregion

        #region  查询未分组下的通道-主用户 
        public IList<Channel> SelectChannelByNoGroupPage
            (int primaryCustomerId, int startCount, int requestCount, string keyWord, string deviceIdList)
        {
            IList<Channel> channelFalg = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT ch.ChannelId,ch.ChannelName,ch.ChannelNumber,ch.DeviceId,ch.IsEnable,ch.ImagePath,ch.BPServerChannelId ";
                    mySqlStr += " FROM channel AS ch ";
                    mySqlStr += string.Format(" WHERE ch.DeviceId IN({0}) ", deviceIdList);
                    mySqlStr += string.Format(" AND ch.ChannelId NOT IN (SELECT gc.ChannelId FROM groupchannel AS gc WHERE gc.CustomerId=({0})) ",primaryCustomerId);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    mySqlStr += string.Format(" ORDER BY ch.ChannelId LIMIT {0},{1} ", startCount, requestCount);
                    channelFalg = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).AddEntity(typeof(Channel))
                                         .List<Channel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 380", e, myLog);
            }
            return channelFalg;
        }
        #endregion

        #region  查询未分组下的通道(总条数)-主用户
        public int SelectChannelByNoGroupPageCount
            (int primaryCustomerId, string keyWord, string deviceIdList)
        {
            object count = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT COUNT(*) ";
                    mySqlStr += " FROM channel AS ch ";
                    mySqlStr += string.Format(" WHERE ch.DeviceId IN({0}) ", deviceIdList);
                    mySqlStr += string.Format(" AND ch.ChannelId NOT IN (SELECT gc.ChannelId FROM groupchannel AS gc WHERE gc.CustomerId=({0})) ", primaryCustomerId);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    count = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).UniqueResult();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 410", e, myLog);
            }
            return Convert.ToInt32(count);
        }
        #endregion

        #region  查询未分组下的通道-子用户 
        public IList<Channel> SelectSubChannelByNoGroupPage
            (int primaryCustomerId, int startCount, int requestCount, string keyWord, string deviceIdList)
        {
            IList<Channel> channelFalg = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT ch.ChannelId,ch.ChannelName,ch.ChannelNumber,ch.DeviceId,ch.IsEnable,ch.ImagePath,ch.BPServerChannelId ";
                    mySqlStr += " FROM channel AS ch, permission AS pm ";
                    mySqlStr += " WHERE ch.ChannelId=pm.NodeId ";
                    mySqlStr += " AND pm.NodeType=2 ";
                    mySqlStr += " AND pm.IsEnable=1 ";
                    mySqlStr += " AND ch.IsEnable=1 ";
                    mySqlStr += string.Format(" AND ch.DeviceId IN({0}) ", deviceIdList);
                    mySqlStr += string.Format(" AND ch.ChannelId NOT IN (SELECT gc.ChannelId FROM groupchannel AS gc WHERE gc.CustomerId=({0})) ", primaryCustomerId);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    mySqlStr += string.Format(" ORDER BY ch.ChannelId LIMIT {0},{1} ", startCount, requestCount);
                    channelFalg = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).AddEntity(typeof(Channel))
                                         .List<Channel>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 440", e, myLog);
            }
            return channelFalg;
        }
        #endregion

        #region  查询未分组下的通道(总条数)-子用户
        public int SelectSubChannelByNoGroupPageCount
            (int primaryCustomerId, string keyWord, string deviceIdList)
        {
            object count = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT COUNT(*) ";
                    mySqlStr += " FROM channel AS ch, permission AS pm ";
                    mySqlStr += " WHERE ch.ChannelId=pm.NodeId ";
                    mySqlStr += " AND pm.NodeType=2 ";
                    mySqlStr += " AND pm.IsEnable=1 ";
                    mySqlStr += " AND ch.IsEnable=1 ";
                    mySqlStr += string.Format(" AND ch.DeviceId IN({0}) ", deviceIdList);
                    mySqlStr += string.Format(" AND ch.ChannelId NOT IN (SELECT gc.ChannelId FROM groupchannel AS gc WHERE gc.CustomerId=({0})) ", primaryCustomerId);
                    mySqlStr += string.Format(" AND ch.ChannelName LIKE '%{0}%' ", keyWord);
                    count = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).UniqueResult();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 471", e, myLog);
            }
            return Convert.ToInt32(count);
        }
        #endregion

        #region 自定义批量删除 按 channelId
        /// <summary>
        /// NHibernate自定义sql做删除语法 按 channelId
        /// </summary>
        public void DeleteGroupChannelByChannelId(Channel channel)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<GroupChannel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM groupchannel ";
                    mySqlStr += string.Format(" WHERE ChannelId =({0}) ", channel.ChannelId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 506", e, myLog);
            }

        }
        #endregion
    }
}
