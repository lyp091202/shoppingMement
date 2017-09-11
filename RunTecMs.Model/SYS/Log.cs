using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.SYS
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [Serializable]
    public class OperateLog
    {
        // 操作日志ID
        public int OperateID { get; set; }
        // 操作用户ID
        public int OperateUserID { get; set; }
        // 操作用户名
        public string OperateUserName { get; set; }
        // 操作用户部门ID
        public int OperateUserDepartID { get; set; }
        // 操作用户部门名
        public string OperateUserDepartName { get; set; }
        // 操作地址
        public string OperateUrl { get; set; }
        // 操作动作
        public string OperateAction { get; set; }
        // 操作时间
        public string OperateTime { get; set; }
    }

    /// <summary>
    /// 登录日志
    /// </summary>
    [Serializable]
    public class LoginLog
    {
        // 登录ID
        public int LoginID { get; set; }
        // 登录时间
        public string LoginTime { get; set; }
        // 登录IP地址
        public string LoginIP { get; set; }
        // 登录平台
        public string LoginPlatform { get; set; }
        // 登录内容
        public string LoginContext { get; set; }
        // 登录用户ID
        public int LoginUserID { get; set; }
        // 登录用户名
        public string LoginUserName { get; set; }
        // 登录用户手机
        public string LoginUserPhone { get; set; }
        // 登录角色ID
        public int LoginUserRoleID { get; set; }
        // 登录角色名
        public string LoginUserRoleName { get; set; }
        // 用户部门ID
        public int LoginUserDepartID { get; set; }
        // 用户部门名
        public string LoginUserDepartName { get; set; }
    }

    /// <summary>
    /// 错误日志
    /// </summary>
    [Serializable]
    public class ErrorLog
    {
        // 日志ID
        public int ErrorID { get; set; }
        // 用户ID
        public int ErrorUserID { get; set; }
        // 用户名
        public string ErrorUserName { get; set; }
        // 用户部门ID
        public int ErrorUserDepartID { get; set; }
        // 用户部门名
        public string ErrorUserDepartName { get; set; }
        // 地址
        public string ErrorUrl { get; set; }
        // 动作
        public string ErrorAction { get; set; }
        // 内容
        public string ErrorInfo { get; set; }
        // 时间
        public string ErrorTime { get; set; }
    }
}
