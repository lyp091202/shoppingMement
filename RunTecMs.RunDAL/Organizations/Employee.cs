using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using RunTecMs.Common.DBUtility;
using RunTecMs.Common.ConvertUtility;
using RunTecMs.Common;
using RunTecMs.Common.DEncrypt;
using RunTecMs.RunIDAL.Organizations;
using RunTecMs.Model.Parameter;
using RunTecMs.Model.EnumType;

namespace RunTecMs.RunDAL.Organizations
{
    public class Employee : IEmployee
    {
        #region  获取员工信息
        /// <summary>
        /// 通过登录名或密码获取用户信息
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pwd">密码</param>
        /// <returns>用户实体</returns>
        public Model.ORG.LoginEmployee GetEmployeeByLoginName(string loginName, string pwd = "")
        {
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> paraList = new List<SqlParameter>();
            sb.AppendLine("SELECT TOP 1  OE.*,"); 
            // 归属信息
            sb.AppendLine("       EDR.CompanyID, EDR.DepID, EDR.RoleID AS MaxRoleID,");
            // 角色信息
            sb.AppendLine("       Role.Name AS RoleName, Role.AuthorityRole,");
            // 部门信息
            sb.AppendLine("       dep.Name AS DepName,");
            sb.AppendLine("       CASE WHEN EDR.RoleID <= 4 THEN 1 ELSE 0 END AS IsManager ");
            sb.AppendLine("  FROM Org_Employee OE, Org_EmployeeDepartmentRole EDR  ");
            sb.AppendLine(" LEFT JOIN Org_Role Role ON Role.RoleID = EDR.RoleID");
            sb.AppendLine(" LEFT JOIN Org_Department dep ON dep.DepID = EDR.DepID");
            sb.AppendLine(" WHERE EDR.EmployeeID = OE.EmployeeID ");
            sb.AppendLine("   AND OE.LoginName = @LoginName");
            paraList.Add(new SqlParameter("@LoginName", loginName));
            if (!string.IsNullOrEmpty(pwd))
            {
                sb.AppendLine("   AND OE.Password = @Password");
                paraList.Add(new SqlParameter("@Password", pwd));
            }

            sb.AppendLine(" ORDER BY EDR.RoleID");

            DataTable dt = DbHelperSQL.Query(sb.ToString(), paraList.ToArray()).Tables[0];

            if (!DataTableTools.DataTableIsNull(dt))
            {
                return ConvertToList.DataTableToList<Model.ORG.LoginEmployee>(dt)[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过员工ID获取用户信息
        /// </summary>
        /// <param name="EmployeeID">员工ID</param>
        /// <returns>用户实体</returns>
        public Model.ORG.EmployeeModel GetEmployeeByEmployeeID(int EmployeeID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select * from Org_Employee");
            sb.AppendLine("where EmployeeID=@EmployeeID");

            SqlParameter[] para = new SqlParameter[]{
             new SqlParameter("@EmployeeID",SqlDbType.Int)
            };

            para[0].Value = EmployeeID;

            DataTable dt = DbHelperSQL.Query(sb.ToString(), para).Tables[0];

            if (!DataTableTools.DataTableIsNull(dt))
            {
                return ConvertToList.DataTableToList<Model.ORG.LoginEmployee>(dt)[0];
            }
            else
            {
                return null;
            }
        }
        #endregion  获取员工信息

        #region  获取员工列表
        /// <summary>
        /// 获取员工列表（全部员工）
        /// </summary>
        /// <param name="CustomerOwenShip">客户参数结构体</param>
        /// <returns></returns>
        public IList<Model.ORG.EmployeeDetailInfo> GetAllEmployeeList(ParaStruct.EmployeeStruct employee)
        {
            IList<Model.ORG.EmployeeDetailInfo> employeeList = new List<Model.ORG.EmployeeDetailInfo>();
            IList<Model.ORG.EmployeeDetailInfo> UnDistributionEmployeeList = new List<Model.ORG.EmployeeDetailInfo>();

            // 取得登录者的权限
            int RoleMax = employee.MaxRoleID;

            // 全部或已分配场合，查询已分配员工
            if (employee.DistributionStatus == (int)DistributionStatus.全部 || employee.DistributionStatus == (int)DistributionStatus.已分配)
            {
                // 取得已分配员工
                employeeList = GetEmployeeDetailInfo(employee, (int)SearchKbn.表单);

                // 选择条件为已分配场合，返回查询结果
                if (employee.DistributionStatus == (int)DistributionStatus.已分配)
                {
                    return employeeList;
                }
            }

            // 管理员并且没选公司场合，还需取得未分配客户
            if (RoleMax <= Convert.ToInt32(RoleValue.总经理) && employee.CompanyID <= 0)
            {
                if (employeeList == null)
                {
                    employeeList = new List<Model.ORG.EmployeeDetailInfo>();
                }
                // 全部或未分配场合，查询未分配客户
                if (employee.DistributionStatus == (int)DistributionStatus.全部 || employee.DistributionStatus == (int)DistributionStatus.未分配)
                {
                    // 取得未分配客户
                    UnDistributionEmployeeList = GetUNDistributionEmployee(employee);
                    if (UnDistributionEmployeeList == null)
                    {
                        return employeeList;
                    }
                    for (int i = 0; i < UnDistributionEmployeeList.Count; i++)
                    {
                        employeeList.Add(UnDistributionEmployeeList[i]);
                    }
                }
            }

            return employeeList;
        }

        /// <summary>
        /// 获取未分配的员工列表
        /// </summary>
        /// <param name="employee">查询参数</param>
        /// <returns>用户实体</returns>
        private IList<Model.ORG.EmployeeDetailInfo> GetUNDistributionEmployee(ParaStruct.EmployeeStruct employee)
        {
            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT EmployeeModel.*, '未分配' as DistributionStatus from Org_Employee EmployeeModel  ");
            sb.AppendLine("  WHERE EmployeeModel.EmployeeID not in (   ");
            sb.AppendLine("  SELECT emp.EmployeeID from Org_Employee emp, ");
            sb.AppendLine("  Org_EmployeeDepartmentRole EDR ");
            sb.AppendLine("  WHERE  emp.EmployeeID = EDR.EmployeeID ");
            // 业务
            if (employee.BusinessValue > 0)
            {
                sb.AppendLine("     AND EDR.BusinessValue = @BusinessValue");
                listPara.Add(new SqlParameter("@BusinessValue", employee.BusinessValue));
            }
            sb.AppendLine("    GROUP BY emp.EmployeeID ) ");
            sb.AppendLine("  AND Active = 1 ");
            // 员工名
            if (employee.Name.Trim() != "")
            {
                sb.AppendLine("AND EmployeeModel.TrueName LIKE @Name");
                listPara.Add(new SqlParameter("@Name", "%" + employee.Name + "%"));
            }
            // 员工手机号
            if (employee.Phone.Trim() != "")
            {
                sb.AppendLine("AND EmployeeModel.Mobile LIKE @Mobile");
                listPara.Add(new SqlParameter("@Mobile", "%" + employee.Phone + "%"));
            }

            DataTable dt = DbHelperSQL.Query(sb.ToString(), listPara.ToArray()).Tables[0];

            if (!DataTableTools.DataTableIsNull(dt))
            {
                return ConvertToList.DataTableToList<Model.ORG.EmployeeDetailInfo>(dt);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取员工组织关系
        /// </summary>
        /// <param name="employeeRole">员工参数结构</param>
        /// <returns>用户实体</returns>
        public IList<Model.ORG.EmployeeDepartmentRole> GetEmployeeRole(Model.ORG.EmployeeDepartmentRole employeeRole)
        {
            StringBuilder sbSql = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();

            // 分页查询
            sbSql.AppendLine("SELECT * FROM ( ");

            // 查询实体
            sbSql.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY EDID ASC) AS Row, ");
            sbSql.AppendLine("       EDR.* ");
            sbSql.AppendLine("  FROM Org_Employee EP,Org_EmployeeDepartmentRole EDR ");
            sbSql.AppendLine(" WHERE EP.EmployeeID = EDR.EmployeeID ");

            // ID
            if (employeeRole.EDID > 0)
            {
                sbSql.AppendLine("AND EDR.EDID = @EDID");
                listPara.Add(new SqlParameter("@EDID", employeeRole.EDID));
            }
            // 员工ID
            if (employeeRole.EmployeeID > 0)
            {
                sbSql.AppendLine("AND EDR.EmployeeID = @EmployeeID");
                listPara.Add(new SqlParameter("@EmployeeID", employeeRole.EmployeeID));
            }

            // 公司
            if (employeeRole.CompanyID>0)
            {
                sbSql.AppendLine("AND EDR.CompanyID = @CompanyID");
                listPara.Add(new SqlParameter("@CompanyID", employeeRole.CompanyID));
            }

            // 部门
            if (employeeRole.DepID > 0)
            {
                sbSql.AppendLine("AND EDR.DepID = @DepID");
                listPara.Add(new SqlParameter("@DepID", employeeRole.DepID));
            }

            // 角色
            if (employeeRole.RoleID > 0)
            {
                sbSql.AppendLine("AND EDR.RoleID = @RoleID");
                listPara.Add(new SqlParameter("@RoleID", employeeRole.RoleID));
            }

            // 业务
            if (employeeRole.BusinessValue > 0)
            {
                sbSql.AppendLine("AND EDR.BusinessValue = @BusinessValue");
                listPara.Add(new SqlParameter("@BusinessValue", employeeRole.BusinessValue));
            }

            // 分页查询
            sbSql.AppendLine(" ) TT ");
            

            DataTable dt = DbHelperSQL.Query(sbSql.ToString(), listPara.ToArray()).Tables[0];

            if (!DataTableTools.DataTableIsNull(dt))
            {
                return ConvertToList.DataTableToList<Model.ORG.EmployeeDepartmentRole>(dt);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool UpdateEmployee(string strSql)
        {
            // 执行SQL
            int result = DbHelperSQL.ExecuteSql(strSql);
            return result > 0;
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool ExeSql(string strSql, List<SqlParameter> listPara)
        {
            // 执行SQL
            int result = DbHelperSQL.ExecuteSql(strSql, listPara.ToArray());
            return result > 0;
        }


        /// <summary>
        /// 通过获取指定公司/部门/角色的员工列表
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <returns>用户实体</returns>
        public IList<Model.ORG.EmployeeDetailInfo> GetEmployee(ParaStruct.EmployeeStruct employee)
        {
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();
            sb.AppendLine("SELECT EmployeeModel.*, EDR.CompanyID, EDR.DepID, EDR.EmployeeID as EDREmployeeID, EDR.RoleID, EDR.BusinessValue, Company.Name as CompanyName, Department.Name as DepName, Role.Name as RoleName, bus.BusinessName, EDR.EDID ");
            sb.AppendLine("  FROM Org_Employee EmployeeModel,  ");
            sb.AppendLine("  Org_EmployeeDepartmentRole EDR ");
            sb.AppendLine("  LEFT JOIN Org_Company Company ON EDR.CompanyID = Company.CompanyID  ");
            sb.AppendLine("  LEFT JOIN Org_Department Department ON EDR.DepID = Department.DepID  ");
            sb.AppendLine("  LEFT JOIN Per_Role Role ON Role.RoleID = EDR.RoleID ");
            sb.AppendLine("  LEFT JOIN (  SELECT BusinessName, BusinessValue FROM dbo.CRM_Business GROUP BY BusinessName,BusinessValue) bus ON bus.BusinessValue = EDR.BusinessValue ");
            sb.AppendLine(" WHERE EmployeeModel.EmployeeID = EDR.EmployeeID ");
            sb.AppendLine("   AND Active = 1 ");

            if (employee.CompanyID > 0)
            {
                sb.AppendLine("   AND EDR.CompanyID = @CompanyID");
                listPara.Add(new SqlParameter("@CompanyID", employee.CompanyID));
            }

            if (employee.DepID == 0 && employee.RoleID <= Convert.ToInt32(RoleValue.总经理))
            {
                sb.AppendLine("   AND EDR.DepID = 0");
            }
            else if (employee.DepID > 0 && employee.RoleID > Convert.ToInt32(RoleValue.总经理))
            {
                sb.AppendLine("   AND EDR.DepID = @DepID");
                listPara.Add(new SqlParameter("@DepID", employee.DepID));
            }

            if (employee.RoleID > 0)
            {
                sb.AppendLine("   AND EDR.RoleID = @RoleID");
                listPara.Add(new SqlParameter("@RoleID", employee.RoleID));
            }

            if (employee.BusinessValue > 0)
            {
                sb.AppendLine("   AND EDR.BusinessValue = @BusinessValue");
                listPara.Add(new SqlParameter("@BusinessValue", employee.BusinessValue));
            }

            DataTable dt = DbHelperSQL.Query(sb.ToString(), listPara.ToArray()).Tables[0];

            if (!DataTableTools.DataTableIsNull(dt))
            {
                return ConvertToList.DataTableToList<Model.ORG.EmployeeDetailInfo>(dt);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取员工详细信息列表(已分配)
        /// </summary>
        /// <param name="employee">员工参数结构</param>
        /// <param name="kbn">查询方式区分</param>
        /// <returns>用户详细信息</returns>
        public IList<Model.ORG.EmployeeDetailInfo> GetEmployeeDetailInfo(ParaStruct.EmployeeStruct employee, int kbn)
        {
            IList<Model.ORG.EmployeeDetailInfo> returnList = new List<Model.ORG.EmployeeDetailInfo>();

            // 通过检索条件中公司/部门筛选登录用户可以查看的员工信息
            IList<Model.ORG.EmployeeTreeInfo> showEmployee = GetShowEmployee(employee, kbn);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT  emp.*, EDR.CompanyID, EDR.DepID, EDR.EmployeeID AS EDREmployeeID, EDR.RoleID, EDR.BusinessValue, Company.Name AS CompanyName, Department.Name AS DepName,");
            sb.AppendLine(" Role.Name AS RoleName, Role.Name AS RoleName, bus.BusinessName, EDR.EDID, '已分配' as DistributionStatus");
            sb.AppendLine("  FROM Org_Employee emp, Org_EmployeeDepartmentRole EDR ");
            sb.AppendLine("  LEFT JOIN Org_Company Company ON EDR.CompanyID = Company.CompanyID   ");
            sb.AppendLine("  LEFT JOIN Org_Department Department ON EDR.DepID = Department.DepID  ");
            sb.AppendLine("  LEFT JOIN (SELECT BusinessValue, BusinessName FROM CRM_Business GROUP BY BusinessValue, BusinessName) bus ON EDR.BusinessValue = bus.BusinessValue  ");
            sb.AppendLine("  LEFT JOIN Per_Role Role ON Role.RoleID = EDR.RoleID");
            sb.AppendLine("  WHERE (emp.StartDate <= @EndDate OR emp.StartDate is null) ");
            sb.AppendLine("    AND (emp.EndDate >= @StartDate OR emp.EndDate is null) ");
            sb.AppendLine("    AND emp.EmployeeID = EDR.EmployeeID ");
            sb.AppendLine("    AND emp.Active = 1 ");
            sb.AppendLine("    AND emp.EmployeeID = @userEmployeeID ");
            sb.AppendLine("    AND EDR.CompanyID = @CompanyID ");
            sb.AppendLine("    AND EDR.DepID = @DepID ");
            if (employee.BusinessValue > 0)
            {
                sb.AppendLine(" AND EDR.BusinessValue = @BusinessValue ");
            }

            List<SqlParameter> listPara = new List<SqlParameter>();

            // 数据库连接打开
            SqlConnection sqlConnection = DbHelperSQL.GetConnection;
            sqlConnection.Open();

            for (int i = 0; i < showEmployee.Count; i++)
            {
                int employeeID = showEmployee[i].EmployeeID;
                if (employeeID == 0)
                {
                    continue;
                }

                int companyID = showEmployee[i].CompanyID;
                int depID = showEmployee[i].DepID;
                // 指定部门中最高权限
                int roleID = showEmployee[i].RoleID;
                StringBuilder strb = new StringBuilder(sb.ToString());

                listPara = new List<SqlParameter>();
                if (employee.StartTime == null)
                {
                    employee.StartTime = Convert.ToDateTime("1753/1/1 00:00:00");
                }
                if (employee.EndTime == null)
                {
                    employee.EndTime = DateTime.MaxValue;
                }
                listPara.Add(new SqlParameter("@EndDate", employee.EndTime));
                listPara.Add(new SqlParameter("@StartDate", employee.StartTime));
                listPara.Add(new SqlParameter("@userEmployeeID", employeeID));
                listPara.Add(new SqlParameter("@CompanyID", companyID));
                listPara.Add(new SqlParameter("@DepID", depID));
                if (!employee.BusinessValue.Equals("0") && employee.BusinessValue != 0)
                {
                    listPara.Add(new SqlParameter("@BusinessValue", employee.BusinessValue));
                }

                if (!string.IsNullOrEmpty(employee.Name))
                {
                    // 员工名
                    if (kbn == (int)SearchKbn.树形)
                    {
                        // 树形场合
                        strb.AppendLine("AND emp.TrueName = @TrueName ");
                        listPara.Add(new SqlParameter("@TrueName", employee.Name));
                    }
                    else
                    {
                        // 表单场合
                        strb.AppendLine("AND emp.TrueName LIKE @TrueName ");
                        listPara.Add(new SqlParameter("@TrueName", "%" + employee.Name + "%"));
                    }
                }

                if (!string.IsNullOrEmpty(employee.Phone))
                {
                    // 手机号
                    strb.AppendLine("AND emp.Mobile LIKE @Mobile ");
                    listPara.Add(new SqlParameter("@Mobile", "%" + employee.Phone + "%"));
                }
                // 角色
                if (employee.RoleID > 0)
                {
                    strb.AppendLine("  AND EDR.RoleID=@RoleID ");
                    listPara.Add(new SqlParameter("@RoleID", employee.RoleID));
                }
                //strb.AppendLine(" ) useInfo ");
                strb.AppendLine(" ORDER BY EDR.CompanyID, EDR.DepID, EDR.EmployeeID ");

                DataTable dt = DbHelperSQL.QueryConn(strb.ToString(), sqlConnection, listPara.ToArray()).Tables[0];

                if (!DataTableTools.DataTableIsNull(dt))
                {
                    IList<Model.ORG.EmployeeDetailInfo> employeeDetailInfoList = ConvertToList.DataTableToList<Model.ORG.EmployeeDetailInfo>(dt);

                    for (int j = 0; j < employeeDetailInfoList.Count; j++)
                    {
                        returnList.Add(employeeDetailInfoList[j]);
                    }
                }
                else
                {
                    continue;
                }
            }

            sqlConnection.Close();

            return returnList;
        }

        /// <summary>
        /// 根据用户ID和业务及检索条件（公司/部门）取得相应权限的员工列表
        /// </summary>
        /// <param name="CompanyID">公司ID（为空时设置为0）</param>
        /// <param name="DepID">部门ID（为空时设置为0）</param>
        /// <param name="EmployeeID">登录者ID</param>
        /// <param name="BusinessValue">业务值</param>
        /// <param name="kbn">区分（0：默认无 1：树形）</param>
        /// <returns></returns>
        public IList<Model.ORG.EmployeeTreeInfo> GetShowEmployee(ParaStruct.EmployeeStruct employee, int kbn = 0)
        {
            IList<Model.ORG.EmployeeTreeInfo> showEmployee = new List<Model.ORG.EmployeeTreeInfo>();
            Organizations.Organization dalOrganization = new Organization();
            int EmployeeID = employee.EmployeeID;
            int BusinessValue = employee.BusinessValue;
            int CompanyID = employee.CompanyID;
            int DepID = employee.DepID;

            // 取得登录用户最大权限(指定业务)
            IList<int> depList = new List<int>();
            IList<Model.ORG.EmployeeDepartmentRole>  employeeDepRoleList = GetEmployeeDepartmentID(false, 0, 0, EmployeeID, BusinessValue);
            int maxUserRoleID = 0;
            if (employeeDepRoleList != null)
            {
                maxUserRoleID = employeeDepRoleList[0].RoleID;
            }
            else
            {
                return showEmployee;
            }

            // 获取登录用户可以查看的所有员工信息
            RunDAL.Organizations.Organization organization = new RunDAL.Organizations.Organization();
            IList< Model.ORG.EmployeeTreeInfo> employeeTree = organization.GetEmployeeTreeByEmployee(EmployeeID, BusinessValue, maxUserRoleID);

            // 输入条件为空时直接返回所有信息
            if (CompanyID == 0)
            {
                for (int i = 0; i < employeeTree.Count; i++)
                {
                    if (employeeTree[i].EmployeeID != 0)
                    {
                        showEmployee.Add(employeeTree[i]);
                    }
                }

                return showEmployee;
            }

            // 
            if (kbn == 0 && maxUserRoleID <= Convert.ToInt32(RoleValue.总经理))
            {
                DepID = 0;
            }

            // 根据权限取得权限部门列表
            ParaStruct.DepartStruct departStruct = new ParaStruct.DepartStruct();
            for (int i = 0; i < employeeDepRoleList.Count; i++)
            {
                int tempCompanyID = employeeDepRoleList[i].CompanyID;
                int tempDepID = employeeDepRoleList[i].DepID;
                // 部门为0场合，指定公司的部门信息
                departStruct.CompanyID = tempCompanyID;
                if (tempDepID == 0)
                {
                    IList<Model.ORG.DepartmentDetail>  depInfoList = organization.GetDepartment(departStruct);
                    if (depInfoList == null)
                    {
                        continue;
                    }
                    for (int j = 0; j < depInfoList.Count; j++)
                    {
                        depList.Add(depInfoList[j].DepID);
                    }
                }
                else
                {
                    if (employeeDepRoleList[i].RoleID <= Convert.ToInt32(RoleValue.经理))
                    {
                        // 查询下级部门
                        IList<Model.ORG.DepInfo> depInfoList = dalOrganization.GetAllDepList(CompanyID, tempDepID);
                        if (depInfoList == null)
                        {
                            depList.Add(tempDepID);
                            continue;
                        }

                        for (int j = 0; j < depInfoList.Count; j++)
                        {
                            depList.Add(depInfoList[j].DepID);
                        }
                    }
                    else
                    {
                        depList.Add(tempDepID);
                    }
                }
            }

            IList<int> showDepList = new List<int>();
            if (depList.Contains(DepID))
            {
                // 查询下级部门
                IList<Model.ORG.DepInfo> depInfoList = dalOrganization.GetAllDepList(CompanyID, DepID);
                if (depInfoList == null)
                {
                    showDepList.Add(DepID);
                }
                else
                {
                    for (int j = 0; j < depInfoList.Count; j++)
                    {
                        showDepList.Add(depInfoList[j].DepID);
                    }
                }
            }
            else
            {
                showDepList.Add(DepID);
            }

            // 通过检索条件中公司/部门筛选员工信息
            bool addFlg = false;

            string tempPerID = "";
            for (int i = 0; i < employeeTree.Count; i++)
            {
                if (employeeTree[i].EmployeeID == 0)
                {
                    continue;
                }

                addFlg = false;
                // 如果出现(同级部门节点/或上级节点)则不再追加
                if (tempPerID.Equals(employeeTree[i].PreID) || (employeeTree[i].PreID).Length < tempPerID.Length)
                {
                    addFlg = false;
                }
                // 公司ID跟列表中ID相同，部门为空或指定部门相等或指定部门在自己的权限内
                if (CompanyID == employeeTree[i].CompanyID && (DepID == 0 || DepID == employeeTree[i].DepID || showDepList.Contains(employeeTree[i].DepID)) && addFlg == false)
                {
                    addFlg = true;
                    tempPerID = employeeTree[i].PreID;
                }

                if (addFlg && employeeTree[i].EmployeeID != 0)
                {
                    showEmployee.Add(employeeTree[i]);
                }
            }
            return showEmployee;
        }

        /// <summary>
        /// 通过公司/部门等获取员工所属ID
        /// </summary>
        /// <param name="PreciseFlg">是否精确查询(true: 精确查询 公司ID/部门ID为必须输入)</param>
        /// <param name="CompanyID">公司</param>
        /// <param name="DepID">部门</param>
        /// <param name="EmployeeID">员工</param>
        /// <param name="BusinessID">业务</param>
        /// <param name="RoleID">角色</param>
        /// <returns></returns>
        public IList<Model.ORG.EmployeeDepartmentRole> GetEmployeeDepartmentID(bool PreciseFlg, int CompanyID = 0, int DepID = 0, int EmployeeID = 0, int BusinessID = 0, int RoleID = 0)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();
            strSql.AppendLine("SELECT EDR.*, OE.LoginName, OE.TrueName  FROM Org_EmployeeDepartmentRole EDR, Org_Employee OE ");
            strSql.AppendLine(" WHERE EDR.EmployeeID = OE.EmployeeID ");
            // 公司ID
            if (PreciseFlg || (PreciseFlg == false && CompanyID > 0))
            {
                strSql.AppendLine(" AND EDR.CompanyID = @CompanyID ");
                listPara.Add(new SqlParameter("@CompanyID", CompanyID));
            }
            // 部门
            if (PreciseFlg || (PreciseFlg == false && DepID > 0))
            {
                strSql.AppendLine(" AND EDR.DepID = @DepID ");
                listPara.Add(new SqlParameter("@DepID", DepID));
            }

            // 员工
            if (EmployeeID > 0)
            {
                strSql.AppendLine("   AND EDR.EmployeeID = @EmployeeID ");
                listPara.Add(new SqlParameter("@EmployeeID", EmployeeID));
            }

            // 角色
            if (RoleID > 0)
            {
                strSql.AppendLine("   AND EDR.RoleID = @RoleID ");
                listPara.Add(new SqlParameter("@RoleID", RoleID));
            }

            // 业务
            if (BusinessID > 0)
            {
                if (GetSystemEmployeeID().Contains(EmployeeID))
                {
                    strSql.AppendLine("   AND EDR.BusinessValue = 0 ");
                }
                else
                {
                    strSql.AppendLine("   AND EDR.BusinessValue = @BusinessValue ");
                    listPara.Add(new SqlParameter("@BusinessValue", BusinessID));
                }
            }

            strSql.AppendLine(" ORDER BY EDR.RoleID, EDR.CompanyID, EDR.DepID, EDR.EmployeeID ");

            DataTable dt = DbHelperSQL.Query(strSql.ToString(), listPara.ToArray()).Tables[0];
            if (!DataTableTools.DataTableIsNull(dt))
            {
                IList<Model.ORG.EmployeeDepartmentRole> employeeDepInfoList = ConvertToList.DataTableToList<Model.ORG.EmployeeDepartmentRole>(dt);

                return employeeDepInfoList;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 取得超管及总裁的员工ID
        /// </summary>
        /// <returns></returns>
        private IList<int> GetSystemEmployeeID()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("SELECT EDR.EmployeeID FROM Org_EmployeeDepartmentRole EDR WHERE RoleID IN (1, 3)");
            DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];
            return ConvertToList.DataTableToIntList(dt);
        }

        #endregion  获取员工列表

        #region  添加员工信息
        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="model">员工详细实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool AddEmployee(Model.ORG.EmployeeDetailInfo model)
        {
            int result = 0;
            string salt = HashEncode.GetRandomString64();
            string encryptedPwd = DEncrypt.CalculatePassword(model.Password, salt);
            IDataParameter[] para = {
              new SqlParameter("@JobID",SqlDbType.Int,4),
              new SqlParameter("@LoginName",SqlDbType.VarChar,50),
              new SqlParameter("@Password",SqlDbType.VarChar,256),
              new SqlParameter("@NickName",SqlDbType.VarChar,50),
              new SqlParameter("@AvatarPicName",SqlDbType.VarChar,256),
              new SqlParameter("@TrueName",SqlDbType.VarChar,50),
              new SqlParameter("@Sex",SqlDbType.Int,4),
              new SqlParameter("@Birthday",SqlDbType.DateTime),
              new SqlParameter("@Address",SqlDbType.VarChar,200),
              new SqlParameter("@Mobile",SqlDbType.Char,11),
              new SqlParameter("@IDCard",SqlDbType.VarChar,18),
              new SqlParameter("@StartDate",SqlDbType.DateTime),
              new SqlParameter("@EndDate",SqlDbType.DateTime),
              new SqlParameter("@Email",SqlDbType.VarChar,128),
              new SqlParameter("@QQ",SqlDbType.VarChar,20),
              new SqlParameter("@WeChat",SqlDbType.VarChar,64),
              new SqlParameter("@LastLoginTime",SqlDbType.DateTime),
              new SqlParameter("@LastLoginIP",SqlDbType.Int,4),
              new SqlParameter("@Active",SqlDbType.Bit),
              new SqlParameter("@EmployeeCode",SqlDbType.VarChar,50),
              new SqlParameter("@PwdSalt",SqlDbType.VarChar,64),
              new SqlParameter("@InvitationCode",SqlDbType.VarChar,50),
              new SqlParameter("@MaritalStatus",SqlDbType.Bit),
              new SqlParameter("@Education",SqlDbType.VarChar,50),
              new SqlParameter("@CompanyID",SqlDbType.Int),
              new SqlParameter("@DepID",SqlDbType.Int),
              new SqlParameter("@RoleID",SqlDbType.Int),
              new SqlParameter("@BusinessValue",SqlDbType.Int),
              new SqlParameter("@BusinessValues",SqlDbType.VarChar,100),
              new SqlParameter("@AddCompanyID",SqlDbType.Int),
              new SqlParameter("@AddDepID",SqlDbType.Int),
              new SqlParameter("@AddRoleID",SqlDbType.Int)
            };

            int i = 0;
            para[i++].Value = model.JobID;
            para[i++].Value = model.LoginName;
            para[i++].Value = encryptedPwd;
            para[i++].Value = model.NickName;
            para[i++].Value = model.AvatarPicName;
            para[i++].Value = model.TrueName;
            para[i++].Value = model.Sex;
            para[i++].Value = model.Birthday;
            para[i++].Value = model.Address;
            para[i++].Value = model.Mobile;
            para[i++].Value = model.IDCard;
            para[i++].Value = model.StartDate;
            para[i++].Value = model.EndDate;
            para[i++].Value = model.Email;
            para[i++].Value = model.QQ;
            para[i++].Value = model.WeChat;
            para[i++].Value = model.LastLoginTime ?? DateTime.Now;
            para[i++].Value = model.LastLoginIP;
            para[i++].Value = model.Active;
            para[i++].Value = model.EmployeeCode;
            para[i++].Value = salt;
            para[i++].Value = model.InvitationCode;
            para[i++].Value = model.MaritalStatus;
            para[i++].Value = model.Education;
            para[i++].Value = model.CompanyID;
            para[i++].Value = model.DepID;
            para[i++].Value = model.RoleID;
            para[i++].Value = model.AddBusinessValue;
            para[i++].Value = model.SBusinessValues;
            para[i++].Value = model.AddCompanyID;
            para[i++].Value = model.AddDepID;
            para[i++].Value = model.AddRoleID;

            int count = DbHelperSQL.RunProcedure("sp_AddOrgEmployee", para, out result);
            return count > 0;
        }
        #endregion  添加员工信息

        #region  更新员工信息
        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <param name="companyID">公司ID</param>
        /// <param name="depID">部门ID</param>
        /// <param name="roleID">角色ID</param>
        /// <param name="EDID">员工所属部门角色ID</param>
        /// <returns>true:成功，false:失败</returns>
        public bool UpdateEmployee(Model.ORG.EmployeeDetailInfo model)
        {
            int result = 0;

            IDataParameter[] para = {
              new SqlParameter("@JobID",SqlDbType.Int,4),
              new SqlParameter("@LoginName",SqlDbType.VarChar,50),
              new SqlParameter("@NickName",SqlDbType.VarChar,50),
              new SqlParameter("@AvatarPicName",SqlDbType.VarChar,256),
              new SqlParameter("@TrueName",SqlDbType.VarChar,50),
              new SqlParameter("@Sex",SqlDbType.Int,4),
              new SqlParameter("@Birthday",SqlDbType.DateTime),
              new SqlParameter("@Address",SqlDbType.VarChar,200),
              new SqlParameter("@Mobile",SqlDbType.Char,11),
              new SqlParameter("@IDCard",SqlDbType.VarChar,18),
              new SqlParameter("@StartDate",SqlDbType.DateTime),
              new SqlParameter("@EndDate",SqlDbType.DateTime),
              new SqlParameter("@Email",SqlDbType.VarChar,128),
              new SqlParameter("@QQ",SqlDbType.VarChar,20),
              new SqlParameter("@WeChat",SqlDbType.VarChar,64),
              new SqlParameter("@LastLoginTime",SqlDbType.DateTime),
              new SqlParameter("@LastLoginIP",SqlDbType.Int,4),
              new SqlParameter("@Active",SqlDbType.Bit),
              new SqlParameter("@EmployeeCode",SqlDbType.VarChar,50),
              new SqlParameter("@InvitationCode",SqlDbType.VarChar,50),
              new SqlParameter("@MaritalStatus",SqlDbType.Bit),
              new SqlParameter("@Education",SqlDbType.VarChar,50),
              new SqlParameter("@CompanyID",SqlDbType.Int),
              new SqlParameter("@DepID",SqlDbType.Int),
              new SqlParameter("@RoleID",SqlDbType.Int),
              new SqlParameter("@EmployeeID",SqlDbType.Int),
              new SqlParameter("@EDID",SqlDbType.Int),   // 员工所属部门ID
              new SqlParameter("@BusinessValue",SqlDbType.Int),
              new SqlParameter("@BusinessValues",SqlDbType.VarChar,100),
              // 操作者公司和部门ID
              new SqlParameter("@OperateCompanyID",SqlDbType.Int),
              new SqlParameter("@OperateDepID",SqlDbType.Int),
            };

            int i = 0;
            para[i++].Value = model.JobID;
            para[i++].Value = model.LoginName;
            para[i++].Value = model.NickName;
            para[i++].Value = model.AvatarPicName;
            para[i++].Value = model.TrueName;
            para[i++].Value = model.Sex;
            if (model.Birthday != DateTime.MinValue)
            {
                para[i++].Value = model.Birthday;
            }
            else
            {
                para[i++].Value = null;
            }
            para[i++].Value = model.Address;
            para[i++].Value = model.Mobile;
            para[i++].Value = model.IDCard;
            para[i++].Value = model.StartDate ?? DateTime.Now;
            para[i++].Value = model.EndDate ?? DateTime.Now;
            para[i++].Value = model.Email;
            para[i++].Value = model.QQ;
            para[i++].Value = model.WeChat;
            para[i++].Value = model.LastLoginTime ?? DateTime.Now;
            para[i++].Value = model.LastLoginIP;
            para[i++].Value = model.Active;
            para[i++].Value = model.EmployeeCode;
            para[i++].Value = model.InvitationCode;
            para[i++].Value = model.MaritalStatus;
            para[i++].Value = model.Education;
            para[i++].Value = model.CompanyID;
            para[i++].Value = model.DepID;
            para[i++].Value = model.RoleID;
            para[i++].Value = model.EmployeeID;
            para[i++].Value = model.EDID;
            para[i++].Value = model.BusinessValue;
            para[i++].Value = model.SBusinessValues;
            // 操作者公司和部门ID
            para[i++].Value = model.OperateCompanyID;
            para[i++].Value = model.OperateDepID;

            int count = DbHelperSQL.RunProcedure("sp_BackUpdOrgEmployee", para, out result);

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 重置员工密码
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public bool ResetEmpPassword(int empId)
        {
            string salt = HashEncode.GetRandomString64();
            string pwd = "123456";
            string encryptedPwd = DEncrypt.CalculatePassword(pwd, salt);
            string strSql = "UPDATE Org_Employee SET Password=@pwd,PwdSalt=@pwdSalt  WHERE EmployeeID=@empId";
            SqlParameter[] para ={
                                     new SqlParameter("@pwd",SqlDbType.VarChar,256),
                                     new SqlParameter("@pwdSalt",SqlDbType.VarChar,64),
                                     new SqlParameter("@empId",SqlDbType.Int)
                                    };
            para[0].Value = encryptedPwd;
            para[1].Value = salt;
            para[2].Value = empId;
            int result = DbHelperSQL.ExecuteSql(strSql, para);
            return result > 0;
        }

        /// <summary>
        /// 修改员工密码
        /// </summary>
        /// <param name="model">员工实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool UpdateEmployeePwd(Model.ORG.EmployeeModel model)
        {
            string strSql = "update Org_Employee set Password = @Password where EmployeeID = @EmployeeID";

            SqlParameter[] para =
            {
                 new SqlParameter("@Password",SqlDbType.VarChar,256),
                 new SqlParameter("@EmployeeID",SqlDbType.Int)
            };
            para[0].Value = DEncrypt.CalculatePassword(model.Password, model.PwdSalt);
            para[1].Value = model.EmployeeID;

            int result = DbHelperSQL.ExecuteSql(strSql, para);

            return result > 0;
        }

        /// <summary>
        /// 修改个人中心
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updatePersonal(Model.ORG.LoginEmployee model)
        {
            int result = 0;

            IDataParameter[] para = {
                      new SqlParameter("@EmployeeID",SqlDbType.Int),
                      new SqlParameter("@LoginName",SqlDbType.VarChar,50),
                      new SqlParameter("@NickName",SqlDbType.VarChar,50),
                      new SqlParameter("@AvatarPicName",SqlDbType.VarChar,256),
                      new SqlParameter("@TrueName",SqlDbType.VarChar,50),
                      new SqlParameter("@Sex",SqlDbType.Int,4),
                      new SqlParameter("@Birthday",SqlDbType.DateTime),
                      new SqlParameter("@Address",SqlDbType.VarChar,200),
                      new SqlParameter("@Mobile",SqlDbType.Char,11),
                      new SqlParameter("@IDCard",SqlDbType.VarChar,18),
                      new SqlParameter("@StartDate",SqlDbType.DateTime),
                      new SqlParameter("@EndDate",SqlDbType.DateTime),
                      new SqlParameter("@Email",SqlDbType.VarChar,128),
                      new SqlParameter("@QQ",SqlDbType.VarChar,20),
                      new SqlParameter("@WeChat",SqlDbType.VarChar,64),
                      new SqlParameter("@EmployeeCode",SqlDbType.VarChar,50),
                      new SqlParameter("@InvitationCode",SqlDbType.VarChar,50),
                      new SqlParameter("@MaritalStatus",SqlDbType.Bit),
                      new SqlParameter("@Education",SqlDbType.VarChar,50)
                // 员工所属部门ID
            };

            int i = 0;
            para[i++].Value = model.EmployeeID;
            para[i++].Value = model.LoginName;
            para[i++].Value = model.NickName;
            para[i++].Value = model.AvatarPicName;
            para[i++].Value = model.TrueName;
            para[i++].Value = model.Sex;
            if (model.Birthday != DateTime.MinValue)
            {
                para[i++].Value = model.Birthday;
            }
            else
            {
                para[i++].Value = null;
            }
            para[i++].Value = model.Address;
            para[i++].Value = model.Mobile;
            para[i++].Value = model.IDCard;
            para[i++].Value = model.StartDate ?? DateTime.Now;
            para[i++].Value = model.EndDate ?? DateTime.Now;
            para[i++].Value = model.Email;
            para[i++].Value = model.QQ;
            para[i++].Value = model.WeChat;
            para[i++].Value = model.EmployeeCode;
            para[i++].Value = model.InvitationCode;
            para[i++].Value = model.MaritalStatus;
            para[i++].Value = model.Education;

            int count = DbHelperSQL.RunProcedure("updateEmployee", para, out result);

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion  更新员工信息

        #region  删除员工信息
        /// <summary>
        /// 删除员工信息
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <returns>true:成功，false:失败</returns>
        public bool DeleteEmployee(int employeeID)
        {
            int result = 0;

            IDataParameter[] para = {
              new SqlParameter("@EmployeeID",SqlDbType.Int)
            };

            int i = 0;
            para[i++].Value = employeeID;

            int count = DbHelperSQL.RunProcedure("sp_DelOrgEmployee", para, out result);

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion  删除员工信息


        #region  检查员工信息
        /// <summary>
        /// 检查登录名是否存在
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>true:存在，false:不存在</returns>
        public bool IsExistLoginName(string loginName, int empId)
        {
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();
            sb.AppendLine("select count(0) from Org_Employee where LoginName=@LoginName");
            listPara.Add(new SqlParameter("@LoginName", loginName));
            if (empId > 0)
            {
                sb.AppendLine(" and EmployeeID <> @EmployeeID ");
                listPara.Add(new SqlParameter("@EmployeeID", empId));
            }

            return DbHelperSQL.Exists(sb.ToString(), listPara.ToArray());
        }

        /// <summary>
        /// 检查手机号是否存在
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>true:存在，false:不存在</returns>
        public bool IsExistMobile(string mobile)
        {
            string strSql = "select count(0) from Org_Employee where Mobile=@Mobile";

            SqlParameter[] para = { new SqlParameter("@Mobile", SqlDbType.Char, 11) };

            para[0].Value = mobile;

            return DbHelperSQL.Exists(strSql, para);
        }
        #endregion  检查员工信息

        #region  员工所属部门角色业务编辑
        /// <summary>
        /// 员工所属部门角色追加
        /// </summary>
        /// <param name="model">员工所属部门角色实体</param>
        /// <returns>追加的主键值</returns>
        public int AddCompanyIDDepInfo(Model.ORG.EmployeeDepartmentRole model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO Org_EmployeeDepartmentRole (EmployeeID, CompanyID, DepID, RoleID, BusinessValue, AddTime, UpdateTime) ");
            strSql.Append(" values(@EmployeeID, @CompanyID, @DepID, @RoleID, @BusinessValue, @AddTime, @UpdateTime) ");

            SqlParameter[] para = {
              new SqlParameter("@EmployeeID",SqlDbType.Int),
              new SqlParameter("@CompanyID",SqlDbType.Int),
              new SqlParameter("@DepID",SqlDbType.Int),
              new SqlParameter("@RoleID",SqlDbType.Int),
              new SqlParameter("@BusinessValue",SqlDbType.Int),
              new SqlParameter("@AddTime",SqlDbType.DateTime),
              new SqlParameter("@UpdateTime",SqlDbType.DateTime)
            };

            int i = 0;
            para[i++].Value = model.EmployeeID;
            para[i++].Value = model.CompanyID;
            para[i++].Value = model.DepID;
            para[i++].Value = model.RoleID;
            para[i++].Value = model.BusinessValue;
            para[i++].Value = model.AddTime ?? DateTime.Now;
            para[i++].Value = model.UpdateTime ?? DateTime.Now;

            int Result = DbHelperSQL.ExecuteSql(strSql.ToString(), para);

            if (Result > 0)
            {
                // 返回新追加ID
                string sqlartId = "select IDENT_CURRENT('Org_EmployeeDepartmentRole')";
                object artobject = DbHelperSQL.GetSingle(sqlartId);
                if (artobject == null)
                {
                    return 0;
                }
                return Convert.ToInt32(artobject);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 员工所属部门角色删除
        /// </summary>
        /// <param name="model">员工所属部门角色实体</param>
        /// <returns>true:成功，false:失败</returns>
        public bool DeleteCompanyIDDepInfo(Model.ORG.EmployeeDepartmentRole model)
        {
            StringBuilder strSql = new StringBuilder();
            int Result = 0;
            strSql.AppendLine("DELETE FROM Org_EmployeeDepartmentRole ");
            if (model.EDID > 0 && model.EmployeeID > 0)
            {
                // 员工ID存在的时候
                strSql.AppendLine(" WHERE EmployeeID = @EmployeeID ");

                SqlParameter[] para = {
                  new SqlParameter("@EmployeeID",SqlDbType.Int)
                };
                para[0].Value = model.EmployeeID;

                Result = DbHelperSQL.ExecuteSql(strSql.ToString(), para);
            }
            else if (model.EDID > 0 && model.EmployeeID == 0)
            {
                // 主键存在场合，按照主键进行删除
                strSql.AppendLine(" WHERE EDID = @EDID ");

                SqlParameter[] para = {
                  new SqlParameter("@EDID",SqlDbType.Int)
                };
                para[0].Value = model.EDID;

                Result = DbHelperSQL.ExecuteSql(strSql.ToString(), para);
            }
            else
            {
                // 主键不存在场合，按照员工详细信息进行删除
                strSql.AppendLine(" WHERE EmployeeID = @EmployeeID ");
                strSql.AppendLine("   AND CompanyID = @CompanyID ");
                strSql.AppendLine("   AND DepID = @DepID ");
                strSql.AppendLine("   AND RoleID = @RoleID ");
                strSql.AppendLine("   AND BusinessValue = @BusinessValue ");

                SqlParameter[] para = {
                  new SqlParameter("@EmployeeID",SqlDbType.Int),
                  new SqlParameter("@CompanyID",SqlDbType.Int),
                  new SqlParameter("@DepID",SqlDbType.Int),
                  new SqlParameter("@RoleID",SqlDbType.Int),
                  new SqlParameter("@BusinessValue",SqlDbType.Int)
                };

                int i = 0;
                para[i++].Value = model.EmployeeID;
                para[i++].Value = model.CompanyID;
                para[i++].Value = model.DepID;
                para[i++].Value = model.RoleID;
                para[i++].Value = model.BusinessValue;

                Result = DbHelperSQL.ExecuteSql(strSql.ToString(), para);
            }

            if (Result > 0)
            {
                // 变更后归属
                Model.ORG.EmployeeDepartmentRole afterModel = new Model.ORG.EmployeeDepartmentRole();
                afterModel.EmployeeID = model.EmployeeID;
                afterModel.CompanyID = model.CompanyID;
                afterModel.DepID = 0;

                // 变更客户归属
                UpdateCustomerByEmp(model, afterModel);
                // 变更订单归属(业务)
                UpdateEmpOrderInfo(model, afterModel, 1);
                // 变更订单归属(客服)
                UpdateEmpOrderInfo(model, afterModel, 2);
                // 变更订单归属(升级员工)
                UpdateEmpOrderInfo(model, afterModel, 3);

                return Result > 0 ? true: false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 变更客户归属
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCustomerByEmp(Model.ORG.EmployeeDepartmentRole beforeModel, Model.ORG.EmployeeDepartmentRole afterModel)
        { 
            // 更新客户归属
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();

            strSql.AppendLine("UPDATE Fafa_CustomerOwnershipRecord ");
            strSql.AppendLine("SET EmployeeID = @afEmployeeID, CompanyID = @afCompanyID, DepID = @afDepID ");
            strSql.AppendLine("WHERE EmployeeID = @beEmployeeID ");
            strSql.AppendLine("  AND BusinessID = @beBusinessValue");
            strSql.AppendLine("  AND CompanyID = @beCompanyID ");
            strSql.AppendLine("  AND DepID = @beDepID ");

            listPara.Add(new SqlParameter("@afEmployeeID", afterModel.EmployeeID));
            listPara.Add(new SqlParameter("@afCompanyID", afterModel.CompanyID));
            listPara.Add(new SqlParameter("@afDepID", afterModel.DepID));
            listPara.Add(new SqlParameter("@beEmployeeID", beforeModel.EmployeeID));
            listPara.Add(new SqlParameter("@beBusinessValue", beforeModel.BusinessValue));
            listPara.Add(new SqlParameter("@beCompanyID", beforeModel.CompanyID));
            listPara.Add(new SqlParameter("@beDepID", beforeModel.DepID));

            return DbHelperSQL.ExecuteSql(strSql.ToString(), listPara.ToArray());
        }

        /// <summary>
        /// 变更订单归属
        /// </summary>
        /// <param name="beforeModel"></param>
        /// <param name="afterModel"></param>
        /// <param name="kbn">1:业务员工 2：客服 3：升级</param>
        /// <returns></returns>
        public int UpdateEmpOrderInfo(Model.ORG.EmployeeDepartmentRole beforeModel, Model.ORG.EmployeeDepartmentRole afterModel, int kbn)
        {
            // 更新种类
            string EmpName = "";
            string compName = "";
            string DepName = "";
            switch (kbn)
            {
                case 1:
                    EmpName = "BusinessEmpID";
                    compName = "BusinessCompanyID";
                    DepName = "BusinessDepID";
                    break;
                case 2:
                    EmpName = "ServiceEmpID";
                    compName = "ServiceCompanyID";
                    DepName = "ServiceDepID";
                    break;
                case 3:
                    EmpName = "UPEmployeeID";
                    compName = "UPCompanyID";
                    DepName = "UPDepID";
                    break;
            }
                
            // 更新订单归属
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();

            strSql.AppendLine("UPDATE SYS_Order ");
            strSql.AppendFormat("SET {0} = @afEmployeeID, {1} = @afCompanyID, {2} = @afDepID ", EmpName, compName, DepName);
            strSql.AppendFormat("WHERE {0} = @beEmployeeID ", EmpName);
            strSql.AppendFormat("  AND {0} = @beCompanyID ", compName);
            strSql.AppendFormat("  AND {0} = @beDepID ", DepName);
            strSql.AppendLine("  AND BusinessValue = @beBusinessValue");

            listPara.Add(new SqlParameter("@afEmployeeID", afterModel.EmployeeID));
            listPara.Add(new SqlParameter("@afCompanyID", afterModel.CompanyID));
            listPara.Add(new SqlParameter("@afDepID", afterModel.DepID));
            listPara.Add(new SqlParameter("@beEmployeeID", beforeModel.EmployeeID));
            listPara.Add(new SqlParameter("@beBusinessValue", beforeModel.BusinessValue));
            listPara.Add(new SqlParameter("@beCompanyID", beforeModel.CompanyID));
            listPara.Add(new SqlParameter("@beDepID", beforeModel.DepID));

            return DbHelperSQL.ExecuteSql(strSql.ToString(), listPara.ToArray());
        }

        /// <summary>
        /// 更新员工所属角色,部门，业务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateComDepRoleInfo(Model.ORG.EmployeeDepartmentRole model)
        {
            // 获取更新前所属角色部门业务
            Model.ORG.EmployeeDepartmentRole employeeRole = new Model.ORG.EmployeeDepartmentRole();
            Model.ORG.EmployeeDepartmentRole beforeModel = new Model.ORG.EmployeeDepartmentRole();
            employeeRole.EDID = model.EDID;
            IList<Model.ORG.EmployeeDepartmentRole> roleList = GetEmployeeRole(employeeRole);
            if (roleList.Count > 0)
            {
                beforeModel = roleList[0];
            }

            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("UPDATE Org_EmployeeDepartmentRole ");
            strSql.AppendLine(" SET RoleID = @RoleID, BusinessValue = @BusinessValue,CompanyID = @CompanyID , DepID = @DepID ");
            strSql.AppendLine(" WHERE EDID=@EDID  ");

            SqlParameter[] para = {
              new SqlParameter("@RoleID",SqlDbType.Int),
              new SqlParameter("@BusinessValue",SqlDbType.Int),
              new SqlParameter("@CompanyID",SqlDbType.Int),
              new SqlParameter("@DepID",SqlDbType.Int),
              new SqlParameter("@EDID",SqlDbType.Int)
            };

            int i = 0;
            para[i++].Value = model.RoleID;
            para[i++].Value = model.BusinessValue;
            para[i++].Value = model.CompanyID;
            para[i++].Value = model.DepID;
            para[i++].Value = model.EDID;
            int Result = DbHelperSQL.ExecuteSql(strSql.ToString(), para);

            // 变更客户归属
            UpdateCustomerByEmp(beforeModel, model);
            // 变更订单归属(业务)
            UpdateEmpOrderInfo(beforeModel, model, 1);
            // 变更订单归属(客服)
            UpdateEmpOrderInfo(beforeModel, model, 2);
            // 变更订单归属(升级员工)
            UpdateEmpOrderInfo(beforeModel, model, 3);

            return Result;
        }

        #endregion  员工所属部门角色业务编辑
        
    }
}
