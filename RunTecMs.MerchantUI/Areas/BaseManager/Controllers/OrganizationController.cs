using System.Web.Mvc;
using RunTecMs.MerchantUI.Controllers;


namespace RunTecMs.MerchantUI.Areas.BaseManager.Controllers
{
    /// <summary>
    /// 账户信息
    /// </summary>
    public class OrganizationController : BaseController
    {
        /// <summary>
        /// 收入
        /// </summary>
        /// <returns></returns>
        public ActionResult IncomeInfo()
        {
            return View();
        }
        
        /// <summary>
        /// 结算设置
        /// </summary>
        /// <returns></returns>
        public ActionResult PaymentSet()
        {
            return View();
        }
    }
}
