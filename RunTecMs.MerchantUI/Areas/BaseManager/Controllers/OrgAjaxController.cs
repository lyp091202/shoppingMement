using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using System.Drawing;
using RunTecMs.MerchantUI.Controllers;
using RunTecMs.Model.EnumType;
using RunTecMs.Model.Parameter;

namespace RunTecMs.RunUI.Areas.OrganizationManager.Controllers
{
    public class OrgAjaxController : BaseController
    {
        RunBLL.BusinessData.BaseData bllBase = new RunBLL.BusinessData.BaseData();
        RunBLL.Organizations.Employee bllEmp = new RunBLL.Organizations.Employee();
        RunBLL.Organizations.Organization bllOrg = new RunTecMs.RunBLL.Organizations.Organization();

        /// <summary>
        /// 检验公司名是否存在
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TestCompanyName()
        {
            string Name = Request["name"].ToString();
            if (string.IsNullOrEmpty(Name))
            {
                return Json(new { status = TestType.Empty });
            }
            else
            {
                bool ExistName = bllOrg.SameCompanyName(Name);
                if (ExistName == true)
                {
                    return Json(new { status = TestType.Existence });
                }
                else
                {
                    return Json(new { status = TestType.success });
                }
            }
        }

        /// <summary>
        /// 部门页面获取公司名称的下拉列表
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
        /// 获取部门名称的下拉列表
        /// </summary>
        /// <returns></returns>
        public JsonResult DepartCombo(int? id)
        {
            int comId = id ?? -1;
            if (comId < 0)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
            IList<Model.ORG.DepTreeInfo> depTree = bllOrg.GetDepartmentInfo(comId);
            if (depTree == null)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
            //var data = module.Where(m => m.ModuleID != navId);
            return Json(depTree, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取 公司-部门 树形结构
        /// </summary>
        /// <returns></returns>
        public JsonResult OrganizeTree()
        {
            Model.ORG.LoginEmployee userInfo = CurrentUser;
            IList<Model.ORG.DepTreeInfo> TreeInfo = bllOrg.GetDepTreeByEmployee(userInfo.EmployeeID);
            if (TreeInfo == null)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
            return Json(TreeInfo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取 公司-部门-员工 树形结构
        /// </summary>
        /// <returns></returns>
        public JsonResult CompDepartEmpTree()
        {
            Model.ORG.LoginEmployee userInfo = CurrentUser;
            IList<Model.ORG.EmployeeTreeInfo> TreeInfo = bllOrg.GetEmployeeTreeByEmployee(userInfo.EmployeeID);
            if (TreeInfo == null)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
            return Json(TreeInfo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 逻辑删除部门
        /// </summary>
        /// <returns></returns>
        public JsonResult DestoryDepart()
        {
            string DepId = Request["departId"].ToString();
            bool result = bllOrg.DeleteDepartment(DepId);
            if (result)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }

        /// <summary>
        /// 上传logo窗口
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadLogoWindow()
        {
            return PartialView("_UploadLogoWindow");
        }

        //上传logo（存在物理地址）
        public void UploadLogo()
        {
            if (Request.Files[0] != null)
            {
                HttpPostedFileBase postFile = Request.Files[0];
                string fileName = Path.GetFileName(postFile.FileName);//获取文件名
                string fileExt = Path.GetExtension(fileName);//获取扩展名
                string path = ConfigurationManager.AppSettings["Logo"] + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(path)));//创建文件夹

                string newfileName = Guid.NewGuid().ToString();//新的文件名
                string fileDir = path + newfileName + fileExt;//完整路径
                postFile.SaveAs(Request.MapPath(fileDir));
                Response.Write("<script type='text/javascript' type='language'>window.parent.uploadCallback('" + fileDir + "')</script>");
                return;
            }
            Response.Write("<script type='text/javascript' type='language'>$.messager.alert('提示','出错，稍后再试','info');</script>");
        }


        /// <summary>
        /// 检验用户登录名是否存在
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TestEmploginName()
        {
            string Name = Request["name"].ToString();
            int empId = 0;
            if (string.IsNullOrEmpty(Name))
            {
                return Json(new { status = TestType.Empty });
            }
            else
            {
                bool ExistName = bllEmp.IsExistLoginName(Name,empId);
                if (ExistName == true)
                {
                    return Json(new { status = TestType.Existence });
                }
                else
                {
                    return Json(new { status = TestType.success });
                }
            }
        }



        /// <summary>
        /// 获取角色的下拉列表（不包含超管和管理员）
        /// </summary>
        /// <returns></returns>
        public JsonResult RoleComboSys()
        {
            IList<Model.ORG.Role> RoleList = bllOrg.GetRole((int)Session["RoleMax"], (int)RoleCombobox.权限查询);
            if (RoleList != null)
            {
                for (int i = 0; i < RoleList.Count; i++)
                {
                    if (RoleList[i].RoleID == 1 )
                    {
                        RoleList.Remove(RoleList[i]);
                    }
                    if (RoleList[i].RoleID == 2)
                    {
                        RoleList.Remove(RoleList[i]);
                        break;
                    }
                }
                return Json(RoleList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取角色的下拉列表
        /// </summary>
        /// <returns></returns>
        public JsonResult RoleCombo()
        {
            IList<Model.ORG.Role> RoleList = bllOrg.GetRole(0, (int)RoleCombobox.权限查询);
            if (RoleList != null)
            {
                for (int i = 0; i < RoleList.Count; i++)
                {
                    if (RoleList[i].RoleID == 1)
                    {
                        RoleList.Remove(RoleList[i]);
                        break;
                    }
                }
                return Json(RoleList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取职位的下拉列表
        /// </summary>
        /// <returns></returns>
        public JsonResult JobCombo()
        {
            IList<Model.ORG.Job> JobList = bllOrg.GetAllJob();
            if (JobList != null)
            {
                return Json(JobList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <returns></returns>
        public JsonResult DepartmentCombo()
        {
            ParaStruct.CompanyStruct comStruct = new ParaStruct.CompanyStruct { };
            ParaStruct.DepartStruct departStruct = new ParaStruct.DepartStruct { };
            IList<Model.ORG.Company> CompanyList = bllOrg.GetCompanyInfo(comStruct);
            IList<Model.ORG.DepartmentDetail> departmentList = bllOrg.GetDepartment(departStruct);
            if (departmentList != null)
            {
                return Json(new { comList = CompanyList, depList = departmentList });
            }
            else
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <returns></returns>
        public JsonResult DestoryEmployee()
        {
            bool result = false;
            Model.ORG.EmployeeDepartmentRole empCDR = new Model.ORG.EmployeeDepartmentRole();
            string employeeId = Request["employeeId"] ?? "";
            string edId = Request["edID"] ?? "";
            int EmployeeId = employeeId == "" ? 0 : Convert.ToInt32(employeeId),
                EdId = edId == "" ? 0 : Convert.ToInt32(edId);

            if (EmployeeId > 0 && EdId == 0)
            {
                result = bllEmp.DeleteEmployee(EmployeeId);
            }
            if (EmployeeId > 0 && EdId > 0)
            {
                empCDR.EDID = EdId;
                empCDR.EmployeeID = EmployeeId;
                result = bllEmp.DeleteCompanyIDDepInfo(empCDR);
            }
            if (result)
            {
                return Json(new { status = "001" });
            }
            else
            {
                return Json(new { status = "009" });
            }
        }

        /// <summary>
        /// 重置员工密码
        /// </summary>
        /// <returns></returns>
        public JsonResult ResetPassword()
        {
            int empId = 0;
            int.TryParse(Request["empId"], out empId);
            if (empId == CurrentUser.EmployeeID)
            {
                return Json(new { status = TestType.Existence });
            }
            bool result = bllEmp.ResetEmpPassword(empId);
            return Json(new { status = true });
        }
        /// <summary>
        /// 判断是否存在员工所属部门角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TestExistRelation()
        {
            Model.ORG.EmployeeDepartmentRole model=new Model.ORG.EmployeeDepartmentRole();
            model.EDID = Convert.ToInt32(Request["edId"] ?? "");
            model.CompanyID = Convert.ToInt32(Request["compId"] ?? "");
            model.DepID = Convert.ToInt32(Request["depId"] ?? "");
            model.RoleID = Convert.ToInt32(Request["roleId"] ?? "");
            model.EmployeeID = Convert.ToInt32(Request["empId"] ?? "");
            IList<Model.ORG.EmployeeDepartmentRole> GetEmpDR = bllEmp.GetEmployeeDepartmentID(model);
            if (GetEmpDR.Count > 0)
            {
                if (GetEmpDR[0].EDID == model.EDID)
                {
                    return Json(new { status = TestType.Undisposed });
                }
                else
                {
                    return Json(new { status = TestType.Existence });
                }
            }
            else
            {
                int result = bllEmp.UpdateComDepRoleInfo(model);
                return Json(new { status = true });
            }
        }

        #region 获取裁剪好的logo
        [HttpPost]
        public JsonResult CutLogo(string imgSrc, string x, string y, string x1, string y1, string zoom, string Id, string Type)
        {
            if (!string.IsNullOrEmpty(imgSrc) && !string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y) && !string.IsNullOrEmpty(x1) && !string.IsNullOrEmpty(y1) && !string.IsNullOrEmpty(zoom))
            {
                if (imgSrc == "/Images/transparent.gif")
                {
                    return Json(new { status = "0004", msg = "请选择图片" });
                }
                int dx = Convert.ToInt32(x);
                int dy = Convert.ToInt32(y);
                int dx1 = Convert.ToInt32(x1);
                int dy1 = Convert.ToInt32(y1);
                double dzoom = Convert.ToDouble(zoom);
                string Exe = ".jpg";//获取文件扩展名
                Image image = Image.FromFile(Server.MapPath(imgSrc));////建立Image对象
                int width = dx1 - dx, height = dy1 - dy;
                if (dzoom != 1)
                {
                    width = (int)((dx1 - dx) * dzoom);
                    height = (int)((dy1 - dy) * dzoom);
                    dx = (int)(dx * dzoom);
                    dy = (int)(dy * dzoom);
                }
                //180 *180  
                Image CutedImg = new Bitmap(180, 180);//头像缩放到180 *180
                Graphics gra = Graphics.FromImage(CutedImg);//创建Graphics对象,为准备好在image上作图
                gra.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                gra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gra.Clear(Color.White);
                gra.DrawImage(image, new Rectangle(0, 0, CutedImg.Width, CutedImg.Height), new Rectangle(dx, dy, width, height), System.Drawing.GraphicsUnit.Pixel);//根据指定位置和指定大小绘图
                int guid = 0;
                if (Id != "" && Id != null)
                {
                    guid = Convert.ToInt32(Id);
                }
                else
                {
                    if (Type.ToString().Trim() == "Company")
                    {
                        guid = bllOrg.GainIdentity();
                    }
                    if (Type.ToString().Trim() == "Department")
                    {
                        guid = bllOrg.GainDepIdentity();
                    }
                }
                string dir = ConfigurationManager.AppSettings["Logo"] + Type.ToString() + "/" + "180/";//生成存储180*180的地址
                string fulldir = Server.MapPath(dir);
                if (!Directory.Exists(fulldir))
                {
                    Directory.CreateDirectory(fulldir);
                }
                string saveOriPath = dir + guid.ToString() + Exe;//完整路径
                System.IO.FileInfo file = new System.IO.FileInfo(saveOriPath);
                if (file.Exists)
                {
                    file.Delete();
                }
                CutedImg.Save(Server.MapPath(saveOriPath));//保存文件
                CutedImg.Dispose();
                gra.Dispose();
                //180 *180  结束


                //50 *50  
                CutedImg = new Bitmap(50, 50);//头像缩放到50 *50
                gra = Graphics.FromImage(CutedImg);//创建Graphics对象,为准备好在image上作图
                gra.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                gra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gra.Clear(Color.White);
                gra.DrawImage(image, new Rectangle(0, 0, CutedImg.Width, CutedImg.Height), new Rectangle(dx, dy, width, height), System.Drawing.GraphicsUnit.Pixel);//根据指定位置和指定大小绘图
                //buffer = Guid.NewGuid().ToByteArray();
                //guid = BitConverter.ToInt64(buffer, 0);

                dir = ConfigurationManager.AppSettings["Logo"] + Type.ToString() + "/" + "50/";//生成存储50*50的地址
                fulldir = Server.MapPath(dir);
                if (!Directory.Exists(fulldir))
                {
                    Directory.CreateDirectory(fulldir);
                }
                string saveMidPath = dir + guid.ToString() + Exe;//完整路径
                System.IO.FileInfo Midfile = new System.IO.FileInfo(saveMidPath);
                if (Midfile.Exists)
                {
                    Midfile.Delete();
                }
                CutedImg.Save(Server.MapPath(saveMidPath));//保存文件
                CutedImg.Dispose();
                gra.Dispose();
                //50 *50  结束


                //30 *30  
                CutedImg = new Bitmap(30, 30);//头像缩放到30 *30
                gra = Graphics.FromImage(CutedImg);//创建Graphics对象,为准备好在image上作图
                gra.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                gra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gra.Clear(Color.White);
                gra.DrawImage(image, new Rectangle(0, 0, CutedImg.Width, CutedImg.Height), new Rectangle(dx, dy, width, height), System.Drawing.GraphicsUnit.Pixel);//根据指定位置和指定大小绘图

                dir = ConfigurationManager.AppSettings["Logo"] + Type.ToString() + "/" + "30/";//生成存储30*30的地址
                fulldir = Server.MapPath(dir);
                if (!Directory.Exists(fulldir))
                {
                    Directory.CreateDirectory(fulldir);
                }
                string saveSmallPath = dir + guid.ToString() + Exe;//完整路径
                System.IO.FileInfo Smallfile = new System.IO.FileInfo(saveSmallPath);
                if (Smallfile.Exists)
                {
                    Smallfile.Delete();
                }
                CutedImg.Save(Server.MapPath(saveSmallPath));//保存文件
                CutedImg.Dispose();
                gra.Dispose();
                //30 *30  结束


                if (saveOriPath != "" && saveMidPath != "" && saveSmallPath != "")
                {
                    return Json(new { status = "0001", AvaSmall = saveSmallPath, AvaMid = saveMidPath, AvaOri = saveOriPath });
                }
                else
                {
                    return Json(new { status = "0002", AvaSmall = "", AvaMid = "", AvaOri = "", msg = "上传失败" });
                }
            }
            else
            {
                return Json(new { status = "0003", msg = "系统错误" });
            }
        }

        #endregion

    }
}
