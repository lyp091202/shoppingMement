using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Management;

namespace RunTecMs.Common.DBUtility
{
    public enum EffentNextType
    {
        None,
        WhenHaveContine,
        WhenNoHaveContine,
        ExcuteEffectRows,
        SolicitationEvent
    }
    public class DbHelperSQL
    {
        public static string connectionString = PubConstant.ConnectionString;

        public static SqlConnection GetConnection
        {
            get
            {
                return new SqlConnection(DbHelperSQL.connectionString);
            }
        }

        public DbHelperSQL()
        {
        }

        public DbHelperSQL(string ConnectionString)
        {
            DbHelperSQL.connectionString = ConnectionString;
        }

        public static bool ColumnExists(string tableName, string columnName)
        {
            string sQLString = string.Concat(new string[]
            {
                "select count(1) from syscolumns where [id]=object_id('",
                tableName,
                "') and [name]='",
                columnName,
                "'"
            });
            object single = DbHelperSQL.GetSingle(sQLString);
            return single != null && Convert.ToInt32(single) > 0;
        }

        public static int GetMaxID(string FieldName, string TableName)
        {
            string sQLString = "select max(" + FieldName + ")+1 from " + TableName;
            object single = DbHelperSQL.GetSingle(sQLString);
            int result;
            if (single == null)
            {
                result = 1;
            }
            else
            {
                result = int.Parse(single.ToString());
            }
            return result;
        }

        public static bool Exists(string strSql)
        {
            object single = DbHelperSQL.GetSingle(strSql);
            int num;
            if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
            {
                num = 0;
            }
            else
            {
                num = int.Parse(single.ToString());
            }
            return num != 0;
        }

        public static bool TabExists(string TableName)
        {
            string sQLString = "select count(*) from sysobjects where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            object single = DbHelperSQL.GetSingle(sQLString);
            int num;
            if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
            {
                num = 0;
            }
            else
            {
                num = int.Parse(single.ToString());
            }
            return num != 0;
        }

        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object single = DbHelperSQL.GetSingle(strSql, cmdParms);
            int num;
            if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
            {
                num = 0;
            }
            else
            {
                num = int.Parse(single.ToString());
            }
            return num != 0;
        }

        public static int ExecuteSql(string SQLString)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        int num = sqlCommand.ExecuteNonQuery();
                        result = num;
                    }
                    catch (SqlException ex)
                    {
                        sqlConnection.Close();
                        throw ex;
                    }
                }
            }
            return result;
        }

        public static int ExecuteSqlByTime(string SQLString, int Times)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandTimeout = Times;
                        int num = sqlCommand.ExecuteNonQuery();
                        result = num;
                    }
                    catch (SqlException ex)
                    {
                        sqlConnection.Close();
                        throw ex;
                    }
                }
            }
            return result;
        }

        public static int ExecuteSqlTran(List<CommandInfo> list, List<CommandInfo> oracleCmdSqlList)
        {
            return 0;
        }

        public static int ExecuteSqlTran(List<string> SQLStringList)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    int num = 0;
                    for (int i = 0; i < SQLStringList.Count; i++)
                    {
                        string text = SQLStringList[i];
                        if (text.Trim().Length > 1)
                        {
                            sqlCommand.CommandText = text;
                            num += sqlCommand.ExecuteNonQuery();
                        }
                    }
                    sqlTransaction.Commit();
                    result = num;
                }
                catch
                {
                    sqlTransaction.Rollback();
                    result = 0;
                }
            }
            return result;
        }

        public static int ExecuteSql(string SQLString, string content)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection);
                SqlParameter sqlParameter = new SqlParameter("@content", SqlDbType.NText);
                sqlParameter.Value = content;
                sqlCommand.Parameters.Add(sqlParameter);
                try
                {
                    sqlConnection.Open();
                    int num = sqlCommand.ExecuteNonQuery();
                    result = num;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
            }
            return result;
        }

        public static object ExecuteSqlGet(string SQLString, string content)
        {
            object result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection);
                SqlParameter sqlParameter = new SqlParameter("@content", SqlDbType.NText);
                sqlParameter.Value = content;
                sqlCommand.Parameters.Add(sqlParameter);
                try
                {
                    sqlConnection.Open();
                    object obj = sqlCommand.ExecuteScalar();
                    if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                    {
                        result = null;
                    }
                    else
                    {
                        result = obj;
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
            }
            return result;
        }

        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
                SqlParameter sqlParameter = new SqlParameter("@fs", SqlDbType.Image);
                sqlParameter.Value = fs;
                sqlCommand.Parameters.Add(sqlParameter);
                try
                {
                    sqlConnection.Open();
                    int num = sqlCommand.ExecuteNonQuery();
                    result = num;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
            }
            return result;
        }

        public static object GetSingle(string SQLString)
        {
            object result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        object obj = sqlCommand.ExecuteScalar();
                        if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                        {
                            result = null;
                        }
                        else
                        {
                            result = obj;
                        }
                    }
                    catch (SqlException ex)
                    {
                        sqlConnection.Close();
                        throw ex;
                    }
                }
            }
            return result;
        }

        public static object GetSingle(string SQLString, int Times)
        {
            object result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandTimeout = Times;
                        object obj = sqlCommand.ExecuteScalar();
                        if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                        {
                            result = null;
                        }
                        else
                        {
                            result = obj;
                        }
                    }
                    catch (SqlException ex)
                    {
                        sqlConnection.Close();
                        throw ex;
                    }
                }
            }
            return result;
        }

        public static SqlDataReader ExecuteReader(string strSQL)
        {
            SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString);
            SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
            SqlDataReader result;
            try
            {
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                result = sqlDataReader;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return result;
        }

        public static DataSet Query(string SQLString)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SQLString, sqlConnection);
                    sqlDataAdapter.Fill(dataSet, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                result = dataSet;
            }
            return result;
        }


        public static DataSet Query(string SQLString, int Times)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    sqlConnection.Open();
                    new SqlDataAdapter(SQLString, sqlConnection)
                    {
                        SelectCommand =
                        {
                            CommandTimeout = Times
                        }
                    }.Fill(dataSet, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                result = dataSet;
            }
            return result;
        }

        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
                        int num = sqlCommand.ExecuteNonQuery();
                        sqlCommand.Parameters.Clear();
                        result = num;
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }

        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        foreach (DictionaryEntry dictionaryEntry in SQLStringList)
                        {
                            string cmdText = dictionaryEntry.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])dictionaryEntry.Value;
                            DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, cmdText, cmdParms);
                            int num = sqlCommand.ExecuteNonQuery();
                            sqlCommand.Parameters.Clear();
                        }
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static int ExecuteSqlTran(List<CommandInfo> cmdList)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        int num = 0;
                        foreach (CommandInfo current in cmdList)
                        {
                            string commandText = current.CommandText;
                            SqlParameter[] cmdParms = (SqlParameter[])current.Parameters;
                            DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, commandText, cmdParms);
                            if (current.EffentNextType == EffentNextType.WhenHaveContine || current.EffentNextType == EffentNextType.WhenNoHaveContine)
                            {
                                if (current.CommandText.ToLower().IndexOf("count(") == -1)
                                {
                                    sqlTransaction.Rollback();
                                    result = 0;
                                    return result;
                                }
                                object obj = sqlCommand.ExecuteScalar();
                                if (obj == null && obj == DBNull.Value)
                                {
                                }
                                bool flag = Convert.ToInt32(obj) > 0;
                                if (current.EffentNextType == EffentNextType.WhenHaveContine && !flag)
                                {
                                    sqlTransaction.Rollback();
                                    result = 0;
                                    return result;
                                }
                                if (current.EffentNextType == EffentNextType.WhenNoHaveContine && flag)
                                {
                                    sqlTransaction.Rollback();
                                    result = 0;
                                    return result;
                                }
                            }
                            else
                            {
                                int num2 = sqlCommand.ExecuteNonQuery();
                                num += num2;
                                if (current.EffentNextType == EffentNextType.ExcuteEffectRows && num2 == 0)
                                {
                                    sqlTransaction.Rollback();
                                    result = 0;
                                    return result;
                                }
                                sqlCommand.Parameters.Clear();
                            }
                        }
                        sqlTransaction.Commit();
                        result = num;
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }

        public static int ExecuteSqlTran4Indentity(List<CommandInfo> cmdList)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        int num = 0;
                        int num2 = 0;
                        foreach (CommandInfo current in cmdList)
                        {
                            string commandText = current.CommandText;
                            SqlParameter[] array = (SqlParameter[])current.Parameters;
                            SqlParameter[] array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                SqlParameter sqlParameter = array2[i];
                                if (sqlParameter.Direction == ParameterDirection.InputOutput)
                                {
                                    sqlParameter.Value = num2;
                                }
                            }
                            DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, commandText, array);
                            int num3 = sqlCommand.ExecuteNonQuery();
                            num += num3;
                            if (current.EffentNextType == EffentNextType.ExcuteEffectRows && num3 == 0)
                            {
                                sqlTransaction.Rollback();
                                result = 0;
                                return result;
                            }
                            array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                SqlParameter sqlParameter = array2[i];
                                if (sqlParameter.Direction == ParameterDirection.Output)
                                {
                                    num2 = Convert.ToInt32(sqlParameter.Value);
                                }
                            }
                            sqlCommand.Parameters.Clear();
                        }
                        sqlTransaction.Commit();
                        result = num;
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }

        public static int ExecuteSqlTran4Indentity(List<CommandInfo> cmdList, SqlTransaction trans)
        {
            SqlCommand sqlCommand = new SqlCommand();
            int num = 0;
            int num2 = 0;
            foreach (CommandInfo current in cmdList)
            {
                string commandText = current.CommandText;
                SqlParameter[] array = (SqlParameter[])current.Parameters;
                SqlParameter[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    SqlParameter sqlParameter = array2[i];
                    if (sqlParameter.Direction == ParameterDirection.InputOutput)
                    {
                        sqlParameter.Value = num2;
                    }
                }
                DbHelperSQL.PrepareCommand(sqlCommand, trans.Connection, trans, commandText, array);
                int num3 = sqlCommand.ExecuteNonQuery();
                num += num3;
                if (current.EffentNextType == EffentNextType.ExcuteEffectRows && num3 == 0)
                {
                    throw new SqlExecutionException("DbHelperSQL.ExecuteSqlTran4Indentity - [" + sqlCommand.CommandText + "] 未执行成功!");
                }
                array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    SqlParameter sqlParameter = array2[i];
                    if (sqlParameter.Direction == ParameterDirection.Output)
                    {
                        num2 = Convert.ToInt32(sqlParameter.Value);
                    }
                }
                sqlCommand.Parameters.Clear();
            }
            return num;
        }

        [Obsolete]
        public static void ExecuteSqlTranWithIndentity(List<CommandInfo> SQLStringList)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        int num = 0;
                        foreach (CommandInfo current in SQLStringList)
                        {
                            string commandText = current.CommandText;
                            SqlParameter[] array = (SqlParameter[])current.Parameters;
                            SqlParameter[] array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                SqlParameter sqlParameter = array2[i];
                                if (sqlParameter.Direction == ParameterDirection.InputOutput)
                                {
                                    sqlParameter.Value = num;
                                }
                            }
                            DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, commandText, array);
                            int num2 = sqlCommand.ExecuteNonQuery();
                            array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                SqlParameter sqlParameter = array2[i];
                                if (sqlParameter.Direction == ParameterDirection.Output)
                                {
                                    num = Convert.ToInt32(sqlParameter.Value);
                                }
                            }
                            sqlCommand.Parameters.Clear();
                        }
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static void ExecuteSqlTranWithIndentity(Hashtable SQLStringList)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        int num = 0;
                        foreach (DictionaryEntry dictionaryEntry in SQLStringList)
                        {
                            string cmdText = dictionaryEntry.Key.ToString();
                            SqlParameter[] array = (SqlParameter[])dictionaryEntry.Value;
                            SqlParameter[] array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                SqlParameter sqlParameter = array2[i];
                                if (sqlParameter.Direction == ParameterDirection.InputOutput)
                                {
                                    sqlParameter.Value = num;
                                }
                            }
                            DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, cmdText, array);
                            int num2 = sqlCommand.ExecuteNonQuery();
                            array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                SqlParameter sqlParameter = array2[i];
                                if (sqlParameter.Direction == ParameterDirection.Output)
                                {
                                    num = Convert.ToInt32(sqlParameter.Value);
                                }
                            }
                            sqlCommand.Parameters.Clear();
                        }
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            object result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
                        object obj = sqlCommand.ExecuteScalar();
                        sqlCommand.Parameters.Clear();
                        if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                        {
                            result = null;
                        }
                        else
                        {
                            result = obj;
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }

        public static object GetSingle4Trans(CommandInfo commandInfo, SqlTransaction trans)
        {
            SqlCommand sqlCommand = new SqlCommand();
            string commandText = commandInfo.CommandText;
            SqlParameter[] cmdParms = (SqlParameter[])commandInfo.Parameters;
            DbHelperSQL.PrepareCommand(sqlCommand, trans.Connection, trans, commandText, cmdParms);
            object obj = sqlCommand.ExecuteScalar();
            sqlCommand.Parameters.Clear();
            object result;
            if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
            {
                result = null;
            }
            else
            {
                result = obj;
            }
            return result;
        }

        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString);
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader result;
            try
            {
                DbHelperSQL.PrepareCommand(sqlCommand, conn, null, SQLString, cmdParms);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                sqlCommand.Parameters.Clear();
                result = sqlDataReader;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return result;
        }

        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand();
                DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    DataSet dataSet = new DataSet();
                    try
                    {
                        sqlDataAdapter.Fill(dataSet, "ds");
                        sqlCommand.Parameters.Clear();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    result = dataSet;
                }
            }
            return result;
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                for (int i = 0; i < cmdParms.Length; i++)
                {
                    SqlParameter sqlParameter = cmdParms[i];
                    if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null)
                    {
                        sqlParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(sqlParameter);
                }
            }
        }

        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = DbHelperSQL.BuildQueryCommand(sqlConnection, storedProcName, parameters);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                DataSet dataSet = new DataSet();
                sqlConnection.Open();
                new SqlDataAdapter
                {
                    SelectCommand = DbHelperSQL.BuildQueryCommand(sqlConnection, storedProcName, parameters)
                }.Fill(dataSet, tableName);
                sqlConnection.Close();
                result = dataSet;
            }
            return result;
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, out int returnValue)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                DataSet dataSet = new DataSet();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                SqlCommand sqlCommand = DbHelperSQL.BuildQueryCommand(sqlConnection, storedProcName, parameters);
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataSet, tableName);
                returnValue = (int)sqlCommand.Parameters["ReturnValue"].Value;
                sqlConnection.Close();
                result = dataSet;
            }
            return result;
        }


        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand sqlCommand = new SqlCommand(storedProcName, connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < parameters.Length; i++)
            {
                SqlParameter sqlParameter = (SqlParameter)parameters[i];
                if (sqlParameter != null)
                {
                    if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null)
                    {
                        sqlParameter.Value = DBNull.Value;
                    }
                    sqlCommand.Parameters.Add(sqlParameter);
                }
            }
            return sqlCommand;
        }

        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = DbHelperSQL.BuildIntCommand(sqlConnection, storedProcName, parameters);
                rowsAffected = sqlCommand.ExecuteNonQuery();
                int num = (int)sqlCommand.Parameters["ReturnValue"].Value;
                result = num;
            }
            return result;
        }

        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand sqlCommand = DbHelperSQL.BuildQueryCommand(connection, storedProcName, parameters);
            sqlCommand.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return sqlCommand;
        }

        public static int ExecuteSql(string SQLString, string strConnectionString, params SqlParameter[] cmdParms)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(strConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
                        int num = sqlCommand.ExecuteNonQuery();
                        sqlCommand.Parameters.Clear();
                        result = num;
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }

        public static DataSet Query(string SQLString, string strConnectionString)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(strConnectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SQLString, sqlConnection);
                    sqlDataAdapter.Fill(dataSet, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                result = dataSet;
            }
            return result;
        }

        private static SqlParameter CreateParam(string ParamName, SqlDbType DbType, int Size, ParameterDirection Direction, object Value)
        {
            SqlParameter sqlParameter;
            if (Size > 0)
            {
                sqlParameter = new SqlParameter(ParamName, DbType, Size);
            }
            else
            {
                sqlParameter = new SqlParameter(ParamName, DbType);
            }
            sqlParameter.Direction = Direction;
            if (Direction != ParameterDirection.Output || Value != null)
            {
                sqlParameter.Value = Value;
            }
            return sqlParameter;
        }

        public static SqlParameter CreateInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return DbHelperSQL.CreateParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static SqlParameter CreateOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            return DbHelperSQL.CreateParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        public static SqlParameter CreateInputOutParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return DbHelperSQL.CreateParam(ParamName, DbType, Size, ParameterDirection.InputOutput, Value);
        }

        public static SqlParameter CreateReturnParam(string ParamName, SqlDbType DbType, int Size)
        {
            return DbHelperSQL.CreateParam(ParamName, DbType, Size, ParameterDirection.ReturnValue, null);
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dbTableName">数据库表名</param>
        /// <param name="dt">表(和数据库表中的列一一对应)</param>
        public static void SqlBulkCopyByDatatable(string dbTableName, DataTable dt)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //生成SqlBulkCopy 实例，构造函数指定了目标数据库，使用SqlBulkCopyOptions.UseInternalTransaction是指迁移动作指定在一个Transaction当中，如果数据迁移中产生错误或异常将发生回滚。  
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlbulkcopy.BulkCopyTimeout = 10000; //指定操作完成的Timeout时间  
                        sqlbulkcopy.DestinationTableName = dbTableName;
                        if (dt != null && dt.Rows.Count != 0)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                            }
                            sqlbulkcopy.WriteToServer(dt);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="sqlString">查询字符串</param>
        /// <param name="cmdParms">参数</param>
        /// <returns></returns>
        public static List<T> GetModelList<T>(string sqlString, params SqlParameter[] cmdParms) where T : new()
        {
            List<T> list = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    PrepareCommand(sqlCommand, conn, null, sqlString, cmdParms);
                    SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    sqlCommand.Parameters.Clear();
                    if (reader.HasRows)
                    {
                        Type type = typeof(T);
                        while (reader.Read())
                        {
                            T model = new T();
                            for (int i = 0, count = reader.FieldCount; i < count; i++)
                            {
                                var pi = type.GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                                if (pi == null) continue;
                                if (!string.Equals(reader.GetName(i), pi.Name, StringComparison.CurrentCultureIgnoreCase)) continue;
                                if (reader[i] == DBNull.Value) continue;
                                Type baseType = Nullable.GetUnderlyingType(pi.PropertyType);
                                pi.SetValue(model,
                                    baseType != null
                                        ? Convert.ChangeType(reader[i], baseType)
                                        : Convert.ChangeType(reader[i], pi.PropertyType), null);
                            }
                            list.Add(model);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static List<T> GetModelList<T>(string storedProcName, IDataParameter[] parameters) where T : new()
        {
            List<T> list = new List<T>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = BuildQueryCommand(sqlConnection, storedProcName, parameters))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.HasRows)
                    {
                        Type type = typeof(T);
                        while (reader.Read())
                        {
                            T model = new T();
                            for (int i = 0, count = reader.FieldCount; i < count; i++)
                            {
                                var pi = type.GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                                if (pi == null) continue;
                                if (!string.Equals(reader.GetName(i), pi.Name, StringComparison.CurrentCultureIgnoreCase)) continue;
                                if (reader[i] == DBNull.Value) continue;
                                Type baseType = Nullable.GetUnderlyingType(pi.PropertyType);
                                pi.SetValue(model,
                                    baseType != null
                                        ? Convert.ChangeType(reader[i], baseType)
                                        : Convert.ChangeType(reader[i], pi.PropertyType), null);
                            }
                            list.Add(model);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="sqlString">查询字符串</param>
        /// <param name="cmdParms">参数</param>
        /// <returns></returns>
        public static T GetModel<T>(string sqlString, params SqlParameter[] cmdParms) where T : new()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    PrepareCommand(sqlCommand, conn, null, sqlString, cmdParms);
                    SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    sqlCommand.Parameters.Clear();
                    if (reader.HasRows)
                    {
                        Type type = typeof(T);
                        if (!reader.Read()) return default(T);
                        T model = new T();
                        for (int i = 0, count = reader.FieldCount; i < count; i++)
                        {
                            var pi = type.GetProperty(reader.GetName(i),
                                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            if (pi == null) continue;
                            if (
                                !string.Equals(reader.GetName(i), pi.Name, StringComparison.CurrentCultureIgnoreCase))
                                continue;
                            if (reader[i] == DBNull.Value) continue;
                            Type baseType = Nullable.GetUnderlyingType(pi.PropertyType);
                            pi.SetValue(model,
                                baseType != null
                                    ? Convert.ChangeType(reader[i], baseType)
                                    : Convert.ChangeType(reader[i], pi.PropertyType), null);
                        }
                        return model;
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static T GetModel<T>(string storedProcName, IDataParameter[] parameters) where T : new()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = BuildQueryCommand(sqlConnection, storedProcName, parameters))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.HasRows)
                    {
                        Type type = typeof(T);
                        if (!reader.Read()) return default(T);
                        T model = new T();
                        for (int i = 0, count = reader.FieldCount; i < count; i++)
                        {
                            var pi = type.GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            if (pi == null) continue;
                            if (!string.Equals(reader.GetName(i), pi.Name, StringComparison.CurrentCultureIgnoreCase)) continue;
                            if (reader[i] == DBNull.Value) continue;
                            Type baseType = Nullable.GetUnderlyingType(pi.PropertyType);
                            pi.SetValue(model,
                                baseType != null
                                    ? Convert.ChangeType(reader[i], baseType)
                                    : Convert.ChangeType(reader[i], pi.PropertyType), null);
                        }
                        return model;
                    }
                }
            }
            return default(T);
        }

        #region 执行部分存储过程,通过指定数据库连接
        /// <summary>
        /// 通过指定数据连接去检索数据
        /// </summary>
        /// <param name="SQLString">检索语句</param>
        /// <param name="sqlConnection">数据库连接</param>
        public static DataSet QueryConn(string SQLString, SqlConnection sqlConnection)
        {
            DataSet result;
            {
                DataSet dataSet = new DataSet();
                try
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SQLString, sqlConnection);
                    sqlDataAdapter.Fill(dataSet, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                result = dataSet;
            }
            return result;
        }

        /// <summary>
        /// 通过指定数据连接去检索数据
        /// </summary>
        /// <param name="SQLString">检索语句</param>
        /// <param name="sqlConnection">数据库连接</param>
        /// <param name="cmdParms">参数</param>
        public static DataSet QueryConn(string SQLString, SqlConnection sqlConnection, params SqlParameter[] cmdParms)
        {
            DataSet result;

            SqlCommand sqlCommand = new SqlCommand();
            DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    sqlDataAdapter.Fill(dataSet, "ds");
                    sqlCommand.Parameters.Clear();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                result = dataSet;
            }

            return result;
        }

        /// <summary>
        /// 通过指定数据连接去执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">参数</param>
        /// <param name="tableName">表名</param>
        /// <param name="sqlConnection">数据库连接</param>
        public static DataSet RunProcedureConn(string storedProcName, IDataParameter[] parameters, string tableName, SqlConnection sqlConnection)
        {

            DataSet dataSet = new DataSet();
            new SqlDataAdapter
            {
                SelectCommand = DbHelperSQL.BuildQueryCommand(sqlConnection, storedProcName, parameters)
            }.Fill(dataSet, tableName);

            return dataSet;
        }

        /// <summary>
        /// 通过指定数据库连接去执行SQL语句
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="SQLString">返回受影响的行数</param>
        /// <param name="cmdParms">参数</param>
        /// <returns></returns>
        public static int ExecuteSqlConn(SqlConnection sqlConnection, string SQLString, params SqlParameter[] cmdParms)
        {
            int result;
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                try
                {
                    DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
                    int num = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                    result = num;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            return result;
        }

        #endregion

        #region 执行带事务sql  author 杨建辉  20170320
        /// <summary>
        /// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // Execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command
        /// </summary>
        /// <param name="command">The SqlCommand to be prepared</param>
        /// <param name="connection">A valid SqlConnection, on which to execute this command</param>
        /// <param name="transaction">A valid SqlTransaction, or 'null'</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        /// <param name="mustCloseConnection"><c>true</c> if the connection was opened by the method, otherwose is false.</param>
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        /// <summary>
        /// This method is used to attach array of SqlParameters to a SqlCommand.
        /// 
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// 
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of SqlParameters to be added to command</param>
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
        #endregion
    }
}
