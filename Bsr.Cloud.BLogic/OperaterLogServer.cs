using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model.Entities;
using Bsr.Core.Hibernate;
using Bsr.Cloud.Core;

namespace Bsr.Cloud.BLogic
{
    public class OperaterLogServer
    {
        #region  构参
        private static readonly object _object = new object();
        private static OperaterLogServer instance;
        private OperaterLogServer() { }
        public static OperaterLogServer GetInstance()
        {
            if (instance == null)
            {
                lock (_object)
                {
                    if (instance == null)
                    {
                        instance = new OperaterLogServer();
                    }
                }
            }
            return instance;
        }
        #endregion
        INHFactory nhFactory = NHFactory.Instance;
        static private ILogger myLog = new Logger<OperaterLogServer>();

        #region  添加用户日志 InsertOperaterLog
        /// <summary>
        /// 添加用户日志
        /// </summary>
        /// <param name="operaterLog">日志 实体</param>
        /// <returns>operaterLogId</returns>
        public  int InsertOperaterLog(OperaterLog operaterLog)
        {
            int operaterLogId = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<OperaterLog>())
                {
                    sessionFactory.Session.BeginTransaction();
                    operaterLogId = (int)sessionFactory.Save(operaterLog);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 45", e, myLog);
            }
            return operaterLogId;
        }
        #endregion

        #region  分页-日志查询 OperaterLogForPage
        /// <summary>
        /// 分页-日志查询
        /// </summary>
        /// <param name="customerIdList">用户Id集合,其他用法：可查出当前子用户的登陆信息</param>
        /// <param name="pageSize">信息条目</param>
        /// <param name="page">信息页数</param>
        /// <param name="actionWhere">动作条件</param>
        /// <returns></returns>
        public IList<OperaterLog> OperaterLogForPage(List<int> customerIdList, int requestCount, int startCount, string actionWhere)
        {
            IList<OperaterLog> operaterLogFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<OperaterLog>())
                {
                    sessionFactory.Session.BeginTransaction();
                    operaterLogFlag = sessionFactory.Session.GetISession().
                        CreateQuery(" FROM OperaterLog AS o WHERE o.CustomerId IN (:cidList) AND o.Action LIKE :actionWhere ORDER BY o.OperaterId DESC")
                        .SetParameterList("cidList", customerIdList)
                        .SetString("actionWhere", string.Format("%{0}%", actionWhere))
                        .SetFirstResult(startCount)
                        .SetMaxResults(requestCount)
                        .List<OperaterLog>(); 
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 75", e, myLog);
            }

            return operaterLogFlag;

        }
        #endregion

        #region  服务于分页-日志查询方法 返回数据总条数 OperaterLogForPageCount
        /// <summary>
        /// 分页-日志查询
        /// </summary>
        /// <param name="customerIdList">用户Id集合,其他用法：可查出当前子用户的登陆信息</param>
        /// <param name="pageSize">信息条目</param>
        /// <param name="page">信息页数</param>
        /// <param name="actionWhere">动作条件</param>
        /// <returns></returns>
        public int OperaterLogForPageCount(List<int> customerIdList, string actionWhere)
        {
            object count = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<OperaterLog>())
                {
                    sessionFactory.Session.BeginTransaction();
                    count = sessionFactory.Session.GetISession().
                        CreateQuery(" SELECT COUNT(*) FROM OperaterLog AS o WHERE o.CustomerId IN (:cidList) AND o.Action LIKE :actionWhere ")
                        .SetParameterList("cidList", customerIdList)
                        .SetString("actionWhere", string.Format("%{0}%", actionWhere))
                        .UniqueResult();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 112", e, myLog);
            }
            return Convert.ToInt32(count);

        }
        #endregion

        #region 日志查询 以用户Id集合  SelectOperaterLogByCustomerIdList
        /// <summary>
        /// 日志查询 以用户Id集合
        /// </summary>
        /// <param name="customerIdList">用户Id集合,其他用法：可查出当前子用户的登陆信息</param>
        /// <param name="pageSize">信息条目</param>
        /// <param name="page">信息页数</param>
        /// <returns></returns>
        public IList<OperaterLog> SelectOperaterLogByCustomerIdList(List<int> customerIdList)
        {
            IList<OperaterLog> operaterLogFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<OperaterLog>())
                {
                    sessionFactory.Session.BeginTransaction();
                    operaterLogFlag = sessionFactory.Session.GetISession().
                        CreateQuery(" FROM OperaterLog AS o WHERE o.CustomerId IN (:cidList) ORDER BY o.OperaterId DESC")
                        .SetParameterList("cidList", customerIdList)
                        .List<OperaterLog>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 145", e, myLog);
            }

            return operaterLogFlag;
        }
        #endregion

        #region 自定义批量删除
        /// <summary>
        /// 自定义sql做删除语法
        /// </summary>
        /// <param name="device">customer.CustomerId</param>
        public void DeleteOperaterLogByCustomerId(Customer customer)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Channel>())
                {
                    sessionFactory.Session.BeginTransaction();
                    string mySqlStr = string.Empty;
                    mySqlStr = " DELETE FROM  operaterlog ";
                    mySqlStr += string.Format(" WHERE CustomerId =({0}) ", customer.CustomerId);
                    sessionFactory.Session.GetISession().CreateSQLQuery(mySqlStr).ExecuteUpdate();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 174", e, myLog);
            }

        }
        #endregion
    }
}
