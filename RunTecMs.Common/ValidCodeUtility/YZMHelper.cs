using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Security.Cryptography;
using System.Web;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using RunTecMs.Common.WebUtility;

namespace RunTecMs.Common.ValidCodeUtility
{
    /// <summary>
    /// 验证图片类
    /// </summary>
    public class YZMHelper
    {
        #region 私有字段
        private string text;
        private Bitmap image;
        private int letterCount = 4;   //验证码位数
        private int letterWidth = 16;  //单个字体的宽度范围
        private int letterHeight = 20; //单个字体的高度范围
        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        private Font[] fonts = 
    {
       new Font(new FontFamily("Times New Roman"),10 +Next(1),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Georgia"), 10 + Next(1),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Arial"), 10 + Next(1),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Comic Sans MS"), 10 + Next(1),System.Drawing.FontStyle.Regular)
    };
        #endregion

        #region 公有属性
        /// <summary>
        /// 验证码
        /// </summary>
        public string Text
        {
            get { return this.text; }
        }

        /// <summary>
        /// 验证码图片
        /// </summary>
        public Bitmap Image
        {
            get { return this.image; }
        }
        #endregion

        #region 构造函数
        public YZMHelper()
        {
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.AddHeader("pragma", "no-cache");
            HttpContext.Current.Response.CacheControl = "no-cache";
            this.text = Rand.Number(4);
            CreateImage();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        private static int Next(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 绘制验证码
        /// </summary>
        public void CreateImage()
        {
            int int_ImageWidth = this.text.Length * letterWidth;
            Bitmap image = new Bitmap(int_ImageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            for (int i = 0; i < 2; i++)
            {
                int x1 = Next(image.Width - 1);
                int x2 = Next(image.Width - 1);
                int y1 = Next(image.Height - 1);
                int y2 = Next(image.Height - 1);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            int _x = -12, _y = 0;
            for (int int_index = 0; int_index < this.text.Length; int_index++)
            {
                _x += Next(12, 16);
                _y = Next(-2, 2);
                string str_char = this.text.Substring(int_index, 1);
                str_char = Next(1) == 1 ? str_char.ToLower() : str_char.ToUpper();
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(_x, _y);
                g.DrawString(str_char, fonts[Next(fonts.Length - 1)], newBrush, thePos);
            }
            for (int i = 0; i < 10; i++)
            {
                int x = Next(image.Width - 1);
                int y = Next(image.Height - 1);
                image.SetPixel(x, y, Color.FromArgb(Next(0, 255), Next(0, 255), Next(0, 255)));
            }
            image = TwistImage(image, true, Next(1, 3), Next(4, 6));
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, (letterHeight - 1));
            this.image = image;
        }

        /// <summary>
        /// 字体随机颜色
        /// </summary>
        public Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(180);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高,一般为3</param>
        /// <param name="dPhase">波形的起始相位,取值区间[0-2*PI)</param>
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double PI = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI * (double)j) / dBaseAxisLen : (PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }
        #endregion

    }


    /// <summary>
    /// 验证码类
    /// </summary>
    public class Rand
    {
        #region 生成随机数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        #endregion

        #region 生成随机字母与数字
        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        public static string Str(int Length)
        {
            return Str(Length, false);
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        #endregion

        #region 生成随机纯字母随机数
        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        public static string Str_char(int Length)
        {
            return Str_char(Length, false);
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str_char(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        #endregion
    }

    public class MobileValidate
    {
        #region 验证手机号码
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool ValidPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            string pattern = @"^1[3|4|5|8][0-9]\d{8}$";
            return Regex.IsMatch(phone, pattern);
        }
        #endregion

        #region FafaWebApi2.0 短信验证
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public static bool SendValidCode(string mobile)
        {
            if (!ValidPhone(mobile))
            {
                return false;
            }
            CacheHelper.RemoveAllCache("c_" + mobile);
            string yzm = "123456";
            Random rand = new Random();
            //先暂时注释掉,发布正常使用时取消注释
            //for (int i = 0; i < 6; i++)
            //{
            //    yzm += rand.Next(0, 9);
            //}
            //string msg = "您好，您本次的验证码是：" + yzm + "，请勿泄露并尽快进行验证，谢谢！";
            //bool result = SendShortMsg(mobile, msg);
            
            //if (!result)
            //{
            //    return false;
            //}
            CacheHelper.SetCache("c_"+mobile, yzm, TimeSpan.FromMinutes(10));
            return true;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public static string GetValidCode(string mobile)
        {
            if (!ValidPhone(mobile))
            {
                return "";
            }
            var obj = CacheHelper.GetCache("c_"+mobile);
            return obj == null ? "" : obj.ToString();
        }
        #endregion

        #region 发送手机短信，返回发送结果
        public static bool SendShortMsg(string mobile, string msg)
        {
            string sendResult = SendMsg(mobile, msg);
            Int64 st = Convert.ToInt64(sendResult);

            bool sendSucces = false;
            string sendError = "";
            switch (st)
            {
                case -1:
                    sendError = "账号未注册";
                    break;
                case -2:
                    sendError = "其他错误";
                    break;
                case -3:
                    sendError = "帐号或密码错误";
                    break;
                case -4:
                    sendError = "一次提交信息不能超过10000个手机号码，号码逗号隔开";
                    break;
                case -5:
                    sendError = "余额不足，请先充值";
                    break;
                case -6:
                    sendError = "定时发送时间不是有效的时间格式";
                    break;
                case -8:
                    sendError = "发送内容需在3到250字之间";
                    break;
                case -9:
                    sendError = "发送号码为空";
                    break;
                case -104:
                    sendError = "短信内容包含关键字";
                    break;
                default:
                    sendSucces = true;
                    sendError = "发送成功";
                    break;
            }

            if (!sendSucces)
            {
                // to-do: write log: send error
            }

            return sendSucces;
        }

        #endregion

        #region 发送手机短信

        /// <summary>
        /// 这个方法是从WebSerivice拷贝过来的
        /// </summary>
        /// <param name="obphone"></param>
        /// <param name="strmsg"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string SendMsg(string phone, string msg)
        {
            string strMsg = System.Web.HttpUtility.UrlEncode("【发发】 " + msg, System.Text.Encoding.GetEncoding("GB2312"));
            string url;
            string corpid = "HMJ03235"; // "HMJ01730";//"HMJ000619";
            string pwd = "runwang"; // "jinling016";//"060429";
            url = string.Format("http://api.bjszrk.com/sdk/BatchSend2.aspx?CorpID={0}&Pwd={1}&Mobile={2}&Content={3}&Cell=&SendTime=&encode=gbk", corpid, pwd, phone, strMsg);

            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.GetEncoding("GB2312");
            byte[] data = encoding.GetBytes("");
            // 准备请求...  
            try
            {
                // 设置参数  
                request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据 

                response = request.GetResponse() as HttpWebResponse;

                //直到request.GetResponse()程序才开始向目标网页发送Post请求 
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码  
                string content = sr.ReadToEnd();
                return content;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
