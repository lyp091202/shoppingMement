using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RunTecMs.RunIDAL.Organizations;
using System.Data.SqlClient;
using RunTecMs.Model.EnumType;
using RunTecMs.Common.DBUtility;
using RunTecMs.Common.ConvertUtility;

namespace RunTecMs.RunDAL.Organizations
{
    /// <summary>
    /// 查找相应层级时用到了菜单模型ID的命名方式。
    /// 如果方式变更，下面的层级也需要变更
    /// 菜单模型ID的命名方式如下：
    ///    现阶段从导航到最底层菜单用了八位数(XXXXXXXX)表示，每一位代表相应的层级，
    ///    每位具体意义如下（从左到右顺序）：
    ///    第1-2位：代表导航栏
    ///    第3-4位：代表左侧菜单栏
    ///    第5-6位：代表菜单栏下面的分组（不一定都有，没有场合用0补充，无页面）
    ///    第7-8位：代表真正页面
    /// </summary>
    public class Module : IModule<Model.SYS.Module>
    {
        /// <summary>
        /// 获取菜单 
        /// </summary>
        /// <param name="parentId">父级模块(0:获取父级导航 其他：指定导航下菜单)</param>
        /// <param name="LoginUserId">登录者ID</param>
        /// <param name="ManagerTypeID">后台商户类型ID（1：管理后台 2：商户管理后台）</param>
        /// <returns></returns>
        public IList<Model.SYS.Module> GetMenu(int parentId, int LoginUserId, int ManagerTypeID)
        {
            // 取得全部直属菜单
            IList<Model.SYS.Module> allMenu = GetAllMenu(parentId, ManagerTypeID);

            // 获取权限模型集合
            string modules = GetPermissionModules(LoginUserId, ManagerTypeID);
            string[] module = modules.Split(',');
            if (module.Contains(Convert.ToString(parentId)))
            {
                return allMenu;
            }
            else
            {
                return GetShowModule(allMenu, modules, parentId);
            }
        }

        /// <summary>
        /// 获取导航栏下的子菜单
        /// </summary>
        /// <param name="moduleId">模型ID</param>
        /// <param name="LoginUserId">登录者ID</param>
        /// <param name="ManagerTypeID">后台商户类型ID（1：管理后台 2：商户管理后台）</param>
        /// <returns></returns>
        public IList<Model.SYS.Module> GetAllChildMenuList(int moduleId, int LoginUserId, int ManagerTypeID)
        {
            IList<Model.SYS.Module> showChildMenu = new List<Model.SYS.Module>();
            IList<Model.SYS.Module> childMenu = new List<Model.SYS.Module>();
            // 获取所有子菜单
            IList<Model.SYS.Module> allChildMenu = GetChildMenuList(moduleId, ManagerTypeID);

            // 取得登录者所有模型
            string modules = GetPermissionModules(LoginUserId, ManagerTypeID);
            // 登录者模型中存在该模型ID的场合，返回所有子菜单
            if (modules.Contains(Convert.ToString(moduleId)))
            {
                return allChildMenu;
            }
            else
            {
                // 父菜单获取
                string strParentId = Convert.ToString(moduleId);
                // 导航栏取得
                // 如果已经包含导航栏则返回所有子菜单
                if (modules.Contains(GetParentId(strParentId)))
                {
                    return allChildMenu;
                }

                // 如果只有最底层子集菜单则查询
                IList<string> MenuList = new List<string>();
                for (int i = 0; i < allChildMenu.Count; i++)
                {
                    string childmoduleId = Convert.ToString(allChildMenu[i].ModuleID);
                    if (modules.Contains(childmoduleId))
                    {
                        // 最底层菜单场合，关联上级菜单及本菜单
                        if (!childmoduleId.EndsWith("00"))
                        {
                            // 获取父级菜单
                            string child = childmoduleId.Substring(0, childmoduleId.Length - 2) + "00";
                            // 追加最底层菜单的父菜单
                            if (!MenuList.Contains(child))
                            {
                                showChildMenu.Add(GetChildMenuList(Convert.ToInt32(child), ManagerTypeID)[0]);
                                MenuList.Add(child);
                            }
                            // 追加最后菜单
                            showChildMenu.Add(GetChildMenuList(Convert.ToInt32(childmoduleId), ManagerTypeID)[0]);
                            MenuList.Add(childmoduleId);
                        }
                        else
                        {
                            // 不是最底层菜单场合，直接关联下面所有的菜单
                            childMenu = GetChildMenuList(allChildMenu[i].ModuleID, ManagerTypeID);
                            for (int j = 0; j < childMenu.Count; j++)
                            {
                                showChildMenu.Add(childMenu[j]);
                            }
                        }
                    }
                }

                return showChildMenu;
            }
        }

        /// <summary>
        /// 获取指定员工/商户的模型集合
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="ManagerTypeID">后台管理类型</param>
        /// <returns></returns>
        public static string GetPermissionModules(int SearchID, int ManagerTypeID)
        {
            // 模型集合
            string modules = "";
            string moduleId = "";
            IList<Model.ORG.Permission> permissionList = null;
            // 总管理后台场合
            if (ManagerTypeID == (int)ManagerType.管理后台)
            {
                permissionList = GetPermissionSet(SearchID, 1);
            }
            else if (ManagerTypeID == (int)ManagerType.第三方商户)
            {
                // 商户管理后台场合
                permissionList = GetMerchantPermissionSet(SearchID, 0);
            }

            if (permissionList == null)
            {
                return modules;
            }
            for (int i = 0; i < permissionList.Count; i++)
            {
                moduleId = Convert.ToString(permissionList[i].ModuleID);
                // 判断是否有重复模型ID
                if (!modules.Contains(moduleId))
                {
                    modules = modules + permissionList[i].ModuleID;
                    modules = modules + ",";
                }
            }
            if (modules.EndsWith(","))
            {
                modules = modules.Substring(0, modules.Length - 1);
            }

            return modules;
        }

        #region 私有方法
        /// <summary>
        /// 获取指定父菜单下的直属菜单
        /// </summary>
        /// <returns></returns>
        private IList<Model.SYS.Module> GetAllMenu(int parentId, int ManagerTypeID)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("SELECT * FROM SYS_Module ");
            strSql.AppendLine(" WHERE ParentModuleID = @parentId ");
            strSql.AppendLine("   AND Enabled = 1 ");
            strSql.AppendLine("   AND ManagerTypeID = @ManagerTypeID ");
            strSql.AppendLine(" ORDER BY OrderValue");
            paraList.Add(new SqlParameter("@parentId", parentId));
            paraList.Add(new SqlParameter("@ManagerTypeID", ManagerTypeID));

            DataTable dt = DbHelperSQL.Query(strSql.ToString(), paraList.ToArray()).Tables[0];
            return ConvertToList.DataTableToList<Model.SYS.Module>(dt);
        }


        /// <summary>
        /// 取得顶级菜单ID
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        private string GetParentId(string strId)
        {
            string parent = "";
            if (strId.Length == 7)
            {
                parent = strId.Substring(0, 1) + "000000";
            }
            else if (strId.Length == 8)
            {
                parent = strId.Substring(0, 2) + "000000";
            }
            else
            {
                parent = "00000000";
            }
            return parent;
        }

        /// <summary>
        /// 获取导航栏下的所有子菜单
        /// </summary>
        /// <param name="moduleId">模型ID</param>
        /// <param name="ManagerTypeID">后台商户类型ID（1：管理后台 2：商户管理后台）</param>
        /// <returns></returns>
        private IList<Model.SYS.Module> GetChildMenuList(int moduleId, int ManagerTypeID)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            paraList.Add(new SqlParameter("@moduleId", moduleId));
            paraList.Add(new SqlParameter("@ManagerTypeID", ManagerTypeID));

            DataTable dt = DbHelperSQL.RunProcedure("sp_BackGetPerModule", paraList.ToArray(), "module").Tables[0];
            return ConvertToList.DataTableToList<Model.SYS.Module>(dt);
        }

        /// <summary>
        /// 取得表示模型
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="LoginUserId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private IList<Model.SYS.Module> GetShowModule(IList<Model.SYS.Module> dataList, string modules, int parentId)
        {
            IList<Model.SYS.Module> showModule = new List<Model.SYS.Module>();

            if (string.IsNullOrWhiteSpace(modules))
            {
                return showModule;
            }
            // 取得登录者权限模型集
            string[] module = modules.Split(',');
            IList<string> moduleList = new List<string>();

            // 头部导航时
            if (parentId == 0)
            {
                moduleList = module.ToList();
            }
            else
            {
                // 内部菜单时
                for (int i = 0; i < module.Length; i++)
                {
                    if (Convert.ToString(parentId).Equals(GetParentId(module[i])))
                    {
                        moduleList.Add(module[i]);
                    }
                }

            }

            // 取得登录者所有模型中导航栏
            for (int i = 0; i < dataList.Count; i++)
            {
                for (int j = 0; j < moduleList.Count; j++)
                {
                    // 最后一条以外
                    if (i != dataList.Count - 1)
                    {
                        // 通常状态下，判断权限模型，是否在所有模型范围内，在范围内的，追加到表示序列中
                        if ((Convert.ToInt32(moduleList[j]) >= dataList[i].ModuleID) && (Convert.ToInt32(moduleList[j]) < dataList[i + 1].ModuleID))
                        {
                            if (!showModule.Contains(dataList[i]))
                            {
                                showModule.Add(dataList[i]);
                            }
                            break;
                        }
                    }
                    else
                    {
                        // 全部模型的最后一条时，判断权限模型，是否大于全部模型的最后一条，在范围内的，追加到表示序列中
                        if ((Convert.ToInt32(moduleList[j]) >= dataList[i].ModuleID))
                        {
                            if (!showModule.Contains(dataList[i]))
                            {
                                showModule.Add(dataList[i]);
                            }
                            break;
                        }
                    }
                }
            }

            return showModule;
        }

        /// <summary>
        /// 通过员工ID获取模型集合
        /// </summary>
        /// <param name="EmployeeID">员工ID</param>
        /// <param name="OrderType">0：按角色升序 1:按公司部门角色升序 </param>
        /// <returns>true:成功，false:失败</returns>
        private static IList<Model.ORG.Permission> GetPermissionSet(int EmployeeID, int OrderType)
        {
            // 默认使用角色
            int getType = (int)PermissionGetType.按角色;
            // 获取权限集获取方式
            IList<Model.Common.Config> configList = Common.GetConfig(Convert.ToString((int)SAConfig.权限集获取方式));
            if (configList.Count > 0)
            {
                getType = Convert.ToInt32(configList[0].ConfigValue.Trim());
            }
            
            // SQL语句
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("SELECT EDR.CompanyID, EDR.DepID, EDR.RoleID, EDR.EmployeeID, PS.ModuleID, PS.OpIDs ");
            strSql.AppendLine("  FROM Org_EmployeeDepartmentRole EDR, Org_PermissionSet PS, Org_Employee  EmployeeModel");
            strSql.AppendLine(" WHERE PS.ManagerTypeID = 1 ");           // 管理后台
            
            strSql.AppendLine("   AND PS.BusinessValue = EDR.BusinessValue ");

            if (getType == (int)PermissionGetType.按角色)
            {
                strSql.AppendLine("   AND EDR.RoleID = PS.RoleID ");
            }
            else if (getType == (int)PermissionGetType.按部门)
            {
                strSql.AppendLine("   AND PS.DepID = EDR.DepID ");
            }

            strSql.AppendLine("   AND EDR.EmployeeID = EmployeeModel.EmployeeID ");
            strSql.AppendLine("   AND EmployeeModel.Active = 1 ");
            strSql.AppendLine("   AND EDR.EmployeeID = @EmployeeID ");
            if (OrderType == 0)
            {
                strSql.AppendLine("  ORDER BY EDR.RoleID ");
            }
            else if (OrderType == 1)
            {
                strSql.AppendLine("  ORDER BY EDR.CompanyID, EDR.DepID, EDR.RoleID ");
            }

            SqlParameter[] para =
            {
                 new SqlParameter("@EmployeeID",SqlDbType.Int)

            };

            para[0].Value = EmployeeID;

            DataTable dt = DbHelperSQL.Query(strSql.ToString(), para).Tables[0];
            return ConvertToList.DataTableToList<Model.ORG.Permission>(dt);
        }

        /// <summary>
        /// 通过商户ID获取模型集合
        /// </summary>
        /// <param name="MerchantID">商户ID</param>
        /// <param name="OrderType">0：按模型ID升序序 </param>
        /// <returns>true:成功，false:失败</returns>
        private static IList<Model.ORG.Permission> GetMerchantPermissionSet(int MerchantID, int OrderType)
        {
            // SQL语句
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("select Manager.MerchantID, PS.ModuleID, PS.OpIDs ");
            strSql.AppendLine(" from  Org_PermissionSet PS, SYS_MerchantManager  Manager");
            strSql.AppendLine(" WHERE PS.ManagerTypeID = 2 ");           // 商户管理后台
            strSql.AppendLine("   AND Manager.Isusing = 1 ");
            strSql.AppendLine("   AND Manager.MerchantID = @MerchantID ");
            strSql.AppendLine("   AND Manager.MerchantTypeID = PS.RoleID ");
            strSql.AppendLine("   AND PS.BusinessValue = Manager.BusinessValue ");
            if (OrderType == 0)
            {
                strSql.AppendLine("  ORDER BY PS.ModuleID ");
            }

            SqlParameter[] para =
            {
                 new SqlParameter("@MerchantID",SqlDbType.Int)
            };

            para[0].Value = MerchantID;

            DataTable dt = DbHelperSQL.Query(strSql.ToString(), para).Tables[0];
            return ConvertToList.DataTableToList<Model.ORG.Permission>(dt);
        }
        #endregion
    }
}
