using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.RunDALFactory;
using RunTecMs.RunIDAL.Organizations;
using RunTecMs.Model.Parameter;
using System.Data.SqlClient;

namespace RunTecMs.RunBLL.Organizations
{
    public class Employee
    {
        private IEmployee dal = DataAccess.CreateEmployee();

        /// <summary>
        /// 通过登录名获取用户信息
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>用户实体</returns>
        public Model.ORG.LoginEmployee GetEmployeeByLoginName(string loginName)
        {
            return dal.GetEmployeeByLoginName(loginName, "");
        }

        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool AddEmployee(Model.ORG.EmployeeDetailInfo model)
        {
            return model != null && dal.AddEmployee(model);
        }

        /// <summary>
        /// 修改员工密码
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool UpdateEmployeePwd(Model.ORG.EmployeeModel model)
        {
            return model != null && dal.UpdateEmployeePwd(model);
        }


        /// <summary>
        /// 检查登录名是否存在
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>true:存在，false:不存在</returns>
        public bool IsExistLoginName(string loginName, int empId)
        {
            return dal.IsExistLoginName(loginName, empId);
        }

        /// <summary>
        /// 检查手机号是否存在
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>true:存在，false:不存在</returns>
        public bool IsExistMobile(string mobile)
        {
            return !string.IsNullOrEmpty(mobile) && dal.IsExistMobile(mobile);
        }

        /// <summary>
        /// 获取员工详细信息
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <returns>用户详细信息</returns>
        public IList<Model.ORG.EmployeeDetailInfo> GetAllEmployeeList(ParaStruct.EmployeeStruct employee)
        {
            return dal.GetAllEmployeeList(employee);
        }

        /// <summary>
        /// 获取员工组织关系
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <returns>用户详细信息</returns>
        public IList<Model.ORG.EmployeeDepartmentRole> GetEmployeeRole(Model.ORG.EmployeeDepartmentRole employeeRole)
        {
            return dal.GetEmployeeRole(employeeRole);
        }

        /// <summary>
        /// 增加员工组织关系
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <returns>用户详细信息</returns>
        public bool AddEmployeeRole(Model.ORG.EmployeeDepartmentRole employeeRole)
        {
            StringBuilder sbSql = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();

            // 拼接SQL
            sbSql.Append("INSERT INTO Org_EmployeeDepartmentRole(");
            sbSql.Append("EmployeeID,CompanyID,DepID,RoleID,BusinessValue,AddTime,UpdateTime)");
            sbSql.Append("VALUES(@EmployeeID,@CompanyID,@DepID,@RoleID,@BusinessValue,GETDATE(),GETDATE())");

            // 添加参数
            listPara.Add(new SqlParameter("@EmployeeID", employeeRole.EmployeeID));
            listPara.Add(new SqlParameter("@CompanyID", employeeRole.CompanyID));
            listPara.Add(new SqlParameter("@DepID", employeeRole.DepID));
            listPara.Add(new SqlParameter("@RoleID", employeeRole.RoleID));
            listPara.Add(new SqlParameter("@BusinessValue", employeeRole.BusinessValue));
            return dal.ExeSql(sbSql.ToString(), listPara);
        }

        /// <summary>
        /// 更新员工组织关系
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <returns>用户详细信息</returns>
        public bool EditEmployeeRole(Model.ORG.EmployeeDepartmentRole employeeRole)
        {
            // 变更前归属关系获取
            Model.ORG.EmployeeDepartmentRole employee = new Model.ORG.EmployeeDepartmentRole();
            Model.ORG.EmployeeDepartmentRole beforeModel = new Model.ORG.EmployeeDepartmentRole();
            employee.EDID = employeeRole.EDID;
            IList<Model.ORG.EmployeeDepartmentRole> roleList = GetEmployeeRole(employee);
            if (roleList.Count > 0)
            {
                beforeModel = roleList[0];
            }

            StringBuilder sbSql = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();

            // 拼接SQL
            sbSql.Append("UPDATE Org_EmployeeDepartmentRole SET ");
            sbSql.Append(" CompanyID = @CompanyID");
            sbSql.Append(" ,DepID = @DepID");
            sbSql.Append(" ,RoleID = @RoleID");
            sbSql.Append(" ,BusinessValue = @BusinessValue");
            sbSql.Append(" ,UpdateTime = getdate()");
            // 条件
            sbSql.Append(" WHERE EDID = @EDID");

            // 添加参数
            listPara.Add(new SqlParameter("@CompanyID", employeeRole.CompanyID));
            listPara.Add(new SqlParameter("@DepID", employeeRole.DepID));
            listPara.Add(new SqlParameter("@RoleID", employeeRole.RoleID));
            listPara.Add(new SqlParameter("@BusinessValue", employeeRole.BusinessValue));
            listPara.Add(new SqlParameter("@EDID", employeeRole.EDID));

            var result = dal.ExeSql(sbSql.ToString(), listPara);
            if (result)
            {
                
                // 变更客户归属
                dal.UpdateCustomerByEmp(beforeModel, employeeRole);
                // 变更订单归属(业务)
                dal.UpdateEmpOrderInfo(beforeModel, employeeRole, 1);
                // 变更订单归属(客服)
                dal.UpdateEmpOrderInfo(beforeModel, employeeRole, 2);
                // 变更订单归属(升级员工)
                dal.UpdateEmpOrderInfo(beforeModel, employeeRole, 3);

            }
            return dal.ExeSql(sbSql.ToString(), listPara);
        }

        /// <summary>
        /// 删除员工组织关系
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <returns>用户详细信息</returns>
        public bool DelEmployeeRole(int EDID)
        {
            // 变更前归属信息获取
            Model.ORG.EmployeeDepartmentRole employeeRole = new Model.ORG.EmployeeDepartmentRole();
            Model.ORG.EmployeeDepartmentRole beforeModel = new Model.ORG.EmployeeDepartmentRole();
            employeeRole.EDID = EDID;
            IList<Model.ORG.EmployeeDepartmentRole> roleList = GetEmployeeRole(employeeRole);
            if (roleList.Count > 0)
            {
                beforeModel = roleList[0];
            }

            StringBuilder sbSql = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();

            // 拼接SQL
            sbSql.Append("DELETE FROM Org_EmployeeDepartmentRole ");
            // 条件
            sbSql.Append(" WHERE EDID = @EDID");

            // 添加参数
            listPara.Add(new SqlParameter("@EDID", EDID));

            var result = dal.ExeSql(sbSql.ToString(), listPara);
            if (result)
            {

                // 变更后归属
                Model.ORG.EmployeeDepartmentRole afterModel = new Model.ORG.EmployeeDepartmentRole();
                afterModel.EmployeeID = beforeModel.EmployeeID;
                afterModel.CompanyID = beforeModel.CompanyID;
                afterModel.DepID = 0;

                // 变更客户归属
                dal.UpdateCustomerByEmp(beforeModel, afterModel);
                // 变更订单归属(业务)
                dal.UpdateEmpOrderInfo(beforeModel, afterModel, 1);
                // 变更订单归属(客服)
                dal.UpdateEmpOrderInfo(beforeModel, afterModel, 2);
                // 变更订单归属(升级员工)
                dal.UpdateEmpOrderInfo(beforeModel, afterModel, 3);
            }

            return result;
        }

        /// <summary>
        /// 通过获取指定公司/部门/角色的员工列表
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <returns>用户实体</returns>
        public IList<Model.ORG.EmployeeDetailInfo> GetEmployee(ParaStruct.EmployeeStruct employee)
        {
            return dal.GetEmployee(employee);
        }

        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool UpdateEmployee(Model.ORG.EmployeeDetailInfo model)
        {
            return dal.UpdateEmployee(model);
        }

        /// <summary>
        /// 删除员工信息
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <returns>true:成功，false:失败</returns>
        public bool DeleteEmployee(int employeeID)
        {
            return dal.DeleteEmployee(employeeID);
        }
        /// <summary>
        /// 重置员工密码
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public bool ResetEmpPassword(int empId)
        {
            return dal.ResetEmpPassword(empId);
        }

        /// <summary>
        /// 通过公司/部门等获取员工所属ID
        /// </summary>
        /// <param name="model">员工所属部门角色实体</param>
        /// <returns>true:成功，false:失败</returns>
        public IList<Model.ORG.EmployeeDepartmentRole> GetEmployeeDepartmentID(Model.ORG.EmployeeDepartmentRole model)
        {
            return dal.GetEmployeeDepartmentID(false, model.CompanyID, model.DepID, model.EmployeeID, model.BusinessValue, model.RoleID);
        }

        /// <summary>
        /// 员工所属部门角色追加
        /// </summary>
        /// <param name="model">员工所属部门角色实体</param>
        /// <returns>追加的主键值</returns>
        public int AddCompanyIDDepInfo(Model.ORG.EmployeeDepartmentRole model)
        {
            return dal.AddCompanyIDDepInfo(model);
        }

        /// <summary>
        /// 员工所属部门角色删除
        /// </summary>
        /// <param name="model">员工所属部门角色实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool DeleteCompanyIDDepInfo(Model.ORG.EmployeeDepartmentRole model)
        {
            return dal.DeleteCompanyIDDepInfo(model);
        }

        /// <summary>
        /// 更新员工所属角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateComDepRoleInfo(Model.ORG.EmployeeDepartmentRole model)
        {
            return dal.UpdateComDepRoleInfo(model);
        }

        /// <summary>
        /// 更新个人中心
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updatePersonal(Model.ORG.LoginEmployee model)
        {
            return dal.updatePersonal(model);
        }

        /// <summary>
        /// 启用员工
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool ActiveEmployee(Model.ORG.EmployeeModel model)
        {
            // 操作用SQL
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE Org_Employee SET Active = 1 ");
            sbSql.AppendFormat("WHERE EmployeeID = {0}", model.EmployeeID);

            return dal.UpdateEmployee(sbSql.ToString());
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool DestoryEmployee(Model.ORG.EmployeeModel model)
        {
            // 操作用SQL
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("DELETE FROM Org_Employee ");
            sbSql.AppendFormat("WHERE EmployeeID = {0}", model.EmployeeID);

            return dal.UpdateEmployee(sbSql.ToString());
        }

    }
}
