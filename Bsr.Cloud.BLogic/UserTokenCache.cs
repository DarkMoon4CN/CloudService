using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model;
using Bsr.Cloud.Core;
using System.Collections;

namespace Bsr.Cloud.BLogic
{
    /// <summary>
    /// 需存入Token缓存中的用户属性，
    /// 该类的每一个对象代表一个在线用户
    /// </summary>
    [Serializable]
    public class TokenCacheProperty
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime LastAccessTime { get; set; }
        public int LoginType { get; set; } // 用户登录的终端,见LoginTypeEnum
        public string AgentVersion { get; set; } // 登录终端软件的版本 Web端：浏览器类型， 手机端：手机版本
        public int IsEnable { get; set; }
        public int SignInType { get; set; } // 用户的身份,见CustomerSignInTypeEnum
        public int ParentId { get; set; }//ta的上级用户
        public string remoteEndpointAddress { get; set; } // 客户端来自哪个IP地址
    };

    public class UserTokenCache
    {

        #region construct
        private UserTokenCache()
        {
        }
        #endregion

        public static UserTokenCache GetInstance()
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
                        instance = new UserTokenCache();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 向缓存中登记一个新的Token,将CustomerId作为唯一key
        /// </summary>
        /// <param name="property">对应的用户属性</param>
        /// <param name="EndTime">过期时限</param>
        /// <returns>返回0为成功，其它为错误值</returns>
        public int AddToken(string tokenKey, TokenCacheProperty property, DateTime EndTime)
        {
            bool bFlag = mc.AddObject(tokenKey, property, EndTime);
            if (bFlag)
            {
                return 0;
            }
            else
            {
                return (int)CodeEnum.ApplicationErr;
            }
        }

        /// <summary>
        /// 移走一个指定的Token
        /// </summary>
        /// <param name="tokenKey">该Token的值</param>
        /// <returns>返回0为成功，其它为错误值</returns>
        public int RemoveToken(string tokenKey)
        {
            bool bFlag = mc.RemoveObject(tokenKey);
            if (bFlag)
            {
                return 0;
            }
            else
            {
                return (int)CodeEnum.ApplicationErr;
            }
        }

        /// <summary>
        /// 移走所有Token
        /// </summary>
        /// <returns>返回0为成功，其它为错误值</returns>
        public int RemoveAllToken()
        {
            bool bFlag = mc.ClearALL();
            if (bFlag)
            {
                return 0;
            }
            else
            {
                return (int)CodeEnum.ApplicationErr;
            }
        }


        /// <summary>
        /// 检测某Token是否有效。
        /// </summary>
        /// <param name="tokenKey">该Token的值</param>
        /// <returns>不存在返回false,找到返回true</returns>
        public bool IsValid(string tokenKey)
        {
            return mc.ExistObject(tokenKey);
        }


        /// <summary>
        ///  以Token 更新 TokenCacheProperty 下的最后访问时间
        ///  注：这里会同时更新该token的property，在性能上可能存在隐患。
        /// </summary>
        /// <param name="tokenKey">CustomerToken</param>
        /// <param name="property">对应的用户属性</param>
        /// <param name="EndTime">过期时限</param>/// 
        ///  <returns></returns>
        public int RefreshAccessTime(string tokenKey, TokenCacheProperty property, DateTime EndTime)
        {
            bool bFlag = mc.UpdateObject(tokenKey, property, EndTime);
            if (bFlag)
            {
                return 0;
            }
            else
            {
                return (int)CodeEnum.ApplicationErr;
            }
        } 


       /// <summary>
        /// 查找tokenKey下的对应 TokenCacheProperty 对象
       /// </summary>
        /// <param name="tokenKey">tokenKey</param>
       /// <returns></returns>
        public TokenCacheProperty FindByCustomerToken(string tokenKey)
        {
            try
            {
                return mc.GetObject(tokenKey) as TokenCacheProperty;
            }
            catch
            {
                return null;
            }
           
        }

        /// <summary>
        /// 返回当期在线所有用户
        /// </summary>
        public IList<TokenCacheProperty> FindAll()
        {
            IList<TokenCacheProperty> tokenCachePropertyFlag = new List<TokenCacheProperty>();
            try
            {
                //用于查找标记为 customer_ 的信息
                string customerTag = "customer_";
                Hashtable resultHashtable = mc.GetStats(customerTag);
                foreach (string key in resultHashtable.Keys)
                {
                    TokenCacheProperty tcp = FindByCustomerToken(key);
                    tokenCachePropertyFlag.Add(tcp);
                }
            }
            catch 
            {
                return null;
            }
            return tokenCachePropertyFlag;
        }
        #region 成员变量

        MemCache mc = new MemCache("CustomerToken");
        private static readonly object _object = new object();

        private static UserTokenCache instance;

        #endregion

    }

}
