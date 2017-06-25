using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Bsr.Cloud.Model;
using Bsr.Cloud.Model.Entities;
using System.ServiceModel.Web;
using Bsr.Cloud.BLogic;

namespace Bsr.Cloud.WebEntry.RestService
{

    #region  GetReceiverPhone
    [DataContract]
    public class GetReceiverPhoneRequestDto : RequestBaseDto
    {
        [DataMember]
        public string ReceiverCellPhone;
    }
    [DataContract]
    public class GetReceiverPhoneResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsUse;
    }
    #endregion  

    #region  GetCustomerName
    [DataContract]
    public class GetCustomerNameRequestDto : RequestBaseDto
    {
        [DataMember]
        public string CustomerName;
    }

    [DataContract]
    public class GetCustomerNameResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsUse;
    }
    #endregion  

    #region  GetReceiverEmail
    [DataContract]
    public class GetReceiverEmailRequestDto : RequestBaseDto
    {
        [DataMember]
        public string ReceiverEmail;
    }
    [DataContract]
    public class GetReceiverEmailResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsUse;
    }
    #endregion  GetReceiverEmail

    #region  SignIn
    [DataContract]
    public class SignInRequestDto : RequestBaseDto
    {
        [DataMember]
        public string LoginName;
        [DataMember]
        public string Password;
        [DataMember]
        public int LoginType; // 登录来自于哪里，见 LoginTypeEnum
        [DataMember]
        public string AgentVersion;//Web端：浏览器类型， 手机端：手机版本
        
    }

    [DataContract]
    public class SignInResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int CustomerId;
        [DataMember]
        public string CustomerName;
        [DataMember]
        public string CustomerToken;
        [DataMember]
        public int SignInType;//登陆类型
    }
    #endregion  SignIn

    #region  TimingCheck 客户端定时请求向服务器发送心跳
    [DataContract]
    public class TimingCheckRequestDto : RequestBaseDto
    {
    }
    [DataContract]
    public class TimingCheckResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    #region  EnableSubCustomer 子用户冻结与解冻
    [DataContract]
    public class EnableSubCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public int SubCustomerId;//被冻结用户
        [DataMember]
        public int IsEnable;     //1解冻 0冻结
    }
    [DataContract]
    public class EnableSubCustomerResponseDto : ResponseBaseDto
    {

    }
    #endregion  EnableCustomer 用户冻结

    #region  AddSubCustomer创建子用户
    [DataContract]
    public class AddSubCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public string CustomerName;
        [DataMember]
        public string Password;
        [DataMember]
        public string ReceiverName;
        [DataMember]
        public string ReceiverEmail;
        [DataMember]
        public string ReceiverCellPhone;
        [DataMember]
        public string AccountTelephone;
        [DataMember]
        public string AccountCompanyAddress;
        [DataMember]
        public string ValidTime;//账号有效期
        [DataMember]
        public string LoginTypes;//允许登陆的集合
        [DataMember]
        public string ImageByteBase64; //图片的二进制数据(BASE64编码)
        [DataMember]
        public string ExtName; //图片的扩展名: jpg 注:没有<.>

    }
    [DataContract]
    public class AddSubCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int CustomerId;
    }
    #endregion  CustomerInsert创建子用户

    #region  AddPrimaryCustomer创建主用户 (注册主账号)
    [DataContract]
    public class AddPrimaryCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public string CustomerName;
        [DataMember]
        public string Password;
        [DataMember]
        public string ReceiverName;
        [DataMember]
        public string ReceiverEmail;
        [DataMember]
        public string ReceiverCellPhone;   
        [DataMember]
        public string AccountIDNumber;
        [DataMember]
        public string AccountTelephone;
        [DataMember]
        public string AccountCompany;
        [DataMember]
        public string AccountCompanyAddress;
        [DataMember]
        public string AccountHomeAddress;
        [DataMember]
        public int AgentType;     //注册类型见 RegisterTypeEnum
        [DataMember]
        public string VerifyKey;  //验证码
    }
    [DataContract]
    public class AddPrimaryCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int CustomerId;
    }
    #endregion  

    #region  CheckServerCustomerToken 校验流媒体，BPserver等服务器需要验证的token
    [DataContract]
    public class CheckServerCustomerTokenRequestDto : RequestBaseDto
    {
        [DataMember]
        public int NodeId;
        [DataMember]
        public int NodeType;//
        [DataMember]
        public string PermissionName;//PermissionNameTypeEnum :Video or Playback
    }
    [DataContract]
    public class CheckServerCustomerTokenResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsUse; //1为可以使用 0为不可用
    }
    #endregion  CheckServerCustomerToken

    #region  SignOut Server端注销
    [DataContract]
    public class SignOutRequestDto : RequestBaseDto
    {
    }
    [DataContract]
    public class SignOutResponseDto : ResponseBaseDto
    {
        
    }
    #endregion  

    #region  GetSubCustomer 获取当前主用户的所有子用户信息
    [DataContract]
    public class GetSubCustomerRequestDto : RequestBaseDto
    {
    }
    [DataContract]
    public class GetSubCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
         public List<CustomerResponse> customerReponseList;
    }

    
    #endregion  

    #region  SearchSubCustomer 检索当前主用户下所有子用户信息
    [DataContract]
    public class SearchSubCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public string KeyWord;
    }
    [DataContract]
    public class SearchSubCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<CustomerResponse> customerReponseList;
    }
    #endregion  

    #region  UpdateSubSafeByPrimary 主用户修改子用户的安全信息
    [DataContract]
    public class UpdateSubSafeByPrimaryRequestDto : RequestBaseDto
    {
        [DataMember]
        public int SubCustomerId;
        [DataMember]
        public string LoginTypes;//允许的登陆类型
        [DataMember]
        public int IsModify;//1修改,0修改
        [DataMember]
        public string ValidTime;//有效期,MinValue为永久有效
    }
    [DataContract]
    public class UpdateSubSafeByPrimaryResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    #region  UpdateSubPassWordByPrimary 主用户重置子用户密码
    [DataContract]
    public class UpdateSubPassWordByPrimaryRequestDto : RequestBaseDto
    {
        [DataMember]
        public int SubCustomerId;
        [DataMember]
        public string OldPassWord;
        [DataMember]
        public string NewPassWord;
    }
    [DataContract]
    public class UpdateSubPassWordByPrimaryResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    #region GetAuthorizeSubCustomer  主用户获取子用户的授权信息
    [DataContract]
    public class GetAuthorizeSubCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public int SubCustomerId;
    }
    [DataContract]
    public class GetAuthorizeSubCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public Model.AuthorizeSubResponse authorizeSubResponse;
    }
    #endregion  

    #region  AuthorizeSubByPrimary 主用户将权限设定给子用户
    [DataContract]
    public class AuthorizeSubByPrimaryRequestDto : RequestBaseDto
    {
        [DataMember]
        public Model.AuthorizeSubResponse authorizeSubResponse;
    }
    [DataContract]
    public class AuthorizeSubByPrimaryResponseDto : ResponseBaseDto
    {

    }
    #endregion  

    #region  UpdateSubCustomer 主用更新子用户的基本信息
    [DataContract]
    public class UpdateSubCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public int SubCustomerId;
        [DataMember]
        public string ReceiverName;
        [DataMember]
        public string ReceiverCellPhone;
        [DataMember]
        public string AccountCompanyAddress;
        [DataMember]
        public string AccountTelephone;
        [DataMember]
        public string ReceiverEmail;
        [DataMember]
        public string ImageByteBase64; // 图片的二进制数据(BASE64编码)
        [DataMember]
        public string ExtName; //图片的扩展名: jpg 注:没有<.>
    }
    [DataContract]
    public class UpdateSubCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public string ImagePath;
    }
    #endregion  

    #region  GetSingleSubCustomer 主用户获取单一的子用户信息
    [DataContract]
    public class GetSingleSubCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public int SubCustomerId;
    }
    [DataContract]
    public class GetSingleSubCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public CustomerResponse customerReponse;
    }
    #endregion  

    #region  GetSelfCustomer 获取当前用户信息
    [DataContract]
    public class GetSelfCustomerRequestDto : RequestBaseDto
    {

    }
    [DataContract]
    public class GetSelfCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public CustomerResponse customerReponse;
    }
    #endregion  

    #region  UpdateSelfCustomer 更新当前用户信息
    [DataContract]
    public class UpdateSelfCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public string ReceiverName;
        [DataMember]
        public string ReceiverCellPhone;
        [DataMember]
        public string AccountCompanyAddress;
        [DataMember]
        public string AccountTelephone;
        [DataMember]
        public string ReceiverEmail;
        [DataMember]
        public string ImageByteBase64; // 图片的二进制数据(BASE64编码)
        [DataMember]
        public string ExtName; //图片的扩展名: jpg 注:没有<.>
    }
    [DataContract]
    public class UpdateSelfCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public string ImagePath;
    }
    #endregion  

    #region  SearchPrimaryCustomer 检索当前前台管理员下的主账号信息
    [DataContract]
    public class SearchPrimaryCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public string KeyWord;
    }
    [DataContract]
    public class SearchPrimaryCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<CustomerResponse> customerReponseList;
    }
    #endregion  

    #region  AddPrimaryCustomerByManagerAccount 由前台管理添加主账号
    [DataContract]
    public class AddPrimaryCustomerByManagerAccountRequestDto : RequestBaseDto
    {
        [DataMember]
        public string CustomerName;
        [DataMember]
        public string Password;
        [DataMember]
        public string ReceiverName;
        [DataMember]
        public string ReceiverEmail;
        [DataMember]
        public string ReceiverCellPhone;   
        [DataMember]
        public string AccountTelephone;
        [DataMember]
        public string AccountCompanyAddress;
        [DataMember]
        public string ValidTime;//账号有效期
        [DataMember]
        public string LoginTypes;//允许登陆的集合
        [DataMember]
        public string ImageByteBase64; // 图片的二进制数据(BASE64编码)
        [DataMember]
        public string ExtName; //图片的扩展名: jpg 注:没有<.>
    }
    [DataContract]
    public class AddPrimaryCustomerByManagerAccountResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int CustomerId;
    }
    #endregion  

    #region  UpdateSelfSafeCustomer 更新当前用户安全信息
    [DataContract]
    public class UpdateSelfSafeCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public string LoginTypes;
    }
    [DataContract]
    public class UpdateSelfSafeCustomerResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    #region  UpdateSelfCustomerPassWord 重置用户的密码
    [DataContract]
    public class UpdateSelfCustomerPassWordRequestDto : RequestBaseDto
    {
        [DataMember]
        public string OldPassWord;
        [DataMember]
        public string NewPassWord;
    }
    [DataContract]
    public class UpdateSelfCustomerPassWordResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    #region  UpdatePrimarySafeByManagerAccount 前台管理员修改主用户的安全信息
    [DataContract]
    public class UpdatePrimarySafeByManagerAccountRequestDto : RequestBaseDto
    {
        [DataMember]
        public int PrimaryCustomerId;
        [DataMember]
        public string LoginTypes;//允许的登陆类型
        [DataMember]
        public int IsModify;//1修改,0修改
        [DataMember]
        public string ValidTime;//有效期,MinValue为永久有效
    }
    [DataContract]
    public class UpdatePrimarySafeByManagerAccountResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    #region  UpdatePrimaryPassWordByManagerAccount 前台管理员重置主用户的密码
    [DataContract]
    public class UpdatePrimaryPassWordByManagerAccountRequestDto : RequestBaseDto
    {
        [DataMember]
        public int PrimaryCustomerId;
        [DataMember]
        public string OldPassWord;
        [DataMember]
        public string NewPassWord;
    }
    [DataContract]
    public class UpdatePrimaryPassWordByManagerAccountResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    #region  GetSinglePrimaryCustomer 获取当前前台管理员下的单一主账号信息
    [DataContract]
    public class GetSinglePrimaryCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public int PrimaryCustomerId;
    }
    [DataContract]
    public class GetSinglePrimaryCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public CustomerResponse customerReponse;
    }
    #endregion  

    #region  AuthorizePrimaryByManagerAccount 管理员账号将设备授权更新给主用户
    [DataContract]
    public class AuthorizePrimaryByManagerAccountRequestDto : RequestBaseDto
    {
        [DataMember]
        public List<Model.AuthorizePrimaryResponse> authorizeCustomerResponseList;
    }
    [DataContract]
    public class AuthorizePrimaryByManagerAccountResponseDto : ResponseBaseDto
    {

    }
    #endregion  

    #region GetAuthorizeOtherPrimary  获取其他主用户所有设备授权信息
    [DataContract]
    public class GetAuthorizeOtherPrimaryRequestDto : RequestBaseDto
    {
        [DataMember]
        public int PrimaryCustomerId;
    }
    [DataContract]
    public class GetAuthorizeOtherPrimaryResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<Model.AuthorizePrimaryResponse> authorizeCustomerResponseList;
    }
    #endregion  

    #region  EnablePrimaryCustomer 前台管理员冻结与解除冻结主用户
    [DataContract]
    public class EnablePrimaryCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public int PrimaryCustomerId;//被冻结用户
        [DataMember]
        public int IsEnable;//1解冻 0冻结
    }
    [DataContract]
    public class EnablePrimaryCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int PrimaryCustomerId;//被冻结用户
        [DataMember]
        public int IsEnable;//1解冻 0冻结
    }
    #endregion

    #region  UpdatePrimaryCustomer 前台管理员修改主用户的基本信息
    [DataContract]
    public class UpdatePrimaryCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public int PrimaryCustomerId;
        [DataMember]
        public string ReceiverName;
        [DataMember]
        public string ReceiverCellPhone;
        [DataMember]
        public string AccountCompanyAddress;
        [DataMember]
        public string AccountTelephone;
        [DataMember]
        public string ReceiverEmail;
        [DataMember]
        public string ImageByteBase64; // 图片的二进制数据(BASE64编码)
        [DataMember]
        public string ExtName; //图片的扩展名: jpg 注:没有<.>
    }
    [DataContract]
    public class UpdatePrimaryCustomerResponseDto : ResponseBaseDto
    {
        [DataMember]
        public string ImagePath;
    }
    #endregion  

    #region   DeletePrimaryCustomer 删除1-n个主用户
    [DataContract]
    public class DeletePrimaryCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public int[] PrimaryCustomerId;
    }
    [DataContract]
    public class DeletePrimaryCustomerResponseDto : ResponseBaseDto
    {

    }
    #endregion  

    #region   DeleteSubCustomer 删除1-n个子用户
    [DataContract]
    public class DeleteSubCustomerRequestDto : RequestBaseDto
    {
        [DataMember]
        public int[] SubCustomer;
    }
    [DataContract]
    public class DeleteSubCustomerResponseDto : ResponseBaseDto
    {

    }
    #endregion  

    #region GetSelfPermission 获取当前用户访问权限
    [DataContract]
    public class GetSelfPermissionRequestDto : RequestBaseDto
    {
        
    }
    [DataContract]
    public class GetSelfPermissionResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<Permission> customerPermission;
    }
    #endregion

    #region   GetCustomerPermission 获取用户的权限类型
    [DataContract]
    public class GetCustomerPermissionRequestDto : RequestBaseDto
    {
    }
    [DataContract]
    public class GetCustomerPermissionResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<CustomerPermissionName> customerPermissionName;
    }
    public class CustomerPermissionName
    {
        public int CustomerPermissionKey;
        public string CustomerPermissionValue;
    }
    #endregion  

    #region   SendRegisterUserWithSum 发送手机验证码(注册)
    [DataContract]
    public class SendRegisterUserWithSumRequestDto : RequestBaseDto
    {
        [DataMember]
        public string PhoneNum;//手机号码
    }
    [DataContract]
    public class SendRegisterUserWithSumResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsValid;  //1有效 0无效
    }
    #endregion  

    #region   CheckRegisterUserWithSum 验证 验证码是否有效（注册）
    [DataContract]
    public class CheckRegisterUserWithSumRequestDto : RequestBaseDto
    {
        [DataMember]
        public string PhoneNum;   //手机号码
        [DataMember]
        public string VerifyKey;  //验证码
    }
    [DataContract]
    public class CheckRegisterUserWithSumResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsValid;       //1有效 0无效
    }
    #endregion  

    #region IsRegisterByLoginName 用户（用户名，手机号，邮箱）是否被注册过
    [DataContract]
    public class IsRegisterByLoginNameRequestDto : RequestBaseDto
    {
        [DataMember]
        public string LoginName;//输入用户名,手机,邮箱
    }
    [DataContract]
    public class IsRegisterByLoginNameResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsRegister;  //1已经注册 0没有注册过
    }
    #endregion  

    #region   SendFindPasswordVerifyKey 发送验证码
    [DataContract]
    public class SendFindPasswordVerifyKeyRequestDto : RequestBaseDto
    {
        [DataMember]
        public string LoginName;//输入的 用户名，手机，邮箱
    }
    [DataContract]
    public class SendFindPasswordVerifyKeyResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsValid;//1有效 0无效
    }
    #endregion  

    #region   CheckFindPasswordVerfyKey 校验验证码
    [DataContract]
    public class CheckFindPasswordVerfyKeyRequestDto : RequestBaseDto
    {
        [DataMember]
        public string LoginName;//输入的 用户名，手机，邮箱
        [DataMember]
        public string VerifyKey;
    }
    [DataContract]
    public class CheckFindPasswordVerfyKeyResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsValid;         //1有效 0无效
        [DataMember]
        public string VerifyToken; //设置token防止用户恶意操作 customerId_VerifyKey
    }
    #endregion  

    #region  UpdateCustomerPassWord 重置用户的密码(找回密码)
    [DataContract]
    public class UpdateCustomerPassWordRequestDto : RequestBaseDto
    {
        [DataMember]
        public string NewPassWord;//重置的密码
        [DataMember]
        public string VerifyToken;//验证token
    }
    [DataContract]
    public class UpdateCustomerPassWordResponseDto : ResponseBaseDto
    {
    }
    #endregion  

    #region   GetCustomerOnlineTotal 获取当前在线用户
    [DataContract]
    public class GetCustomerOnlineTotalRequestDto : RequestBaseDto
    {
    }
    [DataContract]
    public class GetCustomerOnlineTotalResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<TokenCacheProperty> tokenCachePropertyList;  //当前在线用户
    }
    #endregion  
    
    /// <summary>
    ///  用户服务接口
    /// </summary>
    [ServiceContract]
    public interface ICustomer
    {
         //校验用户注册时的用户名是否可用
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetCustomerName", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetCustomerNameResponseDto GetCustomerName(GetCustomerNameRequestDto req);

        //登陆
        [OperationContract]
        [WebInvoke(UriTemplate = "/Login", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SignInResponseDto Login(SignInRequestDto req);
        //校验用户注册时的电话号码是否可用
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReceiverPhone", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetReceiverPhoneResponseDto GetReceiverPhone(GetReceiverPhoneRequestDto req);

        //校验用户注册时的Email是否可用
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReceiverEmail", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetReceiverEmailResponseDto GetReceiverEmail(GetReceiverEmailRequestDto req);

        //定时心跳
        [OperationContract]
        [WebInvoke(UriTemplate = "/Timing", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        TimingCheckResponseDto TimingCheck(TimingCheckRequestDto req);

        //解冻或者冻结子用户
        [OperationContract]
        [WebInvoke(UriTemplate = "/EnableCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EnableSubCustomerResponseDto EnableSubCustomer(EnableSubCustomerRequestDto req);

        //创建子用户
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddSubCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AddSubCustomerResponseDto AddSubCustomer(AddSubCustomerRequestDto req);

        //注册主账号信息
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddPrimaryCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AddPrimaryCustomerResponseDto AddPrimaryCustomer(AddPrimaryCustomerRequestDto req); 

        //校验其他服务器给出的用户Token
        [OperationContract]
        [WebInvoke(UriTemplate = "/CheckServerCustomerToken", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CheckServerCustomerTokenResponseDto CheckServerCustomerToken(CheckServerCustomerTokenRequestDto req);

        //注销
        [OperationContract]
        [WebInvoke(UriTemplate = "/LogOut", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SignOutResponseDto LogOut(SignOutRequestDto req);

        //获取当前主用户的所有子用户信息
        [OperationContract]
        [ServiceKnownType(typeof(CustomerResponse))]
        [WebInvoke(UriTemplate = "/GetSubCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetSubCustomerResponseDto GetSubCustomer(GetSubCustomerRequestDto req);

        //获取当前用户信息
        [OperationContract]
        [ServiceKnownType(typeof(CustomerResponse))]
        [WebInvoke(UriTemplate = "/GetSelfCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetSelfCustomerResponseDto GetSelfCustomer(GetSelfCustomerRequestDto req);

        //更新当前用户信息
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateSelfCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateSelfCustomerResponseDto UpdateSelfCustomer(UpdateSelfCustomerRequestDto req);

        //检索当前台管理员下的主账号信息
        [OperationContract]
        [ServiceKnownType(typeof(CustomerResponse))]
        [WebInvoke(UriTemplate = "/SearchPrimaryCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SearchPrimaryCustomerResponseDto SearchPrimaryCustomer(SearchPrimaryCustomerRequestDto req);


        //前台管理获取单一的主用户信息
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetSinglePrimaryCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetSinglePrimaryCustomerResponseDto GetSinglePrimaryCustomer(GetSinglePrimaryCustomerRequestDto req);


        //由前台管理注册主账号
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddPrimaryCustomerByManagerAccount", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AddPrimaryCustomerByManagerAccountResponseDto AddPrimaryCustomerByManagerAccount(AddPrimaryCustomerByManagerAccountRequestDto req);

        //更新当前用户的安全信息
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateSelfSafeCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateSelfSafeCustomerResponseDto UpdateSelfSafeCustomer(UpdateSelfSafeCustomerRequestDto req);

        //重置当前用户密码
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateSelfCustomerPassWord", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateSelfCustomerPassWordResponseDto UpdateSelfCustomerPassWord(UpdateSelfCustomerPassWordRequestDto req);

        //前台管理员修改主用户的安全信息
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdatePrimarySafeByManagerAccount", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdatePrimarySafeByManagerAccountResponseDto UpdatePrimarySafeByManagerAccount(UpdatePrimarySafeByManagerAccountRequestDto req);

        //前台管理员重置主用户的密码
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdatePrimaryPassWordByManagerAccount", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdatePrimaryPassWordByManagerAccountResponseDto UpdatePrimaryPassWordByManagerAccount(UpdatePrimaryPassWordByManagerAccountRequestDto req);

        //管理员账号将设备授权给主用户
        [OperationContract]
        [WebInvoke(UriTemplate = "/AuthorizePrimaryByManagerAccount", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AuthorizePrimaryByManagerAccountResponseDto AuthorizePrimaryByManagerAccount(AuthorizePrimaryByManagerAccountRequestDto req);

        //获取其他主用户所有设备授权信息
        [OperationContract]
        [ServiceKnownType(typeof(Model.AuthorizePrimaryResponse))]
        [WebInvoke(UriTemplate = "/GetAuthorizeOtherPrimary", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetAuthorizeOtherPrimaryResponseDto GetAuthorizeOtherPrimary(GetAuthorizeOtherPrimaryRequestDto req);

        //前台管理员冻结与解除冻结主用户
        [OperationContract]
        [WebInvoke(UriTemplate = "/EnablePrimaryCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EnablePrimaryCustomerResponseDto EnablePrimaryCustomer(EnablePrimaryCustomerRequestDto req);

        //前台管理员批量删除主用户
        [OperationContract]
        [WebInvoke(UriTemplate = "/DeletePrimaryCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        DeletePrimaryCustomerResponseDto DeletePrimaryCustomer(DeletePrimaryCustomerRequestDto req);

        //主用户删除子用户
        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteSubCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        DeleteSubCustomerResponseDto DeleteSubCustomer(DeleteSubCustomerRequestDto req);

        //前台管理员修改主用户的基本信息
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdatePrimaryCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdatePrimaryCustomerResponseDto UpdatePrimaryCustomer(UpdatePrimaryCustomerRequestDto req);

        //检索当前主用户下的所有子用户
        [OperationContract]
        [ServiceKnownType(typeof(CustomerResponse))]
        [WebInvoke(UriTemplate = "/SearchSubCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SearchSubCustomerResponseDto SearchSubCustomer(SearchSubCustomerRequestDto req);

        //修改主用户下的子用户的安全信息
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateSubSafeByPrimary", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateSubSafeByPrimaryResponseDto UpdateSubSafeByPrimary(UpdateSubSafeByPrimaryRequestDto req);

        //主用户重置子用户密码
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateSubPassWordByPrimary", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateSubPassWordByPrimaryResponseDto UpdateSubPassWordByPrimary(UpdateSubPassWordByPrimaryRequestDto req);

        //主用户获取子用户的授权信息
        [OperationContract]
        [ServiceKnownType(typeof(Model.AuthorizeSubResponse))]
        [WebInvoke(UriTemplate = "/GetAuthorizeSubCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetAuthorizeSubCustomerResponseDto GetAuthorizeSubCustomer(GetAuthorizeSubCustomerRequestDto req);

        //主用户将权限设定给子用户
        [OperationContract]
        [WebInvoke(UriTemplate = "/AuthorizeSubByPrimary", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AuthorizeSubByPrimaryResponseDto AuthorizeSubByPrimary(AuthorizeSubByPrimaryRequestDto req);

        //主用更新子用户的基本信息
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateSubCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateSubCustomerResponseDto UpdateSubCustomer(UpdateSubCustomerRequestDto req);

        //获取用户的权限类型
        [OperationContract]
        [ServiceKnownType(typeof(CustomerPermissionName))]
        [WebInvoke(UriTemplate = "/GetCustomerPermission", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetCustomerPermissionResponseDto GetCustomerPermission(GetCustomerPermissionRequestDto req);

        //发送一个临时的检验码至手机
        [OperationContract]
        [WebInvoke(UriTemplate = "/SendRegisterUserWithSum", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SendRegisterUserWithSumResponseDto SendRegisterUserWithSum(SendRegisterUserWithSumRequestDto req);

        //验证一个检验码是否有效
        [OperationContract]
        [WebInvoke(UriTemplate = "/CheckRegisterUserWithSum", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CheckRegisterUserWithSumResponseDto CheckRegisterUserWithSum(CheckRegisterUserWithSumRequestDto req);

        //用户找回时需要的更新方法
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateCustomerPassWord", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateCustomerPassWordResponseDto UpdateCustomerPassWord(UpdateCustomerPassWordRequestDto req);

        //用户（用户名，手机号，邮箱）是否被注册过(找回密码)
        [OperationContract]
        [WebInvoke(UriTemplate = "/IsRegisterByLoginName", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IsRegisterByLoginNameResponseDto IsRegisterByLoginName(IsRegisterByLoginNameRequestDto req);

        //发送验证码（用户名，手机或者邮箱）(找回密码)
        [OperationContract]
        [WebInvoke(UriTemplate = "/SendFindPasswordVerifyKey", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SendFindPasswordVerifyKeyResponseDto SendVerifyKey(SendFindPasswordVerifyKeyRequestDto req);

        //验证验证码(找回密码)
        [OperationContract]
        [WebInvoke(UriTemplate = "/CheckFindPasswordVerfyKey", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CheckFindPasswordVerfyKeyResponseDto CheckFindPasswordVerfyKey(CheckFindPasswordVerfyKeyRequestDto req);

        //获取在线用户
        [OperationContract]
        [ServiceKnownType(typeof(TokenCacheProperty))]
        [WebInvoke(UriTemplate = "/GetCustomerOnlineTotal", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetCustomerOnlineTotalResponseDto GetCustomerOnlineTotal(GetCustomerOnlineTotalRequestDto req);

        //主用户获取单一的子用户信息
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetSingleSubCustomer", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetSingleSubCustomerResponseDto GetSingleSubCustomer(GetSingleSubCustomerRequestDto req);

        //获取当前用户访问权限
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetSelfPermission", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetSelfPermissionResponseDto GetSelfPermission(GetSelfPermissionRequestDto req);
    }
}
