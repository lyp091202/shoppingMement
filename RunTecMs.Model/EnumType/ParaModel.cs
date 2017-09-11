using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.EnumType
{
    /// <summary>
    /// 添加归属
    /// </summary>
    public class AddShip
    {
        /// <summary>
        /// 对哪个客户添加归属
        /// </summary>
        public int customerId { get; set; }
        /// <summary>
        /// 该归属属于哪个业务
        /// </summary>
        public int businessId { get; set; }
        /// <summary>
        /// 归属ID
        /// </summary>
        public int owenShipId { get; set; }
        /// <summary>
        /// 所在资源池
        /// </summary>
        public string resourcePool { get; set; }

    }

    /// <summary>
    /// 编辑归属
    /// </summary>
    public class EditShip
    {
        public int customerId { get; set; }

        public int newcomId { get; set; }

        public int newdepId { get; set; }

        public int newempId { get; set; }

        public int newbusId { get; set; }

        public int ownershipId { get; set; }

    }
}
