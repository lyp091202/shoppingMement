using System;

namespace RunTecMs.Model.ORG
{
    /// <summary>
    /// 导出相关
    /// </summary>
    public class MerchantExport
    {
        public static string MerchantPaymentInfo = "商户结算信息";
        public static string MerchantPaymentHistory = "商户结算履历";
        public static string LecturePaymentInfo = "大咖结算信息";
        public static string LecturePaymentHistory = "大咖结算履历";
        public static string IncomeTitle = "收入一览";
        public static string IncomeDetailTitle = "收入详情";
        public static string[] IncomeColumnsName = { "时间", "直接到账", "未结算", "已结算", "讲师结算" };
        public static string[] IncomeDetailColumnsName = { "时间", "次数", "发贝", "元", "已到账(发贝)", "已到账(元)", "直接到账(发贝)", "直接到账(元)", "未结算(发贝)", "未结算(元)", "已结算(发贝)", "已结算(元)", "未结算(发贝)", "未结算(元)", "已结算(发贝)", "已结算(元)" };
        public static string[] IncomeMerchantDetailColumnsName = { "时间", "次数", "发贝", "已到账(发贝)", "直接到账(发贝)", "未结算(发贝)", "未结算(发贝)", "已结算(发贝)", "已结算(发贝)" };
        public static string[] MerchantPaymentColumnsName = { "商户名", "收入时间", "本次结算(单位:发贝)", "已结算(单位:发贝)", "提交结算时间", "结算时间", "结算状态" };
        public static string[] LecturePaymentColumnsName = { "讲师姓名", "手机号", "收入时间", "应结算(单位:发贝)", "打赏", "百宝箱", "问答" };
        public static string[] LecturePaymentHistoryColumnsName = { "讲师姓名", "手机号", "已结算(单位:发贝)", "收入时间", "结算时间", "结算状态", "打赏", "百宝箱", "问答" };
        public static string[] MerchantPaymentHistoryColumnsName = { "商户名", "收入时间", "已结算(单位:发贝)", "未结算(单位:发贝)", "提交时间", "结算时间", "结算状态" };
    }
}
