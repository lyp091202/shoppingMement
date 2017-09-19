using System.Web.Mvc;

namespace RunTecMs.MerchantUI.Areas.BaseManager
{
   public class BaseManagerAreaRegistration : AreaRegistration
   {
      public override string AreaName
      {
         get
         {
            return "BaseManager";
         }
      }

      public override void RegisterArea(AreaRegistrationContext context)
      {
         context.MapRoute(
             "BaseManager_default",
             "BaseManager/{controller}/{action}/{id}",
             new { action = "Index", id = UrlParameter.Optional }
         );
      }
   }
}
