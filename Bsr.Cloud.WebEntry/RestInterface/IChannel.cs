using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Bsr.Cloud.Model;
using Bsr.Cloud.Model.Entities;
using System.ServiceModel.Web;
using System.IO;

namespace Bsr.Cloud.WebEntry.RestService
{

    #region 更新通道名称  UpdateChannelName
    [DataContract]
    public class UpdateChannelNameRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ChannelId;
        [DataMember]
        public string NewChannelName;
    }
    [DataContract]
    public class UpdateChannelNameResponseDto : ResponseBaseDto
    {

    }
    #endregion  

    #region 更新通道图片  UpLoadChannelImage
    [DataContract]
    public class UpLoadChannelImageRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ChannelId;
        [DataMember]
        public string ImageByteBase64; // 图片的二进制数据(BASE64编码)
        [DataMember]
        public string extName; // 图片的扩展名: jpg
    }
    [DataContract]
    public class UpLoadChannelImageResponseDto : ResponseBaseDto
    {
        [DataMember]
        public string ImagePath; // 图片的URL地址后缀
    }
    #endregion UpLoadChannelImage

    #region 通道分页  GetChannelByPage
    [DataContract]
    public class GetChannelByPageRequestDto : RequestBaseDto
    {
        [DataMember]
        public int RequestCount;//请求条数
        [DataMember]
        public int StartCount;//请求起始条数
    }
    [DataContract]
    public class GetChannelByPageResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int Total;
        [DataMember]
        public List<GroupChannelResponse> groupChannelResponseList;
    }
    #endregion 

    #region 检索通道并分页 SearchChannelByPage
    [DataContract]
    public class SearchChannelByPageRequestDto : RequestBaseDto
    {
        [DataMember]
        public string KeyWord;     //关键字
        [DataMember]
        public int RequestCount;   //请求条数
        [DataMember]
        public int StartCount;     //请求起始条数
        [DataMember]
        public int IsGroup;        //是否加入组节点查询 1.是 0否
        [DataMember]
        public int ResouceGroupId; //分组节点Id
    }
    [DataContract]
    public class SearchChannelByPageResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int Total;
        [DataMember]
        public List<GroupChannelResponse> groupChannelResponseList;
    }
    #endregion 

    #region 查询单条通道信息  GetSingleChannel
    [DataContract]
    public class GetSingleChannelRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ChannelId;
    }
    [DataContract]
    public class GetSingleChannelResponseDto : ResponseBaseDto
    {
        [DataMember]
        public GroupChannelResponse groupChannel;
    }
    #endregion  

    #region 更新通道是否启用  EnableChannel
    [DataContract]
    public class EnableChannelRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ChannelId;
        [DataMember]
        public int IsEnable;
    }
    [DataContract]
    public class EnableChannelResponseDto : ResponseBaseDto
    {

    }
    #endregion  

    #region 设置通道码流  UpdateChannelEncoderInfo
    [DataContract]
    public class UpdateChannelEncoderInfoRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ChannelId;
        [DataMember]
        public int StreamType;//1高清 2均衡 3流畅
    }
    [DataContract]
    public class UpdateChannelEncoderInfoResponseDto : ResponseBaseDto
    {

    }
    #endregion  

    #region 查询所有通道的权限名 GetChanenlPermission
    [DataContract]
    public class GetChanenlPermissionRequestDto : RequestBaseDto
    {
    }
    [DataContract]
    public class GetChanenlPermissionResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<ChannelPermissionName> channelPermissionName;
    }
    public class ChannelPermissionName 
    {
        public int ChannelPermissionKey;
        public string ChannelPermissionValue;
    }
    #endregion  

    /// <summary>
    /// 通道服务接口
    /// </summary>
    [ServiceContract]
    public interface IChannel
    {

        //更新通道名称
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateChannelName", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateChannelNameResponseDto UpdateChannelName(UpdateChannelNameRequestDto req);

        //更新通道是否启用
        [OperationContract]
        [WebInvoke(UriTemplate = "/EnableChannel", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EnableChannelResponseDto EnableChannel(EnableChannelRequestDto req);

        //更新通道图片
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpLoadChannelImage", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpLoadChannelImageResponseDto UpLoadChannelImage(UpLoadChannelImageRequestDto req);

        //通道分页
        [OperationContract]
        [ServiceKnownType(typeof(Bsr.Cloud.Model.Entities.Channel))]
        [WebInvoke(UriTemplate = "/GetChannelByPage", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetChannelByPageResponseDto GetChannelByPage(GetChannelByPageRequestDto req);

        //查询单一通道
        [OperationContract]
        [ServiceKnownType(typeof(GroupChannelResponse))]
        [WebInvoke(UriTemplate = "/GetSingleChannel", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetSingleChannelResponseDto GetSingleChannel(GetSingleChannelRequestDto req);

        //查询所有通道的权限名
        [OperationContract]
        [ServiceKnownType(typeof(ChannelPermissionName))]
        [WebInvoke(UriTemplate = "/GetChanenlPermissionName", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetChanenlPermissionResponseDto GetChanenlPermissionName(GetChanenlPermissionRequestDto req);

        //检索通道并分页
        [OperationContract]
        [ServiceKnownType(typeof(GroupChannelResponse))]
        [WebInvoke(UriTemplate = "/SearchChannelByPage", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SearchChannelByPageResponseDto SearchChannelByPage(SearchChannelByPageRequestDto req);

        //设置通道码流
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateChannelEncoderInfo", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateChannelEncoderInfoResponseDto UpdateChannelEncoderInfo(UpdateChannelEncoderInfoRequestDto req);
    }


}
