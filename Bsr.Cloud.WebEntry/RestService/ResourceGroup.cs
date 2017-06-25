using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bsr.Cloud.BLogic;
using System.ServiceModel.Web;
using System.ServiceModel;
using Bsr.Core.Hibernate.WCF;
using Bsr.Cloud.Core;
using Bsr.Cloud.Model;
using Bsr.Cloud.Model.Entities;
using Bsr.Cloud.BLogic.BLL;

namespace Bsr.Cloud.WebEntry.RestService
{
    [NHInstanceContext]
    public class ResourceGroup : IResourceGroup
    {
        static private ILogger myLog = new Logger<Customer>();//日志全局
        ResourceGroupBLL resourceGroupBLL = new ResourceGroupBLL();
        UserTokenCache userTokenCache = UserTokenCache.GetInstance();//缓存全局

         //添加分组
        public AddGroupByNameResponseDto AddGroupByName(AddGroupByNameRequestDto req)
        {
            AddGroupByNameResponseDto arb = new AddGroupByNameResponseDto();
            //收集信息 :BstarCloud-User-Token
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                arb.Code = (int)CodeEnum.ReqNoToken;
                arb.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //收集信息 pid:用户的父级节点Id,rgName:分组名称
                Bsr.Cloud.Model.Entities.ResourceGroup res =
                    new Model.Entities.ResourceGroup()
                    {
                        ResourceGroupName = req.ResourceGroupName
                      , ParentResourceGroupId = req.ParentResourceGroupId
                    };
                //传递数据 token + 分组名 + 分组父节点
                ResponseBaseDto dto = resourceGroupBLL.AddResourceGroupByName(res, customerToken);
                arb.Code = dto.Code;
                arb.Message = dto.Message;
            }
            
            return arb;
        }

         //添加通道（暂时不使用）
        public AddChannelResponseDto AddChannel(AddChannelRequestDto req)
        {
            //暂时不使用
            AddChannelResponseDto ard = new AddChannelResponseDto();
            //收集信息:BstarCloud-User-Token
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                ard.Code = (int)CodeEnum.ReqNoToken;
                ard.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //收集信息:Channel对象
                Bsr.Cloud.Model.Entities.Channel channel = new Bsr.Cloud.Model.Entities.Channel();
                channel.ChannelName = req.ChannelName;
                channel.ChannelNumber = Convert.ToInt32(req.ChannelNumber);
                channel.DeviceId = Convert.ToInt32(req.DeviceId);
                //设置返回通道Id 为0
                int channelId = 0;
                ResponseBaseDto dto = resourceGroupBLL.AddChannel(channel, customerToken, ref channelId);
                ard.Code = dto.Code;
                ard.Message = dto.Message;
                ard.ChannelId = channelId;
                ard.ChannelName = req.ChannelName;
                ard.ChannelNumber = req.ChannelNumber;
                ard.DeviceId = req.DeviceId;
            }
            return ard;
        }

        //删除一个分组下的一个通道
        public DeleteGroupChannelResponseDto DeleteGroupChannel(DeleteGroupChannelRequestDto req)
        {
            DeleteGroupChannelResponseDto dcr = new DeleteGroupChannelResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                dcr.Code = (int)CodeEnum.ReqNoToken;
                dcr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
              Bsr.Cloud.Model.Entities.Customer customer=new Model.Entities.Customer();
              Bsr.Cloud.Model.Entities.Channel channel = new Bsr.Cloud.Model.Entities.Channel();
              Bsr.Cloud.Model.Entities.ResourceGroup resourceGroup = new Bsr.Cloud.Model.Entities.ResourceGroup();
              channel.ChannelId=req.ChannelId;
              resourceGroup.ResourceGroupId = req.ResourceGroupId;
              ResponseBaseDto dto = resourceGroupBLL.DeleteSingleChannelByChannelId(resourceGroup,channel, customerToken);
              dcr.Code = dto.Code;
              dcr.Message = dto.Message;
            }
            return dcr;
        }

        //查询当前用户的分组
        public GetResourceGroupResponseDto GetResourceGroup(GetResourceGroupRequestDto req)
        {
            GetResourceGroupResponseDto srgr = new GetResourceGroupResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                srgr.Code = (int)CodeEnum.ReqNoToken;
                srgr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {

                IList<ResourceGroupResponse> resourceGroupResponse = new List<ResourceGroupResponse>();
                ResponseBaseDto dto=
                  resourceGroupBLL.GetGroupByCustomerId(customerToken, ref resourceGroupResponse);
                srgr.Code = dto.Code;
                srgr.Message = dto.Message;
                srgr.resourceGroupList = (List<ResourceGroupResponse>)resourceGroupResponse;
            }
            
            return srgr;
        }

        //查询当前用的通道
        public GetChannelResponseDto GetChannel(GetChannelRequestDto req)
        {
            GetChannelResponseDto scr = new GetChannelResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                scr.Code = (int)CodeEnum.ReqNoToken;
                scr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                IList<Bsr.Cloud.Model.Entities.Channel> channel = null;
                ResponseBaseDto dto =
                  resourceGroupBLL.GetChannelByCustomerId(customerToken,ref channel);
                scr.Code = dto.Code;
                scr.Message = dto.Message;
                scr.channelList = (List<Bsr.Cloud.Model.Entities.Channel>)channel;
            }
            return scr;
        }

        //移动分组
        public MoveResourceGroupResponseDto MoveResourceGroup(MoveResourceGroupRequestDto req) 
        {
            MoveResourceGroupResponseDto mrgr = new MoveResourceGroupResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                mrgr.Code = (int)CodeEnum.ReqNoToken;
                mrgr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //code
                Bsr.Cloud.Model.Entities.ResourceGroup resourceGroup = new Model.Entities.ResourceGroup();
                resourceGroup.ResourceGroupId = req.ResourceGroupId;
                resourceGroup.ParentResourceGroupId = req.ParentResourceGroupId;
                ResponseBaseDto dto= resourceGroupBLL.UpdateResourceGroupByParentId(resourceGroup, customerToken);
                mrgr.Code = dto.Code;
                mrgr.Message = dto.Message;
            }
            return mrgr;
        }

        //更新分组名称
        public UpdateResourceGroupNameResponseDto UpdateResourceGroupName(UpdateResourceGroupNameRequestDto req)
        {
            UpdateResourceGroupNameResponseDto urgn = new UpdateResourceGroupNameResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                urgn.Code = (int)CodeEnum.ReqNoToken;
                urgn.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //code
                Bsr.Cloud.Model.Entities.ResourceGroup resourceGroup = new Model.Entities.ResourceGroup();
                resourceGroup.ResourceGroupId = req.ResourceGroupId;
                resourceGroup.ResourceGroupName = req.ResourceGroupName;
                ResponseBaseDto dto = resourceGroupBLL.UpdateResourceGorupByName(resourceGroup, customerToken);
                urgn.Code = dto.Code;
                urgn.Message = dto.Message;
            }
            return urgn;
        }

        //删除一个分组且删除分组下的通道关系
        public DeleteResourceGroupResponseDto DeleteResourceGroup(DeleteResourceGroupRequestDto req)
        {
            DeleteResourceGroupResponseDto ddr = new DeleteResourceGroupResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                ddr.Code = (int)CodeEnum.ReqNoToken;
                ddr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //code
                Bsr.Cloud.Model.Entities.ResourceGroup resourceGroup = new Model.Entities.ResourceGroup();
                resourceGroup.ResourceGroupId = req.ResourceGroupId;
                ResponseBaseDto dto = resourceGroupBLL.RemoveResourceGroup(resourceGroup, customerToken);
                ddr.Code = dto.Code;
                ddr.Message = dto.Message;
            }
            return ddr;
        }

        //以分组集合查询通道
        public GetChannelByResourceGroupIdListResponseDto GetChannelByResourceGroupIdList(GetChannelByResourceGroupIdListRequestDto req) 
        {
            GetChannelByResourceGroupIdListResponseDto gbrg = new GetChannelByResourceGroupIdListResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gbrg.Code = (int)CodeEnum.ReqNoToken;
                gbrg.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Bsr.Cloud.Model.Entities.ResourceGroup resourceGroup = new Model.Entities.ResourceGroup();
                resourceGroup.ResourceGroupId = req.ResourceGroupId;
                IList<Bsr.Cloud.Model.Entities.GroupChannel> groupChannel = new List<Bsr.Cloud.Model.Entities.GroupChannel>();
                ResponseBaseDto dto = resourceGroupBLL.GetChannelByResourceGroupIdList(resourceGroup, customerToken, ref groupChannel);
                gbrg.Code=dto.Code;
                gbrg.Message = dto.Message;
                List<GroupChannelResponse> responseGroupChannelFlag = new List<GroupChannelResponse>();
                for (int i = 0; i < groupChannel.Count; i++)
                {
                    GroupChannelResponse responseGroupChannel = new GroupChannelResponse();
                    responseGroupChannel.ChannelId = groupChannel[i].channel.ChannelId;
                    responseGroupChannel.ChannelName = groupChannel[i].channel.ChannelName;
                    responseGroupChannel.ChannelNumber = groupChannel[i].channel.ChannelNumber;
                    responseGroupChannel.DeviceId = groupChannel[i].channel.DeviceId;
                    responseGroupChannel.ResourceGroupId = groupChannel[i].resourceGroup.ResourceGroupId;
                    responseGroupChannel.IsEnable = groupChannel[i].channel.IsEnable;
                    responseGroupChannel.ImagePath += @"channelImage\" + groupChannel[i].channel.ImagePath;

                    responseGroupChannelFlag.Add(responseGroupChannel);
                }
                gbrg.responseGroupChannelList = responseGroupChannelFlag;
            }
            return gbrg;
        }

        //以通道集合移动至一个分组内
        public MoveChannelListByResourceGroupIdResponseDto MoveChannelListByResourceGroupId(MoveChannelListByResourceGroupIdRequestDto req) 
        {
            MoveChannelListByResourceGroupIdResponseDto mcbr = new MoveChannelListByResourceGroupIdResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                mcbr.Code = (int)CodeEnum.ReqNoToken;
                mcbr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Model.Entities.ResourceGroup resourceGroup=new Model.Entities.ResourceGroup();
                resourceGroup.ResourceGroupId=req.ResourceGroupId;
                ResponseBaseDto dto =
                    resourceGroupBLL.UpdateChannelListByResourceGroupId(resourceGroup, req.Channel, customerToken);
                mcbr.Code = dto.Code;
                mcbr.Message = dto.Message;
            }
            return mcbr;
        }
        
    }
}