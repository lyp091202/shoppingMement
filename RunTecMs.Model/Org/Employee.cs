using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.ORG
{
    /// <summary>
    /// 员工
    /// </summary>
    [Serializable]
    public class EmployeeModel
    {
        //员工ID
        public int EmployeeID { get; set; }

        //职位ID，不为空
        public int JobID { get; set; }

        //登录名，不为空
        public string LoginName { get; set; }

        //密码，不为空
        public string Password { get; set; }

        //昵称，可为空
        public string NickName { get; set; }

        //头像，可为空
        public string AvatarPicName { get; set; }

        //真实名字，可为空
        public string TrueName { get; set; }

        //性别，0：女，1男，可为空
        public int Sex { get; set; }

        //出生年月，可为空
        public DateTime? Birthday { get; set; }

        //地址，可为空
        public string Address { get; set; }

        //手机号，不为空
        public string Mobile { get; set; }

        //身份证号，可为空
        public string IDCard { get; set; }

        //开始时间，可为空
        public DateTime? StartDate { get; set; }

        //结束时间，可为空
        public DateTime? EndDate { get; set; }

        //邮箱，可为空
        public string Email { get; set; }

        //腾讯QQ，可为空
        public string QQ { get; set; }

        //微信，可为空
        public string WeChat { get; set; }

        //最后登录时间，可为空
        public DateTime? LastLoginTime { get; set; }

        //最后登录IP，可为空
        public string LastLoginIP { get; set; }

        //是否启用，不为空
        public bool Active { get; set; }

        //员工代码，不为空
        public string EmployeeCode { get; set; }

        //密码盐
        public string PwdSalt { get; set; }

        //邀请码
        public string InvitationCode { get; set; }
        
        //婚姻状况
        public bool MaritalStatus { get; set; }

        //学历
        public string Education { get; set; }

        // 分配状态
        public string DistributionStatus { get; set; }

        //添加时间，可为空
        public DateTime? AddTime { get; set; }

        //更新时间，可为空
        public DateTime? UpdTime { get; set; }

        //删除时间，可为空
        public DateTime? DelTime { get; set; }

    }

    /// <summary>
    /// 员工所属部门角色
    /// </summary>
    [Serializable]
    public class EmployeeDepartmentRole
    {
        //ID
        public int EDID { get; set; }
        //员工ID
        public int EmployeeID { get; set; }
        //公司ID
        public int CompanyID { get; set; }
        //部门ID
        public int DepID { get; set; }
        //角色ID
        public int RoleID { get; set; }
        //业务ID
        public int BusinessValue { get; set; }
        //追加时间
        public DateTime? AddTime { get; set; }
        //更新时间
        public DateTime? UpdateTime { get; set; }
        //登录名，不为空
        public string LoginName { get; set; }
        //真实名字，可为空
        public string TrueName { get; set; }
    }

    /// <summary>
    /// 员工详细信息
    /// </summary>
    [Serializable]
    public class EmployeeDetailInfo : EmployeeModel
    {
        // 发发相关信息
        //公司ID
        public int CompanyID { get; set; }
        //部门ID
        public int DepID { get; set; }
        //角色ID
        public int RoleID { get; set; }
        // 追加组织关系信息用
        //公司ID
        public int AddCompanyID { get; set; }
        //部门ID
        public int AddDepID { get; set; }
        //角色ID
        public int AddRoleID { get; set; }
        // 操作者公司和部门
        //公司ID
        public int OperateCompanyID { get; set; }
        //部门ID
        public int OperateDepID { get; set; }

        //业务值
        public int BusinessValue { get; set; }
        //业务值
        public int AddBusinessValue { get; set; }
        //业务范围
        public string SBusinessValues { get; set; }
        //公司名
        public string CompanyName { get; set; }
        //部门名
        public string DepName { get; set; }
        //角色名
        public string RoleName { get; set; }
        //职位名
        public string JobName { get; set; }
        //业务名
        public string BusinessName { get; set; }
        //员工所属部门角色主键
        public int EDID { get; set; }

    }

    /// <summary>
    /// 登录员工信息
    /// </summary>
    public class LoginEmployee : EmployeeModel
    {
        /// <summary>
        /// 当前员工最大角色Id
        /// </summary>
        public int MaxRoleID { get; set; }
        /// <summary>
        /// 当前员工最大角色所在公司Id
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 当前员工最大角色所在部门Id
        /// </summary>
        public int DepID { get; set; }
        /// <summary>
        /// 当前员工最大角色所属业务Id
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepName { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 是否是管理权限
        /// </summary>
        public int IsManager { get; set; }
    }
}
