using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RunTecMs.Common.ExcelUtility;
using System.Reflection;
using System.Data;

namespace RunTecMs.Common.CsvUtility
{
    public class DataTableToCsv
    {
       /// <summary>
       /// 导出
       /// </summary>
       /// <param name="ColumnsName">一览表头</param>
       /// <param name="dt">一览数据</param>
       /// <param name="listDtColumnsName">详情表头</param>
       /// <param name="dtlist">详情数据</param>
       /// <param name="fileName">文件名</param>
       /// <returns></returns>
        private static bool SaveDataToCSV(string[] ColumnsName, DataTable dt, string[] listDtColumnsName,List<DataTable> dtlist, string fileName)
        {
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            bool successFlag = true;
            // 详情存在Flg
            bool detailFlag = true;
            // 详情列表
            DataTable detaildt = null;
            try
            {
                if (dtlist == null || dtlist.Count == 0)
                {
                    detailFlag = false;
                }

                string data = "";
                //一览的表头
                data=ExprotColumnsName(ColumnsName);
                sw.WriteLine(data);
                //一览的数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        data += dt.Rows[i][j].ToString();
                        if (j < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                    //如果一览存在详情则导入详情
                    if (detailFlag)
                    {
                        //详情的表头
                        data = "";
                        data = ExprotColumnsName(listDtColumnsName);
                        sw.WriteLine(data);
                        //详情的数据
                        detaildt = dtlist[i];
                        for (int m = 0; m < detaildt.Rows.Count; m++)
                        {
                            data = "";
                            for (int d = 0; d < detaildt.Columns.Count; d++)
                            {
                                data += detaildt.Rows[m][d].ToString();
                                if (d < detaildt.Columns.Count - 1)
                                {
                                    data += ",";
                                }
                            }
                            sw.WriteLine(data);
                        }


                        //每行一览加入表头
                        if (i < dt.Rows.Count-1)
                        {
                            data = "";
                            //一览的表头
                            data = ExprotColumnsName(ColumnsName);
                            sw.WriteLine(data);
                        }
                    }

                }
            
            }
            catch (Exception ex)
            {

                LogRecord.WriteLog("导出错误" + ex.Message);
                successFlag = false;
            }

            finally
            {
                sw.Close();
                fs.Close();
            }
            return successFlag;
        }
        /// <summary>
        /// 遍历表头
        /// </summary>
        /// <param name="ColumnsName">表头名</param>
        /// <returns></returns>
        private static string ExprotColumnsName(string[] ColumnsName)
        {
            //一览的表头
            string data = "";
            for (int i = 0; i < ColumnsName.Length; i++)
            {
                data += ColumnsName[i].ToString();
                if (i < ColumnsName.Length - 1)
                {
                    data += ",";
                }
            }
            return data;
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="ColumnsName">一览列名</param>
        /// <param name="DT">一览的DataTable</param>
        /// <param name="listDt">详情的数据</param>
        /// <param name="listDtColumnsName">详情的列名</param>
        /// <returns></returns>
        public static bool EricExportCSVData(string filePath, string[] ColumnsName, DataTable DT, List<DataTable> listDt, params string[] listDtColumnsName)
        {
            bool Saveflag = SaveDataToCSV(ColumnsName, DT, listDtColumnsName, listDt, filePath);
            return Saveflag;
        }
    }
}




