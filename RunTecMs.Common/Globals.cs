using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Data;
using System.Globalization;
using System.Collections;
using System.Net.Mail;

namespace RunTecMs.Common
{
    public sealed class Globals
    {
        public const int PAY_BALANCE_PAYMENTMODEID = -2;
        //public const string PAY_SECURITY_CODE = "XhglMS_SENDING";
        //public const string PAY_SECURITY_KEY = "XhglMS_SECURITY_CODE";
        public static decimal POINT_RATIO;
        public static string SESSIONKEY_ADMIN;
        public static string SESSIONKEY_AGENTS;
        public static string SESSIONKEY_ENTERPRISE;
        public static string SESSIONKEY_USER;

        static Globals()
        {
            SESSIONKEY_ADMIN = "UserInfo";
            SESSIONKEY_ENTERPRISE = "Enterprise_UserInfo";
            SESSIONKEY_AGENTS = "Agents_UserInfo";
            SESSIONKEY_USER = SESSIONKEY_AGENTS = SESSIONKEY_ENTERPRISE = SESSIONKEY_ADMIN = "UserInfo";
            POINT_RATIO = 1M;
        }
        private Globals()
        {
        }
        public static string AppendQuerystring(string url, string querystring)
        {
            return AppendQuerystring(url, querystring, false).Trim();
        }

        public static string AppendQuerystring(string url, string querystring, bool urlEncoded)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            string str = "?";
            if (url.IndexOf('?') > -1)
            {
                if (!urlEncoded)
                {
                    str = "&";
                }
                else
                {
                    str = "&amp;";
                }
            }
            return (url + str + querystring);
        }
        public static string FullPath(string local)
        {
            if (string.IsNullOrEmpty(local))
            {
                return local;
            }
            if (local.ToLower(CultureInfo.InvariantCulture).StartsWith("http://"))
            {
                return local;
            }
            if (HttpContext.Current == null)
            {
                return local;
            }
            return FullPath(HostPath(HttpContext.Current.Request.Url), local);
        }
        public static string FullPath(string hostPath, string local)
        {
            return (hostPath + local);
        }
        public static string GenRandomCodeFor6()
        {
            long ticks = DateTime.Now.Ticks;
            Random random = new Random(((int)(((ulong)ticks) & 0xffffffffL)) | ((int)(ticks >> 0x20)));
            return random.Next(0, 0xf423f).ToString(CultureInfo.InvariantCulture);
        }
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0)
            {
                return dt;
            }
            DataTable table = dt.Copy();
            table.Clear();
            int num = (PageIndex - 1) * PageSize;
            int count = PageIndex * PageSize;
            if (num < dt.Rows.Count)
            {
                if (count > dt.Rows.Count)
                {
                    count = dt.Rows.Count;
                }
                for (int i = num; i <= (count - 1); i++)
                {
                    DataRow row = table.NewRow();
                    DataRow row2 = dt.Rows[i];
                    foreach (DataColumn column in dt.Columns)
                    {
                        row[column.ColumnName] = row2[column.ColumnName];
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
        public static string HostPath(Uri uri)
        {
            if (uri == null)
            {
                return string.Empty;
            }
            string str = (uri.Port == 80) ? string.Empty : (":" + uri.Port.ToString(CultureInfo.InvariantCulture));
            return string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { uri.Scheme, uri.Host, str });
        }
        public static string HtmlDecode(object target)
        {
            if (StringPlus.IsNullOrEmpty(target))
            {
                return "";
            }
            return HttpUtility.HtmlDecode(target.ToString().Trim());
        }
        public static string HtmlDecodeForSpaceWrap(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            return HttpUtility.HtmlDecode(content).Replace("<br />", "\n").Replace("&nbsp;", " ");
        }
        public static string HtmlEncode(object target)
        {
            if (StringPlus.IsNullOrEmpty(target))
            {
                return "";
            }
            return HttpUtility.HtmlEncode(target.ToString().Trim());
        }
        public static string HtmlEncodeForSpaceWrap(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            return HttpUtility.HtmlEncode(content).Replace(" ", "&nbsp;").Replace("\n", "<br />");
        }
        public static int PageCount(int count, int pageye)
        {
            int num = 0;
            int num2 = pageye;
            if ((count % num2) == 0)
            {
                num = count / num2;
            }
            else
            {
                num = (count / num2) + 1;
            }
            if (num == 0)
            {
                num++;
            }
            return num;
        }
        public static void RedirectToSSL(HttpContext context)
        {
            if ((context != null) && !context.Request.IsSecureConnection)
            {
                Uri url = context.Request.Url;
                context.Response.Redirect("https://" + url.ToString().Substring(7));
            }
        }
        public static bool SafeBool(string text, bool defaultValue)
        {
            bool flag;
            if (bool.TryParse(text, out flag))
            {
                defaultValue = flag;
            }
            return defaultValue;
        }
        public static DateTime SafeDateTime(string text, DateTime defaultValue)
        {
            DateTime time;
            if (DateTime.TryParse(text, out time))
            {
                defaultValue = time;
            }
            return defaultValue;
        }
        public static decimal SafeDecimal(string text, decimal defaultValue)
        {
            decimal num;
            if (decimal.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static int SafeInt(string text, int defaultValue)
        {
            int num;
            if (int.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }
        public static long SafeLong(string text, long defaultValue)
        {
            long num;
            if (long.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }
        public static short SafeShort(string text, short defaultValue)
        {
            short num;
            if (short.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }
        public static string SafeString(object target, string defaultValue)
        {
            if ((target != null) && ("" != target.ToString()))
            {
                return target.ToString();
            }
            return defaultValue;
        }
        public static string StripAllTags(string strToStrip)
        {
            strToStrip = Regex.Replace(strToStrip, @"</p(?:\s*)>(?:\s*)<p(?:\s*)>", "\n\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = Regex.Replace(strToStrip, @"<br(?:\s*)/>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = Regex.Replace(strToStrip, "\"", "''", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = StripHtmlXmlTags(strToStrip);
            return strToStrip;
        }

        public static string StripForPreview(string content)
        {
            content = Regex.Replace(content, "<br>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<br/>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<br />", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<p>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = content.Replace("'", "&#39;");
            return StripHtmlXmlTags(content);
        }

        public static string StripHtmlXmlTags(string content)
        {
            return Regex.Replace(content, "<[^>]+>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        public static string StripScriptTags(string content)
        {
            content = Regex.Replace(content, "<script((.|\n)*?)</script>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "'javascript:", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return Regex.Replace(content, "\"javascript:", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }
        public static string ToDelimitedString(ICollection collection, string delimiter)
        {
            if (collection == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            if (collection is Hashtable)
            {
                foreach (object obj2 in ((Hashtable)collection).Keys)
                {
                    builder.Append(obj2.ToString() + delimiter);
                }
            }
            if (collection is ArrayList)
            {
                foreach (object obj3 in (ArrayList)collection)
                {
                    builder.Append(obj3.ToString() + delimiter);
                }
            }
            if (collection is string[])
            {
                foreach (string str in (string[])collection)
                {
                    builder.Append(str + delimiter);
                }
            }
            if (collection is MailAddressCollection)
            {
                foreach (MailAddress address in (MailAddressCollection)collection)
                {
                    builder.Append(address.Address + delimiter);
                }
            }
            return builder.ToString().TrimEnd(new char[] { Convert.ToChar(delimiter, CultureInfo.InvariantCulture) });
        }
        public static string UnHtmlEncode(string formattedPost)
        {
            RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
            formattedPost = Regex.Replace(formattedPost, "&quot;", "\"", options);
            formattedPost = Regex.Replace(formattedPost, "&lt;", "<", options);
            formattedPost = Regex.Replace(formattedPost, "&gt;", ">", options);
            return formattedPost;
        }
        public static string UrlDecode(string urlToDecode)
        {
            if (string.IsNullOrEmpty(urlToDecode))
            {
                return urlToDecode;
            }
            return HttpUtility.UrlDecode(urlToDecode, Encoding.UTF8);
        }
        public static string UrlEncode(string urlToEncode)
        {
            if (string.IsNullOrEmpty(urlToEncode))
            {
                return urlToEncode;
            }
            return HttpUtility.UrlEncode(urlToEncode, Encoding.UTF8);
        }
        public static string ApplicationPath
        {
            get
            {
                string applicationPath = "/";
                if (HttpContext.Current != null)
                {
                    applicationPath = HttpContext.Current.Request.ApplicationPath;
                }
                if (applicationPath == "/")
                {
                    return string.Empty;
                }
                return applicationPath.ToLower(CultureInfo.InvariantCulture);
            }
        }
        public static string DomainFullName
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Empty;
                }
                return HttpContext.Current.Request.Url.Authority;
            }
        }
        public static string DomainName
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Empty;
                }
                return HttpContext.Current.Request.Url.Host;
            }
        }
        public static bool IsPublicSession
        {
            get
            {
                return false;
            }
        }
 

 

 

 


 


 

 

 

 

 

 

 

 

 

 

 

 

 


 


 


 

 

 

 

 

 

 

 

 

 

 

 

 

 

 

 

 


 



 

 

 

 


 

 

 




    }
}
