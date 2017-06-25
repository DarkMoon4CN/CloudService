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
    #region 添加收藏  AddUserFavorite
    [DataContract]
    public class AddUserFavoriteRequestDto : RequestBaseDto
    {
        [DataMember]
        public int NodeType;     // 1表示通道,2表示事件,3表示录像文件
        [DataMember]
        public int NodeId;       //对应channel,events的id
        [DataMember]
        public string AliasName; //收藏显示用的别名
    }
    [DataContract]
    public class AddUserFavoriteResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int UserFavoriteId;
    }
    #endregion  

    #region 删除收藏  DeleteUserFavoriteByTid
    [DataContract]
    public class DeleteUserFavoriteByTidRequestDto : RequestBaseDto
    {
        [DataMember]
        public int NodeType;      // 1表示通道,2表示事件,3表示录像文件
        [DataMember]
        public int[] NodeId;      // 需要删除多个收藏的NodeId
    }
    [DataContract]
    public class DeleteUserFavoriteByTidResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    #region 删除收藏  DeleteUserFavorite
    [DataContract]
    public class DeleteUserFavoriteRequestDto : RequestBaseDto
    {
        [DataMember]
        public int[] UserFavoriteId;     //收藏Id集合
    }
    [DataContract]
    public class DeleteUserFavoriteResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    #region 分页获取收藏信息  GetUserFavoriteByPage
    [DataContract]
    public class GetUserFavoriteByPageRequestDto : RequestBaseDto
    {
        [DataMember]
        public int RequestCount;//请求条数
        [DataMember]
        public int StartCount;//请求起始条数
    }
    [DataContract]
    public class GetUserFavoriteByPageResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int Total;
        [DataMember]
        public List<UserFavoriteResponse> userFavoriteList;
    }
    #endregion  


    /// <summary>
    /// 收藏服务接口
    /// </summary>
    [ServiceContract]
    public interface IUserFavorite
    {
        //添加收藏接口
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddUserFavorite", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AddUserFavoriteResponseDto AddUserFavorite(AddUserFavoriteRequestDto req);

        //删除1-n个收藏 以UserFavoriteId
        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteUserFavorite", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        DeleteUserFavoriteResponseDto DeleteUserFavorite(DeleteUserFavoriteRequestDto req);

        //删除1-n个收藏 以NodeId
        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteUserFavoriteByTid", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        DeleteUserFavoriteByTidResponseDto DeleteUserFavoriteByTid(DeleteUserFavoriteByTidRequestDto req);

        //分页获取收藏信息
        [OperationContract]
        [ServiceKnownType(typeof(Model.Entities.UserFavorite))]
        [WebInvoke(UriTemplate = "/GetUserFavoriteByPage", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetUserFavoriteByPageResponseDto GetUserFavoriteByPage(GetUserFavoriteByPageRequestDto req);
    }
}
