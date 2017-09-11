using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.Common
{
    /// <summary>
    /// 文件导出
    /// </summary>
    public class Export
    {
        public string[] ColumnsName { get; set; }
        public string FileName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
