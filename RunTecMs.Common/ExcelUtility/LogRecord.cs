using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace RunTecMs.Common.ExcelUtility
{
    public class LogRecord
    {
        /// <summary>
        /// 将错误消息进行输出
        /// </summary>
        /// <param name="txt"></param>
        public static void WriteLog(string txt)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error("被系统过滤捕获的错误： " + txt);
        }


        /// <summary>
        /// debug 输出添加文件
        /// </summary>
        /// <param name="mssg"></param>
        public static void ErrorLog(string mssg)
        {
            string FilePath = HttpContext.Current.Server.MapPath("~/Logs/debug.txt");

            try
            {
                if (File.Exists(FilePath))
                {
                    using (StreamWriter tw = File.AppendText(FilePath))
                    {
                        tw.WriteLine(mssg);
                    }

                }
                else
                {
                    TextWriter tw = new StreamWriter(FilePath);
                    tw.WriteLine(mssg);
                    tw.Flush();
                    tw.Close();
                }

            }
            catch (Exception ex)
            {

            }

        }

    }
}
