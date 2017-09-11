using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.Common
{
    /// <summary>
    /// 描述：分页信息
    /// </summary>
    public  class PageInfo<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int PageTotal { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 排序的字段
        /// </summary>
        public string StrOrder { get; set; }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IList<T> ilist { get;set; }
    }

    /// <summary>
    /// 取得分页情报的参数
    /// </summary>
    public struct GetPageInfo
    {
        // 需要查看的表名
        public string tabName;
        // 主键
        public string pkColumn;
        // 表示字段
        public string showColumn;
        // 排序字段
        public string ascColumn;
        // 条件
        public string where;
        // 排序 (0为升序,1为降序)
        public int intOrderType;
        // 当前页页码 (即Top currPage)
        public int pageIndex;
        // 分页大小
        public int pageSize;
    }
}
