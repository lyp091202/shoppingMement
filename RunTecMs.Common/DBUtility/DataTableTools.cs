using System.Data;

namespace RunTecMs.Common
{
    public class DataTableTools
    {
        public static bool DataTableIsNull(DataTable dt)
        {
            return dt == null || dt.Columns.Count == 0 || dt.Rows.Count == 0;
        }
    }
}
