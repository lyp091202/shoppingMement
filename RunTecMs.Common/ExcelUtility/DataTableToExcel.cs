using System;
using System.Reflection; // 引用这个才能使用Missing字段 
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;

namespace RunTecMs.Common.ExcelUtility
{
    public class DataTableToExcelComm
    {

        /// <summary>
        /// 表数据导出到Excel
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="ColumnsName"></param>
        private MemoryStream DataTableToExcel(System.Data.DataTable datas, string[] ColumnsName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);

            // 设置表头
            foreach (DataColumn column in datas.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(ColumnsName[column.Ordinal]);
            }

            int rowIndex = 1;

            foreach (DataRow row in datas.Rows)
            {
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                foreach (DataColumn column in datas.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }

        /// <summary>
        /// Excel导出功能
        /// </summary>
        /// <param name="SourceTable">原数据</param>
        /// <param name="FileName">文件名</param>
        /// <param name="ColumnsName">表头名</param>
        public bool DataTableToExcel(System.Data.DataTable SourceTable, string FileName, string[] ColumnsName)
        {
            try
            {
                // 全路径
                string filePath = System.Web.HttpContext.Current.Server.MapPath("/") + "Upload/" + FileName + ".xls";
                // 保存文件（如果存在，先删除）
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                // 保存数据
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                // 获取数据流
                MemoryStream ms = DataTableToExcel(SourceTable, ColumnsName);

                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
                fs = null;
                data = null;
                ms = null;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        } 
    }
}
