using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.Model.Parameter;
using System.Data.SqlClient;
using RunTecMs.Common.DBUtility;
using System.Data;
using RunTecMs.Common.ConvertUtility;
using RunTecMs.RunIDAL.Organizations;

namespace RunTecMs.RunDAL.Organizations
{
    public class Permission : IPermission
    {

        #region 数据范围
        /// <summary>
        /// 获取数据范围列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public IList<Model.ORG.Per_DataRange> GetDataRangeList(int pageIndex, int pageSize, string Name, string Value)
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
            info.tabName = " Per_DataRange";
            // 主键
            info.pkColumn = "DataRangeID";
            // 表示项目
            info.showColumn = "*";
            // 排序条件
            info.ascColumn = "DataRangeID asc";
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
            return ConvertToList.DataTableToList<Model.ORG.Per_DataRange>(dt);

        }
        /// <summary>
        /// 获取数据范围个数
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public int GetDataRangeCount(string Name, string Value)
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
            return Common.GetRowsCount("Per_DataRange", sb.ToString(), listPara.ToArray());
        }
        /// <summary>
        /// 修改和添加
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="DataRange"></param>
        /// <returns></returns>
        public bool EditDataRange(string UpdateID, ParaStruct.DataRange DataRange)
        {
            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            listPara.Add(new SqlParameter("@Name", DataRange.Name));
            listPara.Add(new SqlParameter("@Value", DataRange.Value));
            listPara.Add(new SqlParameter("@DataRange", DataRange.DialogDataRange));
            listPara.Add(new SqlParameter("@IsSYSDBA", DataRange.IsSYSDBA));
            if (UpdateID == "true")
            {
                sb.AppendLine("insert into Per_DataRange (DataRangeID, Name, Value, OrderValue, DataRange, IsSYSDBA) values ((select( select max(DataRangeID) from Per_DataRange) +1),@Name,@Value,null,@DataRange,@IsSYSDBA)");

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
                sb.AppendLine("update  Per_DataRange  set Name=@Name,Value=@Value,DataRange=@DataRange  where   DataRangeID=@DataRangeID");
                int id = Convert.ToInt32(UpdateID);
                listPara.Add(new SqlParameter("@DataRangeID", id));
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
        /// 删除数据范围数据
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        public bool DeleteDataRange(string deleteID)
        {

            return Common.DeleteRecordByPK("Per_DataRange", "DataRangeID", deleteID);

        }
        #endregion

        #region 角色

        /// <summary>
        /// 获得角色列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public IList<Model.ORG.RoleInfo> GetRoleList(int pageIndex, int pageSize, string Name, string Value)
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
            info.tabName = " V_Per_Role";
            // 主键
            info.pkColumn = "RoleID";
            // 表示项目
            info.showColumn = "*";
            // 排序条件
            info.ascColumn = "RoleID asc";
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
            return ConvertToList.DataTableToList<Model.ORG.RoleInfo>(dt);

        }
        /// <summary>
        /// 获取角色数据个数
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public int GetRoleCount(string Name, string Value)
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
            return Common.GetRowsCount("V_Per_Role", sb.ToString(), listPara.ToArray());
        }
        /// <summary>
        /// 角色数据范围
        /// </summary>
        /// <returns></returns>
        public IList<Model.ORG.DataRang> GetRoleDataRang()
        {
            string sql = "select DataRangeid,Name  from Per_DataRange ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return ConvertToList.DataTableToList<Model.ORG.DataRang>(dt);
        }
        /// <summary>
        /// 修改和添加
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="DataRangeID"></param>
        /// <param name="Descrption"></param>
        /// <returns></returns>
        public bool EditRole(string UpdateID, ParaStruct.RolePage Role)
        {
            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            listPara.Add(new SqlParameter("@Name", Role.Name));
            listPara.Add(new SqlParameter("@Value", Role.Value));
            listPara.Add(new SqlParameter("@DataRangeID", Role.RoleDataRange));
            listPara.Add(new SqlParameter("@Descrption", Role.Descrption));
            listPara.Add(new SqlParameter("@IsSYSDBA", Role.IsSYSDBA));

            if (UpdateID == "true")
            {
                sb.AppendLine("insert into Per_Role (RoleID, DataRangeID, Name, Value, OrderValue, Descrption, IsSYSDBA) values((select( select max(roleid) from Per_Role) +1),@DataRangeID,@Name,@Value,(select( select max(ordervalue) from Per_Role) +1),@Descrption,@IsSYSDBA)");
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
                sb.AppendLine("update  Per_Role  set Name=@Name,Value=@Value,DataRangeID=@DataRangeID,Descrption=@Descrption  where   RoleID=@RoleID");
                int id = Convert.ToInt32(UpdateID);
                listPara.Add(new SqlParameter("@RoleID", id));
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
        /// 删除角色数据
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        public bool DeleteRole(string deleteID)
        {
            return Common.DeleteRecordByPK("Per_Role", "RoleID", deleteID);
        }

        #endregion

        #region 模板定义
        /// <summary>
        ///获取模板列表数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="SearchEnabled"></param>
        /// <returns></returns>
        public IList<Model.SYS.Module> GetModuleList(int pageIndex, int pageSize, string Name, string Value, string SearchEnabled)
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
            if (!string.IsNullOrEmpty(SearchEnabled) && SearchEnabled != "0")
            {
                if (SearchEnabled == "2")
                {
                    SearchEnabled = "0";
                }
                sb.AppendLine("and Enabled =@Enabled");
                listPara.Add(new SqlParameter("@Enabled", SearchEnabled));
            }

            // 表名
            info.tabName = "Per_Module";
            // 主键
            info.pkColumn = "ModuleID";
            // 表示项目
            info.showColumn = "*";
            // 排序条件
            info.ascColumn = "ModuleID asc";
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
            return ConvertToList.DataTableToList<Model.SYS.Module>(dt);

        }

        /// <summary>
        /// 得到模板数据个数
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="SearchEnabled"></param>
        /// <returns></returns>
        public int GetModuleCount(string Name, string Value, string SearchEnabled)
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

            if (!string.IsNullOrEmpty(SearchEnabled) && SearchEnabled != "0")
            {
                if (SearchEnabled == "2")
                {
                    SearchEnabled = "0";
                }
                sb.AppendLine("and Enabled =@Enabled");
                listPara.Add(new SqlParameter("@Enabled", SearchEnabled));
            }
            return Common.GetRowsCount("Per_Module", sb.ToString(), listPara.ToArray());
        }

        /// <summary>
        ///查询父级名称
        /// </summary>
        /// <param name="ParentModuleID"></param>
        /// <returns></returns>
        public string SearchParentName(string ParentModuleID)
        {
            string sql = " select  name from  Per_Module  where moduleid=@ParentModuleID";

            SqlParameter[] para =
                {
                     new SqlParameter("@ParentModuleID",SqlDbType.Char)
                };
            para[0].Value = ParentModuleID;

            object ob = DbHelperSQL.GetSingle(sql, para);

            string ss = Convert.ToString(ob);
            return ss;

        }
        /// <summary>
        /// * 获取菜单的树的方法*
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <returns></returns>
        public List<Model.SYS.TreeModule> GetSubNodes(string parentNodeId)
        {
            string sql = "select * from  Per_Module ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            List<Model.SYS.TreeModule> Tree = new List<Model.SYS.TreeModule>();
            Model.SYS.TreeModule TM = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] rows = dt.Select("ParentModuleID ='" + parentNodeId + "'");

                foreach (DataRow item in rows)
                {
                    string id = item["ModuleID"].ToString();
                    string text = item["Name"].ToString();
                    TM = new Model.SYS.TreeModule();

                    DataRow[] IsNulRows = dt.Select("ParentModuleID ='" + id + "'");

                    if (IsNulRows.Length > 0)
                    {
                        TM.state = "closed";
                    }

                    TM.id = id;
                    TM.text = id + " " + text;

                    Tree.Add(TM);
                }
            }
            return Tree;
        }
        /// <summary>
        /// 验证模板ID
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool SameModuleIDNum(string ModuleID)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.AppendLine(" select count(0) from Per_Module");
            strsql.AppendLine(" where ModuleID=@ModuleID");
            SqlParameter[] para = { new SqlParameter("@ModuleID", SqlDbType.VarChar, 10) };
            para[0].Value = ModuleID;
            return DbHelperSQL.Exists(strsql.ToString(), para);
        }


        //修改和添加
        public bool EditModule(string UpdateID, ParaStruct.ModulePage ModulePage)
        {
            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();

            listPara.Add(new SqlParameter("@ModuleID", ModulePage.ModuleID));
            listPara.Add(new SqlParameter("@Name", ModulePage.Name));
            listPara.Add(new SqlParameter("@Value", ModulePage.Value));
            listPara.Add(new SqlParameter("@ParentModuleID", ModulePage.ParentModuleID));
            listPara.Add(new SqlParameter("@Description", ModulePage.Description));
            listPara.Add(new SqlParameter("@Path", ModulePage.Path));
            listPara.Add(new SqlParameter("@IsSYSDBA", ModulePage.IsSYSDBA));
            if (ModulePage.Enabled == "2")
            {
                ModulePage.Enabled = "0";
            }
            listPara.Add(new SqlParameter("@Enabled", ModulePage.Enabled));
            if (UpdateID == "true")
            {
                sb.AppendLine("insert into Per_Module(ModuleID, ParentModuleID, Name, Value, OrderValue, Path, Description, Enabled, AvailableOpIDs, IsSYSDBA) values(@ModuleID,@ParentModuleID,@Name,@Value,null,@Path,@Description,@Enabled,null,@IsSYSDBA)");
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
                sb.AppendLine("update  Per_Module  set ParentModuleID=@ParentModuleID,Name=@Name,Value=@Value, Path=@Path,Description=@Description,Enabled=@Enabled where   ModuleID=@ModuleID1");
                listPara.Add(new SqlParameter("@ModuleID1", UpdateID));
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
        /// 删除模板
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        public bool DeleteModule(string deleteID)
        {

            return Common.DeleteRecordByPK("Per_Module", "ModuleID", deleteID);

        }


        #endregion

        #region  权限集

        /// <summary>
        ///获取权限集列表
        /// </summary>
        /// <returns></returns>
        public IList<Model.ORG.PermissionSetInfo> GetPermissionSetList(int pageIndex, int pageSize, ParaStruct.PermissionSet Permission)
        {
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();
            sb.AppendLine("select  * from (SELECT  row_number()over(order by a.RoleID asc )as row_num,");
            sb.AppendLine(" a.RoleID,a.BusinessValue,ManagerTypeID,b.Name RoleName,c.BusinessName,");
            sb.AppendLine(" CASE WHEN  ManagerTypeID=1 THEN '管理后台' ELSE '商户后台' END ManagerName,a.IsSYSDBA");
            sb.AppendLine(" FROM  Per_PermissionSet  a");
            sb.AppendLine(" LEFT  JOIN   Per_Role b  ON   a.RoleID=b.RoleID ");
            sb.AppendLine(" LEFT JOIN CRM_Business c ON  a.BusinessValue=c.BusinessValue where 1=1");

            Dictionary<string, List<SqlParameter>> SqlParam = ReturnSqlParam(Permission);
            foreach (var item in SqlParam)
            {
                sb.AppendLine(item.Key);
                listPara = item.Value;
            }
            sb.AppendLine(" GROUP  BY  a.RoleID,a.BusinessValue,ManagerTypeID,b.Name,c.BusinessName,a.IsSYSDBA");
            sb.AppendLine(")PermissionSet ");
            sb.AppendFormat("where PermissionSet.row_num between {0} and {1} ", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
            DataTable dt = DbHelperSQL.Query(sb.ToString(), listPara.ToArray()).Tables[0];
            return ConvertToList.DataTableToList<Model.ORG.PermissionSetInfo>(dt);
        }
        /// <summary>
        /// 获取权限集个数
        /// </summary>
        /// <returns></returns>
        public int GetPermissionSetCount(ParaStruct.PermissionSet Permission)
        {
            StringBuilder sb = new StringBuilder(); ;
            List<SqlParameter> listPara = new List<SqlParameter>();
            sb.AppendLine("select count(0) from  (select ");
            sb.AppendLine(" a.RoleID,a.BusinessValue,ManagerTypeID,b.Name RoleName,c.BusinessName,");
            sb.AppendLine(" CASE WHEN  ManagerTypeID=1 THEN '管理后台' ELSE '商户后台' END ManagerName");
            sb.AppendLine(" FROM  Per_PermissionSet  a");
            sb.AppendLine(" LEFT  JOIN   Per_Role b  ON   a.RoleID=b.RoleID ");
            sb.AppendLine(" LEFT JOIN CRM_Business c ON  a.BusinessValue=c.BusinessValue  where 1=1");

            Dictionary<string, List<SqlParameter>> SqlParam = ReturnSqlParam(Permission);
            foreach (var item in SqlParam)
            {
                sb.AppendLine(item.Key);
                listPara = item.Value;
            }
            sb.AppendLine(" GROUP  BY  a.RoleID,a.BusinessValue,ManagerTypeID,b.Name,c.BusinessName) PermissionSet");
            object obj = DbHelperSQL.GetSingle(sb.ToString(), listPara.ToArray());
            return obj != null ? Convert.ToInt32(obj) : 0;
        }

        /// <summary>
        /// 返回条件参数
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, List<SqlParameter>> ReturnSqlParam(ParaStruct.PermissionSet Permission)
        {
            Dictionary<string, List<SqlParameter>> SqlParam = new Dictionary<string, List<SqlParameter>>();
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Permission.BusinessName))
            {
                sb.AppendLine("and c.BusinessName  like @BusinessName");
                listPara.Add(new SqlParameter("@BusinessName", "%" + Permission.BusinessName + "%"));
            }
            if (!string.IsNullOrEmpty(Permission.RoleName))
            {
                sb.AppendLine("and b.Name like @RoleName");
                listPara.Add(new SqlParameter("@RoleName", "%" + Permission.RoleName + "%"));
            }
            SqlParam.Add(sb.ToString(), listPara);
            return SqlParam;
        }
        /// <summary>
        /// 获取权限集
        /// </summary>
        /// <returns></returns>
        public List<string> GetPermissionSetView(string param)
        {
            string[] str = param.Split(',');
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();
            sb.AppendLine("SELECT  ModuleID,ManagerTypeID ");
            sb.AppendLine("  FROM  Per_PermissionSet ");
            sb.AppendLine(" WHERE  BusinessValue=@BusinessValue AND ManagerTypeID=@ManagerTypeID AND RoleID=@RoleID");
            listPara.Add(new SqlParameter("@RoleID", str[0]));
            listPara.Add(new SqlParameter("@BusinessValue", str[1]));
            listPara.Add(new SqlParameter("@ManagerTypeID ", str[2]));
            DataTable dt = DbHelperSQL.Query(sb.ToString(), listPara.ToArray()).Tables[0];
            List<string> list = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(item["ModuleID"].ToString() + "-" + item["ManagerTypeID"].ToString());
            }
            return list;
        }

        /// <summary>
        /// * 获取菜单的树的方法*
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <returns></returns>
        public List<Model.SYS.TreeNode> GetPermissionSetTree(string parentNodeId, string param)
        {
            string[] str = param.Split(',');
            List<string> list = new List<string>();

            if (param != "isadd")
            {
                list = GetPermissionSetView(param);
            }
            List<SqlParameter> listPara = new List<SqlParameter>();
            string sql = "select * from  Per_Module";
            if (!string.IsNullOrEmpty(str[2]))
            {
                sql = sql + " WHERE ManagerTypeID = @ManagerTypeID";
                listPara.Add(new SqlParameter("@ManagerTypeID ", str[2]));
            }
            DataTable dt = DbHelperSQL.Query(sql, listPara.ToArray()).Tables[0];
            List<Model.SYS.Module> Module = ConvertToList.DataTableToList<Model.SYS.Module>(dt).ToList();
            List<Model.SYS.TreeNode> Tree = new List<Model.SYS.TreeNode>();
            LoadTreeNode(Module, Tree, list, 0);

            return Tree;
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="Module"></param>
        /// <param name="Tree"></param>
        /// <param name="list"></param>
        /// <param name="pid"></param>
        public void LoadTreeNode(List<Model.SYS.Module> Module, List<Model.SYS.TreeNode> Tree, List<string> list, int pid)
        {
            foreach (var permission in Module)
            {
                //如果组织父id=参数
                if (permission.ParentModuleID == pid)
                {
                    Model.SYS.TreeNode node = new Model.SYS.TreeNode
                    {
                        id = permission.ModuleID,
                        text = permission.ModuleID + " " + permission.Name,
                        iconCls = "icon-blank",
                        children = new List<Model.SYS.TreeNode>()
                    };
                    if (Module.Where(m => m.ParentModuleID == node.id).Count() > 0)
                    {
                        node.state = "closed";
                    }
                    if (list.Contains(Convert.ToString(node.id) + "-" + Convert.ToString(permission.ManagerTypeID)))
                    {
                        node.Checked = true;
                    }
                    else
                    {
                        node.Checked = false;
                    }
                    //将节点 加入到 树节点集合
                    Tree.Add(node);
                    //递归 为这个新创建的 树节点找 子节点
                    LoadTreeNode(Module, node.children, list, node.id);
                }
            }
        }


        /// <summary>
        /// 编辑权限集
        /// </summary>
        /// <returns></returns>
        public bool EditAddPermissionSet(string flag, ParaStruct.PermissionSet Permission)
        {
            bool ReturnValue = false;
            //当选择为添加的时候
            if (flag == "isadd")
            {

                string[] str = Permission.NewModuleID.Split(new char[] { ',' });
                for (int i = 0; i < str.Length; i++)
                {
                    Permission.ModuleManagerTypeID = str[i];
                    ReturnValue = AddPermissionSet(Permission);
                }
            }
            ////当选择为编辑时
            else
            {

                string[] OldID = Permission.OldModuleID.Split(new char[] { ',' });

                string[] NewID = Permission.NewModuleID.Split(new char[] { ',' });
                //做添加
                var add = NewID.Where(c => !OldID.Contains(c)).ToArray();

                for (int i = 0; i < add.Length; i++)
                {
                    Permission.ModuleManagerTypeID = add[i];
                    ReturnValue = AddPermissionSet(Permission);
                }
                //做删除
                var del = OldID.Where(c => !NewID.Contains(c)).ToArray();
                for (int i = 0; i < del.Length; i++)
                {
                    Permission.ModuleManagerTypeID = del[i];
                    ReturnValue = DeletePermissionSet(Permission);
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// 添加操作（权限集）
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="Permission"></param>
        /// <returns></returns>
        public bool AddPermissionSet(ParaStruct.PermissionSet Permission)
        {
            int ReturnValue = 0;
            List<SqlParameter> listPara1 = new List<SqlParameter>();
            StringBuilder sb1 = new StringBuilder();
            string[] str = Permission.ModuleManagerTypeID.Split(new char[] { '-' });

            sb1.AppendLine("SELECT  Name FROM  Per_Module  WHERE   ManagerTypeID=@ManagerTypeID  AND  ModuleID=@ModuleID");

            listPara1.Add(new SqlParameter("@ModuleID", str[0]));
            listPara1.Add(new SqlParameter("@ManagerTypeID", str[1]));

            string ModuleName = DbHelperSQL.GetSingle(sb1.ToString(), listPara1.ToArray()).ToString();

            // 设置权限ID（后台区分（1位）+业务(2位)+角色(2位)+部门(2位)+菜单顺序(3位)）
            string PermissionID = "";
            PermissionID = PermissionID + Permission.ManagerTypeID;
            PermissionID = PermissionID + Permission.BusinessValue.PadLeft(2, '0');
            PermissionID = PermissionID + Permission.RoleID.PadLeft(2, '0');
            PermissionID = PermissionID + Permission.DepID.PadLeft(2, '0');
            // 取得最大权限及最小权限
            string MinPermissionID = PermissionID + "000";
            string MaxPermissionID = PermissionID + "999";

            StringBuilder sb2 = new StringBuilder();
            List<SqlParameter> listPara2 = new List<SqlParameter>();
            listPara2.Add(new SqlParameter("@Permission", PermissionID));
            listPara2.Add(new SqlParameter("@MinPermissionID", MinPermissionID));
            listPara2.Add(new SqlParameter("@MaxPermissionID", MaxPermissionID));
            listPara2.Add(new SqlParameter("@ModuleID", str[0]));
            listPara2.Add(new SqlParameter("@RoleID", Permission.RoleID));
            listPara2.Add(new SqlParameter("@BusinessValue", Permission.BusinessValue));
            listPara2.Add(new SqlParameter("@PermissionName", ModuleName));
            listPara2.Add(new SqlParameter("@ManagerTypeID", Permission.ManagerTypeID));
            listPara2.Add(new SqlParameter("@IsSYSDBA", Permission.IsSYSDBA));
            sb2.AppendLine("INSERT INTO Per_PermissionSet(PermissionID, ModuleID, RoleID, BusinessValue, PermissionName, ManagerTypeID, OpIDs, IsSYSDBA)");
            sb2.AppendLine("VALUES");
            sb2.AppendLine("((SELECT ISNULL(MAX(PermissionID), @Permission + '000') + 1 FROM Per_PermissionSet WHERE PermissionID >= @MinPermissionID AND PermissionID <= @MaxPermissionID),");
            sb2.AppendLine("@ModuleID,@RoleID,@BusinessValue,@PermissionName,@ManagerTypeID,'1',@IsSYSDBA)");

            ReturnValue = DbHelperSQL.ExecuteSql(sb2.ToString(), listPara2.ToArray());

            return ReturnValue > 0;
        }

        /// <summary>
        /// 删除操作（权限集）
        /// </summary>
        /// <returns></returns>
        public bool DeletePermissionSet(ParaStruct.PermissionSet Permission)
        {
            int count = 0;
            List<SqlParameter> listPara = new List<SqlParameter>();
            string[] str = Permission.ModuleManagerTypeID.Split(new char[] { '-' });

            listPara.Add(new SqlParameter("@ModuleID ", str[0]));
            listPara.Add(new SqlParameter("@BusinessValue", Permission.BusinessValue));
            listPara.Add(new SqlParameter("@RoleID", Permission.RoleID));
            listPara.Add(new SqlParameter("@ManagerTypeID", Permission.ManagerTypeID));
            string Sql = "DELETE Per_PermissionSet WHERE  ManagerTypeID =@ManagerTypeID AND RoleID=@RoleID  AND BusinessValue=@BusinessValue AND  ModuleID= @ModuleID ";

            count = DbHelperSQL.ExecuteSql(Sql, listPara.ToArray());
            return count > 0;
        }
        /// <summary>
        /// 删除权限集列表
        /// </summary>
        /// <param name="Permission"></param>
        /// <returns></returns>
        public bool DeletePermissionSetList(ParaStruct.PermissionSet Permission)
        {
            int count = 0;
            List<SqlParameter> listPara = new List<SqlParameter>();
            listPara.Add(new SqlParameter("@BusinessValue", Permission.BusinessValue));
            listPara.Add(new SqlParameter("@RoleID", Permission.RoleID));
            listPara.Add(new SqlParameter("@ManagerTypeID", Permission.ManagerTypeID));
            string Sql = "DELETE Per_PermissionSet WHERE  ManagerTypeID =@ManagerTypeID AND RoleID=@RoleID  AND BusinessValue=@BusinessValue ";

            count = DbHelperSQL.ExecuteSql(Sql, listPara.ToArray());
            return count > 0;

        }

        #endregion

    }
}

