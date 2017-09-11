using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RunTecMs.MerchantUI.Controllers
{
    /// <summary>
    /// 菜单导航控制器
    /// </summary>
    public class MenuController : BaseController
    {
        private readonly RunBLL.Organizations.Module bllModule = new RunBLL.Organizations.Module();

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id">父级模块id</param>
        /// <returns></returns>
        public JsonResult GetMenuList(int? id)
        {
            var navId = id ?? -1;
            var module = bllModule.GetMenu(navId, CurrentUser.EmployeeID);
            return module != null ? Json(module.Select(m => new { m.ModuleID, m.Name, m.Path }), JsonRequestBehavior.AllowGet) : Json(new object[] { }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取导航栏下的子菜单
        /// </summary>
        /// <param name="id">父级菜单id</param>
        /// <returns></returns>
        public JsonResult GetChildMenuList(int? id)
        {
            var navId = id ?? -1;
            var module = bllModule.GetMenu(navId, CurrentUser.EmployeeID);
            var nodeList = new List<Model.SYS.JsonTree>();
            if (module == null)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
            var data = module.Select(m => new { m.ModuleID, m.Name, m.Path });
            foreach (var item in data)
            {
                var jsonTree = new Model.SYS.JsonTree
                {
                    id = item.ModuleID,
                    text = item.Name
                };
                var attr = new Dictionary<string, string> { { "url", item.Path } };
                jsonTree.attributes = attr;
                nodeList.Add(jsonTree);
            }
            return Json(nodeList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取导航栏下的菜单列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetAllChildMenuList(int? id)
        {
            var navId = id ?? -1;
            var module = bllModule.GetAllChildMenuList(navId, CurrentUser.EmployeeID);

            if (module == null)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
            var data = module.Where(m => m.ModuleID != navId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
