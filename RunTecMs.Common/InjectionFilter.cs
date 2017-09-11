using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace RunTecMs.Common
{
    /// <summary>
    /// 注入过滤！！
    /// </summary>
    public static class InjectionFilter
    {
        public static string Filter(string inputString)
        {
            string result;
            if (!string.IsNullOrEmpty(inputString))
            {
                string htmlCode = InjectionFilter.XSSFilter(inputString);
                string source = InjectionFilter.FlashFilter(htmlCode);
                string text = InjectionFilter.SqlFilter(source);
                result = text;
            }
            else
            {
                result = inputString;
            }
            return result;
        }

        public static string XSSFilter(string source)
        {
            string result;
            if (string.IsNullOrEmpty(source))
            {
                result = source;
            }
            else
            {
                string text = HttpUtility.UrlDecode(source);
                string replacement = " onXXX =";
                string pattern = "<[^<>]*>";
                string pattern2 = "([\\s]|[:])+[o]{1}[n]{1}\\w*\\s*={1}";
                string pattern3 = "\\s*((javascript)|(vbscript))\\s*[:]?";
                string pattern4 = "<([\\s/])*script.*>";
                string pattern5 = "<([\\s/])*\\S.+>";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Regex regex2 = new Regex(pattern2, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Regex regex3 = new Regex(pattern4, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Regex regex4 = new Regex(pattern3, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Regex regex5 = new Regex(pattern5, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match match = regex.Match(text);
                while (match.Success)
                {
                    string text2 = match.Groups[0].Value;
                    Match match2 = regex3.Match(text2);
                    if (match2.Success)
                    {
                        text2 = regex3.Replace(text2, "");
                        text = text.Replace(match.Groups[0].Value, text2);
                    }
                    match = match.NextMatch();
                }
                match = regex.Match(text);
                Match match3;
                while (match.Success)
                {
                    string text2 = match.Groups[0].Value;
                    match3 = regex2.Match(text2);
                    if (match3.Success)
                    {
                        text2 = regex2.Replace(text2, replacement);
                        text = text.Replace(match.Groups[0].Value, text2);
                    }
                    match = match.NextMatch();
                }
                match = regex.Match(text);
                while (match.Success)
                {
                    string text2 = match.Groups[0].Value;
                    Match match4 = regex4.Match(text2);
                    if (match4.Success)
                    {
                        text2 = regex4.Replace(text2, "");
                        text = text.Replace(match.Groups[0].Value, text2);
                    }
                    match = match.NextMatch();
                }
                match3 = regex2.Match(text);
                while (match3.Success)
                {
                    string text2 = match3.Groups[0].Value;
                    text2 = regex2.Replace(text2, replacement);
                    text = text.Replace(match3.Groups[0].Value, text2);
                    match3 = match3.NextMatch();
                }
                Match match5 = regex5.Match(text);
                while (match5.Success)
                {
                    string text2 = match5.Groups[0].Value;
                    text2 = regex5.Replace(text2, "");
                    text = text.Replace(match5.Groups[0].Value, text2);
                    match5 = match5.NextMatch();
                }
                result = text;
            }
            return result;
        }

        public static string FlashFilter(string htmlCode)
        {
            string result;
            if (string.IsNullOrEmpty(htmlCode))
            {
                result = htmlCode;
            }
            else
            {
                string pattern = "\\w*<OBJECT\\s+.*(macromedia)[\\s*|\\S*]{1,1300}</OBJECT>";
                result = Regex.Replace(htmlCode, pattern, "", RegexOptions.Multiline);
            }
            return result;
        }

        public static string SqlFilter(string source)
        {
            string result;
            if (string.IsNullOrEmpty(source))
            {
                result = source;
            }
            else
            {
                source = source.Replace("'", "''").Replace(";", "；").Replace("(", "（").Replace(")", "）");
                source = Regex.Replace(source, "select", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "insert", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "update", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "delete", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "drop", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "truncate", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "declare", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "/add", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "net user", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "exec", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "execute", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "xp_", "x p_", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "sp_", "s p_", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "0x", "0 x", RegexOptions.IgnoreCase);
                result = source;
            }
            return result;
        }

        public static string QuoteFilter(string source)
        {
            string result;
            if (string.IsNullOrEmpty(source))
            {
                result = source;
            }
            else
            {
                source = source.Replace("'", "");
                source = source.Replace("\"", "");
                result = source;
            }
            return result;
        }
    }
}
