using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RunTecMs.Model.Common;
using RunTecMs.Common.DBUtility;
using RunTecMs.Common.ConvertUtility;
using RunTecMs.RunDAL.Organizations;

namespace RunTecMs.RunDAL
{
    public class Common
    {
        /// <summary>
        ///  是否是超管数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="keyName">主键名</param>
        /// <param name="keyValue">主键值</param>
        /// <returns>true:属于超管数据</returns>
        public bool GetIsSYSDBA(string tableName, string keyName, int keyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("SELECT IsSYSDBA FROM @tableName");
            strSql.AppendLine(" WHERE @keyName = @keyValue");

            SqlParameter[] para = {
              new SqlParameter("@tableName",SqlDbType.VarChar, 50),
              new SqlParameter("@keyName",SqlDbType.VarChar, 50),
              new SqlParameter("@keyValue",SqlDbType.Int),
            };

            int index = 0;
            para[index++].Value = tableName;
            para[index++].Value = keyName;
            para[index++].Value = keyValue;

            DataTable dt = DbHelperSQL.Query(strSql.ToString(), para).Tables[0];

            IList<string> dataList = ConvertToList.DataTableToList<string>(dt);

            if (dataList[0] == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得数据件数
        /// </summary>
        /// <param name="tabName">查询表名</param>
        /// <param name="colName">字段名</param>
        /// <param name="condition">条件(不带where)</param>
        /// <returns></returns>
        public static int GetRowsCount(string tabName, string condition = "", params SqlParameter[] para)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  SELECT COUNT(0) from " + tabName);
            sb.AppendLine(" where 1=1 ");
            if (!string.IsNullOrEmpty(condition.Trim()))
            {
                sb.AppendLine(condition);
            }

            object obj = DbHelperSQL.GetSingle(sb.ToString(), para);
            return obj != null ? Convert.ToInt32(obj) : 0;

        }

        /// <summary>
        /// 按主键删除
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="pkColumn">主键</param>
        /// <param name="condition">条件(不带where)</param>
        /// <returns></returns>
        public static bool DeleteRecordByPK(string tabName, string pkColumn, string condition)
        {
            string strSql = "delete " + tabName;
            if (!string.IsNullOrEmpty(condition))
            {
                if (condition.Contains(","))
                {
                    strSql = strSql + " where " + pkColumn + " in (" + condition + ")";
                }
                else
                {
                    strSql = strSql + " where " + pkColumn + " = " + condition;
                }
            }

            int result = DbHelperSQL.ExecuteSql(strSql);
            return result > 0;
        }


        /// <summary>
        /// 根据主键修改单个信息
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="column">字段</param>
        /// <param name="colType">字段类型（1：string 2：int 3：date）</param>
        /// <param name="value">值</param>
        /// <param name="PK">PK</param>
        /// <param name="PKValue">PKValue</param>
        /// <returns></returns>
        public static bool UpdateSignalRecord(string tabName, string column, int colType, string value, string PK, string PKValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE " + tabName);
            if (colType == 1 || colType == 3)
            {
                sb.AppendLine(" SET " + column + " = '" + value + "'");
            } 
            else if (colType == 2)
            {
                sb.AppendLine(" SET " + column + " = " + value);
            }
            
            if (!string.IsNullOrEmpty(PK))
            {
                if (PK.Contains(","))
                {
                    sb.AppendLine(" where " + PK + " in (" + PKValue + ")");
                }
                else
                {
                    sb.AppendLine(" where " + PK + " = " + PKValue);
                }
            }

            int result = DbHelperSQL.ExecuteSql(sb.ToString());
            return result > 0;
        }

        /// <summary>
        /// 取得分页情报的SQL语句
        /// </summary>
        /// <param name="info">分页情报的参数</param>
        /// <returns></returns>
        public static string GetPageResult(GetPageInfo info)
        {
            string strTemp = "";
            string strOrderType = " ORDER BY " + info.ascColumn;
            StringBuilder sb = new StringBuilder();

            // 1的场合执行降序
            if (info.intOrderType == 1)
            {
                strTemp = "<(SELECT min";
            }
            else
            {
                strTemp = ">(SELECT max";
            }

            // 
            if (info.pageIndex == 1)
            {
                sb.AppendLine(" SELECT TOP " + info.pageSize + "  " + info.showColumn + " FROM " + info.tabName);
                sb.AppendLine(" WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(info.where))
                {
                    sb.AppendLine(info.where);
                }
                sb.AppendLine(strOrderType);
            }
            else
            {
                sb.AppendLine(" SELECT TOP " + info.pageSize + "  " + info.showColumn + " FROM " + info.tabName);
                sb.AppendLine(" WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(info.where))
                {
                    sb.AppendLine(info.where);
                }
                sb.AppendLine(" AND " + info.pkColumn + strTemp + "(" + info.pkColumn + ")");
                sb.AppendLine(" FROM (SELECT TOP " + (info.pageIndex - 1) * info.pageSize + " " + info.pkColumn + " FROM ");
                sb.AppendLine(info.tabName + strOrderType + ") AS TabTemp) " + strOrderType);
            }

            return sb.ToString();
        }


        /// <summary>
        /// 通过参数转换SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static string GetSQL(string sql, params SqlParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                for (int i = 0; i < cmdParms.Length; i++)
                {
                    SqlParameter sqlParameter = cmdParms[i];
                    if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null)
                    {
                        sqlParameter.Value = DBNull.Value;
                    }
                    else
                    {
                        sql.Replace(sqlParameter.ParameterName.ToString(), "'" + sqlParameter.Value.ToString() + "'");

                    }
                }
            }
            return sql;
        }

        /// <summary>
        ///  取得配置文件信息
        /// </summary>
        /// <param name="tableName">表名</param>
        public static IList<Config> GetConfig(string ids)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM  dbo.SYS_Config ");
            sb.AppendFormat(" WHERE  ConfigID IN ( SELECT *  FROM  dbo.fn_StrToTable('{0}') )", ids);
            return DbHelperSQL.GetModelList<Config>(sb.ToString());
        }

        /// <summary>
        /// 创建列名
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateColTable()
        {

            DataTable dt = new DataTable("myTable");

            //field列  
            DataColumn columnField = new DataColumn();//创建一列  
            columnField.DataType = System.Type.GetType("System.String");//数据类型  
            columnField.ColumnName = "field";//列名  
            dt.Columns.Add(columnField);//添加到table  
                                        //title列  
            DataColumn columnTitle = new DataColumn();
            columnTitle.DataType = System.Type.GetType("System.String");
            columnTitle.ColumnName = "title";
            dt.Columns.Add(columnTitle);
            //align列  
            DataColumn columnAlign = new DataColumn();
            columnAlign.DataType = System.Type.GetType("System.String");
            columnAlign.ColumnName = "align";
            dt.Columns.Add(columnAlign);
            //width列  
            DataColumn columnWidth = new DataColumn();
            columnWidth.DataType = System.Type.GetType("System.Int32");
            columnWidth.ColumnName = "width";
            dt.Columns.Add(columnWidth);
            //hidden  
            DataColumn columnHidden = new DataColumn();
            columnHidden.DataType = System.Type.GetType("System.String");
            columnHidden.ColumnName = "hidden";
            dt.Columns.Add(columnHidden);
            //formatter  
            DataColumn columnformatter = new DataColumn();
            columnformatter.DataType = System.Type.GetType("System.String");
            columnformatter.ColumnName = "formatter";
            dt.Columns.Add(columnformatter);

            //halign  
            DataColumn columnhalign = new DataColumn();
            columnhalign.DataType = System.Type.GetType("System.String");
            columnhalign.ColumnName = "halign";
            dt.Columns.Add(columnhalign);

            //rowspan列  
            DataColumn columnrowspan = new DataColumn();
            columnrowspan.DataType = System.Type.GetType("System.Int32");
            columnrowspan.ColumnName = "rowspan";
            dt.Columns.Add(columnrowspan);

            //colspan列  
            DataColumn columncolspan = new DataColumn();
            columncolspan.DataType = System.Type.GetType("System.Int32");
            columncolspan.ColumnName = "colspan";
            dt.Columns.Add(columncolspan);

            // sortable列
            DataColumn columnsortable = new DataColumn();
            columnsortable.DataType = System.Type.GetType("System.String");
            columnsortable.ColumnName = "sortable";
            dt.Columns.Add(columnsortable);

            // checkbox列
            DataColumn columncheckbox = new DataColumn();
            columncheckbox.DataType = System.Type.GetType("System.String");
            columncheckbox.ColumnName = "checkbox";
            dt.Columns.Add(columncheckbox);

            // resizable列
            DataColumn columnresizable = new DataColumn();
            columnresizable.DataType = System.Type.GetType("System.String");
            columnresizable.ColumnName = "resizable";
            dt.Columns.Add(columnresizable);

            // orderType列
            DataColumn columnorderType = new DataColumn();
            columnorderType.DataType = System.Type.GetType("System.String");
            columnorderType.ColumnName = "orderType";
            dt.Columns.Add(columnorderType);

            return dt;
        }
    }
}
