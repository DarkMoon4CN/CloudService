using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model.Entities;
using Bsr.Cloud.Model;
using Bsr.Cloud.Core;
using Bsr.Core.Hibernate.WCF;
using System.Collections;

namespace Bsr.Cloud.BLogic.BLL
{
    public class CustomerBLL
    {
        CustomerServer customerServer = CustomerServer.GetInstance();
        ChannelServer channelServer = ChannelServer.GetInstance();
        DeviceServer deviceServer = DeviceServer.GetInstance();
        ResourceGroupServer resourceGroupServer = ResourceGroupServer.GetInstance();
        GroupChannelServer groupChannelServer = GroupChannelServer.GetInstance();
        PermissionServer permissionServer = PermissionServer.GetInstance();
        UserFavoriteServer userFavoriteServer = UserFavoriteServer.GetInstance();
        OperaterLogServer operaterLogServer = OperaterLogServer.GetInstance();
        CellphoneMsgNotice cellphoneMsgNotice = CellphoneMsgNotice.GetInstance();
        DeviceBLL deviceBLL = new DeviceBLL();
        public BLLHelper bllHelper = new BLLHelper(); 

        public CustomerBLL() {
           // Log4NetOutPut.OutPutInit();
        }

        static private ILogger myLog = new Logger<CustomerBLL>();


        #region  登陆 SignIn
        /// <summary>
        /// 登录实现
        /// </summary>
        /// <param name="loginName">登陆使用的用户名称/手机/邮箱</param>
        /// <param name="password">登陆密码</param>
        /// <param name="loginType">登陆来源</param>
        /// <param name="customerId">返回参数 用户ID</param>
        /// <param name="agentVersion">客户端程序版本</param>
        /// <param name="remoteEndpointAddress">客户端IP地址</param>
        /// <param name="customerName">返回参数 用户名称</param>
        /// <param name="customerToken">返回参数 用户Token</param>
        /// <returns>登录的应答结果</returns>
        public ResponseBaseDto SignIn(string loginName, string password, int loginType,
            string agentVersion, string remoteEndpointAddress,
            ref int customerId,ref string customerName,
            ref int signInType,ref string customerToken)
        {
            UserTokenCache utc = UserTokenCache.GetInstance();        
            ResponseBaseDto rbd = new ResponseBaseDto();
            TokenCacheProperty tcp = new TokenCacheProperty();
            OperaterLog oLog = new OperaterLog();//用户操作日志
            oLog.Action = "登录";
            IList<Customer> customerFlag = null;
            Customer customer = new Customer();
            customer.Password = password;
            //判定用户使用用户名，邮箱或者移动电话登录
            if (loginName.Length == 11 && CheckCellPhone(loginName)==true) 
            {
                //并且可以转为int类型 手机                 
                customer.ReceiverCellPhone = loginName;
                customerFlag = customerServer.SelectCustomerByReceiverPhone(customer);
          
            }
            else if (loginName.IndexOf("@") != -1)
            {
                //邮箱
                customer.ReceiverEmail = loginName;
                customerFlag = customerServer.SelectCustomerByReceiverEmail(customer);   
            }
            else 
            {
                //用户名
                customer.CustomerName = loginName;
                customerFlag = customerServer.SelectCustomerByCustomerName(customer);
            }
            try
            {
                if (customerFlag != null && customerFlag.Count == 1)
                {
                    //主账户冻结时间超过一年
                    DateTime forBiddenTime = customerFlag[0].ForbiddenTime;
                    DateTime validTime = customerFlag[0].ValidTime;
                    if (forBiddenTime != null
                          && forBiddenTime > DateTime.MinValue && forBiddenTime.AddYears(1) < DateTime.Now)
                    {
                        //清理超过冻结一年的主用户
                        if (customerFlag[0].SignInType == (int)CustomerSignInTypeEnum.PrimaryCustomer)
                        {
                            ClearCustomer(customerFlag[0]);
                        }
                        rbd.Code = (int)CodeEnum.NoUser;
                        rbd.Message = "此用户已被清理，请重新注册!";
                        return rbd;
                    }
                    else if (validTime != null
                               && validTime > DateTime.MinValue && validTime < DateTime.Now) 
                    {
                        if (customerFlag[0].SignInType == (int)CustomerSignInTypeEnum.PrimaryCustomer)
                        {
                            // 当账户有效期结束，则设置现在时间为:冻结时间
                            ForBiddenCustomer(customerFlag[0], 0);
                        }
                        rbd.Code = (int)CodeEnum.NoUser;
                        rbd.Message = "冻结状态无法登陆，请联系管理员解除冻结！";
                        return rbd;
                    }
                    else if (customerFlag[0].IsEnable == 0)
                    {
                        rbd.Code = (int)CodeEnum.NoUser;
                        rbd.Message = "该用户已被禁用，请联系管理员";
                        return rbd;
                    }
                    else if (customerFlag[0].LoginTypes != null
                            && customerFlag[0].LoginTypes.IndexOf(loginType.ToString()) == -1) 
                    {
                        rbd.Code = (int)CodeEnum.NoUser;
                        switch (signInType)
                        {
                            case 1: rbd.Message = "WEB";  break;
                            case 2: rbd.Message = "C/S";  break;
                            case 3: rbd.Message = "手机"; break;
                            case 4: rbd.Message = "手机"; break;
                        }
                        rbd.Message +=" 端方式不允许登陆,请选用其他方式登录";
                        return rbd;
                    }
                    else if (customerFlag[0].Password.ToLower() == customer.Password.ToLower())
                    {
                        // token格式：用户id_用户登录类型_时间
                        string tokenText = string.Format("{0}_{1}_{2}",
                                           customerFlag[0].CustomerId, loginType, DateTime.Now.ToBinary());
                        // 加密token
                        customerToken = "customer_";
                        customerToken+= MDKey.Encrypt(tokenText);

                        customerName = customerFlag[0].CustomerName;
                        signInType = customerFlag[0].SignInType;
                        customerId = customerFlag[0].CustomerId;

                        //向UserTokenCache 发送添加请求
                        tcp.ParentId = customerFlag[0].ParentId;
                        tcp.CustomerId = customerId;
                        tcp.CustomerName = customerName;
                        tcp.LoginType = loginType;
                        tcp.SignInType = signInType;
                        tcp.AgentVersion = agentVersion;
                        tcp.IsEnable = customerFlag[0].IsEnable;
                        tcp.LastAccessTime = DateTime.Now;
                        tcp.remoteEndpointAddress = remoteEndpointAddress;

                        //设定时间参数设定120秒后自动移除此用户信息
                        DateTime dt = DateTime.Now.AddSeconds(120);
                        myLog.WarnFormat("{0} 登录，账号验证通过", customer.CustomerName);
                        int CacheState = utc.AddToken(customerToken, tcp, dt);
                        if (CacheState != 0)
                        {
                            myLog.WarnFormat("缓存服务器存入失败,时间：{0}", dt);
                            rbd.Code = (int)CodeEnum.ApplicationErr;
                            rbd.Message = "服务器异常，请联系技术人员";
                        }
                        else
                        {
                            rbd.Code = (int)CodeEnum.Success;
                            rbd.Message = "登陆成功,跳转中...";
                            oLog.Remarks = "登录已成功，来自: " + remoteEndpointAddress;
                        }
                    }
                    else
                    {
                        rbd.Code = (int)CodeEnum.NoUser;
                        rbd.Message = "用户密码输入错误";
                    }
                }
                else
                {
                    rbd.Code = (int)CodeEnum.InvalidPassword;
                    rbd.Message = "用户名不存在，请确认后输入";
                }
            }
            catch(Exception ex) 
            {
                rbd.Code = (int)CodeEnum.ApplicationErr;
                rbd.Message = "数据请求异常";
                myLog.WarnFormat("Login方法异常,登陆名:{0},登陆类型:{1},登陆IP:{2}", ex, loginName, loginType, remoteEndpointAddress);
            }
            oLog.Result = rbd.Code;
            
            bllHelper.AddOperaterLog(oLog,tcp);
            return rbd;
        }
        #endregion

        #region  注销 SignOut
        /// <summary>
        /// 服务器端注销
        /// </summary>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto SignOut(string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            oLog.Action = "注销";
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
                    oLog.Result = dto.Code;
                    oLog.Remarks = dto.Message;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else
                {
                    int bFlag = utc.RemoveToken(customerToken);
                    dto.Code = bFlag;
                    if (dto.Code == 0)
                    {
                        dto.Message = "已安全退出";
                    }
                    else
                    {
                        dto.Message = "退出时出现未知错误";
                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！";
                myLog.WarnFormat("SignOut方法异常,用户Id:{0}", ex,tcp.CustomerId);
            }
            oLog.Result = dto.Code;
            oLog.Remarks = dto.Message;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region  检查用户是否存在
        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <returns></returns>
        public ResponseBaseDto GetCustomerName(Customer customer, ref int isUse)
        {
            ResponseBaseDto rbd = new ResponseBaseDto();
            try
            {

                IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerName(customer);
                if (customerFlag != null && customerFlag.Count> 0)
                {
                    rbd.Code = (int)CodeEnum.Success;
                    rbd.Message = "此用户名已被注册";
                    isUse = 0;
                }
                else
                {
                    rbd.Code = (int)CodeEnum.Success;
                    rbd.Message = "用户名可以使用";
                    isUse = 1;
                }
            }
            catch(Exception ex)
            {
                 rbd.Code = (int)CodeEnum.ApplicationErr;
                 rbd.Message = "网络异常，请刷新页面后";
                 myLog.WarnFormat("GetCUstomerName方法异常,用户名:{0}",ex,customer.CustomerName);
            }
            return rbd;
        }
        #endregion

        #region  检查邮箱是否存在
        /// <summary>
        /// 检查邮箱是否存在
        /// </summary>
        /// <returns></returns>
        public ResponseBaseDto GetReceiverEmail(Customer customer, ref int isUse)
        {
            ResponseBaseDto rbd = new ResponseBaseDto();
            try
            {
                IList<Customer> customerFlag = customerServer.SelectCustomerByReceiverEmail(customer);
                if (customerFlag != null && customerFlag.Count > 0)
                {
                    rbd.Code = (int)CodeEnum.Success;
                    rbd.Message = "此邮箱已被注册";
                    isUse = 0;
                }
                else
                {
                    rbd.Code = (int)CodeEnum.Success;
                    rbd.Message = "此邮箱可以使用";
                    isUse = 1;
                }
            }
            catch(Exception ex)
            {
                rbd.Code = (int)CodeEnum.ApplicationErr;
                rbd.Message = "网络异常，请刷新页面后继续";
                myLog.WarnFormat("GetReceiverEmail方法异常,邮箱:{0}", ex, customer.ReceiverEmail);
            }
            return rbd;
        }
        #endregion

        #region  检查手机是否存在
        /// <summary>
        /// 检查手机是否存在
        /// </summary>
        /// <returns></returns>
        public ResponseBaseDto GetReceiverPhone(Customer customer, ref int isUse)
        {
            ResponseBaseDto rbd = new ResponseBaseDto();
            try
            {
                IList<Customer> customerFlag = customerServer.SelectCustomerByReceiverPhone(customer);
                if (customerFlag != null && customerFlag.Count >0 )
                {
                    rbd.Code = (int)CodeEnum.Success;
                    rbd.Message = "此手机已被注册";
                    isUse = 0;
                }
                else
                {
                    rbd.Code = (int)CodeEnum.Success;
                    rbd.Message = "此手机可以使用";
                    isUse = 1;
                }
            }
            catch(Exception ex)
            {
                rbd.Code = (int)CodeEnum.ApplicationErr;
                rbd.Message = "网络异常，请刷新页面后继续";
                myLog.WarnFormat("GetReceiverPhone方法异常,手机:{0}", ex, customer.ReceiverCellPhone);
            }
            return rbd;
        }
        #endregion

        #region   定时校验 TimingCheck
        /// <summary>
        ///  定时校验用户token
        /// </summary>
        /// <returns></returns>
        public static ResponseBaseDto TimingCheck(string customerToken)
        {
            ResponseBaseDto rbd = new ResponseBaseDto();
            TokenCacheProperty tcp = new TokenCacheProperty();
            try
            {
                //查找出UserTokenCache的token             
                UserTokenCache utc = UserTokenCache.GetInstance();
                //根据token获取用户信息
                tcp = utc.FindByCustomerToken(customerToken);
                if(tcp == null)
                {
                    rbd.Code = (int)CodeEnum.ServerNoToken;
                    rbd.Message = "Token 已失效";
                }
                else
                {
                    //完成操作
                    rbd.Code = (int)CodeEnum.Success;
                    rbd.Message = "心跳接收完成";
                    //更新缓存服务器中的最后更新时间
                    tcp.LastAccessTime = DateTime.Now;
                    utc.RefreshAccessTime(customerToken, tcp, DateTime.Now.AddSeconds(120));  
                } 
            }
            catch(Exception ex)
            {
                rbd.Code = (int)CodeEnum.ApplicationErr;
                rbd.Message = "网络异常，请刷新页面后操作";
                myLog.WarnFormat("TimingCheck方法异常,用户id:{0}",ex,tcp.CustomerId);
            }
            return rbd;
        }
        #endregion

        /*主用户对子用户 start*/
        #region  主账户对子账户冻结或者解冻 EnableSubCustomer
        /// <summary>
        ///  由主账号去执行子账号下的IsEnable
        /// </summary>
        /// <returns>0 执行完成 其他 失败</returns>
        public ResponseBaseDto EnableSubCustomer(int subCstomerId, int isEnable, string customerToken)
        {
            Customer customer = new Customer();
            customer.CustomerId = subCstomerId;
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp=new TokenCacheProperty();
            oLog.Action = "解冻或冻结子用户";
            ResponseBaseDto dto = new ResponseBaseDto();
            try
            {
                //找出需要冻结的子账户CustomerId信息
                IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(customer);
                oLog.TargetId = subCstomerId;
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
                else 
                {
                    if (customerFlag != null && customerFlag.Count == 1)
                    {
                        customer.ParentId = tcp.CustomerId;
                        customer.CustomerId = subCstomerId;
                        customer.IsEnable = isEnable;
                        //子账户下的ParentId于请求的父级Id对比
                        if (customerFlag[0].ParentId == customer.ParentId)
                        {
                            if (ForBiddenCustomer(customerFlag[0], isEnable))
                            {
                                dto.Code = (int)CodeEnum.Success;
                                dto.Message = isEnable == 1 ? "解除冻结已完成！" : "已将用户冻结！";
                            }
                            else
                            {
                                dto.Code = (int)CodeEnum.NoUser;
                                dto.Message = isEnable == 1 ? "解除冻结失败！" : "用户冻结失败！";
                            }
                            oLog.Remarks = "已将 子用户：" + customerFlag[0].CustomerName+dto.Message;
                        }
                        else
                        {
                            dto.Code = (int)CodeEnum.NoUser;
                            dto.Message = "没有权限";
                            oLog.Remarks = string.Format("没有权限操作{0}",customerFlag[0].CustomerName);
                        }
                    }
                    else
                    {
                        dto.Code = (int)CodeEnum.NoUser;
                        dto.Message = "没有此账户信息";
                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常,操作失败！";
                myLog.WarnFormat("EnableSubCustomer方法异常,用户id(操作者):{0},用户id(被操作者):{1}", ex,tcp.CustomerId ,subCstomerId);
            }
            oLog.Result = dto.Code;
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            bllHelper.AddOperaterLog(oLog,tcp);
            return dto;
        }
        #endregion

        #region  创建主用户 AddParentCustomerInfo
        /// <summary>
        ///  创建主用户
        /// </summary>
        /// <param name="customer">主账号对象</param>
        /// <param name="CustomerId">返回customerId</param>
        /// <returns></returns>
        public ResponseBaseDto AddParentCustomerInfo(Customer customer, ref int CustomerId)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            try
            {
                //判定必要的条件信息
                if (customer.CustomerName == null || customer.CustomerName == ""
                    || string.IsNullOrEmpty(customer.Password) || customer.Password == "") 
                {
                    dto.Code = (int)CodeEnum.NoUser;
                    dto.Message = "用户提交数据不完整！";
                    return dto;
                }
                if (string.IsNullOrEmpty(customer.ReceiverCellPhone) || customer.ReceiverCellPhone == "")
                {
                    dto.Code = (int)CodeEnum.InvalidNoCellPhone;
                    dto.Message = "用户提交数据不完整！";
                    return dto;
                }
                else
                {
                    //判定邮箱是否用可用
                    int isUse = 0;
                    dto = GetReceiverPhone(customer, ref isUse);
                    if (isUse == 0)
                    {
                        return dto;
                    }
                }
                //if (string.IsNullOrEmpty(customer.ReceiverEmail) || customer.ReceiverEmail == "")
                //{
                //    dto.Code = (int)CodeEnum.InvalidNoEmail;
                //    dto.Message = "用户提交数据不完整！";
                //    return dto;
                //}
                //else
                //{
                //    //判定邮箱是否用可用
                //    int isUse = 0;
                //    dto = GetReceiverEmail(customer, ref isUse);
                //    if (isUse == 0)
                //    {
                //        return dto;
                //    }
                //}
                //插入单条主用户信息
                customer.IsEnable = 1;
                customer.ImagePath = "default.jpg";
                customer.LoginTypes = "1,2,3,4";
                int parentCustomerId =
                    GetCustomerIdByCustomerId(new Bsr.Cloud.Model.Entities.Customer() { CustomerName = "adminF888" });
                if (parentCustomerId == -1)
                {
                    customer.ParentId = 1;
                }
                else
                {
                    customer.ParentId = parentCustomerId;
                }

                CustomerId = customerServer.InsertCustomer(customer);
                if (CustomerId > 0)
                {
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = customer.CustomerName + " 用户添加成功！";
                }
                else
                {
                    dto.Code = (int)CodeEnum.NoUser;
                    dto.Message = customer.CustomerName + "用户创建失败！请联系技术人员";
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = customer.CustomerName + "创建主账号失败！请刷新后继续";
                myLog.ErrorFormat("AddParentCustomerInfo方法异常",ex);
            }
            return dto;
        }
        #endregion

        #region  查询主账户下的所有子账户 GetSubCustomer
        /// <summary>
        ///  GetSubCustomer 查询主账户下的所有子账户
        /// </summary>
        /// <param name="customerToken"></param> 
        /// <returns></returns>
        public ResponseBaseDto GetSubCustomer(string customerToken ,ref IList<Customer> customerFlag)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
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
                if (tcp.SignInType == (int)CustomerSignInTypeEnum.PrimaryCustomer)
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "当前用户非前台主账号！";
                    return dto;
                }

                //获取子账号信息
                Customer customer = new Customer();
                customer.CustomerId=tcp.CustomerId;
                customerFlag=customerServer.SelectCustomerByParentId(customer);
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "子用户信息已获取完成！";
                
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！";
                myLog.WarnFormat("GetSubCustomer方法异常,用户Id:{0}", ex,tcp.CustomerId);
            }
            return dto;
        }
        #endregion

        #region  主用户创建子用户 AddSubCustomer
        /// <summary>
        ///  主用户创建子用户
        /// </summary>
        /// <param name="customer">主账号对象</param>
        /// <param name="CustomerId">返回customerId</param>
        /// <returns></returns>
        public ResponseBaseDto AddSubCustomer(Customer customer, byte[] imageByte, string extName,
            string customerToken, ref int customerId)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
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
               
                //插入单条用户信息
                customer.IsEnable = 1;
                //添加主用户与子用户的关系
                customer.ParentId = tcp.CustomerId;
                customer.ImagePath = "default.jpg";
                if (customer.LoginTypes == null || customer.LoginTypes == "")
                {
                    customer.LoginTypes = "1,2,3,4";
                }
                if (imageByte != null && imageByte.Length > 0)
                {
                    string oldImageName = customer.ImagePath;
                    string newfileName = bllHelper.SaveImage(imageByte, "customerImage", extName, oldImageName, tcp);
                    customer.ImagePath = newfileName;
                }/*如果为空表示用户没有添加用户头像*/

                customerId = customerServer.InsertCustomer(customer);
                if (customerId > 0)
                {
                    string[] permissionNames =Enum.GetNames(typeof(PermissionCustomerEnum));
                    //添加默认权限（我的空间,消息事件,云视频）
                    for (int i = 0; i < permissionNames.Length; i++)
                    {
                        Permission permission = new Permission();
                        permission.CustomerId = customerId;
                        //用户页面权限时，NodeType和NodeId都赋值为0
                        permission.NodeType = 0;
                        permission.NodeId = 0;
                        permission.PermissionName = permissionNames[i];
                        permission.IsEnable =1;
                        permissionServer.InsertPermission(permission);
                    }
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = customer.CustomerName + " 用户添加成功！";
                }
                else
                {
                    dto.Code = (int)CodeEnum.NoUser;
                    dto.Message = customer.CustomerName + "子用户创建失败！请联系技术人员";
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = customer.CustomerName + "创建主账号失败！请刷新后继续";
                myLog.WarnFormat("AddSubCustomer方法异常,用户id(创建者):{0},用户名(被创建者):{1}",ex,tcp.CustomerId,customer.CustomerName);
            }
            return dto;
        }
        #endregion

        #region 检索当前主用户对子用户信息 SearchSubCustomer
        /// <summary>
        /// 检索当前主用户对子用户信息 SearchSubCustomer
        /// </summary>
        /// <param name="customerToken">token</param>
        /// <param name="customerFlag">返回：主账号结果集</param>
        /// <returns></returns>
        public ResponseBaseDto SearchSubCustomer(string keyWord, string customerToken, ref IList<Customer> customerFlag)
        {
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
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
                //是否是主账号
                if (tcp.SignInType != (int)CustomerSignInTypeEnum.PrimaryCustomer)
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "当前非主用户账号！";
                    return dto;
                }

                //查询所有子用户
                Customer customer = new Customer();
                customer.CustomerId = tcp.CustomerId;
                customerFlag = customerServer.SearchCustomerByParentId(customer, keyWord);
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "子用户用户信息已获取完成！";
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,查询主账号信息失败！";
                myLog.WarnFormat("SearchSubCustomer方法异常,用户id:{0},关键字:{1}",ex,tcp.CustomerId,keyWord);
            }
            return dto;
        }
        #endregion

        #region 主用户查询单一的子用户 GetSingleSubCustomer
        /// <summary>
        /// 前台管理员查询单一主用户 GetSingleSubCustomer
        /// </summary>
        /// <param name="subCustomer">主用户Id</param>
        /// <param name="customerToken">token</param>
        /// <param name="customer">用户信息</param>
        /// <returns></returns>
        public ResponseBaseDto GetSingleSubCustomer(Customer subCustomer, string customerToken, ref Customer customer)
        {
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
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
                else if (tcp.SignInType != (int)CustomerSignInTypeEnum.PrimaryCustomer)
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "您使用的非前台管理员,无查询权限！";
                    return dto;
                }
                else// if(dto.Code ==0)
                {
                    IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(subCustomer);
                    if (customerFlag == null && customerFlag.Count == 0)
                    {
                        dto.Code = (int)CodeEnum.NoUser;
                        dto.Message = "未找到相应的子用户信息";
                    }
                    else if (tcp.CustomerId == customerFlag[0].ParentId)
                    {
                        customer = customerFlag[0];
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "用户已获取完成";
                    }
                    else
                    {
                        dto.Code = (int)CodeEnum.NoAuthorization;
                        dto.Message = "当前主用户无法使用该权限";
                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,查询用户信息失败！";
                myLog.WarnFormat("GetSingleSubCustomer方法异常,用户id(操作者):{0},用户Id(被操作者):{1}",ex,tcp.CustomerId,subCustomer.CustomerId);
            }
            return dto;
        }
        #endregion

        #region 主用户修改子用户的安全信息 UpdateSubSafeByPrimary
        /// <summary>
        /// 主用户修改子用户的安全信息
        /// </summary>
        /// <param name="subCustomer">子用户</param>
        /// <param name="loginTypes">可登陆类型集合</param>
        /// <param name="IsModify">是否可以修改有效期</param>
        /// <param name="validTime">有效期时间</param>
        /// <param name="customerToken">Token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateSubSafeByPrimary(Customer subCustomer, string loginTypes, int IsModify,
            string validTime, string customerToken)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            oLog.Action = "主用户更新子用户";
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
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else if (tcp.SignInType != (int)CustomerSignInTypeEnum.PrimaryCustomer)
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "没有权限，当前用户非主用户";
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else// if(dto.Code ==0)
                {
                    //判定必要的入参
                    if (subCustomer == null || subCustomer.CustomerId == 0)
                    {
                        dto.Code = (int)CodeEnum.NoAuthorization;
                        dto.Message = "缺少需要操作的子用户";
                    }
                    else
                    {
                        //找出前台管理员用户与主用户的关系
                        IList<Customer> subCustomerFlag = customerServer.SelectCustomerByCustomerId(subCustomer);
                        if (subCustomerFlag != null || subCustomerFlag.Count == 1)
                        {
                            if (tcp.CustomerId == subCustomerFlag[0].ParentId)
                            {
                                //更新
                                subCustomerFlag[0].LoginTypes = loginTypes;
                                //确定需要执行有效期操作
                                if (IsModify == 1 && validTime != null && validTime.Length > 0)
                                {
                                    //用户因为有效期过期导致冻结,获取有效期后变为可用
                                    if (subCustomerFlag[0].ValidTime < DateTime.Now)
                                    {
                                        subCustomerFlag[0].IsEnable = 1;
                                    }
                                    if (validTime == "" || validTime.ToLower() == "null")
                                    {
                                        validTime = DateTime.MinValue.ToString();
                                    }
                                    DateTime dt = Convert.ToDateTime(validTime);
                                    subCustomerFlag[0].ValidTime = dt;
                                }
                                customerServer.UpdateCustomer(subCustomerFlag[0]);
                                dto.Code = (int)CodeEnum.Success;
                                dto.Message = "对用户 " + subCustomerFlag[0].CustomerName + " 安全设置已完成";
                            }
                            else
                            {
                                dto.Code = (int)CodeEnum.NoAuthorization;
                                dto.Message = "没有权限，当前用户非主用户";
                            }
                        }
                        else
                        {
                            dto.Code = (int)CodeEnum.NoAuthorization;
                            dto.Message = "缺少需要操作的子用户";
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,更新用户安全信息失败！";
                myLog.WarnFormat("UpdateSubSafeByPrimary方法异常,用户id(操作者):{0},用户id(被操作者):{1},登陆类型:{2},冻结时间:{3}",
                    ex,tcp.CustomerId,subCustomer.CustomerId,subCustomer.LoginTypes,validTime);
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region  主用户更新子用户基本信息 UpdateSubCustomer
        /// <summary>
        ///  主用户更新子用户基本信息 
        /// </summary>
        /// <param name="subCustomer">更改用户实体对象</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateSubCustomer
            (Customer subCustomer, byte[] imageByte, string extName, string customerToken, ref string imagePath)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            oLog.Action = "更新子用户基本信息";
            try
            {

                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token失效";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                IList<Customer> primaryCustomerFlag = customerServer.SelectCustomerByCustomerId(subCustomer);
                if (dto.Code != 0)
                {
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else if (primaryCustomerFlag == null && primaryCustomerFlag.Count == 0)
                {
                    dto.Code = (int)CodeEnum.NoUser;
                    dto.Message = "未找到需要更新的用户";
                    return dto;
                }
                else if (primaryCustomerFlag[0].ReceiverCellPhone != subCustomer.ReceiverCellPhone)
                {   //判定手机是否可用
                    int isUse = 0;
                    dto = GetReceiverPhone(subCustomer, ref isUse);
                    if (isUse == 0)
                    {
                        return dto;
                    }
                }
                else if (primaryCustomerFlag[0].ReceiverEmail != subCustomer.ReceiverEmail)
                {
                    //判定邮箱是否用可用
                    int isUse = 0;
                    dto = GetReceiverEmail(subCustomer, ref isUse);
                    if (isUse == 0)
                    {
                        return dto;
                    }
                }
                if (tcp.CustomerId != primaryCustomerFlag[0].ParentId)
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "当前用户无权限更新";
                    return dto;
                }
                if (imageByte != null && imageByte.Length > 0)
                {
                    string oldImageName = primaryCustomerFlag[0].ImagePath;
                    string newfileName = bllHelper.SaveImage(imageByte, "customerImage", extName, oldImageName, tcp);
                    primaryCustomerFlag[0].ImagePath = newfileName;
                    imagePath = newfileName;
                }/*如果为空表示用户没有操作更新用户头像*/
                if (subCustomer.ReceiverEmail != null)
                    primaryCustomerFlag[0].ReceiverEmail = subCustomer.ReceiverEmail;
                if (subCustomer.ReceiverCellPhone != null)
                    primaryCustomerFlag[0].ReceiverCellPhone = subCustomer.ReceiverCellPhone;
                if (subCustomer.AccountTelephone != null)
                    primaryCustomerFlag[0].AccountTelephone = subCustomer.AccountTelephone;
                if (subCustomer.AccountCompanyAddress != null)
                    primaryCustomerFlag[0].AccountCompanyAddress = subCustomer.AccountCompanyAddress;
                if (subCustomer.ReceiverName != null)
                    primaryCustomerFlag[0].ReceiverName = subCustomer.ReceiverName;
                //更新用户信息
                customerServer.UpdateCustomer(primaryCustomerFlag[0]);
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "更新用户 " + tcp.CustomerName + "个人信息完成";
            }
            catch (Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,更新用户信息失败！";
                myLog.WarnFormat("UpdateSubCustomer方法异常,用户Id(操作者):{0},用户Id(被操作者):{1}", ex, tcp.CustomerId, subCustomer.CustomerId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 主用户重置子用户密码 UpdateSubPassWordByPrimary
        /// <summary>
        /// 主用户重置子用户密码
        /// </summary>
        /// <param name="subCustomer">子用户Id</param>
        /// <param name="oldPassWord">旧密码</param>
        /// <param name="newPassWord">新密码</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateSubPassWordByPrimary(Customer subCustomer, string oldPassWord, string newPassWord, string customerToken)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            oLog.Action = "重置当前用户密码";
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
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else if (tcp.SignInType != (int)CustomerSignInTypeEnum.PrimaryCustomer)
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "没有权限，当前用户非主用户";
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else// if(dto.Code ==0)
                {
                    //判定必要的入参
                    if (subCustomer == null || subCustomer.CustomerId == 0)
                    {
                        dto.Code = (int)CodeEnum.NoAuthorization;
                        dto.Message = "缺少需要操作的子用户";
                    }
                    else
                    {
                        //主用户与子用户的关系
                        IList<Customer> subCustomerFlag = customerServer.SelectCustomerByCustomerId(subCustomer);
                        if (subCustomerFlag != null || subCustomerFlag.Count == 1)
                        {
                            if (tcp.CustomerId == subCustomerFlag[0].ParentId)
                            {
                                if (subCustomerFlag[0].Password.ToLower() == oldPassWord.ToLower())
                                {
                                    //更新用户信息
                                    subCustomerFlag[0].Password = newPassWord.ToLower();
                                    customerServer.UpdateCustomer(subCustomerFlag[0]);
                                    dto.Code = (int)CodeEnum.Success;
                                    dto.Message = "对用户 ：" + subCustomerFlag[0].CustomerName + "密码重置完成！";
                                }
                                else
                                {
                                    dto.Code = (int)CodeEnum.NoUser;
                                    dto.Message = "用户 ：" + subCustomerFlag[0].CustomerName + "原密码输入错误！";
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,更新用户安全信息失败！";
                myLog.WarnFormat("UpdateSubPassWordByPrimary方法异常,时间：", DateTime.Now.ToString());
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 主用户查询子用户授权信息 GetAuthorizeSubCustomer
        /// <summary>
        /// 主用户查询子用户授权信息
        /// </summary>
        /// <param name="subCustomer">被选中的子用户</param>
        /// <param name="customerToken"></param>
        /// <param name="authorizePrimaryResponseFlag">授权结果集</param>
        /// <returns></returns>
        public ResponseBaseDto GetAuthorizeSubCustomer(Customer subCustomer, string customerToken,
            ref  AuthorizeSubResponse authorizeSubResponse)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
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
                else// if(dto.Code ==0)
                {
                    IList<Customer> subCustomerFlag = customerServer.SelectCustomerByCustomerId(subCustomer);
                    if (subCustomerFlag == null || subCustomerFlag.Count == 0)
                    {
                        dto.Code = (int)CodeEnum.NoUser;
                        dto.Message = "未找到相应的子用户信息";
                    }
                    else if (tcp.CustomerId == subCustomerFlag[0].ParentId)
                    {
                        Permission permission = new Permission();
                        permission.NodeType = (int)PermissionNodeTypeEnum.Channel;

                        //查询出当前主用户有多少设备
                        IList<Device> deviceFlag =
                            deviceServer.SelectDeviceCustomerId(new Customer() { CustomerId = tcp.CustomerId });
                        IList<Permission> subCustomerPermissionFlag =
                            permissionServer.SelectPermissionByCustomerId(subCustomer);
                        //查找出子用户的信息
                        authorizeSubResponse.authorizeDeviceResponseList = new List<AuthorizeDeviceResponse>();
                        authorizeSubResponse.subCustomerPermissionList = new List<AuthorizePermissionResponse>();
                        List<int> authorizeOtherDeviceIdFlag = new List<int>();
                        //查找子用户的所有权限
                        for (int i = 0; i < subCustomerPermissionFlag.Count; i++)
                        {
                            if (subCustomerPermissionFlag[i].NodeType == 0 && subCustomerPermissionFlag[i].NodeId == 0)
                            {
                                //加入用户权限
                                string permissionName= subCustomerPermissionFlag[i].PermissionName;
                                foreach (int  cpKey in  Enum.GetValues(typeof(PermissionCustomerEnum)))
                                {
                                    //cp:CustomerPermission
                                    string cpName = Enum.GetName(typeof(PermissionCustomerEnum), cpKey);
                                    if (permissionName == cpName) 
                                    {
                                        AuthorizePermissionResponse authorizePermissionResponse = new AuthorizePermissionResponse();
                                        authorizePermissionResponse.IsEnable = 1;
                                        authorizePermissionResponse.PermissionName = permissionName;
                                        //加入到输出数据内
                                        authorizeSubResponse.subCustomerPermissionList.Add(authorizePermissionResponse);
                                    }
                                }
                            }
                        }
                        //查找当前主用户的所有权限
                        IList<Permission> primaryCustomerPermissionFlag = 
                            permissionServer.SelectPermissionByCustomerId(new Customer() { CustomerId = tcp.CustomerId });
                        //授权的设备
                        for (int i = 0; i < primaryCustomerPermissionFlag.Count; i++)
                        {
                              if(primaryCustomerPermissionFlag[i].NodeType==(int)PermissionNodeTypeEnum.Device)
                              {
                                  IList<Device> authorizeOtherDeviceFlag=
                                      deviceServer.SelectDeviceByDeviceId(new Device(){DeviceId=primaryCustomerPermissionFlag[i].NodeId});
                                  if (authorizeOtherDeviceFlag != null && authorizeOtherDeviceFlag.Count == 1)
                                  {
                                      authorizeOtherDeviceIdFlag.Add(authorizeOtherDeviceFlag[0].DeviceId);
                                      deviceFlag.Add(authorizeOtherDeviceFlag[0]);
                                  }
                              }
                        }
                        //加入到输出数据内
                        for (int i = 0; i < deviceFlag.Count; i++)
                        {
                            AuthorizeDeviceResponse authorizeDeviceResponse = new AuthorizeDeviceResponse();
                            Device device = deviceFlag[i];
                            authorizeDeviceResponse.DeviceId = device.DeviceId;
                            authorizeDeviceResponse.DeviceName = device.DeviceName;
                            //是否是授权设备
                            if (authorizeOtherDeviceIdFlag.Count != 0 
                                && authorizeOtherDeviceIdFlag.Contains(device.DeviceId))
                            {
                                authorizeDeviceResponse.IsEnable = 1;
                            }
                            //设备下的所有通道
                            IList<Channel> channelFlag = channelServer.SelectChannelByDeviceId(device);
                            authorizeDeviceResponse.authorizeChannelResponse = new List<AuthorizeChannelResponse>();
                            for (int j   = 0; j < channelFlag.Count; j++)
                            {
                                Channel channel = channelFlag[j];
                                AuthorizeChannelResponse authorizeChannelResponse = new AuthorizeChannelResponse();
                                authorizeChannelResponse.ChannelId = channel.ChannelId;
                                authorizeChannelResponse.ChannelName = channel.ChannelName;
                                authorizeChannelResponse.IsEnable = channel.IsEnable;
                                permission.NodeId = channel.ChannelId;
                                //查询这个通道下权限
                                IList<Permission> permissionFlag =
                                    permissionServer.SelectPermissionByCidAndNid(subCustomer, permission);
                                authorizeChannelResponse.authorizePermissionResponse = new List<AuthorizePermissionResponse>();
                                for (int m = 0; m < permissionFlag.Count; m++)
                                {
                                    Permission pr = permissionFlag[m];
                                    AuthorizePermissionResponse authorizePermissionResponse = new AuthorizePermissionResponse();
                                    authorizePermissionResponse.PermissionName = pr.PermissionName;
                                    authorizePermissionResponse.IsEnable = pr.IsEnable;
                                    authorizeChannelResponse.authorizePermissionResponse.Add(authorizePermissionResponse);
                                }/*end (for m....*/
                                authorizeDeviceResponse.authorizeChannelResponse.Add(authorizeChannelResponse);
                            }/*end for(int j...*/
                            authorizeSubResponse.authorizeDeviceResponseList.Add(authorizeDeviceResponse);
                        }/*end for(int i....*/
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "获取授权信息完成！";
                    }/*end else if (tcp.CustomerId == customerFlag[0].ParentId)*/
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,用户授权信息获取失败！";
                myLog.WarnFormat("GetAuthorizeSubCustomer方法异常,用户Id(操作者):{0},用户id(被操作者):{1}", ex,tcp.CustomerId,subCustomer.CustomerId);
            }
            return dto;
        }
        #endregion
 
        #region  主用户将权限设定给子用户 AuthorizeSubByPrimary
        /// <summary>
        ///  主用户将权限设定给子用户 AuthorizeSubByPrimary
        /// </summary>
        /// <returns></returns>
        public ResponseBaseDto AuthorizeSubByPrimary
            (AuthorizeSubResponse authorizeSubResponse, string customerToken)
        {
            Customer subCustomer = new Customer();
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            oLog.Action = "主用户对子用户授权";
            ResponseBaseDto dto = new ResponseBaseDto();
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
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else
                {
                     IList<Customer> subCustomerFlag=null;
                    //不等于空时清理权限
                    if (authorizeSubResponse != null) 
                    {
                        subCustomerFlag = 
                            customerServer.SelectCustomerByCustomerId(new Customer() { CustomerId = authorizeSubResponse.CustomerId });
                        if (subCustomerFlag==null || subCustomerFlag.Count==0) 
                        {
                            dto.Code = (int)CodeEnum.NoUser;
                            dto.Message = "未找到需要修改的子用户";
                            oLog.TargetType = (int)OperaterLogEnum.Customer;
                            oLog.Remarks = dto.Message;
                            oLog.Result = dto.Code;
                            bllHelper.AddOperaterLog(oLog, tcp);
                            return dto;
                        }
                        Permission permission = new Permission();
                        permission.CustomerId = authorizeSubResponse.CustomerId;
                        permissionServer.DeletePermissionByCustomerId(permission);
                    }
                    //子用户可访问的页面
                    for (int i = 0; i < authorizeSubResponse.subCustomerPermissionList.Count; i++)
                    {
                      AuthorizePermissionResponse  subCustomerPermission= authorizeSubResponse.subCustomerPermissionList[i];
                      Permission permission = new Permission();
                      permission.CustomerId = authorizeSubResponse.CustomerId;
                      //用户页面权限时，NodeType和NodeId都赋值为0
                      permission.NodeType = 0;
                      permission.NodeId = 0;
                      permission.PermissionName = subCustomerPermission.PermissionName;
                      permission.IsEnable = subCustomerPermission.IsEnable;
                      permissionServer.InsertPermission(permission);
                    }
                    //子用户对通道的访问权限
                    List<AuthorizeDeviceResponse>  authorizeDeviceFlag= authorizeSubResponse.authorizeDeviceResponseList;
                    for (int i = 0; i < authorizeDeviceFlag.Count; i++)
                    {
                        List<AuthorizeChannelResponse> authorizeChannelFlag = authorizeDeviceFlag[i].authorizeChannelResponse;
                        for (int j = 0; j < authorizeChannelFlag.Count; j++)
                        {
                            List<AuthorizePermissionResponse> authorizePermissionFlag = authorizeChannelFlag[j].authorizePermissionResponse;
                            for (int m = 0; m < authorizePermissionFlag.Count; m++)
                            {
                                AuthorizePermissionResponse apr = authorizePermissionFlag[m];
                                Permission permission = new Permission();
                                permission.CustomerId = authorizeSubResponse.CustomerId;
                                permission.NodeType = (int)PermissionNodeTypeEnum.Channel;
                                permission.NodeId = authorizeChannelFlag[j].ChannelId;
                                permission.PermissionName = authorizePermissionFlag[m].PermissionName;
                                permission.IsEnable = authorizePermissionFlag[m].IsEnable;
                                permissionServer.InsertPermission(permission);
                            }
                        }
                    }

                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "对用户" + subCustomerFlag[0].CustomerName+ "授权已完成！";
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常,操作失败！";
                myLog.WarnFormat("AuthorizeSubByPrimary方法异常,用户Id(操作者):{0},用户Id(被操作者):{1}",ex,tcp.CustomerId,subCustomer.CustomerId);
            }
            oLog.Result = dto.Code;
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion
        /*主用户对子用户 end*/

        /*前台管理员对主用户 start*/
        #region 检索当前台管理员下的所有主账号信息 SearchPrimaryCustomer
        /// <summary>
        /// 查找当前台管理员下的所有主账号信息 SearchPrimaryCustomer
        /// </summary>
        /// <param name="customerToken">token</param>
        /// <param name="customerFlag">返回：主账号结果集</param>
        /// <returns></returns>
        public ResponseBaseDto SearchPrimaryCustomer(string keyWord,string customerToken, ref IList<Customer> customerFlag) 
        {
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
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
                //是否是前台管理员
                if (tcp.SignInType != (int)CustomerSignInTypeEnum.ManagerAccount) 
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "当前用户非前台管理账号！";
                    return dto;
                }
                //后台管理员干预代码 wait...


                //查询所有主账号信息
                Customer customer = new Customer();
                customer.CustomerId = tcp.CustomerId;
                customerFlag = customerServer.SearchCustomerByParentId(customer, keyWord);
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "主账号用户信息已获取完成！";
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,查询主账号信息失败！";
                myLog.WarnFormat("SearchPrimaryCustomer方法异常,用户Id(操作者):{0},关键字:{1}", ex, tcp.CustomerId, keyWord);
            }
            return dto;
        }
        #endregion

        #region  前台管理添加主账号 AddPrimaryCustomerByManagerAccount
        /// <summary>
        ///  前台管理添加主账号
        /// </summary>
        /// <param name="customer">主账号对象</param>
        /// <param name="customerId">返回customerId</param>
        /// <returns></returns>
        public ResponseBaseDto AddPrimaryCustomerByManagerAccount(Customer customer, byte[] imageByte, string extName, 
            string customerToken, ref int customerId)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
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
                //插入单条用户信息
                customer.IsEnable = 1;
                //添加前台管理与主账号的子级关系
                customer.ParentId = tcp.CustomerId;
                customer.ImagePath = "default.jpg";
                if (customer.LoginTypes == null || customer.LoginTypes == "")
                {
                    customer.LoginTypes = "1,2,3,4";
                }
                if (imageByte != null && imageByte.Length > 0)
                {
                    string oldImageName = customer.ImagePath;
                    string newfileName = bllHelper.SaveImage(imageByte, "customerImage", extName, oldImageName, tcp);
                    customer.ImagePath = newfileName;
                }/*如果为空表示用户没有添加用户头像*/

                customerId = customerServer.InsertCustomer(customer);
                if (customerId > 0)
                {
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = customer.CustomerName + " 用户添加成功！";
                }
                else
                {
                    dto.Code = (int)CodeEnum.NoUser;
                    dto.Message = customer.CustomerName + "主用户创建失败！请联系技术人员";
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = customer.CustomerName + "创建主账号失败！请刷新后继续";
                myLog.WarnFormat("AddPrimaryCustomerByManagerAccount方法异常,用户Id(操作者):{0},用户名(被操作者):{1}", ex, tcp.CustomerId, customer.CustomerName);
            }
            return dto;
        }
        #endregion

        #region 前台管理员修改主用户的安全信息 UpdatePrimarySafeByManagerAccount
        /// <summary>
        /// 前台管理员修改主用户的安全信息
        /// </summary>
        /// <param name="primaryCustomer">主用户Id</param>
        /// <param name="loginTypes">可登陆类型集合</param>
        /// <param name="IsModify">是否可以修改有效期</param>
        /// <param name="validTime">有效期时间</param>
        /// <param name="customerToken">Token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdatePrimarySafeByManagerAccount(Customer primaryCustomer, string loginTypes, int IsModify,
            string validTime, string customerToken)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            oLog.Action = "前台管理员更新主用户";
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
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else if (tcp.SignInType != (int)CustomerSignInTypeEnum.ManagerAccount)
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "没有权限，当前用户非前台管理员";
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else// if(dto.Code ==0)
                {
                    //判定必要的入参
                    if (primaryCustomer == null || primaryCustomer.CustomerId == 0)
                    {
                        dto.Code = (int)CodeEnum.NoAuthorization;
                        dto.Message = "缺少需要操作的主用户";
                    }
                    else
                    {
                        //找出前台管理员用户与主用户的关系
                        IList<Customer> primaryCustomerFlag = customerServer.SelectCustomerByCustomerId(primaryCustomer);
                        if (primaryCustomerFlag != null || primaryCustomerFlag.Count == 1)
                        {
                            if (tcp.CustomerId == primaryCustomerFlag[0].ParentId)
                            {
                                //更新
                                primaryCustomerFlag[0].LoginTypes = loginTypes;
                                //确定需要执行有效期操作
                                if (IsModify == 1 && validTime != null && validTime.Length > 0)
                                {
                                    //用户因为有效期过期导致冻结,获取有效期后变为可用
                                    if (primaryCustomerFlag[0].ValidTime < DateTime.Now)
                                    {
                                        primaryCustomerFlag[0].IsEnable = 1;
                                    }
                                    if (validTime == "" || validTime.ToLower() == "null")
                                    {
                                        validTime = DateTime.MinValue.ToString();
                                    }
                                    DateTime dt = Convert.ToDateTime(validTime);
                                    primaryCustomerFlag[0].ValidTime = dt;
                                }
                                customerServer.UpdateCustomer(primaryCustomerFlag[0]);
                                dto.Code = (int)CodeEnum.Success;
                                dto.Message = "对用户 " + primaryCustomerFlag[0].CustomerName + " 安全设置已完成";
                            }
                            else
                            {
                                dto.Code = (int)CodeEnum.NoAuthorization;
                                dto.Message = "没有权限，当前用户非前台管理员";
                            }
                        }
                        else
                        {
                            dto.Code = (int)CodeEnum.NoAuthorization;
                            dto.Message = "缺少需要操作的主用户";
                        }

                    }

                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,更新用户安全信息失败！";
                myLog.WarnFormat("UpdatePrimarySafeByManagerAccount方法异常,用户Id(操作者):{0},用户id(被操作者):{1}", ex, tcp.CustomerId,primaryCustomer.CustomerId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 前台管理重置主用户密码 UpdatePrimaryPassWordByManagerAccount
        /// <summary>
        /// 前台管理重置主用户密码
        /// </summary>
        /// <param name="primaryCustomer">主用户Id</param>
        /// <param name="oldPassWord">旧密码</param>
        /// <param name="newPassWord">新密码</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdatePrimaryPassWordByManagerAccount(Customer primaryCustomer, string oldPassWord, string newPassWord, string customerToken)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            oLog.Action = "重置当前用户密码";
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
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else if (tcp.SignInType != (int)CustomerSignInTypeEnum.ManagerAccount)
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "没有权限，当前用户非前台管理员";
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else// if(dto.Code ==0)
                {
                    //判定必要的入参
                    if (primaryCustomer == null || primaryCustomer.CustomerId == 0)
                    {
                        dto.Code = (int)CodeEnum.NoAuthorization;
                        dto.Message = "缺少需要操作的主用户";
                    }
                    else
                    {
                        //找出前台管理员用户与主用户的关系
                        IList<Customer> primaryCustomerFlag = customerServer.SelectCustomerByCustomerId(primaryCustomer);
                        if (primaryCustomerFlag != null || primaryCustomerFlag.Count == 1)
                        {
                            if (tcp.CustomerId == primaryCustomerFlag[0].ParentId)
                            {
                                if (primaryCustomerFlag[0].Password.ToLower() == oldPassWord.ToLower())
                                {
                                    //更新用户信息
                                    primaryCustomerFlag[0].Password = newPassWord.ToLower();
                                    customerServer.UpdateCustomer(primaryCustomerFlag[0]);
                                    dto.Code = (int)CodeEnum.Success;
                                    dto.Message = "对用户 ：" + primaryCustomerFlag[0].CustomerName + "密码重置完成！";
                                }
                                else
                                {
                                    dto.Code = (int)CodeEnum.NoUser;
                                    dto.Message = "用户 ：" + primaryCustomerFlag[0].CustomerName + "原密码输入错误！";
                                }
                            }
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,更新用户安全信息失败！";
                myLog.WarnFormat("UpdatePrimaryPassWordByManagerAccount方法异常,用户Id(操作者):{0},用户id(被操作者):{1}", ex, tcp.CustomerId, primaryCustomer.CustomerId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 前台管理员查询单一主用户 GetSinglePrimaryCustomer
        /// <summary>
        /// 前台管理员查询单一主用户 GetSinglePrimaryCustomer
        /// </summary>
        /// <param name="primaryCustomer">主用户Id</param>
        /// <param name="customerToken">token</param>
        /// <param name="customer">用户信息</param>
        /// <returns></returns>
        public ResponseBaseDto GetSinglePrimaryCustomer(Customer primaryCustomer, string customerToken, ref Customer customer)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
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
                else if (tcp.SignInType != (int)CustomerSignInTypeEnum.ManagerAccount) 
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "您使用的非前台管理员,无查询权限！";
                    return dto;
                }
                else// if(dto.Code ==0)
                {
                    IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(primaryCustomer);
                    if (customerFlag == null && customerFlag.Count == 0)
                    {
                        dto.Code = (int)CodeEnum.NoUser;
                        dto.Message = "未找到相应的主用户信息";
                    }
                    else if (tcp.CustomerId == customerFlag[0].ParentId)
                    {
                        customer = customerFlag[0];
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "用户已获取完成";
                    }
                    else
                    {
                        dto.Code = (int)CodeEnum.NoAuthorization;
                        dto.Message = "当前管理员无法使用该权限";
                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,查询用户信息失败！";
                myLog.WarnFormat("GetSinglePrimaryCustomer方法异常,用户Id(操作者):{0},用户id(被操作者):{1}", ex, tcp.CustomerId, primaryCustomer.CustomerId);
            }
            return dto;
        }
        #endregion

        #region 前台管理员查询授权信息 GetAuthorizeOtherPrimary
        /// <summary>
        /// 前台管理员查询授权信息
        /// </summary>
        /// <param name="primaryCustomer">被选中的主用户（不包含在内）</param>
        /// <param name="customerToken"></param>
        /// <param name="authorizePrimaryResponseFlag">授权结果集</param>
        /// <returns></returns>
        public ResponseBaseDto GetAuthorizeOtherPrimary(Customer primaryCustomer, string customerToken,
            ref List<AuthorizePrimaryResponse> authorizePrimaryResponseFlag)
        {
            /*primaryCustomer 从RESTServer传来的前台管理员选中的主用户
             *primaryCustomerFlag 数据库中收集的信息,之后会排除选中主用户
             *deviceFlag 每一个主用户下的设备
             */
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
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
                else// if(dto.Code ==0)
                {
                    IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(primaryCustomer);
                    if (customerFlag == null || customerFlag.Count == 0)
                    {
                        dto.Code = (int)CodeEnum.NoUser;
                        dto.Message = "未找到相应的主用户信息";
                    }
                    else if (tcp.CustomerId == customerFlag[0].ParentId)
                    {
                        Permission permission = new Permission();
                        permission.NodeType = (int)PermissionNodeTypeEnum.Device;
                        IList<Customer> primaryCustomerFlag =
                            customerServer.SelectCustomerByParentId(new Customer() { CustomerId = tcp.CustomerId });
                        for (int i = 0; i < primaryCustomerFlag.Count; i++)
                        {
                            //遍历用户获取设备信息排除当前用户
                            if (primaryCustomer.CustomerId != primaryCustomerFlag[i].CustomerId)
                            {
                                AuthorizePrimaryResponse apr = new AuthorizePrimaryResponse();
                                apr.authorizeDeviceResponseList = new List<AuthorizeDeviceResponse>();
                                //搜集前台管理员下的所有主用户信息
                                apr.CustomerId = primaryCustomerFlag[i].CustomerId;
                                apr.CustomerName = primaryCustomerFlag[i].CustomerName;
                                IList<Device> deviceFlag = deviceServer.SelectDeviceCustomerId(primaryCustomerFlag[i]);
                                for (int j = 0; j < deviceFlag.Count; j++)
                                {
                                    AuthorizeDeviceResponse adr = new AuthorizeDeviceResponse();
                                    adr.DeviceId = deviceFlag[j].DeviceId;
                                    adr.DeviceName = deviceFlag[j].DeviceName;
                                    //查询权限
                                    permission.NodeId = deviceFlag[j].DeviceId;
                                    permission.PermissionName = PermissionNameTypeEnum.Authorize.ToString();
                                    IList<Permission> permissionFlag =
                                               permissionServer.SelectPermissionBySome(primaryCustomer, permission);
                                    if (permissionFlag != null && permissionFlag.Count == 1)
                                    {
                                        adr.IsEnable = permissionFlag[0].IsEnable;
                                    }
                                    else
                                    {
                                        adr.IsEnable = 0;
                                    }
                                    apr.authorizeDeviceResponseList.Add(adr);
                                }
                                if (deviceFlag != null)
                                    apr.DeviceCount = deviceFlag.Count;
                                authorizePrimaryResponseFlag.Add(apr);
                            }
                        }/*end  for (int i = 0; i < primaryCustomerFlag.Count; i++)*/
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "获取授权信息完成！";
                    }/*end else if (tcp.CustomerId == customerFlag[0].ParentId)*/
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,查询用户信息失败！";
                myLog.WarnFormat("GetAuthorizeOtherPrimary方法异常,用户Id(操作者):{0},用户id(被操作者):{1}", ex, tcp.CustomerId, primaryCustomer.CustomerId);
            }
            return dto;
        }
        #endregion

        #region  前台管理员冻结与解除冻结主用户 EnablePrimaryCustomer
        /// <summary>
        ///  前台管理员冻结与解除冻结主用户 EnablePrimaryCustomer
        /// </summary>
        /// <returns></returns>
        public ResponseBaseDto EnablePrimaryCustomer(Customer primaryCustomer, int isEnable, string customerToken)
        {
            Customer customer = new Customer();
            customer.CustomerId = primaryCustomer.CustomerId;
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            oLog.Action = "解冻或冻结主账户";
            ResponseBaseDto dto = new ResponseBaseDto();
            try
            {
                oLog.TargetId = primaryCustomer.CustomerId;
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token失效";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else
                {
                    if (ForBiddenCustomer(customer, isEnable))
                    {
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = isEnable == 1 ? "解除冻结已完成！" : "已将用户冻结！";
                    }
                    else
                    {
                        dto.Code = (int)CodeEnum.NoUser;
                        dto.Message = isEnable == 1 ? "解除冻结失败！" : "用户冻结失败！";
                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常,操作失败！";
                myLog.WarnFormat("EnablePrimaryCustomer方法异常,用户Id(操作者):{0},用户id(被操作者):{1}", ex, tcp.CustomerId, primaryCustomer.CustomerId);
            }
            oLog.Result = dto.Code;
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region  前台管理员对主用户的授权修改 AuthorizePrimaryByManagerAccount
        /// <summary>
        ///  前台管理员对主用户的授权修改 AuthorizePrimaryByManagerAccount
        /// </summary>
        /// <returns></returns>
        public ResponseBaseDto AuthorizePrimaryByManagerAccount
            (List<AuthorizePrimaryResponse> authorizeCustomerResponseFlag, string customerToken)
        {
            Customer primaryCustomer = new Customer();
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            oLog.Action = "前台管理员授权";
            ResponseBaseDto dto = new ResponseBaseDto();
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
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else
                {
                    //确认2个账号的关系
                    if (authorizeCustomerResponseFlag != null && authorizeCustomerResponseFlag.Count == 1)
                    {
                        primaryCustomer.CustomerId = authorizeCustomerResponseFlag[0].CustomerId;
                    }
                    else
                    {
                        dto.Code = (int)CodeEnum.NoUser;
                        dto.Message = "未找到需要授权的用户信息";
                        oLog.TargetType = (int)OperaterLogEnum.Customer;
                        oLog.Remarks = dto.Message;
                        oLog.Result = dto.Code;
                        bllHelper.AddOperaterLog(oLog, tcp);
                        return dto;
                    }
                    oLog.TargetId = primaryCustomer.CustomerId;
                    IList<Customer> primaryCustomerFlag = customerServer.SelectCustomerByCustomerId(primaryCustomer);
                    if (primaryCustomerFlag == null || primaryCustomerFlag.Count == 0)
                    {
                        dto.Code = (int)CodeEnum.NoUser;
                        dto.Message = "未找到相应的主用用户信息";
                    }
                    else //if(primaryCustomerFlag!=null && primaryCustomerFlag.Count!=0) 
                    {
                        if (tcp.CustomerId == primaryCustomerFlag[0].ParentId)
                        {
                            Permission permission = new Permission();
                            permission.CustomerId = primaryCustomer.CustomerId;
                            permission.NodeType = (int)PermissionNodeTypeEnum.Device;
                            //删除这个用户下的授权信息
                            permissionServer.DeletePermissionByCidAndNTid(permission);
                            for (int i = 0; i < authorizeCustomerResponseFlag.Count; i++)
                            {
                                AuthorizePrimaryResponse authorizePrimaryResponse = authorizeCustomerResponseFlag[i];
                                List<AuthorizeDeviceResponse> authorizeDeviceResponseFlag =
                                    authorizePrimaryResponse.authorizeDeviceResponseList;
                                //收集权限数据,并写入
                                for (int j = 0; j < authorizeDeviceResponseFlag.Count; j++)
                                {
                                    AuthorizeDeviceResponse authorizeDeviceResponse = authorizeDeviceResponseFlag[j];
                                    permission.NodeId = authorizeDeviceResponse.DeviceId;
                                    permission.IsEnable = authorizeDeviceResponse.IsEnable;
                                    permission.PermissionName = "Authorize";
                                    permissionServer.InsertPermission(permission);
                                }
                            }
                            dto.Code = (int)CodeEnum.Success;
                            dto.Message = "已完成对 " + primaryCustomerFlag[0].CustomerName + " 的设备授权";
                        }
                        else
                        {
                            dto.Code = (int)CodeEnum.NoAuthorization;
                            dto.Message = "没有权限操作";
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常,操作失败！";
                myLog.WarnFormat("AuthorizePrimaryByManagerAccount方法异常,用户Id(操作者):{0},用户id(被操作者):{1}", ex, tcp.CustomerId, primaryCustomer.CustomerId);
            }
            oLog.Result = dto.Code;
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region  前台管理员更新主用户的基本信息 UpdatePrimaryCustomer
        /// <summary>
        ///  前台管理员更新主用户的基本信息 
        /// </summary>
        /// <param name="primaryCustomer">更改 用户实体对象</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdatePrimaryCustomer
            (Customer primaryCustomer, byte[] imageByte, string extName, string customerToken, ref string imagePath)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            oLog.Action = "更新主用户信息";
            try
            {

                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token失效";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                IList<Customer> primaryCustomerFlag = customerServer.SelectCustomerByCustomerId(primaryCustomer);
                if (dto.Code != 0)
                {
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else if (primaryCustomerFlag == null && primaryCustomerFlag.Count == 0)
                {
                    dto.Code = (int)CodeEnum.NoUser;
                    dto.Message = "未找到需要更新的用户";
                    return dto;
                }
                else if (primaryCustomerFlag[0].ReceiverCellPhone != primaryCustomer.ReceiverCellPhone)
                {
                    //判定手机是否用可用
                    int isUse = 0;
                    dto = GetReceiverPhone(primaryCustomer, ref isUse);
                    if (isUse == 0)
                    {
                        return dto;
                    }
                }
                else if (primaryCustomerFlag[0].ReceiverEmail != primaryCustomer.ReceiverEmail)
                {
                    //判定邮箱是否用可用
                    int isUse = 0;
                    dto = GetReceiverEmail(primaryCustomer, ref isUse);
                    if (isUse == 0)
                    {
                        return dto;
                    }
                }

                if (tcp.CustomerId != primaryCustomerFlag[0].ParentId)
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "当前用户无权限更新";
                    return dto;
                }
                if (imageByte != null && imageByte.Length > 0)
                {
                    string oldImageName = primaryCustomerFlag[0].ImagePath;
                    string newfileName = bllHelper.SaveImage(imageByte, "customerImage", extName, oldImageName, tcp);
                    primaryCustomerFlag[0].ImagePath = newfileName;
                    imagePath = newfileName;
                }/*如果为空表示用户没有操作更新用户头像*/

                if (primaryCustomer.ReceiverEmail != null)
                    primaryCustomerFlag[0].ReceiverEmail = primaryCustomer.ReceiverEmail;
                if (primaryCustomer.ReceiverCellPhone != null)
                    primaryCustomerFlag[0].ReceiverCellPhone = primaryCustomer.ReceiverCellPhone;
                if (primaryCustomer.AccountTelephone != null)
                    primaryCustomerFlag[0].AccountTelephone = primaryCustomer.AccountTelephone;
                if (primaryCustomer.AccountCompanyAddress != null)
                    primaryCustomerFlag[0].AccountCompanyAddress = primaryCustomer.AccountCompanyAddress;
                if (primaryCustomer.ReceiverName != null)
                    primaryCustomerFlag[0].ReceiverName = primaryCustomer.ReceiverName;
                //更新用户信息
                customerServer.UpdateCustomer(primaryCustomerFlag[0]);
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "更新用户 " + tcp.CustomerName + "个人信息完成";
            }
            catch (Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,更新用户信息失败！";
                myLog.WarnFormat("UpdatePrimaryCustomer方法异常,用户Id(操作者):{0},用户id(被操作者):{1}", ex, tcp.CustomerId, primaryCustomer.CustomerId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion
        /*前台管理员对主用户 end*/

        #region 判断字符串是否为数字
        private bool CheckCellPhone(string str)
        {
            char[] ch = new char[str.Length];
            ch = str.ToCharArray();
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i] < 48 || ch[i] > 57)
                    return false;
            }
            return true;
        }
        #endregion

        #region 判断从服务器Token是否可以有权限被使用 CheckServerCustomerToken
        /// <summary>
        ///  判断从服务器Token是否可以有权限被使用
        /// </summary>
        /// <param name="customerToken"></param>
        /// <returns></returns>
        public ResponseBaseDto CheckServerCustomerToken(string permissionName, int nodeId, int nodeType, string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            try
            {
                tcp = utc.FindByCustomerToken(customerToken);
                if (tcp == null)
                {
                    //没有找到当前Key为token的数据
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "没有找到相匹配的用户令牌";
                }
                else /* if(tcp !=null)*/
                {
                    //v1.0
                    Customer customer = new Customer();
                    customer.CustomerId = tcp.CustomerId;
                    Permission permission = new Permission();
                    permission.NodeType = nodeType;
                    permission.NodeId = nodeId;
                    permission.PermissionName = permissionName;

                    if (nodeType == (int)PermissionNodeTypeEnum.Channel && nodeId != 0)
                    {
                        
                        if (tcp.SignInType == (int)CustomerSignInTypeEnum.SubCustomer)
                        {
                            //如果当前是子用户
                            IList<Permission> permissionFlag = permissionServer.SelectPermissionBySome(customer, permission);
                            //子用户
                            if (permissionFlag == null || permissionFlag.Count <= 0)
                            {
                                dto.Code = (int)CodeEnum.NoHardWare;
                                dto.Message = "未查询出相关通道信息";
                            }
                            else
                            {
                                dto.Code = (int)CodeEnum.Success;
                                dto.Message = "此通道可以被使用";
                            }
                            return dto;
                        }
                        else if (tcp.SignInType == (int)CustomerSignInTypeEnum.PrimaryCustomer)
                        {
                            //如果当前是主用户将多表连接查询
                            Device device = new Device();
                            device.CustomerId = customer.CustomerId;
                            Channel channel = new Channel();
                            channel.ChannelId = nodeId;
                            //查出这个用户的设备下的通道是否可用
                            IList result = channelServer.SelectChannelAndDeviceBySome(device, channel);
                            if (result != null && result.Count != 0)
                            {
                                object[] resultObj = (object[])result[0];
                                if ((int)resultObj[3] == 0)
                                {
                                    //Channel.IsEnable==0
                                    dto.Code = (int)CodeEnum.NoUser;
                                    dto.Message = "当前通道已禁用";
                                }
                                else
                                {
                                    dto.Code = (int)CodeEnum.Success;
                                    dto.Message = "此通道可以被使用";
                                }
                                return dto;
                            }
                            else
                            {
                                //如果是主用户并且主用户没有设备，查找授权的设备中的这个通道
                                IList<Permission> customerPermissionFlag = permissionServer.SelectPermissionByCustomerId(customer);
                                if (customerPermissionFlag == null || customerPermissionFlag.Count <= 0)
                                {
                                    dto.Code = (int)CodeEnum.NoHardWare;
                                    dto.Message = "未查询出相关通道信息";
                                    return dto;
                                }
                                for (int i = 0; i < customerPermissionFlag.Count; i++)
                                {
                                    if (customerPermissionFlag[i].NodeType == (int)PermissionNodeTypeEnum.Channel)
                                    {
                                        //如果权限里有这个通道
                                        IList<Channel> channelFlag =
                                            channelServer.SelectChannelByChannelId(new Channel() { ChannelId = nodeId });
                                        if (channelFlag != null && channelFlag.Count == 1)
                                        {
                                            if (channelFlag[0].IsEnable == 1
                                                && customerPermissionFlag[i].IsEnable == 1
                                                && channelFlag[0].ChannelId == nodeId
                                                && customerPermissionFlag[i].PermissionName == permissionName)
                                            {
                                                dto.Code = (int)CodeEnum.Success;
                                                dto.Message = "此通道可以被使用";
                                            }
                                            else
                                            {
                                                dto.Code = (int)CodeEnum.NoHardWare;
                                                dto.Message = "当前设备已被暂时禁用";
                                            }
                                        }
                                    }
                                    else if (customerPermissionFlag[i].NodeType == (int)PermissionNodeTypeEnum.Device)
                                    {
                                        //如果是授权设备,遍历设备下的通道
                                        IList<Channel> channelFlag =
                                            channelServer.SelectChannelByDeviceId(new Device() { DeviceId = customerPermissionFlag[i].NodeId });
                                        for (int j = 0; j < channelFlag.Count; j++)
                                        {
                                            if (channelFlag[j].IsEnable == 1
                                                && customerPermissionFlag[i].IsEnable == 1
                                                && channelFlag[j].ChannelId == nodeId
                                                && customerPermissionFlag[i].PermissionName == permission.PermissionName)
                                            {
                                                dto.Code = (int)CodeEnum.Success;
                                                dto.Message = "此通道可以被使用";
                                                return dto;
                                            }
                                        }
                                    }
                                }/*end for(int i = 0; i < permissionFlag.Count; i++) */
                                //遍历后如果未找到数据
                                if (dto.Message == null || dto.Message.Length == 0)
                                {
                                    dto.Code = (int)CodeEnum.NoHardWare;
                                    dto.Message = "无此设备信息";
                                    return dto;
                                }
                            }
                        }
                    }/* end  if (nodeType == (int)PermissionNodeTypeEnum.Channel && nodeId != 0)  */
                    else if (nodeType == (int)PermissionNodeTypeEnum.Device && nodeId != 0)
                    {
                        //设备
                        IList<Device> deviceFlag = deviceServer.SelectDeviceCustomerId(customer);
                        if (deviceFlag != null && deviceFlag.Count == 1)
                        {
                            dto.Code = (int)CodeEnum.Success;
                            dto.Message = "此设备可以被使用";
                            return dto;
                        }
                        else
                        {
                            //查找权限内的设备
                            IList<Permission> customerPermissionFlag = permissionServer.SelectPermissionByCustomerId(customer);
                            if (customerPermissionFlag == null || customerPermissionFlag.Count == 0)
                            {
                                dto.Code = (int)CodeEnum.NoHardWare;
                                dto.Message = "无此设备信息";
                                return dto;
                            }
                            else
                            {
                                for (int i = 0; i < customerPermissionFlag.Count; i++)
                                {
                                    //遍历当前用户权限集合
                                    if (customerPermissionFlag[i].NodeId == nodeId
                                        && customerPermissionFlag[i].IsEnable == 1
                                        && customerPermissionFlag[i].PermissionName == permission.PermissionName)
                                    {
                                        dto.Code = (int)CodeEnum.Success;
                                        dto.Message = "此设备可以被使用";
                                        return dto;
                                    }
                                }
                                //遍历后如果未找到数据
                                if (dto.Message == null || dto.Message.Length == 0)
                                {
                                    dto.Code = (int)CodeEnum.NoHardWare;
                                    dto.Message = "无此设备信息";
                                    return dto;
                                }
                            }
                        }
                    } /*end else if(nodeType == (int)PermissionNodeTypeEnum.Device && nodeId != 0)*/
                    else
                    {
                        dto.Code = (int)CodeEnum.NoHardWare;
                        dto.Message = "无此设备信息";
                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，请刷新页面后继续";
                myLog.WarnFormat("CheckServerCustomerToken方法异常！NodeType:{0},NodeId:{1}",ex,nodeType,nodeId);
            }
            return dto;
        }
        #endregion

        #region 清理过期的用户及相关信息（私有方法）ClearCustomer
        /// <summary>
        /// 清理过期的用户及相关信息
        /// </summary>
        /// <param name="customer">customer</param>
        private bool ClearCustomer(Customer customer) 
        {
            try
            {
                IList<Customer> primaryCustomerFlag = customerServer.SelectCustomerByCustomerId(customer);
                if (primaryCustomerFlag != null && primaryCustomerFlag.Count == 1)
                {
                    IList<Customer> subCustomerFlag = customerServer.SelectCustomerByParentId(customer);              
                    if (subCustomerFlag != null)
                    {
                        subCustomerFlag.Add(primaryCustomerFlag[0]);
                    }
                    for (int i = 0; i < subCustomerFlag.Count; i++)
                    {
                        //删除用户权限
                        permissionServer.DeletePermissionByCustomerId
                                               (new Permission() { CustomerId = subCustomerFlag[i].CustomerId });
                        //删除设备，只有是主用户的时候才会触发
                        if (subCustomerFlag[i].SignInType == (int)CustomerSignInTypeEnum.PrimaryCustomer)
                        {
                            IList<Device> deviceFlag = deviceServer.SelectDeviceCustomerId(subCustomerFlag[i]);
                            for (int n = 0; n < deviceFlag.Count; n++)
                            {
                                deviceBLL.ClearDevice(deviceFlag[i]);
                            }
                        }
                        //删除用户日志
                        operaterLogServer.DeleteOperaterLogByCustomerId(subCustomerFlag[i]);
                        //删除用户
                        customerServer.DeleteCustomer(subCustomerFlag[i]);
                        //删除用户收藏
                        userFavoriteServer.DeleteUserFavoriteByCustomerId(subCustomerFlag[i].CustomerId);
                    }
                }/*end  if(primaryCustomerFlag != null && primaryCustomerFlag.Count == 1)*/
                return true;
            }
            catch 
            {
                return false;
            }
        }
        #endregion

        #region  冻结或解冻用户及其子用户（私有方法） ForBiddenCustomer
        /// <summary>
        /// 冻结或解冻用户及其子用户
        /// </summary>
        /// <param name="customer"></param>
        private bool ForBiddenCustomer(Customer customer, int isEnable)
        {
            try
            {
                IList<Customer> primaryCustomerFlag = customerServer.SelectCustomerByCustomerId(customer);
                if (primaryCustomerFlag != null && primaryCustomerFlag.Count == 1)
                {
                    IList<Customer> subCustomerFlag = customerServer.SelectCustomerByParentId(customer);
                    if (subCustomerFlag != null)
                    {
                        subCustomerFlag.Add(primaryCustomerFlag[0]);
                    }
                    for (int i = 0; i < subCustomerFlag.Count; i++)
                    {
                        subCustomerFlag[i].IsEnable = isEnable;

                        if (isEnable == 0)
                        {
                            subCustomerFlag[i].ForbiddenTime = DateTime.Now;
                        }
                        else 
                        {
                            subCustomerFlag[i].ForbiddenTime = DateTime.MinValue; 
                        }
                        customerServer.UpdateCustomer(subCustomerFlag[i]);
                    }
                    return true;
                }
                else
                {
                    return false;
                } /*end  if(primaryCustomerFlag != null && primaryCustomerFlag.Count == 1)*/
            }
            catch 
            {
                return false;
            }

        }
        #endregion

        #region 更新用户安全信息 UpdateSelfSafeCustomer
        /// <summary> 
        /// 更新用户安全信息
        /// </summary>
        /// <param name="loginTypes">可登陆的权限</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateSelfSafeCustomer(string loginTypes,string customerToken) 
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            oLog.Action = "更新安全信息（本人）";
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
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else// if(dto.Code ==0)
                {
                    Customer customer = new Customer();
                    customer.CustomerId = tcp.CustomerId;
                    IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(customer);
                    customerFlag[0].LoginTypes = loginTypes;
                    //更新用户信息
                    customerServer.UpdateCustomer(customerFlag[0]);
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "更新用户 " + tcp.CustomerName + "安全信息完成";
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,更新用户安全信息失败！";
                myLog.WarnFormat("UpdateSelfSafeCustomer方法异常,用户Id:{0}", ex, tcp.CustomerId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 重置当前用户密码 UpdateSelfCustomerPassWord
        /// <summary> 
        /// 重置当前用户密码
        /// </summary>
        /// <param name="loginTypes">可登陆的权限</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateSelfCustomerPassWord(string oldPassWord,string newPassWord, string customerToken)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            oLog.Action = "重置当前用户密码";
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
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else// if(dto.Code ==0)
                {
                    Customer customer = new Customer();
                    customer.CustomerId = tcp.CustomerId;
                    IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(customer);
                    if (customerFlag[0].Password.ToLower() == oldPassWord.ToLower())
                    {
                        //更新用户信息
                        customerFlag[0].Password = newPassWord.ToLower();
                        customerServer.UpdateCustomer(customerFlag[0]);
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = tcp.CustomerName + "密码重置完成！";
                    }
                    else 
                    {
                        dto.Code = (int)CodeEnum.NoUser;
                        dto.Message =tcp.CustomerName + "密码重置失败！";
                        dto.Message = "用户 ：" + tcp.CustomerName + "原密码输入错误！";
                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,更新用户安全信息失败！";
                myLog.WarnFormat("UpdateSelfCustomerPassWord方法异常,用户Id:{0},待修改的密码:{1}", ex, tcp.CustomerId, newPassWord);
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region  删除用户 CustomerId集合
        /// <summary>
        /// 删除用户 CustomerId集合
        /// </summary>
        /// <param name="customer">Customer.CustomerId</param>
        /// <returns></returns>
        public ResponseBaseDto DeleteCustomerById(int[] customerIdList, string customerToken)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            try
            {
                //校验token
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token失效";
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    return dto;
                }
                else
                {
                    oLog.Action = "删除子用户";
                    if (tcp.SignInType == (int)CustomerSignInTypeEnum.ManagerAccount)
                    {
                        oLog.Action = "删除主用户";
                    }
                    Customer customer = new Customer();
                    string delCustomerNameList = "";
                    for (int i = 0; i < customerIdList.Length; i++)
                    {
                        //找出需要删除用户的CustomerId信息
                        customer.CustomerId = customerIdList[i];
                        IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(customer);
                        oLog.TargetId = customerIdList[i];
                        if (customerFlag != null && customerFlag.Count == 1)
                        {
                            ClearCustomer(customerFlag[0]);
                            delCustomerNameList += "[" + customerFlag[0].CustomerName + "]";
                        }
                    }
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "已将用户 " + delCustomerNameList + "删除完成！";
                }
            }
            catch
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "删除失败！请刷新后继续";
                myLog.WarnFormat("DeleteCustomerById方法异常,时间：", DateTime.Now.ToString());
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;

        }
        #endregion

        #region 查找用户 GetSelfCustomer 以CustomerId
        /// <summary>
        ///  GetSelfCustomer 以CustomerId查找用户
        /// </summary>
        /// <param name="customerToken">token</param>
        /// <param name="customer">customer.CustomerId</param>
        /// <returns></returns>
        public ResponseBaseDto GetSelfCustomer(string customerToken, ref Customer customer)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
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
                else// if(dto.Code ==0)
                {
                    customer.CustomerId = tcp.CustomerId;
                    IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(customer);
                    if (customerFlag != null && customerFlag.Count == 1)
                    {
                        customer = customerFlag[0];
                        dto.Code = (int)CodeEnum.Success;
                        dto.Message = "用户已获取完成";
                    }
                    else
                    {
                        dto.Code = (int)CodeEnum.NoUser;
                        dto.Message = "没有查找到用户信息";
                    }
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,查询用户信息失败！";
                myLog.WarnFormat("GetSelfCustomer方法异常,用户Id:{0}", ex, tcp.CustomerId);
            }
            return dto;
        }
        #endregion

        #region  更新当前用户信息 UpdateSelfCustomer
        /// <summary>
        ///  更新当前用户信息
        /// </summary>
        /// <param name="customer">更改 用户实体对象</param>
        /// <param name="customerToken">token</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateSelfCustomer
            (Customer customer, byte[] imageByte, string extName, string customerToken, ref string imagePath)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            oLog.Action = "更新用户本人信息";
            try
            {

                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token失效";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                customer.CustomerId = tcp.CustomerId;
                IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerId(customer);
                if (dto.Code != 0)
                {
                    oLog.TargetType = (int)OperaterLogEnum.Customer;
                    oLog.Remarks = dto.Message;
                    oLog.Result = dto.Code;
                    bllHelper.AddOperaterLog(oLog, tcp);
                    return dto;
                }
                else if (customerFlag == null && customerFlag.Count == 0)
                {
                    dto.Code = (int)CodeEnum.NoUser;
                    dto.Message = "未找到需要更新的用户";
                    return dto;
                }
                else if (customerFlag[0].ReceiverCellPhone != customer.ReceiverCellPhone)
                {
                    //判定手机是否用可用
                    int isUse = 0;
                    dto = GetReceiverPhone(customer, ref isUse);
                    if (isUse == 0)
                    {
                        return dto;
                    }
                }
                else if (customerFlag[0].ReceiverEmail != customer.ReceiverEmail)
                {
                    //判定邮箱是否用可用
                    int isUse = 0;
                    dto = GetReceiverEmail(customer, ref isUse);
                    if (isUse == 0)
                    {
                        return dto;
                    }
                }
                if (imageByte != null && imageByte.Length > 0)
                {
                    string oldImageName = customerFlag[0].ImagePath;
                    string newfileName = bllHelper.SaveImage(imageByte, "customerImage", extName, oldImageName, tcp);
                    customerFlag[0].ImagePath = newfileName;
                    imagePath = newfileName;
                }/*如果为空表示用户没有操作更新用户头像*/
                else 
                {
                    imagePath = customerFlag[0].ImagePath;
                }
                if(customer.ReceiverEmail!=null)
                      customerFlag[0].ReceiverEmail = customer.ReceiverEmail;
                if(customer.ReceiverCellPhone!=null)
                      customerFlag[0].ReceiverCellPhone = customer.ReceiverCellPhone;
                if(customer.AccountTelephone!=null)
                      customerFlag[0].AccountTelephone = customer.AccountTelephone;
                if(customer.AccountCompanyAddress!=null)
                      customerFlag[0].AccountCompanyAddress = customer.AccountCompanyAddress;
                if(customer.ReceiverName!=null)
                      customerFlag[0].ReceiverName = customer.ReceiverName;
                //更新用户信息
                customerServer.UpdateCustomer(customerFlag[0]);
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "更新用户 " + tcp.CustomerName + "个人信息完成";
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,更新用户信息失败！";
                myLog.WarnFormat("UpdateSelfCustomer方法异常,用户Id:{0}", ex, tcp.CustomerId);
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks = dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region  检查用户返回用户id
        /// <summary>
        ///检查用户返回用户id
        /// </summary>
        /// <returns></returns>
        private int GetCustomerIdByCustomerId(Customer customer)
        {
            int customerId = -1;
            try
            {
                IList<Customer> customerFlag = customerServer.SelectCustomerByCustomerName(customer);
                if (customerFlag != null && customerFlag.Count == 1)
                {
                    customerId = customerFlag[0].CustomerId;
                }
            }
            catch(Exception ex)
            {
                myLog.WarnFormat("GetCustomerIdByCustomerId方法异常,用户Id:{0}", ex, customer.CustomerId);
            }
            return customerId;
        }
        #endregion

        #region 发送一个临时的检验码至手机 SendMsgWithSum ylc
        /// <summary>
        /// 发送一个临时的检验码至手机 SendMsgWithSum ylc
        /// </summary>
        /// <param name="phoneNum">手机号</param>
        /// <param name="noticeType">是注册 还是找回</param>
        /// <param name="isValid">是否发送成功</param>
        /// <returns></returns>
        public ResponseBaseDto SendMsgWithSum(string phoneNum, int noticeType, ref int isValid)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            try
            {
                MsgNoticeTypeEnum type=MsgNoticeTypeEnum.FindPassword;
                DateTime expireTime=DateTime.Now;
                //注册有效期为2分,找回有效期为5分钟
                if (noticeType == (int)MsgNoticeTypeEnum.RegisterUser) 
                {
                    expireTime = DateTime.Now.AddMinutes(2);
                    type = MsgNoticeTypeEnum.RegisterUser;
                }
                else if (noticeType == (int)MsgNoticeTypeEnum.FindPassword)
                {
                    expireTime = DateTime.Now.AddMinutes(5);
                    //手机发送时，确认此手机是否属于某个用户的
                    Customer customer = new Customer();
                    customer.ReceiverCellPhone = phoneNum;
                    IList<Customer> customerFlag = customerServer.SelectCustomerByReceiverPhone(customer);
                    if (customerFlag==null || customerFlag.Count !=1)
                    {
                        dto.Code = (int)CodeEnum.InvalidNoCellPhone;
                        dto.Message = "没有找到此电话号码的用户！";
                        return dto;
                    }
                }
                else
                {
                    dto.Code = (int)CodeEnum.NoComplete;
                    dto.Message ="未知的服务类型！";
                    return dto;
                }
                int sendState = cellphoneMsgNotice.SendMsgWithSum(phoneNum, type, expireTime);
                isValid = sendState == 0 ? 1 : 0;
                dto.Code = (int)CodeEnum.Success;
                if (isValid == 1)
                {
                    dto.Message = "已将验证码发送至手机 ！";
                }
                else 
                {
                    dto.Message = "验证码发送过程出现未知错误！";
                }
            }
            catch(Exception ex)
            {
                //如果是短信服务方未及时响应
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "发送失败,服务器未能及时响应！";
                myLog.ErrorFormat("SendMsgWithSum方法异常验证码服务方无法请求", ex);
            }
            return dto;
        }
        #endregion

        #region 验证一个检验码是否有效 CheckingWithSum ylc
        /// <summary>
        /// 验证一个检验码是否有效 CheckingWithSum ylc
        /// </summary>
        /// <param name="phoneNum">电话号码</param>
        /// <param name="verifyKey">验证码</param>
        /// <param name="noticeType">是注册还是找回</param>
        /// <param name="isValid">ref IsValid 1有效 0无效</param>
        /// <returns></returns>
        public ResponseBaseDto CheckingWithSum(string phoneNum, string verifyKey, int noticeType, ref int isValid) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            bool bFlag = false;
            try
            {
                MsgNoticeTypeEnum type = MsgNoticeTypeEnum.FindPassword;
                if (string.IsNullOrEmpty(phoneNum) || string.IsNullOrEmpty(verifyKey))
                {
                    dto.Code = (int)CodeEnum.InvalidMsgSum;
                    dto.Message = "没有填写手机或验证码";
                    return dto;
                }
                else if (noticeType == (int)MsgNoticeTypeEnum.RegisterUser)
                {
                    type = MsgNoticeTypeEnum.RegisterUser;
                }
                bFlag = cellphoneMsgNotice.CheckingWithSum(phoneNum, verifyKey, type);
                if (bFlag == true)
                {
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "验证码可以使用！";
                }
                else
                {
                    dto.Code = (int)CodeEnum.InvalidMsgSum;
                    dto.Message = "无效的验证码！";
                }
            }
            catch(Exception ex)
            {
                //如果是短信服务方未及时响应
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，短信服务方未响应";
                myLog.ErrorFormat("CheckingWithSum方法异常验证码服务方无法请求",ex);
            }
            isValid = bFlag == true ? 1 : 0;
            return dto;
        }
        #endregion

        #region 重置用户的密码(找回密码)UpdateCustomerPassWord
        /// <summary>
        /// 重置用户的密码(使用手机号找回密码)
        /// </summary>
        /// <param name="newPassWord">新密码</param>
        /// <returns></returns>
        public ResponseBaseDto UpdateCustomerPassWord(string newPassWord, string verifyToken)
        {
            OperaterLog oLog = new OperaterLog();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            oLog.Action = "密码找回";
            string customerName=string.Empty;
            int customerId = 0;
            try
            {
                if (verifyToken == null || newPassWord == null || verifyToken == "" || newPassWord == "")
                {
                    dto.Code = (int)CodeEnum.NoComplete;
                    dto.Message = "用户数据提交不完整！";
                    return dto;
                } 
                verifyToken = MDKey.Decrypt(verifyToken);
                if (verifyToken.IndexOf("_") == -1) 
                {
                    dto.Code = (int)CodeEnum.ApplicationErr;
                    dto.Message = "请勿提交非法数据！";
                    return dto;
                }
                int.TryParse(verifyToken.Split('_')[0],out customerId);
                Customer customer = new Customer();
                customer.CustomerId=customerId;
                IList<Customer> customerFlag =customerServer.SelectCustomerByCustomerId(customer) ;
                if (customerFlag == null || customerFlag.Count == 0)
                {
                    dto.Code = (int)CodeEnum.NoUser;
                    dto.Message = "未找到需要修改的用户！";
                    return dto;
                }
                else 
                {
                    //更新用户密码
                    customerFlag[0].Password = newPassWord;
                    customerServer.UpdateCustomer(customerFlag[0]);
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "用户密码已重置完成！";
                    customerName = customer.CustomerName;
                }
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,重置密码出现未知错误！";
                myLog.WarnFormat("UpdateCustomerPassWord方法异常！用户id:{0},带修改的密码:{1}", ex,customerId,newPassWord);
            }
            oLog.TargetType = (int)OperaterLogEnum.Customer;
            oLog.Remarks ="["+customerName +"]"+ dto.Message;
            oLog.Result = dto.Code;
            bllHelper.AddOperaterLog(oLog, tcp);
            return dto;
        }
        #endregion

        #region 发送验证码（找回密码）SendVerifyKey
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="LoinName">用户名，手机或者邮箱</param>
        /// <returns></returns>
        public ResponseBaseDto SendVerifyKey(string loginName, ref int isValid)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            isValid = 0;//默认表示失败
            //loginName=用户名
            IList<Customer> customerFlag = 
                customerServer.SelectCustomerByCustomerName(new Customer() { CustomerName=loginName});
            if (customerFlag != null && customerFlag.Count == 1)
            {
                if (string.IsNullOrEmpty(customerFlag[0].ReceiverCellPhone)!=true && customerFlag[0].ReceiverCellPhone!="") 
                {
                    //如果电话不等于空，将赋值给loginName
                    loginName = customerFlag[0].ReceiverCellPhone;
                }
                else if (string.IsNullOrEmpty(customerFlag[0].ReceiverEmail) != true && customerFlag[0].ReceiverEmail != "")
                {
                    //如果邮箱不等于空，将赋值给loginName
                    loginName = customerFlag[0].ReceiverEmail;
                }
                else 
                {
                    //此用户手机和邮箱都是空！
                    isValid =0 ;
                    dto.Code = (int)CodeEnum.NoData;
                    dto.Message = "手机和邮箱都未填写！,无法进行找回服务，请联系管理人员";
                }
            }
            if (loginName.Length == 11 && CheckCellPhone(loginName) == true)
            {
                //loginName=手机
                dto = SendMsgWithSum(loginName, (int)MsgNoticeTypeEnum.FindPassword,ref isValid);
       
            }
            else if (loginName.IndexOf("@") != -1)
            {
                //loginName=邮箱  
                isValid = 0;
                dto.Code =(int)CodeEnum.Success;
                dto.Message = "未实现";
            }
            return dto;
        }
        #endregion

        #region 校验验证码（找回密码） CheckVerfyKey
        /// <summary>
        ///  校验验证码（找回密码）
        /// </summary>
        /// <param name="loginName">手机或者邮箱</param>
        /// <param name="verifyKey">验证码</param>
        /// <param name="isValid">1有效 0无效</param>
        /// <param name="verifyToken"></param>
        /// <returns></returns>
        public ResponseBaseDto CheckVerfyKey(string loginName, string verifyKey,  ref int isValid, ref string verifyToken) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            IList<Customer> customerFlag = null;
            customerFlag=    customerServer.SelectCustomerByCustomerName(new Customer() { CustomerName = loginName });
            if (customerFlag != null && customerFlag.Count == 1)
            {
                if (string.IsNullOrEmpty(customerFlag[0].ReceiverCellPhone) != true && customerFlag[0].ReceiverCellPhone != "") 
                {
                    //如果电话不等于空，将赋值给loginName
                    loginName = customerFlag[0].ReceiverCellPhone;
                }
                else if (string.IsNullOrEmpty(customerFlag[0].ReceiverEmail) != true && customerFlag[0].ReceiverEmail != "")
                {
                    //如果邮箱不等于空，将赋值给loginName
                    loginName = customerFlag[0].ReceiverEmail;
                }
                else
                {
                    //此用户手机和邮箱都是空！
                    isValid = 0;
                    dto.Code = (int)CodeEnum.NoData;
                    dto.Message = "手机和邮箱都未填写！,无法进行找回服务，请联系管理人员";
                }
            }
            if (loginName.Length == 11 && CheckCellPhone(loginName) == true)
            {
               customerFlag = customerServer.SelectCustomerByReceiverPhone(new Customer() {ReceiverCellPhone=loginName });
                //如果是手机
               if (customerFlag != null && customerFlag.Count == 1)
               {
                   //验证这个验证码是否可用
                   dto = CheckingWithSum(loginName, verifyKey,(int)MsgNoticeTypeEnum.FindPassword, ref isValid);
                   //设置token防止用户恶意操作
                   if (verifyKey == null || verifyKey == "") //添加测试参数
                   {
                       verifyKey = "666666";
                   }
                   verifyToken =MDKey.Encrypt(string.Format("{0}_{1}", customerFlag[0].CustomerId, verifyKey));
                   isValid = 1;
               }
               else 
               {
                   isValid = 0;
                   dto.Code = (int)CodeEnum.NoUser;
                   dto.Message = "该手机未注册";
                   return dto;
               }
            }
            else if (loginName.IndexOf("@") != -1)
            {
                //loginName=邮箱  
                isValid = 1;
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "未实现";
            }
            return dto;
        }
        #endregion

        #region  检查用户输入的是否在 用户名，手机，邮箱 是否是注册过 IsRegisterByLoginName
        /// <summary>
        /// 检查用户输入的是否在 用户名，手机，邮箱 是否是注册过的 IsRegisterLoginName
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="isRegister"></param>
        /// <returns></returns>
        public ResponseBaseDto IsRegisterByLoginName(string loginName, ref int isRegister)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            IList<Customer> customerFlag = null;
            Customer customer = new Customer();
            /*判定用户使用用户名，邮箱或者移动电话登录*/
            if (loginName.Length == 11 && CheckCellPhone(loginName) == true)
            {
                //并且可以转为int类型 手机                 
                customer.ReceiverCellPhone = loginName;
                customerFlag = customerServer.SelectCustomerByReceiverPhone(customer);
            }
            else if (loginName.IndexOf("@") != -1)
            {
                //邮箱  
                customer.ReceiverEmail = loginName;
                customerFlag = customerServer.SelectCustomerByReceiverEmail(customer);
            }
            else
            {
                //用户名
                customer.CustomerName = loginName;
                customerFlag = customerServer.SelectCustomerByCustomerName(customer);
            }
            isRegister = customerFlag != null && customerFlag.Count > 0 ? 1 : 0;
            dto.Code = (int)CodeEnum.Success;
            dto.Message = "已完成检索！";
            return dto;
        }
        #endregion

        #region 获取当前在线用户
        /// <summary>
        ///  获取当前在线用户 待修改
        /// </summary>
        /// <param name="tokenCachePropertyFlag"></param>
        /// <returns></returns>
        public ResponseBaseDto GetCustomerOnlineTotal(ref IList<TokenCacheProperty> tokenCachePropertyFlag)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            try
            {
                tokenCachePropertyFlag = utc.FindAll();
                dto.Code = (int)CodeEnum.Success;
                dto.Message = "在线用户已获取完成！";
            }
            catch 
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "数据异常！，请联系技术人员";
            }
            return dto;
        }
        #endregion

        #region 获取当前用户访问权限
        /// <summary>
        /// 获取当前用户访问权限
        /// </summary>
        /// <param name="customerToken">token</param>
        /// <param name="permissionFlag">ref 用户权限集合</param>
        /// <returns></returns>
        public ResponseBaseDto GetSelfPermission(string customerToken, ref  IList<Permission> permissionFlag)
        {
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            ResponseBaseDto dto = new ResponseBaseDto();
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token已过期,请重新登录！";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
                else if (tcp.SignInType != (int)CustomerSignInTypeEnum.SubCustomer)
                {
                    dto.Code = (int)CodeEnum.NoAuthorization;
                    dto.Message = "用户访问权限只限子用户！";
                    return dto;
                }
                else// if(dto.Code ==0)
                {
                    List<string> permissionNames = new List<string>();
                    foreach (string name in Enum.GetNames(typeof(PermissionCustomerEnum)))
                    {
                        permissionNames.Add(name);
                    }
                    permissionFlag=permissionServer.SelectPermissionByCidAndPname(tcp.CustomerId, permissionNames);
                }
            }
            catch (Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常！,查询用户权限信息失败！";
                myLog.WarnFormat("GetSelfPermission方法异常,用户id(操作者):{0}", ex, tcp.CustomerId);
            }
            return dto;

        }
        #endregion
    }
}
