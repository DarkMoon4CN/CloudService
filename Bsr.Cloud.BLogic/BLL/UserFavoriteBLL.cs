using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Core;
using Bsr.Cloud.Model;
using Bsr.Cloud.Model.Entities;

namespace Bsr.Cloud.BLogic.BLL
{
    public class UserFavoriteBLL
    {
        UserFavoriteServer userFavoriteServer = UserFavoriteServer.GetInstance();
        ChannelServer channelServer = ChannelServer.GetInstance();
        static private ILogger myLog = new Logger<ResourceGroupBLL>();
        public BLLHelper bllHelper = new BLLHelper();
        #region 添加收藏
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="userFavorite">收藏实体对象</param>
        /// <param name="customerToken">token</param>
        /// <param name="userFavoriteId">ref 已完成添加的收藏Id</param>
        /// <returns></returns>
        public ResponseBaseDto AddUserFavorite(UserFavorite userFavorite, string customerToken, ref int userFavoriteId) 
        {
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            OperaterLog oLog = new OperaterLog();
            oLog.Action = "用户添加收藏";
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token失效";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
              
               userFavorite.CustomerId = tcp.CustomerId;
               userFavorite.FavoriteTime = DateTime.Now;
               switch (userFavorite.UserFavoriteType)
               {
                   case 1: oLog.Remarks = "通道,通道Id："; break;
                   case 2: oLog.Remarks = "事件,事件Id："; break;
                   case 3: oLog.Remarks = "视频，视频文件Id:"; break;
               }
               oLog.Remarks += userFavorite.UserFavoriteId;
               //判定添加收藏是否已经存在
               IList<UserFavorite> userFavoriteFlag=userFavoriteServer.SelectCustomerByTid(userFavorite);
               if (userFavoriteFlag != null && userFavoriteFlag.Count > 0) 
               {
                   dto.Code = (int)CodeEnum.ExistFavorite;
                   dto.Message = "收藏已经存在！";
                   return dto;
               }
               //添加用户收藏
               userFavoriteId=userFavoriteServer.InsertUserFavorite(userFavorite);
               if (userFavoriteId > 0)
               {
                   dto.Code = (int)CodeEnum.Success;
                   dto.Message = "已加入收藏！";
                   oLog.Remarks += dto.Message;
               }
               else 
               {
                   dto.Code = (int)CodeEnum.ApplicationErr;
                   dto.Message = "加入收藏失败！";
                   oLog.Remarks += dto.Message;
               }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,加入收藏失败！";
                myLog.ErrorFormat("AddUserFavorite方法异常,用户：{0},UserFavoriteType:{1},UserFavoriteTypeId:{2}", 
                    ex, userFavorite.CustomerId, userFavorite.UserFavoriteType,userFavorite.UserFavoriteTypeId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Favorite;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            return dto;
        }
        #endregion

        #region 删除1-n个收藏 以userFavoriteId
        /// <summary>
        ///  删除1-n个收藏 以userFavoriteId
        /// </summary>
        /// <param name="userFavoriteIdList">收藏Id</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto DeleteUserFavorite(int[] userFavoriteIdList, string customerToken) 
        {
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            OperaterLog oLog = new OperaterLog();
            oLog.Action = "用户删除收藏";
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token失效";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
                string userFavoriteIdListStr = "";
                for (int i = 0; i < userFavoriteIdList.Length; i++)
                {
                    if (i != userFavoriteIdList.Length - 1)
                    {
                        userFavoriteIdListStr += userFavoriteIdList[i] + ",";
                    }
                    else
                    {
                        userFavoriteIdListStr += userFavoriteIdList[i];
                    }
                }
                userFavoriteServer.DeleteUserFavorite(tcp.CustomerId,userFavoriteIdListStr);
                oLog.Remarks=tcp.CustomerName+"批量删除收藏 收藏Id:"+ userFavoriteIdListStr;
                oLog.TargetType = (int)OperaterLogEnum.Favorite;
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "已完成批量删除";
                oLog.Remarks += dto.Message;

            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,删除收藏失败！";
                myLog.ErrorFormat("DeleteUserFavorite方法异常,收藏Id集合：{0}", ex,userFavoriteIdList.ToString());
            }
            return dto;
        }
        #endregion

        #region 删除1-n个收藏 以NodeId
        /// <summary>
        /// 删除1-n个收藏 以NodeId
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="nodeIdList"></param>
        /// <param name="customerToken"></param>
        /// <returns></returns>
        public ResponseBaseDto DeleteUserFavorite(int nodeType, int[] nodeIdList, string customerToken)
        {
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            OperaterLog oLog = new OperaterLog();
            oLog.Action = "用户删除收藏";
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token失效";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
                if (nodeType==0 || nodeIdList.Length==0)
                {
                    dto.Code = (int)CodeEnum.NoComplete;
                    dto.Message = "用户数据提交不完整！";
                    return dto;
                }
                string nodeidListStr = "";
                for (int i = 0; i < nodeIdList.Length; i++)
                {
                    if (i != nodeIdList.Length - 1)
                    {
                        nodeidListStr += nodeIdList[i] + ",";
                    }
                    else
                    {
                        nodeidListStr += nodeIdList[i];
                    }
                }
                userFavoriteServer.DeleteUserFavorite(tcp.CustomerId,nodeType,nodeidListStr);
                oLog.Remarks = tcp.CustomerName + "批量删除收藏类型:"+nodeType+" 收藏nodeId:" + nodeidListStr;
                oLog.TargetType = (int)OperaterLogEnum.Favorite;
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "已完成批量删除！";
                oLog.Remarks += dto.Message;

            }
            catch (Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,删除收藏失败！";
                myLog.ErrorFormat("DeleteUserFavorite方法异常,收藏类型:{0},收藏NodeId集合:{0}", ex,nodeType,nodeIdList.ToString());
            }
            return dto;
        }
        #endregion

        #region 分页查询收藏信息
        /// <summary>
        /// 分页查询收藏信息
        /// </summary>
        /// <param name="startCount">起始条数</param>
        /// <param name="requestCount">请求条数</param>
        /// <param name="customerToken">token</param>
        /// <param name="userfavoriteFlag">ref 实际收藏结果</param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        public ResponseBaseDto GetUserFavoriteByPage(int startCount, int requestCount,string customerToken,
                                 ref List<UserFavoriteResponse> userFavoriteRespnseFlag, ref int total)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            int customerId = 0;
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token失效";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
                customerId = tcp.CustomerId;
                startCount -= 1;
                startCount = startCount < 0 ? 0 : startCount;
                IList<UserFavorite> userFavoriteFlag = userFavoriteServer.SelectUserFavoriteByPage(tcp.CustomerId, startCount, requestCount);
                total = userFavoriteServer.SelectUserFavoriteByPageCount(tcp.CustomerId);
                for (int i = 0; i < userFavoriteFlag.Count; i++)
                {
                    UserFavoriteResponse ufr = new UserFavoriteResponse();
                    ufr.CustomerId = userFavoriteFlag[i].CustomerId;
                    ufr.UserFavoriteId = userFavoriteFlag[i].UserFavoriteId;
                    ufr.UserFavoriteType = userFavoriteFlag[i].UserFavoriteType;
                    ufr.UserFavoriteTypeId = userFavoriteFlag[i].UserFavoriteTypeId;
                    ufr.AliasName = userFavoriteFlag[i].AliasName;
                    ufr.FavoriteTime = userFavoriteFlag[i].FavoriteTime.ToString();//无法输出datetime类型,进行转换
                    //如果是通道的情况需加入通道封面
                    if (ufr.UserFavoriteType == (int)UserFavoriteTypeEnum.Channel) 
                    {
                       IList<Channel> channelFlag=
                           channelServer.SelectChannelByChannelId(new Channel() { ChannelId = ufr.UserFavoriteTypeId });
                       if (channelFlag != null && channelFlag.Count > 0)
                       {
                           ufr.ImagePath = channelFlag[0].ImagePath == string.Empty ? "default.jpg" : channelFlag[0].ImagePath;
                       }
                    }
                    userFavoriteRespnseFlag.Add(ufr);
                }
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "已完成收藏信息获取";
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,查询收藏失败！";
                myLog.WarnFormat("GetUserFavoriteByPage方法异常,用户Id:{0},请求的起始条数:{1},请求条数:{2}", ex,customerId,startCount,requestCount);
            }
            return dto;
        }
        #endregion
    }
}
