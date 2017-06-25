using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Bsr.Services.Contract;
using Bsr.Domain.Dto;
using Bsr.Domain.Models;
using Bsr.Domain.Entities;
using Bsr.DeviceAdapter.Model;
using Bsr.Cloud.Core;
using Bsr.Cloud.Model;
using Bsr.Cloud.Model.Entities;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace Bsr.Cloud.BLogic.BLL
{
    public class DeviceBLL
    {

        //DeviceServer ChannelServer  Log4Net
        DeviceServer deviceServer = DeviceServer.GetInstance();
        ChannelServer channelServer = ChannelServer.GetInstance();
        PermissionServer permissionServer = PermissionServer.GetInstance();
        BPServerConfigServer bpServerConfigServer = BPServerConfigServer.GetInstance();
        GroupChannelServer groupChannelServer = GroupChannelServer.GetInstance();
        UserFavoriteServer userFavoriteServer = UserFavoriteServer.GetInstance();
        static private ILogger myLog = new Logger<DeviceBLL>();
        public BLLHelper bllHelper = new BLLHelper();
        DeviceCache deviceCache = DeviceCache.GetInstance();

        #region  1添加设备 2初次添加设备通道
        /// <summary>
        ///  1添加设备 2初次添加设备通道
        /// </summary>
        /// <returns>ResponseBaseDto</returns>
        public ResponseBaseDto AddDevice(string deviceName, string serialNumber, string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            TokenCacheProperty tcp = new TokenCacheProperty();
            UserTokenCache utc = UserTokenCache.GetInstance();
            OperaterLog oLog = new OperaterLog();
            oLog.Action = " 添加设备 ";
            try
            {
                dto = bllHelper.CheckCustomer(dto, customerToken,ref tcp);
                //获取 判定token
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "Token已失效";
                }
                else
                {

                    Device device = new Device();
                    device.SerialNumber = serialNumber;
                    IList<Device> deviceFlag = deviceServer.SelectDeviceSerialNumber(device);

                    if (device != null && deviceFlag.Count != 0)
                    {
                        //设备被添加过
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "此设备正在被使用中";
                        oLog.Result = dto.Code;
                        oLog.Remarks = dto.Message;
                        bllHelper.AddOperaterLog(oLog, tcp);
                        return dto;
                    }
                    //参数 0=IP  3等于 CN
                    AddDeviceResponseDto result = AddDeviceMethod(serialNumber);
                    if (result != null && result.Success == true)
                    {
                        //添加本地设备列表
                        Bsr.Cloud.Model.Entities.Device dev = new Model.Entities.Device();
                        dev.SerialNumber = result.DeviceDto.AddDeviceEntity.SerialNumber;
                        dev.BPServerDeviceId = result.DeviceDto.AddDeviceEntity.Id;
                        if (deviceName == null || deviceName == "") 
                        {
                            deviceName = "BStar";
                        }
                        dev.DeviceName = deviceName;
                        // 序列号添加时填3,需用IDeviceEntity.getDeviceType获取真正实际类型
                        dev.DeviceType = result.DeviceDto.AddDeviceEntity.AddMode;
                        dev.HardwareType = result.DeviceDto.AddDeviceEntity.HardwareType;
                        dev.CustomerId = tcp.CustomerId;
                        dev.UserName = result.DeviceDto.AddDeviceEntity.User;
                        dev.PassWord = result.DeviceDto.AddDeviceEntity.Password;
                        //以规则需要定义服务器位置 
                         BPServerConfig bpServerConfig=new BPServerConfig();
                         bpServerConfig.Domain="default";
                         IList<BPServerConfig> bpServerConfigFlag = bpServerConfigServer.GetBPServerConfigByDomain(bpServerConfig);
                         if (bpServerConfigFlag != null && bpServerConfigFlag.Count != 0)
                         {
                             dev.BPServerConfigId = bpServerConfigFlag[0].BPServerConfigId;
                         }
                         else
                         {
                             dev.BPServerConfigId = 1;//需修改
                         }
                         int deviceId = deviceServer.InsertDevice(dev);

                        //日志动作的目标对象Id
                        oLog.TargetId = deviceId;
                        //通道添加
                        IList<CameraEntity> channelFlag = result.DeviceDto.AddDeviceEntity.Cameras;
                        for (int i = 0; i < channelFlag.Count; i++)
                        {
                            Bsr.Cloud.Model.Entities.Channel channel = new Model.Entities.Channel();
                            CameraEntity ce = channelFlag[i];
                            channel.DeviceId = deviceId;
                            channel.ChannelNumber = ce.ChannelNo;
                            channel.ChannelName = ce.Name;
                            channel.IsEnable = 1;
                            channel.BPServerChannelId = ce.Id;
                            channel.ImagePath = "default.jpg";
                            channelServer.InsertChannel(channel);
                        }
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "设备" + deviceName + "已添加完成！";
                    }
                    else
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message =result.Message;
                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，添加设备时，远程连接超时！";
                myLog.ErrorFormat("AddDevice方法异常，设备名：{0}, 设备序列号：{1}", ex, deviceName, serialNumber);
            }
            oLog.TargetType = (int)OperaterLogEnum.Device;
            oLog.Result = dto.Code;
            oLog.Remarks = dto.Message;
            bllHelper.AddOperaterLog(oLog,tcp);
            return dto;
        }
        #endregion

        #region 删除设备
        /// <summary>
        ///  删除个设备（慎用）
        /// </summary>
        /// <param name="customer">customerId</param>
        /// <param name="DeviceId">设备本地数据库Id</param>
        /// <param name="customerToken"></param>
        /// <returns></returns>
        public ResponseBaseDto DeleteDeviceByDeviceId(Device device, string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            OperaterLog oLog = new OperaterLog();
            TokenCacheProperty tcp = new TokenCacheProperty();
            oLog.Action = "删除设备";
            try
            {
                //获取 判定token
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "Token已失效 ";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    oLog.TargetType = (int)OperaterLogEnum.Device;
                    oLog.Result = dto.Code;
                    oLog.Remarks = dto.Message;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else
                {
                    //查询本地的deviceId
                    IList<Device> deviceFlag= deviceServer.SelectDeviceByDeviceId(device);
                    if (deviceFlag == null || deviceFlag.Count <= 0) 
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "删除时没有检索到需要的设备";
                    }
                    else if (deviceFlag[0].CustomerId != tcp.CustomerId)
                    {
                        dto.Code = (int)CodeEnum.NoAuthorization;
                        dto.Message = "删除时用户没有拥有对此设备权限";
                    }
                    else 
                    {
                        oLog.TargetId = deviceFlag[0].DeviceId;
                        bool bFlag = ClearDevice(deviceFlag[0]);
                        if (bFlag)
                        {
                            dto.Code = (int)CodeEnum.Success;
                            dto.Message = "用户删除设备 " + deviceFlag[0].DeviceName + " 完成！";
                        }
                        else //if(bFlag==false)
                        {
                            dto.Code = (int)CodeEnum.ServerNoToken;
                            dto.Message = "删除设备设备失败！";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后操作";
                myLog.ErrorFormat("DeleteDeviceByDeviceId方法异常, 设备id:{0}", ex, device.DeviceId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Device;
            oLog.Result = dto.Code;
            oLog.Remarks = dto.Message;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 提供添加和删除的服务方法 yjm
        /// <summary>
        ///  添加设备
        /// </summary>
        /// <param name="serialNumber">设备号</param>
        /// <returns></returns>
        private AddDeviceResponseDto AddDeviceMethod(string serialNumber) 
        {
            var deviceTypeEntity = GetDeviceTypeEntity(WellKnownVenderName.BSR, WellKnownSdkNames.LimitDevice);

            DockDeviceEntity deviceEntity = new DockDeviceEntity();
            deviceEntity.DeviceType = deviceTypeEntity;
            deviceEntity.ChannelCount = 0;
            deviceEntity.ControlPort = 3721;
            deviceEntity.DataPort = 3720;
            deviceEntity.DefaultConn = 0;
            deviceEntity.DownloadPort = 3720;
            deviceEntity.Password = "123456";
            deviceEntity.Protocol = 0;
            deviceEntity.User = "admin";
            deviceEntity.HardwareType = (short)HardwareType.Unknown;
            deviceEntity.IsFetchChannel = true;
            deviceEntity.MulticastAddress = "";
            deviceEntity.MulticastPort = 0;

       
            if (serialNumber.IndexOf(".")!=-1)
            {
                deviceEntity.Ip = serialNumber;
                deviceEntity.AddMode = 0;
                deviceEntity.Host = "";
            }
            else
            {
                deviceEntity.Ip = "";
                deviceEntity.AddMode = 3;
                deviceEntity.Host = serialNumber;
            }
            deviceEntity.Host = serialNumber;
            deviceEntity.Name = serialNumber;

            deviceEntity.RegisterServerId = deviceEntity.RegisterType = 0;
            deviceEntity.RegisterAliases = deviceEntity.RegisterSerialNumber = deviceEntity.RegisterMACAddress = "";

            AddDeviceRequestDto addDto = new AddDeviceRequestDto();
            addDto.MaxCount = 512;
            addDto.AddDeviceEntity = deviceEntity;


            Bsr.ServiceProxy.Utils.ServiceFactory serviceFactory = new ServiceProxy.Utils.ServiceFactory();
            DDNSConfigDto tmp = new DDNSConfigDto();
            var seviceAddr =bllHelper.GetServerModelStr();
            AddDeviceResponseDto result=null;
            serviceFactory.GetProxy<IDevice>(new Uri(seviceAddr)).Use(
                p =>
                {
                    result = p.Add(addDto, tmp);
                }
                );
            return result;            
        }
        /// <summary>
        /// 删除BP4Server服务器的设备
        /// </summary>
        /// <param name="deleteMode"></param>
        /// <param name="ip"></param>
        /// <param name="host"></param>
        public  bool DeleteDeviceMethod(int serverDeviceId) 
        {
            bool bFlag = false;
            Bsr.ServiceProxy.Utils.ServiceFactory serviceFactory = new ServiceProxy.Utils.ServiceFactory();
            var seviceAddr = bllHelper.GetServerModelStr();
            serviceFactory.GetProxy<IDevice>(new Uri(seviceAddr)).Use(
                p =>
                {
                  bFlag= p.Delete(serverDeviceId);
                }
                );
            return bFlag;
        }
        /// <summary>
        ///  BP4Server修改设备名
        /// </summary>
        /// <param name="deleteMode"></param>
        /// <param name="ip"></param>
        /// <param name="host"></param>
        public void UpdateDeviceNameMethod(int serverDeviceId, string serverDeviceName)
        {
            Bsr.ServiceProxy.Utils.ServiceFactory serviceFactory = new ServiceProxy.Utils.ServiceFactory();
            var seviceAddr = bllHelper.GetServerModelStr();
            serviceFactory.GetProxy<IDevice>(new Uri(seviceAddr)).Use(
                p =>
                {
                    p.UpdateName(serverDeviceId, serverDeviceName);
                }
                );
        }
        /// <summary>
        /// 通过设备产商名和类型，向bp4server获取到DeviceTypeEntity，这个参数用在添加设备到bp4server时使用
        /// </summary>
        /// <param name="venderName"></param>
        /// <param name="deviceKey"></param>
        /// <returns></returns>
        private  DeviceTypeEntity GetDeviceTypeEntity(string venderName, string deviceKey)
        {
            Bsr.ServiceProxy.Utils.ServiceFactory serviceFactory = new ServiceProxy.Utils.ServiceFactory();
            var seviceAddr = bllHelper.GetServerModelStr();
            IList<DeviceTypeEntity> typeList=null;
            serviceFactory.GetProxy<IDeviceType>(new Uri(seviceAddr)).Use(
                c=>
                {
                    DeviceTypeCondDto typeDto = new DeviceTypeCondDto(null, null, (int)DeviceTypeLevel.Vender, null);
                    var venderList = c.GetDeviceTypeListFor(typeDto, int.MaxValue, 1);
                    int parentId = venderList.Where(p => p.Name == venderName).First().Id;

                    typeDto = new DeviceTypeCondDto(null, null, (int)DeviceTypeLevel.Model, parentId);
                    typeList = c.GetDeviceTypeListFor(typeDto, int.MaxValue, 1);
                }
                );
            return typeList.Where(p => p.Key == deviceKey).First();
        }

        //设置binding
        internal  Binding GetBinding(Uri baseAddress, string bindingConfigurationName)
        {
            if (baseAddress == null)
            {
                //throw new ArgumentNullException("baseAddress", string.Format("服务基地址不能为null，请在AppSettings配置{0}默认地址，或检查Uri输入参数有效。", _defaultBaseAddrKey));
            }
            Binding binding = null;
            string scheme = baseAddress.Scheme.ToLower();
            switch (scheme)
            {
                case "net.tcp":
                    {
                        NetTcpBinding tcpBinding;
                        if (string.IsNullOrEmpty(bindingConfigurationName))
                        {
                            tcpBinding = new NetTcpBinding(SecurityMode.None);
                            int max = int.MaxValue - 1;
                            tcpBinding.MaxBufferSize = max;
                            tcpBinding.MaxBufferPoolSize = max;
                            tcpBinding.MaxReceivedMessageSize = max;
                            tcpBinding.ReaderQuotas.MaxNameTableCharCount = max;
                            tcpBinding.ReaderQuotas.MaxStringContentLength = max;
                            tcpBinding.ReaderQuotas.MaxDepth = max;
                            tcpBinding.ReaderQuotas.MaxBytesPerRead = max;
                            tcpBinding.ReaderQuotas.MaxArrayLength = max;
                            tcpBinding.ReceiveTimeout = timeout;
                            tcpBinding.SendTimeout = timeout;
                            tcpBinding.OpenTimeout = timeout;
                            tcpBinding.CloseTimeout = timeout;
                        }
                        else
                        {
                            tcpBinding = new NetTcpBinding(bindingConfigurationName);
                        }

                        binding = tcpBinding;
                        break;
                    }
                case "net.pipe":
                    {
                        NetNamedPipeBinding pipeBinding;
                        if (string.IsNullOrEmpty(bindingConfigurationName))
                        {
                            pipeBinding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                            int max = int.MaxValue - 1;
                            pipeBinding.MaxBufferSize = max;
                            pipeBinding.MaxBufferPoolSize = max;
                            pipeBinding.MaxReceivedMessageSize = max;


                            pipeBinding.ReaderQuotas.MaxNameTableCharCount = max;
                            pipeBinding.ReaderQuotas.MaxStringContentLength = max;
                            pipeBinding.ReaderQuotas.MaxDepth = max;
                            pipeBinding.ReaderQuotas.MaxBytesPerRead = max;
                            pipeBinding.ReaderQuotas.MaxArrayLength = max;
                           
                            pipeBinding.ReceiveTimeout = timeout;
                            pipeBinding.SendTimeout = timeout;
                            pipeBinding.OpenTimeout = timeout;
                            pipeBinding.CloseTimeout = timeout;
                        }
                        else
                        {
                            pipeBinding = new NetNamedPipeBinding(bindingConfigurationName);
                        }

                        binding = pipeBinding;
                        break;
                    }
                default:
                    break;
            }

            return binding;
        }
        //连接过期时间
        internal static readonly TimeSpan timeout = TimeSpan.FromSeconds(15);

        #endregion

        #region  以用户Id返回所有设备 GetDeviceByCustomerId
        /// <summary>
        ///  以用户Id返回所有设备
        /// </summary>
        /// <param name="customer">CustomerId</param>
        /// <param name="customerToken">token</param>
        /// <param name="deviceFlag">返回结果 设备</param>
        /// <returns></returns>
        public ResponseBaseDto GetDeviceByCustomerId(string customerToken, ref List<DeviceResponse> deviceResponseFlag) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            int customerId=0;
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.TokenTimeOut;
                    dto.Message = "用户token已失效，请重新登录后继续";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
                else if (tcp.SignInType != (int)CustomerSignInTypeEnum.PrimaryCustomer) 
                {
                    dto.Code = (int)CodeEnum.NoUser;
                    dto.Message = "没有权限";
                    return dto;
                }
                else
                {
                    Customer customer = new Customer();
                    customerId=tcp.CustomerId;
                    customer.CustomerId = tcp.CustomerId;
                    List<int> authorizeDeviceIdList=new  List<int>();
                    //当前主账号下的设备
                    IList<Device> deviceFlag = deviceServer.SelectDeviceCustomerId(customer);
                    //准备返回的结果集
                   deviceResponseFlag = new List<DeviceResponse>();
                    //添加授权的设备
                    IList<Permission> permissionFlag=permissionServer.SelectPermissionByCustomerId(customer);
                    for (int j = 0; j < permissionFlag.Count; j++)
                    {
                        //判定当前用户对授权设备是否是可用的
                        if (permissionFlag[j].IsEnable == 1 && permissionFlag[j].NodeType == 1) 
                        {
                            IList<Device> otherDeviceFalg=
                                deviceServer.SelectDeviceByDeviceId(new Device() { DeviceId = permissionFlag[j].NodeId });
                            if (otherDeviceFalg != null && otherDeviceFalg.Count == 1)
                            {
                                authorizeDeviceIdList.Add(otherDeviceFalg[0].DeviceId);
                                deviceFlag.Add(otherDeviceFalg[0]);
                            }
                        } 
                     }
                    Bsr.ServiceProxy.Utils.ServiceFactory serviceFactory = new ServiceProxy.Utils.ServiceFactory();
                    var seviceAddr = bllHelper.GetServerModelStr();
                    for (int i = 0; i < deviceFlag.Count; i++)
                    {
                        DeviceResponse dr = new DeviceResponse();
                        Bsr.Cloud.Model.Entities.Device device = deviceFlag[i];
                        dr.DeviceId = device.DeviceId;
                        dr.DeviceName = device.DeviceName;
                        dr.DeviceType = device.DeviceType;
                        dr.SerialNumber = device.SerialNumber;
                        dr.HardwareType =device.HardwareType;
                        dr.IsAuthorize = 0;
                        if (authorizeDeviceIdList.Contains(device.DeviceId))
                        {
                            dr.IsAuthorize =1;
                        }
                        deviceResponseFlag.Add(dr);
                    }
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "设备已获取完成";
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.ErrorFormat("GetDeviceByCustomerId方法异常, 用户Id:{0}",ex ,customerId);
            }
            return dto;
        }
        #endregion

        #region  以设备Id返回所有通道 GetChannelByDeviceId
        /// <summary>
        ///  以设备Id返回所有通道
        /// </summary>
        /// <param name="device">DeviceId</param>
        /// <param name="customerToken">token</param>
        /// <param name="deviceFlag">返回结果 设备</param>
        /// <returns></returns>
        public ResponseBaseDto GetChannelByDeviceId(Device device, string customerToken, ref IList<Channel> channeFlag)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.TokenTimeOut;
                    dto.Message = "用户token已失效，请重新登录后继续";
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
                else
                {
                    channeFlag = channelServer.SelectChannelByDeviceId(device);
                    dto.Code = 0;
                    dto.Message = "通道数据获取已完成";
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.ErrorFormat("GetChannelByDeviceId方法异常,设备Id:{0}", ex,device.DeviceId);
            }
            return dto;
        }
        #endregion

        #region 更新设备名称
        /// <summary>
        ///  更新设备名称 UpdateDeviceByDeviceName
        /// </summary>
        /// <param name="device"></param>
        /// <param name="customerToken"></param>
        /// <returns></returns>
        public ResponseBaseDto UpdateDeviceByDeviceName(Device device,string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            OperaterLog oLog = new OperaterLog();
            oLog.Action = "修改设备名";
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token已失效，请重新登录后继续";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    oLog.TargetType = (int)OperaterLogEnum.Device;
                    oLog.Result = dto.Code;
                    oLog.Remarks = dto.Message;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else
                {
                    IList<Device> deviceFlag = deviceServer.SelectDeviceByDeviceId(device);
                    oLog.TargetId = device.DeviceId;
                    if (deviceFlag == null ||deviceFlag.Count <=0)
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message =  device.DeviceName + "设备不存在";
                    }
                    else if (deviceFlag[0].CustomerId != tcp.CustomerId)
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "用户没有权限修改设备 "+device.DeviceName;
                    }
                    else
                    {
                        try 
                        {
                            //修改BP4Server的设备名
                            UpdateDeviceNameMethod(deviceFlag[0].BPServerDeviceId, deviceFlag[0].DeviceName);
                        }
                        catch (Exception ex)
                        {
                            dto.Code = (int)CodeEnum.ApplicationErr;
                            dto.Message = "设备已离线！离线状态无法修改设备名";
                            myLog.ErrorFormat("UpdateDeviceByDeviceName（BP4Server设备已离线）设备id：{0}，新设备名：{1}",
                                  ex, device.DeviceId, device.DeviceName);
                            return dto;
                        }
                        deviceFlag[0].DeviceName = device.DeviceName;
                        deviceServer.UpdateDevice(deviceFlag[0]);
                        dto.Code = 0;
                        dto.Message = "修改设备 "+device.DeviceName+" 完成！";
                    }
                }/*end if(utc.IsValid(customerToken) == false)*/
            }
            catch (Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，修改设备名时，远程连接超时！";
                myLog.ErrorFormat("UpdateDeviceByDeviceName方法异常, 设备id：{0}，新设备名：{1}", 
                                  ex, device.DeviceId, device.DeviceName);
            }
            oLog.TargetType = (int)OperaterLogEnum.Device;
            oLog.Result = dto.Code;
            oLog.Remarks = dto.Message;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 获取设备状态
        /// <summary>
        ///  获取设备状态
        /// </summary>
        /// <param name="deviceIdList">Device.DeviceId</param>
        /// <param name="deviceResponseFlag">ref 集合对象</param>
        /// <returns></returns>
        public ResponseBaseDto GetServerDeviceState(int[] deviceIdList, string customerToken, ref List<DeviceResponse> deviceResponseFlag)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token已失效";
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
                else
                {
                   IList<DeviceTokenDto>  deviceTokenDtoFlag=new List<DeviceTokenDto>();
                   for (int i = 0; i < deviceIdList.Length; i++)
                   {
                       //缓存里是否有这个设备
                       DeviceResponse deviceResponse = deviceCache.FindByDeviceKey(GetDeivceKey(deviceIdList[i]));
                       if (deviceResponse != null)
                       {
                           deviceResponseFlag.Add(deviceResponse);
                       }
                       else
                       {
                           DeviceTokenDto deviceTokenDto = new DeviceTokenDto();
                           //服务器的设备ID
                           IList<Device> dFlag = 
                               deviceServer.SelectDeviceByDeviceId(new Device() { DeviceId = deviceIdList[i] });
                           if (dFlag != null && dFlag.Count != 0)
                           {
                               deviceTokenDto.ID = dFlag[0].BPServerDeviceId;
                               deviceTokenDtoFlag.Add(deviceTokenDto);
                           }
                       }
                   }
                   try 
                   {
                       Bsr.ServiceProxy.Utils.ServiceFactory serviceFactory = new ServiceProxy.Utils.ServiceFactory();
                       string seviceAddr = bllHelper.GetServerModelStr();
                       IDictionary<int, DeviceStateDto> deviceState=null;
                       serviceFactory.GetProxy<IDevice>(new Uri(seviceAddr)).Use(
                           p =>
                           {
                              deviceState= p.RefreshDevices(deviceTokenDtoFlag);
                           }
                           );
                       for (int i = 0; i < deviceTokenDtoFlag.Count; i++)
                       {
                           DeviceStateDto dsDto = deviceState[(deviceTokenDtoFlag[i].ID)];
                           IList<Device> dbDeviceFlag =
                               deviceServer.SelectDeviceByBPServerDeviceId(new Device() { BPServerDeviceId = deviceTokenDtoFlag[i].ID });
                           if (dbDeviceFlag != null && dbDeviceFlag.Count == 1)
                           {
                               Device device = dbDeviceFlag[i];
                               DeviceResponse deviceResponse = new DeviceResponse();
                               deviceResponse.DeviceId = device.DeviceId;
                               if (dsDto.IsOnline == true)
                               {
                                   deviceResponse.BPServerDeviceState = 1;
                               }
                               else 
                               {
                                   deviceResponse.BPServerDeviceState =0;
                               }
                               //数据放入缓存
                               DateTime dt = DateTime.Now.AddSeconds(300);//失效时间定义为5分钟
                               deviceCache.AddDeviceCache(GetDeivceKey(dbDeviceFlag[0].DeviceId), deviceResponse, dt);
                               deviceResponseFlag.Add(deviceResponse);
                           }
                       }
                   }
                   catch(Exception ex)
                   {
                       dto.Code = (int)CodeEnum.ApplicationErr;
                       dto.Message = "服务未开启";
                       myLog.WarnFormat("GetServerGetDeviceState方法异常,BP4Server服务未开启！",ex);
                   }

                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "设备状态已获取完成";
                }/*end if(utc.IsValid(customerToken) == false)*/
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.ErrorFormat("GetServerGetDeviceState方法异常,设备Id{0}", ex, deviceIdList.ToString());
            }
            return dto;
        }
        #endregion

        #region  为各客户端提供播放流需要的相关信息 GetStreamerParameterByCustomerToken
        /// <summary>
        /// 为各客户端提供播放流需要的相关信息 GetStreamerParameterByCustomerToken
        /// </summary>
        /// <param name="channel">channel.ChannelId</param>
        /// <param name="customerToken">Token</param>
        /// <param name="streamerParmeter">ref 流媒体播放参数</param>
        /// <returns></returns>
        public ResponseBaseDto GetStreamerParameterByCustomerToken(Channel channel,string customerToken,ref BP4StreamerParameter streamerParmeter) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            try
            {
                UserTokenCache utc = UserTokenCache.GetInstance();
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token已失效，请重新登录后继续";
                }
                else
                {
                    /*  channel.ChannelId 通道Id
                     *  channel.DeviceId 设备Id 
                     *  bpServerConfig.BPServerConfigId 设备调用时需要的源
                     *  3个参数不可缺少
                     */
                    if (channel == null) 
                    {
                        dto.Code = (int)CodeEnum.NoComplete;
                        dto.Message = "提交数据不完整";
                        return dto;
                    }
                    IList<Channel> channelFlag=channelServer.SelectChannelByChannelId(channel);
                    if (channelFlag == null || channelFlag.Count <= 0) 
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "未找到需要的通道";
                        return dto;
                    }
                    Device device = new Device();
                    device.DeviceId = channelFlag[0].DeviceId;
                    IList<Device> deviceFlag = deviceServer.SelectDeviceByDeviceId(device);
                    if (deviceFlag == null || deviceFlag.Count <= 0)
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "未找到需要的设备";
                        return dto;
                    }
                    BPServerConfig bpServerConfig = new BPServerConfig();
                    bpServerConfig.BPServerConfigId = deviceFlag[0].BPServerConfigId;
                    IList<BPServerConfig> bpServerConfigFlag = bpServerConfigServer.GetBPServerConfigByKey(bpServerConfig);
                    if (bpServerConfigFlag == null || bpServerConfigFlag.Count <= 0) 
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "没有找到需要请求流媒体的服务端";
                        return dto;
                    }
                    //进行必须的流媒体配置
                    streamerParmeter.LoginModel = new BP4DeviceLoginModel();
                    streamerParmeter.RealStreamModel = new BP4RealStreamModel();
                    streamerParmeter.CustomerToken = customerToken;
                    streamerParmeter.LoginModel.Address =deviceFlag[0].SerialNumber;//设备的SN码 IP不可使用
                    streamerParmeter.LoginModel.AddressType = 5;
                    streamerParmeter.PlayerType = 2;
                    if (deviceFlag[0].UserName != null && deviceFlag[0].UserName != "")
                    {
                        streamerParmeter.LoginModel.UserName = deviceFlag[0].UserName;
                    }
                    if (deviceFlag[0].PassWord != null && deviceFlag[0].PassWord != "")
                    { 
                        streamerParmeter.LoginModel.Password = MDKey.Encrypt(deviceFlag[0].PassWord);
                    }
                    streamerParmeter.RealStreamModel.Channel = channelFlag[0].ChannelNumber;//该通道所属设备的插槽号
                    streamerParmeter.RealStreamModel.ChannelId = channel.ChannelId;//本地的通道Id

                    if (bpServerConfigFlag[0].StreamerPublicAddress.IndexOf(":") != -1)
                    {
                        string[] streamerAddress = bpServerConfigFlag[0].StreamerPublicAddress.Split(':');
                        streamerParmeter.RestServIp = streamerAddress[0];
                        streamerParmeter.RestServPort = Convert.ToInt32(streamerAddress[1]);
                    }
                    streamerParmeter.RealStreamModel.SubStream = 0;
                    streamerParmeter.RealStreamModel.TransProc = 0;
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "流媒体参数已准备完成";
                }/*end if(utc.IsValid(customerToken) == false)*/
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.ErrorFormat("GetStreamerParameterByCustomerToken方法异常,通道Id:{0}", ex, channel.ChannelId);

            }
            return dto;
        }
        #endregion

        #region 检索设备以设备名称和SN码 模糊查询 SearchDevice
        /// <summary>
        /// 检索设备以设备名称和SN码 模糊查询 SearchDevice
        /// </summary>
        /// <param name="keyWord">关键字</param>
        /// <param name="customerToken">Token</param>
        /// <param name="deviceFlag">ref 设备结果集</param>
        /// <returns></returns>
        public ResponseBaseDto SearchDevice(string keyWord, string customerToken,
            ref IList<Device> deviceFlag) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.TokenTimeOut;
                    dto.Message = "用户token已失效，请重新登录后继续";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
                else
                {
                    //针对用户下的设备进行查询不以用户查询
                    Customer customer = new Customer();
                    customer.CustomerId = tcp.CustomerId;
                    deviceFlag = deviceServer.SelectDeviceCustomerId(customer);

                    //查询出权限表中可以使用的设备Id
                    IList<Permission> permissionFlag = permissionServer.SelectPermissionByCustomerId(customer);
                    //当前用户下所有可以使用的设备
                    for (int i = 0; i < permissionFlag.Count; i++)
                    {
                        Permission permission = permissionFlag[0];
                        if (permission.NodeType == (int)PermissionNodeTypeEnum.Device && permission.IsEnable == 1)
                        {
                            Device device = new Device();
                            device.DeviceId = permission.NodeId;
                            IList<Device> otherDeviceFlag = deviceServer.SelectDeviceByDeviceId(device);
                            if (otherDeviceFlag != null && otherDeviceFlag.Count == 1)
                            {
                                deviceFlag.Add(otherDeviceFlag[0]);
                            }
                        }
                    }
                    List<int> deviceIdList = new List<int>();
                    for (int j = 0; j < deviceFlag.Count; j++)
                    {
                        deviceIdList.Add(deviceFlag[j].DeviceId);
                    }
                    if (keyWord == null)
                    {
                        keyWord = string.Empty;
                    }
       
                    deviceFlag= deviceServer.SelectDeviceSerialNumber(keyWord,deviceIdList);
                    dto.Message = "设备已查找完成";
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.ErrorFormat("SearchDevice方法异常，查找关键字：{0}",ex,keyWord);
            }
            return dto;
        }
        #endregion

        #region (私有方法) GetDeivceKey 设定以"device_"+deviceId 作为key添加或查找缓存
        /// <summary>
        /// (私有方法) GetDeivceKey 设定以"device_"+deviceId 作为key添加或查找缓存
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private  string GetDeivceKey(int  deviceId) 
        {
            return "Device_" + deviceId.ToString();
        }
        #endregion

        #region  以设备SN查询设备 并返回是否可以添加以及设备硬件类型 CheckDeviceBySN
        /// <summary>
        /// 获取设备SN 并返回是否可以添加以及设备硬件类型 CheckDeviceBySN
        /// </summary>
        /// <param name="SerialNumber">SN码</param>
        /// <param name="isExist">1已存在 0不存在</param>
        /// <param name="online">设备是否在线</param>
        /// <param name="hardwareType">硬件类型</param>
        /// <returns></returns>
        public ResponseBaseDto CheckDeviceBySN(string serialNumber, ref int isEnable, ref int isOnline, ref int hardwareType)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            try
            {
                IList<Device> deviceFlag=
                    deviceServer.SelectDeviceSerialNumber(new Device() { SerialNumber = serialNumber });
                if (deviceFlag != null && deviceFlag.Count != 0)
                {
                    isEnable = 0;//设备已存在，不可用
                    hardwareType = deviceFlag[0].HardwareType;
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "SN已验证完毕！";
                    return dto;
                }
                else 
                {
                    //向BP4Server请求SN码
                    Bsr.ServiceProxy.Utils.ServiceFactory serviceFactory = new ServiceProxy.Utils.ServiceFactory();
                    string seviceAddr = bllHelper.GetServerModelStr();
                    HardwareTypeDto htDto = new HardwareTypeDto();
                    htDto.IsOnline = 0;
                    htDto.Type = HardwareType.Unknown;
                    serviceFactory.GetProxy<IDevice>(new Uri(seviceAddr)).Use(
                        p =>
                        {
                          htDto=p.GetHardwareTypeBySerialNumber(serialNumber);
                        }
                        );
                    if (htDto.IsOnline != 0)
                    {
                        isEnable = 1;
                        isOnline = htDto.IsOnline;
                        hardwareType = (int)htDto.Type;
                    }
                }
            }
            catch (Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.ErrorFormat("GetDeviceBySN方法异常,设备serialNumber:{0}", ex, serialNumber);
            }
            return dto;
        }
        #endregion

        #region  以设备SN查询设备 并返回此SN是否存在 GetDeviceBySN
        /// <summary>
        ///以设备SN查询设备 并返回此SN是否存在 GetDeviceBySN
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <param name="isExist">1已存在 0不存在 </param>
        /// <returns></returns>
        public ResponseBaseDto GetDeviceBySN(string SerialNumber, ref int isExist)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            try
            {
                IList<Device> deviceFlag =
                    deviceServer.SelectDeviceSerialNumber(new Device() { SerialNumber = SerialNumber });
                isExist = deviceFlag != null && deviceFlag.Count != 0 ? 1 : 0;
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "设备SN码已确认完毕！";
            }
            catch (Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.ErrorFormat("GetDeviceBySN方法异常,设备SerialNumber:{0}", ex, SerialNumber);
            }
            return dto;
        }
        #endregion

        #region 删除设备下所有设备关系
        /// <summary>
        ///  删除设备下所有设备关系 
        /// </summary>
        /// <param name="device">device.DeviceId 需必填</param>
        /// <returns></returns>
        internal bool ClearDevice(Device device) 
        {
            bool bFlag=false;
            //删除BP4Server上设备
            bFlag = DeleteDeviceMethod(device.BPServerDeviceId);
            int deviceId = device.DeviceId;
            if (bFlag)
            {
                //删除设备
                deviceServer.DeleteDeviceByDeviceId(device);
                //删除设备相关权限
                permissionServer.DeletePermissionByNTid((int)PermissionNodeTypeEnum.Device, deviceId);
                IList<Channel> channelFlag = channelServer.SelectChannelByDeviceId(device);
                for (int i = 0; i < channelFlag.Count; i++)
                {
                    //删除通道关系
                    groupChannelServer.DeleteGroupChannelByChannelId(channelFlag[i]);
                    //删除通道相关的权限 
                    permissionServer.DeletePermissionByNTid((int)PermissionNodeTypeEnum.Channel, channelFlag[i].ChannelId);
                    //删除收藏相关通道的信息
                    userFavoriteServer.DeleteUserFavoriteByType(1, channelFlag[i].ChannelId);
                    #region 删除通道图片
                    //大图（源图）
                    // 检查存放源图片的目录是否存在，不存在则创建
                    var bigImagePath = string.Format(@"{0}\channelImage", AppDomain.CurrentDomain.BaseDirectory);
                    Directory.CreateDirectory(bigImagePath);
                    //小图（封面）
                    //检查存放图片的目录是否存在，不存在则创建
                    var smallImagepath = string.Format(@"{0}\channelImage\small", AppDomain.CurrentDomain.BaseDirectory);
                    Directory.CreateDirectory(smallImagepath);
                    //删除通道图片
                    if (channelFlag[i].ImagePath != null && !channelFlag[i].ImagePath.Contains("default"))
                    {
                        File.Delete(bigImagePath + "\\" + channelFlag[i].ImagePath);
                        File.Delete(smallImagepath + "\\" + channelFlag[i].ImagePath);
                    }
                    #endregion
                }
                //删除通道以设备Id
                channelServer.DeleteChannelByDeviceId(device);
            }
            return bFlag;
        }
        #endregion
    }
}
