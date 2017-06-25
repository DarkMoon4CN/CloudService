using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model;
using System.Threading;
using System.Net;
using System.IO;

namespace Bsr.Cloud.Core
{
    public enum MsgNoticeTypeEnum
    {
        RegisterUser = 1,   // 用户注册时检验用
        FindPassword,       // 用户找回密码时使用
    }

    /// <summary>
    /// 需存入memcache的信息
    /// </summary>
    [Serializable]
    public class PhoneSumCache
    {
        public string sum { get; set; }
    };

    /// <summary>
    /// 短信检验码的发送和检验，工具类。
    /// </summary>
    public class CellphoneMsgNotice
    {

        #region construct
        private CellphoneMsgNotice()
        {
            sentCache = new Dictionary<string, DateTime>();
        }
        #endregion

        public static CellphoneMsgNotice GetInstance()
        {
            //如实例不存在，则New一个新实例，否则返回已有实例
            if (instance == null)
            {
                //在同一时刻加了锁的那部分程序只有一个线程可以进入，
                lock (_object)
                {
                    //如实例不存在，则New一个新实例，否则返回已有实例
                    if (instance == null)
                    {
                        instance = new CellphoneMsgNotice();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 发送一个临时的检验码至手机
        /// </summary>
        /// <param name="phoneNum">手机号</param>
        /// <param name="type">功能用途</param>
        /// <param name="expireTime">超时时间，到时间会自动清除</param>
        /// <returns>0为成功，其它值表示错误</returns>
        public int SendMsgWithSum(string phoneNum, MsgNoticeTypeEnum type, DateTime expireTime)
        {
            string key = CreateMemcacheKey(phoneNum, type);
            DateTime curDate = DateTime.Now;

            // 检测该号码是否过于频繁被发送（同一号码一分钟内最多1次）
            DateTime dt;
            if (sentCache.TryGetValue(key, out dt) == true)
            {
                if (dt.AddSeconds(60) >= curDate)
                {
                    myLog.WarnFormat("短信发送太频繁，禁止。号码：{0}，功能：{1}", phoneNum, type);
                    return (int)CodeEnum.TooManyMsg;
                }
            }

            // 检测整个系统发送频率是否太高（一分钟内最多5个）
            int cnt = 0;
            foreach (var e in sentCache)
            {
                if (e.Value.AddSeconds(60) >= curDate)
                {
                    cnt++;
                    if (cnt > 5)
                    {
                        myLog.WarnFormat("系统在1分钟内的短信请求太频繁，禁止");
                        return (int)CodeEnum.TooManyMsg;
                    }
                }
            }

            //// 开始发送，并写入到memcache，以及sentCache
            int result = -1;
            string strRet = null;
            StringBuilder url = new StringBuilder();
            string mobile = "&mobile=" + phoneNum;
            string randomDigit = GenerateRandomDigit();
            string message = "【星际云】提醒您：本次操作的验证码是" + randomDigit + "，请勿将验证码泄露于他人，验证码有效期2分钟";
            string content = "&content=" + Base64.FromStringToBase64(message);

            // url文档参考："短信验证 - HTTP接口说明文档.docx"
            url.Append("http://api.shumi365.com:8090/sms/send.do?userid=410110&pwd=06D1CEBD15D7F15ABD6EF30757178453&timespan=20141229172508&msgfmt=UTF8");
            url.Append(mobile);
            url.Append(content);

            try
            {
                HttpWebRequest request = WebRequest.Create(url.ToString()) as HttpWebRequest;
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader ser = new StreamReader(stream, Encoding.Default);
                strRet = ser.ReadToEnd();

                if (strRet != null && strRet != "")
                {
                    if (int.TryParse(strRet, out result))
                    {
                        if (result >= 0)
                        {
                            result = 0;
                            PhoneSumCache psc = new PhoneSumCache();
                            psc.sum = randomDigit;
                            mc.AddObject(key, psc, expireTime);
                            sentCache.Add(key, curDate);
                        }
                        else
                        {
                            myLog.WarnFormat("发送临时的检验码至手机失败,号码：{0}，功能：{1}, 错误码：{2}", phoneNum, type, result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                myLog.WarnFormat("发送临时的检验码至手机失败, 号码：{0}，功能：{1}", ex, phoneNum, type);
            }
            return result;
        }

        /// <summary>
        /// 验证一个检验码是否有效
        /// </summary>
        /// <param name="phoneNum">手机号</param>
        /// <param name="sum">检验码</param>
        /// <param name="type">功能用途</param> 
        /// <returns>true表示有效,false表示无效</returns>
        public bool CheckingWithSum(string phoneNum, string sum, MsgNoticeTypeEnum type)
        {
            string key = CreateMemcacheKey(phoneNum, type);
            try
            {
                PhoneSumCache sc = mc.GetObject(key) as PhoneSumCache;
                if (sc == null)
                {
                    myLog.WarnFormat("找不到这个号码的检验码, 号码：{0}，功能：{1}", phoneNum, type);
                    return false;
                }

                if (sum != sc.sum)
                {
                    // 如果探测检验码太频，则强制做个延时
                    if (this.lastCheckingInvalid >= DateTime.Now)
                    {
                        Thread.Sleep(500);
                    }
                    this.lastCheckingInvalid = DateTime.Now;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                myLog.WarnFormat("检测号码的检验码时有异常, 号码：{0}，功能：{1}", e, phoneNum, type);
            }

            return false;
        }



        private string CreateMemcacheKey(string phoneNum, MsgNoticeTypeEnum type)
        {
            return string.Format("{0}_{1}", phoneNum, type);
        }

        /// <summary>
        /// 创建一个6字节长度的随机数字
        /// </summary>
        /// <returns></returns>
        private string GenerateRandomDigit()
        {
            Random ra = new Random(DateTime.Now.Millisecond);
            int rnum = ra.Next(100000, 999999);
            return rnum.ToString();
        }


        #region 成员变量

        MemCache mc = new MemCache("CellphoneMsgNotice");
        static private  readonly object _object = new object();

        static private  CellphoneMsgNotice instance;

        static private ILogger myLog = new Logger<CellphoneMsgNotice>();

        // 记录每个电话号码发送的最近时间
        IDictionary<string, DateTime> sentCache = null;

        // 最后一次检测校验码出错时，系统时间
        DateTime lastCheckingInvalid = DateTime.Now;

        #endregion
    }
}
