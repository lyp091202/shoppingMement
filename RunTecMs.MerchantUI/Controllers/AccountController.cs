using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using RunTecMs.Common;
using RunTecMs.Common.DEncrypt;
using RunTecMs.Common.ValidCodeUtility;
using RunTecMs.MerchantUI.Common;
using RunTecMs.Model.EnumType;

namespace RunTecMs.MerchantUI.Controllers
{
    public class AccountController : Controller
    {
        private static readonly RunBLL.Organizations.Employee empBll = new RunBLL.Organizations.Employee();

        /// <summary>
        /// 登录首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        #region ajax请求的方法
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="login">登录对象</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(Model.ORG.Login login)
        {
            //数据模型验证是否通过
            if (!ModelState.IsValid)
            {
                foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
                {
                    return Json(new { status = ReturnType.数据验证有误, msg = error.ErrorMessage });
                }
            }
            //验证验证码是否为空
            if (IsNullValidCode())
            {
                return Json(new { status = ReturnType.请重新获取验证码, msg = ReturnType.请重新获取验证码.ToString() });
            }
            //验证验证码是否相等
            if (!IsEqualValidCode(login.ValidCode.ToLower()))
            {
                return Json(new { status = ReturnType.验证码错误, msg = "验证码不正确，请重新输入。" });
            }

            // 获取用户
            Model.ORG.LoginEmployee employee = empBll.GetEmployeeByLoginName(login.UserName);
            //验证用户是否为空
            if (employee == null)
            {
                return Json(new { status = ReturnType.用户名不存在, msg = "用户名不正确，请重新输入。" });
            }
            string userPwd = DEncrypt.CalculatePassword(login.Pwd.Trim(), employee.PwdSalt);
            //验证商户密码是否正确
            if (employee.Password.Trim() != userPwd)
            {
                return Json(new { status = ReturnType.密码错误, msg = "密码不正确，请重新输入。" });
            }
            // 账户是否禁用
            if (!employee.Active)
            {
                return Json(new { status = ReturnType.此账号已禁用, msg = "此账户已禁用，请联系管理员。"});
            }
            //设置Cookie
            FormsAuthentication.SetAuthCookie(login.UserName, false);
            Session["employee"] = employee;

            employee.LastLoginTime = DateTime.Now;
            //判断登录的用户的角色
            int RoleMax = employee.MaxRoleID;
            Session["RoleMax"] = RoleMax;

            ViewBag.Role = false;
            if (RoleMax == (int)RoleValue.超级管理员)
            {
                ViewBag.Role = true;
            }
            Session["Role"] = ViewBag.Role;

            return Json(new { status = ReturnType.登录成功, msg = "/Home/Index" });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePwd()
        {
            string pwd = Request["newpwd"] ?? "";
            bool result = InputValidate.CheckPwd(pwd);
            if (!result)
            {
                return Json(new { status = ReturnType.密码格式不正确, msg = ReturnType.密码格式不正确.ToString() });
            }
            if (Session["employee"] == null)
            {
                return Json(new { status = ReturnType.登录超时, msg = "/Account/Index" });
            }
            Model.ORG.LoginEmployee employee = (Model.ORG.LoginEmployee)Session["employee"];
            employee.Password = pwd;
            bool isUpdate = empBll.UpdateEmployeePwd(employee);
            if (isUpdate)
            {
                Session["employee"] = null;
            }
            return Json(isUpdate ? new { status = ReturnType.密码修改成功, msg = "/Account/Index" } : new { status = ReturnType.密码修改失败, msg = ReturnType.密码修改失败.ToString() });
        }
        #endregion

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return View("Index");
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidCode()
        {
            YZMHelper yzm = new YZMHelper();
            Session["code"] = yzm.Text;
            yzm.CreateImage();

            Bitmap b = yzm.Image;
            MemoryStream ms = new MemoryStream();
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = ms.GetBuffer();
            ms.Close();

            return File(bytes, @"image/Jpeg");
        }

        /// <summary>
        /// 检查验证码是否是空
        /// </summary>
        /// <returns></returns>
        private bool IsNullValidCode()
        {
            return Session["code"] == null || string.IsNullOrEmpty(Session["code"].ToString());
        }

        /// <summary>
        /// 检查验证码是否相等
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        private bool IsEqualValidCode(string code)
        {
            if (Session["code"] == null || string.IsNullOrEmpty(Session["code"].ToString())) return false;
            return Session["code"].ToString() == code.Trim();
        }
    }
}
