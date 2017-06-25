using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model.Entities;
using Bsr.Core.Hibernate;
using Bsr.Cloud.Core;

namespace Bsr.Cloud.BLogic
{
    public class BPServerConfigServer
    {

        #region  构参
        private static readonly object _object = new object();
        private static BPServerConfigServer instance;
        private BPServerConfigServer()
        {

        }
        public static BPServerConfigServer GetInstance()
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
                        instance = new BPServerConfigServer();
                    }
                }
            }
            return instance;
        }
        #endregion  构参
        INHFactory nhFactory = NHFactory.Instance;
         static private ILogger myLog = new Logger<BPServerConfigServer>();
        #region 查询本地配置的需要的服务器位置
         /// <summary>
         ///  查询本地配置的需要的服务器位置 GetBPServerConfigById
        /// </summary>
        /// <param name="serverConfig"> BPServerConfig 实体</param>
        /// <returns></returns>
        public  IList<BPServerConfig>  GetBPServerConfigByKey(BPServerConfig serverConfig)
        {
            IList<BPServerConfig> serverConfigFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<BPServerConfig>())
                {
                    sessionFactory.Session.BeginTransaction();
                    serverConfigFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM BPServerConfig AS s WHERE s.BPServerConfigId=? ")
                        .SetInt32(0, serverConfig.BPServerConfigId).List<BPServerConfig>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 47",e,myLog);
            }
            return serverConfigFlag;
        }
        #endregion

        #region  添加本地配置(BPServerConfig)
        /// <summary>
        ///  需要配置的服务信息
        /// </summary>
        /// <param name="serverConfig">ServerConfig 实体</param>
        public void  InsertBPServerConfig(BPServerConfig serverConfig)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<BPServerConfig>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Save(serverConfig);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 78", e, myLog);
            }
        }
        #endregion 添加本地配置


        #region 查询本地配置的Domain
        /// <summary>
        ///  查询本地配置的Domain 
        /// </summary>
        /// <param name="serverConfig"> BPServerConfig 实体</param>
        /// <returns></returns>
        public IList<BPServerConfig> GetBPServerConfigByDomain(BPServerConfig serverConfig)
        {
            IList<BPServerConfig> serverConfigFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<BPServerConfig>())
                {
                    sessionFactory.Session.BeginTransaction();
                    serverConfigFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM BPServerConfig AS s WHERE s.Domain=? ")
                        .SetString(0, serverConfig.Domain).List<BPServerConfig>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 105", e, myLog);
            }
            return serverConfigFlag;
        }
        #endregion
    }
}
