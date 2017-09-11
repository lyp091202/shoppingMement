using System;
using System.Net;
using System.Text;
using System.Data;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;

namespace RunTecMs.Common.MailUtility
{
    public class MailHelper
    {
        /// <summary>
        /// 发送邮件功能
        /// </summary>
        /// <param name="recevier">接收者地址(可以为多个，之间用"；"号隔开)</param> 
        /// <param name="subject">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="fileName">附件文件名</param>
        /// <returns></returns>
        public static bool SendMailUseSmtp(string receviers, string subject, string content, string fileName)
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            // 发送者地址
            string senderAddr = ConfigHelper.GetConfigString("senderAddr");
            // 发送者姓名
            string sname = ConfigHelper.GetConfigString("senderName");
            // 发送者用户名
            string username = ConfigHelper.GetConfigString("username");
            // 发送者用户名
            string pass = ConfigHelper.GetConfigString("pass");
            // SMTP主机地址
            string host = ConfigHelper.GetConfigString("host");

            // 判断添附文件名是否存在
            if (!string.IsNullOrEmpty(fileName) && FileUtility.DirFile.IsExistFile(fileName) == false)
            {
                return false;
            }
            // 邮件标题或内容为空
            if (String.IsNullOrEmpty(subject) || String.IsNullOrEmpty(content))
            {
                return false;
            }

            try
            {
                // 拆分接收者地址字符串
                string[] recevier = receviers.Trim().Split(';');
                for (int i = 0; i < recevier.Length; i++)
                {
                    msg.To.Add(recevier[i]);
                }
            }
            catch
            {
                return false;
            }

            /* 
            *  msg.CC.Add("c@c.com");可以抄送给多人 // 这里的处理与发送多个人一样
            */
            /*3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.From = new MailAddress(senderAddr, sname, System.Text.Encoding.UTF8);
            msg.Subject = subject; //邮件标题
            msg.SubjectEncoding = System.Text.Encoding.UTF8; //邮件标题编码
            if (!string.IsNullOrEmpty(fileName))
            {
                var attach = new Attachment(fileName);
                msg.Attachments.Add(attach); // 发送附件
            }
            msg.Body = content; //邮件内容
            msg.BodyEncoding = System.Text.Encoding.UTF8; //邮件内容编码
            msg.IsBodyHtml = true; //是否是HTML邮件
            msg.Priority = MailPriority.High; //邮件优先级

            SmtpClient client = new SmtpClient(host, 25);
            //username-邮箱用户名  pass-密码
            client.Credentials = new System.Net.NetworkCredential(username, pass);

            //client.Host = host; //邮箱服务器地址
            //client.Port = 465; //邮箱服务器使用的端口
            //client.EnableSsl = true;//经过ssl加密

            //client.UseDefaultCredentials = true;
            //client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //object userState = msg;

            try
            {
                //client.SendAsync(msg, userState);
                client.Send(msg);
                //Console.WriteLine("发送成功");
                // 发送成功
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                throw new Exception("发送邮件出错:" + ex.Message);
            }
        }

        /// <summary>
        /// 发送邮件验证码
        /// </summary>
        /// <param name="receviers">接收者地址(可以为多个，之间用"；"号隔开)</param> 
        /// <param name="subject">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <returns></returns>
        public static bool SendMailCodeUseSmtp(string receviers, string subject, string content)
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            // 发送者地址
            string senderAddr = ConfigHelper.GetConfigString("senderAddr");
            // 发送者姓名
            string sname = ConfigHelper.GetConfigString("senderName");
            // 发送者用户名
            string username = ConfigHelper.GetConfigString("username");
            // 发送者用户名
            string pass = ConfigHelper.GetConfigString("pass");
            // SMTP主机地址
            string host = ConfigHelper.GetConfigString("host");

            // 判断添附文件名是否存在
            //if (FileUtility.DirFile.IsExistFile(fileName) == false)
            //{
            //    return false;
            //}
            // 邮件标题或内容为空
            if (String.IsNullOrEmpty(subject) || String.IsNullOrEmpty(content))
            {
                return false;
            }

            try
            {
                // 拆分接收者地址字符串
                string[] recevier = receviers.Trim().Split(';');
                for (int i = 0; i < recevier.Length; i++)
                {
                    msg.To.Add(recevier[i]);
                }
            }
            catch
            {
                return false;
            }

            /* 
            *  msg.CC.Add("c@c.com");可以抄送给多人 // 这里的处理与发送多个人一样
            */
            /*3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.From = new MailAddress(senderAddr, sname, System.Text.Encoding.UTF8);
            msg.Subject = subject; //邮件标题
            msg.SubjectEncoding = System.Text.Encoding.UTF8; //邮件标题编码
            //var attach = new Attachment(fileName);
            //msg.Attachments.Add(attach); // 发送附件
            msg.Body = content; //邮件内容
            msg.BodyEncoding = System.Text.Encoding.UTF8; //邮件内容编码
            msg.IsBodyHtml = true; //是否是HTML邮件
            msg.Priority = MailPriority.High; //邮件优先级

            SmtpClient client = new SmtpClient(host, 25);
            //username-邮箱用户名  pass-密码
            client.Credentials = new System.Net.NetworkCredential(username, pass);
            //client.Host = host; //邮箱服务器地址
            //client.Port = port; //邮箱服务器使用的端口
            //client.EnableSsl = true;//经过ssl加密

            //client.UseDefaultCredentials = true;
            //client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            object userState = msg;

            try
            {
                //client.SendAsync(msg, userState);
                client.Send(msg);
                //Console.WriteLine("发送成功");
                // 发送成功
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                throw new Exception("发送邮件出错:" + ex.Message);
            }
        }

    }
}
