using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.RunIDAL.BusinessData;
using System.Data.SqlClient;
using RunTecMs.RunDALFactory;
using System.Data;
using RunTecMs.Model.EnumType;
using System.Transactions;
using RunTecMs.Common.DBUtility;
using RunTecMs.Model.Parameter;
using System.IO;
using RunTecMs.Common.ExcelUtility;

namespace RunTecMs.RunBLL.BusinessData
{
    public class BaseData
    {
        private readonly IBaseData dal = DataAccess.CreateBusBaseData();

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="UsePage"></param>
        /// <param name="UseGrid"></param>
        /// <returns></returns>
        public IList<Model.SYS.ShowGridColumns> GetGridColumns(string UsePage, string UseGrid, string EmpRole, bool IsRole = false)
        {
            return dal.GetGridColumns(UsePage, UseGrid, EmpRole, IsRole);
        }
        /// <summary>
        /// 获取错误日志列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="LogError"></param>
        /// <returns></returns>
        public IList<Model.SYS.LogErrorInfo> GetLogErrorList(int pageIndex, int pageSize, ParaStruct.LogError LogError)
        {
            return dal.GetLogErrorList(pageIndex, pageSize, LogError);
        }
        /// <summary>
        /// 获取错误日志个数
        /// </summary>
        /// <param name="LogError"></param>
        /// <returns></returns>
        public int GetLogErrorCount(ParaStruct.LogError LogError)
        {
            return dal.GetLogErrorCount(LogError);
        }
        /// <summary>
        /// 插入错误日志表
        /// </summary>
        /// <param name="LogError"></param>
        /// <returns></returns>
        public bool InsertLogError(Model.SYS.LogError LogError)
        {
            return dal.InsertLogError(LogError);
        }
        /// <summary>
        /// 删除错误日志
        /// </summary>
        /// <param name="ErrorID"></param>
        /// <returns></returns>
        public bool DeleteLogError(string ErrorID)
        {
            return dal.DeleteLogError(ErrorID);
        }
       /// <summary>
       /// 获取错误日志信息
       /// </summary>
       /// <param name="ErrorID"></param>
       /// <returns></returns>
        public IList<Model.SYS.LogError> GetErrorInfo(int ErrorID)
        {
            return dal.GetErrorInfo(ErrorID);
        }
    }
}
