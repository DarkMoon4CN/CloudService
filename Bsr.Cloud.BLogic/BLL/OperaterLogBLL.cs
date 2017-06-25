using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model;
using Bsr.Cloud.Core;
using Bsr.Cloud.Model.Entities;

namespace Bsr.Cloud.BLogic.BLL
{
    public class OperaterLogBLL
    {
        static private ILogger myLog = new Logger<OperaterLogBLL>();
        BLLHelper bllHelper = new BLLHelper();
        OperaterLogServer operaterLogServer = OperaterLogServer.GetInstance();
        #region 查询当前用户登陆日志
        /// <summary>
        ///  查询当前用户登陆日志
        /// </summary>
        /// <param name="requestCount">请求条数</param>
        /// <param name="startCount">起始条数</param>
        /// <param name="customerToken">token</param>
        /// <param name="Total">总条数</param>
        /// <param name="operaterLogFlag">ref  结果集对象</param>
        /// <returns></returns>
        public ResponseBaseDto GetSelfOperaterLog(int requestCount, int startCount,
            string customerToken, ref int Total, ref IList<OperaterLog> operaterLogFlag) 
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token已失效，请重新登录后继续";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
                else
                {
                    List<int> customerIdList = new List<int>();
                    //当前用户Id
                    customerIdList.Add(tcp.CustomerId);
                    string actionWhere = "登录";
                    startCount -= 1;
                    startCount = startCount < 0 ? 0 : startCount;

                    operaterLogFlag = operaterLogServer.OperaterLogForPage(customerIdList, requestCount, startCount, actionWhere);
                    Total = operaterLogServer.OperaterLogForPageCount(customerIdList, actionWhere);
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "用户登陆信息已获取完成！";
                }/*end if(utc.IsValid(customerToken) == false)*/
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，用户登陆信息获取失败！";
                myLog.WarnFormat("GetSelfOperaterLog方法异常,用户Id:{0},起始条数:{1},请求条数:{2}",ex,tcp.CustomerId,startCount,requestCount);
            }
            return dto;
        }
        #endregion

        #region 前台管理员对指定的主用户查询登陆日志
        /// <summary>
        ///  前台管理员对指定的主用户查询登陆日志
        /// </summary>
        /// <param name="requestCount">请求条数</param>
        /// <param name="startCount">起始条数</param>
        /// <param name="customerToken">token</param>
        /// <param name="Total">总条数</param>
        /// <param name="operaterLogFlag">ref  结果集对象</param>
        /// <returns></returns>
        public ResponseBaseDto GetPrimaryOrSubLoginInfo(Customer psCustomer, int requestCount, int startCount,
            string customerToken, ref int Total, ref IList<OperaterLog> operaterLogFlag)
        {
            ResponseBaseDto dto = new ResponseBaseDto();
            UserTokenCache utc = UserTokenCache.GetInstance();
            TokenCacheProperty tcp = new TokenCacheProperty();
            try
            {
                if (utc.IsValid(customerToken) == false)
                {
                    dto.Code = (int)CodeEnum.ServerNoToken;
                    dto.Message = "用户token已失效";
                    return dto;
                }
                dto = bllHelper.CheckCustomer(dto, customerToken, ref tcp);
                if (dto.Code != 0)
                {
                    return dto;
                }
                else
                {
                    List<int> customerIdList = new List<int>();
                    //当前用户Id
                    customerIdList.Add(psCustomer.CustomerId);
                    string actionWhere = "登录";
                    startCount -= 1;
                    startCount = startCount < 0 ? 0 : startCount;
                    operaterLogFlag = operaterLogServer.OperaterLogForPage(customerIdList, requestCount, startCount, actionWhere);
                    Total = operaterLogServer.OperaterLogForPageCount(customerIdList, actionWhere);
                    dto.Code = (int)CodeEnum.Success;
                    dto.Message = "用户登陆信息已获取完成！";
                }/*end if(utc.IsValid(customerToken) == false)*/
            }
            catch(Exception ex)
            {
                dto.Code = (int)CodeEnum.ApplicationErr;
                dto.Message = "网络异常，用户登陆信息获取失败！";
                myLog.WarnFormat("GetPrimaryOrSubLoginInfo方法异常,用户id(查询者):{0},用户id(被查询者):{1},起始条数:{2},请求条数:{3}",
                    ex,tcp.CustomerId,psCustomer.CustomerId,startCount,requestCount);

            }
            return dto;
        }
        #endregion
    }
}
