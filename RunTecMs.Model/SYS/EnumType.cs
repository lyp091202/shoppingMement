using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.SYS
{
    /// <summary>
    /// 问题类型操作
    /// </summary>
    public enum QuestionTypeAction
    {
        /// <summary>
        /// 所有问题类型
        /// </summary>
        All = 0,

        /// <summary>
        /// 1对1
        /// </summary>
        OneToOne = 1,

        /// <summary>
        /// 1对3
        /// </summary>
        OneToThree = 2,

        /// <summary>
        /// 专属问
        /// </summary>
        Exclusive = 3

    }

    /// <summary>
    /// 未回答数
    /// </summary>
    public enum UnansweredCount
    {
        One = 1,

        Two = 2,

        ALL = 3,

    }

    public enum ExamineAndApproveStatus
    {
        /// <summary>
        /// 发起申请
        /// </summary>
        Applystart = 1,

        /// <summary>
        /// 审批中
        /// </summary>
        Approval = 2,

        /// <summary>
        /// 审批驳回
        /// </summary>
        Reject = 3,

        /// <summary>
        /// 审批通过
        /// </summary>
        Pass = 4
    }

    /// <summary>
    /// 职位
    /// </summary>
    public enum RoleValue
    {
        超级管理员 = 1,
        管理员 = 2,
        总裁 = 3,
        总经理 = 5,  //运营经理
        总监 = 5,
        经理 = 6,
        员工 = 7,
        运营经理 = 8//运营经理权限同总经理
    }


    /// <summary>
    /// 商户角色
    /// </summary>
    public enum MerchantValue
    {
        润网超级管理 = 1,
        润网普通管理 = 2,
        外部普通商户 = 3
    }

    /// <summary>
    /// 分配状态
    /// </summary>
    public enum DistributionStatus
    {
        全部 = 1,
        已分配 = 2,
        未分配 = 3
    }

    /// <summary>
    /// 讲师启用状态
    /// </summary>
    public enum LectureActive
    { 
        全部 = 0,
        启用 = 1,
        禁用 = 2
    }

    /// <summary>
    /// 查询方式
    /// </summary>
    public enum SearchKbn
    {
        树形 = 1,
        表单 = 2
    }

    /// <summary>
    /// 分配检查区分
    /// </summary>
    public enum CheckKbn
    {
        包含员工信息 = 1,
        不包含员工信息 = 2
    }

    /// <summary>
    /// 业务值
    /// </summary>
    public enum BusinessValue
    { 
        全部 = 0,
        发发 = 1,
        大决策 = 2,
        沐融教育 = 3
    }
    /// <summary>
    /// 角色下拉框
    /// </summary>
    public enum RoleCombobox
    { 
        精确查询=0,
        权限查询=1
    }

}
