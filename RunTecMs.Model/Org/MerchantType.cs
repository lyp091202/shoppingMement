using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.ORG
{
    /// <summary>
    /// 
    /// </summary>
   public  class MerchantType
    {
       /// <summary>
        /// 商户类型ID
       /// </summary>
       public int MerchantTypeID { get; set; }
       /// <summary>
       /// 商户类型名
       /// </summary>
       public string MerchantTypeName { get; set; }
       /// <summary>
       /// 说明
       /// </summary>
       public string Description { get; set; }
       /// <summary>
       /// 追加时间
       /// </summary>
       public DateTime AddTime { get; set; }
       /// <summary>
       /// 追加者
       /// </summary>
       public string AddUser { get; set; }
       /// <summary>
       /// 更新时间
       /// </summary>
       public DateTime UpdateTime { get; set; }
       /// <summary>
       /// 更新者
       /// </summary>
       public string UpdateUser { get; set; }
       /// <summary>
       /// 时间戳
       /// </summary>
       public byte[] timestamp { get; set; }
       /// <summary>
       /// 超管标识
       /// </summary>
       public bool IsSYSDBA { get; set; }
    }

}
