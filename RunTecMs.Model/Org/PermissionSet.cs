using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.ORG
{

    /// <summary>
    /// 权限
    /// </summary>
    [Serializable]
    public class Permission
    {
        // 公司ID
        public int CompanyID { get; set; }

        // 部门ID
        public int DepID { get; set; }

        // 角色ID
        public int RoleID { get; set; }

        // 员工ID
        public int EmployeeID { get; set; }

        // 模型ID
        public int ModuleID { get; set; }

        // 操作ID集合
        public string OpIDs { get; set; }
    }

    public class PermissionSet
    {
       /// <summary>
        /// ID
       /// </summary>
       public int PermissionID { get; set; }
       /// <summary>
       /// 数据模型ID
       /// </summary>
       public int ModuleID { get; set; }
       /// <summary>
       /// 角色ID
       /// </summary>
       public int RoleID { get; set; }
        /// <summary>
        /// 业务id
        /// </summary>
       public int BusinessValue { get; set; }
       /// <summary>
       /// 权限名称
       /// </summary>
       public string PermissionName { get; set; }
       /// <summary>
       /// 后台管理区分
       /// </summary>
       public int   ManagerTypeID { get; set; }
       /// <summary>
       /// 操作ID集合
       /// </summary>
       public string  OpIDs { get; set; }
       /// <summary>
       /// 超管标识
       /// </summary>
       public bool IsSYSDBA { get; set; }
       /// <summary>
       /// 时间戳
       /// </summary>
       public byte[] timestamp { get; set; }

    }
    /// <summary>
    /// 权限集内容
    /// </summary>
   public class PermissionSetInfo:PermissionSet
   {
       /// <summary>
       /// 角色名称
       /// </summary>
       public string RoleName { get; set; }
       /// <summary>
       /// 业务名称
       /// </summary>
       public string BusinessName { get; set; }
       /// <summary>
       /// 管理名称
       /// </summary>
       public string ManagerName { get; set; }
   
   }
}
