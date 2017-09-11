using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.RunIDAL.Organizations;
using RunTecMs.RunDALFactory;
using RunTecMs.Model.Parameter;

namespace RunTecMs.RunBLL.Organizations
{
    public class Organization
    {
        private readonly IOrganization dal = DataAccess.CreateOrganization();

         /// <summary>
        ///  获取公司-部门树形信息
        /// </summary>
        /// <param name="companyID">公司ID（可以为空）</param>
        /// <param name="depID">部门ID（可以为空）</param>
        /// <returns></returns>
        public IList<Model.ORG.DepTreeInfo> GetDepartmentInfo(int companyID = 0, int depID = 0)
        {
            return dal.GetDepartmentInfo(companyID, depID);
        }

        /// <summary>
        ///  获取公司-部门-员工树形信息
        /// </summary>
        /// <param name="companyID">公司ID（可以为空）</param>
        /// <param name="depID">部门ID（可以为空）</param>
        /// <returns></returns>
        public IList<Model.ORG.EmployeeTreeInfo> GetEmployeeInfo(int companyID = 0, int depID = 0, int BusinessValue = 0)
        {
            return dal.GetEmployeeInfo(companyID, depID, BusinessValue);
        }

        /// <summary>
        /// 根据用户ID获取公司-部门树形信息
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public IList<Model.ORG.DepTreeInfo> GetDepTreeByEmployee(int EmployeeID)
        {
            return dal.GetDepTreeByEmployee(EmployeeID);
        }

        /// <summary>
        /// 根据用户ID获取公司-部门-员工树形信息
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public IList<Model.ORG.EmployeeTreeInfo> GetEmployeeTreeByEmployee(int EmployeeID, int BusinessValue = 0, int maxUserRoleID = 0)
        {
            return dal.GetEmployeeTreeByEmployee(EmployeeID, BusinessValue, maxUserRoleID);
        }

        /// <summary>
        ///  获取公司信息
        /// </summary>
        /// <param name="companyID">公司ID(可以为空)</param>
        /// <returns>公司列表</returns>
        public IList<Model.ORG.Company> GetCompanyInfo(ParaStruct.CompanyStruct para)
        {
            return dal.GetCompanyInfo(para);
        }

        /// <summary>
        /// 验证公司名字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool SameCompanyName(string name, int comId = 0)
        {
            return dal.SameCompanyName(name, comId);
        }

        /// <summary>
        ///  追加公司信息
        /// </summary>
        /// <param name="company">公司情报</param>
        /// <returns>公司ID</returns>
        public int AddCompanyInfo(Model.ORG.Company company)
        {
            return dal.AddCompanyInfo(company);
        }

        /// <summary>
        ///  删除公司信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns>true:成功 false:失败</returns>
        public bool DeleteCompanyInfo(int companyId)
        {
            return dal.DeleteCompanyInfo(companyId);
        }

        /// <summary>
        ///  更新公司信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="company">公司情报</param>
        /// <returns>true:成功 false:失败</returns>
        public bool UpdateCompanyInfo(int companyId, Model.ORG.Company company)
        {
            return dal.UpdateCompanyInfo(companyId,company);
        }

        /// <summary>
        ///  获取部门信息
        /// </summary>
        /// <param name="companyID">公司ID(可以为空)</param>
        /// <param name="depID">部门ID(可以为空)</param>
        /// <returns>部门列表</returns>
        public IList<Model.ORG.DepartmentDetail> GetDepartment(ParaStruct.DepartStruct departStruct)
        {
            return dal.GetDepartment(departStruct);
        }

        /// <summary>
        ///  追加部门信息
        /// </summary>
        /// <param name="company">部门情报</param>
        /// <returns>部门ID</returns>
        public int AddDepartInfo(Model.ORG.Department department)
        {
            return dal.AddDepartInfo(department);
        }

        /// <summary>
        /// 验证部门是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <param name="companyId">所属公司</param>
        /// <param name="depId">部门ID</param>
        /// <param name="parentDepId">亲部门ID</param>
        /// <returns></returns>
        public bool SameDepName(string name, string code, int companyId, int depId, int parentDepId)
        {
            return dal.SameDepName(name, code, companyId, depId, parentDepId);
        }

        /// <summary>
        ///  删除部门信息
        /// </summary>
        /// <param name="companyId">部门ID</param>
        /// <returns>true:成功 false:失败</returns>
        public bool DeleteDepartment(int depID)
        {
            return dal.DeleteDepartment(depID);
        }

        /// <summary>
        ///  更新部门信息
        /// </summary>
        /// <param name="companyId">部门ID</param>
        /// <param name="company">部门情报</param>
        /// <returns>true:成功 false:失败</returns>
        public bool UpdateDepartmentInfo(int departmentId, Model.ORG.Department department)
        {
            return dal.UpdateDepartmentInfo(departmentId, department);
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        public IList<Model.ORG.Department> GetAllDepartment()
        {
            return dal.GetAllDepartment();
        }

        /// <summary>
        ///获取业务个数
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public int GetBusinessCount(string Name, string Value)
        {
            return dal.GetBusinessCount(Name, Value);
        
        }
        /// <summary>
        /// 编辑业务
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="Business"></param>
        /// <returns></returns>
        public bool EditBusiness(string UpdateID, ParaStruct.Business Business)
        {
            return dal.EditBusiness(UpdateID, Business);
        }

        /// <summary>
        /// 删除业务
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        public bool DeleteBusiness(string deleteID)
        {
            return dal.DeleteBusiness(deleteID);
        }
        /// <summary>
        /// 是否存在ID
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public bool IsNoExist(string BusinessID)
        {
            return dal.IsNoExist(BusinessID);
        }

        #region  job

        //获取所有职位
        public IList<Model.ORG.Job> GetAllJob()
        {
            return dal.GetJob();
        }
        //获取所有数据范围
        public IList<Model.ORG.DataRang> GetAllDataRange()
        {
            return dal.GetDataRange();
        }

        /// <summary>
        ///  获取角色
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>角色列表</returns>
        public IList<Model.ORG.Role> GetRole(int roleID, int flag)
        {
            return dal.GetRole(roleID, flag);
        }

        /// <summary>
        /// 获取职位列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public IList<Model.ORG.JobInfo> GetJobList(int pageIndex, int pageSize, string Name, string Value)
        {
            return dal.GetJobList(pageIndex, pageSize, Name, Value);
        }

        /// <summary>
        /// 职位数据个数
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public int GetJobCount(string Name, string Value)
        {
            return dal.GetJobCount(Name, Value);
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        public IList<Model.ORG.JobInfo> GetJobDepartment()
        {
            return dal.GetJobDepartment();
        }

        /// <summary>
        /// 编辑职位
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="perjobid"></param>
        /// <returns></returns>
        public bool EditJob(string UpdateID,ParaStruct.JobPage job)
        {
            return dal.EditJob(UpdateID,job);
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        public bool DeleteJob(string deleteID)
        {
            return dal.DeleteJob(deleteID);
        }
        #endregion



    }
}
