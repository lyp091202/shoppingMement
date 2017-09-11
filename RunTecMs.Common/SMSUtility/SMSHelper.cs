using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Web;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using RunTecMs.Common.DBUtility;
using RunTecMs.Common.WebUtility;
using RunTecMs.Log4Net;
using System.Collections.Generic;
using RunTecMs.Common.ConvertUtility;

namespace RunTecMs.Common.SMSUtility
{
    public class SMSHelper
    {

        #region 构造函数
        public SMSHelper()
        {
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.AddHeader("pragma", "no-cache");
            HttpContext.Current.Response.CacheControl = "no-cache";
        }
        #endregion

        #region 验证手机号码
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool ValidPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            string pattern = @"^1[3|4|5|7|8][0-9]\d{8}$";
            return Regex.IsMatch(phone, pattern);
        }
        #endregion

        #region 发送手机短信，返回发送结果
        /// <summary>
        /// 发送端消息
        /// </summary>
        /// <param name="mobile">手机号码（多个场合，使用[,]隔开）</param>
        /// <param name="msg">消息</param>
        /// <param name="bussinessValue">业务值</param>
        /// <returns></returns>
        public static bool SendShortMsg(string mobile, string msg, int bussinessValue,string type="0")
        {
            // 确定发送手机
            string sendMobile = "";
            if (mobile.Contains(","))
            {
                string[] mobiles = mobile.Split(',');
                for (int i = 0; i < mobiles.Length; i++)
                {
                    if (!ValidPhone(mobiles[i]))
                    {
                        continue;
                    }
                    else
                    { 
                        if (string.IsNullOrEmpty(sendMobile))
                        {
                            sendMobile = mobiles[i];
                        }
                        else
                        {
                            sendMobile = sendMobile + "," + mobiles[i];
                        }
                        
                    }
                }
            }
            else
            {
                if (!ValidPhone(mobile))
                {
                    return false;
                }
                else
                {
                    sendMobile = mobile;
                }
            }

            // 发送内容业务追加
            switch (bussinessValue)
            {
                case 1:
                    msg = "【发发】 " + msg;
                    break;
                case 2:
                    msg = "【大决策】 " + msg;
                    break;
                case 3:
                    msg = "【沐融教育】 " + msg;
                    break;
                default:
                    msg = "【中润】 " + msg;
                    break;
            }

            string result = SendMsg(sendMobile, msg,type);
            if (string.IsNullOrWhiteSpace(result))
            {
                return false;
            }

            var list = ConvertToList.XmlToList<HxtReturnsms>(result, "returnsms");
            if (list == null || list.Count < 1)
            {
                return false;
            }

            var model = list[0];
            string sendError = "";
            if (string.Equals("success", model.returnstatus, StringComparison.CurrentCultureIgnoreCase))
            {
                if (string.Equals("ok", model.message, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            sendError = model.message;
            LogHelper.Error(sendError);
            return false;
        }

        #endregion

        #region 发送手机短信

        /// <summary>
        /// 这个方法是从WebSerivice拷贝过来的
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="msg"></param>
        /// <param name="type">值为1的时候群发，值为0时候不是群发</param>
        /// <returns></returns>
        private static string SendMsg(string phone, string msg,string type)
        {
            string strMsg = HttpUtility.UrlEncode(msg, Encoding.GetEncoding("UTF-8"));
            string url = "";
            string account = "";
            string password = "";
            string userid = "";
            if ("0".Equals(type))
            {
                 account = PubConstant.GetConfigString("SMSAccount") ?? "rwwhcm";
                 password = PubConstant.GetConfigString("SMSPassword") ?? "rwwhcm";
                 userid = ConfigHelper.GetConfigString("SMSUserId") ?? "16890";
            }
            else if ("1".Equals(type))
            {
                account = PubConstant.GetConfigString("SMSAccountBatch") ?? "djcqf";
                password = PubConstant.GetConfigString("SMSPasswordBatch") ?? "jndjc888";
                userid = ConfigHelper.GetConfigString("SMSUserIdBatch") ?? "16890";
            }
            url = string.Format("http://www.lcqxt.com/sms.aspx?userid={0}&account={1}&password={2}&mobile={3}&content={4}&sendTime=&action=send&checkcontent=0", userid, account, password, phone, strMsg);

            Encoding encoding = Encoding.GetEncoding("UTF-8");
            byte[] data = encoding.GetBytes("");
            // 准备请求...  
            try
            {
                // 设置参数  
                var request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                var outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据 
                var response = request.GetResponse() as HttpWebResponse;

                //直到request.GetResponse()程序才开始向目标网页发送Post请求 
                var instream = response.GetResponseStream();
                var sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码  
                string content = sr.ReadToEnd();
                return content;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
                return string.Empty;
            }
        }
        #endregion

        /// <summary>
        /// 皓信通短信返回实体
        /// </summary>
        public class HxtReturnsms
        {
            public string returnstatus { get; set; }
            public string message { get; set; }
            public string remainpoint { get; set; }
            public string taskID { get; set; }
            public string successCounts { get; set; }
        }
    }
}
