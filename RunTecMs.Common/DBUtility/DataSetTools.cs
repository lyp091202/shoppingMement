using System.Data;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace RunTecMs.Common
{
    public class DataSetTools
    {
        public static DataRowCollection GetDataSetRows(DataSet ds)
        {
            DataRowCollection result;
            if (ds.Tables.Count > 0)
            {
                result = ds.Tables[0].Rows;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static bool DataSetIsNull(DataSet ds)
        {
            return ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Columns.Count == 0;
        }

    }
}
