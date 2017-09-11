using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.ORG
{
    /// <summary>
    /// 职位实体
    /// </summary>
    [Serializable]
    public class Job
    {
        //职位ID
        public int JobID { get; set; }
        //名称
        public string Name { get; set; }
        //值
        public string Value { get; set; }
        //排序
        public int OrderValue { get; set; }
        //亲职位ID
        public int PerJobID { get; set; }
        //超管标识
        public bool IsSYSDBA { get; set; }

    }
    public class JobInfo : Job
    {
        public string department { get; set; }
    }
    /// <summary>
    /// 数据范围
    /// </summary>
    [Serializable]
    public class DataRang
    {
        //ID，不为空
        public int DataRangeID { get; set; }
        //名称，不为空
        public string Name { get; set; }
        //值，不为空
        public string Value { get; set; }
        //排序，可为空
        public int OrderValue { get; set; }
        //数据范围说明，不为空
        public string DataRange { get; set; }
        //超管标识
        public bool IsSYSDBA { get; set; }
    }

    /// <summary>
    /// 业务
    /// </summary>
    [Serializable]
    public class Business
    {
        //业务ID
        public int BusinessID { get; set; }
        //业务名称
        public string Name { get; set; }
        //业务值
        public string Value { get; set; }
        //业务值（新的）
        public int BusinessValue { get; set; }
        // 业务名（后台所用）
        public string BusinessName { get; set; }
        //排序
        public int OrderValue { get; set; }
        //超管标识
        public bool IsSYSDBA { get; set; }
    }

    /// <summary>
    /// 角色
    /// </summary>
    [Serializable]
    public class Role
    {
        //角色ID
        public int RoleID { get; set; }
        //数据范围ID
        public int DataRangeID { get; set; }
        //角色名称
        public string Name { get; set; }
        //角色值
        public string Value { get; set; }
        //排序
        public int OrderValue { get; set; }
        //说明
        public string Descrption { get; set; }
        //超管标识
        public bool IsSYSDBA { get; set; }
    }

    public class RoleInfo : Role
    {
        public string DataRangeName { get; set; } 
    }

}
