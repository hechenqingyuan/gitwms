using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
    public enum EEquipmentStatus
    {
        /// <summary>
        /// 闲置
        /// </summary>
        [Description("闲置")]
        Unused = 1,

        /// <summary>
        /// 正在使用
        /// </summary>
        [Description("正在使用")]
        IsUsing = 2,

        /// <summary>
        /// 报修
        /// </summary>
        [Description("报修")]
        Repair = 3,

        /// <summary>
        /// 报损
        /// </summary>
        [Description("报损")]
        Breakage = 4,

        /// <summary>
        /// 遗失
        /// </summary>
        [Description("遗失")]
        Lost = 5,
    }
}
