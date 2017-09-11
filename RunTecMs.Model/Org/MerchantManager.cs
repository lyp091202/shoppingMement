using System;

namespace RunTecMs.Model.ORG
{
   /// <summary>
   /// 商户管理
   /// </summary>
   [Serializable]
   public class MerchantManager
   {
      /// <summary>
      /// 商户ID
      /// </summary>
      public int MerchantID { get; set; }
      /// <summary>
      /// 业务ID
      /// </summary>
      public int BusinessValue { get; set; }
      /// <summary>
      /// 商户名
      /// </summary>
      public string MerchantName { get; set; }
      /// <summary>
      /// 密码
      /// </summary>
      public string Password { get; set; }
      /// <summary>
      /// 密码盐
      /// </summary>
      public string Passwordsalt { get; set; }
      /// <summary>
      /// 商户账户
      /// </summary>
      public string MerchantAccount { get; set; }
      /// <summary>
      /// 商户类型ID
      /// </summary>
      public int MerchantTypeID { get; set; }
      /// <summary>
      /// 手续费比例
      /// </summary>
      public decimal Poundage { get; set; }
      /// <summary>
      /// 打款方式
      /// </summary>
      public bool PaymentType { get; set; }
      /// <summary>
      /// 自动打款时间
      /// </summary>
      public DateTime PaymentTime { get; set; }
      /// <summary>
      /// 商户账户类型ID
      /// </summary>
      public int MerchantAccountTypeID { get; set; }
      /// <summary>
      /// 是否正在使用
      /// </summary>
      public bool Isusing { get; set; }
      /// <summary>
      /// 地址
      /// </summary>
      public string Address { get; set; }
      /// <summary>
      /// 联系电话
      /// </summary>
      public string Mobile { get; set; }
      /// <summary>
      /// 邮箱
      /// </summary>
      public string Email { get; set; }
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
      public DateTime? UpdateTime { get; set; }
      /// <summary>
      /// 更新者
      /// </summary>
      public string UpdateUser { get; set; }
      /// <summary>
      /// 时间戳
      /// </summary>
      public byte[] timestamp { get; set; }
   }
    /// <summary>
    /// 商户管理info
    /// </summary>
   public class MerchantManagerInfo:MerchantManager
   {
       /// <summary>
       /// 业务名称
       /// </summary>
       public string BusinessName { get; set; }
       /// <summary>
       ///  商户类型名称
       /// </summary>
       public string MerchantTypeName { get; set; }
   }


}
