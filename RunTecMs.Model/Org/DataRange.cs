using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.ORG
{
    /// <summary>
    /// 数据范围
    /// </summary>
    [Serializable]
  public class Per_DataRange
    { 
        /// <summary>
        /// 数据范围id  
        /// </summary>
        public int DataRangeID { get; set; }
        /// <summary>
        /// 数据范围名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据范围值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderValue { get; set; }
        /// <summary>
        /// 数据范围说明
        /// </summary>
        public string DataRange { get; set; }
        /// <summary>
        /// 超管标识
        /// </summary>
        public Boolean IsSYSDBA { get; set; }
    }
}
