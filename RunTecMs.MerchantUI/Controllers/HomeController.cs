using System.Collections.Generic;
using System.Web.Mvc;
using RunTecMs.Model.EnumType;
using System.Linq;
using System.Text;

namespace RunTecMs.MerchantUI.Controllers
{
    public class HomeController : BaseController
    {
        RunBLL.Organizations.Module bllModule = new RunBLL.Organizations.Module();
        RunBLL.BusinessData.MessageManager bllMessage = new RunBLL.BusinessData.MessageManager();
        RunBLL.BusinessData.BaseData bllBaseData = new RunBLL.BusinessData.BaseData();

        /// <summary>
        /// 管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.staff = (int)RoleValue.销售顾问;
            ViewBag.RoleMax = CurrentUser.MaxRoleID;
            return View(CurrentUser);
        }

        /// <summary>
        /// 获取父级导航
        /// </summary>
        /// <returns></returns>
        public ActionResult GetNavList()
        {
            IList<Model.SYS.Module> modList = bllModule.GetMenu(0, CurrentUser.EmployeeID);
            return PartialView("_NavList", modList);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateEmployeePwd()
        {
            return PartialView("_UpdateEmployeePwd");
        }

        /// <summary>
        /// 工作台部分页面
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkHub()
        {
            return PartialView("_WorkHub", CurrentUser);
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMessageGridList(FormCollection from)
        {
            string UsePage = Request["UsePage"].ToString() ?? "";
            string UseGrid = Request["UseGrid"].ToString() ?? "";
            string role = CurrentUser.MaxRoleID.ToString() + ",";
            int page = 1;
            int rows = 20;
            int.TryParse(Request["page"], out page);//当前页数
            int.TryParse(Request["rows"], out rows);//每页显示的记录条数

            // 获取列表列名
            IList<Model.SYS.ShowGridColumns> colList = bllBaseData.GetGridColumns(UsePage, UseGrid, role);

            // 获取登陆者ID
            int LoginEmployeeID = CurrentUser.EmployeeID;

            Model.Common.MessageManager modeData = new Model.Common.MessageManager()
            {
                ReceivedEmpID = LoginEmployeeID.ToString()
            };

            int msgCount = bllMessage.GetNoReadMessageCount(LoginEmployeeID);
            IList<Model.Common.UIMessageManager> list = bllMessage.GetMessageByFilter(page, rows, modeData);

            if (list == null || list.Count == 0)
            {
                return Json(new { total = 0, columns = colList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { total = msgCount, rows = list, columns = colList }, JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}
