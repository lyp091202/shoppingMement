using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace RunTecMs.Common
{
    public class StringPlus
    {
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }
        public static string GetArrayStr(List<int> list)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == (list.Count - 1))
                {
                    builder.Append(list[i].ToString());
                }
                else
                {
                    builder.Append(list[i]);
                    builder.Append(",");
                }
            }
            return builder.ToString();
        }
        public static Size SplitToSize(string str, char splitstr, int defWidth, int defHeight)
        {
            int width = defWidth;
            int height = defHeight;
            if (!string.IsNullOrEmpty(str))
            {
                string[] strArray = str.Split(new char[] { splitstr }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length == 2)
                {
                    width = Globals.SafeInt(strArray[0], defWidth);
                    height = Globals.SafeInt(strArray[1], defHeight);
                }
            }
            return new Size(width, height);
        }
        public static string SubString(object target, string sign, int subLength, bool isShow)
        {
            string text = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                text = target.ToString();
                if (text.Length > subLength)
                {
                    text = text.Substring(0, subLength);
                    if (isShow)
                    {
                        text += sign;
                    }
                }
            }
            return text;
        }
        public static string SubString(object target, int subLength, string sign = null)
        {
            string text = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                text = target.ToString();
                if (text.Length > subLength)
                {
                    if (!string.IsNullOrEmpty(sign))
                    {
                        text = text.Substring(0, subLength - sign.Length / 2);
                        text += sign;
                    }
                    else
                    {
                        text = text.Substring(0, subLength);
                    }
                }
            }
            return text;
        }

        #region 删除最后一个字符之后的字符

        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        #endregion



        public static bool IsNullOrEmpty(object target)
        {
            if ((target != null) && ("" != target.ToString()))
            {
                return (target.ToString().Trim().Length == 0);
            }
            return true;
        }

        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        public static List<string> GetSubStringList(string o_str, char sepeater)
        {
            List<string> list = new List<string>();
            string[] ss = o_str.Split(sepeater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                {
                    list.Add(s);
                }
            }
            return list;
        }

        public static string TrimStart(string srcString, string trimString)
        {
            if (srcString == null)
            {
                return null;
            }
            if (trimString == null)
            {
                return srcString;
            }
            if (IsNullOrEmpty(srcString))
            {
                return string.Empty;
            }
            if (!srcString.StartsWith(trimString))
            {
                return srcString;
            }
            return srcString.Substring(trimString.Length);
        }

        public static bool IsBase64(string str)
        {

            if ((str.Length % 4) != 0)
            {
                return false; //不是正确的BASE64编码
            }
            if (!Regex.IsMatch(str, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
            {
                return false;  //包含不正确的BASE64编码
            }
            return true;
        }

    }
}
