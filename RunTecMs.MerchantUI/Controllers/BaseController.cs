using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using RunTecMs.Common;
using RunTecMs.Common.CsvUtility;
using RunTecMs.Common.ExcelUtility;
using RunTecMs.Common.FileUtility;
using RunTecMs.MerchantUI.Common;

namespace RunTecMs.MerchantUI.Controllers
{
    public class BaseController : Controller
    {
        private RunBLL.Organizations.Employee bllEmployee = new RunBLL.Organizations.Employee();
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            var isAuthenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
            if (filterContext.HttpContext.Session != null)
            {
                var objSession = filterContext.HttpContext.Session["employee"];
                if (!isAuthenticated || objSession == null)
                {
                    var content = new ContentResult
                    {
                        Content = string.Format("<script type='text/javascript' defer>alert('您没有权限进入本页或当前登录用户已过期！\\n请重新登录或与管理员联系！');parent.window.location.href='{0}';</script>", FormsAuthentication.LoginUrl)
                    };
                    filterContext.Result = content;
                }
                else
                {
                    if (CurrentUser == null)
                    {
                        CurrentUser = (Model.ORG.LoginEmployee)objSession;
                    }
                    if (filterContext.HttpContext.Session["userBusiness"] == null)
                    {
                        //RunBLL.BusinessData.BaseData bllBase = new RunBLL.BusinessData.BaseData();
                        //CurrentBusinessList = bllBase.GetSelectBusiness(CurrentUser.EmployeeID);
                        //Session["userBusiness"] = CurrentBusinessList;
                    }
                    else
                    {
                        CurrentBusinessList = (IList<Model.ORG.Business>)filterContext.HttpContext.Session["userBusiness"];
                    }
                }
            }
        }
        /// <summary>
        /// 当前已登录用户
        /// </summary>
        public Model.ORG.LoginEmployee CurrentUser { get; set; }

        public IList<Model.ORG.Business> CurrentBusinessList { get; set; }


        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="fileName">//客户端保存的文件名 </param>
        /// <returns></returns>
        public bool DownFile(string fileName)
        {
            try
            {

                string filePath = System.Web.HttpContext.Current.Server.MapPath("/") + "Upload/" + fileName;
                FileStream fs = new FileStream(filePath, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog("下载失败：" + ex.Message);
                return false;
            }
            DirFile.DeleteFile("Upload/" + fileName);
            return true;
        }
    }
}
