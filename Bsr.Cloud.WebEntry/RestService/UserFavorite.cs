using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bsr.Cloud.BLogic;
using Bsr.Core.Hibernate.WCF;
using Bsr.Cloud.Model;
using Bsr.Cloud.BLogic.BLL;

namespace Bsr.Cloud.WebEntry.RestService
{
    [NHInstanceContext]
    public class UserFavorite : IUserFavorite
    {
        UserFavoriteBLL userFavoriteBLL = new UserFavoriteBLL(); 
        //添加一条收藏
        public AddUserFavoriteResponseDto AddUserFavorite(AddUserFavoriteRequestDto req)
        {
            AddUserFavoriteResponseDto auf = new AddUserFavoriteResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                auf.Code = (int)CodeEnum.ReqNoToken;
                auf.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                if (req.NodeType == 0) 
                {
                    //默认设定为通道
                    req.NodeType = 1;
                }
                Model.Entities.UserFavorite userFavorite=new Model.Entities.UserFavorite();
                userFavorite.UserFavoriteType=req.NodeType;
                userFavorite.UserFavoriteTypeId=req.NodeId;
                userFavorite.AliasName=req.AliasName;
                int userFavoriteId=0;
                ResponseBaseDto dto= userFavoriteBLL.AddUserFavorite(userFavorite, customerToken, ref userFavoriteId);
                auf.Code = dto.Code;
                auf.Message = dto.Message;
                auf.UserFavoriteId = userFavoriteId;
            }
            return auf;
        }

        //删除1-n个收藏 以UserFavoriteId
        public DeleteUserFavoriteResponseDto DeleteUserFavorite(DeleteUserFavoriteRequestDto req) 
        {
            DeleteUserFavoriteResponseDto duf = new DeleteUserFavoriteResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                duf.Code = (int)CodeEnum.ReqNoToken;
                duf.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                ResponseBaseDto dto = userFavoriteBLL.DeleteUserFavorite(req.UserFavoriteId, customerToken);
                duf.Code = dto.Code;
                duf.Message = dto.Message;
            }
            return duf;
        }

        //删除1-n个收藏 以NodeId
        public DeleteUserFavoriteByTidResponseDto DeleteUserFavoriteByTid(DeleteUserFavoriteByTidRequestDto req)
        {
            DeleteUserFavoriteByTidResponseDto duf = new DeleteUserFavoriteByTidResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                duf.Code = (int)CodeEnum.ReqNoToken;
                duf.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                ResponseBaseDto dto = userFavoriteBLL.DeleteUserFavorite(req.NodeType,req.NodeId, customerToken);
                duf.Code = dto.Code;
                duf.Message = dto.Message;
            }
            return duf;
        }

        //分页获取收藏信息
        public GetUserFavoriteByPageResponseDto GetUserFavoriteByPage(GetUserFavoriteByPageRequestDto req) 
        {
            GetUserFavoriteByPageResponseDto guf = new GetUserFavoriteByPageResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                guf.Code = (int)CodeEnum.ReqNoToken;
                guf.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                
                List<UserFavoriteResponse> userFavoriteList = new List<UserFavoriteResponse>();
                int total=0;
                ResponseBaseDto dto =
                    userFavoriteBLL.GetUserFavoriteByPage(req.StartCount, req.RequestCount, customerToken, ref userFavoriteList, ref total);
                guf.Code = dto.Code;
                guf.Message = dto.Message;
                guf.userFavoriteList = userFavoriteList;
                guf.Total = total;
            }
            return guf;
        }

    }
}