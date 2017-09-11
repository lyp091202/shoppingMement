using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.Common
{
    /// <summary>
    /// 配置问价
    /// </summary>
    public class Config
    {
        public int ConfigID { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        public string ConfigKey { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string ConfigValue { get; set; }
        /// <summary>
        /// 超管标识
        /// </summary>
        public int IsSYSDBA { get; set; }
    }
}
