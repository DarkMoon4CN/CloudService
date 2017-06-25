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
    public class Device : IDevice
    {
        DeviceBLL deviceBLL = new DeviceBLL();

        //添加设备
        public AddDeviceResponseDto AddDevice(AddDeviceRequestDto req)
        {
            AddDeviceResponseDto ard = new AddDeviceResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                ard.Code = (int)CodeEnum.ReqNoToken;
                ard.Message = RestHelper.SecNoTokenMessage;
            }
            else 
            {
             //code
             ResponseBaseDto dto=deviceBLL.AddDevice
                 (req.DeviceName, req.SerialNumber, customerToken);
             ard.Code = dto.Code;
             ard.Message = dto.Message;
            }
            return ard;
        }

        //删除设备
        public DeleteDeviceResponseDto DeleteDevice(DeleteDeviceRequestDto req)
        {
            DeleteDeviceResponseDto ddr = new DeleteDeviceResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                ddr.Code = (int)CodeEnum.ReqNoToken;
                ddr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //code
                Bsr.Cloud.Model.Entities.Device device = new Model.Entities.Device();
                device.DeviceId = req.Deviceid;
                ResponseBaseDto dto = deviceBLL.DeleteDeviceByDeviceId(device, customerToken);
                
                ddr.Code = dto.Code;
                ddr.Message = dto.Message;
            }
            return ddr;
        }

        //修改设备名称
        public UpdateDeviceNameResponseDto UpdateDeviceName(UpdateDeviceNameRequestDto req)
        {
            UpdateDeviceNameResponseDto udnr = new UpdateDeviceNameResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                udnr.Code = (int)CodeEnum.ReqNoToken;
                udnr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //code
                Bsr.Cloud.Model.Entities.Device device = new Model.Entities.Device();
                device.DeviceName = req.NewDeviceName;
                device.DeviceId = req.DeviceId;
                ResponseBaseDto dto = deviceBLL.UpdateDeviceByDeviceName(device,customerToken);
                udnr.Code = dto.Code;
                udnr.Message = dto.Message;
            }
            return udnr;
        }

        //根据当前用户获取设备
        public GetDeviceBySelfResponseDto GetDeviceBySelf(GetDeviceBySelfRequestDto req)
        {
            GetDeviceBySelfResponseDto gbcr = new GetDeviceBySelfResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gbcr.Code = (int)CodeEnum.ReqNoToken;
                gbcr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Bsr.Cloud.Model.Entities.Customer customer = new Model.Entities.Customer();
                List<DeviceResponse> deviceResponseFlag = new List<DeviceResponse>();
                ResponseBaseDto dto = deviceBLL.GetDeviceByCustomerId(customerToken, ref deviceResponseFlag);
                gbcr.Code = dto.Code;
                gbcr.Message = dto.Message;
                if (gbcr.Code == 0) 
                {
                    gbcr.deviceResponseList = deviceResponseFlag;
                }
            }
            return gbcr;
        }

        //根据设备Id获取通道
        public GetChannelByDeviceIdResponseDto GetChannelByDeviceId(GetChannelByDeviceIdRequestDto req)
        {
            GetChannelByDeviceIdResponseDto gbdr = new GetChannelByDeviceIdResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gbdr.Code = (int)CodeEnum.ReqNoToken;
                gbdr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                //code
                Bsr.Cloud.Model.Entities.Device device = new Model.Entities.Device();
                device.DeviceId = req.DeviceId;
                IList<Bsr.Cloud.Model.Entities.Channel> channelFlag = null;
                ResponseBaseDto dto = deviceBLL.GetChannelByDeviceId(device, customerToken, ref channelFlag);
                gbdr.Code = dto.Code;
                gbdr.Message = dto.Message;
                gbdr.channelList = (List<Bsr.Cloud.Model.Entities.Channel>)channelFlag;
            }
            return gbdr;
        }

        //查询设备状态
        public GetServerDeviceStateResponseDto GetServerDeviceState(GetServerDeviceStateRequestDto req)
        {
            GetServerDeviceStateResponseDto gdsr = new GetServerDeviceStateResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gdsr.Code = (int)CodeEnum.ReqNoToken;
                gdsr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                List<DeviceResponse> deviceResponseFlag = new List<DeviceResponse>();
                ResponseBaseDto dto = deviceBLL.GetServerDeviceState(req.DeviceIdList, customerToken, ref deviceResponseFlag);
                List<DeviceState> deviceStateFlag = new List<DeviceState>();
                if (dto.Code == 0) 
                {
                    for (int i = 0; i < deviceResponseFlag.Count; i++)
                    {
                        DeviceState ds = new DeviceState();
                        ds.DeviceId = deviceResponseFlag[i].DeviceId;
                        ds.State = deviceResponseFlag[i].BPServerDeviceState;
                        deviceStateFlag.Add(ds);
                    }
                }
                gdsr.deviceState = deviceStateFlag;
                gdsr.Code = dto.Code;
                gdsr.Message = dto.Message;
            }/*end  if(!RestHelper.SecurityCheck(ref customerToken))*/
            return gdsr;
        }

        //流媒体播放时需要的参数
        public GetStreamerParameterResponseDto GetStreamerParameter(GetStreamerParameterRequestDto req) 
        {
            GetStreamerParameterResponseDto gspr = new GetStreamerParameterResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gspr.Code = (int)CodeEnum.ReqNoToken;
                gspr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Bsr.Cloud.Model.Entities.Channel channel = new Model.Entities.Channel();
                channel.ChannelId = req.ChannelId;
                Bsr.Cloud.Model.BP4StreamerParameter streamerParameter = new Model.BP4StreamerParameter();
                ResponseBaseDto dto= deviceBLL.GetStreamerParameterByCustomerToken(channel,customerToken,ref streamerParameter);
                gspr.Code = dto.Code;
                gspr.Message = dto.Message;
                gspr.streamerParameter = streamerParameter;
            }
            return gspr;
        }

        //检索设备
        public SearchDeviceResponseDto SearchDevice(SearchDeviceRequestDto req) 
        {
            SearchDeviceResponseDto sdrd = new SearchDeviceResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                sdrd.Code = (int)CodeEnum.ReqNoToken;
                sdrd.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                IList<Bsr.Cloud.Model.Entities.Device> deviceFlag=null;
                ResponseBaseDto dto=deviceBLL.SearchDevice(req.KeyWord, customerToken, ref deviceFlag);
                sdrd.Code=dto.Code;
                sdrd.Message = dto.Message;
                sdrd.deviceList = (List<Bsr.Cloud.Model.Entities.Device>)deviceFlag;
            }
            return sdrd;
        }

        //查询设备SN是否已存在
        public IsExistSNResponseDto IsExistSN(IsExistSNRequestDto req)
        {
            IsExistSNResponseDto iesn = new IsExistSNResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                iesn.Code = (int)CodeEnum.ReqNoToken;
                iesn.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
               int isExist=0;
               ResponseBaseDto dto = deviceBLL.GetDeviceBySN(req.SerialNumber, ref isExist);
               iesn.Code = dto.Code;
               iesn.Message = dto.Message;
               iesn.IsExist = isExist;
            }
            return iesn;
        }

        //验证SN码 返回是否可以被添加设备硬件类型 CheckSN
        public CheckSNResponseDto CheckSN(CheckSNRequestDto req) 
        {
            CheckSNResponseDto csnr = new CheckSNResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                csnr.Code = (int)CodeEnum.ReqNoToken;
                csnr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                int isEnable = 0;
                int hardwareType=0;
                int isOnline = 0;
                ResponseBaseDto dto = deviceBLL.CheckDeviceBySN(req.SerialNumber, ref isEnable, ref isOnline, ref hardwareType);
                csnr.Code = dto.Code;
                csnr.Message = dto.Message;
                csnr.IsEnable = isEnable;
                csnr.IsOnline = isOnline;
                csnr.HardwareType = hardwareType;
            }
            return csnr;
        }
    }
}