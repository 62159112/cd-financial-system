using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse_Project.ashx
{
    /// <summary>
    /// bom_prcing 的摘要说明
    /// </summary>
    public class bom_prcing : IHttpHandler
    {
        string result = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request.Form["action"];
            switch (action)
            {
                case "zdxfbsrthwr"://加密测试
                    result = xcvnsgh(context);
                    break;
                case "sfghjwrtw"://解密测试
                    result = sfghjwrtw(context);
                    break;
            }
            context.Response.Write(result);
        }

        


        //加密测试
        private string xcvnsgh(HttpContext context)
        {
            string asdc = context.Request.Form["asdc"];
            return Encrypt(asdc);
        }
        //解密测试
        private string sfghjwrtw(HttpContext context)
        {
            string asdc = context.Request.Form["asdc"];
            return Decrypt(asdc);
        }
        

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }



        private static byte[] _rgbKey = ASCIIEncoding.ASCII.GetBytes("cjavapye");
        private static byte[] _rgbIV = ASCIIEncoding.ASCII.GetBytes("wcjavapy");
        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="text">需要加密的值</param>
        /// <returns>加密后的结果</returns>
        public static string Encrypt(string text)
        {
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            using (MemoryStream memStream = new MemoryStream())
            {
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateEncryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
                StreamWriter sWriter = new StreamWriter(crypStream);
                sWriter.Write(text);
                sWriter.Flush();
                crypStream.FlushFinalBlock();
                memStream.Flush();
                return Convert.ToBase64String(memStream.GetBuffer(), 0, (int)memStream.Length);
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns>解密后的结果</returns>
        public static string Decrypt(string encryptText)
        {
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            byte[] buffer = Convert.FromBase64String(encryptText);

            using (MemoryStream memStream = new MemoryStream())
            {
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
                crypStream.Write(buffer, 0, buffer.Length);
                crypStream.FlushFinalBlock();
                return ASCIIEncoding.UTF8.GetString(memStream.ToArray());
            }
        }




    }
}