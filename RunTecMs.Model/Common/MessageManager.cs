using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.Common
{
    /// <summary>
    /// 消息管理
    /// </summary>
    public class MessageManager
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public int MessageID { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// 追加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 发送者ID
        /// </summary>
        public int SendEmpID { get; set; }

        /// <summary>
        /// 接受者ID列表
        /// </summary>
        public string ReceivedEmpID { get; set; }

        /// <summary>
        /// 阅读者ID列表
        /// </summary>
        public string ReadEmpID { get; set; }
    }

    /// <summary>
    /// 消息内容扩展
    /// </summary>
    public class UIMessageManager : MessageManager
    {
        /// <summary>
        /// 发送者名称
        /// </summary>
        public string SendEmpTrueName { get; set; }
        /// <summary>
        /// 追加时间
        /// </summary>
        public string strAddTime { get; set; }
        /// <summary>
        /// 消息状态
        /// </summary>
        public string Status { get; set; }
    }
}
