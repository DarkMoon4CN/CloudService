using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model.Entities;
using Bsr.Core.Hibernate;
using Bsr.Cloud.Core;

namespace Bsr.Cloud.BLogic
{
    public class DeviceServer
    {
        #region  构参
        private static readonly object _object = new object();
        private static DeviceServer instance;
        private DeviceServer() { }
        public static DeviceServer GetInstance()
        {
            if (instance == null)
            {
                lock (_object)
                {
                    if (instance == null)
                    {
                        instance = new DeviceServer();
                    }
                }
            }
            return instance;
        }
        #endregion
        INHFactory nhFactory = NHFactory.Instance;
        static private ILogger myLog = new Logger<DeviceServer>();

        #region  按用户查询库中设备
        /// <summary>
        /// 按用户查询库中设备
        /// </summary>
        /// <param name="customer">customer.CustomerId</param>
        /// <returns></returns>
        public IList<Device> SelectDeviceCustomerId(Customer customer)
        {
            IList<Device> DeviceFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Device>())
                {
                    sessionFactory.Session.BeginTransaction();
                    DeviceFlag = sessionFactory.Session.GetISession().
                        CreateQuery(" FROM Device AS d WHERE d.CustomerId=?")
                        .SetInt32(0, customer.CustomerId).List<Device>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 47", e, myLog);
            }

            return DeviceFlag;

        }
        #endregion

        #region 添加设备信息
        public int InsertDevice(Device device)
        {
            int deviceId = 0;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Device>())
                {
                    sessionFactory.Session.BeginTransaction();
                    deviceId = (int)sessionFactory.Save(device);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 70", e, myLog);
            }
            return deviceId;
        }
        #endregion

        #region  按设备Id查询库中设备
        /// <summary>
        /// 按设备Id查询库中设备
        /// </summary>
        /// <param name="device">device.DeviceId</param>
        /// <returns></returns>
        public IList<Device> SelectDeviceByDeviceId(Device device)
        {
            IList<Device> deviceFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Device>())
                {
                    sessionFactory.Session.BeginTransaction();
                    deviceFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Device AS d WHERE d.DeviceId=?")
                        .SetInt32(0, device.DeviceId).List<Device>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 99", e, myLog);
            }

            return deviceFlag;

        }
        #endregion

        #region  删除单条设备信息
        public void DeleteDeviceByDeviceId(Device device)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Device>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Delete(device);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 120", e, myLog);
            }
        }
        #endregion 删除单条设备信息

        #region 更新通道
        public void UpdateDevice(Device device)
        {
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Device>())
                {
                    sessionFactory.Session.BeginTransaction();
                    sessionFactory.Update(device);
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 140", e, myLog);
            }
        }
        #endregion 更新通道

        #region  按设备码查询库中设备
        public IList<Device> SelectDeviceSerialNumber(Device device)
        {
            IList<Device> DeviceFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Device>())
                {
                    sessionFactory.Session.BeginTransaction();
                    DeviceFlag = sessionFactory.Session.GetISession().
                        CreateQuery(" FROM Device AS d WHERE d.SerialNumber=?")
                        .SetString(0, device.SerialNumber).List<Device>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 160", e, myLog);
            }

            return DeviceFlag;

        }
        #endregion

        #region 模糊查询按 设备名称和设备SN码
        public IList<Device> SelectDeviceSerialNumber(string keyWord,List<int> deviceIdList)
        {
            IList<Device> DeviceFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Device>())
                {
                    sessionFactory.Session.BeginTransaction();
                    DeviceFlag = sessionFactory.Session.GetISession().
                        CreateQuery(" FROM Device AS d WHERE d.DeviceId IN (:dList) AND (d.DeviceName LIKE :nameKey OR d.SerialNumber LIKE :SNKey)")
                         .SetParameterList("dList", deviceIdList)
                         .SetParameter("nameKey", string.Format("%{0}%", keyWord))
                         .SetParameter("SNKey", string.Format("%{0}%", keyWord))
                         .List<Device>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 185", e, myLog);
            }

            return DeviceFlag;
        }
        #endregion


        #region  按设备BPServerDeviceId查询库中设备
        /// <summary>
        /// 按设备BPServerDeviceId查询库中设备
        /// </summary>
        /// <param name="device">device.BPServerDeviceId</param>
        /// <returns></returns>
        public IList<Device> SelectDeviceByBPServerDeviceId(Device device)
        {
            IList<Device> deviceFlag = null;
            try
            {
                using (var sessionFactory = nhFactory.GetRepositoryFor<Device>())
                {
                    sessionFactory.Session.BeginTransaction();
                    deviceFlag = sessionFactory.Session.GetISession()
                        .CreateQuery(" FROM Device AS d WHERE d.BPServerDeviceId=?")
                        .SetInt32(0, device.BPServerDeviceId).List<Device>();
                    sessionFactory.Session.CommitChanges();
                }
            }
            catch (BPCloudException e)
            {
                throw new BPCloudException("error line 218", e, myLog);
            }
            return deviceFlag;
        }
        #endregion
    }
}

