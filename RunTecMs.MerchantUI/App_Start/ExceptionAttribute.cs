using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RunTecMs.MerchantUI
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class ExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Task.Factory.StartNew(() =>
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                string msgTemplate = "在执行 controller[{0}] 的 action[{1}] 时产生异常，异常信息：{2}";
                //写入本地日志
                Log4Net.LogHelper.Error(string.Format(msgTemplate, controllerName, actionName, filterContext.Exception));

                //向数据库写日志
                Model.ORG.MerchantManager currentUser = null;
                if (filterContext.HttpContext.Session != null)
                {
                    object obj = filterContext.HttpContext.Session["merchant"];
                    if (obj != null)
                    {
                        currentUser = (Model.ORG.MerchantManager)obj;
                    }
                }
                Model.SYS.LogError logError = new Model.SYS.LogError
                {
                    ErrorEmployeeID = currentUser == null ? 0 : currentUser.MerchantID,
                    BusinessID = 99
                };
                if (filterContext.HttpContext.Request.Url != null)
                {
                    logError.ErrorUrl = filterContext.HttpContext.Request.Url.ToString();
                    logError.ErrorAction = filterContext.HttpContext.Request.Url.PathAndQuery;
                }
                logError.ErrorThreadID = Thread.CurrentThread.ManagedThreadId.ToString();
                if (filterContext.Exception.TargetSite.DeclaringType != null)
                {
                    logError.ErrorClassName = filterContext.Exception.TargetSite.DeclaringType.ToString();
                }
                logError.ErrorMethodName = filterContext.Exception.TargetSite.Name;
                logError.ErrorInfo = filterContext.Exception.ToString();
                logError.Level = "ERROR";
                logError.ErrorTime = DateTime.Now;
                //new RunBLL.BusinessData.BaseData().InsertLogError(logError);
            });
        }
    }
}