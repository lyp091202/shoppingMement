using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.Common
{
    /// <summary>
    /// 描述：系统数据字典实体类
    /// create by 杨建辉 20170316
    /// </summary>
    public class WorkBookModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int rowid { get; set; }
        /// <summary>
        /// 数据字典Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
