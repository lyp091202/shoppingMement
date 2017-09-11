using System;
using System.IO;
using System.Text.RegularExpressions;
using RunTecMs.Model.EnumType;
using RunTecMs.Common.ExcelUtility;

namespace RunTecMs.MerchantUI.Common
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Utils
    {
        private const string Suffix = ".csv";

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <returns></returns>
        public static string GetFileName(string startTime, string endTime, string name)
        {
            string fileName = "";
            if (!string.IsNullOrWhiteSpace(startTime) && string.IsNullOrWhiteSpace(endTime))
            {
                fileName = startTime + "_" + endTime + "_" + name + Suffix;
                return fileName;
            }
            fileName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + name + Suffix;
            return fileName;
        }

        /// <summary>
        /// 创建导出的csv文件
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string CreateFile(string startTime, string endTime, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(startTime) && string.IsNullOrWhiteSpace(endTime))
            {
                fileName = startTime + "_" + endTime + "_" + fileName + Suffix;
            }
            else
            {
                fileName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + fileName + Suffix;
            }

            // 获取当前程序集的路径
            string currentPath = AppDomain.CurrentDomain.BaseDirectory + "Upload\\";

            string filePath = currentPath + fileName;
            FileStream fs = null;
            try
            {
                if (!Directory.Exists(currentPath))
                {
                    Directory.CreateDirectory(currentPath);
                }
                fs = File.Create(filePath);
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog("创建文件错误" + ex.Message);
                return "Error";
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
            }
            return filePath;
        }

        public static bool VerifyStringFormat(string str)
        {
            return Regex.IsMatch(str, @"^(\d+(,\d+)*)+$");
        }

        public static bool VerifyIncomeTimeFormat(string str)
        {
            return Regex.IsMatch(str, @"^([\d]{1,4}[/-]+[\d]{1,2}(,[\d]{1,4}[/-]+[\d]{1,2})*)+$");
        }

        public static bool VerifyWeekFormat(string str)
        {
            return Regex.IsMatch(str, @"^[\d]{1,4}[/-]+[\d]{1,3}$");
        }

        public static bool VerifyMonthFormat(string str)
        {
            return Regex.IsMatch(str, @"^[\d]{1,4}[/-]+([0-1]{0,1}[0-2]{0,1})$");
        }

        public static bool VerifyYearFormat(string str)
        {
            return Regex.IsMatch(str, @"^[\d]{4}$");
        }

        public static string GetPaymentStatus(int paymentStatus, int incomeMerchantId = -1, int paymentMerchantId = -1)
        {
            if (incomeMerchantId == 0 && paymentMerchantId == 0)
            {
                return Enum.GetName(typeof(EnumPaymentStatus), (int)EnumPaymentStatus.直接到账);
            }
            return Enum.GetName(typeof(EnumPaymentStatus), paymentStatus == 0 ? (int)EnumPaymentStatus.未结算 : paymentStatus);
        }
    }
}