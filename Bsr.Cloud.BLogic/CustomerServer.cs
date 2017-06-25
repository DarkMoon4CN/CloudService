using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate;
using Bsr.Cloud.Model;
using Bsr.Core.Hibernate;
using Bsr.Cloud.Model.Entities;
using Bsr.Cloud.Core;

namespace Bsr.Cloud.BLogic
{
    public class CustomerServer
    {


        #region  构参
        private static readonly object _object = new object();
        private static CustomerServer instance;
        private CustomerServer()
        {

        }
        public static CustomerServer GetInstance()
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
                        instance = new CustomerServer();
                    }
                }
            }
            return instance;
        }
        #endregion  构参
        INHFactory nhFactory = NHFactory.Instance;
        static private ILogger myLog = new Logger<ChannelServer>();
        #region  查询客户表(CustomerName)
        /// <summary>
        /// 查询客户表中的CustomerName是否存在
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList<Customer> SelectCustomerByCustomerName(Customer customer)
        {
            IList<Customer> customerFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Customer>())
                {
                    sessionFactory.Session.BeginTransaction();
                    customerFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Customer AS c WHERE c.CustomerName=?")
                        .SetString(0, customer.CustomerName).List<Customer>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 57", e, myLog);
            }
            return customerFlag;
        }
        #endregion  查询客户表

        #region  更新客户表
        /// <summary>
        ///  更新Customer
        /// </summary>
        /// <param name="custmer"></param>
        public void UpdateCustomer(Customer customer)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Customer>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Update(customer);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 85", e, myLog);
            }
        }
        #endregion  更新客户表

        #region  添加单个客户
        /// <summary>
        /// 添加单个用户
        /// </summary>
        /// <param name="customer"> 用户实体对象</param>
        /// <returns></returns>
        public int InsertCustomer(Customer customer)
        {
            int CustomerId = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Customer>())
                {
                    sessionFactory.Session.BeginTransaction();
                    CustomerId = (int)sessionFactory.Save(customer);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 107", e, myLog);
            }
            return CustomerId;
        }
        #endregion 添加单个客户

        #region  查询客户表(CustomerId)
        /// <summary>
        /// 查询客户表中的CustomerId是否存在
        /// </summary>
        /// <param name="customer">customer.CustomerId</param>
        /// <returns></returns>
        public IList<Customer> SelectCustomerByCustomerId(Customer customer)
        {
            IList<Customer> customerFlag = null;
            try
            {
                using (var cust = nhFactory.GetRepositoryFor<Customer>())
                {
                    cust.Session.BeginTransaction();
                    customerFlag = cust.Session.GetISession().CreateQuery(" FROM Customer AS c WHERE c.CustomerId=?").SetInt32(0, customer.CustomerId).List<Customer>();
                    cust.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 134", e, myLog);
            }

            return customerFlag;
        }
        #endregion  

        #region  查询客户表(ReceiverPhone)
        /// <summary>
        /// 查询客户表中的ReceiverPhone是否存在
        /// </summary>
        /// <param name="customer">Customer.ReceiverCellPhone</param>
        /// <returns></returns>
        public IList<Customer> SelectCustomerByReceiverPhone(Customer customer)
        {
            IList<Customer> customerFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Customer>())
                {
                    sessionFactory.Session.BeginTransaction();
                    customerFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Customer AS c WHERE c.ReceiverCellPhone=?")
                        .SetString(0, customer.ReceiverCellPhone).List<Customer>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 161", e, myLog);
            }

            return customerFlag;
        }
        #endregion  查询客户

        #region  查询客户表(ReceiverEmail)
        /// <summary>
        /// 查询客户表中的ReceiverEmail是否存在
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList<Customer> SelectCustomerByReceiverEmail(Customer customer)
        {
            IList<Customer> customerFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Customer>())
                {
                    sessionFactory.Session.BeginTransaction();
                    customerFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Customer AS c WHERE c.ReceiverEmail=?")
                        .SetString(0, customer.ReceiverEmail).List<Customer>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 190", e, myLog);
            }

            return customerFlag;
        }
        #endregion  查询客户

        #region  删除单个用户(CustomerId)
        public void DeleteCustomer(Customer customer)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Customer>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Delete(customer);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 213", e, myLog);
            }
        }
        #endregion 删除单个用户

        #region  查询客户表(ParentId)
        /// <summary>
        /// 查询客户表中的ParentId是否存在
        /// </summary>
        /// <param name="customer">customer.ParentId</param>
        /// <returns></returns>
        public IList<Customer> SelectCustomerIdByParentId(Customer customer)
        {
            IList<Customer> customerFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Customer>())
                {
                    sessionFactory.Session.BeginTransaction();
                    customerFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Customer AS c WHERE c.CustomerId=?")
                        .SetInt32(0, customer.ParentId).List<Customer>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 238", e, myLog);
            }

            return customerFlag;
        }
        #endregion  查询客户表(CustomerID)

        #region 模糊查询 以ParentId模糊查找CustomerName,ReceiverEmail,ReceiverCellPhone
        /// <summary>
        /// 模糊查询 以ParentId模糊查找下一级用户
        /// </summary>
        /// <param name="customer">customer.ParentId</param>
        /// <returns></returns>
        public IList<Customer> SearchCustomerByParentId(Customer customer, string KeyWord)
        {
            IList<Customer> customerFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Customer>())
                {
                    sessionFactory.Session.BeginTransaction();
                    customerFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Customer AS c WHERE c.ParentId=? AND(c.CustomerName LIKE :nameKey OR c.ReceiverEmail LIKE :emailKey OR c.ReceiverCellPhone LIKE :cellPhoneKey) ORDER BY c.CustomerId DESC")
                        .SetInt32(0, customer.CustomerId)
                        .SetParameter("nameKey", string.Format("%{0}%", KeyWord))
                        .SetParameter("emailKey", string.Format("%{0}%", KeyWord))
                        .SetParameter("cellPhoneKey", string.Format("%{0}%", KeyWord)).List<Customer>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 266", e, myLog);
            }

            return customerFlag;
        }
        #endregion


        #region 以ParentId查找下一级用户
        /// <summary>
        /// 以ParentId查找下一级用户
        /// </summary>
        /// <param name="customer">customer.ParentId</param>
        /// <returns></returns>
        public IList<Customer> SelectCustomerByParentId(Customer customer)
        {
            IList<Customer> customerFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Customer>())
                {
                    sessionFactory.Session.BeginTransaction();
                    customerFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Customer AS c WHERE c.ParentId=? ")
                        .SetInt32(0, customer.CustomerId).List<Customer>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 300", e, myLog);
            }
            return customerFlag;
        }
        #endregion
    }
}
