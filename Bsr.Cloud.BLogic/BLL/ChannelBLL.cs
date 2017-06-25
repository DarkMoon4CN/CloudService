using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model;
using Bsr.Cloud.Model.Entities;
using Bsr.Cloud.Core;
using System.IO;
using System.ServiceModel;
using Bsr.Services.Contract;
using Bsr.DeviceAdapter.Model;
using System.Xml.Serialization;
using System.Xml;

namespace Bsr.Cloud.BLogic.BLL
{
    public class ChannelBLL
    {
        DeviceServer deviceServer = DeviceServer.GetInstance();
        ChannelServer channelServer = ChannelServer.GetInstance();
        PermissionServer permissionServer = PermissionServer.GetInstance();
        BPServerConfigServer bpServerConfigServer = BPServerConfigServer.GetInstance();
        UserFavoriteServer userFavoriteServer = UserFavoriteServer.GetInstance();
        GroupChannelServer groupChannelServer = GroupChannelServer.GetInstance();
        DeviceBLL deviceBLL = new DeviceBLL();
        static private ILogger myLog = new Logger<ChannelBLL>();
        BLLHelper bllHelper = new BLLHelper();

        #region 修改通道名 UpdateChannelByChannelId
        /// <summary>
        ///  修改通道名
        /// </summary>
        /// <param name="channel">Channel.ChannelId</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateChannelByChannelId(Channel channel,string customerToken) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            OperaterLog oLog = new OperaterLog();
            TokenCacheProperty tcp = new TokenCacheProperty();
            oLog.Action = "修改通道名";
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
                    oLog.TargetType = (int) OperaterLogEnum.Channel;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                if (tcp.SignInType ==(int)CustomerSignInTypeEnum.SubCustomer) 
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "子用户状态无法修改通道名！";
                    return dto;
                }
                else
                {
                    IList<Channel> channelFlag = channelServer.SelectChannelByChannelId(channel);
                    oLog.TargetId = channel.ChannelId;
                    if (channelFlag == null)
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "需要修改的通道不存在！";
                    }
                    else
                    {
                        try
                        {
                            Bsr.ServiceProxy.Utils.ServiceFactory serviceFactory = new ServiceProxy.Utils.ServiceFactory();
                            var seviceAddr = bllHelper.GetServerModelStr();
                            serviceFactory.GetProxy<ICamera>(new Uri(seviceAddr)).Use(
                            p =>
                            {
                                p.UpdateName(channelFlag[0].BPServerChannelId, channel.ChannelName);
                            }
                            );
                        }
                        catch (Exception ex)
                        {
                            dto.Code = (int)CodeEnum.ApplicationErr;
                            dto.Message = "修改失败，连接服务器超时！";
                            myLog.ErrorFormat("UpdateChannelByChannelId（Adapter服务器端没有做出响应）通道Id:{0},待修改的通道名:{1}", ex, channel.ChannelId, channel.ChannelName);
                            return dto;
                        }
                        string originalChannelName = channelFlag[0].ChannelName;
                        channelFlag[0].ChannelName = channel.ChannelName;
                        channelServer.UpdateChannel(channelFlag[0]);
                        dto.Code = 0;
                        dto.Message = "通道名 " + originalChannelName + "修改为 " + channel.ChannelName + " 已完成！";
                    }

                }/*end if(utc.IsValid(customerToken) == false)*/
                
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "无法连接服务器,请保证您的网络通畅！";
                myLog.ErrorFormat("UpdateChannelByChannelId方法异常,通道Id:{0},待修改的通道名:{1}", ex, channel.ChannelId, channel.ChannelName);
            }
            oLog.TargetType = (int)OperaterLogEnum.Channel;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion 

        #region 修改通道的默认图片 UpdateChanneImagePathlByChannelId
        /// <summary>
        /// 修改通道的默认图片
        /// </summary>
        /// <param name="channelId">通道的id</param>
        /// <param name="imageByte">图片的二进制数据</param>
        /// <param name="extName">图片扩展名</param>
        /// <param name="customerToken">用户的token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateChanneImagePathlByChannelId(int channelId, 
                    byte[] imageByte, string extName, string customerToken, ref string imagePath)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            OperaterLog oLog = new OperaterLog();
            oLog.Action = "更新通道图片";
            oLog.TargetId = channelId;
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
                    oLog.TargetType = (int)OperaterLogEnum.Channel;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else
                {
                    //大图（源图）
                    //检查存放源图片的目录是否存在，不存在则创建
                    var bigImagePath = string.Format(@"{0}\channelImage", AppDomain.CurrentDomain.BaseDirectory);
                    Directory.CreateDirectory(bigImagePath);

                    //小图（封面）
                    //检查存放图片的目录是否存在，不存在则创建
                    var smallImagepath = string.Format(@"{0}\channelImage\small", AppDomain.CurrentDomain.BaseDirectory);
                    Directory.CreateDirectory(smallImagepath);

                    //生成随机且唯一的image文件名，将数据保存
                    string imageFile = string.Format("{0}_{1}.{2}",
                                       PwdMD5.StringToMD5(DateTime.Now.ToString()), channelId, extName);

                    //打开文件，写入图片数据
                    string bigImageFullPath = string.Format(@"{0}\{1}", bigImagePath, imageFile);
                    string smallImageFullPath = string.Format(@"{0}\{1}", smallImagepath, imageFile);
                    FileStream bigfs = new System.IO.FileStream(bigImageFullPath, FileMode.Create, FileAccess.Write);
                    bigfs.Write(imageByte, 0, imageByte.Length);
                    bigfs.Close();

                    #region 将图片进行尺寸的压缩
                    byte[] dstImageByte;
                    ImageConvertibleInfo icInfo = new ImageConvertibleInfo();
                    icInfo.srcByte = imageByte;
                    icInfo.dstWidth = 320;   // 目标图片像素宽
                    icInfo.dstHeight = 240;  // 目标图片像素高
                    icInfo.dstFmt = extName; // 目标图片格式

                    int rs = 0;
                    if ((rs = ImageConvertion.Convert(icInfo, out dstImageByte)) != 0)
                    {
                        myLog.WarnFormat("图片压缩成{0}失败，错误码：{1}", icInfo.dstFmt, rs);
                    }
                    else
                    {
                        imageByte = dstImageByte; // 将压缩后的图片数据重新赋给imageByte
                    }
                    #endregion

                    FileStream smallfs = new System.IO.FileStream(smallImageFullPath, FileMode.Create, FileAccess.Write);
                    smallfs.Write(dstImageByte, 0, imageByte.Length);
                    smallfs.Close();
                    // 通过通道的id找到数据库记录，将文件名更新
                    Channel channel = new Channel() { ChannelId = channelId };
                    IList<Channel> channelFlag = channelServer.SelectChannelByChannelId(channel);
                    
                    if (channelFlag == null)
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "需要修改的通道不存在";
                    }
                    else
                    {
                        // 将原来指向的文件名删除。
                        // 如果是默认的文件名则不做删除。
                        if (channelFlag[0].ImagePath != null && !channelFlag[0].ImagePath.Contains("default"))
                        {
                            File.Delete(bigImagePath + "\\" + channelFlag[0].ImagePath);
                            File.Delete(smallImagepath + "\\" + channelFlag[0].ImagePath);
                        }

                        channelFlag[0].ImagePath = imageFile;
                        channelServer.UpdateChannel(channelFlag[0]);
                        dto.Code = 0;
                        dto.Message = "修改通道图片完成！";
                        // 返回的图片路径是: "channelImage/图片名.jpg"
                        imagePath = string.Format(@"channelImage/{0}", imageFile);
                    }

                }/*end if(utc.IsValid(customerToken) == false)*/
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.WarnFormat("UpdateChanneImagePathlByChannelId方法异常,通道id:{0}", ex,channelId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Channel;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion 

        #region 通道分页 GetChannelByPage
        /// <summary>
        /// 通道分页
        /// </summary>
        /// <param name="startCount">从那一条数据开始</param>
        /// <param name="requestCount">需要多少条</param>
        /// <param name="customerToken"></param>
        /// <param name="Total">返回总数据数</param>
        /// <param name="channelFlag">返回通道</param>
        /// <returns></returns>
        public ResponseBaseDto GetChannelByPage(int startCount, int requestCount, 
             string keyWord,string customerToken,
             ref IList<GroupChannelResponse> groupChannelResponseFlag,ref int Total)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
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
                    return dto;
                }
                else
                {
                   //查询当前用户下的设备
                   Customer customer=new Customer();
                   customer.CustomerId=tcp.CustomerId;
                   int primaryCustomerId = tcp.CustomerId;
                   int subCustomerId = tcp.CustomerId;
                   if(tcp.SignInType==(int)CustomerSignInTypeEnum.SubCustomer)
                   {
                       primaryCustomerId=tcp.ParentId;
                       customer.CustomerId = tcp.ParentId;
                   }

                   IList<Device> deviceFlag= deviceServer.SelectDeviceCustomerId(customer);
                   //查询出权限表中可以使用的设备Id
                   IList<Permission> permissionFlag = permissionServer.SelectPermissionByCustomerId(customer);
                   //当前用户下所有可以使用的设备
                   for (int i = 0; i < permissionFlag.Count; i++)
                   {
                       Permission permission = permissionFlag[i];
                       if (permission.NodeType == (int)PermissionNodeTypeEnum.Device && permission.IsEnable==1) 
                       {
                           Device device=new Device();
                           device.DeviceId=permission.NodeId;
                           IList<Device> otherDeviceFlag = deviceServer.SelectDeviceByDeviceId(device);
                           if (otherDeviceFlag!= null && otherDeviceFlag.Count == 1) 
                           {
                               deviceFlag.Add(otherDeviceFlag[0]);
                           }
                       }
                   }
                    string deviceIdListStr = "";
                    List<int> deviceIdList = new List<int>();
                    for (int j = 0; j < deviceFlag.Count; j++)
                    {
                        if (j != deviceFlag.Count - 1)
                        {
                            deviceIdListStr += deviceFlag[j].DeviceId + ",";
                        }
                        else
                        {
                            deviceIdListStr += deviceFlag[j].DeviceId.ToString();
                        }
                        deviceIdList.Add(deviceFlag[j].DeviceId);
                    }
                    startCount -= 1;
                    startCount = startCount < 0 ? 0 : startCount;
                    
                    IList<Channel> channelFlag = null;
                    if (tcp.SignInType == (int)CustomerSignInTypeEnum.SubCustomer)
                    {
                        //子用户登陆权限    
                        if (deviceIdListStr != string.Empty && deviceIdListStr != "")
                        {
                            channelFlag = channelServer.SelectSubChannelByDeviceIdListPage(subCustomerId, deviceIdListStr, startCount, requestCount, keyWord);
                            Total = channelServer.SelectSubChannelByDeviceIdListPageCount(subCustomerId, deviceIdListStr, keyWord);
                        }
                    }
                    else if (tcp.SignInType == (int)CustomerSignInTypeEnum.PrimaryCustomer)
                    {
                        //主用户登陆后数据
                        if (deviceIdListStr!=string.Empty && deviceIdListStr!= "")
                        {
                            channelFlag = channelServer.SelectChannelByDeviceIdListPage(deviceIdListStr, startCount, requestCount, keyWord);
                            Total = channelServer.SelectChannelByDeviceIdListPageCount(deviceIdListStr, keyWord);
                        }
                    }

                    //填充 需要的 授权字段
                    groupChannelResponseFlag = ConvertGroupChannelResponse(tcp,primaryCustomerId, channelFlag);
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "通道数据获取完成";
                }/*end if(utc.IsValid(customerToken) == false)*/
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.WarnFormat("GetChannelByPage方法异常,用户id:{0},起始条数:{1},请求条数:{2},关键字:{3}",
                    ex,tcp.CustomerId,startCount,requestCount,keyWord);

            }
            return dto;
        }
        #endregion

        #region 查询单一通道信息 GetSingleChannel
        /// <summary>
        /// 查询单一通道信息 GetSingleChannel
        /// </summary>
        /// <param name="customerToken">token</param>
        /// <param name="channel">通道信息</param>
        /// <returns></returns>
        public ResponseBaseDto GetSingleChannel(int channelId, string customerToken, ref GroupChannelResponse groupChannelResponse) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
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
                    return dto;
                }
                else
                {
                    int primaryCustomerId=0;
                    IList<Channel> channelFlag = channelServer.SelectChannelByChannelId(new Channel() { ChannelId = channelId });
                    if (tcp.SignInType == (int)CustomerSignInTypeEnum.SubCustomer) 
                    {
                      IList<Permission> permissionFlag=  permissionServer.SelectPermissionByCidAndNid(
                            new Customer() { CustomerId = tcp.CustomerId }, 
                            new Permission() {NodeType=(int)PermissionNodeTypeEnum.Channel,NodeId=channelId });
                      if (permissionFlag == null || permissionFlag.Count == 0) 
                      {
                          dto.Code = (int)CodeEnum.NoAuthorization;
                          dto.Message = "您当前账户里没有此观看权限！";
                          return dto;
                      }
                        primaryCustomerId=tcp.ParentId;
                    }
                    else if (tcp.SignInType == (int)CustomerSignInTypeEnum.PrimaryCustomer) 
                    {
                        if (channelFlag == null || channelFlag.Count == 0) 
                        {
                            dto.Code = (int)CodeEnum.NoAuthorization;
                            dto.Message = "您当前账户里没有此观看权限！";
                            return dto;
                        }
                        IList<Device> deviceFlag = deviceServer.SelectDeviceByDeviceId(new Device() { DeviceId = channelFlag[0].DeviceId });
                        if (deviceFlag == null || deviceFlag.Count == 0 || deviceFlag[0].CustomerId != tcp.CustomerId) 
                        {
                            dto.Code = (int)CodeEnum.NoAuthorization;
                            dto.Message = "您当前账户里没有此观看权限！";
                            return dto;
                        }
                        primaryCustomerId=tcp.CustomerId;
                    }
                    if (channelFlag != null && channelFlag.Count==1) 
                    {
                        var result = ConvertGroupChannelResponse(tcp,primaryCustomerId, channelFlag);
                        if (result != null && result.Count == 1) 
                        {
                            groupChannelResponse = result[0];
                        }
                    }
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "通道数据获取完成";
                }/*end if(utc.IsValid(customerToken) == false)*/
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.WarnFormat("GetSingleChannel方法异常,通道Id:{0}",ex,channelId);
            }
            return dto;
        }
        #endregion

        #region 修改通道是否启用 EnableChannel
        /// <summary>
        ///  修改通道是否启用 EnableChannel
        /// </summary>
        /// <param name="channel">Channel.ChannelId</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto EnableChannel(Channel channel, string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            OperaterLog oLog = new OperaterLog();
            TokenCacheProperty tcp = new TokenCacheProperty();
            oLog.Action = "启用或禁用通道";
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
                    oLog.TargetType = (int)OperaterLogEnum.Channel;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else
                {
                    IList<Channel> channelFlag = channelServer.SelectChannelByChannelId(channel);
                    oLog.TargetId = channel.ChannelId;
                    if (channelFlag == null)
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "需要修改的通道不存在";
                    }
                    else
                    {
                        string originalChannelName = channelFlag[0].ChannelName;
                        channelFlag[0].IsEnable = channel.IsEnable;
                        channelServer.UpdateChannel(channelFlag[0]);
                        dto.Code = 0;
                        dto.Message = originalChannelName;
                        dto.Message += channel.IsEnable == 1 ? "通道已启用" : "通道已禁用";
                    }
                }/*end if(utc.IsValid(customerToken) == false)*/

            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.WarnFormat("EnableChannel方法异常,用户Id:{0},通道Id:{1}",ex,tcp.CustomerId,channel.ChannelId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Channel;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion 

        #region 设置通道码流
        /// <summary>
        /// 设置通道码流
        /// </summary>
        /// <param name="channelId">通道id</param>
        /// <param name="streamType">码流类型</param>
        /// <param name="customerToken">用户token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateChannelEncoderInfo(int channelId, int streamType, string customerToken) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            OperaterLog oLog = new OperaterLog();
            TokenCacheProperty tcp = new TokenCacheProperty();
            oLog.Action = "设置通道码流";
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
                    oLog.TargetType = (int)OperaterLogEnum.Channel;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else
                {
                    IList<Channel> channelFlag = channelServer.SelectChannelByChannelId(new Channel() {ChannelId=channelId });
                    oLog.TargetId = channelId;
                    if (channelFlag == null)
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "需要修改的通道不存在";
                    }
                    else if (streamType == 0)
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "未选中码流类型";
                    }
                    else
                    {                       
                        try
                        {
                            //获取已预制的码流配置
                            IDictionary<int, AVEncoderInfoDto> avEncoderInfoList = GetXMLChannelEncoderList();
                            AVEncoderInfoDto enCoderInfo = avEncoderInfoList[streamType];
                            //调用BP4Server 设置码流
                            Bsr.ServiceProxy.Utils.ServiceFactory serviceFactory = new ServiceProxy.Utils.ServiceFactory();
                            var seviceAddr = bllHelper.GetServerModelStr();
                            serviceFactory.GetProxy<ICamera>(new Uri(seviceAddr)).Use(
                            p =>
                            {
                                if (streamType == 1)
                                {
                                    //主码流
                                    p.UpdateEncoderInfo(channelFlag[0].BPServerChannelId, (byte)(1), enCoderInfo);
                                }
                                else if (streamType == 2 || streamType == 3)
                                {
                                    //选择码流类型（streamType）在BP4Server中 2和3都属于子码流
                                    p.UpdateEncoderInfo(channelFlag[0].BPServerChannelId, (byte)(1 << 1), enCoderInfo);
                                }
                                else 
                                {
                                    //第3码流
                                    p.UpdateEncoderInfo(channelFlag[0].BPServerChannelId, (byte)(1 << 2), enCoderInfo);
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            dto.Code = (int)CodeEnum.ApplicationErr;
                            dto.Message = "通道所属设备已离线！离线状态无法修改通道码流";
                            myLog.ErrorFormat("UpdateChannelEncoderInfo（BP4Server设备已离线）通道Id:{0}", ex, channelId);
                            return dto;
                        }

                    }
                }/*end if(utc.IsValid(customerToken) == false)*/

            }
            catch (Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.WarnFormat("UpdateChannelEncoderInfo方法异常,用户Id:{0},通道Id:{1}", ex,tcp.CustomerId,channelId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Channel;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 将数据转换成 GroupChannelResponse 并将中的设备 授权 进行填充
        /// <summary>
        ///  将数据转换成 GroupChannelResponse 并将中的设备 授权 收藏状态  分组信息 进行填充
        /// </summary>
        /// <param name="tcp">当前用户信息</param>
        /// <param name="primaryCustomerId">需要调用谁的设备和分组信息</param>
        /// <param name="channelFlag">通道信息</param>
        /// <returns></returns>
        internal List<GroupChannelResponse> ConvertGroupChannelResponse(TokenCacheProperty tcp,int primaryCustomerId, IList<Channel> channelFlag)
        {
            List<int> deviceIdFlag = new List<int>();
            List<int> channelIdFlag = new List<int>();//用于进行分组获取
            List<GroupChannelResponse> tmpGroupChannelResponseList = new List<GroupChannelResponse>();
            if (channelFlag == null || channelFlag.Count == 0)
            {
                return tmpGroupChannelResponseList;
            }
            for (int i = 0; i < channelFlag.Count; i++)
            {
                deviceIdFlag.Add(channelFlag[i].DeviceId);
            }
            //去除重复设备Id
            deviceIdFlag = deviceIdFlag.Distinct().ToList();
            //获取出所有通道的所属 设备的授权集合
            IList<Permission> permissionFlag = permissionServer.SelectDeviceAuthorizeByDeviceIdList(primaryCustomerId, deviceIdFlag);
            //通道进行添加是否是授权设备，是否被收藏
            for (int i = 0; i < channelFlag.Count; i++)
            {
                Channel channel = channelFlag[i];
                GroupChannelResponse gcr = new GroupChannelResponse();
                gcr.ChannelId = channel.ChannelId;
                gcr.ChannelName = channel.ChannelName;
                for (int j = 0; j < permissionFlag.Count; j++)
                {
                    if (permissionFlag[j].NodeType == (int)PermissionNodeTypeEnum.Device
                        && channel.DeviceId == permissionFlag[j].NodeId
                        && permissionFlag[j].IsEnable == 1)
                    {
                        //当用户 是设备 + 通道的设备字段与权限相同 +权限的中的可用状态为1
                        //将这个通道输出时，变为是授权
                        gcr.IsAuthorize = 1;
                    }
                }
                //是否收藏了通道
                IList<UserFavorite> userFavoriteFlag = userFavoriteServer.SelectCustomerByTid(
                      new UserFavorite() {CustomerId=tcp.CustomerId, UserFavoriteType=(int)UserFavoriteTypeEnum.Channel,UserFavoriteTypeId=gcr.ChannelId });
                if (userFavoriteFlag != null && userFavoriteFlag.Count == 1)
                {
                    gcr.IsFavorite = 1;
                }
                gcr.ImagePath = channel.ImagePath;
                gcr.IsEnable = channel.IsEnable;
                gcr.DeviceId = channel.DeviceId;
                gcr.ChannelNumber = channel.ChannelNumber;
                tmpGroupChannelResponseList.Add(gcr);
                channelIdFlag.Add(gcr.ChannelId);
            }
            //以primaryCustomerId和多个channelId查找出它们的分组
            IList<GroupChannel> groupChannelFlag = 
                               groupChannelServer.SelectGroupChannelByChannelIdListAndCustomerId(primaryCustomerId, channelIdFlag);
            //将tmpGroupChannelResponseList里的通道进行分组赋值
            for (int i = 0; i < tmpGroupChannelResponseList.Count; i++)
            {
                var result = from gc in groupChannelFlag
                             where tmpGroupChannelResponseList[i].ChannelId == gc.channel.ChannelId
                             select gc;
                //防止迭代器版本号丢失和空数据
                if (result != null && result.Count() != 0)
                {
                    tmpGroupChannelResponseList[i].ResourceGroupId = result.First().resourceGroup.ResourceGroupId;
                }
            }
            return tmpGroupChannelResponseList;
        }
        #endregion

        #region 将ChannelEncoder.xml转换成AVEncoderInfoDto 字典
        /// <summary>
        ///  将ChannelEncoder.xml转换成AVEncoderInfoDto 字典
        /// </summary>
        /// <returns></returns>
        private IDictionary<int, AVEncoderInfoDto> GetXMLChannelEncoderList() 
        {
            IDictionary<int, AVEncoderInfoDto> avEncoderInfo = new Dictionary<int, AVEncoderInfoDto>();
            string xmlPath = AppDomain.CurrentDomain.BaseDirectory;
            string xmlName = "ChannlEncoder.xml";
            string fullPath = xmlPath + "\\" + xmlName;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fullPath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("ChannlEncoder").ChildNodes;
            foreach (XmlNode xn in nodeList)
            {
                AVEncoderInfoDto avInfo = new AVEncoderInfoDto();
                XmlElement xe = (XmlElement)xn;

                string streamType = xe.Attributes["StreamType"].Value;
                int subStream = 0;
                int.TryParse(streamType, out subStream);
                XmlNodeList resultList = xe.ChildNodes;
                if (resultList != null && resultList.Count == 5)
                {
                    XmlElement resolution = (XmlElement)resultList[0];
                    XmlElement frameRate = (XmlElement)resultList[1];
                    XmlElement bitrateMode = (XmlElement)resultList[2];
                    XmlElement bitrate = (XmlElement)resultList[3];
                    XmlElement iFrameInterval = (XmlElement)resultList[4];

                    byte btRes;
                    byte.TryParse(resolution.InnerText.Trim(), out btRes);
                    avInfo.Resolution = btRes;

                    byte btFra;
                    byte.TryParse(frameRate.InnerText.Trim(), out btFra);
                    avInfo.FrameRate = btFra;

                    byte btMode;
                    byte.TryParse(bitrateMode.InnerText.Trim(), out btMode);
                    avInfo.BitrateMode = btMode;

                    byte btIFr;
                    byte.TryParse(iFrameInterval.InnerText.Trim(), out btIFr);
                    avInfo.IFrameInterval = btIFr;

                    uint uiBit;
                    uint.TryParse(bitrate.InnerText.Trim(), out uiBit);
                    avInfo.Bitrate = uiBit;

                    avEncoderInfo.Add(subStream, avInfo);
                }
            }
            return avEncoderInfo;
        }
        #endregion
    }
}
