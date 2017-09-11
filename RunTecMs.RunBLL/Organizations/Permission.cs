using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.RunIDAL.Organizations;
using RunTecMs.RunDALFactory;
using RunTecMs.Model.Parameter;

namespace RunTecMs.RunBLL.Organizations
{
    public class Permission
    {

        private static IPermission dal = DataAccess.CreatePermission();

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public IList<Model.ORG.RoleInfo> GetRoleList(int pageIndex, int pageSize, string Name, string Value)
        {
            return dal.GetRoleList(pageIndex, pageSize, Name, Value);
        }
        /// <summary>
        /// 获取角色数量
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public int GetRoleCount(string Name, string Value)
        {
            return dal.GetRoleCount(Name, Value);
        }
        /// <summary>
        /// 修改和增加角色
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="Role"></param>
        /// <param name="Value"></param>
        /// <param name="DataRangeID"></param>
        /// <param name="Descrption"></param>
        /// <returns></returns>
        public bool EditRole(string UpdateID, ParaStruct.RolePage Role)
        {
            return dal.EditRole(UpdateID, Role);

        }
        /// <summary>
        ///删除角色
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        public bool DeleteRole(string deleteID)
        {
            return dal.DeleteRole(deleteID);
        }

        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<Model.SYS.Module> GetModuleList(int pageIndex, int pageSize, string Name, string Value, string SearchEnabled)
        {
            return dal.GetModuleList(pageIndex, pageSize, Name, Value, SearchEnabled);
        }
        /// <summary>
        /// 获取模板数据个数
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="SearchEnabled"></param>
        /// <returns></returns>
        public int GetModuleCount(string Name, string Value, string SearchEnabled)
        {
            return dal.GetModuleCount(Name, Value, SearchEnabled);
        }
        /// <summary>
        /// 模板ID
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public bool SameModuleIDNum(string ModuleID)
        {
            return dal.SameModuleIDNum(ModuleID);
        }

        /// <summary>
        /// 获取tree节点数据
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <returns></returns>
        public List<Model.SYS.TreeModule> GetSubNodes(string parentNodeId)
        {
            return dal.GetSubNodes(parentNodeId);
        }
        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="ModulePage"></param>
        /// <returns></returns>
        public bool EditModule(string UpdateID, ParaStruct.ModulePage ModulePage)
        {

            return dal.EditModule(UpdateID, ModulePage);
        }
        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        public bool DeleteModule(string deleteID)
        {
            return dal.DeleteModule(deleteID);
        }
        /// <summary>
        /// 获取父级节点
        /// </summary>
        /// <param name="ParentModuleID"></param>
        /// <returns></returns>
        public string SearchParentName(string ParentModuleID)
        {
            return dal.SearchParentName(ParentModuleID);
        }

        /// <summary>
        /// 获取权限集列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Permission"></param>
        /// <returns></returns>
        public IList<Model.ORG.PermissionSetInfo> GetPermissionSetList(int pageIndex, int pageSize, ParaStruct.PermissionSet Permission)
        {
            return dal.GetPermissionSetList(pageIndex, pageSize, Permission);
        }
        /// <summary>
        /// 获取权限集个数
        /// </summary>
        /// <param name="Permission"></param>
        /// <returns></returns>
        public int GetPermissionSetCount(ParaStruct.PermissionSet Permission)
        {
            return dal.GetPermissionSetCount(Permission);
        }
        /// <summary>
        /// 获取moduleID
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<string> GetPermissionSetView(string param)
        {
            return dal.GetPermissionSetView(param);
        }
        /// <summary>
        /// 获取权限集树
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Model.SYS.TreeNode> GetPermissionSetTree(string parentNodeId, string param)
        {
            return dal.GetPermissionSetTree(parentNodeId, param);
        }
        /// <summary>
        /// 编辑添加权限集
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="Permission"></param>
        /// <returns></returns>
        public bool EditAddPermissionSet(string flag, ParaStruct.PermissionSet Permission)
        {
            return dal.EditAddPermissionSet(flag, Permission);
        }
        /// <summary>
        /// 删除权限集列表
        /// </summary>
        /// <param name="Permission"></param>
        /// <returns></returns>
        public bool DeletePermissionSetList(ParaStruct.PermissionSet Permission)
        {
            return dal.DeletePermissionSetList(Permission);

        }
    }
}
