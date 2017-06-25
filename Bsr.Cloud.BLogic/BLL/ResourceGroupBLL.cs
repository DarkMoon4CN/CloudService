using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model;
using Bsr.Cloud.Model.Entities;
using Bsr.Cloud.Core;
using System.Collections;

namespace Bsr.Cloud.BLogic.BLL
{
    public class ResourceGroupBLL
    {
        CustomerServer customerServer = CustomerServer.GetInstance();//用户
        ResourceGroupServer resourceGroupServer = ResourceGroupServer.GetInstance();//分组
        GroupChannelServer groupChannelServer = GroupChannelServer.GetInstance();//通道分组
        DeviceServer deviceServer = DeviceServer.GetInstance();//设备
        ChannelServer channelServer = ChannelServer.GetInstance();//通道
        PermissionServer permissionServer = PermissionServer.GetInstance();//权限
        UserFavoriteServer userFavoriteServer = UserFavoriteServer.GetInstance();//收藏
        ChannelBLL channelBLL = new ChannelBLL();
        static private ILogger myLog = new Logger<ResourceGroupBLL>();
        public BLLHelper bllHelper = new BLLHelper(); 

        #region 添加通道分组 AddResourceGroupByName
        /// <summary>
        ///  添加通道分组 AddResourceGroupByName
        /// </summary>
        /// <param name="resourceGroup">分组中的 分组名与 分组的父节点</param>
        /// <param name="customerToken">用户的唯一Token</param>
        /// <returns>0 表示成功   3 代表服务器异常  5 代表失效</returns>
        public ResponseBaseDto AddResourceGroupByName(ResourceGroup resourceGroup, string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            OperaterLog oLog = new OperaterLog();
            oLog.Action = "添加分组";
            //校验token
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            int customerId = 0;
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }else
            {
                try
                {
                    dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                    if (dto.Code != 0)
                    {
                        oLog.Remarks = dto.Message;
                        oLog.Result = dto.Code;
                        bllHelper.AddOperaterLog(oLog, tcp);
                        return dto;
                    }
                    //添加组
                    customerId = tcp.CustomerId;
                    resourceGroup.CustomerId = tcp.CustomerId;
                    int resourceGroupId = (int)resourceGroupServer.InsertResourceGorup(resourceGroup);
                    oLog.TargetId = resourceGroupId;
                    if (resourceGroupId > 0)
                    {
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "添加通道分组已完成";
                    }
                    else 
                    {
                        dto.Code = (int)CodeEnum.ApplicationErr;
                        dto.Message = "分组添加异常";
                    }
                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，添加分组失败！";
                    myLog.ErrorFormat("AddResourceGroupByName方法异常,用户Id:{0},添加的分组名:{2}", ex,customerId,resourceGroup.ResourceGroupName);
                }
            
            }
            oLog.TargetType = (int)OperaterLogEnum.ResourceGroup;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 添加通道（暂时不使用）
        /// <summary>
        ///  添加通道
        /// <param name="Channel">通道名,通道的设备Id,通道的编号</param>
        /// <param name="CustomerToken">用户的唯一Token</param>
        /// <returns>0 表示成功   3 代表服务器异常  5代表失效</returns>
        public ResponseBaseDto AddChannel(Channel  channel,string customerToken,ref int channelId) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            //校验token
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }
            else
            {
                try {
                    channelId= channelServer.InsertChannel(channel);
                    if (channelId != 0)
                    {
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "添加通道分组已完成";
                    }
                    else
                    {
                        dto.Code = (int)CodeEnum.ApplicationErr;
                        dto.Message = "网络中断，未添加成功";
                    }
                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，请刷新页面后继续";
                    myLog.WarnFormat("AddChannel方法异常", ex);
                }   
            }
            return dto;
        }
        #endregion

        #region 查询用户通道以CustomerId和List<DeviceId>查询 GetChannelByCustomerId 
        /// <summary>
        /// 查询用户设备下的通道
        /// </summary>
        /// <param name="customer">customerId</param>
        /// <param name="customerToken">token</param>
        /// <param name="deviceChannel">返回数据：通道</param>
        /// <returns></returns>
        public ResponseBaseDto GetChannelByCustomerId(string customerToken, ref IList<Channel> deviceChannel) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            int customerId = 0;
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }
            else
            {
                try
                {
                    dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                    if (dto.Code != 0)
                    {
                        return dto;
                    }
                    else
                    {
                        //获取当前用户所有的Device
                        //区分主用户与子用户
                        Customer customer = new Customer();
                        customer.CustomerId = tcp.CustomerId;
                        customerId = tcp.CustomerId;
                        IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(customer);
                        if (customerFlag[0].ParentId != 0 && customerFlag[0].SignInType != 2)
                        {
                            //子账户将查询主用户的分组信息
                            customerFlag[0].CustomerId = customerFlag[0].ParentId;  
                        }
                        //获取用户下的设备
                        IList<Device> customerDevice = deviceServer.SelectDeviceCustomerId(customer);
                        //获取设备下的通道
                        if (customerDevice != null &&customerDevice.Count!=0)
                        {
                            List<int> deviceIdFlag=new List<int>();
                            for (int i = 0; i < customerDevice.Count; i++)
                            {
                                deviceIdFlag.Add(customerDevice[0].DeviceId);
                            }
                            deviceChannel = channelServer.SelectChannelByDeviceIdList(deviceIdFlag);
                        }
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "设备数据获取完成";
                    }
                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，请刷新页面后继续";
                    myLog.WarnFormat("GetChannelByCustomerId方法异常,用户Id:{0},",ex,customerId);
                }
            }
            return dto;
        }
        #endregion

        #region 查询用户分组 GetGroupByCustomerId
        /// <summary>
        /// 查询用户分组
        /// </summary>
        /// <param name="customer">customerid</param>
        /// <param name="customerToken">token</param>
        /// <param name="ResourceGroup">返回数据 分组</param>
        /// <returns></returns>
        public ResponseBaseDto GetGroupByCustomerId(string customerToken, ref IList<ResourceGroupResponse> resourceGroupResponse)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }
            else 
            {
                try
                {
                    dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                    if (dto.Code != 0)
                    {
                        return dto;
                    }
                    else
                    {
                        Customer customer = new Customer();
                        customer.CustomerId = tcp.CustomerId;
                        int primaryCustomerId = tcp.CustomerId;
                        IList<ResourceGroup> resourceGroup = new List<ResourceGroup>();
                        bool isPrimaryCustomer = true; // 主用户 is true
                        IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(customer);
                        if (customerFlag[0].ParentId != 0 && customerFlag[0].SignInType == (int)CustomerSignInTypeEnum.SubCustomer)
                        {
                            //子账户将查询主用户的分组信息
                            primaryCustomerId = customerFlag[0].ParentId;
                            isPrimaryCustomer = false;
                        }
                        //子用户起来后添加权限逻辑
                        var allResourceGroup = resourceGroupServer.SelectResourceGorupByCustomerId(primaryCustomerId);
                        // 主账号直接返回
                        if (isPrimaryCustomer == true)
                        {
                            resourceGroup = allResourceGroup;
                        }
                        else
                        {
                            // 子账号要过滤掉没有镜头的空分组
                            IList<ResourceGroup> validResourceGroup = resourceGroupServer.GetResourceGroupBySubCustomerId(tcp.CustomerId, primaryCustomerId);
                            // 返回的resourceGroup个数至少是validResourceGroup,
                            // 所以先复制出来
                            foreach (var it in validResourceGroup)
                            {
                                resourceGroup.Add(it);
                            }
                            // 遍历validResourceGroup, 把其节点中，父节点不在resourceGroup内的，加进去。
                            foreach (var it in validResourceGroup)
                            {
                                CheckAndCopy(allResourceGroup,ref resourceGroup, it);
                            }
                        }
                        //加载分组是否有下一级
                        for (int i = 0; i < resourceGroup.Count; i++)
                        {
                             var result= from rs in resourceGroup 
                                         where resourceGroup[i].ResourceGroupId==rs.ParentResourceGroupId
                                         select rs;
                             ResourceGroupResponse rg = new ResourceGroupResponse();
                             if (result.Count()!=0) 
                             {
                                 rg.IsNext = 1;
                             }
                             rg.CustomerId = resourceGroup[i].CustomerId;
                             rg.ResourceGroupId = resourceGroup[i].ResourceGroupId;
                             rg.ResourceGroupName = resourceGroup[i].ResourceGroupName;
                             rg.ParentResourceGroupId = resourceGroup[i].ParentResourceGroupId;
                             resourceGroupResponse.Add(rg);
                        }
                    }
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "分组数据获取完成";

                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，请刷新页面后继续";
                    myLog.WarnFormat("GetGroupByCustomerId方法异常,用户Id:{0}",ex,tcp.CustomerId);
                }
            }
            return dto;
        }
        #endregion

        /// <summary>
        /// 检查并拷贝it的父节点至dstList
        /// </summary>
        /// <param name="allRgList"></param>
        /// <param name="dstList"></param>
        /// <param name="it"></param>
        private void CheckAndCopy(IList<ResourceGroup> allRgList,ref IList<ResourceGroup> dstList, ResourceGroup it)
        {
            // 如果是根分组，直接返回
            if (it.ParentResourceGroupId == 0)
            {
                return;
            }

            // 检查it的父id在不在dstList里面，如果在，则直接返回
            foreach (var item in dstList)
            {
                if (it.ParentResourceGroupId == item.ResourceGroupId)
                {
                    return;
                }
            }

            // it 的父id不在rgList里面
            foreach (var item in allRgList)
            {
                // 从allRgList中拷贝it的父节点到dstList中，然后在检查这个父节点的父节点在不在，依次递归。
                if (it.ParentResourceGroupId == item.ResourceGroupId)
                {
                    dstList.Add(item);
                    CheckAndCopy(allRgList, ref dstList, item);
                }
            }
            return;
        }


        #region 私有参数查询出分组下的所有分组 GetchildResourceGroup
        /// <summary>
        /// 查询分组下的所有子分组
        /// </summary>
        /// <param name="resourceGroup">分组Id</param>
        /// <param name="resourceGroupList">返回结果集</param>
        private void GetchildResourceGroup(ResourceGroup resourceGroup, ref IList<ResourceGroup> resourceGroupList) 
        {
            //查询分组下的所有子分组
            IList<ResourceGroup> myResourceGroupList = resourceGroupServer.SelectResourceGorupByChildId(resourceGroup);
            for (int i = 0; i < myResourceGroupList.Count; i++)
            {
                resourceGroupList.Add(myResourceGroupList[i]);
                GetchildResourceGroup(myResourceGroupList[i], ref resourceGroupList);
            }
        }
        #endregion

        #region 移动分组UpdateResourceGroupByParentId
        /// <summary>
        ///  用户移动分组时调用 UpdateResourceGroupByParentId
        /// </summary>
        /// <param name="resourceGroup">ResourceGroupId, 需要移动到哪个父节点上ParentResourceGroupId</param>
        /// <param name="customerToken"></param>
        /// <returns></returns>
        public ResponseBaseDto UpdateResourceGroupByParentId(ResourceGroup resourceGroup, string customerToken) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            OperaterLog oLog = new OperaterLog();
            oLog.Action = "移动分组";
            int customerId = 0;
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }
            else
            {
                try 
                {
                    dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                    if (dto.Code != 0)
                    {
                        oLog.Remarks = dto.Message;
                        oLog.Result = dto.Code;
                        bllHelper.AddOperaterLog(oLog, tcp);
                        return dto;
                    }
                    else if (resourceGroup.ResourceGroupId == 0 && resourceGroup.ParentResourceGroupId == 0)
                    {
                        dto.Code = (int)CodeEnum.NoComplete;
                        dto.Message = "数据请求不完整";
                        oLog.Remarks = dto.Message;
                        oLog.Result = dto.Code;
                        bllHelper.AddOperaterLog(oLog, tcp);
                        return dto;
                    }
                    else
                    {
                        customerId = tcp.CustomerId;
                        IList<ResourceGroup> resourceGroupFlag  =
                            resourceGroupServer.SelectResourceGorupByResourceGroupId(resourceGroup);
                        if (resourceGroupFlag != null && resourceGroupFlag.Count != 0)
                        {
                            ResourceGroup rg = resourceGroupFlag[0];
                            rg.ResourceGroupId = resourceGroup.ResourceGroupId;
                            rg.ParentResourceGroupId = resourceGroup.ParentResourceGroupId;
                            rg.CustomerId = tcp.CustomerId;
                            resourceGroupServer.UpdateResourceGorupByParentId(rg);
                            dto.Code = (int)CodeEnum.Success;
                            dto.Message = "分组移动已完成！";
                        }
                        else 
                        {
                            dto.Code = (int)CodeEnum.NoData;
                            dto.Message = "用户数据不存在！";
                        }
                    }
                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，移动分组失败！";
                    myLog.ErrorFormat("UpdateResourceGroupByParentId方法异常,用户Id:{0},分组id:{1}",ex,customerId,resourceGroup.ResourceGroupId);
                }          
            }
            oLog.TargetType = (int)OperaterLogEnum.ResourceGroup;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 更新分组名称 UpdateResourceGorupByName
        /// <summary>
        ///  更新分组名称 UpdateResourceGorupByName
        /// </summary>
        /// <param name="resourceGroup">ResourceGroupId, 需要修改的节点名称</param>
        /// <param name="customerToken"></param>
        /// <returns></returns>
        public ResponseBaseDto UpdateResourceGorupByName(ResourceGroup resourceGroup, string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            OperaterLog oLog = new OperaterLog();
            oLog.Action = "更新分组名称";
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }
            else
            {
                try
                {
                    dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                    if (dto.Code != 0)
                    {
                        oLog.Remarks = dto.Message;
                        oLog.Result = dto.Code;
                        bllHelper.AddOperaterLog(oLog, tcp);
                        return dto;
                    }
                    if (resourceGroup.ResourceGroupId == 0 && resourceGroup.ResourceGroupName ==null)
                    {
                        dto.Code = (int)CodeEnum.NoComplete;
                        dto.Message = "数据请求不完整";
                        oLog.Remarks = dto.Message;
                        oLog.Result = dto.Code;
                        bllHelper.AddOperaterLog(oLog, tcp);
                        return dto;
                    }
                    else
                    {
                         IList<ResourceGroup> resourceGroupFlag  =
                            resourceGroupServer.SelectResourceGorupByResourceGroupId(resourceGroup);
                         if (resourceGroupFlag != null && resourceGroupFlag.Count != 0)
                         {

                             ResourceGroup rg = resourceGroupFlag[0];
                             rg.ResourceGroupId = resourceGroup.ResourceGroupId;
                             rg.ResourceGroupName = resourceGroup.ResourceGroupName;
                             resourceGroupServer.UpdateResourceGorupByName(rg);
                             dto.Code = (int)CodeEnum.Success;
                             dto.Message = "分组名称更新完成！";
                             oLog.Remarks = "已将分组名为："+rg.ResourceGroupName +"修改为："+resourceGroup.ResourceGroupName;
                         }
                         else 
                         {
                             dto.Code = (int)CodeEnum.NoData;
                             dto.Message = "用户数据不存在！";
                         }
                    }
                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，更新分组名称失败！";
                    myLog.ErrorFormat("UpdateResourceGorupByName方法异常,用户Id:{0},分组id:{1},分组名:{2}", ex, tcp.CustomerId, resourceGroup.ResourceGroupId,resourceGroup.ResourceGroupName);
                }

            }
            if (oLog.Remarks == null || oLog.Remarks == "") 
            {
                oLog.Remarks = dto.Message;
            }
            oLog.TargetType = (int)OperaterLogEnum.ResourceGroup;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        //------------------------------------------------------
        #region 移动分组下所有通道 UpdateChannelListByResourceGroupId Date:2014/12/15
        /// <summary>
        /// 移动分组下的所有通道 UpdateChannelListByResourceGroupId 
        /// Date:2014/12/15 
        /// </summary>
        /// <param name="resourceGroup">ResourceGroupId</param>
        /// <param name="channelIdList">通道数组</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateChannelListByResourceGroupId(ResourceGroup resourceGroup, int[] channelIdList, string customerToken) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            OperaterLog oLog = new OperaterLog();
            oLog.Action = "移动分组下通道";
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }
            else
            {
                try
                {
                    dto = bllHelper.CheckCustomer(dto, customerToken,ref tcp);
                    if (dto.Code != 0) 
                    {
                        oLog.Remarks = dto.Message;
                        oLog.Result = dto.Code;
                        bllHelper.AddOperaterLog(oLog, tcp);
                        return dto;
                    }
                    //查找关联其他通道信息
                    string channelNameListStr = "";
                    string channelMoveErrName = "";
                    for (int i = 0; i < channelIdList.Length; i++)
                    {
                        GroupChannel groupChannel = new GroupChannel();
                        groupChannel.CustomerId = tcp.CustomerId;
                        groupChannel.channel = new Channel() { ChannelId = channelIdList[i] };
                        groupChannel.resourceGroup = new ResourceGroup { ResourceGroupId =resourceGroup.ResourceGroupId };
                        IList<GroupChannel> groupChannelFlag = groupChannelServer.SelectGroupChannelByChannelIdAndCustomerId(groupChannel);
                        channelNameListStr += "["+groupChannel.channel.ChannelName + "] ";
                        channelNameListStr += channelIdList[i] + "]";
                        if (groupChannelFlag != null && groupChannelFlag.Count != 0)
                        {
                            groupChannelFlag[0].resourceGroup.ResourceGroupId = 
                                                     groupChannel.resourceGroup.ResourceGroupId;
                            groupChannelServer.UpdateGroupChannel(groupChannelFlag[0]);
                        }
                        else 
                        {
                            try
                            {
                                groupChannelServer.InertGroupChannel(groupChannel);
                            }
                            catch 
                            {
                                channelMoveErrName+="["+groupChannel.channel.ChannelName+"]" ;
                            }
                            dto.Code = (int)CodeEnum.Success;
                            dto.Message = "已完成通道移动！"+channelMoveErrName+"通道已有过分组";
                        }

                    }
                    IList<ResourceGroup> resourceGroupFlag=resourceGroupServer.SelectResourceGorupByResourceGroupId(resourceGroup);
                    if (resourceGroupFlag !=null &&resourceGroupFlag.Count!=0
                        && channelNameListStr.Length > 0) 
                    {
                        channelNameListStr = channelNameListStr.Substring(0,channelNameListStr.Length-1);
                        dto.Message = "已将通道" + channelNameListStr + "移动至分组" + resourceGroupFlag[0].ResourceGroupName;
                    }
                    
                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，通道移动失败";
                    myLog.ErrorFormat("UpdateChannelListByResourceGroupId方法异常,分组id:{0},通道id集合:{1}",ex,resourceGroup.ResourceGroupId,channelIdList.ToString());
                }
            
            }
            oLog.TargetType = (int)OperaterLogEnum.ResourceGroup;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 查询分组下(包括子分组)的所有通道 分页 GetChannelByPageOrResourceGroupId Date：2015/1/6
        /// <summary>
        /// 查询分组下(包括子分组)的所有通道 分页 GetChannelByPageOrResourceGroupId
        /// </summary>
        /// <param name="resourceGroup"></param>
        /// <param name="customerToken"></param>
        /// <param name="groupChannel"></param>
        /// <param name="Total"></param>
        /// <returns></returns>
        public ResponseBaseDto GetChannelByPageOrResourceGroupId
            (ResourceGroup resourceGroup,int startCount,int requestCount,string keyWord,
             string customerToken, ref IList<GroupChannelResponse> groupChannelResponseFlag, ref int total)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }
            else
            {
                try
                {
                    dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                    if (dto.Code != 0)
                    {
                        return dto;
                    }
                    //查询当前用户的分组
                    List<int> resourceGroupIdFlag = new List<int>();
                    IList<ResourceGroup> allResourceList = new List<ResourceGroup>();
                    bool isSubCustomer = false;
                    int primaryCustomerId = 0;
                    string deviceIdListStr = "";
                    startCount -= 1;
                    startCount = startCount < 0 ? 0 : startCount;
                    if (tcp.SignInType == (int)CustomerSignInTypeEnum.SubCustomer)
                    {
                        isSubCustomer = true;
                        primaryCustomerId = tcp.ParentId;
                    }
                    else 
                    {
                        isSubCustomer = false;
                        primaryCustomerId = tcp.CustomerId;
                    }
                    //查找是否有授权的设备
                    Customer primaryCustomer = new Customer() { CustomerId = primaryCustomerId };
                    IList<Device> primaryDeviceFlag =
                        deviceServer.SelectDeviceCustomerId(primaryCustomer);
                    //查询出权限表中可以使用的设备Id
                    IList<Permission> primaryPermissionFlag = permissionServer.SelectPermissionByCustomerId(primaryCustomer);
                    //当前用户下所有可以使用的设备
                    for (int i = 0; i < primaryPermissionFlag.Count; i++)
                    {
                        Permission permission = primaryPermissionFlag[0];
                        if (permission.NodeType == (int)PermissionNodeTypeEnum.Device && permission.IsEnable == 1)
                        {
                            Device device = new Device();
                            device.DeviceId = permission.NodeId;
                            IList<Device> otherDeviceFlag = deviceServer.SelectDeviceByDeviceId(device);
                            if (otherDeviceFlag != null && otherDeviceFlag.Count == 1)
                            {
                                primaryDeviceFlag.Add(otherDeviceFlag[0]);
                            }
                        }
                    }
                    //将设备集合对象中的设备ID修改为 21,22,33
                    for (int j = 0; j < primaryDeviceFlag.Count; j++)
                    {
                        if (j != primaryDeviceFlag.Count - 1)
                        {
                            deviceIdListStr += primaryDeviceFlag[j].DeviceId + ",";
                        }
                        else
                        {
                            deviceIdListStr += primaryDeviceFlag[j].DeviceId.ToString();
                        }
                    }
                    //如果用户的分组为0(isGroup=1,ResourceGroupId=0),将查询未分组的通道
                    if(resourceGroup !=null && resourceGroup.ResourceGroupId==0
                        && deviceIdListStr != string.Empty && deviceIdListStr != "")
                    { 
                        IList<Channel> channelFlag=new List<Channel>();
                        GetChannelByNoGroup(primaryCustomerId, deviceIdListStr, isSubCustomer,
                                     startCount, requestCount, keyWord, ref channelFlag,ref total);
                        groupChannelResponseFlag =
                                     channelBLL.ConvertGroupChannelResponse(tcp,primaryCustomerId, channelFlag);
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "通道获取完成！";
                        return dto;
                    }

                    GetchildResourceGroup(resourceGroup, ref allResourceList);
                    allResourceList.Add(resourceGroup);
                    for (int i = 0; i < allResourceList.Count; i++)
                    {
                        resourceGroupIdFlag.Add(allResourceList[i].ResourceGroupId);
                    }
                    string resourceGroupIdListStr = string.Empty;
                    for (int j = 0; j < resourceGroupIdFlag.Count; j++)
                    {
                        if (j != resourceGroupIdFlag.Count - 1)
                        {
                            resourceGroupIdListStr += resourceGroupIdFlag[j] + ",";
                        }
                        else
                        {
                            resourceGroupIdListStr += resourceGroupIdFlag[j].ToString();
                        }
                    }
                    IList result = null;
                    if (isSubCustomer)
                    {
                        //子用户有权限
                        if (deviceIdListStr != string.Empty && deviceIdListStr != "")
                        {
                            result = groupChannelServer.SelectSubChannelByPageOrResourceGroupId
                                                                             (tcp.CustomerId, startCount, requestCount, keyWord, resourceGroupIdListStr);
                            total = groupChannelServer.SelectSubChannelByPageOrResourceGroupIdCount(tcp.CustomerId, keyWord, resourceGroupIdListStr);
                        }
                    }
                    else 
                    {
                        //检索关键字,当前用户可用的组集合，当前可用设备的集合
                        //返回 List<object[]>输出时需装箱
                        if (deviceIdListStr != string.Empty && deviceIdListStr != "")
                        {
                            result = groupChannelServer.SelectChannelByPageOrResourceGroupId
                                                             (startCount, requestCount, keyWord, deviceIdListStr, resourceGroupIdListStr);
                            total = groupChannelServer.SelectChannelByPageOrResourceGroupIdCount(keyWord, deviceIdListStr, resourceGroupIdListStr);
                        }
                    }
                    //通道数据装箱
                    if (result != null)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            object[] data = (object[])result[i];
                            GroupChannelResponse gcr = new GroupChannelResponse();
                            gcr.ChannelId = (int)data[0];
                            gcr.ResourceGroupId = (int)data[1];
                            gcr.ChannelName = (string)data[2];
                            gcr.ChannelNumber = (int)data[3];
                            gcr.DeviceId = (int)data[4];
                            gcr.IsEnable = (int)data[5];
                            gcr.ImagePath = (string)data[6];
                            groupChannelResponseFlag.Add(gcr);
                        }
                    }
                    //填充 需要的 授权字段
                    groupChannelResponseFlag = ConvertGroupChannelResponse(primaryCustomerId, groupChannelResponseFlag);
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "通道获取完成！";
                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，请刷新页面后继续";
                    myLog.WarnFormat("GetChannelByPageOrResourceGroupId方法异常, 组id:{0}, 起始值：{1}，请求数量：{2}, 关键字：{3}",
                                    ex, resourceGroup.ResourceGroupId, startCount, requestCount, keyWord);
                }
            }
            return dto;
        }
        #endregion

        #region 查询分组下所有通道关系(包括子分组下) GetChannelByResourceGroupIdList 2014/12/15
        /// <summary>
        ///  查询分组下所有通道(包括子分组下) GetChannelByResourceGroupIdList Date:2014/12/15
        /// </summary>
        /// <param name="resourceGroup">组节点Id</param>
        /// <param name="customerToken">token</param>
        /// <param name="resourceGroupChannel">返回数据  通道关系集合</param>
        /// <returns></returns>
        public ResponseBaseDto GetChannelByResourceGroupIdList
            (ResourceGroup resourceGroup, string customerToken, ref IList<GroupChannel> groupChannel)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }
            else
            {
                try
                {
                    dto = bllHelper.CheckCustomer(dto, customerToken ,ref tcp);
                    if (dto.Code != 0)
                    {
                        return dto;
                    }
                    //以List<ResourceGroupId>查询
                    List<int> resourceGroupIdFlag = new List<int>();
                    IList<ResourceGroup> allResourceList = new List<ResourceGroup>();
                    GetchildResourceGroup(resourceGroup, ref allResourceList);
                    allResourceList.Add(resourceGroup);
                    for (int i = 0; i < allResourceList.Count; i++)
                    {
                        resourceGroupIdFlag.Add(allResourceList[i].ResourceGroupId);
                    }
                    groupChannel = groupChannelServer.SelectGroupChannelByResourceGroupIdList(resourceGroupIdFlag);

                    //权限
                    Customer customer = new Customer();
                    customer.CustomerId = tcp.CustomerId;
                    IList<Permission> permissionFlag = permissionServer.SelectPermissionByCustomerId(customer);
                    for (int n = 0; n < permissionFlag.Count; n++)
                    {
                        for (int m = 0; m < groupChannel.Count; m++)
                        {
                            //判定此用户里这个通道的IsEnable
                            if (permissionFlag[n].NodeType == (int)PermissionNodeTypeEnum.Channel &&
                                permissionFlag[n].NodeId==groupChannel[m].channel.ChannelId) 
                            {
                                //1表示可以使用， 0表示不可以被使用
                                if (permissionFlag[n].IsEnable == 0) 
                                {
                                    groupChannel.RemoveAt(m);
                                }
                            }
                        }
                    }
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "通道获取完成";
                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，请刷新页面后继续";
                    myLog.WarnFormat("GetChannelByResourceGroupIdList方法异常,分组id:{0}",ex,resourceGroup.ResourceGroupId);
                }
            }
            return dto;
        }
        #endregion

        #region 删除一个分组时删除下面所有分组和通道信息  RemoveResourceGroup 2014/12/15
        /// <summary>
        ///  删除一个分组并移除分组下的所有(虚)通道
        /// </summary>
        /// <param name="resourceGroup">通道分组Id</param>
        public ResponseBaseDto RemoveResourceGroup(ResourceGroup resourceGroup, string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            TokenCacheProperty tcp = new TokenCacheProperty();
            string groupChannelIdList = "";
            //校验token
            OperaterLog oLog = new OperaterLog();
            oLog.Action = "删除分组(所有)";
            UserTokenCache utc = UserTokenCache.GetInstance();
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }
            else
            {
                dto = bllHelper.CheckCustomer(dto, customerToken ,ref tcp);
                if (dto.Code != 0)
                {
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                //查找当前分组下的所有子分组
                IList<ResourceGroup> allResourceList = new List<ResourceGroup>();
                try
                {
                    GetchildResourceGroup(resourceGroup, ref allResourceList);
                    IList<ResourceGroup> rgFlag = resourceGroupServer.SelectResourceGorupByResourceGroupId(resourceGroup);
                    if (rgFlag != null && rgFlag.Count != 0)
                    {
                        allResourceList.Add(rgFlag[0]);
                    }
                    for (int i = 0; i < allResourceList.Count; i++)
                    {
                        ResourceGroup rg = allResourceList[i];//重复命名：rg
                        //判定当前分组下通道
                        IList<GroupChannel> groupChannelFlag =
                            groupChannelServer.SelectGroupChannelByResourceGroupId(rg);
                        for (int j = 0; j < groupChannelFlag.Count; j++)
                        {
                            GroupChannel groupChannel = groupChannelFlag[j];
                            //关联时必须先删除关系表
                            groupChannelServer.DeleteGroupChannel(groupChannel);
                            groupChannelIdList += groupChannel.GroupChannelId+" ";
                        }
                        resourceGroupServer.DeleteResourceGorup(rg);
                    }
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "分组移除完成！";
                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，请刷新页面后继续";
                    myLog.WarnFormat("RemoveResourceGroup方法异常,分组id:{0},分组关系id:{1}", ex, resourceGroup.ResourceGroupId, groupChannelIdList);
                }
            }
            oLog.TargetType = (int)OperaterLogEnum.ResourceGroup;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 删除一个分组下的一个通道（虚） DeleteSingleChannelByChannelId 2014/12/15
        /// <summary>
        /// 删除一个分组下的一个通道（虚） DeleteSingleChannelByChannelId 2014/12/15
        /// </summary>
        /// <param name="resourceGroup">resourceGroup.resourceGroupId</param>
        /// <param name="channel">channel.ChannelId</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto DeleteSingleChannelByChannelId(ResourceGroup resourceGroup,Channel channel, string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            OperaterLog oLog = new OperaterLog();
            TokenCacheProperty tcp = new TokenCacheProperty();
            oLog.Action = "删除通道";
            UserTokenCache utc = UserTokenCache.GetInstance();
            if (utc.IsValid(customerToken) == false)
            {
                dto.Code = (int)CodeEnum.ServerNoToken;
                dto.Message = "用户token已失效，请重新登录后继续";
            }
            else
            {
                try
                {
                    dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                    if (dto.Code != 0)
                    {
                        oLog.Remarks = dto.Message;
                        oLog.Result = dto.Code;
                        bllHelper.AddOperaterLog(oLog, tcp);
                        return dto;
                    }
                    GroupChannel groupChannel = new GroupChannel();
                    groupChannel.channel = new Channel() { ChannelId = channel .ChannelId};
                    groupChannel.resourceGroup = new ResourceGroup() { ResourceGroupId = resourceGroup.ResourceGroupId };
                    IList<GroupChannel> groupChannelFlag = 
                        groupChannelServer.SelectGroupChannelByChannelIdAndResourceGroupId(groupChannel);
                    if (groupChannelFlag == null&& groupChannelFlag.Count<=0)
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "需要删除的通道不存在";
                    }
                    else
                    {
                        groupChannelServer.DeleteGroupChannel(groupChannelFlag[0]);
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "通道" + groupChannelFlag[0].channel.ChannelName + "已被删除";
                    }
                }
                catch(Exception ex)
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "网络异常，删除通道失败！";
                    myLog.ErrorFormat("DeleteSingleChannelByChannelId方法异常,分组Id:{0},通道Id:{1}",ex,resourceGroup.ResourceGroupId,channel.ChannelId);
                }
            }
            oLog.TargetType = (int)OperaterLogEnum.ResourceGroup;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 将数据转换成 GroupChannelResponse 并将中的设备 授权字段 进行填充
        /// <summary>
        ///  将数据转换成 GroupChannelResponse 并将中的设备 授权字段 进行填充
        /// </summary>
        /// <param name="primaryCustomerId"></param>
        /// <param name="channelFlag"></param>
        /// <returns></returns>
        private List<GroupChannelResponse> ConvertGroupChannelResponse(int primaryCustomerId, IList<GroupChannelResponse> groupChannelResponseFlag) 
        {
            List<int> deviceIdFlag = new List<int>();
            List<GroupChannelResponse> tmpGroupChannelResponseList = new List<GroupChannelResponse>();

            if (groupChannelResponseFlag == null || groupChannelResponseFlag.Count == 0) 
            {
                return tmpGroupChannelResponseList;
            }
            for (int i = 0; i < groupChannelResponseFlag.Count; i++)
            {
                deviceIdFlag.Add(groupChannelResponseFlag[i].DeviceId); 
            }
            //去除重复设备Id
            deviceIdFlag=deviceIdFlag.Distinct().ToList();
            //获取出所有通道的所属 设备的授权集合
            IList<Permission> permissionFlag= permissionServer.SelectDeviceAuthorizeByDeviceIdList(primaryCustomerId, deviceIdFlag);
            for (int i = 0; i < groupChannelResponseFlag.Count; i++)
            {
                GroupChannelResponse groupChannelResponse = groupChannelResponseFlag[i];
                GroupChannelResponse gcr = new GroupChannelResponse();
                gcr.ChannelId = groupChannelResponse.ChannelId;
                gcr.ChannelName = groupChannelResponse.ChannelName;
                for (int j = 0; j < permissionFlag.Count; j++)
                {
                    if (permissionFlag[j].NodeType == (int)PermissionNodeTypeEnum.Device
                        && groupChannelResponse.DeviceId == permissionFlag[j].NodeId
                        && permissionFlag[j].IsEnable == 1)
                    {
                        //当用户 是设备 + 通道的设备字段与权限相同 +权限的中的可用状态为1
                        //将这个通道输出时，变为是授权
                        gcr.IsAuthorize =1;
                    }
                }
                IList<UserFavorite> userFavoriteFlag = userFavoriteServer.SelectCustomerByTid(
                      new UserFavorite() { UserFavoriteType = (int)UserFavoriteTypeEnum.Channel, UserFavoriteTypeId = gcr.ChannelId });
                if (userFavoriteFlag != null && userFavoriteFlag.Count == 1)
                {
                    gcr.IsFavorite = 1;
                }
                gcr.ImagePath = groupChannelResponse.ImagePath;
                gcr.IsEnable = groupChannelResponse.IsEnable;
                gcr.ResourceGroupId = groupChannelResponse.ResourceGroupId;
                gcr.DeviceId = groupChannelResponse.DeviceId;
                gcr.ChannelNumber = groupChannelResponse.ChannelNumber;
                tmpGroupChannelResponseList.Add(gcr);
            }
            return tmpGroupChannelResponseList;
        }
        #endregion

        #region 将查询当前用户未分组的通道
        /// <summary>
        /// 将查询当前用户未分组的通道
        /// </summary>
        /// <param name="primaryCustomerId">主用户Id</param>
        /// <param name="deviceIdListStr">设备集合</param>
        /// <param name="channelFlag">ref 返回通道集合</param>
        /// <param name="total">ref 返回总条数</param>
        /// <returns></returns>
        private void GetChannelByNoGroup(int primaryCustomerId, string deviceIdListStr,bool isSubCustomer,
            int startCount,int requestCount,string keyWord, ref IList<Channel> channelFlag, ref int total) 
        {
                if (isSubCustomer == true)
                {
                    //子用户
                    channelFlag = groupChannelServer.SelectSubChannelByNoGroupPage
                                     (primaryCustomerId, startCount, requestCount, keyWord, deviceIdListStr);

                    total = groupChannelServer.SelectSubChannelByNoGroupPageCount(primaryCustomerId, keyWord, deviceIdListStr);
                }
                else
                {
                    //主用户
                    channelFlag = groupChannelServer.SelectChannelByNoGroupPage
                                     (primaryCustomerId, startCount, requestCount, keyWord, deviceIdListStr);

                    total = groupChannelServer.SelectChannelByNoGroupPageCount(primaryCustomerId, keyWord, deviceIdListStr);
                }

        }
        #endregion

    }
}
