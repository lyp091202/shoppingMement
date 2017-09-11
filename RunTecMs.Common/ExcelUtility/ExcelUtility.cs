using System;
using System.Reflection; // 引用这个才能使用Missing字段 
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;


namespace RunTecMs.Common.ExcelUtility
{
    // 输出文件模型
    public enum FileModelType
    {
        // 讲师收入
        lectureIncome = 1
    }

    public class ExcelUtility
    {

        Application outApp = new Application();
        Workbook xBook;

        // 执行工程绝对路径取得
        string modelFilePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        string modelFileName = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExcelUtility()
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

            if (outApp == null)
            {
                outApp = new Application();
            }

            outApp.Visible = false;
            if (xBook == null)
            {
                // 打开工作簿
                xBook = outApp.Workbooks._Open(fileName,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            }

            string[] str = new string[endRow - startRow];

            // 指定读取sheet页
            Worksheet xSheet = (Worksheet)xBook.Sheets[Sheet];

            // 读取指定内容
            for (int i = startRow; i < endRow; i++)
            {
                Range ran = (Range)xSheet.Cells[i, colum];
                if (ran.Value2 != null)
                {
                    str[i - startRow] = Convert.ToString(ran.Value2);
                }
            }

            return str;
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="SheetsNum">sheet页索引</param>
        /// <param name="dt">输出的DataTable</param>
        /// <param name="startRow">输出开始行</param>
        /// <param name="startCol">输出开始列</param>
        /// <param name="outCols">输出列数</param>
        /// <returns></returns>
        public bool outToExcel(string fileName, int SheetsNum, System.Data.DataTable dt, int startRow, int startCol, int outCols)
        {
            bool errorFlg = false;
            // 输出行数
            int rowNumber = dt.Rows.Count;
            int columnNumber = dt.Columns.Count;

            if (outApp == null)
            {
                outApp = new Application();
            }
            if (rowNumber == 0)
            {
                xBook = null;
                //Kill(outApp);
                outApp.Quit(); //这一句是非常重要的，否则Excel对象不能从内存中退出 
                PublicMethod.Kill(outApp);//调用kill当前excel进程
                outApp = null;
                //没有数据可以导入
                return true;
            }

            outApp.Visible = false;
            if (xBook == null || !fileName.Contains(outApp.Workbooks.get_Item(1).Name))
            {
                // 打开工作簿
                xBook = outApp.Workbooks._Open(fileName,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            }
            // 指定输出sheet
            Worksheet xSheet = (Worksheet)xBook.Sheets[SheetsNum];

            try
            {
                //输出列>DataTable列时，按照DataTable列输出
                if (outCols > columnNumber)
                {
                    outCols = columnNumber;
                }

                for (int c = 0; c < rowNumber; c++)
                {
                    for (int j = 0; j < outCols; j++)
                    {
                        xSheet.Cells[c + startRow, j + startCol] = dt.Rows[c].ItemArray[j];
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog("导入Excel时出错" + ex);
                errorFlg = true;
            }
            finally
            {
                if (errorFlg == false)
                {
                    xBook.Save();
                }

                xBook = null;
                //Kill(outApp);
                outApp.Quit(); //这一句是非常重要的，否则Excel对象不能从内存中退出 
                PublicMethod.Kill(outApp);//调用kill当前excel进程
                outApp = null;
            }

            return true;
        }

        /// <summary>
        /// 复制指定sheet，并设定内容
        /// </summary>
        /// <param name="DT">输出内容</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="sheetName">复制的sheet名</param>
        /// <param name="fromIndex">复制源sheet索引</param>
        /// <param name="toIndex">复制sheet索引</param>
        /// <returns></returns>
        public bool CopyExcelSheet(System.Data.DataTable DT, string fileName, string sheetName, int fromIndex, int toIndex)
        {
            bool errorFlg = false;

            if (outApp == null)
            {
                outApp = new Application();
                outApp.Visible = false;

            }

            if (xBook == null || !fileName.Contains(outApp.Workbooks.get_Item(1).Name))
            {
                // 打开工作簿
                xBook = outApp.Workbooks._Open(fileName,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                outApp.Application.Workbooks.Add(true);
            }

            try
            {
                int rowNumber = DT.Rows.Count;
                int columnNumber = DT.Columns.Count;

                Worksheet templetSheet = (Worksheet)xBook.Sheets.get_Item(fromIndex);

                Worksheet copySheet = (Worksheet)xBook.Sheets[toIndex - 1];
                (templetSheet).Copy(System.Reflection.Missing.Value, copySheet);
                Worksheet newSheet = (Worksheet)xBook.Sheets.get_Item(toIndex);
                newSheet.Name = sheetName.ToString();

                for (int i = 0; i < rowNumber; i++)
                {
                    for (int j = 0; j < columnNumber - 1; j++)
                    {
                        newSheet.Cells[i + 2, j + 1] = DT.Rows[i].ItemArray[j];
                    }
                }
            }
            catch (Exception ex)
            {
                errorFlg = true;
                LogRecord.WriteLog("复制指定sheet时出错" + ex);
            }
            finally
            {
                if (errorFlg == false)
                {
                    outApp.DisplayAlerts = false;
                    xBook.Save();
                    outApp.DisplayAlerts = true;
                }

                xBook = null;
                outApp.Quit(); //这一句是非常重要的，否则Excel对象不能从内存中退出 
                PublicMethod.Kill(outApp);//调用kill当前excel进程
                outApp = null;
            }

            return true;
        }

        /// <summary>
        /// 复制文件并复制sheet页
        /// </summary>
        /// <param name="fileName">输出文件名</param>
        /// <returns></returns>
        public bool CreateFile(string fileType, string fileName)
        {
            if (Convert.ToString(FileModelType.lectureIncome).Equals(fileType))
            {
                // 复制文件
                FileUtility.DirFile.CopyFileAllPath(modelFileName, @fileName);
            }
            return true;
        }
        /// <summary>
        /// 读取excel数据行数
        /// </summary>
        /// <param name="fileName">excel文件</param>
        /// <param name="Sheet">读取sheet页索引</param>
        /// <returns>dataCnt</returns>
        public int GetRowCount(string fileName, int Sheet)
        {
            if (xBook == null)
            {
                // 打开工作簿
                xBook = outApp.Workbooks._Open(fileName,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            }
            // 指定读取sheet页
            Worksheet xSheet = (Worksheet)xBook.Sheets[Sheet];

            int c2 = xSheet.UsedRange.CurrentRegion.Rows.Count;
            return c2;
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

            if (outApp == null)
            {
                outApp = new Application();
            }

            outApp.Visible = false;
            System.Data.DataTable dt = new System.Data.DataTable();
            if (xBook == null)
            {
                // 打开工作簿
                xBook = outApp.Workbooks._Open(fileName,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                         Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            }
            // 指定读取sheet页
            Worksheet xSheet = (Worksheet)xBook.Sheets[Sheet];
            startRow = startRow == 0 ? 1 : startRow;
            startCol = startCol == 0 ? 1 : startCol;
            endRow = endRow == 0 ? xSheet.UsedRange.Rows.Count : endRow;
            endCol = endCol == 0 ? xSheet.UsedRange.Columns.Count : endCol;

            // 生成列头
            for (int i = startCol, j = 0; i < endCol + 1; i++)
            {
                DataColumn column = new DataColumn("column" + j);
                dt.Columns.Add(column);
                j++;
            }

            // string[,] str = new string[endRow - startRow, endCol - startCol];
            Range ran;
            // 生成行数据
            // 读取指定行内容
            for (int i = startRow; i < endRow + 1; i++)
            {
                DataRow dr = dt.NewRow();
                // 读指定列内容
                for (int j = startCol, p = 0; j < endCol + 1; j++)
                {

                    ran = (Range)xSheet.Cells[i, j];
                    if (ran.Value2 != null)
                    {
                        //str[i - startRow, j - startCol] = Convert.ToString(ran.Value2);
                        dr[p] = Convert.ToString(ran.Value2);
                        p++;
                    }
                    else
                    {
                        dr[p] = "";
                        p++;
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        //public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);  
        ///// <summary>
        ///// 杀掉Excel进程
        ///// </summary>
        ///// <param name="excel"></param>
        //public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        //{
        //    excel.Quit();
        //    IntPtr t = new IntPtr(excel.Hwnd);
        //    int k = 0;
        //    GetWindowThreadProcessId(t, out k);
        //    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
        //    p.Kill();
        //}


    }
}
