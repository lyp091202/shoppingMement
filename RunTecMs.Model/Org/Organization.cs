using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.ORG
{
    /// <summary>
    /// 公司实体
    /// </summary>
    [Serializable]
    public class Company
    {
        //公司ID
        public int CompanyID { get; set; }
        //公司代码
        public string CompanyCode { get; set; }
        //公司名
        public string Name { get; set; }
        //公司值
        public string Value { get; set; }
        //排序
        public int OrderValue { get; set; }
        //公司描述
        public string CompanyDescription { get; set; }
        //公司级别
        public int CompanyLevel { get; set; }
        //公司摘要
        public string Remark { get; set; }
        //公司图片名
        public string CompanyPicName { get; set; }
        //公司主页
        public string CompanyURL { get; set; }
        //超管标识
        public bool IsSYSDBA { get; set; }
        //注册时间
        public DateTime? RegistrationTime { get; set; }
        //注册地点
        public string RegistrationAddr { get; set; }
        //注册资金
        public int RegisteredCapital { get; set; }
        //追加时间
        public DateTime? AddTime { get; set; }
        //更新时间
        public DateTime? UpdateTime { get; set; }
        //删除时间
        public DateTime? DelTime { get; set; }
    }

    /// <summary>
    /// 部门
    /// </summary>
    [Serializable]
    public class Department
    {
        //部门ID
        public int DepID { get; set; }
        //亲部门ID
        public int ParentDepID { get; set; }
        //部门名
        public string Name { get; set; }
        //部门值
        public string Value { get; set; }
        //排序
        public int OrderValue { get; set; }
        //部门代码
        public string DepCode { get; set; }
        //部门描述
        public string DepDescription { get; set; }
        //部门级别
        public byte DepLevel { get; set; }
        //部门摘要
        public string Remark { get; set; }
        //部门图片名
        public string DepPicName { get; set; }
        //所属公司ID
        public int CompanyID { get; set; }
        //超管标识
        public bool IsSYSDBA { get; set; }
        //成立时间
        public DateTime? AddTime { get; set; }
        //超管标识
        public bool IsDel { get; set; }
        //更新时间
        public DateTime? UpdateTime { get; set; }
        //删除时间
        public DateTime? DelTime { get; set; }
    }


    /// <summary>
    /// 部门详细情报
    /// </summary>
    [Serializable]
    public class DepartmentDetail : Department
    {
        //亲部门名
        public string ParentDepName { get; set; }
        //所属公司名
        public string CompanyName { get; set; }
    }

    /// <summary>
    /// 公司和直属部门ID
    /// </summary>
    [Serializable]
    public class CompanyDepInfo
    {
        //公司ID
        public int CompanyID { get; set; }
        //公司代码
        public string CompanyCode { get; set; }
        //公司名
        public string CompanyName { get; set; }
        //部门ID
        public int DepID { get; set; }
        //部门ID
        public string DepName { get; set; }
    }

    /// <summary>
    /// 部门信息
    /// </summary>
    [Serializable]
    public class DepInfo
    {
        //部门ID
        public int DepID { get; set; }
        //亲部门ID
        public int ParentDepID { get; set; }
        //部门名
        public string Name { get; set; }
        //公司ID
        public int CompanyID { get; set; }
    }

    /// <summary>
    /// 部门树形信息
    /// </summary>
    [Serializable]
    public class DepTreeInfo
    {
        //ID
        public string ID { get; set; }
        //亲ID
        public string PreID { get; set; }
        //公司ID
        public int CompanyID { get; set; }
        //公司代码
        public string CompanyCode { get; set; }
        //公司名
        public string CompanyName { get; set; }
        //部门ID
        public int DepID { get; set; }
        //部门名
        public string DepName { get; set; }
    }

    /// <summary>
    /// 员工树形信息
    /// </summary>
    [Serializable]
    public class EmployeeTreeInfo : DepTreeInfo
    {
        //员工ID
        public int EmployeeID { get; set; }
        //员工登录名
        public string EmployeeLoginName { get; set; }
        //员工真实姓名
        public string EmployeeTrueName { get; set; }
        //在指定部门中最高角色ID
        public int RoleID { get; set; }
    }

}
