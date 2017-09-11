using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using RunTecMs.Model.Parameter;
using RunTecMs.Model.ORG;

namespace RunTecMs.RunIDAL.Organizations
{
    public interface IEmployee
    {
        /// <summary>
        /// 通过登录名获取用户信息
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pwd">密码</param>
        /// <returns>用户实体</returns>
        LoginEmployee GetEmployeeByLoginName(string loginName, string pwd = "");

        /// <summary>
        /// 通过获取指定公司/部门/角色的员工列表
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <returns>用户实体</returns>
        IList<EmployeeDetailInfo> GetEmployee(ParaStruct.EmployeeStruct employee);

        /// <summary>
        /// 获取员工组织关系
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <returns>用户实体</returns>
        IList<EmployeeDepartmentRole> GetEmployeeRole(EmployeeDepartmentRole employeeRole);

        /// <summary>
        /// 获取员工详细信息
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <returns>用户详细信息</returns>
        IList<Model.ORG.EmployeeDetailInfo> GetAllEmployeeList(ParaStruct.EmployeeStruct employee);

        /// <summary>
        /// 获取员工详细信息(已分配)
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <param name="kbn">查询方式区分</param>
        /// <returns>用户详细信息</returns>
        IList<Model.ORG.EmployeeDetailInfo> GetEmployeeDetailInfo(ParaStruct.EmployeeStruct employee, int kbn);

        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        bool AddEmployee(Model.ORG.EmployeeDetailInfo model);

        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        bool UpdateEmployee(Model.ORG.EmployeeDetailInfo model);

        /// <summary>
        /// 删除员工信息
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <returns>true:成功，false:失败</returns>
        bool DeleteEmployee(int employeeID);

        /// <summary>
        /// 重置员工密码
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        bool ResetEmpPassword(int empId);

        /// <summary>
        /// 修改员工密码
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        bool UpdateEmployeePwd(Model.ORG.EmployeeModel model);

        /// <summary>
        /// 检查登录名是否存在
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>true:存在，false:不存在</returns>
        bool IsExistLoginName(string loginName, int empId);

        /// <summary>
        /// 检查手机号是否存在
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>true:存在，false:不存在</returns>
        bool IsExistMobile(string mobile);

        /// <summary>
        /// 通过公司/部门等获取员工所属ID
        /// </summary>
        /// <param name="PreciseFlg">是否精确查询</param>
        /// <param name="CompanyID">公司</param>
        /// <param name="CompanyID">公司</param>
        /// <param name="DepID">部门</param>
        /// <param name="EmployeeID">员工</param>
        /// <param name="BusinessID">业务</param>
        /// <param name="RoleID">角色</param>
        /// <returns></returns>
        IList<Model.ORG.EmployeeDepartmentRole> GetEmployeeDepartmentID(bool PreciseFlg, int CompanyID = 0, int DepID = 0, int EmployeeID = 0, int BusinessID = 0, int RoleID = 0);

        /// <summary>
        /// 员工所属部门角色追加
        /// </summary>
        /// <param name="model">员工所属部门角色实体</param>
        /// <returns>追加的主键值</returns>
        int AddCompanyIDDepInfo(EmployeeDepartmentRole model);

        /// <summary>
        /// 员工所属部门角色删除
        /// </summary>
        /// <param name="model">员工所属部门角色实体</param>
        /// <returns>true:成功，false:失败</returns>
        bool DeleteCompanyIDDepInfo(EmployeeDepartmentRole model);

        /// <summary>
        /// 更新员工所属角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateComDepRoleInfo(EmployeeDepartmentRole model);

        /// <summary>
        /// 变更客户归属
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateCustomerByEmp(EmployeeDepartmentRole beforeModel, EmployeeDepartmentRole afterModel);

        /// <summary>
        /// 变更订单归属
        /// </summary>
        /// <param name="beforeModel"></param>
        /// <param name="afterModel"></param>
        /// <param name="kbn">1:业务员工 2：客服 3：升级</param>
        /// <returns></returns>
        int UpdateEmpOrderInfo(EmployeeDepartmentRole beforeModel, EmployeeDepartmentRole afterModel, int kbn);

        //个人中心的修改
        bool updatePersonal(LoginEmployee model);

        //更新员工信息
        bool UpdateEmployee(string strSql);

        // 执行SQL
        bool ExeSql(string strSql, List<SqlParameter> listPara);
    }
}
