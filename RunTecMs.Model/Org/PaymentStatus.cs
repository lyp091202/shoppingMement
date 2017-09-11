using System;

namespace RunTecMs.Model.ORG
{
   /// <summary>
   /// 结算状态
   /// </summary>
   public class PaymentStatus
   {
      /// <summary>
      /// 状态ID
      /// </summary>
      public int StatusID { get; set; }
      /// <summary>
      /// 状态名
      /// </summary>
      public string StatusName { get; set; }
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
      public DateTime? UpdateTime { get; set; }
      /// <summary>
      /// 更新者
      /// </summary>
      public string UpdateUser { get; set; }
      /// <summary>
      /// 时间戳
      /// </summary>
      public byte[] timestamp { get; set; }
      /// <summary>
      /// 
      /// </summary>
      public bool IsSYSDBA { get; set; }
   }

   public class PaymentData
   {
      public string IncomeTime { get; set; }
      public int MerchantId { get; set; }
      public int MerchantType { get; set; }
      public string DetailId { get; set; }
      public int PaymentStatus { get; set; }
      public DateTime? PaymentTime { get; set; }
      public DateTime? EndTime { get; set; }
      public DateTime? ConfirmTime { get; set; }
      public string DismissInfo { get; set; }
   }
}
