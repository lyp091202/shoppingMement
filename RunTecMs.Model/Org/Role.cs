using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.FaFa
{

  [Serializable]
   public class Role
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 数据范围ID
        /// </summary>
        public int DataRangeID { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderValue { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Descrption { get; set; }
        /// <summary>
        /// 超管标识
        /// </summary>
        public string IsSYSDBA { get; set; }



    }
}
