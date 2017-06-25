using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bsr.Cloud.BLogic;
using Bsr.Cloud.Model;
using Bsr.Cloud.BLogic.BLL;
using Bsr.Core.Hibernate.WCF;
using Bsr.Cloud.Core;

namespace Bsr.Cloud.WebEntry.RestService
{
    [NHInstanceContext]
    public class Channel : IChannel
    {

        static private ILogger myLog = new Logger<Channel>();

        ChannelBLL channelBLL = new ChannelBLL();
        ResourceGroupBLL resourceGroupBLL = new ResourceGroupBLL();

        public UpdateChannelNameResponseDto UpdateChannelName(UpdateChannelNameRequestDto req)
        {
            UpdateChannelNameResponseDto ucnr = new UpdateChannelNameResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                ucnr.Code = (int)CodeEnum.ReqNoToken;
                ucnr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //code
                Bsr.Cloud.Model.Entities.Channel channel = new Model.Entities.Channel();
                channel.ChannelName=req.NewChannelName;
                channel.ChannelId = req.ChannelId;
                ResponseBaseDto dto = channelBLL.UpdateChannelByChannelId(channel,customerToken);
                ucnr.Code = dto.Code;
                ucnr.Message = dto.Message;
            }
            return ucnr;
        }
        //更新通道图片
        public UpLoadChannelImageResponseDto UpLoadChannelImage(UpLoadChannelImageRequestDto req) 
        {
            UpLoadChannelImageResponseDto uird = new UpLoadChannelImageResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                uird.Code = (int)CodeEnum.ReqNoToken;
                uird.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                int channelId = req.ChannelId;
                string imageBase64 = req.ImageByteBase64;
                string extName = req.extName;
                if (extName == null || extName.Length == 0)
                {
                    extName = "jpg";
                }
                if (channelId > 0 && imageBase64.Length > 0)
                {
                    
                    string imagePath = "";
                    // 解码图片二进制数据, bin为还原后的Byte[]
                    byte[] bin = Base64.FromBase64ToByte(imageBase64);

                    ResponseBaseDto dto = channelBLL.UpdateChanneImagePathlByChannelId(
                                        channelId, bin, extName, customerToken, ref imagePath);
                    uird.Code = dto.Code;
                    uird.Message = dto.Message;
                    uird.ImagePath = imagePath;
                }
                else
                {
                    myLog.WarnFormat("上传通道图片时参数错误, channelId:{0}", channelId);
                    uird.Code = (int)CodeEnum.NoComplete;
                    uird.Message = "数据请求不完整";
                }
            }

            return uird;
        }
        //通道分页提供数据
        public GetChannelByPageResponseDto GetChannelByPage(GetChannelByPageRequestDto req) 
        {
            GetChannelByPageResponseDto gbrd = new GetChannelByPageResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gbrd.Code = (int)CodeEnum.ReqNoToken;
                gbrd.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                IList<GroupChannelResponse> groupChannelResponseFlag = new List<GroupChannelResponse>();
                int total = 0;
                ResponseBaseDto dto = channelBLL.GetChannelByPage(req.StartCount, req.RequestCount, string.Empty, customerToken, ref groupChannelResponseFlag,ref total);
                gbrd.Code = dto.Code;
                gbrd.Message = dto.Message;
                gbrd.groupChannelResponseList = (List<GroupChannelResponse>)groupChannelResponseFlag;
                gbrd.Total = total;
            }
            return gbrd;
        }
        //查询单一通道
        public GetSingleChannelResponseDto GetSingleChannel(GetSingleChannelRequestDto req) 
        {
            GetSingleChannelResponseDto cscr = new GetSingleChannelResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                cscr.Code = (int)CodeEnum.ReqNoToken;
                cscr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                GroupChannelResponse groupChannelResponse = new GroupChannelResponse();
                ResponseBaseDto dto = channelBLL.GetSingleChannel(req.ChannelId, customerToken, ref groupChannelResponse);
                cscr.Code = dto.Code;
                cscr.Message = dto.Message;
                cscr.groupChannel = groupChannelResponse;                
            }
            return cscr;
        }
        //开启或关闭一个通道
        public EnableChannelResponseDto EnableChannel(EnableChannelRequestDto req) 
        {
            EnableChannelResponseDto ucnr = new EnableChannelResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                ucnr.Code = (int)CodeEnum.ReqNoToken;
                ucnr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //code
                Bsr.Cloud.Model.Entities.Channel channel = new Model.Entities.Channel();
                channel.IsEnable = req.IsEnable;
                channel.ChannelId = req.ChannelId;
                ResponseBaseDto dto = channelBLL.EnableChannel(channel, customerToken);
                ucnr.Code = dto.Code;
                ucnr.Message = dto.Message;
            }
            return ucnr;
        }
        //查询所有通道的权限名
        public GetChanenlPermissionResponseDto GetChanenlPermissionName(GetChanenlPermissionRequestDto req) 
        {
            GetChanenlPermissionResponseDto gcpn = new GetChanenlPermissionResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gcpn.Code = (int)CodeEnum.ReqNoToken;
                gcpn.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                List<ChannelPermissionName> cpnList = new List<ChannelPermissionName>();
                foreach (int cpKey in Enum.GetValues(typeof(PermissionChannelNameEnum)))
                {
                    ChannelPermissionName cpn = new ChannelPermissionName();
                    cpn.ChannelPermissionKey = cpKey;
                    cpn.ChannelPermissionValue = Enum.GetName(typeof(PermissionCustomerEnum), cpKey);
                    cpnList.Add(cpn);
                }
                gcpn.Code = (int)CodeEnum.Success;
                gcpn.Message = "通道权限已获取完成";
                gcpn.channelPermissionName = cpnList;
            }
            return gcpn;
        }
        //检索通道并分页
        public SearchChannelByPageResponseDto SearchChannelByPage(SearchChannelByPageRequestDto req) 
        {
            SearchChannelByPageResponseDto scbp = new SearchChannelByPageResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                scbp.Code = (int)CodeEnum.ReqNoToken;
                scbp.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                IList<GroupChannelResponse> groupChannelResponseFlag = new List<GroupChannelResponse>();
                int total = 0;
                ResponseBaseDto dto=null;
                if (req.IsGroup!=0 )
                {
                   dto= resourceGroupBLL.GetChannelByPageOrResourceGroupId(
                        new Model.Entities.ResourceGroup() { ResourceGroupId = req.ResouceGroupId },
                        req.StartCount,
                        req.RequestCount,
                        req.KeyWord,
                        customerToken,
                        ref groupChannelResponseFlag,
                        ref total);
                }
                else //if(req.IsGroup==0) 
                {

                    dto = channelBLL.GetChannelByPage(
                        req.StartCount,
                        req.RequestCount, 
                        req.KeyWord, 
                        customerToken,
                        ref groupChannelResponseFlag,
                        ref total);
                    
                }
                scbp.Code = dto.Code;
                scbp.Message = dto.Message;
                scbp.groupChannelResponseList = (List<GroupChannelResponse>)groupChannelResponseFlag;
                scbp.Total = total;
            }
            return scbp;
        }

        //设置通道码流
        public UpdateChannelEncoderInfoResponseDto UpdateChannelEncoderInfo(UpdateChannelEncoderInfoRequestDto req)
        {
            UpdateChannelEncoderInfoResponseDto ucei = new UpdateChannelEncoderInfoResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                ucei.Code = (int)CodeEnum.ReqNoToken;
                ucei.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //code
                Bsr.Cloud.Model.Entities.Channel channel = new Model.Entities.Channel();
                ResponseBaseDto dto = channelBLL.UpdateChannelEncoderInfo(req.ChannelId,req.StreamType,customerToken);
                ucei.Code = dto.Code;
                ucei.Message = dto.Message;
            }
            return ucei;
        }
    }
}