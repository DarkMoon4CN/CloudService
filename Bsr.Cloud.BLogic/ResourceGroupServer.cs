using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model.Entities;
using Bsr.Core.Hibernate;
using Bsr.Cloud.Core;

namespace Bsr.Cloud.BLogic
{
    public class ResourceGroupServer
    {
        #region  构参
        private static readonly object _object = new object();
        private static ResourceGroupServer instance;
        private ResourceGroupServer()
        {
           // Log4NetOutPut.OutPutInit();
        }
        public static ResourceGroupServer GetInstance()
        {
            if (instance == null)
            {
                lock (_object)
                {
                    if (instance == null)
                    {
                        instance = new ResourceGroupServer();
                    }
                }
            }
            return instance;
        }
        #endregion  构参
        INHFactory nhFactory = NHFactory.Instance;
        static private ILogger myLog = new Logger<ResourceGroupServer>();
    
        #region  添加单条分组信息
        public int InsertResourceGorup(ResourceGroup resourceGroup)
        {
            int ResourceGroupId = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<ResourceGroup>())
                {
                    sessionFactory.Session.BeginTransaction();
                    ResourceGroupId = (int)sessionFactory.Save(resourceGroup);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 44", e, myLog);
            }
            return ResourceGroupId;
        }
        #endregion 添加单条分组信息
        
        #region  移出单条分组信息
        public void  DeleteResourceGorup(ResourceGroup resourceGroup)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<ResourceGroup>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Delete(resourceGroup);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 64", e, myLog);
            }
        }
        #endregion 移出单条分组信息

        #region 更新分组名称
        public void UpdateResourceGorupByName(ResourceGroup resourceGroup)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<ResourceGroup>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Update(resourceGroup);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 83", e, myLog);
            }
        }
        #endregion 更新分组名称

        #region 查询分组
        public IList<ResourceGroup> SelectResourceGorupByResourceGroupId(ResourceGroup resourceGroup)
        {
            IList<ResourceGroup> resourceGroupFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<ResourceGroup>())
                {
                    sessionFactory.Session.BeginTransaction();
                    resourceGroupFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM ResourceGroup AS r WHERE r.ResourceGroupId=?")
                        .SetInt32(0, resourceGroup.ResourceGroupId).List<ResourceGroup>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 103", e, myLog);
            }
            return resourceGroupFlag;
        }
        #endregion 查询子分组

        #region 查询子分组
        public IList<ResourceGroup> SelectResourceGorupByChildId(ResourceGroup resourceGroup)
        {
            IList<ResourceGroup> resourceGroupFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<ResourceGroup>())
                {
                    sessionFactory.Session.BeginTransaction();
                    resourceGroupFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM ResourceGroup AS r WHERE r.ParentResourceGroupId=?")
                        .SetInt32(0, resourceGroup.ResourceGroupId).List<ResourceGroup>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 126", e, myLog);
            }
            return resourceGroupFlag;
        }
        #endregion 查询子分组

        #region 查询当前用户下的分组
        public IList<ResourceGroup> SelectResourceGorupByCustomerId(int customerId)
        {
            IList<ResourceGroup> resourceGroupFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<ResourceGroup>())
                {
                    sessionFactory.Session.BeginTransaction();
                    resourceGroupFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM ResourceGroup AS r WHERE r.CustomerId=?")
                        .SetInt32(0, customerId).List<ResourceGroup>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 150", e, myLog);
            }
            return resourceGroupFlag;
        }
        #endregion 查询子分组

        #region 更新分组位置
        public void UpdateResourceGorupByParentId(ResourceGroup resourceGroup)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<ResourceGroup>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Update(resourceGroup);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 171", e, myLog);
            }
        }
        #endregion 更新分组名称

        #region 批量删除分组
        /// <summary>
        /// 自定义批量删除
        /// </summary>
        /// <param name="customer">customer.CustomerId</param>
        public void DeleteResourceGorupByCustomerId(Customer customer)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<ResourceGroup>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM resourcegroup ";
                    mySqlStr += string.Format(" WHERE CustomerId =({0}) ", customer.CustomerId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 194", e, myLog);
            }
        }
        #endregion

        #region  查询某个子用户的分组，这写分组节点都有通道，没有通道的分组不会返回
        /// <summary>
        /// 查询某个子用户的分组，这写分组节点都有通道，没有通道的分组不会返回
        /// </summary>
        /// <param name="customerId">子用户的id</param>
        /// <param name="primaryCustomerId">该子用户的父id</param>
        /// <returns></returns>
        public IList<ResourceGroup> GetResourceGroupBySubCustomerId(int subCustomerId, int primaryCustomerId)
        {
            IList<ResourceGroup> resourceGroupFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<ResourceGroup>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT rg.ResourceGroupId, rg.ResourceGroupName, rg.ParentResourceGroupId, rg.CustomerId  ";
                    mySqlStr += " FROM resourcegroup AS rg, groupchannel AS gc , permission AS pms   ";
                    mySqlStr += " WHERE rg.ResourceGroupId = gc.ResourceGroupId   ";
                    mySqlStr += " AND gc.ChannelId = pms.NodeId ";
                    mySqlStr += " AND pms.NodeType=2   ";
                    mySqlStr += " AND pms.IsEnable = 1 ";
                    mySqlStr += string.Format(" AND rg.CustomerId={0}  ", primaryCustomerId);
                    mySqlStr += string.Format(" AND pms.CustomerId={0} ", subCustomerId);
                    mySqlStr += " GROUP BY rg.ResourceGroupId ";
                    resourceGroupFlag = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).AddEntity(typeof(ResourceGroup)).List<ResourceGroup>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 223", e, myLog);
            }
            return resourceGroupFlag;
        }
        #endregion
    }
}
