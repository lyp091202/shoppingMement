using System;
using System.Reflection;
using RunTecMs.Common;
using RunTecMs.Common.WebUtility;
//20170227 wangfangxun
//增加沐融教育的定义
namespace RunTecMs.RunDALFactory
{
    public class DataAccessBase
    {
        protected static readonly string AssemblyPath = ConfigHelper.GetConfigString("DAL");
        protected static readonly string fafaAssemblyPath = AssemblyPath + ".FFManager";
        protected static readonly string cusAssemblyPath = AssemblyPath + ".Customer";
        protected static readonly string orgAssemblyPath = AssemblyPath + ".Organizations";
        protected static readonly string busAssemblyPath = AssemblyPath + ".BusinessData";
        protected static readonly string DJCAssemblyPath = AssemblyPath + ".DJCManager";
        protected static readonly string ComAssemblyPath = AssemblyPath + ".CommonSys";
        protected static readonly string MRAssemblyPath = AssemblyPath + ".MRManager";
        protected static readonly string OAAssemblyPath = AssemblyPath + ".OAManager";
        protected static readonly string wechatAssemblyPath = AssemblyPath + ".WechatManager";
        
        #region CreatObject

        private static string Getpath()
        {
            string path = AssemblyPath;
            //switch (kbn)
            //{
            //    case "FFManager":
            //        path = fafaAssemblyPath;
            //        break;
            //    default:
            //        path = AssemblyPath;
            //        break;
            //}
            return path;
        }

        /// <summary>
        /// 不使用缓存
        /// </summary>
        /// <param name="classNamespace"></param>
        /// <param name="kbn">区分：FFManager等</param>
        /// <returns></returns>
        protected static object CreateObjectNoCache(string classNamespace)
        {
            try
            {
                object obj = Assembly.Load(Getpath()).CreateInstance(classNamespace);
                return obj;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 不使用缓存
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="classNamespace"></param>
        /// <param name="kbn">区分：FFManager等</param>
        /// <returns></returns>
        protected static object CreateObjectNoCache(string assemblyPath, string classNamespace)
        {
            try
            {
                object obj = Assembly.Load(Getpath()).CreateInstance(classNamespace);
                return obj;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 使用缓存
        /// </summary>
        /// <param name="classNamespace"></param>
        /// <param name="kbn">区分：FFManager等</param>
        /// <returns></returns>
        protected static object CreateObject(string classNamespace)
        {
            object obj = CacheHelper.GetCache(classNamespace);
            if (obj != null) return obj;
            try
            {
                obj = Assembly.Load(Getpath()).CreateInstance(classNamespace);
                CacheHelper.SetCache(classNamespace, obj);
            }
            catch(Exception e)
            {
                // ignored
            }
            return obj;
        }

        /// <summary>
        /// 使用缓存
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="classNamespace"></param>
        /// <param name="kbn">区分：FFManager等</param>
        /// <returns></returns>
        protected static object CreateObject(string assemblyPath, string classNamespace)
        {
            object obj = CacheHelper.GetCache(classNamespace);
            if (obj != null) return obj;
            try
            {
                obj = Assembly.Load(Getpath()).CreateInstance(classNamespace);
                CacheHelper.SetCache(classNamespace, obj);
            }
            catch
            {
                // ignored
            }
            return obj;
        }


        #endregion
    }

}
