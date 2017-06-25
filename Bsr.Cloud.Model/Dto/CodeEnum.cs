using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model
{
    public enum CodeEnum
    {
        Success = 0,       // 成功
        NoUser,            // 没有找到指定的用户
        InvalidPassword,   // 用户使用的密码无效
        ApplicationErr,    // 应用程序错误
        ReqNoToken,        // 在请求报文中没有token
        ServerNoToken,     // 在服务器缓存中没有该token
        TokenTimeOut,      // 该token已超时
        NoHardWare,        // 无设备或没有找到该设备
        NoAuthorization,   // 没有权限操作
        NoComplete,        // 数据提交不完整
        NoData,            // 没有数据
        TooManyMsg,        // 请求短信过于频繁
        InvalidMsgSum,     // 无效的短信检验码
        InvalidNoCellPhone,// 用户使用的手机号无效
        InvalidNoEmail,    // 用户的使用的邮箱无效
        ExistFavorite,     // 已存在的收藏
    }


}
