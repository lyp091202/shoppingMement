using System.Web.Mvc;

namespace RunTecMs.MerchantUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //注册异常处理过滤器
            filters.Add(new ExceptionAttribute());
        }
    }
}