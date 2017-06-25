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
    public class OperaterLog : IOperaterLog
    {
        OperaterLogBLL operaterLogBLL = new OperaterLogBLL();
        //用户当前登陆日志（分页）
        public GetSelfLoginInfoResponseBaseDto GetSelfLoginInfo(GetSelfLoginInfoRequestBaseDto req)
        {
            GetSelfLoginInfoResponseBaseDto gslr = new GetSelfLoginInfoResponseBaseDto();
         
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gslr.Code = (int)CodeEnum.ReqNoToken;
                gslr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                int pageCount = 0;
                IList<Bsr.Cloud.Model.Entities.OperaterLog> operaterLogFlag = null;
                ResponseBaseDto dto = operaterLogBLL.GetSelfOperaterLog(req.RequestCount, req.StartCount, customerToken,ref pageCount,ref operaterLogFlag);
                gslr.Code = dto.Code;
                gslr.Message = dto.Message;
                List<OperaterLogResponse> operaterLogResponseFlag=new List<OperaterLogResponse>();
                for (int i = 0; i < operaterLogFlag.Count; i++)
                {
                  OperaterLogResponse operaterLogResponse=new OperaterLogResponse();
                  operaterLogResponse.AgentType = operaterLogFlag[i].AgentType;
                  operaterLogResponse.AgentVersion = operaterLogFlag[i].AgentVersion;
                  operaterLogResponse.OperaterTime =
                      operaterLogFlag[i].OperaterTime == DateTime.MinValue ? string.Empty : operaterLogFlag[i].OperaterTime.ToString();
                  operaterLogResponse.OperaterId = operaterLogFlag[i].OperaterId;
                  operaterLogResponseFlag.Add(operaterLogResponse);
                }
                gslr.operaterLogList = operaterLogResponseFlag;
            }
            return gslr;
        }
        //前台管理员查看主用户的登陆分页信息
        public GetPrimaryCustomerLoginInfoResponseBaseDto GetPrimaryCustomerLoginInfo(GetPrimaryCustomerLoginInfoRequestBaseDto req) 
        {
            GetPrimaryCustomerLoginInfoResponseBaseDto gpcl = new GetPrimaryCustomerLoginInfoResponseBaseDto();

            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gpcl.Code = (int)CodeEnum.ReqNoToken;
                gpcl.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                int pageCount = 0;
                Model.Entities.Customer primaryCustomer = new Model.Entities.Customer();
                primaryCustomer.CustomerId = req.PrimaryCustomerId;
                IList<Bsr.Cloud.Model.Entities.OperaterLog> operaterLogFlag = null;
                ResponseBaseDto dto = operaterLogBLL.GetPrimaryOrSubLoginInfo(primaryCustomer, req.RequestCount, req.StartCount, customerToken, ref pageCount, ref operaterLogFlag);
                gpcl.Code = dto.Code;
                gpcl.Message = dto.Message;
                List<OperaterLogResponse> operaterLogResponseFlag = new List<OperaterLogResponse>();
                for (int i = 0; i < operaterLogFlag.Count; i++)
                {
                    OperaterLogResponse operaterLogResponse = new OperaterLogResponse();
                    operaterLogResponse.AgentType = operaterLogFlag[i].AgentType;
                    operaterLogResponse.AgentVersion = operaterLogFlag[i].AgentVersion;
                    operaterLogResponse.OperaterTime =
                        operaterLogFlag[i].OperaterTime == DateTime.MinValue ? string.Empty : operaterLogFlag[i].OperaterTime.ToString();
                    operaterLogResponse.OperaterId = operaterLogFlag[i].OperaterId;
                    operaterLogResponseFlag.Add(operaterLogResponse);
                }
                gpcl.operaterLogList = operaterLogResponseFlag;
            }
            return gpcl;
        }
        //主用户查看子用户的登陆分页信息
        public GetSubCustomerLoginInfoResponseBaseDto GetSubCustomerLoginInfo(GetSubCustomerLoginInfoRequestBaseDto req) 
        {
            GetSubCustomerLoginInfoResponseBaseDto gpcl = new GetSubCustomerLoginInfoResponseBaseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gpcl.Code = (int)CodeEnum.ReqNoToken;
                gpcl.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                int pageCount = 0;
                Model.Entities.Customer subCustomer = new Model.Entities.Customer();
                subCustomer.CustomerId = req.SubCustomerId;
                IList<Bsr.Cloud.Model.Entities.OperaterLog> operaterLogFlag = null;
                ResponseBaseDto dto = operaterLogBLL.GetPrimaryOrSubLoginInfo(subCustomer, req.RequestCount, req.StartCount, customerToken, ref pageCount, ref operaterLogFlag);
                gpcl.Code = dto.Code;
                gpcl.Message = dto.Message;
                List<OperaterLogResponse> operaterLogResponseFlag = new List<OperaterLogResponse>();
                for (int i = 0; i < operaterLogFlag.Count; i++)
                {
                    OperaterLogResponse operaterLogResponse = new OperaterLogResponse();
                    operaterLogResponse.AgentType = operaterLogFlag[i].AgentType;
                    operaterLogResponse.AgentVersion = operaterLogFlag[i].AgentVersion;
                    operaterLogResponse.OperaterTime =
                        operaterLogFlag[i].OperaterTime == DateTime.MinValue ? string.Empty : operaterLogFlag[i].OperaterTime.ToString();
                    operaterLogResponse.OperaterId = operaterLogFlag[i].OperaterId;
                    operaterLogResponseFlag.Add(operaterLogResponse);
                }
                gpcl.operaterLogList = operaterLogResponseFlag;
            }
            return gpcl;
        }
    }
}