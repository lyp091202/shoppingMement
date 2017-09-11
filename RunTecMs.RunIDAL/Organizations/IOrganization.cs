using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.Model.Parameter;

namespace RunTecMs.RunIDAL.Organizations
{
    public interface IOrganization
    {
        /// <summary>
        ///  获取公司-部门树形信息
        /// </summary>
        /// <param name="companyID">公司ID（可以为空）</param>
        /// <param name="depID">部门ID（可以为空）</param>
        /// <returns></returns>
        IList<Model.ORG.DepTreeInfo> GetDepartmentInfo(int companyID = 0, int depID = 0);

        /// <summary>
        ///  获取公司-部门-员工树形信息
        /// </summary>
        /// <param name="companyID">公司ID（可以为空）</param>
        /// <param name="depID">部门ID（可以为空）</param>
        /// <returns></returns>
        IList<Model.ORG.EmployeeTreeInfo> GetEmployeeInfo(int companyID = 0, int depID = 0, int BusinessValue = 0);

        /// <summary>
        /// 根据用户ID获取公司-部门树形信息
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        IList<Model.ORG.DepTreeInfo> GetDepTreeByEmployee(int EmployeeID);

        /// <summary>
        /// 根据用户ID获取公司-部门-员工树形信息
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        IList<Model.ORG.EmployeeTreeInfo> GetEmployeeTreeByEmployee(int EmployeeID, int BusinessValue, int maxUserRoleID);

        ///  获取公司信息
        /// </summary>
        /// <param name="companyID">公司ID(可以为空)</param>
        /// <returns>公司列表</returns>
        IList<Model.ORG.Company> GetCompanyInfo(ParaStruct.CompanyStruct para);

        /// <summary>
        ///  追加公司信息
        /// </summary>
        /// <param name="company">公司情报</param>
        /// <returns>公司ID</returns>
        int AddCompanyInfo(Model.ORG.Company company);

        /// <summary>
        ///  更新公司信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="company">公司情报</param>
        /// <returns>true:成功 false:失败</returns>
        bool UpdateCompanyInfo(int companyId, Model.ORG.Company company);

        /// <summary>
        ///  删除公司信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns>true:成功 false:失败</returns>
        bool DeleteCompanyInfo(int companyId);

        // <summary>
        /// 验证公司名字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool SameCompanyName(string name, int comId = 0);

        /// <summary>
        ///  获取部门信息
        /// </summary>
        /// <param name="companyID">公司ID(可以为空)</param>
        /// <param name="depID">部门ID(可以为空)</param>
        /// <returns>部门列表</returns>
        IList<Model.ORG.DepartmentDetail> GetDepartment(ParaStruct.DepartStruct departStruct);

        /// <summary>
        /// 验证部门是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <param name="companyId">所属公司</param>
        /// <param name="depId">本部门</param>
        /// <param name="parentDepId">亲部门ID</param>
        /// <returns></returns>
        bool SameDepName(string name, string code, int companyId, int depId , int parentDepId);

        /// <summary>
        ///  追加部门信息
        /// </summary>
        /// <param name="company">部门情报</param>
        /// <returns>部门ID</returns>
        int AddDepartInfo(Model.ORG.Department department);

        /// <summary>
        ///  更新部门信息
        /// </summary>
        /// <param name="companyId">部门ID</param>
        /// <param name="company">部门情报</param>
        /// <returns>true:成功 false:失败</returns>
        bool UpdateDepartmentInfo(int departmentId, Model.ORG.Department department);

        /// <summary>
        ///  删除部门信息
        /// </summary>
        /// <param name="companyId">部门ID</param>
        /// <returns>true:成功 false:失败</returns>
        bool DeleteDepartment(int depID);

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        IList<Model.ORG.Department> GetAllDepartment();

        /// <summary>
        /// 获取业务列表的个数
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        int GetBusinessCount(string Name, string Value);
        /// <summary>
        /// 编辑业务
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        bool EditBusiness(string UpdateID, ParaStruct.Business Business);
        /// <summary>
        /// 删除业务
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        bool DeleteBusiness(string deleteID);
        /// <summary>
        /// 是否存在相同ID
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        bool IsNoExist(string BusinessID);

        #region 职位
        /// <summary>
        /// 获取职位列表数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        IList<Model.ORG.JobInfo> GetJobList(int pageIndex, int pageSize, string Name, string Value);
        /// <summary>
        /// 获取职位数量
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        int GetJobCount(string Name, string Value);
        /// <summary>
        /// 获取职位部门
        /// </summary>
        /// <returns></returns>
        IList<Model.ORG.JobInfo> GetJobDepartment();
        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="Job"></param>
        /// <returns></returns>
        bool EditJob(string UpdateID, ParaStruct.JobPage Job);
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        bool DeleteJob(string deleteID);

        /// <summary>
        /// 获取职位
        /// </summary>
        /// <param name="jobID">职位ID(可以为空)</param>
        /// <returns>职位列表</returns>
        IList<Model.ORG.Job> GetJob(int jobID = 0);

        /// <summary>
        /// 获取数据范围
        /// </summary>
        /// <param name="dataRangeID">数据范围ID(可以为空)</param>
        /// <returns>数据范围列表</returns>
        IList<Model.ORG.DataRang> GetDataRange(int dataRangeID = 0);

        /// <summary>
        ///  获取角色
        /// </summary>
        /// <param name="roleID">角色ID(可以为空)<</param>
        /// <returns>角色列表</returns>
        IList<Model.ORG.Role> GetRole(int roleID, int flag);

        #endregion


    }
}
