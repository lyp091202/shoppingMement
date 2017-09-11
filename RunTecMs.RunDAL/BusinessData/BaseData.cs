using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RunTecMs.Common;
using RunTecMs.Common.DBUtility;
using RunTecMs.Common.ConvertUtility;
using RunTecMs.Model.Parameter;
using RunTecMs.RunIDAL.BusinessData;

namespace RunTecMs.RunDAL.BusinessData
{
    public class BaseData : IBaseData
    {
        #region  列表列名
        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="UsePage"></param>
        /// <param name="UseGrid"></param>
        /// <returns></returns>
        public IList<Model.SYS.ShowGridColumns> GetGridColumns(string UsePage, string UseGrid, string EmpRole = "", bool IsRole = false)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM SYS_GridListColumns ");
            sb.AppendLine("WHERE usePage = @usePage AND useGrid = @useGrid ");
            paraList.Add(new SqlParameter("@usePage", UsePage));
            paraList.Add(new SqlParameter("@useGrid", UseGrid));

            if (IsRole)
            {
                sb.AppendLine("AND useRole like @useRole ");
                paraList.Add(new SqlParameter("@useGrid", "%" + EmpRole + "%"));
            }

            DataTable dt = DbHelperSQL.Query(sb.ToString(), paraList.ToArray()).Tables[0];

            IList<Model.SYS.ShowGridColumns> colList = ConvertToList.DataTableToList<Model.SYS.ShowGridColumns>(dt);

            return colList;
        }
        #endregion


        #region  日志 错误日志
        /// <summary>
        /// 获取错误日志列表数据
        /// </summary>
        /// <returns></returns>
        public IList<Model.SYS.LogErrorInfo> GetLogErrorList(int pageIndex, int pageSize, ParaStruct.LogError LogError)
        { 
             List<SqlParameter> spara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select * from  (select row_number()over(order by ErrorTime desc )as row_num,* from V_LogError  where 1=1");

            if (!string.IsNullOrEmpty(LogError.UserName))
            {
                sb.AppendLine("and UserName like  @UserName");
                spara.Add(new SqlParameter("@UserName", "%" + LogError.UserName + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.EmployeeName))
            {
                sb.AppendLine("and EmployeeName like  @EmployeeName");
                spara.Add(new SqlParameter("@EmployeeName", "%" + LogError.EmployeeName + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.StartTime))  
            {
                sb.AppendLine("and CONVERT(varchar(100), ErrorTime, 23) >= CONVERT(varchar(100), @StartTime, 23)");
                spara.Add(new SqlParameter("@StartTime",  LogError.StartTime));
            }
            if (!string.IsNullOrEmpty(LogError.EndTime))
            {
                sb.AppendLine("and  CONVERT(varchar(100), ErrorTime, 23)<= CONVERT(varchar(100), @EndTime, 23) ");
                spara.Add(new SqlParameter("@EndTime", LogError.EndTime));
            }
            if (!string.IsNullOrEmpty(LogError.ErrorUrl))
            {
                sb.AppendLine("and ErrorUrl like  @ErrorUrl");
                spara.Add(new SqlParameter("@ErrorUrl", "%" + LogError.ErrorUrl + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.ErrorAction))
            {
                sb.AppendLine("and ErrorAction like  @ErrorAction");
                spara.Add(new SqlParameter("@ErrorAction", "%" + LogError.ErrorAction + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.ErrorClassName))
            {
                sb.AppendLine("and ErrorClassName like  @ErrorClassName");
                spara.Add(new SqlParameter("@ErrorClassName", "%" + LogError.ErrorClassName + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.ErrorInfo))
            {
                sb.AppendLine("and ErrorInfo like  @ErrorInfo");
                spara.Add(new SqlParameter("@ErrorInfo", "%" + LogError.ErrorInfo + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.BusinessID) && LogError.BusinessID!="0")
            {
                sb.AppendLine("and BusinessID = @BusinessID");
                spara.Add(new SqlParameter("@BusinessID",LogError.BusinessID));
                
            }

            sb.AppendLine(") LogError ");
            sb.AppendFormat("where LogError.row_num between {0} and {1} ", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
            DataTable dt = DbHelperSQL.Query(sb.ToString(), spara.ToArray()).Tables[0];
            return ConvertToList.DataTableToList<Model.SYS.LogErrorInfo>(dt);
        }
        /// <summary>
        /// 获取错误日志个数
        /// </summary>
        /// <returns></returns>
        public int GetLogErrorCount(ParaStruct.LogError LogError)
        {
            List<SqlParameter> spara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(LogError.UserName))
            {
                sb.AppendLine("and UserName like  @UserName");
                spara.Add(new SqlParameter("@UserName", "%" + LogError.UserName + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.EmployeeName))
            {
                sb.AppendLine("and EmployeeName like  @EmployeeName");
                spara.Add(new SqlParameter("@EmployeeName", "%" + LogError.EmployeeName + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.StartTime))
            {
                sb.AppendLine("and CONVERT(varchar(100), ErrorTime, 23) >= CONVERT(varchar(100), @StartTime, 23)");
                spara.Add(new SqlParameter("@StartTime", LogError.StartTime));
            }
            if (!string.IsNullOrEmpty(LogError.EndTime))
            {
                sb.AppendLine("and  CONVERT(varchar(100), ErrorTime, 23)<= CONVERT(varchar(100), @EndTime, 23) ");
                spara.Add(new SqlParameter("@EndTime", LogError.EndTime));
            }
            if (!string.IsNullOrEmpty(LogError.ErrorUrl))
            {
                sb.AppendLine("and ErrorUrl like  @ErrorUrl");
                spara.Add(new SqlParameter("@ErrorUrl", "%" + LogError.ErrorUrl + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.ErrorAction))
            {
                sb.AppendLine("and ErrorAction like  @ErrorAction");
                spara.Add(new SqlParameter("@ErrorAction", "%" + LogError.ErrorAction + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.ErrorClassName))
            {
                sb.AppendLine("and ErrorClassName like  @ErrorClassName");
                spara.Add(new SqlParameter("@ErrorClassName", "%" + LogError.ErrorClassName + "%"));
            }
            if (!string.IsNullOrEmpty(LogError.ErrorInfo))
            {
                sb.AppendLine("and ErrorInfo like  @ErrorInfo");
                spara.Add(new SqlParameter("@ErrorInfo", "%" + LogError.ErrorInfo + "%"));
            }

            if (!string.IsNullOrEmpty(LogError.BusinessID) && LogError.BusinessID != "0")
            {
                sb.AppendLine("and BusinessID = @BusinessID");
                spara.Add(new SqlParameter("@BusinessID", LogError.BusinessID));

            }
            return Common.GetRowsCount("V_LogError", sb.ToString(), spara.ToArray());
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <returns></returns>
        public IList<Model.SYS.LogError> GetErrorInfo(int ErrorID)
        {
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> spara = new List<SqlParameter>();
            sb.AppendLine("select ErrorInfo from  Log_Error where ErrorID=@ErrorID");
            spara.Add(new SqlParameter("@ErrorID", ErrorID));
            DataTable dt = DbHelperSQL.Query(sb.ToString(),spara.ToArray()).Tables[0];
            return ConvertToList.DataTableToList<Model.SYS.LogError>(dt);
        }
      
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="TweetID"></param>
        /// <returns></returns>
        public bool DeleteLogError(string ErrorID)
        {
            return Common.DeleteRecordByPK("Log_Error", "ErrorID", ErrorID);
        }
        /// <summary>
        /// 插入错误日志表
        /// </summary>
        /// <param name="LogError"></param>
        /// <returns></returns>
        public bool InsertLogError(Model.SYS.LogError LogError)
        {
            int count = 0;
            List<SqlParameter> listPara = new List<SqlParameter>();
            StringBuilder sb = new StringBuilder();
            listPara.Add(new SqlParameter("@BusinessID", LogError.BusinessID));
            listPara.Add(new SqlParameter("@ErrorUserID", LogError.ErrorUserID));
            listPara.Add(new SqlParameter("@ErrorEmployeeID", LogError.ErrorEmployeeID));
            listPara.Add(new SqlParameter("@ErrorUrl", LogError.ErrorUrl));
            listPara.Add(new SqlParameter("@ErrorAction", LogError.ErrorAction));
            listPara.Add(new SqlParameter("@ErrorThreadID", LogError.ErrorThreadID));
            listPara.Add(new SqlParameter("@ErrorClassName", LogError.ErrorClassName));
            listPara.Add(new SqlParameter("@ErrorMethodName", LogError.ErrorMethodName));
            listPara.Add(new SqlParameter("@ErrorInfo", LogError.ErrorInfo));
            listPara.Add(new SqlParameter("@ErrorTime", LogError.ErrorTime));
            listPara.Add(new SqlParameter("@Level", LogError.Level));
            sb.AppendLine("INSERT INTO Log_Error (BusinessID, ErrorUserID, ErrorEmployeeID, ErrorUrl, ErrorAction, ErrorThreadID, ErrorClassName, ErrorMethodName, ErrorInfo, ErrorTime, Level)");
            sb.AppendLine(" VALUES (@BusinessID, @ErrorUserID, @ErrorEmployeeID, @ErrorUrl, @ErrorAction, @ErrorThreadID, @ErrorClassName, @ErrorMethodName, @ErrorInfo, @ErrorTime, @Level)");
            count = DbHelperSQL.ExecuteSql(sb.ToString(), listPara.ToArray());
            return count > 0;
        }

        #endregion 

    }
}
