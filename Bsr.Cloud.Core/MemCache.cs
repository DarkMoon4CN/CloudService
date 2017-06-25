using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Memcached.ClientLibrary;
using System.Collections;

namespace Bsr.Cloud.Core
{
    /// <summary>
    /// 对Memcached客户端的一个简易包装
    /// </summary>
    public class MemCache
    {
        MemcachedClient mc = new MemcachedClient();//初始化一个客户端
        public string[] serverlist = { "192.168.8.207:11211" }; //服务器列表，可多个 
        /// <summary>
        /// 构造一个memcache缓存对象
        /// </summary>
        /// <param name="poolName">poolName名称，对于不同的poolName，数据是隔离的</param>
        public MemCache(string poolName)
        {
            //mc.PoolName= poolName ?? "default instance";
            mc.PoolName = (poolName == null || poolName == "") ? string.Format("default instance") : poolName;
                   
            SockIOPool pool = SockIOPool.GetInstance(mc.PoolName);
            //根据实际情况修改下面参数
            pool.SetServers(serverlist);
            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 10;
            pool.SocketConnectTimeout = 2000;
            pool.SocketTimeout = 2000;
            pool.MaintenanceSleep = 30;
            pool.Failover = true;
            pool.Nagle = false;
            pool.Initialize(); // initialize the pool for memcache servers           
        }
        
        public bool AddObject(string key,object value, DateTime dt)
        {
            return mc.Add(key, value,dt);
        }

        public bool RemoveObject(string key)
        {
            return mc.Delete(key);
        }

        public object GetObject(string key)
        {
            return mc.Get(key);
        }

        public bool UpdateObject(string key, object value, DateTime dt)
        {
            return mc.Replace(key, value,dt);
        }

        public bool ExistObject(string key)
        {
            return mc.KeyExists(key);
        }

        public bool ClearALL() 
        {
            // 该命令使所有数据项立即无效。
            return mc.FlushAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName">customer_,device_ 等</param>
        /// <returns>Hashtable 数据对象,根据tagName</returns>
        public Hashtable GetStats(string tagName) 
        {
            ArrayList arrList=new ArrayList();
            for (int i = 0; i < serverlist.Length; i++)
			{
			  arrList.Add(serverlist[i]);
			}
            Hashtable tmpDataHashTable = new Hashtable();
            Hashtable resultHashtable = new Hashtable();
            Hashtable allHashtable = mc.Stats(arrList, "items");
            foreach (string key in allHashtable.Keys)
            {
                Hashtable dataHashTable = (Hashtable)allHashtable[key];
                foreach (string tmpKey in dataHashTable.Keys)
                {
                    string[] splitKey = tmpKey.Split(':');
                    string tmpValue = (string)dataHashTable[tmpKey];
                    if (splitKey[2] == "number")
                    {
                        tmpDataHashTable.Add(splitKey[1], tmpValue);
                    }
                }
            }
            foreach (string allKey in tmpDataHashTable.Keys)
            {
                //key memCache结果集标记key,tmpDataHashTable[key]条数
                Hashtable hashtable = mc.Stats(arrList, "cachedump " + allKey + " " + tmpDataHashTable[allKey]);
                foreach (string tmpKey in hashtable.Keys)
                {
                    Hashtable result = (Hashtable)hashtable[tmpKey];
                    foreach (string resultKey in result.Keys)
                    {
                        if (resultKey.IndexOf(tagName) != -1)
                        {
                            resultHashtable.Add(resultKey, result[resultKey]);
                        }
                    }
                }
            }
            return resultHashtable;
        }
       
    }
}
