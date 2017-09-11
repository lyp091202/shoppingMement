using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunTecMs.RunIDAL.BusinessData;
using RunTecMs.RunDALFactory;

namespace RunTecMs.RunBLL.BusinessData
{
    public class MessageManager
    {
        private readonly IMessageManager dal = DataAccess.CreateMessageManager();

        /// <summary>
        /// 获取发送者发送消息消息数
        /// </summary>
        /// <param name="SendEmpID">发送者ID</param>
        /// <returns></returns>
        public int GetSendMessageCount(int SendEmpID)
        {
            // 查询用SQL
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT COUNT(1) FROM SYS_MessageManager");
            sbSql.AppendFormat(" WHERE SendEmpID = {0}", SendEmpID);

            return dal.ExeSelectCountSql(sbSql.ToString());
        }

        /// <summary>
        /// 获取接受者未读消息列表
        /// </summary>
        /// <returns></returns>
        public int GetNoReadMessageCount(int ReceivedEmpID)
        {
            // 查询用SQL
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT COUNT(1) FROM SYS_MessageManager");
            sbSql.AppendFormat(" WHERE CHARINDEX(',{0},',ReceivedEmpID) > 0", ReceivedEmpID);
            sbSql.AppendFormat(" AND CHARINDEX(',{0},',ReadEmpID) = 0 ", ReceivedEmpID);

            return dal.ExeSelectCountSql(sbSql.ToString());
        }

        /// <summary>
        /// 插入消息
        /// </summary>
        /// <param name="MessageMode"></param>
        /// <returns></returns>
        public bool InsertReadMessage(Model.Common.MessageManager MessageMode)
        {
            StringBuilder sbSql = new StringBuilder();
            if (MessageMode != null)
            {
                // 发送者ID列表的头部加上“,”
                MessageMode.ReceivedEmpID = "," + MessageMode.ReceivedEmpID + ",";
                MessageMode.ReceivedEmpID = MessageMode.ReceivedEmpID.Replace(",,", ",");

                // 阅读者
                MessageMode.ReadEmpID = "," + MessageMode.ReadEmpID + ",";
                MessageMode.ReadEmpID = MessageMode.ReadEmpID.Replace(",,", ",");

                // 插入用SQL
                sbSql.Append("INSERT INTO SYS_MessageManager ");
                sbSql.Append("(MessageContent,AddTime,SendEmpID,ReceivedEmpID,ReadEmpID)");
                sbSql.AppendFormat("VALUES('{0}',GETDATE(),{1},'{2}','{3}')",
                    MessageMode.MessageContent, MessageMode.SendEmpID,
                    MessageMode.ReceivedEmpID, MessageMode.ReadEmpID);
            }
            else
            {
                return false;
            }
            return dal.ExeUpdateSql(sbSql.ToString());
        }

        /// <summary>
        /// 更新消息内容的阅读者
        /// </summary>
        /// <param name="strMessageId">消息ID列表</param>
        /// <param name="ReadEmpID">阅读者ID</param>
        /// <returns></returns>
        public bool UpdReadMessageEmpID(string strMessageId, int ReadEmpID)
        {
            StringBuilder sbSql = new StringBuilder();

            // 更新语句
            if (!string.IsNullOrEmpty(strMessageId))
            {
                sbSql.AppendFormat("UPDATE SYS_MessageManager SET ReadEmpID = ReadEmpID + '{0}' ", ReadEmpID.ToString() + ",");
                sbSql.AppendFormat(" WHERE MessageID IN ({0})", strMessageId);
            }
            else
            {
                return false;
            }

            return dal.ExeUpdateSql(sbSql.ToString());
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<Model.Common.UIMessageManager> GetMessageByFilter(int pageIndex, int pageSize, Model.Common.MessageManager MessageMode)
        {
            // 查询语句
            StringBuilder sbSql = new StringBuilder();
            
            // 分页查询
            sbSql.Append("SELECT * FROM (");
            // 查询实体
            sbSql.Append("SELECT ROW_NUMBER() OVER(ORDER BY MessageID ASC) AS ROWS,");
            sbSql.Append(" Msg.MessageID,Msg.MessageContent,Msg.AddTime,Oep.TrueName AS SendEmpTrueName,Msg.SendEmpID,");
            sbSql.AppendFormat(" CASE WHEN CHARINDEX(',{0},',ReadEmpID) = 0 THEN '未读' ELSE '已读' END AS Status, ", MessageMode.ReceivedEmpID);
            sbSql.Append(" CONVERT(varchar(25), Msg.AddTime, 120 ) as strAddTime, Msg.ReceivedEmpID,Msg.ReadEmpID");
            sbSql.Append(" FROM SYS_MessageManager Msg");
            sbSql.Append(" LEFT JOIN Org_Employee Oep ON EmployeeID = SendEmpID ");
            sbSql.Append(" WHERE 1=1 ");

            // 查询条件 消息ID
            if (MessageMode.MessageID > 0)
            {
                sbSql.AppendFormat(" AND MessageID = {0}", MessageMode.MessageID);
            }

            // 消息内容
            if (!string.IsNullOrEmpty(MessageMode.MessageContent))
            {
                sbSql.AppendFormat(" AND MessageContent like %{0}%", MessageMode.MessageContent);
            }

            // 发送者ID
            if (MessageMode.SendEmpID > 0)
            {
                sbSql.AppendFormat(" AND SendEmpID = {0}", MessageMode.SendEmpID);
            }

            // 接受者ID/未读
            if (!string.IsNullOrEmpty(MessageMode.ReceivedEmpID))
            {
                // 接受
                sbSql.AppendFormat(" AND CHARINDEX(',{0},',ReceivedEmpID) > 0", MessageMode.ReceivedEmpID);
                // 未读
                sbSql.AppendFormat(" AND CHARINDEX(',{0},',ReadEmpID) = 0", MessageMode.ReceivedEmpID);
            }

            // 阅读者ID
            if (!string.IsNullOrEmpty(MessageMode.ReadEmpID))
            {
                sbSql.AppendFormat(" AND CHARINDEX(',{0},',ReadEmpID) > 0 ", MessageMode.ReadEmpID);
            }

            // 分页查询
            sbSql.Append(" ) T");
            if (pageIndex > 0 && pageSize > 0)
            {
                sbSql.AppendFormat(" WHERE T.ROWS BETWEEN {0} AND {1}", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
            }

            return dal.ExeSelectContentSql(sbSql.ToString());
        }

        /// <summary>
        /// 获取接受者
        /// </summary>
        /// <param name="searchKbn">查询区分（1：财务人员 2：合规人员）</param>
        /// <param name="companyId">公司ID</param>
        /// <param name="depId">部门ID</param>
        /// <returns>查询接受者ID</returns>
        public string GetReceivedEmpID(int searchKbn, int companyId)
        {
            string strEmpId = "";
            // 查询语句
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendLine("SELECT CONVERT(VARCHAR(20), EmployeeID) AS ReceivedEmpID ");
            sbSql.AppendLine("FROM Org_EmployeeDepartmentRole ");
            sbSql.AppendLine(" WHERE 1 = 1");
            if (companyId > 0)
            {
                sbSql.AppendFormat(" AND CompanyID = {0}", companyId);
            }
            
            if (searchKbn == 1)
            {
                sbSql.AppendFormat("   AND RoleID IN ({0})", Convert.ToInt32(Model.EnumType.RoleValue.财务));
            }

            IList<Model.Common.UIMessageManager> userList = dal.ExeSelectContentSql(sbSql.ToString());

            for (int i = 0; i < userList.Count; i++)
            {
                strEmpId = strEmpId + userList[i].ReceivedEmpID;
                if (i < userList.Count - 1)
                {
                    strEmpId = strEmpId + ",";
                }
            }

            return strEmpId;
        }

    }
}