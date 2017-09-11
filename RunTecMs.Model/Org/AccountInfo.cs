using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.ORG
{
    /// <summary>
    /// 账户信息
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public int MerchantID { get; set; }
        /// <summary>
        /// 已到账金额
        /// </summary>
        public int EarnedRevenueAmount { get; set; }
        /// <summary>
        /// 未到账金额
        /// </summary>
        public int UnearnedRevenueAmount { get; set; }
        /// <summary>
        /// 讲师结算金额
        /// </summary>
        public int LectureBalanceAmount { get; set; }
    }

    /// <summary>
    /// 收入一览
    /// </summary>
    public class IncomeInfo
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public int MerchantID { get; set; }
        /// <summary>
        /// 问答金额
        /// </summary>
        public int QuestionAmount { get; set; }
        /// <summary>
        /// 百宝箱金额
        /// </summary>
        public int GoodsAmount { get; set; }
        /// <summary>
        /// 打赏金额
        /// </summary>
        public int TipAmount { get; set; }
    }
}
