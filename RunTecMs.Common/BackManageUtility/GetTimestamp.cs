using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.Common.DBUtility;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using RunTecMs.Common.ConvertUtility;

namespace RunTecMs.Common.BackManageUtility
{
  public class GetTimestamp
    {
      public GetTimestamp()
      {  
      
      
      }
        /// <summary>
        /// 获取表中最大的时间戳
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
      public static string GetTimesTamp(string tabName)
      {
          StringBuilder sb = new StringBuilder();
          sb.AppendLine("  SELECT  MAX(timestamp)  FROM " + tabName);
          sb.AppendLine(" where 1=1 ");
          DataTable dt = DbHelperSQL.Query(sb.ToString()).Tables[0];
          List<timestamp> list = ConvertToList.DataTableToList<timestamp>(dt).ToList();
          string Timestamp = BitConverter.ToString(list[0].TimeStamp);
          return Timestamp;
      }
      /// <summary>
      /// 获取表中当前的时间戳
      /// </summary>
      /// <param name="tabName"></param>
      /// <returns></returns>
      public static string GetTimesTamp(string tabName, string TableID, string CurrentID)
      {
          StringBuilder sb = new StringBuilder();
          List<SqlParameter> listPara = new List<SqlParameter>();
          sb.AppendLine("  SELECT  timestamp  FROM " + tabName);
          sb.AppendLine("  where " + TableID + "=  @CurrentID ");
          listPara.Add(new SqlParameter("@CurrentID", int.Parse(CurrentID)));
          DataTable dt = DbHelperSQL.Query(sb.ToString(), listPara.ToArray()).Tables[0];
          List<timestamp> list = ConvertToList.DataTableToList<timestamp>(dt).ToList();
          string Timestamp = BitConverter.ToString(list[0].TimeStamp);
          return Timestamp;
      }
    }

  /// <summary>
  /// 时间戳
  /// </summary>
  public class timestamp
  {
      public byte[] TimeStamp{get;set;}
  }
}
