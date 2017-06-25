using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Core
{
    /// <summary>
    /// 用于文本和Base64编码文本的互相转换 和 Byte[]和Base64编码文本的互相转换
    /// </summary>
    public class Base64
    {

        /// <summary>
        /// 将普通文本转换成Base64编码的文本
        /// </summary>
        /// <param name="value">普通文本(UTF8)</param>
        /// <returns></returns>
        public static string FromStringToBase64(String value)
        {
            byte[] binBuffer = (new UTF8Encoding()).GetBytes(value);
            int base64ArraySize = (int)Math.Ceiling(binBuffer.Length / 3d) * 4;
            char[] charBuffer = new char[base64ArraySize];
            Convert.ToBase64CharArray(binBuffer, 0, binBuffer.Length, charBuffer, 0);
            string s = new string(charBuffer);
            return s;
        }

        /// <summary>
        /// 将Base64编码的文本转换成普通文本(UTF8)
        /// </summary>
        /// <param name="base64">Base64编码的文本</param>
        /// <returns></returns>
        public static string FromBase64ToString(string base64)
        {
            char[] charBuffer = base64.ToCharArray();
            byte[] bytes = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);
            return (new UTF8Encoding()).GetString(bytes);
        }

        /// <summary>
        /// 将Byte[]转换成Base64编码文本
        /// </summary>
        /// <param name="binBuffer">Byte[]</param>
        /// <returns></returns>
        public static string FromByteToBase64(byte[] binBuffer)
        {
            int base64ArraySize = (int)Math.Ceiling(binBuffer.Length / 3d) * 4;
            char[] charBuffer = new char[base64ArraySize];
            Convert.ToBase64CharArray(binBuffer, 0, binBuffer.Length, charBuffer, 0);
            string s = new string(charBuffer);
            return s;
        }

        /// <summary>
        /// 将Base64编码文本转换成Byte[]
        /// </summary>
        /// <param name="base64">Base64编码文本</param>
        /// <returns></returns>
        public static Byte[] FromBase64ToByte(string base64)
        {
            char[] charBuffer = base64.ToCharArray();
            byte[] bytes = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);
            return bytes;
        }
    }
}
