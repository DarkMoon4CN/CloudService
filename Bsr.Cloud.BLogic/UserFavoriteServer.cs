using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Core.Hibernate;
using Bsr.Cloud.Core;
using Bsr.Cloud.Model.Entities;

namespace Bsr.Cloud.BLogic
{
    public class UserFavoriteServer
    {
        #region  构参
        private static readonly object _object = new object();
        private static UserFavoriteServer instance;
        private UserFavoriteServer()
        {
           // Log4NetOutPut.OutPutInit();
        }
        public static UserFavoriteServer GetInstance()
        {
            if (instance == null)
            {
                lock (_object)
                {
                    if (instance == null)
                    {
                        instance = new UserFavoriteServer();
                    }
                }
            }
            return instance;
        }
        #endregion  构参
        INHFactory nhFactory = NHFactory.Instance;
        static private ILogger myLog = new Logger<UserFavorite>();

        #region 添加单个收藏
        public int InsertUserFavorite(UserFavorite userFavorite)
        {
            int userFavoriteId = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<UserFavorite>())
                {
                    sessionFactory.Session.BeginTransaction();
                    userFavoriteId = (int)sessionFactory.Save(userFavorite);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 42", e, myLog);
            }
            return userFavoriteId;
        }
        #endregion

        #region 批量删除收藏 按userFavoriteId数组
        /// <summary>
        ///  批量删除收藏 按userFavoriteId数组
        /// </summary>
        /// <param name="userFavoriteIdList">多个suerFavoriteId 集合</param>
        /// <returns></returns>
        public void DeleteUserFavorite(int customerId,string userFavoriteIdList)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<UserFavorite>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM UserFavorite ";
                    mySqlStr += string.Format(" WHERE UserFavoriteId IN ({0}) ", userFavoriteIdList);
                    mySqlStr += string.Format(" AND CustomerId={0} ", customerId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 64", e, myLog);
            }
        }
        #endregion

        #region 分页查询收藏信息
        /// <summary>
        /// 分页查询收藏信息
        /// </summary>
        /// <param name="startCount"></param>
        /// <param name="requestCount"></param>
        /// <returns></returns>
        public IList<UserFavorite> SelectUserFavoriteByPage(int customerId,int startCount, int requestCount)
        {
            IList<UserFavorite> GroupChannelResponseFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<UserFavorite>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT uf.UserFavoriteId,uf.UserFavoriteType,uf.UserFavoriteTypeId,uf.FavoriteTime,uf.CustomerId,uf.AliasName ";
                    mySqlStr += " FROM UserFavorite as uf ";
                    mySqlStr += string.Format(" WHERE uf.CustomerId= {0}", customerId);
                    mySqlStr += string.Format(" ORDER BY uf.UserFavoriteId DESC LIMIT {0},{1} ", startCount, requestCount);
                    GroupChannelResponseFlag = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr)
                                       .AddEntity(typeof(UserFavorite))
                                         .List<UserFavorite>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 95", e, myLog);
            }

            return GroupChannelResponseFlag;

        }
        #endregion

        #region 分页查询收藏信息总条数
        /// <summary>
        /// 分页查询收藏信息总条数
        /// </summary>
        /// <returns>总条数</returns>
        public int SelectUserFavoriteByPageCount(int customerId)
        {
            object count = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " SELECT COUNT(*) ";
                    mySqlStr += " FROM UserFavorite as uf ";
                    mySqlStr += string.Format(" WHERE uf.CustomerId= {0}", customerId);
                    count = sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr)
                                       .UniqueResult();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 130", e, myLog);
            }
            return Convert.ToInt32(count);
        }
        #endregion

        #region 自定义批量删除 按customerId
        /// <summary>
        /// 自定义批量删除 按customerId
        /// </summary>
        /// <param name="device">device.DeviceId</param>
        public void DeleteUserFavoriteByCustomerId(int customerId)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<UserFavorite>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM UserFavorite ";
                    mySqlStr += string.Format(" WHERE CustomerId =({0}) ", customerId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 158", e, myLog);
            }

        }
        #endregion

        #region 自定义批量删除 按userFavoriteType和userFavoriteTypeId
        /// <summary>
        /// 自定义批量删除 按userFavoriteType和userFavoriteTypeId
        /// </summary>
        /// <param name="userFavoriteType">类型</param>
        /// <param name="userFavoriteTypeId">实际Id</param>
        public void DeleteUserFavoriteByType(int userFavoriteType, int userFavoriteTypeId)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<UserFavorite>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM UserFavorite ";
                    mySqlStr += string.Format(" WHERE UserFavoriteType =({0}) ", userFavoriteType);
                    mySqlStr += string.Format(" AND UserFavoriteTypeId =({0}) ", userFavoriteTypeId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 186", e, myLog);
            }
        }
        #endregion

        #region  查询收藏(按userFavoriteId)
        /// <summary>
        /// 查询收藏(按userFavoriteId)
        /// </summary>
        /// <param name="userFavoriteId">userFavoriteId</param>
        /// <returns></returns>
        public IList<UserFavorite> SelectCustomerByUserFavoriteId(int userFavoriteId)
        {
            IList<UserFavorite> userFavoriteFlag = null;
            try
            {
                using (var cust = nhFactory.GetRepositoryFor<UserFavorite>())
                {
                    cust.Session.BeginTransaction();
                    userFavoriteFlag = cust.Session.GetISession()
                             .CreateQuery(" FROM UserFavorite AS u WHERE u.UserFavoriteId=?")
                             .SetInt32(0, userFavoriteId).List<UserFavorite>();
                    cust.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 215", e, myLog);
            }

            return userFavoriteFlag;
        }
        #endregion  

        #region  查询收藏(按UserFavoriteType 和 UserFavoriteTypeId)
        /// <summary>
        ///   查询收藏(按UserFavoriteType 和 UserFavoriteTypeId)
        /// </summary>
        /// <param name="userFavorite">UserFavoriteType、UserFavoriteTypeId</param>
        /// <returns></returns>
        public IList<UserFavorite> SelectCustomerByTid(UserFavorite userFavorite)
        {
            IList<UserFavorite> userFavoriteFlag = null;
            try
            {
                using (var cust = nhFactory.GetRepositoryFor<UserFavorite>())
                {
                    cust.Session.BeginTransaction();
                    userFavoriteFlag = cust.Session.GetISession()
                             .CreateQuery(" FROM UserFavorite AS u WHERE u.UserFavoriteType=? AND u.UserFavoriteTypeId=? AND u.CustomerId=?")
                             .SetInt32(0, userFavorite.UserFavoriteType)
                             .SetInt32(1,userFavorite.UserFavoriteTypeId)
                             .SetInt32(2,userFavorite.CustomerId).List<UserFavorite>();
                    cust.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 245", e, myLog);
            }

            return userFavoriteFlag;
        }
        #endregion  

        #region 批量删除收藏 按userFavoriteTypeIdList数组
        /// <summary>
        /// 批量删除收藏 按userFavoriteTypeIdList数组
        /// </summary>
        /// <param name="userFavoriteType">多个 nodeid 集合</param>
        /// <param name="userFavoriteTypeIdList"></param>
        /// <returns></returns>
        public void DeleteUserFavorite(int customerId,int userFavoriteType, string userFavoriteTypeIdList)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<UserFavorite>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM UserFavorite ";
                    mySqlStr += string.Format(" WHERE UserFavoriteType={0}", userFavoriteType);
                    mySqlStr += string.Format(" AND CustomerId={0} ",customerId);
                    mySqlStr += string.Format(" AND UserFavoriteTypeId IN ({0}) ", userFavoriteTypeIdList);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 280", e, myLog);
            }
        }
        #endregion

    }
}
