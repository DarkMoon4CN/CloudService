using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bsr.Cloud.BLogic;
using Bsr.Cloud.Core;
using Bsr.Core.Hibernate.WCF;
using Bsr.Cloud.Model;
using System.ServiceModel.Web;
using Bsr.Cloud.BLogic.BLL;
using Bsr.Cloud.Model.Entities;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace Bsr.Cloud.WebEntry.RestService
{
    [NHInstanceContext]
    public class Customer : ICustomer
    {
        //登陆
        public SignInResponseDto Login(SignInRequestDto req)
        {
            Bsr.Cloud.Model.Entities.Customer cust = new Model.Entities.Customer();
            SignInResponseDto sir = new SignInResponseDto();
            if (req.LoginName != null && req.Password != null && req.LoginType != 0)
            {
                cust.CustomerName = req.LoginName;
                cust.Password = req.Password;
                int customerId = 0;
                string customerToken = "";
                string customerName = "";
                int signInType=0;
                ResponseBaseDto dto = customerBLL.SignIn(req.LoginName, req.Password, 
                        req.LoginType, req.AgentVersion, GetRemoteEndpointAddress(),
                        ref customerId, ref customerName,ref signInType,ref customerToken);

                sir.Code = dto.Code;
                sir.Message = dto.Message;
                sir.CustomerToken = customerToken;
                sir.CustomerId = customerId;
                sir.CustomerName = customerName;
                sir.SignInType = signInType;
            }
            else 
            {
                sir.Code = (int)CodeEnum.NoData;
                sir.Message = "登陆名或登陆密码填写不完整！";
            }
            return sir;
        }
        //查询电话是否可用
        public GetReceiverPhoneResponseDto GetReceiverPhone(GetReceiverPhoneRequestDto req)
        {
           
            Bsr.Cloud.Model.Entities.Customer cust = new Model.Entities.Customer();
            cust.ReceiverCellPhone = req.ReceiverCellPhone;           
            int isUse=0;//默认初始设定为不可用
            ResponseBaseDto dto =customerBLL.GetReceiverPhone(cust,ref isUse);
            GetReceiverPhoneResponseDto grp = new GetReceiverPhoneResponseDto();
            grp.Code = dto.Code;
            grp.Message = dto.Message;
            grp.IsUse= isUse;
            return grp;
        }
        //查询用户名是否可用
        public GetCustomerNameResponseDto GetCustomerName(GetCustomerNameRequestDto req)
        {
            Bsr.Cloud.Model.Entities.Customer cust = new Model.Entities.Customer();
            cust.CustomerName = req.CustomerName;
            int isUse = 0;//默认初始设定为不可用
            ResponseBaseDto dto = customerBLL.GetCustomerName(cust,ref isUse);
            GetCustomerNameResponseDto gnr = new GetCustomerNameResponseDto();
            gnr.Code = dto.Code;
            gnr.Message = dto.Message;
            gnr.IsUse = isUse;
            return gnr;
        }
        //查询邮箱是否可用
        public GetReceiverEmailResponseDto GetReceiverEmail(GetReceiverEmailRequestDto req)
        {
            Bsr.Cloud.Model.Entities.Customer cust = new Model.Entities.Customer();
            cust.ReceiverEmail = req.ReceiverEmail;
            int isUse = 0;//默认初始设定为不可用
            ResponseBaseDto dto =customerBLL.GetReceiverEmail(cust,ref isUse);
            GetReceiverEmailResponseDto rer = new GetReceiverEmailResponseDto();
            rer.Code = dto.Code;
            rer.Message = dto.Message;
            rer.IsUse =isUse;
            return rer;
        }
        //定时心跳
        public TimingCheckResponseDto TimingCheck(TimingCheckRequestDto req)
        {
            // 计算 用户完成一次心跳的 时间
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            ResponseBaseDto dto = new ResponseBaseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                dto.Code = (int)CodeEnum.ReqNoToken;
                dto.Message = RestHelper.SecNoTokenMessage;
            }  
            else
            {
                dto = CustomerBLL.TimingCheck(customerToken);
            }
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 100) // 更新一次心跳时间如果大于100ms,则记日志。
            {
                myLog.WarnFormat("传入token:{0}, 心跳耗时：{1} ms", customerToken, stopwatch.ElapsedMilliseconds);
            }
            TimingCheckResponseDto trd = new TimingCheckResponseDto() { Code = dto.Code, Message = dto.Message };
            return trd;
        }
        //冻结与解冻子用户
        public EnableSubCustomerResponseDto EnableSubCustomer(EnableSubCustomerRequestDto req)
        {
            EnableSubCustomerResponseDto cfr = new EnableSubCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                cfr.Code = (int)CodeEnum.ReqNoToken;
                cfr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                ResponseBaseDto dto = customerBLL.EnableSubCustomer(req.SubCustomerId, req.IsEnable, customerToken);
                cfr.Code = dto.Code;
                cfr.Message = dto.Message; 
            }
            return cfr;
        }
        ///创建子用户
        public AddSubCustomerResponseDto AddSubCustomer(AddSubCustomerRequestDto req)
        {
            AddSubCustomerResponseDto gcbm = new AddSubCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gcbm.Code = (int)CodeEnum.ReqNoToken;
                gcbm.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Bsr.Cloud.Model.Entities.Customer customer = new Model.Entities.Customer();

                customer.CustomerName = req.CustomerName;
                customer.Password = req.Password;
                customer.SignInType = (int)CustomerSignInTypeEnum.SubCustomer;//子账号
                customer.ReceiverName = req.ReceiverName;
                customer.ReceiverCellPhone = req.ReceiverCellPhone;
                customer.ReceiverEmail = req.ReceiverEmail;
                customer.AccountTelephone = req.AccountTelephone;
                customer.AccountCompanyAddress = req.AccountCompanyAddress;
                customer.LoginTypes = req.LoginTypes;
                //过期时间设定：1.传递空值永久有效 2.大于当前时间的值
                if (req.ValidTime == null || req.ValidTime == "" || req.ValidTime == "null")
                {
                    //设置为永久有效
                    req.ValidTime = DateTime.MinValue.ToString();
                }
                DateTime validTime;
                DateTime.TryParse(req.ValidTime, out validTime);
                customer.ValidTime = validTime;
                string imageBase64 = req.ImageByteBase64;
                string extName = req.ExtName;
                byte[] bin = null;
                if (imageBase64 != null && imageBase64 != "")
                {
                    if (extName == null || extName.Length == 0)
                    {
                        extName = "jpg";
                    }
                    // 解码图片二进制数据, bin为还原后的Byte[]
                    bin = Base64.FromBase64ToByte(imageBase64);
                }
                customer.IsEnable = 1;
                int customerId = 0;
                ResponseBaseDto dto = customerBLL.AddSubCustomer(customer, bin, extName, customerToken, ref customerId);
                gcbm.Code = dto.Code;
                gcbm.Message = dto.Message;
                gcbm.CustomerId = customerId;
            }
            return gcbm;
        }
        //注册主账号信息
        public AddPrimaryCustomerResponseDto AddPrimaryCustomer(AddPrimaryCustomerRequestDto req)
        {
            AddPrimaryCustomerResponseDto cpir = new AddPrimaryCustomerResponseDto();
            if (req == null)
            {
                cpir.Code = (int)CodeEnum.ApplicationErr;
                cpir.Message = "数据提交异常";
            }
            else
            {
                Bsr.Cloud.Model.Entities.Customer customer = new Model.Entities.Customer();
                //type注册类型
                int RegisterType = Convert.ToInt32(req.AgentType);
                if (RegisterType == (int)RegisterTypeEnum.android || RegisterType == (int)RegisterTypeEnum.ios)
                {
                    //手机端注册 需填充默认注册参数
                    customer.CustomerName =req.ReceiverCellPhone;
                    customer.ReceiverCellPhone = req.ReceiverCellPhone;
                    customer.ReceiverEmail = "bstar" + req.ReceiverEmail + "@bstar.com.cn";
                }
                else
                {
                    customer.CustomerName = req.CustomerName;
                    customer.ReceiverCellPhone = req.ReceiverCellPhone;
                    customer.ReceiverEmail = req.ReceiverEmail;
                }
                ResponseBaseDto dto = null;
                //确认验证码是否可用在有实际的验证码时解除注释
                int IsEnable = 0;
                dto = customerBLL.CheckingWithSum(req.ReceiverCellPhone, req.VerifyKey, (int)MsgNoticeTypeEnum.RegisterUser, ref IsEnable);
                if (IsEnable==0 && dto.Code!=0)
                {
                    cpir.Code = dto.Code;
                    cpir.Message = dto.Message;
                    cpir.CustomerId = 0;
                    return cpir;
                }
                customer.Password = req.Password;
                customer.ReceiverName = req.ReceiverName;
                customer.SignInType = (int)CustomerSignInTypeEnum.PrimaryCustomer;//主账号

                customer.AccountIDNumber = req.AccountIDNumber;
                customer.AccountTelephone = req.AccountTelephone;
                customer.AccountCompany = req.AccountCompany;
                customer.AccountCompanyAddress = req.AccountCompanyAddress;
                customer.AccountHomeAddress = req.AccountHomeAddress;
                //主动注册有效期为180天
                customer.ValidTime = DateTime.Now.AddDays(180);
                customer.IsEnable = 1;
                int customerId = 0;
                dto  = customerBLL.AddParentCustomerInfo(customer, ref customerId);
                cpir.Code = dto.Code;
                cpir.Message = dto.Message;
                cpir.CustomerId = customerId;
            }
            return cpir;
        }
        //验证服务器来的token
        public CheckServerCustomerTokenResponseDto CheckServerCustomerToken(CheckServerCustomerTokenRequestDto req) 
        {
            CheckServerCustomerTokenResponseDto cctr = new CheckServerCustomerTokenResponseDto();
            cctr.IsUse = 0;
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                cctr.Code = (int)CodeEnum.ReqNoToken;
                cctr.Message = RestHelper.SecNoTokenMessage;
            }
            else 
            {
                ResponseBaseDto dto = customerBLL.CheckServerCustomerToken(req.PermissionName,req.NodeId,req.NodeType,customerToken);
                if (dto.Code == 0) 
                {
                    cctr.IsUse = 1;
                }
                cctr.Code = dto.Code;
                cctr.Message = dto.Message;
            }
            return cctr;
        }
        //注销
        public SignOutResponseDto LogOut(SignOutRequestDto req) 
        {
            SignOutResponseDto sord = new SignOutResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                sord.Code = (int)CodeEnum.ReqNoToken;
                sord.Message = RestHelper.SecNoTokenMessage;
            }
            else 
            {
                ResponseBaseDto dto=customerBLL.SignOut(customerToken);
                sord.Code = dto.Code;
                sord.Message = dto.Message;
            }
            return sord;
        }
        //获取当前主用户的所有子用户信息
        public GetSubCustomerResponseDto GetSubCustomer(GetSubCustomerRequestDto req) 
        {
            GetSubCustomerResponseDto gscr = new GetSubCustomerResponseDto();
            IList<Bsr.Cloud.Model.Entities.Customer> customerFlag = new List<Bsr.Cloud.Model.Entities.Customer>();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gscr.Code = (int)CodeEnum.ReqNoToken;
                gscr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                ResponseBaseDto dto = customerBLL.GetSubCustomer(customerToken, ref customerFlag);
                List<CustomerResponse> customerList = new List<CustomerResponse>();
                for (int i = 0; i < customerFlag.Count; i++)
                {
                    Bsr.Cloud.Model.Entities.Customer customer = customerFlag[i];
                    CustomerResponse cr = new CustomerResponse();
                    cr.CustomerId = customer.CustomerId;
                    cr.CustomerName = customer.CustomerName;
                    cr.SignInType = customer.SignInType;
                    cr.ReceiverName = customer.ReceiverName;
                    cr.ReceiverEmail = customer.ReceiverEmail;
                    cr.ReceiverCellPhone = customer.ReceiverCellPhone;
                    cr.AccountIDNumber = customer.AccountIDNumber;
                    cr.AccountTelephone = customer.AccountTelephone;
                    cr.AccountCompanyAddress = customer.AccountCompanyAddress;
                    cr.IsEnable = customer.IsEnable;
                    cr.AccountHomeAddress = customer.AccountHomeAddress;
                    cr.ValidTime =
                           customer.ValidTime == DateTime.MinValue ? string.Empty : cr.ValidTime = customer.ValidTime.ToString();
                    customerList.Add(cr);
                }
                gscr.customerReponseList = customerList;
                gscr.Code = dto.Code;
                gscr.Message = dto.Message;
            }
            return gscr;
        }
        //获取当前用户信息
        public GetSelfCustomerResponseDto GetSelfCustomer(GetSelfCustomerRequestDto req)
        {
            GetSelfCustomerResponseDto gscr = new GetSelfCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            { 
                gscr.Code = (int)CodeEnum.ReqNoToken;
                gscr.Message = RestHelper.SecNoTokenMessage;
            }
            else 
            {
                Bsr.Cloud.Model.Entities.Customer customer = new Model.Entities.Customer();
                ResponseBaseDto dto= customerBLL.GetSelfCustomer(customerToken,ref customer);
                if (dto.Code == 0) 
                {
                    CustomerResponse cr = new CustomerResponse();
                    cr.CustomerId = customer.CustomerId;
                    cr.CustomerName = customer.CustomerName;
                    cr.SignInType = customer.SignInType;
                    cr.ReceiverName = customer.ReceiverName;
                    cr.ReceiverEmail = customer.ReceiverEmail;
                    cr.ReceiverCellPhone = customer.ReceiverCellPhone;
                    cr.AccountIDNumber = customer.AccountIDNumber;
                    cr.AccountTelephone = customer.AccountTelephone;
                    cr.AccountCompanyAddress = customer.AccountCompanyAddress;
                    cr.IsEnable = customer.IsEnable;
                    cr.AccountHomeAddress = customer.AccountHomeAddress;
                    cr.ImagePath += @"customerImage/" + customer.ImagePath;
                    cr.ForbiddenTime =
                        customer.ForbiddenTime == DateTime.MinValue ? string.Empty : cr.ForbiddenTime = customer.ForbiddenTime.ToString();
                    cr.ValidTime =
                           customer.ValidTime == DateTime.MinValue ? string.Empty : cr.ValidTime = customer.ValidTime.ToString();
                    cr.LoginTypes = customer.LoginTypes;
                    gscr.customerReponse = cr;
                }
                gscr.Code = dto.Code;
                gscr.Message = dto.Message;

            }
            return gscr;
        }
        //更新当前用户信息
        public UpdateSelfCustomerResponseDto UpdateSelfCustomer(UpdateSelfCustomerRequestDto req) 
        {
            UpdateSelfCustomerResponseDto uscr = new UpdateSelfCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                uscr.Code = (int)CodeEnum.ReqNoToken;
                uscr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                string imageBase64 = req.ImageByteBase64;
                string extName = req.ExtName;
                byte[] bin=null;
                if (imageBase64 != null && imageBase64 != "") 
                {
                    if (extName == null || extName.Length == 0)
                    {
                        extName = "jpg";
                    }
                    // 解码图片二进制数据, bin为还原后的Byte[]
                    bin = Base64.FromBase64ToByte(imageBase64);
                }

                //code
                Bsr.Cloud.Model.Entities.Customer customer = new Model.Entities.Customer();
                customer.ReceiverName=req.ReceiverName;
                customer.ReceiverCellPhone=req.ReceiverCellPhone;
                customer.AccountCompanyAddress=req.AccountCompanyAddress;
                customer.AccountTelephone=req.AccountTelephone;
                customer.ReceiverEmail=req.ReceiverEmail;
                string imagePath=string.Empty;
                ResponseBaseDto dto= customerBLL.UpdateSelfCustomer(customer,bin,extName,customerToken,ref imagePath);
                uscr.Code = dto.Code;
                uscr.Message = dto.Message;
                uscr.ImagePath = @"customerImage/" + imagePath;
            }
            return uscr;
        }
        //检索当前前台管理员下的主账号信息
        public SearchPrimaryCustomerResponseDto SearchPrimaryCustomer(SearchPrimaryCustomerRequestDto req) 
        {
            SearchPrimaryCustomerResponseDto gpcr = new SearchPrimaryCustomerResponseDto();
            IList<Bsr.Cloud.Model.Entities.Customer> customerFlag = new List<Bsr.Cloud.Model.Entities.Customer>();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gpcr.Code = (int)CodeEnum.ReqNoToken;
                gpcr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                List<CustomerResponse> customerList = new List<CustomerResponse>();
                ResponseBaseDto dto = customerBLL.SearchPrimaryCustomer(req.KeyWord,customerToken, ref customerFlag);
                for (int i = 0; i < customerFlag.Count; i++)
                {
                    Bsr.Cloud.Model.Entities.Customer primaryCustomer = customerFlag[i];
                    CustomerResponse cr = new CustomerResponse();
                    cr.CustomerId = primaryCustomer.CustomerId;
                    cr.CustomerName = primaryCustomer.CustomerName;
                    cr.SignInType = primaryCustomer.SignInType;
                    cr.ReceiverName = primaryCustomer.ReceiverName;
                    cr.ReceiverEmail = primaryCustomer.ReceiverEmail;
                    cr.ReceiverCellPhone = primaryCustomer.ReceiverCellPhone;
                    cr.AccountIDNumber = primaryCustomer.AccountIDNumber;
                    cr.AccountTelephone = primaryCustomer.AccountTelephone;
                    cr.AccountCompanyAddress = primaryCustomer.AccountCompanyAddress;
                    cr.IsEnable = primaryCustomer.IsEnable;
                    cr.AccountHomeAddress = primaryCustomer.AccountHomeAddress;
                    cr.ImagePath = @"customerImage/"+ primaryCustomer.ImagePath;
                    cr.ForbiddenTime =
                        primaryCustomer.ForbiddenTime == DateTime.MinValue ? string.Format("永久有效") : cr.ForbiddenTime = primaryCustomer.ForbiddenTime.ToString();
                    cr.ValidTime =
                           primaryCustomer.ValidTime == DateTime.MinValue ? string.Format("永久有效") : cr.ValidTime = primaryCustomer.ValidTime.ToString();
                    cr.LoginTypes = primaryCustomer.LoginTypes;
                    customerList.Add(cr);
                }
                gpcr.customerReponseList = customerList;
                gpcr.Code = dto.Code;
                gpcr.Message = dto.Message;
            }
            return gpcr;
        }
        //由前台管理添加主账号
        public AddPrimaryCustomerByManagerAccountResponseDto AddPrimaryCustomerByManagerAccount(AddPrimaryCustomerByManagerAccountRequestDto req)
        {
            AddPrimaryCustomerByManagerAccountResponseDto gcbm = new AddPrimaryCustomerByManagerAccountResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gcbm.Code = (int)CodeEnum.ReqNoToken;
                gcbm.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Bsr.Cloud.Model.Entities.Customer customer = new Model.Entities.Customer();

                customer.CustomerName = req.CustomerName;
                customer.Password = req.Password;
                customer.SignInType = (int)CustomerSignInTypeEnum.PrimaryCustomer;//主账号
                customer.ReceiverName = req.ReceiverName;
                customer.ReceiverCellPhone = req.ReceiverCellPhone;
                customer.ReceiverEmail = req.ReceiverEmail;
                customer.AccountTelephone = req.AccountTelephone;
                customer.AccountCompanyAddress = req.AccountCompanyAddress;
                customer.LoginTypes = req.LoginTypes;
                //过期时间设定：1.传递空值永久有效 2.大于当前时间的值
                if(req.ValidTime==null || req.ValidTime=="" || req.ValidTime=="null")
                {
                    //设置为永久有效
                    req.ValidTime =DateTime.MinValue.ToString();
                }
                DateTime validTime;
                DateTime.TryParse(req.ValidTime, out validTime);
                customer.ValidTime = validTime;
                string imageBase64 = req.ImageByteBase64;
                string extName = req.ExtName;
                byte[] bin = null;
                if (imageBase64 != null && imageBase64 != "")
                {
                    if (extName == null || extName.Length == 0)
                    {
                        extName = "jpg";
                    }
                    // 解码图片二进制数据, bin为还原后的Byte[]
                    bin = Base64.FromBase64ToByte(imageBase64);
                }
                customer.IsEnable = 1;
                int customerId = 0;
                ResponseBaseDto dto = customerBLL.AddPrimaryCustomerByManagerAccount(customer,bin,extName,customerToken ,ref customerId);
                gcbm.Code = dto.Code;
                gcbm.Message = dto.Message;
                gcbm.CustomerId = customerId;
            }
            return gcbm;
        }
        //更新当前用户的安全信息
        public UpdateSelfSafeCustomerResponseDto UpdateSelfSafeCustomer(UpdateSelfSafeCustomerRequestDto req) 
        {
            UpdateSelfSafeCustomerResponseDto uscr = new UpdateSelfSafeCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                uscr.Code = (int)CodeEnum.ReqNoToken;
                uscr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Bsr.Cloud.Model.Entities.Customer customer = new Model.Entities.Customer();
                customer.ReceiverName = req.LoginTypes;
                ResponseBaseDto dto = customerBLL.UpdateSelfSafeCustomer(req.LoginTypes,customerToken);
                uscr.Code = dto.Code;
                uscr.Message = dto.Message;
            }
            return uscr;
        }
        //重置当前用户密码
        public UpdateSelfCustomerPassWordResponseDto UpdateSelfCustomerPassWord(UpdateSelfCustomerPassWordRequestDto req)
        {
            UpdateSelfCustomerPassWordResponseDto upwd = new UpdateSelfCustomerPassWordResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                upwd.Code = (int)CodeEnum.ReqNoToken;
                upwd.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                ResponseBaseDto dto = customerBLL.UpdateSelfCustomerPassWord(req.OldPassWord, req.NewPassWord, customerToken);
                upwd.Code = dto.Code;
                upwd.Message = dto.Message;
            }
            return upwd;
        }
        //前台管理员修改主用户的安全信息
        public UpdatePrimarySafeByManagerAccountResponseDto UpdatePrimarySafeByManagerAccount(UpdatePrimarySafeByManagerAccountRequestDto req) 
        {
            UpdatePrimarySafeByManagerAccountResponseDto upsm = new UpdatePrimarySafeByManagerAccountResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                upsm.Code = (int)CodeEnum.ReqNoToken;
                upsm.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Model.Entities.Customer primaryCustomer=new Model.Entities.Customer();
                primaryCustomer.CustomerId = req.PrimaryCustomerId;
                ResponseBaseDto dto =
                    customerBLL.UpdatePrimarySafeByManagerAccount(primaryCustomer, req.LoginTypes, req.IsModify, req.ValidTime, customerToken);
                upsm.Code = dto.Code;
                upsm.Message = dto.Message;
            }
            return upsm;
        }
        //前台管理员重置主用户的密码
        public UpdatePrimaryPassWordByManagerAccountResponseDto UpdatePrimaryPassWordByManagerAccount(UpdatePrimaryPassWordByManagerAccountRequestDto req) 
        {
            UpdatePrimaryPassWordByManagerAccountResponseDto upwd = new UpdatePrimaryPassWordByManagerAccountResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                upwd.Code = (int)CodeEnum.ReqNoToken;
                upwd.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Model.Entities.Customer primaryCustomer = new Model.Entities.Customer();
                primaryCustomer.CustomerId = req.PrimaryCustomerId;
                ResponseBaseDto dto =
                    customerBLL.UpdatePrimaryPassWordByManagerAccount(primaryCustomer, req.OldPassWord, req.NewPassWord, customerToken);
                upwd.Code = dto.Code;
                upwd.Message = dto.Message;
            }
            return upwd;
        }
        //前台管理员获取单一的主用户信息
        public GetSinglePrimaryCustomerResponseDto GetSinglePrimaryCustomer(GetSinglePrimaryCustomerRequestDto req) 
        {
            GetSinglePrimaryCustomerResponseDto gspc = new GetSinglePrimaryCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gspc.Code = (int)CodeEnum.ReqNoToken;
                gspc.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Bsr.Cloud.Model.Entities.Customer primaryCustomer = new Model.Entities.Customer();
                primaryCustomer.CustomerId = req.PrimaryCustomerId;
                ResponseBaseDto dto = customerBLL.GetSinglePrimaryCustomer(primaryCustomer,customerToken, ref primaryCustomer);
                if (dto.Code == 0)
                {
                    CustomerResponse cr = new CustomerResponse();
                    cr.CustomerId = primaryCustomer.CustomerId;
                    cr.CustomerName = primaryCustomer.CustomerName;
                    cr.SignInType = primaryCustomer.SignInType;
                    cr.ReceiverName = primaryCustomer.ReceiverName;
                    cr.ReceiverEmail = primaryCustomer.ReceiverEmail;
                    cr.ReceiverCellPhone = primaryCustomer.ReceiverCellPhone;
                    cr.AccountIDNumber = primaryCustomer.AccountIDNumber;
                    cr.AccountTelephone = primaryCustomer.AccountTelephone;
                    cr.AccountCompanyAddress = primaryCustomer.AccountCompanyAddress;
                    cr.IsEnable = primaryCustomer.IsEnable;
                    cr.AccountHomeAddress = primaryCustomer.AccountHomeAddress;
                    cr.ImagePath = @"customerImage/" + primaryCustomer.ImagePath;
                    cr.ForbiddenTime =
                        primaryCustomer.ForbiddenTime == DateTime.MinValue ? string.Format("永久有效") : cr.ForbiddenTime = primaryCustomer.ForbiddenTime.ToString();
                    cr.ValidTime =
                        primaryCustomer.ValidTime == DateTime.MinValue ? string.Format("永久有效") : cr.ValidTime = primaryCustomer.ValidTime.ToString();
                    cr.LoginTypes = primaryCustomer.LoginTypes;
                    gspc.customerReponse = cr;
                }
                gspc.Code = dto.Code;
                gspc.Message = dto.Message;

            }
            return gspc;
        }
        //前台管理员账号将设备授权给主用户
        public AuthorizePrimaryByManagerAccountResponseDto AuthorizePrimaryByManagerAccount(AuthorizePrimaryByManagerAccountRequestDto req)
        {
            AuthorizePrimaryByManagerAccountResponseDto apbm = new AuthorizePrimaryByManagerAccountResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                apbm.Code = (int)CodeEnum.ReqNoToken;
                apbm.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                  ResponseBaseDto dto =
                      customerBLL.AuthorizePrimaryByManagerAccount(req.authorizeCustomerResponseList, customerToken);
                apbm.Code = dto.Code;
                apbm.Message = dto.Message;
            }
            return apbm;
        }
        //前台管理员获取其他主用户所有设备授权信息
        public GetAuthorizeOtherPrimaryResponseDto GetAuthorizeOtherPrimary(GetAuthorizeOtherPrimaryRequestDto req) 
        {
            GetAuthorizeOtherPrimaryResponseDto gspr = new GetAuthorizeOtherPrimaryResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gspr.Code = (int)CodeEnum.ReqNoToken;
                gspr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Bsr.Cloud.Model.Entities.Customer primaryCustomer = new Model.Entities.Customer();
                primaryCustomer.CustomerId = req.PrimaryCustomerId;
                List<Model.AuthorizePrimaryResponse> authorizePrimaryResponseFlag =new List<AuthorizePrimaryResponse>();
                ResponseBaseDto dto = customerBLL.GetAuthorizeOtherPrimary(primaryCustomer,customerToken,ref authorizePrimaryResponseFlag);
                if (dto.Code == 0)
                {
                    gspr.authorizeCustomerResponseList = authorizePrimaryResponseFlag;
                }
                gspr.Code = dto.Code;
                gspr.Message = dto.Message;

            }
            return gspr;
        }
        //前台管理员冻结与解除冻结主用户
        public EnablePrimaryCustomerResponseDto EnablePrimaryCustomer(EnablePrimaryCustomerRequestDto req) 
        {
            EnablePrimaryCustomerResponseDto cfr = new EnablePrimaryCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                cfr.Code = (int)CodeEnum.ReqNoToken;
                cfr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Model.Entities.Customer primaryCustomer = new Model.Entities.Customer();
                primaryCustomer.CustomerId = req.PrimaryCustomerId;
                ResponseBaseDto dto = customerBLL.EnablePrimaryCustomer(primaryCustomer, req.IsEnable, customerToken);
                cfr.Code = dto.Code;
                cfr.Message = dto.Message;
            }
            return cfr;
        }
        //删除1-n 的主用户
        public DeletePrimaryCustomerResponseDto DeletePrimaryCustomer(DeletePrimaryCustomerRequestDto req) 
        {
            DeletePrimaryCustomerResponseDto dscr = new DeletePrimaryCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                dscr.Code = (int)CodeEnum.ReqNoToken;
                dscr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                ResponseBaseDto dto = customerBLL.DeleteCustomerById(req.PrimaryCustomerId, customerToken);
                dscr.Code = dto.Code;
                dscr.Message = dto.Message;
            }
            return dscr;

        }
        //删除1-n 的子用户
        public DeleteSubCustomerResponseDto DeleteSubCustomer(DeleteSubCustomerRequestDto req)
        {
            DeleteSubCustomerResponseDto dscr = new DeleteSubCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                dscr.Code = (int)CodeEnum.ReqNoToken;
                dscr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                ResponseBaseDto dto = customerBLL.DeleteCustomerById(req.SubCustomer, customerToken);
                dscr.Code = dto.Code;
                dscr.Message = dto.Message;
            }
            return dscr;
        }
        //前台管理员更新主用户基本信息
        public UpdatePrimaryCustomerResponseDto UpdatePrimaryCustomer(UpdatePrimaryCustomerRequestDto req) 
        {
            UpdatePrimaryCustomerResponseDto upcr = new UpdatePrimaryCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                upcr.Code = (int)CodeEnum.ReqNoToken;
                upcr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                string imageBase64 = req.ImageByteBase64;
                string extName = req.ExtName;
                byte[] bin = null;
                if (imageBase64 != null && imageBase64 != "")
                {
                    if (extName == null || extName.Length == 0)
                    {
                        extName = "jpg";
                    }
                    // 解码图片二进制数据, bin为还原后的Byte[]
                    bin = Base64.FromBase64ToByte(imageBase64);
                }
                //code
                Bsr.Cloud.Model.Entities.Customer primaryCustomer = new Model.Entities.Customer();
                primaryCustomer.CustomerId = req.PrimaryCustomerId;
                primaryCustomer.ReceiverName = req.ReceiverName;
                primaryCustomer.ReceiverCellPhone = req.ReceiverCellPhone;
                primaryCustomer.AccountCompanyAddress = req.AccountCompanyAddress;
                primaryCustomer.AccountTelephone = req.AccountTelephone;
                primaryCustomer.ReceiverEmail = req.ReceiverEmail;
                string imagePath = string.Empty;
                ResponseBaseDto dto = customerBLL.UpdatePrimaryCustomer(primaryCustomer, bin, extName, customerToken, ref imagePath);
                upcr.Code = dto.Code;
                upcr.Message = dto.Message;
                upcr.ImagePath = @"customerImage/" + imagePath;
            }
            return upcr;
        }
        //检索主用户下的所有子用户信息
        public SearchSubCustomerResponseDto SearchSubCustomer(SearchSubCustomerRequestDto req) 
        {
            SearchSubCustomerResponseDto sscr = new SearchSubCustomerResponseDto();
            IList<Bsr.Cloud.Model.Entities.Customer> customerFlag = new List<Bsr.Cloud.Model.Entities.Customer>();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                sscr.Code = (int)CodeEnum.ReqNoToken;
                sscr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                List<CustomerResponse> customerList = new List<CustomerResponse>();
                ResponseBaseDto dto = customerBLL.SearchSubCustomer(req.KeyWord, customerToken, ref customerFlag);
                for (int i = 0; i < customerFlag.Count; i++)
                {
                    Bsr.Cloud.Model.Entities.Customer subCustomer = customerFlag[i];
                    CustomerResponse cr = new CustomerResponse();
                    cr.CustomerId = subCustomer.CustomerId;
                    cr.CustomerName = subCustomer.CustomerName;
                    cr.SignInType = subCustomer.SignInType;
                    cr.ReceiverName = subCustomer.ReceiverName;
                    cr.ReceiverEmail = subCustomer.ReceiverEmail;
                    cr.ReceiverCellPhone = subCustomer.ReceiverCellPhone;
                    cr.AccountIDNumber = subCustomer.AccountIDNumber;
                    cr.AccountTelephone = subCustomer.AccountTelephone;
                    cr.AccountCompanyAddress = subCustomer.AccountCompanyAddress;
                    cr.IsEnable = subCustomer.IsEnable;
                    cr.AccountHomeAddress = subCustomer.AccountHomeAddress;
                    cr.ImagePath = @"customerImage/" + subCustomer.ImagePath;
                    cr.ForbiddenTime =
                        subCustomer.ForbiddenTime == DateTime.MinValue ? string.Format("永久有效") : cr.ForbiddenTime = subCustomer.ForbiddenTime.ToString();
                    cr.ValidTime =
                           subCustomer.ValidTime == DateTime.MinValue ? string.Format("永久有效") : cr.ValidTime = subCustomer.ValidTime.ToString();
                    cr.LoginTypes = subCustomer.LoginTypes;
                    customerList.Add(cr);
                }
                sscr.customerReponseList = customerList;
                sscr.Code = dto.Code;
                sscr.Message = dto.Message;
            }
            return sscr;
        }
        //主用户修改子用户的安全信息
        public UpdateSubSafeByPrimaryResponseDto UpdateSubSafeByPrimary(UpdateSubSafeByPrimaryRequestDto req) 
        {
            UpdateSubSafeByPrimaryResponseDto upsm = new UpdateSubSafeByPrimaryResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                upsm.Code = (int)CodeEnum.ReqNoToken;
                upsm.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Model.Entities.Customer subCustomer = new Model.Entities.Customer();
                subCustomer.CustomerId = req.SubCustomerId;
                
                ResponseBaseDto dto =
                    customerBLL.UpdateSubSafeByPrimary(subCustomer, req.LoginTypes, req.IsModify, req.ValidTime, customerToken);
                upsm.Code = dto.Code;
                upsm.Message = dto.Message;
            }
            return upsm;
        }
        //主用户重置子用户密码
        public UpdateSubPassWordByPrimaryResponseDto UpdateSubPassWordByPrimary(UpdateSubPassWordByPrimaryRequestDto req) 
        {
            UpdateSubPassWordByPrimaryResponseDto suwd = new UpdateSubPassWordByPrimaryResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                suwd.Code = (int)CodeEnum.ReqNoToken;
                suwd.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Model.Entities.Customer subCustomer = new Model.Entities.Customer();
                subCustomer.CustomerId = req.SubCustomerId;
                ResponseBaseDto dto =
                    customerBLL.UpdateSubPassWordByPrimary(subCustomer, req.OldPassWord, req.NewPassWord, customerToken);
                suwd.Code = dto.Code;
                suwd.Message = dto.Message;
            }
            return suwd;
        }
        //主用户获取子用户的授权信息
        public GetAuthorizeSubCustomerResponseDto GetAuthorizeSubCustomer(GetAuthorizeSubCustomerRequestDto req) 
        {
            GetAuthorizeSubCustomerResponseDto gspr = new GetAuthorizeSubCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gspr.Code = (int)CodeEnum.ReqNoToken;
                gspr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Bsr.Cloud.Model.Entities.Customer subCustomer = new Model.Entities.Customer();
                subCustomer.CustomerId = req.SubCustomerId;
                Model.AuthorizeSubResponse authorizeSubResponse = new Model.AuthorizeSubResponse();
                ResponseBaseDto dto = customerBLL.GetAuthorizeSubCustomer(subCustomer, customerToken, ref authorizeSubResponse);
                if (dto.Code == 0)
                {
                    gspr.authorizeSubResponse= authorizeSubResponse;
                }
                gspr.Code = dto.Code;
                gspr.Message = dto.Message;

            }
            return gspr;
        }
        //主用户将权限设定给子用户
        public AuthorizeSubByPrimaryResponseDto AuthorizeSubByPrimary(AuthorizeSubByPrimaryRequestDto req) 
        {
            AuthorizeSubByPrimaryResponseDto spbr = new AuthorizeSubByPrimaryResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                spbr.Code = (int)CodeEnum.ReqNoToken;
                spbr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                ResponseBaseDto dto =
                    customerBLL.AuthorizeSubByPrimary(req.authorizeSubResponse, customerToken);
                spbr.Code = dto.Code;
                spbr.Message = dto.Message;
            }
            return spbr;
        }
        //主用更新子用户的基本信息
        public UpdateSubCustomerResponseDto UpdateSubCustomer(UpdateSubCustomerRequestDto req) 
        {
            UpdateSubCustomerResponseDto uscr = new UpdateSubCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                uscr.Code = (int)CodeEnum.ReqNoToken;
                uscr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                string imageBase64 = req.ImageByteBase64;
                string extName = req.ExtName;
                byte[] bin = null;
                if (imageBase64 != null && imageBase64 != "")
                {
                    if (extName == null || extName.Length == 0)
                    {
                        extName = "jpg";
                    }
                    // 解码图片二进制数据, bin为还原后的Byte[]
                    bin = Base64.FromBase64ToByte(imageBase64);
                }

                //code
                Bsr.Cloud.Model.Entities.Customer subCustomer = new Model.Entities.Customer();
                subCustomer.CustomerId = req.SubCustomerId;
                subCustomer.ReceiverName = req.ReceiverName;
                subCustomer.ReceiverCellPhone = req.ReceiverCellPhone;
                subCustomer.AccountCompanyAddress = req.AccountCompanyAddress;
                subCustomer.AccountTelephone = req.AccountTelephone;
                subCustomer.ReceiverEmail = req.ReceiverEmail;
                string imagePath = string.Empty;
                ResponseBaseDto dto = customerBLL.UpdateSubCustomer(subCustomer, bin, extName, customerToken, ref imagePath);
                uscr.Code = dto.Code;
                uscr.Message = dto.Message;
                uscr.ImagePath = @"customerImage/" + imagePath;
            }
            return uscr;
        }
        //输出用户权限集合
        public GetCustomerPermissionResponseDto GetCustomerPermission(GetCustomerPermissionRequestDto req) 
        {
            GetCustomerPermissionResponseDto gcpr = new GetCustomerPermissionResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gcpr.Code = (int)CodeEnum.ReqNoToken;
                gcpr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                List<CustomerPermissionName> pcList = new List<CustomerPermissionName>();
                foreach (int cpKey in Enum.GetValues(typeof(PermissionCustomerEnum)))
                {
                    CustomerPermissionName pce = new CustomerPermissionName();
                    pce.CustomerPermissionKey = cpKey;
                    pce.CustomerPermissionValue = Enum.GetName(typeof(PermissionCustomerEnum), cpKey);
                    pcList.Add(pce);
                }
                gcpr.Code = (int)CodeEnum.Success;
                gcpr.Message = "权限已获取完成！";
                gcpr.customerPermissionName = pcList;
            }
            return gcpr;
        }
        //发送一个临时的检验码至手机
        public SendRegisterUserWithSumResponseDto SendRegisterUserWithSum(SendRegisterUserWithSumRequestDto req) 
        {
            SendRegisterUserWithSumResponseDto smws = new SendRegisterUserWithSumResponseDto();
            int isValid = 0;
            ResponseBaseDto dto =
                customerBLL.SendMsgWithSum(req.PhoneNum,(int)MsgNoticeTypeEnum.RegisterUser, ref isValid);
            smws.Code = dto.Code;
            smws.Message = dto.Message;
            smws.IsValid = isValid;
            return smws;
        }
        //验证一个检验码是否有效（注册）
        public CheckRegisterUserWithSumResponseDto CheckRegisterUserWithSum(CheckRegisterUserWithSumRequestDto req) 
        {
            CheckRegisterUserWithSumResponseDto cwsr = new CheckRegisterUserWithSumResponseDto();
            int IsValid = 0;
            string verifyToken = string.Empty;
            ResponseBaseDto dto =
                customerBLL.CheckingWithSum(req.PhoneNum, req.VerifyKey, (int)MsgNoticeTypeEnum.RegisterUser, ref IsValid);
            cwsr.Code = dto.Code;
            cwsr.Message = dto.Message;
            cwsr.IsValid = IsValid;
            return cwsr;
        }
        //重置用户的密码(找回密码)
        public UpdateCustomerPassWordResponseDto UpdateCustomerPassWord(UpdateCustomerPassWordRequestDto req) 
        {
            UpdateCustomerPassWordResponseDto ucpw = new UpdateCustomerPassWordResponseDto();
            ResponseBaseDto dto = customerBLL.UpdateCustomerPassWord(req.NewPassWord,req.VerifyToken);
            ucpw.Code = dto.Code;
            ucpw.Message = dto.Message;
            return ucpw;
        }
        //用户（用户名,手机号,邮箱）是否被注册过
        public IsRegisterByLoginNameResponseDto IsRegisterByLoginName(IsRegisterByLoginNameRequestDto req)
        {
            IsRegisterByLoginNameResponseDto irbl = new IsRegisterByLoginNameResponseDto();
            int isRegister=0;//默认没有被注册过
            ResponseBaseDto dto =
               customerBLL.IsRegisterByLoginName(req.LoginName,ref isRegister);
            irbl.Code = dto.Code;
            irbl.Message = dto.Message;
            irbl.IsRegister = isRegister;
            return irbl;
        }
        //发送验证码（用户名，手机或者邮箱）（找回密码）
        public SendFindPasswordVerifyKeyResponseDto SendVerifyKey(SendFindPasswordVerifyKeyRequestDto req) 
        {
            SendFindPasswordVerifyKeyResponseDto svkr = new SendFindPasswordVerifyKeyResponseDto();
            int isValid = 0;
            ResponseBaseDto dto =
               customerBLL.SendVerifyKey(req.LoginName, ref isValid);
            svkr.Code = dto.Code;
            svkr.Message = dto.Message;
            svkr.IsValid = isValid;
            return svkr;
        }

        //验证验证码(找回密码)
        public CheckFindPasswordVerfyKeyResponseDto CheckFindPasswordVerfyKey(CheckFindPasswordVerfyKeyRequestDto req) 
        {
            CheckFindPasswordVerfyKeyResponseDto cfvk = new CheckFindPasswordVerfyKeyResponseDto();
            int isVaild=0;
            string verifyToken="";
            ResponseBaseDto dto =
               customerBLL.CheckVerfyKey(req.LoginName,req.VerifyKey,ref isVaild,ref verifyToken);
            cfvk.Code = dto.Code;
            cfvk.Message = dto.Message;
            cfvk.IsValid = isVaild;
            cfvk.VerifyToken = verifyToken;
            return cfvk;
        }

        //获取当前在线用户
        public GetCustomerOnlineTotalResponseDto GetCustomerOnlineTotal(GetCustomerOnlineTotalRequestDto req) 
        {
            GetCustomerOnlineTotalResponseDto gcot = new GetCustomerOnlineTotalResponseDto();
            IList<TokenCacheProperty>  tokenCachePropertyFlag=new  List<TokenCacheProperty>();
            ResponseBaseDto dto =
                           customerBLL.GetCustomerOnlineTotal(ref tokenCachePropertyFlag);
            gcot.Code = dto.Code;
            gcot.Message = dto.Message;
            gcot.tokenCachePropertyList = (List<TokenCacheProperty>)tokenCachePropertyFlag;
            return gcot;
        }


        //主用户获取单一的子用户信息
        public GetSingleSubCustomerResponseDto GetSingleSubCustomer(GetSingleSubCustomerRequestDto req)
        {
            GetSingleSubCustomerResponseDto gscr = new GetSingleSubCustomerResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gscr.Code = (int)CodeEnum.ReqNoToken;
                gscr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                Bsr.Cloud.Model.Entities.Customer subCustomer = new Model.Entities.Customer();
                subCustomer.CustomerId = req.SubCustomerId;
                ResponseBaseDto dto = customerBLL.GetSingleSubCustomer(subCustomer, customerToken, ref subCustomer);
                if (dto.Code == 0)
                {
                    CustomerResponse cr = new CustomerResponse();
                    cr.CustomerId = subCustomer.CustomerId;
                    cr.CustomerName = subCustomer.CustomerName;
                    cr.SignInType = subCustomer.SignInType;
                    cr.ReceiverName = subCustomer.ReceiverName;
                    cr.ReceiverEmail = subCustomer.ReceiverEmail;
                    cr.ReceiverCellPhone = subCustomer.ReceiverCellPhone;
                    cr.AccountIDNumber = subCustomer.AccountIDNumber;
                    cr.AccountTelephone = subCustomer.AccountTelephone;
                    cr.AccountCompanyAddress = subCustomer.AccountCompanyAddress;
                    cr.IsEnable = subCustomer.IsEnable;
                    cr.AccountHomeAddress = subCustomer.AccountHomeAddress;
                    cr.ImagePath = @"customerImage/" + subCustomer.ImagePath;
                    cr.ForbiddenTime =
                        subCustomer.ForbiddenTime == DateTime.MinValue ? string.Format("永久有效") : cr.ForbiddenTime = subCustomer.ForbiddenTime.ToString();
                    cr.ValidTime =
                        subCustomer.ValidTime == DateTime.MinValue ? string.Format("永久有效") : cr.ValidTime = subCustomer.ValidTime.ToString();
                    cr.LoginTypes = subCustomer.LoginTypes;
                    gscr.customerReponse = cr;
                }
                gscr.Code = dto.Code;
                gscr.Message = dto.Message;
            }
            return gscr;
        }

        //获取当前用户访问权限
        public GetSelfPermissionResponseDto GetSelfPermission(GetSelfPermissionRequestDto req) 
        {
            GetSelfPermissionResponseDto gspr = new GetSelfPermissionResponseDto();
            string customerToken = "";
            if (!RestHelper.SecurityCheck(ref customerToken))
            {
                gspr.Code = (int)CodeEnum.ReqNoToken;
                gspr.Message = RestHelper.SecNoTokenMessage;
            }
            else
            {
                IList<Permission> permissionFlag = new List<Permission>();
                ResponseBaseDto dto= customerBLL.GetSelfPermission(customerToken,ref permissionFlag);
                gspr.Code = dto.Code;
                gspr.Message = dto.Message;
                gspr.customerPermission =(List<Permission>)permissionFlag;
            }
            return gspr;
        }


        #region private 成员

        static private ILogger myLog = new Logger<Customer>();
        private CustomerBLL customerBLL = new CustomerBLL();
        private UserTokenCache userTokenCache = UserTokenCache.GetInstance();

        /// <summary>
        /// 获取发送消息的客户端的 IP 地址。
        /// </summary>
        /// <returns>ip地址信息</returns>
        private string GetRemoteEndpointAddress()
        {
            try
            {
                // 提供方法执行的上下文环境  
                OperationContext context = OperationContext.Current;
                // 获取传进的消息属性  
                MessageProperties properties = context.IncomingMessageProperties;
                // 获取消息发送的远程终结点IP和端口  
                RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

                return endpoint.Address;
            }
            catch (Exception ex)
            {
                return "Unknown";
            }
        }
        #endregion

    }
}