using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Bsr.Cloud.Core
{
    public class PwdMD5
    {
        /// <summary>
        /// 对str进行MD5签名
        /// </summary>
        /// <param name="str">原始串</param>
        /// <param name="lower">true表示MD5值为小写，false为大写</param>
        /// <returns></returns>
        public static string StringToMD5(string str, bool lower = true)
        {
            byte[] result = Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            if(lower == true)
                return BitConverter.ToString(output).Replace("-", "").ToLower();
            else
                return BitConverter.ToString(output).Replace("-", "").ToUpper();
        }

    }
}
