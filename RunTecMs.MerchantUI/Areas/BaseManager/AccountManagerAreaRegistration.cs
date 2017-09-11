using System.Web.Mvc;

namespace RunTecMs.MerchantUI.Areas.AccountManager
{
   public class AccountManagerAreaRegistration : AreaRegistration
   {
      public override string AreaName
      {
         get
         {
            return "AccountManager";
         }
      }

      public override void RegisterArea(AreaRegistrationContext context)
      {
         context.MapRoute(
             "AccountManager_default",
             "AccountManager/{controller}/{action}/{id}",
             new { action = "Index", id = UrlParameter.Optional }
         );
      }
   }
}
