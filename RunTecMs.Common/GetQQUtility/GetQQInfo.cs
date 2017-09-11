using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Text;
using System.Web;
using log4net;

namespace RunTecMs.Common.GetQQUtility
{
    public class GetQQInfo
    {
        public static int QunNum = 0;
        public static int QQNum = 0;
        private static ILog log;

        static GetQQInfo()
        {
            //log = log4net.LogManager.GetLogger(typeof(Utilis));
        }

        /// <summary>
        /// 获取群
        /// </summary>
        /// <param name="webBrs"></param>
        /// <param name="lstView"></param>
        public static void GetQun(WebBrowser webBrs, ListView lstView)
        {
            var byteArray = System.Text.Encoding.Default.GetBytes(webBrs.DocumentText);
            GetQunNum(byteArray, lstView);
        }


        /// <summary>
        /// 获取群
        /// </summary>
        /// <param name="webBrs"></param>
        /// <param name="lstView"></param>
        public static void GetQQ(WebBrowser webBrs, ListView lstView)
        {
            var byteArray = System.Text.Encoding.Default.GetBytes(webBrs.DocumentText);
            GetQQNum(byteArray, lstView);
        }

        /// <summary>
        /// 获取群
        /// </summary>
        /// <param name="byteArr"></param>
        /// <param name="lstView"></param>
        public static void GetQunNum(byte[] byteArr, ListView lstView)
        {
            var array = new byte[10];

            var qunnum = "";
            var qunname = "";
            var b = 0;
            QunNum = 0;
            lstView.BeginUpdate();
            for (var i = 0; i < byteArr.Length; i++)
            {
                if (byteArr[i] == 'g' && byteArr[i + 1] == 'r' && byteArr[i + 2] == 'o' && byteArr[i + 3] == 'u' &&
                    byteArr[i + 4] == 'p' && byteArr[i + 5] == 's' && byteArr[i + 7] == 'l' && byteArr[i + 8] == 'i' &&
                    byteArr[i + 9] == 's' && byteArr[i + 10] == 't')
                {
                    for (var n = i + 11; n < byteArr.Length; n++)
                    {
                        if (byteArr[n] == '-' && byteArr[n + 1] == 'g' && byteArr[n + 2] == 'r' && byteArr[n + 3] == 'o' &&
                            byteArr[n + 4] == 'u' && byteArr[n + 5] == 'p' && byteArr[n + 6] == 'i' &&
                            byteArr[n + 7] == 'd' && byteArr[n + 8] == '=')
                        {
                            b = 0;
                            qunnum = "";
                            QunNum++;
                            var cnum = 0;
                            //获取群号码
                            for (var j = n + 10; byteArr[j] != '"'; j++, b++)
                            {
                                array[b] = byteArr[j];
                                qunnum += array[b] - 48;
                                cnum = j + 1;
                            }
                            var nameArray = new byte[40];
                            b = 0;
                            //获取群名称
                            for (var j = cnum + 24; byteArr[j] != '<'; j++, b++)
                            {
                                if (byteArr[j] == '<')
                                    break;
                                nameArray[b] = byteArr[j];
                            }
                            qunname = Encoding.Default.GetString(nameArray);
                            var item = new ListViewItem(QunNum.ToString());
                            item.SubItems.Add(qunnum);
                            item.SubItems.Add(qunname);
                            lstView.Items.Add(item);
                        }
                    }
                }
                lstView.EndUpdate();
            }
        }

        /// <summary>
        /// 获取qq号码
        /// </summary>
        /// <param name="webBrs"></param>
        /// <param name="lstView"></param>
        public static void GetQQNum(WebBrowser webBrs, ListView lstView)
        {
            var byteArray = System.Text.Encoding.Default.GetBytes(webBrs.DocumentText);
            var array = new byte[20];
            lstView.BeginUpdate();
            for (var i = 0; i < byteArray.Length; i++)
            {
                //获取qq号码
                if (byteArray[i] == 'n' && byteArray[i + 1] == 'a' && byteArray[i + 2] == 'm' && byteArray[i + 3] == 'e' &&
                    byteArray[i + 4] == 'C' && byteArray[i + 5] == 'a' && byteArray[i + 6] == 'r' &&
                    byteArray[i + 7] == 'd' && byteArray[i + 8] == '_')
                {
                    var qnum = "";
                    QQNum++;
                    var b = 0;
                    for (var n = i + 9; byteArray[n] != '"'; n++, b++)
                    {
                        array[b] = byteArray[n];
                        qnum += array[b] - 48;
                    }

                    //获取qq名称
                    var nameArray = new byte[30];
                    for (var n = i; n < byteArray.Length; n++)
                    {
                        b = 0;
                        if (byteArray[n] == 'b' && byteArray[n + 1] == 'l' && byteArray[n + 2] == 'a' &&
                            byteArray[n + 3] == 'n' && byteArray[n + 4] == 'k' && byteArray[n + 5] == '"' &&
                            byteArray[n + 6] == '>')
                        {
                            for (var j = n + 7; byteArray[j] != '<'; j++, b++)
                            {
                                nameArray[b] = byteArray[j];
                            }
                            break;
                        }
                    }
                    var qname = System.Text.Encoding.Default.GetString(nameArray);
                    var item = new ListViewItem(QQNum.ToString());
                    item.SubItems.Add(qnum);
                    item.SubItems.Add(qname);
                    lstView.Items.Add(item);
                }
            }
            lstView.EndUpdate();
        }

        /// <summary>
        /// 获取qq号码
        /// </summary>
        /// <param name="byteArr"></param>
        /// <param name="lstView"></param>
        public static void GetQQNum(byte[] byteArr, ListView lstView)
        {
            var array = new byte[20];
            log.Info("大小:" + byteArr.Length);
            //lstView.BeginUpdate();
            for (var i = 0; i < byteArr.Length; i++)
            {
                //获取qq号码
                if (byteArr[i] == 'n' && byteArr[i + 1] == 'a' && byteArr[i + 2] == 'm' && byteArr[i + 3] == 'e' &&
                    byteArr[i + 4] == 'C' && byteArr[i + 5] == 'a' && byteArr[i + 6] == 'r' &&
                    byteArr[i + 7] == 'd' && byteArr[i + 8] == '_')
                {
                    var qnum = "";
                    QQNum++;
                    var b = 0;
                    for (var n = i + 9; byteArr[n] != '"'; n++, b++)
                    {
                        array[b] = byteArr[n];
                        qnum += array[b] - 48;
                    }

                    //获取qq名称
                    var nameArray = new byte[30];
                    for (var n = i; n < byteArr.Length; n++)
                    {
                        b = 0;
                        if (byteArr[n] == 'b' && byteArr[n + 1] == 'l' && byteArr[n + 2] == 'a' &&
                            byteArr[n + 3] == 'n' && byteArr[n + 4] == 'k' && byteArr[n + 5] == '"' &&
                            byteArr[n + 6] == '>')
                        {
                            for (var j = n + 7; byteArr[j] != '<'; j++, b++)
                            {
                                nameArray[b] = byteArr[j];
                            }
                            break;
                        }
                    }
                    var qname = System.Text.Encoding.Default.GetString(nameArray);
                    var item = new ListViewItem(QQNum.ToString());
                    item.SubItems.Add(qnum);
                    item.SubItems.Add(qname);
                    lstView.Items.Add(item);
                }
            }
            //lstView.EndUpdate();
        }

        public static NameValueCollection GetQueryString(string queryString, Encoding encoding, bool isEncoded)
        {
            queryString = queryString.Replace("?", "");
            var result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                var count = queryString.Length;
                for (var i = 0; i < count; i++)
                {
                    var startIndex = i;
                    var index = -1;
                    while (i < count)
                    {
                        var item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
                        }
                        else if (item == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key = null;
                    string value = null;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }
                    if (isEncoded)
                    {
                        result[UrlDeCode(key, encoding)] = UrlDeCode(value, encoding);
                    }
                    else
                    {
                        result[key] = value;
                    }
                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        result[key] = string.Empty;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 解码URL.
        /// </summary>
        /// <param name="encoding">null为自动选择编码</param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDeCode(string str, Encoding encoding)
        {
            if (encoding != null)
            {
                return HttpUtility.UrlDecode(str, encoding);
            }
            var utf8 = Encoding.UTF8;
            //首先用utf-8进行解码                     
            var code = HttpUtility.UrlDecode(str.ToUpper(), utf8);
            //将已经解码的字符再次进行编码.
            var encode = HttpUtility.UrlEncode(code, utf8).ToUpper();
            encoding = str == encode ? Encoding.UTF8 : Encoding.GetEncoding("gb2312");
            return HttpUtility.UrlDecode(str, encoding);
        }


        public static string GetHtml(string url, CookieContainer cookieContainer)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = "http://qun.qzone.qq.com/group";
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1";
                request.Method = "GET";
                response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    var str = reader.ReadToEnd();
                    reader.Close();
                    responseStream.Close();
                    request.Abort();
                    response.Close();
                    return str;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return string.Empty;
        }

        // TODO
        /// <summary>
        /// 根据地址获取内容
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        private WebBrowser GetPage(string Url)
        {
            WebBrowser myWB = new WebBrowser();
            myWB.ScrollBarsEnabled = false;
            myWB.Navigate(Url);
            while (myWB.ReadyState != WebBrowserReadyState.Complete)
            {
                System.Windows.Forms.Application.DoEvents();
            }
            return myWB;
        }


    }
}
