using System;

namespace RunTecMs.Log4Net
{
    /// <summary>
    /// 记录日志
    /// 使用方法：
    /// 1.在需要记录日志的项目中引用本项目。
    /// 2.将本项目中的log4net.config复制到需要记录日志的项目中。
    /// 3.调用LogHelper类的方法记录日志。
    /// 备注：请勿将本项目中的log4net.config删除，
    /// 以便于在其他项目中需要记录日志时将本项目中的log4net.config复制过去使用。
    /// </summary>
    public class LogHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);

        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="type">出错类的类型</param>
        /// <param name="message">信息</param>
        public static void WriteLog(Type type, string message)
        {
            log4net.LogManager.GetLogger(type).Error(message);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">信息</param>
        public static void Debug(string message)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        /// <summary>
        /// 一般信息
        /// </summary>
        /// <param name="message">信息</param>
        public static void Info(string message)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="message">信息</param>
        public static void Warn(string message)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
        }

        /// <summary>
        ///  一般错误信息
        /// </summary>
        /// <param name="message">信息</param>
        public static void Error(string message)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
        }

        /// <summary>
        /// 致命错误信息
        /// </summary>
        /// <param name="message">信息</param>
        public static void Fatal(string message)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
        }
    }
}
