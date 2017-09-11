using System.Web;
using System.Web.Optimization;

namespace RunTecMs.MerchantUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Script/jquery-2.1.4").Include("~/Scripts/jquery-2.1.4.js"));
            bundles.Add(new StyleBundle("~/Content/Login").Include("~/Content/Login.css"));

            #region easyui的css和js
            bundles.Add(new StyleBundle("~/Scripts/easyui-1.4.5/themes/default/css").Include("~/Scripts/easyui-1.4.5/themes/default/easyui.css"));
            bundles.Add(new StyleBundle("~/Scripts/easyui-1.4.5/themes/css").Include("~/Scripts/easyui-1.4.5/themes/IconExtension.css", "~/Scripts/easyui-1.4.5/themes/icon.css"));
            bundles.Add(new StyleBundle("~/Content/common/css").Include("~/Content/common.css"));

            bundles.Add(new ScriptBundle("~/easyui/js").Include("~/Scripts/jquery-1.11.3.min.js",
                "~/Scripts/easyui-1.4.5/jquery.easyui.min.js", "~/Scripts/easyui-1.4.5/locale/easyui-lang-zh_CN.js",
                "~/Scripts/common.js", "~/Scripts/datagridCommon.js", "~/Scripts/validatebox.js"));
            bundles.Add(new ScriptBundle("~/Scripts/EasyuiExtend").Include("~/Scripts/validatebox.js"));

            #endregion

            #region 图表的js
            bundles.Add(new ScriptBundle("~/Scripts/Chart").Include("~/Areas/FFManager/Scripts/highcharts.js"));
            #endregion

            BundleTable.EnableOptimizations = false;
            BundleTable.Bundles.IgnoreList.Clear();
            BundleTable.Bundles.IgnoreList.Ignore(".min.js", OptimizationMode.Always);  
        }
        
    }
}