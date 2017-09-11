using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace RunTecMs.RunIDAL.BusinessData
{
    public interface IMessageManager
    {
        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<Model.Common.UIMessageManager> ExeSelectContentSql(string sql);

        /// <summary>
        /// 获取个数
        /// </summary>
        /// <returns></returns>
        int ExeSelectCountSql(string sql);

        /// <summary>
        /// 更新插入
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool ExeUpdateSql(string sql);
    }
}
