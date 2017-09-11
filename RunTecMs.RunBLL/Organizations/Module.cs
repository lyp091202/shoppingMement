
using System.Collections.Generic;
using RunTecMs.Model.EnumType;
using RunTecMs.RunDALFactory;
using RunTecMs.RunIDAL.Organizations;

namespace RunTecMs.RunBLL.Organizations
{
   public class Module
    {
       private readonly IModule<Model.SYS.Module> dal = DataAccess.CreateModule();

        /// <summary>
       /// 获取菜单 
       /// </summary>
       /// <param name="parentId">父级模块</param>
       /// <param name="employeeId">登录者ID</param>
       /// <returns></returns>
       public IList<Model.SYS.Module> GetMenu(int parentId, int employeeId)
       {
           return dal.GetMenu(parentId, employeeId, (int)ManagerType.管理后台);
       }

       /// <summary>
       /// 获取导航栏下的子菜单
       /// </summary>
       /// <param name="moduleId"></param>
       /// <param name="employeeId">登录者ID</param>
       /// <returns></returns>
       public IList<Model.SYS.Module> GetAllChildMenuList(int moduleId, int employeeId)
       {
           return dal.GetAllChildMenuList(moduleId, employeeId, (int)ManagerType.管理后台);
       }

       /// <summary>
       /// 获取菜单（商户后台）
       /// </summary>
       /// <param name="parentId">父级模块</param>
       /// <param name="MerchantId">登录者ID</param>
       /// <returns></returns>
       public IList<Model.SYS.Module> GetMerchantMenu(int parentId, int MerchantId)
       {
           return dal.GetMenu(parentId, MerchantId, (int)ManagerType.第三方商户);
       }

       /// <summary>
       /// 获取导航栏下的子菜单（商户后台）
       /// </summary>
       /// <param name="moduleId"></param>
       /// <param name="MerchantId">登录者ID</param>
       /// <returns></returns>
       public IList<Model.SYS.Module> GetMerchantAllChildMenuList(int moduleId, int MerchantId)
       {
           return dal.GetAllChildMenuList(moduleId, MerchantId, (int)ManagerType.第三方商户);
       }
    }
}
