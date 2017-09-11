using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection; // 引用这个才能使用Missing字段 
//using Spire.Xls;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;


namespace RunTecMs.Common.NExcelUtility
{
    // 输出文件模型
    public enum FileModelType
    {
        // 讲师收入
        lectureIncome = 1
    }

    public class NExcelUtility
    {
        // 替换EXCEL处理
        IWorkbook xBook;

        // 执行工程绝对路径取得
        string modelFilePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        string modelFileName = "";
        private FileStream fs = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NExcelUtility()
        {
            modelFileName = modelFilePath + "讲师收入一览模板.xlsx";
        }


        /// <summary>
        ///读取excel数据(指定sheet,开始行，结束行，列)
        /// </summary>
        /// <param name="Sheet">读取sheet页索引</param>
        /// <param name="startRow">开始行</param>
        /// <param name="endRow">结束行</param>
        /// <param name="colum">指定列号</param>
        /// <returns></returns>
        public string[] readExcel(string fileName, int Sheet, int startRow, int endRow, int colum)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;

            string[] str = new string[endRow - startRow];

            fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
            {
                workbook = new XSSFWorkbook();
            }
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
            {
                workbook = new HSSFWorkbook();
            }

            fs.Close();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.GetSheetAt(Sheet);
                }
                else
                {
                    return null;
                }

                 // 读取指定内容
                IRow row;
                for (int i = startRow; i < endRow + 1; i++)
                {
                    row = sheet.GetRow(i);  //读取当前行数据
                    var value = row.GetCell(colum);
                    if (value != null)
                    {
                        str[i - startRow] = Convert.ToString(value);
                    }
                }

            }
            catch (Exception e)
            {
                //只在Debug模式下才输出
                Console.WriteLine(e.Message);
            }
            return str;
        }


        /// <summary>
        /// 读取excel数据(文件名, 指定sheet, 开始行，结束行，开始列，结束列)
        /// </summary>
        /// <param name="fileName">excel文件</param>
        /// <param name="Sheet">读取sheet页索引</param>
        /// <param name="startRow">开始行</param>
        /// <param name="endRow">结束行</param>
        /// <param name="startCol">开始列</param>
        /// <param name="endCol">结束列</param>
        /// <returns>datatable</returns>
        public System.Data.DataTable readExcel(string fileName, int Sheet, int startRow = 0, int endRow = 0, int startCol = 0, int endCol = 0)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            IWorkbook workbook = null;
            ISheet sheet = null;

            fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
            {
                workbook = new XSSFWorkbook(fs);
            }
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
            {
                workbook = new HSSFWorkbook(fs);
            }

            fs.Close();

            try
            {
                
                if (workbook != null)
                {
                    sheet = workbook.GetSheetAt(Sheet);
                }
                else
                {
                    return null;
                }

                startRow = startRow == 0 ? 1 : startRow;
                startCol = startCol == 0 ? 1 : startCol;
                endRow = endRow == 0 ? sheet.LastRowNum : endRow;
                endCol = endCol == 0 ? sheet.GetRow(0).LastCellNum: endCol;

                // 生成列头
                for (int i = startCol; i < endCol + 1; i++)
                {
                    DataColumn column = new DataColumn("column" + (i - startCol));
                    dt.Columns.Add(column);
                }

                // 生成行数据
                // 读取指定行内容
                IRow row;
                for (int i = startRow; i < endRow + 1; i++)
                {
                    DataRow dr = dt.NewRow();
                    row = sheet.GetRow(i);  //读取当前行数据
                    // 读指定列内容
                    for (int j = startCol; j < endCol + 1; j++)
                    {
                        var value = row.GetCell(j);
                        if (value != null)
                        {
                            dr[j - startCol] = Convert.ToString(value).Trim();
                        }
                        else
                        {

                            dr[j - startCol] = "";
                        }
                    }
                    dt.Rows.Add(dr);
                }

            }
            catch (Exception e)
            {
                //只在Debug模式下才输出
                Console.WriteLine(e.Message);
            }

            return dt;
        }

        ///// <summary>
        ///// 输出文件
        ///// </summary>
        ///// <param name="fileName">文件名</param>
        ///// <param name="SheetsNum">sheet页索引</param>
        ///// <param name="dt">输出的DataTable</param>
        ///// <param name="startRow">输出开始行</param>
        ///// <param name="startCol">输出开始列</param>
        ///// <param name="outCols">输出列数</param>
        ///// <returns></returns>
        //public bool outToExcel(string fileName, int SheetsNum, System.Data.DataTable dt, int startRow, int startCol, int outCols)
        //{
        //    bool errorFlg = false;
        //    // 输出行数
        //    int rowNumber = dt.Rows.Count;
        //    int columnNumber = dt.Columns.Count;

        //    Workbook workbook = new Workbook();
        //    workbook.LoadFromFile(fileName);
        //    Worksheet sheet = workbook.Worksheets[SheetsNum];

        //    try
        //    {
        //        //输出列>DataTable列时，按照DataTable列输出
        //        if (outCols > columnNumber)
        //        {
        //            outCols = columnNumber;
        //        }

        //        for (int c = 0; c < rowNumber; c++)
        //        {
        //            for (int j = 0; j < outCols; j++)
        //            {
        //                sheet.Range[c + startRow, j + startCol].Text = Convert.ToString(dt.Rows[c].ItemArray[j]);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //LogRecord.WriteLog("导入Excel时出错" + ex);
        //        errorFlg = true;
        //    }
        //    finally
        //    {
        //        if (errorFlg == false)
        //        {
        //            xBook.Save();
        //        }

        //        xBook = null;
        //    }

        //    return true;
        //}

        ///// <summary>
        ///// 复制指定sheet，并设定内容
        ///// </summary>
        ///// <param name="DT">输出内容</param>
        ///// <param name="fileName">输出文件名</param>
        ///// <param name="sheetName">复制的sheet名</param>
        ///// <param name="fromIndex">复制源sheet索引</param>
        ///// <param name="toIndex">复制sheet索引</param>
        ///// <returns></returns>
        //public bool CopyExcelSheet(System.Data.DataTable DT, string fileName, string sheetName, int fromIndex, int toIndex)
        //{
        //    bool errorFlg = false;

        //    Workbook workbook = new Workbook();
        //    workbook.LoadFromFile(fileName);

        //    try
        //    {
        //        int rowNumber = DT.Rows.Count;
        //        int columnNumber = DT.Columns.Count;

        //        Worksheet templetSheet = workbook.Worksheets[fromIndex];
        //        Worksheet newsheet = (Worksheet)templetSheet.Clone(templetSheet.Parent);
        //        workbook.Worksheets.Add(newsheet);

        //        newsheet.Name = sheetName.ToString();

        //        for (int i = 0; i < rowNumber; i++)
        //        {
        //            for (int j = 0; j < columnNumber - 1; j++)
        //            {
        //                newsheet.Range[i + 2, j + 1].Text = Convert.ToString(DT.Rows[i].ItemArray[j]);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorFlg = true;
        //        //LogRecord.WriteLog("复制指定sheet时出错" + ex);
        //    }
        //    finally
        //    {
        //        if (errorFlg == false)
        //        {
        //            xBook.Save();
        //        }

        //        xBook = null;
        //    }

        //    return true;
        //}

        ///// <summary>
        ///// 复制文件并复制sheet页
        ///// </summary>
        ///// <param name="fileName">输出文件名</param>
        ///// <returns></returns>
        //public bool CreateFile(string fileType, string fileName)
        //{
        //    if (Convert.ToString(FileModelType.lectureIncome).Equals(fileType))
        //    {
        //        // 复制文件
        //        FileUtility.DirFile.CopyFileAllPath(modelFileName, @fileName);
        //    }
        //    return true;
        //}
        ///// <summary>
        ///// 读取excel数据行数
        ///// </summary>
        ///// <param name="fileName">excel文件</param>
        ///// <param name="Sheet">读取sheet页索引</param>
        ///// <returns>dataCnt</returns>
        //public int GetRowCount(string fileName, int Sheet)
        //{
        //    Workbook workbook = new Workbook();
        //    workbook.LoadFromFile(fileName);
        //    Worksheet xSheet = workbook.Worksheets[Sheet];

        //    int c2 = xSheet.Range["A"].RowCount;
        //    return c2;
        //}

    }
}
