using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.Model.Parameter;

namespace RunTecMs.RunIDAL.Organizations
{
    public interface IPermission
    {
        /// <summary>
        /// 数据范围列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        IList<Model.ORG.Per_DataRange> GetDataRangeList(int pageIndex, int pageSize, string Name, string Value);
       /// <summary>
       /// 数据范围统计个数
       /// </summary>
       /// <param name="Name"></param>
       /// <param name="Value"></param>
       /// <returns></returns>
        int GetDataRangeCount(string Name, string Value);
        /// <summary>
        /// 验证模板id是否相同
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        bool SameModuleIDNum(string ModuleID);
        /// <summary>
        /// 修改数据范围
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="DataRange"></param>
        /// <returns></returns>
        bool EditDataRange(string UpdateID, ParaStruct.DataRange DataRange);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        bool DeleteDataRange(string deleteID);

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        IList<Model.ORG.RoleInfo> GetRoleList(int pageIndex, int pageSize, string Name, string Value);

        /// <summary>
        /// 获取角色数量
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        int GetRoleCount(string Name, string Value);
        /// <summary>
        /// 角色数据范围
        /// </summary>
        /// <returns></returns>
        IList<Model.ORG.DataRang> GetRoleDataRang();
        /// <summary>
        /// 修改和增加
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="DataRangeID"></param>
        /// <param name="Descrption"></param>
        /// <returns></returns>
        bool EditRole(string UpdateID, ParaStruct.RolePage Role);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>

        bool DeleteRole(string deleteID);

        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IList<Model.SYS.Module> GetModuleList(int pageIndex, int pageSize, string Name, string Value, string SearchEnabled);

        /// <summary>
        /// 获取模板数量
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="SearchEnabled"></param>
        /// <returns></returns>
        int GetModuleCount(string Name, string Value, string SearchEnabled);

        /// <summary>
        ///获取树的方法
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <returns></returns>
        List<Model.SYS.TreeModule> GetSubNodes(string parentNodeId);

        /// <summary>
        /// 修改模板菜单
        /// </summary>
        /// <param name="UpdateID"></param>
        /// <param name="ModulePage"></param>
        /// <returns></returns>
        bool EditModule(string UpdateID, ParaStruct.ModulePage ModulePage);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns></returns>
        bool DeleteModule(string deleteID);
        /// <summary>
        /// 查询父级名称
        /// </summary>
        /// <param name="ParentModuleID"></param>
        /// <returns></returns>
        string SearchParentName(string ParentModuleID);

        /// <summary>
        /// 权限集
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Permission"></param>
        /// <returns></returns>
        IList<Model.ORG.PermissionSetInfo> GetPermissionSetList(int pageIndex, int pageSize, ParaStruct.PermissionSet Permission);
        /// <summary>
        /// 权限集列表个数
        /// </summary>
        /// <param name="Permission"></param>
        /// <returns></returns>
        int GetPermissionSetCount(ParaStruct.PermissionSet Permission);
        /// <summary>
        /// 显示moduleID
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<string> GetPermissionSetView(string param);

        /// <summary>
        /// 获取权限集的tree
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        List<Model.SYS.TreeNode> GetPermissionSetTree(string parentNodeId, string param);
        /// <summary>
        /// 编辑和添加权限集
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="Permission"></param>
        /// <returns></returns>
        bool EditAddPermissionSet(string flag, ParaStruct.PermissionSet Permission);
        /// <summary>
        /// 删除权限集列表
        /// </summary>
        /// <param name="Permission"></param>
        /// <returns></returns>
        bool DeletePermissionSetList(ParaStruct.PermissionSet Permission);
    }
}
