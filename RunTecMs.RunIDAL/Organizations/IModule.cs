using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.RunIDAL.Organizations
{
    /// <summary>
    /// 模块接口
    /// </summary>
    /// <param name="employeeId">登录者ID</param>
    /// <typeparam name="T"></typeparam>
   public interface IModule<T> where T:class
   {
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="parentId">父菜单ID</param>
        /// <param name="employeeId">LoginUserIdID</param>
        /// <param name="ManagerTypeID">后台商户类型ID（1：管理后台 2：第三方商户管理后台）</param>
        /// <returns></returns>
        IList<T> GetMenu(int parentId, int LoginUserId, int ManagerTypeID);

        /// <summary>
        /// 获取导航栏下的子菜单
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="LoginUserId">登录者ID</param>
        /// <param name="ManagerTypeID">后台商户类型ID（1：管理后台 2：第三方商户管理后台）</param>
        /// <returns></returns>
        IList<T> GetAllChildMenuList(int moduleId, int LoginUserId, int ManagerTypeID);
   }
}
