using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RunTecMs.Common.DBUtility;

namespace RunTecMs.Common
{
    public class FileUrl
    {
        /// <summary>
        /// 获取头像尺寸大小
        /// </summary>
        /// <param name="imgUrl">头像路径</param>
        /// <returns></returns>
        public static string GetAvatarSize(string imgUrl)
        {
            if (string.IsNullOrEmpty(imgUrl))
            {
                return "";
            }
            var result = Regex.Match(imgUrl, @"Avartar/\d+/");
            var size = Regex.Match(result.Value, @"\d+");
            return size.Value;
        }

        /// <summary>
        /// 获取头像地址
        /// </summary>
        /// <param name="path">头像相对路径</param>
        /// <returns></returns>
        public static string GetAvatarUrl(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }
            string pattern = @"http(s)?://([w-]+.)+[w-]+(/[w-./?%&=]*)?";
            bool success = Regex.IsMatch(path, pattern);
            if (success)
            {
                return path;
            }
            string domain = PubConstant.GetConfigString("Domain") ?? "";
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }
            return domain + path;
        }
    }
}
