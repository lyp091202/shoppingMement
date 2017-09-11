using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.ORG
{
    /// <summary>
    /// 账户信息图
    /// </summary>
    public class AccountInfoChart
    {
        /// <summary>
        /// 直接到账发贝
        /// </summary>
        public int ArrivalAccountFacoin { get; set; }
        /// <summary>
        ///  直接到账人民币
        /// </summary>
        public decimal ArrivalAccountRMB { get; set; }
        /// <summary>
        /// 商户未结算发贝(未提交,已提交,驳回)
        /// </summary>
        public int MerchantNoSettlementFacoin { get; set; }
        /// <summary>
        /// 商户未结算人民币(未提交,已提交,驳回)
        /// </summary>
        public decimal MerchantNoSettlementRMB { get; set; }
        /// <summary>
        /// 商户已结算发贝
        /// </summary>
        public int MerchantSettledFacoin { get; set; }
        /// <summary>
        /// 商户已结算人民币
        /// </summary>
        public decimal MerchantSettledRMB { get; set; }
        /// <summary>
        /// 讲师未结算发贝(未提交,已提交)
        /// </summary>
        public int LectureNoSettlementFacoin { get; set; }
        /// <summary>
        /// 讲师未结算人民币
        /// </summary>
        public decimal LectureNoSettlementRMB { get; set; }
        /// <summary>
        /// 讲师已结算发贝
        /// </summary>
        public int LectureSettledFacoin { get; set; }
        /// <summary>
        /// 讲师已结算人民币
        /// </summary>
        public decimal LectureSettledRMB { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
    }

    public class IncomeChart
    {
        public int ArrivalAccountFacoin { get; set; }
        public int MerchantNoSettlementFacoin { get; set; }
        public int MerchantSettledFacoin { get; set; }
        public int LectureNoSettlementFacoin { get; set; }
        public int LectureSettledFacoin { get; set; }
        public int ArrivalCount { get; set; }
        public int MerchantNoSettlementCount { get; set; }
        public int MerchantSettledCount { get; set; }
        public int LectureNoSettlementCount { get; set; }
        public int LectureSettledCount { get; set; }
        public string Date { get; set; }
    }
}
