using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model.Entities;
using Bsr.Cloud.Model;
using Bsr.Cloud.Core;

namespace Bsr.Cloud.BLogic
{
    public class DeviceCache
    {
        #region  构造
        private DeviceCache()
        {
        }
        MemCache mc = new MemCache("Device");
        private static readonly object _object = new object();
        private static DeviceCache instance;
        public static DeviceCache GetInstance()
        {
            if (instance == null)
            {
                lock (_object)
                {
                    if (instance == null)
                    {
                        instance = new DeviceCache();
                    }
                }
            }
            return instance;
        }
         #endregion

        /// <summary>
        /// 向缓存中登记一个新的设备状态信息
        /// </summary>
        /// <param name="property">设备集合状态</param>
        /// <param name="EndTime">过期时限</param>
        /// <returns>返回0为成功，其它为错误值</returns>
        public int AddDeviceCache(string deviceKey, DeviceResponse deviceResponse, DateTime EndTime)
        {
            bool bFlag = mc.AddObject(deviceKey, deviceResponse, EndTime);
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
        /// 检测某DeviceKey是否有效。
        /// </summary>
        /// <param name="tokenKey">该deviceKey的值</param>
        /// <returns>不存在返回false,找到返回true</returns>
        public bool IsValid(string deviceKey)
        {
            return mc.ExistObject(deviceKey);
        }

        /// <summary>
        /// 查找DeviceKey下的对应 device 对象
        /// </summary>
        /// <param name="tokenKey">DeviceKey</param>
        /// <returns></returns>
        public DeviceResponse FindByDeviceKey(string deviceKey)
        {
            try
            {
                return mc.GetObject(deviceKey) as DeviceResponse;
            }
            catch
            {
                return null;
            }

        }
    }
}
