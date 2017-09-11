using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using RunTecMs.Common.DBUtility;

namespace RunTecMs.Common.MapLetterUserUtility
{
    public class MapOpenIMUser
    {
        // API地址
        private static string HostV2 = PubConstant.GetConfigString("APIV2Host");
        // 验证(13165126650:123456)
        private static string Authorization = "Basic MTMxNjUxMjY2NTA6MTIzNDU2";

        /// <summary>
        /// 添加云旺客户
        /// </summary>
        /// <param name="Userid">用户ID</param>
        public static void AddOpenIMUser(int Userid)
        {
            try
            {
                if (Userid == 0 || Userid == -1)
                {
                    return;
                }

                string url = HostV2 + "MapUsersToTop?Id=" + Userid;
                //byte[] bytes = Encoding.Default.GetBytes(LoginName + ":" + Password);
                //string Authorization = Convert.ToBase64String(bytes);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                request.Headers.Add("X-User-Agent", "fafa-web");
                request.Headers.Add("Authorization",Authorization);

                Encoding encoding = Encoding.UTF8;
                byte[] buffer = encoding.GetBytes("");
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var ss = reader.ReadToEnd();
                }
                return;
            }
            catch(Exception ex)
            {
                return;
            } 
        }

        /// <summary>
        /// 更新云旺客户
        /// </summary>
        /// <param name="Userid">用户ID</param>
        public static void UpdateOpenIMUser(int Userid)
        {
            try
            {
                if (Userid == 0 || Userid == -1)
                {
                    return;
                }

                string url = HostV2 + "MapUsersToTop?Id=" + Userid;
                //byte[] bytes = Encoding.Default.GetBytes(LoginName + ":" + Password);
                //string Authorization = Convert.ToBase64String(bytes);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Patch";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                request.Headers.Add("X-User-Agent", "fafa-web");
                request.Headers.Add("Authorization", Authorization);

                Encoding encoding = Encoding.UTF8;
                byte[] buffer = encoding.GetBytes("");
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var ss = reader.ReadToEnd();
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// 删除云旺客户
        /// </summary>
        /// <param name="Userid">用户ID</param>
        public static void DeleteOpenIMUser(int Userid)
        {
            try
            {
                if (Userid == 0 || Userid == -1)
                {
                    return;
                }
                string url = HostV2 + "MapUsersToTop?Id=" + Userid;
                //byte[] bytes = Encoding.Default.GetBytes(LoginName + ":" + Password);
                //string Authorization = Convert.ToBase64String(bytes);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "DELETE";
                request.Headers.Add("X-User-Agent", "fafa-web");
                request.Headers.Add("Authorization", Authorization);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var ss = reader.ReadToEnd();
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

    }
}
