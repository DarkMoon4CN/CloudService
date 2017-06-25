using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Core.Hibernate;
using Bsr.Cloud.Core;
using Bsr.Cloud.Model.Entities;

namespace Bsr.Cloud.BLogic
{
    public class PermissionServer
    {
        #region  构参
        private static readonly object _object = new object();
        private static PermissionServer instance;
        private PermissionServer()
        {

        }
        public static PermissionServer GetInstance()
        {
            //如实例不存在，则New一个新实例，否则返回已有实例
            if (instance == null)
            {
                //在同一时刻加了锁的那部分程序只有一个线程可以进入，
                lock (_object)
                {
                    //如实例不存在，则New一个新实例，否则返回已有实例
                    if (instance == null)
                    {
                        instance = new PermissionServer();
                    }
                }
            }
            return instance;
        }
        #endregion  构参
        INHFactory nhFactory = NHFactory.Instance;
        static private ILogger myLog = new Logger<Permission>();
        #region 以用户id查询出权限 
        /// <summary>
        ///  以用户id查询出权限 
        /// </summary>
        /// <param name="customer">customer.CustomerId</param>
        /// <returns></returns>
        public IList<Permission> SelectPermissionByCustomerId(Customer customer) 
        {
            IList<Permission> permissionFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Permission>())
                {
                    sessionFactory.Session.BeginTransaction();
                    permissionFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Permission AS p WHERE p.CustomerId=?")
                        .SetInt32(0, customer.CustomerId).List<Permission>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 51", e, myLog);
            }
            return permissionFlag;
        }
        #endregion

        #region 添加一条权限记录 InsertPermission
        public int InsertPermission(Permission permission)
        {
            int PermissionId = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Permission>())
                {
                    sessionFactory.Session.BeginTransaction();
                    PermissionId = (int)sessionFactory.Save(permission);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 75", e, myLog);
            }
            return PermissionId;
        }
        #endregion

        #region 以customerId 和 nodeId nodeType permissionName查询
        /// <summary>
        ///   以customerId 和 nodeId nodeType permissionName查询
        /// </summary>
        /// <param name="customer">customer.CustomerId</param>
        /// <param name="permission">permission.NodeType,permission.NodeId,permissionName</param>
        /// <returns></returns>
        public IList<Permission> SelectPermissionBySome(Customer customer,Permission permission)
        {
            IList<Permission> permissionFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Permission>())
                {
                    sessionFactory.Session.BeginTransaction();
                    permissionFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Permission AS p WHERE p.CustomerId=? AND p.NodeType=? AND p.NodeId=? AND p.PermissionName=? ")
                        .SetInt32(0, customer.CustomerId)
                        .SetInt32(1,permission.NodeType)
                        .SetInt32(2,permission.NodeId)
                        .SetString(3,permission.PermissionName)
                        .List<Permission>();   
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 101", e, myLog);
            }
            return permissionFlag;
        }
        #endregion

        #region 以customerId,nodeId ,nodeType查询
        /// <summary>
        ///  以customerId,nodeId ,nodeType查询
        /// </summary>
        /// <param name="customer">customerId</param>
        /// <param name="permission">nodeType,nodeId</param>
        /// <returns></returns>
        public IList<Permission> SelectPermissionByCidAndNid(Customer customer, Permission permission)
        {
            IList<Permission> permissionFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Permission>())
                {
                    sessionFactory.Session.BeginTransaction();
                    permissionFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Permission AS p WHERE p.CustomerId=? AND p.NodeType=? AND p.NodeId=? ")
                        .SetInt32(0, customer.CustomerId)
                        .SetInt32(1, permission.NodeType)
                        .SetInt32(2,permission.NodeId)
                        .List<Permission>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 134", e, myLog);
            }
            return permissionFlag;
        }
        #endregion

        #region 自定义批量删除 CustomerId,NodeType
        /// <summary>
        /// 自定义批量删除CustomerId,NodeType
        /// </summary>
        /// <param name="permission">CustomerId,NodeType</param>
        public void DeletePermissionByCidAndNTid(Permission permission)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Permission>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM Permission ";
                    mySqlStr += string.Format(" WHERE NodeType ={0}", permission.NodeType);
                    mySqlStr += string.Format(" AND CustomerId ={0}", permission.CustomerId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 163", e, myLog);
            }
        }
        #endregion

        #region 自定义批量删除 按CustomerId
        /// <summary>
        /// 自定义批量删除 按CustomerId
        /// </summary>
        /// <param name="permission">CustomerId</param>
        public void DeletePermissionByCustomerId(Permission permission)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Permission>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM Permission ";
                    mySqlStr += string.Format(" WHERE CustomerId ={0}", permission.CustomerId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 190", e, myLog);
            }

        }
        #endregion

        #region 查询多个设备是否被授权
        /// <summary>
        ///  查询多个设备是否被授权
        /// </summary>
        /// <param name="primaryCustomerId">主用户的Id</param>
        /// <param name="deviceIdList">设备id集合</param>
        /// <returns></returns>
        public IList<Permission> SelectDeviceAuthorizeByDeviceIdList(int primaryCustomerId, List<int> deviceIdList)
        {
            IList<Permission> permissionFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Permission>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Session.BeginTransaction();
                    permissionFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Permission AS p WHERE p.CustomerId=? AND p.NodeType=1 AND p.NodeId IN (:dList) ")
                        .SetInt32(0, primaryCustomerId)
                        .SetParameterList("dList", deviceIdList)
                        .List<Permission>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 220", e, myLog);
            }
            return permissionFlag;
        }
        #endregion

        #region 自定义批量删除 NodeType,NodeId
        /// <summary>
        /// 自定义批量删除NodeType,NodeId
        /// </summary>
        /// <param name="permission">CustomerId,NodeType</param>
        public void DeletePermissionByNTid(int NodeType,int NodeId)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Permission>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM Permission ";
                    mySqlStr += string.Format(" WHERE NodeType ={0}", NodeType);
                    mySqlStr += string.Format(" AND NodeId ={0}", NodeId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 250", e, myLog);
            }

        }
        #endregion


        #region 以用户id和权限名查询出权限
        /// <summary>
        /// 以用户id和权限名查询出权限
        /// </summary>
        /// <param name="customerId">用户id</param>
        /// <param name="pNameList">权限名集合</param>
        /// <returns></returns>
        public IList<Permission> SelectPermissionByCidAndPname(int customerId,List<string> pNameList)
        {
            IList<Permission> permissionFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Permission>())
                {
                    sessionFactory.Session.BeginTransaction();
                    permissionFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Permission AS p WHERE p.CustomerId=? AND p.PermissionName IN (:pNameList)")
                        .SetInt32(0, customerId)
                        .SetParameterList("pNameList", pNameList)
                        .List<Permission>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 282", e, myLog);
            }
            return permissionFlag;
        }
        #endregion
    }
}

       