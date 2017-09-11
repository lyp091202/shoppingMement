using System;
using System.Collections.Generic;
using System.Data;
using RunTecMs.Common;
using RunTecMs.Common.ConvertUtility;
using RunTecMs.Common.DBUtility;
using RunTecMs.RunIDAL.Organizations;
using System.Text;
using System.Data.SqlClient;
using RunTecMs.Model.Parameter;
using RunTecMs.Model.EnumType;

namespace RunTecMs.RunDAL.Organizations
{
    /// <summary>
    /// 组织结构
    /// </summary>
    public class Organization : IOrganization
    {
        IList<Model.ORG.DepTreeInfo> treeList = new List<Model.ORG.DepTreeInfo>();

        #region  组织结构操作
        /// <summary>
        ///  获取公司-部门树形信息
        /// </summary>
        /// <param name="companyID">公司ID（可以为空）</param>
        /// <param name="depID">部门ID（可以为空）</param>
        /// <returns></returns>
        public IList<Model.ORG.DepTreeInfo> GetDepartmentInfo(int companyID = 0, int depID = 0)
        {
            StringBuilder strBSql = new StringBuilder();

            IList<Model.ORG.DepInfo> depList = new List<Model.ORG.DepInfo>();
            IList<Model.ORG.DepTreeInfo> DepTreeList = new List<Model.ORG.DepTreeInfo>();

            DataTable dataTable = new DataTable();
            Model.ORG.DepTreeInfo depTreeInfo = new Model.ORG.DepTreeInfo();

            // 数据库连接打开
            SqlConnection sqlConnection = DbHelperSQL.GetConnection;
            sqlConnection.Open();

            // 获取公司及直属部门信息
            IList<Model.ORG.CompanyDepInfo> companyDepList = GetCompanyDepInfo(companyID, depID);
            if (companyDepList == null)
            {
                return null;
            }

            int companyIdTemp = 0;
            for (int i = 0; i < companyDepList.Count; i++)
            {
                // 公司ID
                int companyId = companyDepList[i].CompanyID;
                // 公司代码
                string companyCode = companyDepList[i].CompanyCode;
                // 公司名
                string companyName = companyDepList[i].CompanyName;
                // 直属部门ID
                int baseDepId = 0;
                if (depID > 0)
                {
                    baseDepId = depID;
                }
                else
                {
                    baseDepId = companyDepList[i].DepID;
                }

                // 树形结构追加
                if (companyIdTemp != companyId) 
                {
                    depTreeInfo = new Model.ORG.DepTreeInfo();
                    depTreeInfo.ID = companyId.ToString();
                    depTreeInfo.PreID = "";
                    depTreeInfo.CompanyID = companyId;
                    depTreeInfo.CompanyCode = companyCode;
                    depTreeInfo.CompanyName = companyName;
                    DepTreeList.Add(depTreeInfo);
                    companyIdTemp = companyId;
                }

                // 取得公司部门下所有部门信息
                IDataParameter[] para = { new SqlParameter("@companyId", SqlDbType.Int),
                                          new SqlParameter("@DepId", SqlDbType.Int)};
                para[0].Value = companyId;
                para[1].Value = baseDepId;
                dataTable = DbHelperSQL.RunProcedureConn("sp_GetPerDepartment", para, "Department", sqlConnection).Tables[0];
                depList = ConvertToList.DataTableToList<Model.ORG.DepInfo>(dataTable);
                if (depList == null)
                {
                    continue;
                }

                for (int j = 0; j < depList.Count; j++)
                {
                    // 部门ID
                    int DepId = depList[j].DepID;
                    // 亲部门ID
                    int ParentDepId = depList[j].ParentDepID;
                    // 部门名
                    string DepName = depList[j].Name;

                    // 树形结构追加
                    depTreeInfo = new Model.ORG.DepTreeInfo();
                    depTreeInfo.ID = companyId.ToString() + "_" + DepId.ToString();
                    if (ParentDepId == 0)
                    {
                        depTreeInfo.PreID = companyId.ToString();
                    }
                    else
                    {
                        depTreeInfo.PreID = companyId.ToString() + "_" + ParentDepId.ToString();
                    }
                    depTreeInfo.CompanyID = companyId;
                    depTreeInfo.CompanyCode = companyCode;
                    depTreeInfo.CompanyName = companyName;
                    depTreeInfo.DepID = DepId;
                    depTreeInfo.DepName = DepName;

                    DepTreeList.Add(depTreeInfo);
                }
            }

            sqlConnection.Close();
            return DepTreeList;
        }

        /// <summary>
        ///  获取公司部门信息
        ///  不指定公司 and 不指定部门场合：查询所有公司及直属部门
        ///    指定公司 and 不指定部门场合：查询指定公司的直属部门
        ///    指定公司 and 指定部门场合：查询指定公司的指定部门信息
        /// </summary>
        /// <param name="companyID">公司ID(可以为空)</param>
        /// <param name="depID">部门ID(可以为空)</param>
        /// <returns>部门信息</returns>
        private IList<Model.ORG.CompanyDepInfo> GetCompanyDepInfo(int companyID = 0, int depID = 0)
        {
            DataTable dt = new DataTable();
            StringBuilder strBSql = new StringBuilder();
            List<SqlParameter> paraList = new List<SqlParameter>();

            strBSql.AppendLine("SELECT Company.CompanyID, Company.CompanyCode, Company.Name as CompanyName, Department.DepID, Department.Name DepName ");
            strBSql.AppendLine("  FROM Org_Company Company, Org_Department Department ");
            strBSql.AppendLine(" WHERE Company.CompanyId = Department.CompanyId ");
            strBSql.AppendLine("   AND ISNULL(Company.IsDel, 0) = 0 ");
            strBSql.AppendLine("   AND ISNULL(Department.IsDel, 0) = 0 ");

            if (depID == 0)
            {
                // 不指定部门场合，查询顶级部门
                strBSql.AppendLine("AND Department.ParentDepID = 0 ");
            }
            else
            {
                // 指定部门场合
                strBSql.AppendLine("AND Department.DepID = @DepID ");
                paraList.Add(new SqlParameter("@DepID", depID));
            }
            // 指定公司
            if (companyID > 0)
            {
                strBSql.AppendLine("AND Company.CompanyID = @CompanyID ");
                paraList.Add(new SqlParameter("@CompanyID", companyID));
            }
            strBSql.AppendLine("ORDER BY CompanyID, DepID ");

            dt = DbHelperSQL.Query(strBSql.ToString(), paraList.ToArray()).Tables[0];

            if (DataTableTools.DataTableIsNull(dt)) return null;

            IList<Model.ORG.CompanyDepInfo> departmentList = ConvertToList.DataTableToList<Model.ORG.CompanyDepInfo>(dt);

            return departmentList;
        }



        /// <summary>
        ///  获取公司-部门-员工树形信息
        /// </summary>
        /// <param name="companyID">公司ID（可以为空）</param>
        /// <param name="depID">部门ID（可以为空）</param>
        /// <returns></returns>
        public IList<Model.ORG.EmployeeTreeInfo> GetEmployeeInfo(int companyID = 0, int depID = 0, int BusinessValue = 0)
        {
            // 员工所属部门角色
            RunTecMs.RunDAL.Organizations.Employee employeeInfo = new RunTecMs.RunDAL.Organizations.Employee();
            IList<Model.ORG.EmployeeDepartmentRole> employeeList = new List<Model.ORG.EmployeeDepartmentRole>();

            // 员工树形
            IList<Model.ORG.EmployeeTreeInfo> employeeTreeList = new List<Model.ORG.EmployeeTreeInfo>();
            Model.ORG.EmployeeTreeInfo employeeTreeInfo = new Model.ORG.EmployeeTreeInfo();

            // 取得全部部门树形
            IList<Model.ORG.DepTreeInfo> DepTreeList = GetDepartmentInfo(companyID, depID);
            for (int i = 0; i < DepTreeList.Count; i++)
            {
                int CompanyID = DepTreeList[i].CompanyID;
                int DepID = DepTreeList[i].DepID;

                employeeList = employeeInfo.GetEmployeeDepartmentID(false, CompanyID, depID, 0, BusinessValue);
                if (employeeList == null)
                {
                    continue;
                }
                for (int j = 0; j < employeeList.Count; j++)
                {
                    employeeTreeInfo = new Model.ORG.EmployeeTreeInfo();
                    // 自定义ID
                    employeeTreeInfo.ID = DepTreeList[i].ID + "_" + employeeList[j].EmployeeID;
                    // 自定义亲ID
                    employeeTreeInfo.PreID = DepTreeList[i].ID;

                    // 公司信息
                    employeeTreeInfo.CompanyID = DepTreeList[i].CompanyID;
                    employeeTreeInfo.CompanyCode = DepTreeList[i].CompanyCode;
                    employeeTreeInfo.CompanyName = DepTreeList[i].CompanyName;
                    // 部门信息
                    employeeTreeInfo.DepID = DepTreeList[i].DepID;
                    employeeTreeInfo.DepName = DepTreeList[i].DepName;
                    // 员工信息
                    employeeTreeInfo.EmployeeID = employeeList[j].EmployeeID;
                    employeeTreeInfo.EmployeeLoginName = employeeList[j].LoginName;
                    employeeTreeInfo.EmployeeTrueName = employeeList[j].TrueName;
                    employeeTreeList.Add(employeeTreeInfo);
                }
            }
            return employeeTreeList;
        }

        /// <summary>
        /// 根据用户ID获取公司-部门树形信息(插入树形中缺失结构)
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public IList<Model.ORG.DepTreeInfo> GetDepTreeByEmployee(int EmployeeID)
        {
            IList<Model.ORG.DepTreeInfo> returnTreeList = new List<Model.ORG.DepTreeInfo>();
            Model.ORG.DepTreeInfo showDepTree = new Model.ORG.DepTreeInfo();

            // 取得树形结构
            IList<Model.ORG.DepTreeInfo> depTreeList = GetDepTree(EmployeeID);
            IList<string> idList = new List<string>();

            // 取得树形中所有ID
            for (int i = 0; i < depTreeList.Count; i++)
            {
                idList.Add(depTreeList[i].ID);
            }

            // 取得树形结构循环判断
            for (int i = 0; i < depTreeList.Count; i++)
            {
                int companyID = depTreeList[i].CompanyID;
                // 亲ID
                string perID = depTreeList[i].PreID;

                // 树形中不包含指定亲ID
                if (!idList.Contains(perID) && !string.IsNullOrEmpty(perID))
                {
                    showDepTree = GetPerDepInfo(companyID, EmployeeID, perID);
                    treeList = new List<Model.ORG.DepTreeInfo>();
                    // 递归取得父级节点
                    treeList =  GetPerInfo(showDepTree, idList, companyID, EmployeeID);
                    // 按照倒序追加到新树形中
                    for (int j = treeList.Count - 1; j >= 0; j--)
                    {
                        returnTreeList.Add(treeList[j]);
                    }
                }

                // 检索结果中的树形直接追加到新树形中
                returnTreeList.Add(depTreeList[i]);
            }

            return returnTreeList;
        }

        /// <summary>
        /// 递归取得父级节点
        /// </summary>
        /// <param name="showDepTree"></param>
        /// <param name="idList"></param>
        /// <param name="companyID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        private IList<Model.ORG.DepTreeInfo> GetPerInfo(Model.ORG.DepTreeInfo showDepTree, IList<string> idList, int companyID, int EmployeeID)
        {
            Model.ORG.DepTreeInfo depTree = new Model.ORG.DepTreeInfo();

            treeList.Add(showDepTree);
            if (!idList.Contains(showDepTree.PreID) && !string.IsNullOrEmpty(showDepTree.PreID))
            {
                depTree = GetPerDepInfo(companyID, EmployeeID, showDepTree.PreID);
                treeList.Add(depTree);
            }

            return treeList;
        }

        /// <summary>
        /// 获取父级树节点信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="perID"></param>
        /// <returns></returns>
        private Model.ORG.DepTreeInfo GetPerDepInfo(int companyID, int EmployeeID, string perID)
        {
            Model.ORG.DepTreeInfo showDepTree = new Model.ORG.DepTreeInfo();

            // 取得树形结构
            IList<Model.ORG.DepTreeInfo> depTreeList = GetDepTree(EmployeeID);
            IList<Model.ORG.DepTreeInfo> newDepTreeList = GetDepTree(EmployeeID);
            IList<string> idList = new List<string>();

            // 根据公司ID取得所有部门ID
            IList<Model.ORG.DepTreeInfo> allDepTreeList = GetDepartmentInfo(companyID);

            // 取得树形中所有ID
            for (int i = 0; i < depTreeList.Count; i++)
            {
                idList.Add(depTreeList[i].ID);
            }

            for (int j = 0; j < allDepTreeList.Count; j++)
            {
                if (allDepTreeList[j].ID == perID)
                {
                    showDepTree = allDepTreeList[j];
                    break;
                }
            }
            return showDepTree;
        }


        /// <summary>
        /// 根据用户ID获取登录员工直属公司-部门树形信息
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public IList<Model.ORG.DepTreeInfo> GetDepTree(int EmployeeID)
        {
            // 根据员工取得所属部门及角色
            RunTecMs.RunDAL.Organizations.Employee employeeInfo = new RunTecMs.RunDAL.Organizations.Employee();
            IList<Model.ORG.EmployeeDepartmentRole> employeeDepRoleList = employeeInfo.GetEmployeeDepartmentID(false, 0, 0, EmployeeID);
            if (employeeDepRoleList == null)
            {
                return null;
            }

            // 取得登录用户最大权限
            int maxUserRoleID = employeeDepRoleList[0].RoleID;

            // 定义变量
            IList<Model.ORG.DepTreeInfo> depTreeList = new List<Model.ORG.DepTreeInfo>();
            IList<Model.ORG.DepTreeInfo> showDepTree = new List<Model.ORG.DepTreeInfo>();
            IList<string> depList = new List<string>();

            // 循环取得的部门及角色信息
            for (int i = 0; i < employeeDepRoleList.Count; i++)
            {
                // 公司
                int companyId = employeeDepRoleList[i].CompanyID;
                // 部门
                int depId = employeeDepRoleList[i].DepID;
                // 角色
                int roleId = employeeDepRoleList[i].RoleID;
                // 业务值
                int BusinessValue = employeeDepRoleList[i].BusinessValue;

                int companyIdTemp = 0;
                int depIdTemp = 0;
                // 根据公司和部门取得树形结构
                if ((maxUserRoleID >= Convert.ToInt32(RoleValue.超级管理员) && maxUserRoleID <= Convert.ToInt32(RoleValue.总经理)))
                {
                    depTreeList = GetDepartmentInfo(companyId);
                }
                else
                {
                    depTreeList = GetDepartmentInfo(companyId, depId);
                }
                if (depTreeList == null)
                {
                    continue;
                }

                for (int j = 0; j < depTreeList.Count; j++)
                {
                    string strCompanyDep = Convert.ToString(depTreeList[j].CompanyID) + "," + Convert.ToString(depTreeList[j].DepID);
                    
                    //
                    if (depTreeList[j].CompanyID == companyIdTemp && depTreeList[j].DepID == depIdTemp)
                    {
                        continue;
                    }
                    else
                    {
                        // 判断是否已经存在公司信息
                        if (!depList.Contains(strCompanyDep))
                        {
                            showDepTree.Add(depTreeList[j]);
                            depList.Add(Convert.ToString(strCompanyDep));
                        }
                    }

                    // 变量设定
                    if (depTreeList[j].CompanyID != companyIdTemp)
                    {
                        companyIdTemp = depTreeList[j].CompanyID;
                    }
                    if (depTreeList[j].DepID != depIdTemp)
                    {
                        depIdTemp = depTreeList[j].DepID;
                    }
                }
            }
            return showDepTree;
        }

        /// <summary>
        /// 根据用户ID获取公司-部门-员工树形信息
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="BusinessValue"></param>
        /// <param name="maxUserRoleID"></param>
        /// <returns></returns>
        public IList<Model.ORG.EmployeeTreeInfo> GetEmployeeTreeByEmployee(int EmployeeID, int BusinessValue, int maxUserRoleID)
        {
            // 公司-部门-员工树形信息
            IList<Model.ORG.EmployeeTreeInfo> showEmployeeTree = new List<Model.ORG.EmployeeTreeInfo>();
            Model.ORG.EmployeeTreeInfo employeeTreeInfo = new Model.ORG.EmployeeTreeInfo();
            // 员工所属(公司-部门-员工)
            IList<string> employeeTreeIDList = new List<string>();

            // 变量定义
            RunTecMs.RunDAL.Organizations.Employee employeeInfo = new RunTecMs.RunDAL.Organizations.Employee();
            IList<Model.ORG.EmployeeDepartmentRole> employeeDepRoleList = new List<Model.ORG.EmployeeDepartmentRole>();
            IList<Model.ORG.EmployeeDepartmentRole> employeeDepRoleDetailList = new List<Model.ORG.EmployeeDepartmentRole>();
            IList<Model.ORG.EmployeeDepartmentRole> employeeRoleList = new List<Model.ORG.EmployeeDepartmentRole>();
            Model.ORG.EmployeeDepartmentRole departRolePara = new Model.ORG.EmployeeDepartmentRole();

            // 取得公司-部门树形
            IList<Model.ORG.DepTreeInfo> depTreeList = GetDepTreeByEmployee(EmployeeID);

            // 取得登录用户情报
            if (maxUserRoleID == 0)
            {
                employeeDepRoleList = employeeInfo.GetEmployeeDepartmentID(false, 0, 0, EmployeeID, BusinessValue);
                if (employeeDepRoleList != null)
                {
                    maxUserRoleID = employeeDepRoleList[0].RoleID;
                }
                else
                {
                    return showEmployeeTree;
                }
            }

            // 根据树形去匹配员工信息匹配上的追加到相应节点上
            // 总经理以上权限，可以查看本公司所有员工权限
            if ((maxUserRoleID >= Convert.ToInt32(RoleValue.超级管理员) && maxUserRoleID <= Convert.ToInt32(RoleValue.总经理)))
            {
                // 根据树形去匹配员工信息匹配上的追加到相应节点上
                for (int i = 0; i < depTreeList.Count; i++)
                {
                    // 公司/部门树形追加
                    // 20170421 
                    employeeTreeInfo = GetEmployeeTreeDetailInfo(depTreeList[i], depTreeList[i].ID, depTreeList[i].PreID, 0, 0, "", "");
                    if (!employeeTreeIDList.Contains(employeeTreeInfo.ID))
                    {
                        showEmployeeTree.Add(employeeTreeInfo);
                        employeeTreeIDList.Add(employeeTreeInfo.ID);
                    }

                    //公司/部门精准匹配检索
                    employeeDepRoleDetailList = employeeInfo.GetEmployeeDepartmentID(true, depTreeList[i].CompanyID, depTreeList[i].DepID);
                    if (employeeDepRoleDetailList == null)
                    {
                        continue;
                    }

                    // 属于当前公司/部门员工设定
                    for (int j = 0; j < employeeDepRoleDetailList.Count; j++)
                    {
                        // 自定义ID
                        string ID = depTreeList[i].ID + "_" + employeeDepRoleDetailList[j].EmployeeID;
                        // 自定义亲ID
                        string PreID = depTreeList[i].ID;

                        // 员工信息
                        int employeeIDTemp = employeeDepRoleDetailList[j].EmployeeID;
                        string employeeLoginName = employeeDepRoleDetailList[j].LoginName;
                        string employeeTrueName = employeeDepRoleDetailList[j].TrueName;

                        employeeTreeInfo = GetEmployeeTreeDetailInfo(depTreeList[i], ID, PreID, employeeIDTemp, maxUserRoleID, employeeLoginName, employeeTrueName);
                        if (!employeeTreeIDList.Contains(employeeTreeInfo.ID))
                        {
                            showEmployeeTree.Add(employeeTreeInfo);
                            employeeTreeIDList.Add(employeeTreeInfo.ID);
                        }
                    }
                }
            }
            else
            {
                int userRoleID = 0;
                string perID = "";
                
                // 根据树形去匹配员工信息匹配上的追加到相应节点上
                for (int i = 0; i < depTreeList.Count; i++)
                {
                    bool IsSubDep = false;
                    bool existFlg = false;

                    // 取得树形中公司和部门
                    int compantID = depTreeList[i].CompanyID;
                    int depID = depTreeList[i].DepID;

                    string PreIDTemp = depTreeList[i].PreID;
                    
                    // 公司/部门树形追加
                    employeeTreeInfo = GetEmployeeTreeDetailInfo(depTreeList[i], depTreeList[i].ID, PreIDTemp, 0, 0, "", "");
                    if (!employeeTreeIDList.Contains(employeeTreeInfo.ID))
                    {
                        showEmployeeTree.Add(employeeTreeInfo);
                        employeeTreeIDList.Add(employeeTreeInfo.ID);
                    }

                    //公司/部门精准匹配检索
                    if (BusinessValue == 0)
                    {
                        employeeDepRoleDetailList = employeeInfo.GetEmployeeDepartmentID(true, compantID, depID);
                        employeeRoleList = employeeInfo.GetEmployeeDepartmentID(true, compantID, depID, EmployeeID);
                    }
                    else
                    {
                        employeeDepRoleDetailList = employeeInfo.GetEmployeeDepartmentID(true, compantID, depID, 0, BusinessValue);
                        employeeRoleList = employeeInfo.GetEmployeeDepartmentID(true, compantID, depID, EmployeeID, BusinessValue);
                    }

                    if (employeeRoleList != null)
                    {
                        // 取得在部门中的角色
                        userRoleID = employeeRoleList[0].RoleID;
                        existFlg = true;
                    }
                    else
                    {
                        string perDepID = "";
                        // 取得父级部门ID
                        if (PreIDTemp.Contains("_"))
                        {
                            perDepID = PreIDTemp.Substring(PreIDTemp.IndexOf("_") + 1);

                            employeeRoleList = employeeInfo.GetEmployeeDepartmentID(true, compantID, Convert.ToInt32(perDepID), EmployeeID);

                            if (employeeRoleList != null)
                            {
                                // 取得在部门中的角色
                                userRoleID = employeeRoleList[0].RoleID;
                                existFlg = true;
                                IsSubDep = true;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    // 当父级部门相同时跳转到下一条
                    if (perID.Equals(depTreeList[i].PreID) && IsSubDep == false)
                    {
                        continue;
                    }
                    else
                    {
                        // 父级部门不同时，认为是下级部门
                        if (existFlg == true)
                        {
                            perID = depTreeList[i].PreID;
                        }
                        existFlg = true;
                    }

                    if (userRoleID == Convert.ToInt32(RoleValue.经理) && employeeDepRoleDetailList != null)
                    {

                        // 属于当前公司/部门员工设定
                        for (int j = 0; j < employeeDepRoleDetailList.Count; j++)
                        {
                            // 自定义ID
                            string ID = depTreeList[i].ID + "_" + employeeDepRoleDetailList[j].EmployeeID;
                            // 自定义亲ID
                            string PreID = depTreeList[i].ID;

                            // 员工信息
                            int employeeIDTemp = employeeDepRoleDetailList[j].EmployeeID;
                            string employeeLoginName = employeeDepRoleDetailList[j].LoginName;
                            string employeeTrueName = employeeDepRoleDetailList[j].TrueName;

                            employeeTreeInfo = GetEmployeeTreeDetailInfo(depTreeList[i], ID, PreID, employeeIDTemp, userRoleID, employeeLoginName, employeeTrueName);
                            if (!employeeTreeIDList.Contains(employeeTreeInfo.ID))
                            {
                                showEmployeeTree.Add(employeeTreeInfo);
                                employeeTreeIDList.Add(employeeTreeInfo.ID);
                            }
                            // 20170421 
                        }
                    }
                    else
                    { 
                        // 普通员工场合
                        // 自定义ID
                        string ID = depTreeList[i].ID + "_" + EmployeeID;
                        // 自定义亲ID
                        string PreID = depTreeList[i].ID;
                        // 根据员工ID取得员工信息
                        Model.ORG.EmployeeModel employee = employeeInfo.GetEmployeeByEmployeeID(EmployeeID);

                        employeeTreeInfo = GetEmployeeTreeDetailInfo(depTreeList[i], ID, PreID, EmployeeID, userRoleID, employee.LoginName, employee.TrueName);
                        if (!employeeTreeIDList.Contains(employeeTreeInfo.ID))
                        {
                            showEmployeeTree.Add(employeeTreeInfo);
                            employeeTreeIDList.Add(employeeTreeInfo.ID);
                        }
                        // 20170421 
                    }
                }
            }

            return showEmployeeTree;
        }

        /// <summary>
        /// 获取每个用户详细信息
        /// </summary>
        /// <param name="depTree"></param>
        /// <param name="ID"></param>
        /// <param name="PreID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        private Model.ORG.EmployeeTreeInfo GetEmployeeTreeDetailInfo(Model.ORG.DepTreeInfo depTree, string ID, string PreID, int EmployeeID, int RoleID, string loginName, string trueName)
        {
            Model.ORG.EmployeeTreeInfo employeeTreeInfo = new Model.ORG.EmployeeTreeInfo();

            // 自定义ID
            employeeTreeInfo.ID = ID;
            // 自定义亲ID
            employeeTreeInfo.PreID = PreID;
            // 公司信息
            employeeTreeInfo.CompanyID = depTree.CompanyID;
            employeeTreeInfo.CompanyCode = depTree.CompanyCode;
            employeeTreeInfo.CompanyName = depTree.CompanyName;
            // 部门信息
            employeeTreeInfo.DepID = depTree.DepID;
            employeeTreeInfo.DepName = depTree.DepName;
            // 员工信息
            employeeTreeInfo.EmployeeID = EmployeeID;
            employeeTreeInfo.EmployeeLoginName = loginName;
            employeeTreeInfo.EmployeeTrueName = trueName;
            employeeTreeInfo.RoleID = RoleID;

            return employeeTreeInfo;
        }
        #endregion

        #region 公司信息管理
        /// <summary>
        ///  获取公司信息
        /// </summary>
        /// <param name="companyID">公司ID(可以为空)</param>
        /// <returns>公司列表</returns>
        public IList<Model.ORG.Company> GetCompanyInfo(ParaStruct.CompanyStruct para)
        {
            StringBuilder sb=new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();
            int paraIndex = 0;
            sb.AppendLine("select CompanyID,CompanyCode,Name,Value,OrderValue,CompanyDescription,CompanyLevel,Remark,CompanyPicName,CompanyURL,IsSYSDBA,RegistrationTime,RegistrationAddr,RegisteredCapital from Org_Company");
            sb.AppendLine(" WHERE IsDel = 0 ");
            if (para.CompanyID > 0)
            {
                sb.AppendLine(" AND CompanyID = @CompanyID ");
                listPara.Add(new SqlParameter("@CompanyID", SqlDbType.Int));
                listPara[paraIndex++].Value = para.CompanyID;
            }
            if (para.Name != "" && para.Name != null)
            {
                sb.AppendLine(" AND Name LIKE @Name ");
                listPara.Add(new SqlParameter("@Name", SqlDbType.VarChar,50));
                listPara[paraIndex++].Value ="%" + para.Name + "%";
            }
            if (para.Code != "" && para.Code != null)
            {
                sb.AppendLine(" AND CompanyCode LIKE @Code ");
                listPara.Add(new SqlParameter("@Code", SqlDbType.VarChar, 10));
                listPara[paraIndex++].Value = "%" + para.Code + "%";
            }
            if (para.StartTime != DateTime.MinValue)
            {
                sb.AppendLine(" AND RegistrationTime >=@startTime ");
                listPara.Add(new SqlParameter("@startTime", SqlDbType.DateTime));
                listPara[paraIndex++].Value = para.StartTime;
            }
            if (para.EndTime != DateTime.MaxValue && para.EndTime != DateTime.MinValue)
            {
                sb.AppendLine(" AND RegistrationTime<=@endTime ");
                listPara.Add(new SqlParameter("@endTime", SqlDbType.DateTime));
                listPara[paraIndex++].Value = para.EndTime;
            }
            sb.AppendLine(" ORDER BY CompanyID");

            DataTable dt = DbHelperSQL.Query(sb.ToString(),listPara.ToArray()).Tables[0];

            if (DataTableTools.DataTableIsNull(dt)) return null;

            IList<Model.ORG.Company> companyList = ConvertToList.DataTableToList<Model.ORG.Company>(dt);

            return companyList;
        }

        /// <summary>
        ///  追加公司信息
        /// </summary>
        /// <param name="company">公司情报</param>
        /// <returns>公司ID</returns>
        public int AddCompanyInfo(Model.ORG.Company company)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO Org_Company(CompanyCode , Name ,  Value , OrderValue ,CompanyDescription ,CompanyLevel ,Remark , CompanyPicName ,CompanyURL , IsSYSDBA , RegistrationTime , RegistrationAddr , RegisteredCapital ,IsDel ,AddTime ,UpdateTime ,DelTime)");
            strSql.Append(" values(@CompanyCode,@Name,@Value,@OrderValue,@CompanyDescription,@CompanyLevel, ");
            strSql.Append(" @Remark,@CompanyPicName,@CompanyURL,@IsSYSDBA,@RegistrationTime,@RegistrationAddr,@RegisteredCapital, @IsDel, @AddTime, @UpdateTime, @DelTime) ");

            SqlParameter[] para = {
              new SqlParameter("@CompanyCode",SqlDbType.Char, 10),
              new SqlParameter("@Name",SqlDbType.VarChar, 50),
              new SqlParameter("@Value",SqlDbType.VarChar, 512),
              new SqlParameter("@OrderValue",SqlDbType.Int),
              new SqlParameter("@CompanyDescription",SqlDbType.VarChar, 512),
              new SqlParameter("@CompanyLevel",SqlDbType.Int),
              new SqlParameter("@Remark",SqlDbType.NText, 8000),
              new SqlParameter("@CompanyPicName",SqlDbType.VarChar, 256),
              new SqlParameter("@CompanyURL",SqlDbType.VarChar, 50),
              new SqlParameter("@IsSYSDBA",SqlDbType.Bit),
              new SqlParameter("@RegistrationTime",SqlDbType.DateTime),
              new SqlParameter("@RegistrationAddr",SqlDbType.VarChar, 512),
              new SqlParameter("@RegisteredCapital",SqlDbType.Int),
              new SqlParameter("@IsDel",SqlDbType.Int),
              new SqlParameter("@AddTime",SqlDbType.DateTime),
              new SqlParameter("@UpdateTime",SqlDbType.DateTime),
              new SqlParameter("@DelTime",SqlDbType.DateTime)
            };

            int i = 0;
            para[i++].Value = company.CompanyCode;
            para[i++].Value = company.Name;
            para[i++].Value = company.Value;
            para[i++].Value = company.OrderValue;
            para[i++].Value = company.CompanyDescription;
            para[i++].Value = company.CompanyLevel;
            para[i++].Value = company.Remark;
            para[i++].Value = company.CompanyPicName;
            para[i++].Value = company.CompanyURL;
            para[i++].Value = company.IsSYSDBA;
            if (company.RegistrationTime == System.DateTime.MinValue || company.RegistrationTime == null)
            {
                para[i++].Value = null;
            }
            else
            {
                para[i++].Value = Convert.ToDateTime(company.RegistrationTime);
            }
            para[i++].Value = company.RegistrationAddr;
            para[i++].Value = company.RegisteredCapital;
            para[i++].Value = 0;
            para[i++].Value = DateTime.Now;
            para[i++].Value = null;
            para[i++].Value = null;

            int Result = DbHelperSQL.ExecuteSql(strSql.ToString(), para);

            return Result;
        }

        /// <summary>
        ///  更新公司信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="company">公司情报</param>
        /// <returns>true:成功 false:失败</returns>
        public bool UpdateCompanyInfo(int companyId, Model.ORG.Company company)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE Org_Company ");
            strSql.Append(" SET CompanyCode=@CompanyCode, Name=@Name, Value=@Value, OrderValue=@OrderValue, ");
            strSql.Append(" CompanyDescription=@CompanyDescription, CompanyLevel=@CompanyLevel, Remark=@Remark, CompanyPicName=@CompanyPicName, ");
            strSql.Append(" CompanyURL=@CompanyURL, RegistrationTime=@RegistrationTime, RegistrationAddr=@RegistrationAddr, RegisteredCapital=@RegisteredCapital, UpdateTime=@UpdateTime");
            strSql.Append(" WHERE CompanyId=@CompanyId  ");

            SqlParameter[] para = {
              new SqlParameter("@CompanyCode",SqlDbType.Char, 10),
              new SqlParameter("@Name",SqlDbType.VarChar, 50),
              new SqlParameter("@Value",SqlDbType.VarChar, 512),
              new SqlParameter("@OrderValue",SqlDbType.Int),
              new SqlParameter("@CompanyDescription",SqlDbType.Char, 512),
              new SqlParameter("@CompanyLevel",SqlDbType.Int),
              new SqlParameter("@Remark",SqlDbType.NText, 8000),
              new SqlParameter("@CompanyPicName",SqlDbType.VarChar, 256),
              new SqlParameter("@CompanyURL",SqlDbType.VarChar, 50),
              new SqlParameter("@RegistrationTime",SqlDbType.DateTime),
              new SqlParameter("@RegistrationAddr",SqlDbType.VarChar, 512),
              new SqlParameter("@RegisteredCapital",SqlDbType.Int),
              new SqlParameter("@UpdateTime",SqlDbType.DateTime),
              new SqlParameter("@CompanyId",SqlDbType.Int)
              
            };

            int i = 0;
            para[i++].Value = company.CompanyCode;
            para[i++].Value = company.Name;
            para[i++].Value = company.Value;
            para[i++].Value = company.OrderValue;
            para[i++].Value = company.CompanyDescription;
            para[i++].Value = company.CompanyLevel;
            para[i++].Value = company.Remark;
            para[i++].Value = company.CompanyPicName;
            para[i++].Value = company.CompanyURL;
            para[i++].Value = company.RegistrationTime ?? DateTime.Now;
            para[i++].Value = company.RegistrationAddr;
            para[i++].Value = company.RegisteredCapital;
            para[i++].Value = DateTime.Now;
            para[i++].Value = companyId;

            int result = DbHelperSQL.ExecuteSql(strSql.ToString(), para);
            return result > 0;
        }

        /// <summary>
        ///  删除公司信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns>true:成功 false:失败</returns>
        public bool DeleteCompanyInfo(int companyId)
        {
            string strSql = "UPDATE Org_Company SET IsDel=1, DelTime = @DelTime WHERE CompanyId=@CompanyId";
            SqlParameter[] para = {
                                      new SqlParameter("@CompanyId",SqlDbType.Int),
                                      new SqlParameter("@DelTime",SqlDbType.DateTime)
                                  };
            para[0].Value = companyId;
            para[1].Value = DateTime.Now;
            int Result = DbHelperSQL.ExecuteSql(strSql, para);
            return Result > 0; 
        }

        /// <summary>
        /// 验证公司名字
        /// </summary>
        /// <param name="name"></param>
        /// <param name="comId"></param>
        /// <returns></returns>
        public bool SameCompanyName(string name, int comId = 0)
        {
            List<SqlParameter> paralist = new List<SqlParameter>();

            string strSql = "SELECT COUNT(0) FROM Org_Company where Name=@name";
            paralist.Add(new SqlParameter("@name", name));
            if (comId > 0)
            {
                strSql = strSql + " and CompanyID <> @comId";
                paralist.Add(new SqlParameter("@comId", comId));
            }
            return DbHelperSQL.Exists(strSql, paralist.ToArray());
        }
        #endregion

        #region 部门信息管理
        /// <summary>
        ///  获取部门信息
        /// </summary>
        /// <param name="companyID">公司ID(可以为空)</param>
        /// <param name="depID">部门ID(可以为空)</param>
        /// <returns>部门列表</returns>
        public IList<Model.ORG.DepartmentDetail> GetDepartment(ParaStruct.DepartStruct departStruct)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            StringBuilder strbSql = new StringBuilder();
            strbSql.AppendLine("select t.*, t1.Name as ParentDepName, t2.Name as CompanyName from Org_Department t ");
            strbSql.AppendLine(" left join Org_Department t1 on t.ParentDepID = t1.DepID ");
            strbSql.AppendLine(" left join Org_Company t2 on t.CompanyID = t2.CompanyID ");
            strbSql.AppendLine("WHERE ISNULL(t.IsDel,0)=0 ");

            if (departStruct.CompanyID > 0)
            {
                strbSql.AppendLine(" AND t.CompanyID = @CompanyID");
                paraList.Add(new SqlParameter("@CompanyID", departStruct.CompanyID));
            }

            if (!string.IsNullOrEmpty(departStruct.Code))
            {
                strbSql.AppendLine(" AND t.DepCode LIKE @DepCode");
                paraList.Add(new SqlParameter("@DepCode", "%" + departStruct.Code + "%"));
            }

            if (!string.IsNullOrEmpty(departStruct.Name))
            {
                strbSql.AppendLine(" AND t.Name LIKE @Name");
                paraList.Add(new SqlParameter("@Name", "%" + departStruct.Name + "%"));
            }
            strbSql.AppendLine(" ORDER BY CompanyID, DepID");

            DataTable dt = DbHelperSQL.Query(strbSql.ToString(), paraList.ToArray()).Tables[0];

            if (DataTableTools.DataTableIsNull(dt)) return null;

            IList<Model.ORG.DepartmentDetail> departmentList = ConvertToList.DataTableToList<Model.ORG.DepartmentDetail>(dt);

            return departmentList;
        }

        /// <summary>
        /// 验证部门是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <param name="companyId">所属公司</param>
        /// <param name="depId">本部门</param>
        /// <param name="parentDepId">亲部门ID</param>
        /// <returns></returns>
        public bool SameDepName(string name, string code, int companyId, int depId , int parentDepId )
        {
            List<SqlParameter> paraList = new List<SqlParameter>();

            string strSql = "SELECT COUNT(0) FROM Org_Department WHERE Name=@name AND DepCode = @DepCode";
            strSql = strSql + " AND CompanyID = @CompanyID AND IsDel = 0 AND DepID <> @DepID ";
            paraList.Add(new SqlParameter("@name", name));
            paraList.Add(new SqlParameter("@DepCode", code));
            paraList.Add(new SqlParameter("@CompanyID", companyId));
            paraList.Add(new SqlParameter("@DepID", depId));

            if (parentDepId != 0)
            {
                strSql = strSql + " AND ParentDepID = @ParentDepID ";
                paraList.Add(new SqlParameter("@ParentDepID", parentDepId));
            }
            return DbHelperSQL.Exists(strSql, paraList.ToArray());
        }

        /// <summary>
        ///  追加部门信息
        /// </summary>
        /// <param name="company">部门情报</param>
        /// <returns>部门ID</returns>
        public int AddDepartInfo(Model.ORG.Department department)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Org_Department ");
            strSql.Append("(ParentDepID,Name,Value,OrderValue,DepCode,DepDescription,DepLevel,Remark,DepPicName,CompanyID,IsSYSDBA,AddTime,IsDel)");
            strSql.Append(" values(@ParentDepID,@Name,@Value,@OrderValue,@DepCode,@DepDescription, ");
            strSql.Append(" @DepLevel,@Remark,@DepPicName,@CompanyID,@IsSYSDBA,@AddTime,@IsDel)");

            SqlParameter[] para = {
              new SqlParameter("@ParentDepID",SqlDbType.Int),
              new SqlParameter("@Name",SqlDbType.VarChar, 50),
              new SqlParameter("@Value",SqlDbType.VarChar, 512),
              new SqlParameter("@OrderValue",SqlDbType.Int),
              new SqlParameter("@DepCode",SqlDbType.Char, 3),
              new SqlParameter("@DepDescription",SqlDbType.VarChar, 512),
              new SqlParameter("@DepLevel",SqlDbType.Int),
              new SqlParameter("@Remark",SqlDbType.NText, 8000),
              new SqlParameter("@DepPicName",SqlDbType.VarChar, 256),
              new SqlParameter("@CompanyID",SqlDbType.Int),
              new SqlParameter("@IsSYSDBA",SqlDbType.Bit),
              new SqlParameter("@AddTime",SqlDbType.DateTime),
              new SqlParameter("@IsDel",SqlDbType.Bit)
            };

            int i = 0;
            para[i++].Value = department.ParentDepID;
            para[i++].Value = department.Name;
            para[i++].Value = department.Value;
            para[i++].Value = department.OrderValue;
            para[i++].Value = department.DepCode;
            para[i++].Value = department.DepDescription;
            para[i++].Value = department.DepLevel;
            para[i++].Value = department.Remark;
            para[i++].Value = department.DepPicName;
            para[i++].Value = department.CompanyID;
            para[i++].Value = department.IsSYSDBA;
            para[i++].Value = department.AddTime ?? DateTime.Now;
            para[i++].Value = department.IsDel;

            int Result = DbHelperSQL.ExecuteSql(strSql.ToString(), para);

            if (Result > 0)
            {
                // 返回新追加ID
                string sqlartId = "select IDENT_CURRENT('Org_Department')";
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
        ///  更新部门信息
        /// </summary>
        /// <param name="companyId">部门ID</param>
        /// <param name="company">部门情报</param>
        /// <returns>true:成功 false:失败</returns>
        public bool UpdateDepartmentInfo(int departmentId, Model.ORG.Department department)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE Org_Department ");
            strSql.Append(" SET parentDepID=@ParentDepID, Name=@Name, Value=@Value, OrderValue=@OrderValue, DepCode=@DepCode, ");
            strSql.Append(" DepDescription=@DepDescription, DepLevel=@DepLevel, Remark=@Remark, DepPicName=@DepPicName, ");
            strSql.Append(" CompanyID=@CompanyID, IsSYSDBA=@IsSYSDBA, AddTime=@AddTime,IsDel=@isdel,UpdateTime=@UpdateTime ");
            strSql.Append(" WHERE DepID=@DepID  ");

            SqlParameter[] para = {
              new SqlParameter("@ParentDepID",SqlDbType.Int),
              new SqlParameter("@Name",SqlDbType.VarChar, 50),
              new SqlParameter("@Value",SqlDbType.VarChar, 512),
              new SqlParameter("@OrderValue",SqlDbType.Int),
              new SqlParameter("@DepCode",SqlDbType.Char, 3),
              new SqlParameter("@DepDescription",SqlDbType.VarChar, 512),
              new SqlParameter("@DepLevel",SqlDbType.Int),
              new SqlParameter("@Remark",SqlDbType.NText, 8000),
              new SqlParameter("@DepPicName",SqlDbType.VarChar, 256),
              new SqlParameter("@CompanyID",SqlDbType.Int),
              new SqlParameter("@IsSYSDBA",SqlDbType.Bit),
              new SqlParameter("@AddTime",SqlDbType.DateTime),
              new SqlParameter("@DepID",SqlDbType.Int),
              new SqlParameter("@isdel",SqlDbType.Bit),
              new SqlParameter("@UpdateTime",SqlDbType.DateTime)
            };

            int i = 0;
            para[i++].Value = department.ParentDepID;
            para[i++].Value = department.Name;
            para[i++].Value = department.Value;
            para[i++].Value = department.OrderValue;
            para[i++].Value = department.DepCode;
            para[i++].Value = department.DepDescription;
            para[i++].Value = department.DepLevel;
            para[i++].Value = department.Remark;
            para[i++].Value = department.DepPicName;
            para[i++].Value = department.CompanyID;
            para[i++].Value = department.IsSYSDBA;
            if (department.AddTime == System.DateTime.MinValue || department.AddTime == null)
            {
                para[i++].Value = null;
            }
            else
            {
                para[i++].Value = Convert.ToDateTime(department.AddTime);
            }
            //para[i++].Value = department.AddTime ?? DateTime.Now;
            para[i++].Value = departmentId;
            para[i++].Value = department.IsDel;
            para[i++].Value = DateTime.Now;

            int result = DbHelperSQL.ExecuteSql(strSql.ToString(), para);
            return result > 0;
        }

        /// <summary>
        ///  删除部门信息
        /// </summary>
        /// <param name="companyId">部门ID</param>
        /// <returns>true:成功 false:失败</returns>
        public bool DeleteDepartment(int depID)
        {
            string strSql = "UPDATE Org_Department SET IsDel=1, DelTime=@DelTime WHERE DepID=@DepID";
            SqlParameter[] para = {
                                      new SqlParameter("@DepID",SqlDbType.Int),
                                      new SqlParameter("@DelTime",SqlDbType.DateTime)
                                  };
            para[0].Value = depID;
            para[1].Value = DateTime.Now;
            int Result = DbHelperSQL.ExecuteSql(strSql, para);
            return Result > 0; ;
        }

        /// <summary>
        /// 取得公司部门下所有部门信息
        /// </summary>
        /// <returns></returns>
        public IList<Model.ORG.DepInfo> GetAllDepList(int companyId, int depId)
        {
            IList<Model.ORG.DepInfo> depList = new List<Model.ORG.DepInfo>();
            DataTable dataTable = new DataTable();
            // 取得公司部门下所有部门信息
            IDataParameter[] para = { new SqlParameter("@companyId", SqlDbType.Int),
                                          new SqlParameter("@DepId", SqlDbType.Int)};
            para[0].Value = companyId;
            para[1].Value = depId;
            dataTable = DbHelperSQL.RunProcedure("sp_GetPerDepartment", para, "Department").Tables[0];
            depList = ConvertToList.DataTableToList<Model.ORG.DepInfo>(dataTable);
            if (depList == null)
            {
                return null;
            }
            else
            {
                return depList;
            }
        }

        /// <summary>
        /// 取得上级部门ID
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="depID"></param>
        /// <returns></returns>
        public int GetPerDep(int companyId, int depID)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            StringBuilder strbSql = new StringBuilder();
            strbSql.AppendLine("select ParentDepID from Org_Department t ");
            strbSql.AppendLine("WHERE ISNULL(t.IsDel,0)=0 ");
            strbSql.AppendLine(" AND t.DepID = @DepID");
            strbSql.AppendLine(" AND t.CompanyID = @CompanyID");
            paraList.Add(new SqlParameter("@DepID", depID));
            paraList.Add(new SqlParameter("@CompanyID", companyId));

            DataTable dt = DbHelperSQL.Query(strbSql.ToString(), paraList.ToArray()).Tables[0];

            if (DataTableTools.DataTableIsNull(dt)) return 0;

            IList<Model.ORG.DepartmentDetail> departmentList = ConvertToList.DataTableToList<Model.ORG.DepartmentDetail>(dt);

            return departmentList[0].ParentDepID;
        }
        #endregion

        #region 业务

        /// <summary>
        /// 获取业务列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<Model.ORG.Business> GetBusinessList(int pageIndex, int pageSize, string Name, string Value)
        {
            Model.Common.GetPageInfo info = new Model.Common.GetPageInfo();

            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Name))
            {
                sb.AppendLine("and Name like @Name");
                listPara.Add(new SqlParameter("@Name", "%" + Name + "%"));
            }
            if (!string.IsNullOrEmpty(Value))
            {
                sb.AppendLine("and BusinessValue like @Value");
                listPara.Add(new SqlParameter("@Value", "%" + Value + "%"));
            }
            // 表名
            info.tabName = "CRM_Business";
            // 主键
            info.pkColumn = "BusinessID";
            // 表示项目
            info.showColumn = "*";
            // 排序条件
            info.ascColumn = "BusinessID asc";
            // 条件
            info.where = sb.ToString();
            // 排序 (0为升序,1为降序)
            info.intOrderType = 0;
            // 页码
            info.pageIndex = pageIndex;
            // 页面显示的数据数量
            info.pageSize = pageSize;

            string strSql = Common.GetPageResult(info);

            DataTable dt = DbHelperSQL.Query(strSql, listPara.ToArray()).Tables[0];
            return ConvertToList.DataTableToList<Model.ORG.Business>(dt);

        }
        /// <summary>
        /// 获取业务的数据数量
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public int GetBusinessCount(string Name, string Value)
        {
            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Name))
            {
                sb.AppendLine("and Name like @Name");
                listPara.Add(new SqlParameter("@Name", "%" + Name + "%"));
            }
            if (!string.IsNullOrEmpty(Value))
            {
                sb.AppendLine("and Value like @Value");
                listPara.Add(new SqlParameter("@Value", "%" + Value + "%"));
            }
            return Common.GetRowsCount("CRM_Business", sb.ToString(), listPara.ToArray());
        }
        /// <summary>
        /// 修改业务
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool EditBusiness(string UpdateID, ParaStruct.Business Business)
        {
            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            listPara.Add(new SqlParameter("@BusinessID", Business.BusinessID));
            listPara.Add(new SqlParameter("@Name", Business.Name));
            listPara.Add(new SqlParameter("@BusinessValue", Business.BusinessValue));
            listPara.Add(new SqlParameter("@BusinessName", Business.BusinessName));
            listPara.Add(new SqlParameter("@OrderValue", Business.OrderValue));
            listPara.Add(new SqlParameter("@IsSYSDBA",Business.IsSYSDBA));
      

            //如果UpdateID=true 为插入数据操作
            if (UpdateID == "true")
            {
                sb.AppendLine("INSERT  INTO CRM_Business (BusinessID, Name, BusinessValue, BusinessName, OrderValue, IsSYSDBA)");
                sb.AppendLine("VALUES (@BusinessID, @Name, @BusinessValue, @BusinessName, @OrderValue, @IsSYSDBA)");
                int count = DbHelperSQL.ExecuteSql(sb.ToString(), listPara.ToArray());
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
           //否则为更新操作
            else
            {

                sb.AppendLine("UPDATE  CRM_Business SET BusinessID=@BusinessID, Name=@Name,BusinessValue=@BusinessValue,");
                sb.AppendLine(" BusinessName=@BusinessName, OrderValue=@OrderValue, IsSYSDBA=@IsSYSDBA");
                int count = DbHelperSQL.ExecuteSql(sb.ToString(), listPara.ToArray());
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //删除业务
        public bool DeleteBusiness(string deleteID)
        {
            return Common.DeleteRecordByPK("CRM_Business", "BusinessID", deleteID);
        }

        /// <summary>
        /// 验证是否存在ID
        /// </summary>
        /// <returns></returns>
        public bool IsNoExist(string BusinessID)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.AppendLine(" select count(0) from CRM_Business");
            strsql.AppendLine(" where BusinessID=@BusinessID");
            SqlParameter[] para = { new SqlParameter("@BusinessID", SqlDbType.VarChar, 10) };
            para[0].Value = BusinessID;
            return DbHelperSQL.Exists(strsql.ToString(), para);
        }


        #endregion

        #region 职位/job

        /// <summary>
        ///  获取职位
        /// </summary>
        /// <param name="jobID">职位ID(可以为空)</param>
        /// <returns>职位列表</returns>
        public IList<Model.ORG.Job> GetJob(int jobID)
        {
            string strSql = "SELECT JobID,Name,Value,OrderValue,PerJobID,IsSYSDBA FROM Org_Job";
            if (jobID > 0)
            {
                strSql = strSql + " WHERE JobID = @JobID";
                SqlParameter[] para =
                {
                     new SqlParameter("@JobID",SqlDbType.Int)
                };
                para[0].Value = jobID;
            }

            DataTable dt = DbHelperSQL.Query(strSql).Tables[0];

            if (DataTableTools.DataTableIsNull(dt)) return null;

            IList<Model.ORG.Job> jobList = ConvertToList.DataTableToList<Model.ORG.Job>(dt);

            return jobList;
        }

        /// <summary>
        ///  获取数据范围
        /// </summary>
        /// <param name="dataRangeID">数据范围ID(可以为空)</param>
        /// <returns>数据范围列表</returns>
        public IList<Model.ORG.DataRang> GetDataRange(int dataRangeID)
        {
            string strSql = "SELECT DataRangeID,Name,Value,OrderValue,DataRange,IsSYSDBA FROM Per_DataRange";
            if (dataRangeID > 0)
            {
                strSql = strSql + " WHERE DataRangeID = @DataRangeID";
                SqlParameter[] para =
                {
                     new SqlParameter("@DataRangeID",SqlDbType.Int)
                };
                para[0].Value = dataRangeID;
            }
            DataTable dt = DbHelperSQL.Query(strSql).Tables[0];

            if (DataTableTools.DataTableIsNull(dt)) return null;

            IList<Model.ORG.DataRang> dataRangList = ConvertToList.DataTableToList<Model.ORG.DataRang>(dt);

            return dataRangList;
        }

        /// <summary>
        ///  获取角色
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>角色列表</returns>
        public IList<Model.ORG.Role> GetRole(int roleID, int flag)
        {
            string strSql = "SELECT RoleID,DataRangeID,Name,Value,OrderValue,Descrption,IsSYSDBA FROM Per_Role";
            if (flag == (int)RoleCombobox.精确查询)
            {
                strSql = strSql + " WHERE RoleID = @RoleID";
            }
            else
            {
                if (roleID <= Convert.ToInt32(RoleValue.总经理))
                {
                    strSql = strSql + " WHERE RoleID >= @RoleID";
                }
                else
                {
                    strSql = strSql + " WHERE RoleID >= @RoleID and RoleID <= 8";// 只表示业务相关的
                }
            }
            SqlParameter[] para =
                {
                     new SqlParameter("@RoleID",SqlDbType.Int)
                };
            para[0].Value = roleID;
            DataTable dt = DbHelperSQL.Query(strSql, para).Tables[0];

            if (DataTableTools.DataTableIsNull(dt)) return null;

            IList<Model.ORG.Role> roleList = ConvertToList.DataTableToList<Model.ORG.Role>(dt);

            return roleList;
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        public IList<Model.ORG.Department> GetAllDepartment()
        {
            string strSql = "SELECT * FROM Org_Department ";
            DataTable dt = DbHelperSQL.Query(strSql).Tables[0];
            if (DataTableTools.DataTableIsNull(dt)) return null;

            IList<Model.ORG.Department> departmentList = ConvertToList.DataTableToList<Model.ORG.Department>(dt);

            return departmentList;
        }

        /// <summary>
        /// 获取职位列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<Model.ORG.JobInfo> GetJobList(int pageIndex, int pageSize, string Name, string Value)
        {
            Model.Common.GetPageInfo info = new Model.Common.GetPageInfo();

            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Name))
            {
                sb.AppendLine("and Name like @Name");
                listPara.Add(new SqlParameter("@Name", "%" + Name + "%"));
            }
            if (!string.IsNullOrEmpty(Value))
            {
                sb.AppendLine("and Value like @Value");
                listPara.Add(new SqlParameter("@Value", "%" + Value + "%"));
            }
            // 表名
            info.tabName = " v_Org_Job";
            // 主键
            info.pkColumn = "JobID";
            // 表示项目
            info.showColumn = "*";
            // 排序条件
            info.ascColumn = "JobID DESC";
            // 条件
            info.where = sb.ToString();
            // 排序 (0为升序,1为降序)
            info.intOrderType = 1;
            // 页码
            info.pageIndex = pageIndex;
            // 页面显示的数据数量
            info.pageSize = pageSize;

            string strSql = Common.GetPageResult(info);

            DataTable dt = DbHelperSQL.Query(strSql, listPara.ToArray()).Tables[0];
            return ConvertToList.DataTableToList<Model.ORG.JobInfo>(dt);

        }
        /// <summary>
        /// 获取业务的数据数量
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public int GetJobCount(string Name, string Value)
        {
            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Name))
            {
                sb.AppendLine("and Name like @Name");
                listPara.Add(new SqlParameter("@Name", "%" + Name + "%"));
            }
            if (!string.IsNullOrEmpty(Value))
            {
                sb.AppendLine("and Value like @Value");
                listPara.Add(new SqlParameter("@Value", "%" + Value + "%"));
            }
            return Common.GetRowsCount("Org_Job", sb.ToString(), listPara.ToArray());
        }

        /// <summary>
        /// 获取工作所属
        /// </summary>
        /// <returns></returns>
        public IList<Model.ORG.JobInfo> GetJobDepartment()
        {
            string sql = "select jobid,name  from Org_Job ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return ConvertToList.DataTableToList<Model.ORG.JobInfo>(dt);
        }


        //修改和添加
        public bool EditJob(string UpdateID, ParaStruct.JobPage job)
        {
            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(job.jobName))
            {
                listPara.Add(new SqlParameter("@Name", job.jobName));
            }
            if (!string.IsNullOrEmpty(job.jobValue))
            {
                int value = Convert.ToInt32(job.jobValue);
                listPara.Add(new SqlParameter("@Value", job.jobValue));
            }
            if (string.IsNullOrEmpty(job.department))
            {
                job.department = null;
                listPara.Add(new SqlParameter("@perjobid",job.department));
            }
            else
            {
                listPara.Add(new SqlParameter("@perjobid", job.department));
            }

            listPara.Add(new SqlParameter("@IsSYSDBA", job.IsSYSDBA));

            if (UpdateID == "true")
            {
                sb.AppendLine("insert into Org_Job(JobID, Name, Value, OrderValue,perjobid ,IsSYSDBA) values ((select( select max(JobID) from Org_Job) +1),@Name,@Value,null,@perjobid,@IsSYSDBA)");
                int count = DbHelperSQL.ExecuteSql(sb.ToString(), listPara.ToArray());
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                sb.AppendLine("UPDATE Org_Job SET Name=@Name,Value=@Value ,perjobid=@perjobid  where  jobid=@jobid");
                int id = Convert.ToInt32(UpdateID);
                listPara.Add(new SqlParameter("@jobid", id));
                int count = DbHelperSQL.ExecuteSql(sb.ToString(), listPara.ToArray());
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        public bool DeleteJob(string deleteID)
        {

            return Common.DeleteRecordByPK("Org_Job", "JobID", deleteID);

        }

        #endregion

    }
}
