using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.RunDALFactory;
namespace RunTecMs.RunDALFactory
{
    public sealed class DataAccess : DataAccessBase
    {
        #region RunUI

        // 员工
        public static RunIDAL.Organizations.IEmployee CreateEmployee()
        {
            object objType = CreateObject(orgAssemblyPath + ".Employee");
            return (RunIDAL.Organizations.IEmployee)objType;
        }

        // 权限
        public static RunIDAL.Organizations.IPermission CreatePermission()
        {
            object objType = CreateObject(orgAssemblyPath + ".Permission");
            return (RunIDAL.Organizations.IPermission)objType;
        }

        // 模型
        public static RunIDAL.Organizations.IModule<Model.SYS.Module> CreateModule()
        {
            object objType = CreateObject(orgAssemblyPath + ".Module");
            return (RunIDAL.Organizations.IModule<Model.SYS.Module>)objType;
        }

        // 组织结构
        public static RunIDAL.Organizations.IOrganization CreateOrganization()
        {
            object objType = CreateObject(orgAssemblyPath + ".Organization");
            return (RunIDAL.Organizations.IOrganization)objType;
        }

        // 业务数据
        public static RunIDAL.BusinessData.IBaseData CreateBusBaseData()
        {
            object objType = CreateObject(busAssemblyPath + ".BaseData");
            return (RunIDAL.BusinessData.IBaseData)objType;
        }
        #endregion

        #region 系统管理用
        /// <summary>
        /// 消息管理
        /// </summary>
        /// <returns></returns>
        public static RunIDAL.BusinessData.IMessageManager CreateMessageManager()
        {
            object objType = CreateObject(busAssemblyPath + ".MessageManager");
            return (RunIDAL.BusinessData.IMessageManager)objType;
        }
        #endregion

    }
}
