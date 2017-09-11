using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.SYS
{
    /// <summary>
    /// 表格列属性
    /// </summary>
    [Serializable]
    public class GridListColumns
    {
        /// <summary>
        /// 使用页面
        /// </summary>
        public string UsePage { get; set; }
        /// <summary>
        /// 使用列表
        /// </summary>
        public string UseGrid { get; set; }
        /// <summary>
        /// 表示顺序
        /// </summary>
        public int OrderValue { get; set; }
        /// <summary>
        /// 列英文名
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 列宽
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public string Hidden { get; set; }
        /// <summary>
        /// 列格式
        /// </summary>
        public string formatter { get; set; }
        /// <summary>
        /// 数据对齐
        /// </summary>
        public string align { get; set; }
        /// <summary>
        /// 头部对齐
        /// </summary>
        public string halign { get; set; }
        /// <summary>
        /// 占据行数
        /// </summary>
        public int rowspan { get; set; }
        /// <summary>
        /// 占据列数
        /// </summary>
        public int colspan { get; set; }
        /// <summary>
        /// 是否允许排序
        /// </summary>
        public string sortable { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string orderType { get; set; }
        /// <summary>
        /// 尺寸可变
        /// </summary>
        public string resizable { get; set; }
        /// <summary>
        /// 复选框
        /// </summary>
        public string checkbox { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUsing { get; set; }
        /// <summary>
        /// 使用权限
        /// </summary>
        public string useRole { get; set; }
    }

    /// <summary>
    /// 表格列属性
    /// </summary>
    [Serializable]
    public class ShowGridColumns
    {
        /// <summary>
        /// 列英文名
        /// </summary>
        public string field { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 列宽
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public string hidden { get; set; }
        /// <summary>
        /// 列格式
        /// </summary>
        public string formatter { get; set; }
        /// <summary>
        /// 数据对齐
        /// </summary>
        public string align { get; set; }
        /// <summary>
        /// 头部对齐
        /// </summary>
        public string halign { get; set; }
        /// <summary>
        /// 占据行数
        /// </summary>
        public int rowspan { get; set; }
        /// <summary>
        /// 占据列数
        /// </summary>
        public int colspan { get; set; }
        /// <summary>
        /// 是否允许排序
        /// </summary>
        public string sortable { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string orderType { get; set; }
        /// <summary>
        /// 尺寸可变
        /// </summary>
        public string resizable { get; set; }
        /// <summary>
        /// 复选框
        /// </summary>
        public string checkbox { get; set; }
    }

    [Serializable]
    public class LogError
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public int ErrorID { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public int BusinessID { get; set; }
        /// <summary>
        /// 用户ID（产品端）
        /// </summary>
        public int ErrorUserID { get; set; }
        /// <summary>
        /// 员工ID（后台）
        /// </summary>
        public int ErrorEmployeeID { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string ErrorUrl { get; set; }
        /// <summary>
        /// 动作
        /// </summary>
        public string ErrorAction { get; set; }
        /// <summary>
        /// 线程ID
        /// </summary>
        public string ErrorThreadID { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string ErrorClassName { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string ErrorMethodName { get; set; }
        /// <summary>
        /// 详细内容
        /// </summary>
        public string ErrorInfo { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime ErrorTime { get; set; }
        /// <summary>
        /// 日志级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 用户的IP
        /// </summary>
        public string IpAdress { get; set; }
        /// <summary>
        /// 用户代理
        /// </summary>
        public string UserAgent { get; set; }
    }
    [Serializable]
    public class LogErrorInfo : LogError
    {
      /// <summary>
      /// 业务名称
      /// </summary>
        public string BusinessName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName { get; set; }
    }
}
