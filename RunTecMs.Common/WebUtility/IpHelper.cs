using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RunTecMs.Common.WebUtility
{
    public class IpHelper
    {
        /// <summary>  
        /// 获取IP地址  
        /// </summary>  
        public static string GetIpAddress()
        {
            // HttpRequest Request = HttpContext.Current.Request;  
            HttpRequest request = HttpContext.Current.Request; // ForumContext.Current.Context.Request;  
            // 如果使用代理，获取真实IP  
            var userIp = request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "" ? request.ServerVariables["REMOTE_ADDR"] : request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(userIp))
            {
                userIp = request.UserHostAddress;
            }
            return userIp;
        }
    }
}
