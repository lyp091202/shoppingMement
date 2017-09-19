using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RunTecMs.RunBLL;
using RunTecMs.Model.Parameter;
using RunTecMs.Model.EnumType;
using RunTecMs.MerchantUI.Controllers;


namespace RunTecMs.MerchantUI.Areas.BaseManager.Controllers
{
    /// <summary>
    /// 账户信息
    /// </summary>
    public class OrganizationController : BaseController
    {
        RunBLL.Organizations.Organization bllOrg = new RunTecMs.RunBLL.Organizations.Organization();
        RunBLL.BusinessData.BaseData bllBaseData = new RunTecMs.RunBLL.BusinessData.BaseData();

        #region 公司操作
        /// <summary>
        /// 公司页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyList()
        {
            return View();
        }

        /// <summary>
        /// 添加公司页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCompany()
        {
            return View();
        }

        /// <summary>
        /// 公司名称的下拉列表
        /// </summary>
        /// <returns></returns>
        public JsonResult CompanyCombo()
        {
            ParaStruct.CompanyStruct comStruct = new ParaStruct.CompanyStruct { };
            IList<Model.ORG.Company> CompanyList = bllOrg.GetCompanyInfo(comStruct);
            if (CompanyList != null)
            {
                return Json(CompanyList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCompanyList(FormCollection form)
        {
            string UsePage = Request["UsePage"].ToString() ?? "";
            string UseGrid = Request["UseGrid"].ToString() ?? "";
            string role = CurrentUser.MaxRoleID.ToString() + ",";
            // 获取列表列名
            IList<Model.SYS.ShowGridColumns> colList = bllBaseData.GetGridColumns(UsePage, UseGrid, role);

            ParaStruct.CompanyStruct comStruct;
            DateTime starttime = DateTime.MinValue;
            DateTime endtime = DateTime.MaxValue;
            string companyName = form["txtName"] ?? "";
            string code = form["txtCode"] ?? "";
            if (form["startTime"] != null && form["startTime"] != "")
            {
                starttime = Convert.ToDateTime(form["startTime"]);
            }
            if (form["endTime"] != null && form["endTime"] != "")
            {
                endtime = Convert.ToDateTime(form["endTime"]);
            }
            comStruct = new ParaStruct.CompanyStruct
            {
                Name = companyName,
                Code = code,
                StartTime = starttime,
                EndTime = endtime
            };
            IList<Model.ORG.Company> CompanyList = bllOrg.GetCompanyInfo(comStruct);
            if (CompanyList == null)
            {
                return Json(new { total = 0, rows = new List<Model.ORG.Company>(), columns = colList }, JsonRequestBehavior.AllowGet);
            }
            int count = CompanyList.Count();
            return Json(new { total = count, rows = CompanyList, columns = colList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取某一公司的信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCompanyInfo()
        {
            int comId = 0;
            int.TryParse(Request["compId"], out comId);
            ParaStruct.CompanyStruct comStruct = new ParaStruct.CompanyStruct();
            comStruct.CompanyID = comId;
            Model.ORG.Company comInfo = bllOrg.GetCompanyInfo(comStruct)[0];
            if (comInfo == null)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { comInfo }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加公司
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddCompanyInfo(Model.ORG.Company companyInfo)
        {
            int result = bllOrg.AddCompanyInfo(companyInfo);
            if (result > 0)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }

        /// <summary>
        /// 更新公司信息
        /// </summary>
        /// <param name="companyInfo"></param>
        /// <returns></returns>
        public JsonResult UpdateCompany(Model.ORG.Company companyInfo)
        {
            string comName = companyInfo.Name;
            int compId = companyInfo.CompanyID;
            bool ExistName = bllOrg.UpdateCompanyInfo(compId, companyInfo);
            if (ExistName == true)
            {
                return Json(new { status = TestType.Existence });
            }
            else
            {
                bool result = bllOrg.UpdateCompanyInfo(compId, companyInfo);

                if (result)
                {
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
        }

        /// <summary>
        /// 删除公司
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteCompany()
        {
            string returnValue = string.Empty;
            string deleteID = Request["IDStr"] ?? null;
            string IDStr = Request["IDStr"].ToString();

            returnValue = bllOrg.DeleteCompanyInfo(IDStr) ? "ok" : "error";
            return Content(returnValue);
        }

        #endregion

        #region 部门操作

        /// <summary>
        /// 部门页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DepartmentList()
        {
            return View();
        }

        public ActionResult AddDepartment()
        {
            return View();
        }

        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public JsonResult GetDepartList(FormCollection form)
        {
            ParaStruct.DepartStruct departStruct;
            int comId = 0, depId = 0;
            int.TryParse(Request["editDepId"], out depId);
            int.TryParse(form["CompanyName"], out comId);
            string name = form["txtName"] ?? "";
            string code = form["txtCode"] ?? "";

            departStruct = new ParaStruct.DepartStruct
            {
                CompanyID = comId,
                Name = name,
                Code = code
            };
            IList<Model.ORG.DepartmentDetail> DepartList = bllOrg.GetDepartment(departStruct);
            if (DepartList == null)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
            int count = DepartList.Count();
            if (depId > 0)
            {
                var data = DepartList.Where(m => m.DepID == depId);
                IList<Model.ORG.DepartmentDetail> departlist = (IList<Model.ORG.DepartmentDetail>)data.ToList();
                Model.ORG.DepartmentDetail departInfo = departlist[0];
                return Json(new { departInfo }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { total = count, rows = DepartList }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddDepInfo(Model.ORG.Department depart)
        {
            depart.AddTime = DateTime.Now;
            depart.Value = "";
            depart.IsSYSDBA = false;
            depart.IsDel = false;
            bool ExistDep = bllOrg.SameDepName(depart.Name, depart.DepCode, depart.CompanyID, 0, depart.ParentDepID);
            if (ExistDep == true)
            {
                return Json(new { status = TestType.Existence });
            }
            int result = bllOrg.AddDepartInfo(depart);
            if (result > 0)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }

        /// <summary>
        /// 更新部门信息
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateDepartment(Model.ORG.Department departInfo)
        {
            string depName = departInfo.Name;
            int depId = departInfo.DepID;
            bool ExistName = bllOrg.SameDepName(departInfo.Name, departInfo.DepCode, departInfo.CompanyID, departInfo.DepID, departInfo.ParentDepID);
            if (ExistName == true)
            {
                return Json(new { status = TestType.Existence });
            }
            else
            {
                bool result = bllOrg.UpdateDepartmentInfo(departInfo.DepID, departInfo);

                if (result)
                {
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteDepartment()
        {
            string returnValue = string.Empty;
            string deleteID = Request["IDStr"] ?? null;
            string IDStr = Request["IDStr"].ToString();

            returnValue = bllOrg.DeleteDepartment(IDStr) ? "ok" : "error";
            return Content(returnValue);
        }

        #endregion

        #region  职位(job)操作
        /// <summary>
        /// 职位视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Job()
        {
            ViewBag.Role = Session["Role"];
            return View();
        }

        /// <summary>
        /// 获取职位列表
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public JsonResult GetJobList(FormCollection form)
        {
            int page = int.Parse(Request["page"]);//当前页数
            int rows = int.Parse(Request["rows"]);//每页显示的记录条数
            string Name = form["Name"] ?? null;
            string Value = form["Value"] ?? null;

            IList<Model.ORG.JobInfo> JobList = bllOrg.GetJobList(page, rows, Name, Value);
            if (JobList == null)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
            int count = bllOrg.GetJobCount(Name, Value);
            return Json(new { total = count, rows = JobList }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 修改添加Job
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult EditJob(FormCollection form)
        {
            string returnValue = string.Empty;
            string Name = form["jobName"] ?? null;
            string Value = form["jobValue"] ?? null;
            string department = form["department"] ?? null;
            int JobID = 0;
            int.TryParse(form["JobID"], out JobID);

            Model.ORG.JobInfo job = new Model.ORG.JobInfo
            {
                Name = Name,
                Value = Value,
                department = department,
                JobID = JobID

            };
            //添加
            if (JobID > 0)
            {
                bool role = Convert.ToBoolean(Session["Role"]);
                if (role)
                {
                    job.IsSYSDBA = true;
                }
                else
                {
                    job.IsSYSDBA = false;
                }

                returnValue = bllOrg.EditJob(job) ? "ok" : "error";
            }
            //代表是修改
            else
            {
                returnValue = bllOrg.EditJob(job) ? "ok" : "error";
            }
            return Content(returnValue);

        }
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteJob()
        {
            string returnValue = string.Empty;
            string deleteID = Request["IDStr"] ?? null;
            string IDStr = Request["IDStr"].ToString();

            returnValue = bllOrg.DeleteJob(IDStr) ? "ok" : "error";
            return Content(returnValue);
        }

        #endregion
    }
}
