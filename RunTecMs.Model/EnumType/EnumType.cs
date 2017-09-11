using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.EnumType
{

    /// <summary>
    /// 管理后台区分
    /// </summary>
    public enum ManagerType
    {
        管理后台 = 1,
        第三方商户 = 2
    }

    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ReturnType
    {
        用户名不存在 = 001,
        密码错误 = 002,
        密码格式不正确 = 003,
        密码修改成功 = 004,
        密码修改失败 = 005,
        登录成功 = 006,
        登录超时 = 007,
        验证码错误 = 008,
        请重新获取验证码 = 009,
        此账号已禁用 = 010,
        数据验证有误 = 011,
        状态更改成功 = 012,
        状态更改失败 = 013
    }

    public enum EnumPaymentStatus
    {
        未结算 = 1,
        财务已提交 = 2,
        商户确认中 = 3,
        结算完成 = 4,
        驳回 = 5,
        直接到账 = 16
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
    /// 角色下拉框
    /// </summary>
    public enum RoleCombobox
    {
        精确查询 = 0,
        权限查询 = 1
    }

    /// <summary>
    /// 角色
    /// </summary>
    public enum RoleValue
    {
        超级管理员 = 1,
        管理员 = 2,
        总裁 = 3,
        总经理 = 4,  //运营经理
        经理 = 5,
        销售顾问 = 6,
        //导购员 = 7,//运营经理权限同总经理
        门店经理 = 8,
        仓库管理员 = 9,
        门店运营 = 10,
        财务 = 11,
        行政 = 12
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
    /// 系统配置
    /// </summary>
    public enum SAConfig
    {
        HostName = 1,
        注册赠送 = 2,
        小秘书ID = 3,
        权限集获取方式 = 4
    }

    /// <summary>
    /// 权限集获取方式
    /// </summary>
    public enum PermissionGetType
    {
        按角色 = 1,
        按部门 = 2
    }

    /// <summary>
    /// 导入用户字段定义
    /// </summary>
    public enum InputUser
    {
        手机 = 0,
        昵称,
        真实姓名,
        性别,
        身份证,
        QQ号,
        微信号,
        邮箱,
        地址,
        股龄,
        近期收益,
        投资风格,
        资金量,
        看盘经历,
        投资问题,
        盈利模式,
        态度,
        职业,
        客户类型,
        客户级别,
        客户质量,
        软件意向,
        来源渠道,
        登录名,
        密码,
        开始日期,
        状态,
        软件有效期限
    }
    
    /// <summary>
    /// 客户质量
    /// </summary>
    public enum CustomerQuality
    {
        劣质 = -1,
        普通 = 1,
        优质 = 2
    }
    

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 未支付
        /// </summary>
        未支付 = 1,
        /// <summary>
        /// 未确认
        /// </summary>
        未确认 = 2,
        /// <summary>
        /// 待财务确认
        /// </summary>
        待财务确认 = 3,
        /// <summary>
        /// 财务确认
        /// </summary>
        财务确认 = 4,
        /// <summary>
        /// 财务驳回
        /// </summary>
        财务驳回 = 5,
        /// <summary>
        /// 待合规确认
        /// </summary>
        待合规确认 = 6,
        /// <summary>
        /// 合规确认
        /// </summary>
        合规确认 = 7,
        /// <summary>
        /// 合规驳回
        /// </summary>
        合规驳回 = 8,
        /// <summary>
        /// 服务分配
        /// </summary>
        服务分配 = 9,
        /// <summary>
        /// 产品开通
        /// </summary>
        产品开通 = 10,
        /// <summary>
        /// 订单完成
        /// </summary>
        订单完成 = 11
    }
}
