using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.Model.Parameter;
using System.Data.SqlClient;
using System.Data;

namespace RunTecMs.RunIDAL.BusinessData
{
    public interface IBaseData
    {
        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="UsePage"></param>
        /// <param name="UseGrid"></param>
        /// <returns></returns>
        IList<Model.SYS.ShowGridColumns> GetGridColumns(string UsePage, string UseGrid, string EmpRole, bool IsRole = false);
        /// <summary>
        /// 获取日志错误列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="LogError"></param>
        /// <returns></returns>
        IList<Model.SYS.LogErrorInfo> GetLogErrorList(int pageIndex, int pageSize, ParaStruct.LogError LogError);
        /// <summary>
        /// 获取日志错误个数
        /// </summary>
        /// <param name="LogError"></param>
        /// <returns></returns>
        int GetLogErrorCount(ParaStruct.LogError LogError);
        /// <summary>
        /// 出错导入日志错误表
        /// </summary>
        /// <param name="LogError"></param>
        /// <returns></returns>
        bool InsertLogError(Model.SYS.LogError LogError);
        /// <summary>
        /// 删除错误日志
        /// </summary>
        /// <param name="ErrorID"></param>
        /// <returns></returns>
        bool DeleteLogError(string ErrorID);
        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns></returns>
        IList<Model.SYS.LogError> GetErrorInfo(int ErrorID);
    }
}
