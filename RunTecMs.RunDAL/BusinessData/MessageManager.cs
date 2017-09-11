using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.RunIDAL.BusinessData;
using System.Data.SqlClient;
using System.Data;
using RunTecMs.Common.DBUtility;
using RunTecMs.Common.ConvertUtility;
using RunTecMs.Common.DEncrypt;

namespace RunTecMs.RunDAL.BusinessData
{
    public class MessageManager : IMessageManager
    {
        /// <summary>
        /// 根据条件获取内容
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<Model.Common.UIMessageManager> ExeSelectContentSql(string sql)
        {
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return ConvertToList.DataTableToList<Model.Common.UIMessageManager>(dt);
        }

        /// <summary>
        /// 根据条件获取件数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExeSelectCountSql(string sql)
        {
            object obj = DbHelperSQL.GetSingle(sql);
            return obj != null ? Convert.ToInt32(obj) : 0;
        }

        /// <summary>
        /// 更新或者插入数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExeUpdateSql(string sql)
        {
            int count = DbHelperSQL.ExecuteSql(sql);
            return count > 0;
        }
    }
}
