using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Bsr.Cloud.Model;
using Bsr.Cloud.Model.Entities;
using System.ServiceModel.Web;

namespace Bsr.Cloud.WebEntry.RestService
{
    #region 添加一个分组  AddGroupByName
    [DataContract]
    public class AddGroupByNameRequestDto : RequestBaseDto
    {
        [DataMember]
        public string ResourceGroupName;//分组名
        [DataMember]
        public int ParentResourceGroupId;//上一级分组id
    }
    [DataContract]
    public class AddGroupByNameResponseDto : ResponseBaseDto
    {
      
    }
    #endregion  AddGroupByName

    #region 添加一个通道  AddChannel
    [DataContract]
    public class AddChannelRequestDto : RequestBaseDto
    {
        [DataMember]
        public string ChannelName;//通道名称
        [DataMember]
        public string ChannelNumber;//通道编号
        [DataMember]
        public int DeviceId;//通道属于哪个设备的
    }
    [DataContract]
    public class AddChannelResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int ChannelId;//通道名称
        [DataMember]
        public string ChannelName;//通道名称
        [DataMember]
        public string ChannelNumber;//通道编号
        [DataMember]
        public int DeviceId;//通道属于哪个设备的
    }
    #endregion  AddRGourpByName

    #region 删除一个组下的单个通道DeleteGroupChanne
    [DataContract]
    public class DeleteGroupChannelRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ChannelId;//通道Id
        [DataMember]
        public int ResourceGroupId;//分组Id
    }
    [DataContract]
    public class DeleteGroupChannelResponseDto : ResponseBaseDto
    {
    }
    #endregion  删除一个组下的通道DeleteGroupChanne

    #region 查询分组  GetResourceGroup
    [DataContract]
    public class GetResourceGroupRequestDto : RequestBaseDto
    {

    }
    [DataContract]
    public class GetResourceGroupResponseDto : ResponseBaseDto
    {
      [DataMember]
      public List<ResourceGroupResponse> resourceGroupList;
    }
    #endregion  SelectResourceGroup

    #region 查询通道  GetChannel
    [DataContract]
    public class GetChannelRequestDto : RequestBaseDto
    {

    }
    [DataContract]
    public class GetChannelResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<Bsr.Cloud.Model.Entities.Channel> channelList ;
    }
    #endregion  

    #region 移动分组   MoveResourceGroup
    [DataContract]
    public class MoveResourceGroupRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ResourceGroupId;//需要移动的分组
        [DataMember]
        public int ParentResourceGroupId;//移动父节点位置
    }
    [DataContract]
    public class MoveResourceGroupResponseDto : ResponseBaseDto
    {
    }
    #endregion  MoveResourceGroup

    #region 更新分组名   UpdateResourceGroupName
    [DataContract]
    public class UpdateResourceGroupNameRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ResourceGroupId;//需要更新的分组Id
        [DataMember]
        public string ResourceGroupName;//需要更新的分组名称
    }
    [DataContract]
    public class UpdateResourceGroupNameResponseDto : ResponseBaseDto
    {
    }
    #endregion  MoveResourceGroup

    #region 删除分组   DeleteResourceGroup
    [DataContract]
    public class DeleteResourceGroupRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ResourceGroupId;//需要删除的分组
    }
    [DataContract]
    public class DeleteResourceGroupResponseDto : ResponseBaseDto
    {
    }
    #endregion  MoveResourceGroup

    #region 查询当前分组下的所有通道集合  GetChannelByResourceGroupIdList
    [DataContract]
    public class GetChannelByResourceGroupIdListRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ResourceGroupId;//分组Id
    }
    [DataContract]
    public class GetChannelByResourceGroupIdListResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<GroupChannelResponse> responseGroupChannelList;//简化的GroupChannel
     }

    #endregion  

    #region  移动通道（分组）MoveChannelListByResourceGroupId
    [DataContract]
    public class MoveChannelListByResourceGroupIdRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ResourceGroupId;//需要移动至的分组
        [DataMember]
        public int[] Channel;//需要移动的通道Id集合
    }
    [DataContract]
    public class MoveChannelListByResourceGroupIdResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    /// <summary>
    ///  分组服务接口
    /// </summary>
    [ServiceContract]
    public interface IResourceGroup
    {
        //添加一个通道的分组
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddGroupByName", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AddGroupByNameResponseDto AddGroupByName(AddGroupByNameRequestDto req);

        //添加一个通道到分组
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddChannel", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AddChannelResponseDto AddChannel(AddChannelRequestDto req);

        //删除一个组下的单个通道
        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteGroupChannel", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        DeleteGroupChannelResponseDto DeleteGroupChannel(DeleteGroupChannelRequestDto req);

        //查询用户分组
        [OperationContract]
        [ServiceKnownType(typeof(Bsr.Cloud.Model.Entities.ResourceGroup))]
        [WebInvoke(UriTemplate = "/GetResourceGroup", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetResourceGroupResponseDto GetResourceGroup(GetResourceGroupRequestDto req);

        //查询用户设备通道
        [OperationContract]
        [ServiceKnownType(typeof(Bsr.Cloud.Model.Entities.ResourceGroup))]
        [WebInvoke(UriTemplate = "/GetChannel", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetChannelResponseDto GetChannel(GetChannelRequestDto req);

         //移动用户分组
        [OperationContract]
        [WebInvoke(UriTemplate = "/MoveResourceGroup", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        MoveResourceGroupResponseDto MoveResourceGroup(MoveResourceGroupRequestDto req);

        //更新分组名
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateResourceGroupName", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateResourceGroupNameResponseDto UpdateResourceGroupName(UpdateResourceGroupNameRequestDto req);

        //删除分组
        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteResourceGroup", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        DeleteResourceGroupResponseDto DeleteResourceGroup(DeleteResourceGroupRequestDto req);

        //查询通道根据1-n个分组Id
        [OperationContract]
        [ServiceKnownType(typeof(Bsr.Cloud.Model.Entities.ResourceGroup))]
        [WebInvoke(UriTemplate = "/GetChannelByResourceGroupIdList", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetChannelByResourceGroupIdListResponseDto GetChannelByResourceGroupIdList(GetChannelByResourceGroupIdListRequestDto req);


        //移动通道集合
        [OperationContract]
        [WebInvoke(UriTemplate = "/MoveChannelListByResourceGroupId", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        MoveChannelListByResourceGroupIdResponseDto MoveChannelListByResourceGroupId(MoveChannelListByResourceGroupIdRequestDto req);
    }
}
