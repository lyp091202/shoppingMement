using System;
using System.Linq;
using System.Web.Mvc;
using RunTecMs.Common;
using RunTecMs.Common.DEncrypt;

namespace RunTecMs.MerchantUI.Controllers
{
    public class AjaxController : BaseController
    {
        private RunBLL.Organizations.Employee bllEmployee = new RunBLL.Organizations.Employee();

        #region ajax请求验证方法
        /// <summary>
        /// 检验修改密码时输入的旧密码是否正确
        /// </summary>
        /// <returns></returns>
        public JsonResult TestOldPwd()
        {
            string oldpwd = Request["oldPwd"] ?? "";
            if (string.IsNullOrEmpty(oldpwd))
            {
                return Json(new { success = false, msg = "请输入旧密码" });
            }
            string userpwd = DEncrypt.CalculatePassword(oldpwd.Trim(), CurrentUser.PwdSalt);
            if (userpwd == CurrentUser.Password)
            {
                return Json(new { success = true, msg = "旧密码输入正确" });
            }
            return Json(new { success = false, msg = "密码错误" });
        }

        /// <summary>
        /// 检验手机号是否重复（员工）
        /// </summary>
        /// <returns></returns>
        public JsonResult IsExistMobile()
        {
            string mobile = Request["mobile"] ?? "";
            if (string.IsNullOrEmpty(mobile))
            {
                return Json(new { success = false, msg = "请输入手机号" });
            }
            bool existMobile = bllEmployee.IsExistMobile(mobile);
            return existMobile ? Json(new { success = false, msg = "手机号已使用,请更换" }) : Json(new { success = true, msg = "此手机号可以使用" });
        }

        #endregion
    }
}
