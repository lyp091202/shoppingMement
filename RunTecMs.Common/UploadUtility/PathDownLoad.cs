using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace RunTecMs.Common.UploadUtility
{
    public class PathDownLoad
    {

        /// <summary>
        ///根据 HttpWebRequest下载方法
        /// </summary>
        /// <param name="server_path"></param>
        /// <param name="local_path"></param>
        public  static  bool Download(string server_path, string local_path)
        {
            try
            {
                Uri downUri = new Uri(server_path);
                //建立一个ＷＥＢ请求，返回HttpWebRequest对象           
                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(downUri);
                //设置接收对象的范围为0-10000000字节。

                hwr.AddRange(0, 10000000);

                //流对象使用完后自动关闭
                using (Stream stream = hwr.GetResponse().GetResponseStream())
                {
                    //文件流，流信息读到文件流中，读完关闭
                    using (FileStream fs = System.IO.File.Create(local_path))
                    {
                        //建立字节组，并设置它的大小是多少字节
                        byte[] bytes = new byte[102400];
                        int n = 1;
                        while (n > 0)
                        {
                            //一次从流中读多少字节，并把值赋给Ｎ，当读完后，Ｎ为０,并退出循环
                            n = stream.Read(bytes, 0, 10240);
                            fs.Write(bytes, 0, n);　//将指定字节的流信息写入文件流中
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
