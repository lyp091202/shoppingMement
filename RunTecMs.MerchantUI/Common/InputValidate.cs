using System.Text.RegularExpressions;

namespace RunTecMs.MerchantUI.Common
{
    /// <summary>
    /// 输入验证类
    /// </summary>
    public class InputValidate
    {
        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool CheckPwd(string pwd)
        {
            return Regex.IsMatch(pwd, @"^[a-zA-Z0-9_]{6,12}$");
        }

        /// <summary>
        /// 验证id字符串集合,格式是 1,2,3, 这种形式的字符串
        /// </summary>
        /// <param name="idStr"></param>
        /// <returns></returns>
        public static bool CheckIdSet(string idStr)
        {
            return Regex.IsMatch(idStr, @"^([0-9]+,)+$");
        }
    }
}