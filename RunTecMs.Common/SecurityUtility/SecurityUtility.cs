using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO; 

namespace RunTecMs.Common
{
    public class SecurityUtility
    {
        //加密/解密钥匙
        const string KEY_64 = "fyz7tz8g";//注意了，是8个字符，64位    
        const string IV_64 = "tvbzn9i7";//注意了，是8个字符，64位

        public  string Encode(string data)
        {
     
            return Encode(data, KEY_64, IV_64);
        }
        /// <summary>
        /// 解密的方法
        /// </summary>
        /// <param name="data">解密的数据</param>
        /// <returns>返回加密前的字符串</returns>
        public  string Decode(string data)
        {
            return  Decode(data, KEY_64, IV_64);
        }
        /// <summary>
        ///加密算法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private  string Encode(string data, string KEY_64, string IV_64)
        {

            KEY_64 = ToMD5(KEY_64);
            IV_64 = ToMD5(IV_64);
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

        }
        /// <summary>
        /// 解密的方法（）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="KEY_64"></param>
        /// <param name="IV_64"></param>
        /// <returns></returns>
        private  string Decode(string data, string KEY_64, string IV_64)
        {
            KEY_64 = ToMD5(KEY_64);
            IV_64 = ToMD5(IV_64);
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 转为MD5
        /// </summary>
        /// <param name="KEY"></param>
        /// <returns></returns>
        public static string ToMD5(string KEY)
        {
            byte[] result = Encoding.Default.GetBytes(KEY);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);

            string KEY_64 = BitConverter.ToString(output).Replace("-", "").Substring(0, 8);
            return KEY_64;

        }
    }
}
