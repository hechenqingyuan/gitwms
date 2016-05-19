using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
   
    public enum EOrderStatus
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        [Description("创建订单")]
        CreateOrder = 1,

        /// <summary>
        /// 订单确认
        /// </summary>
        [Description("订单确认")]
        OrderConfirm = 2,

        /// <summary>
        /// 排产中
        /// </summary>
        [Description("排产中")]
        InTheStock = 3,

        /// <summary>
        /// 部分出货
        /// </summary>
        [Description("部分出货")]
        PartialDelivery = 4,

        /// <summary>
        /// 全部出货
        /// </summary>
        [Description("全部出货")]
        AllDelivery = 5,

        /// <summary>
        /// 出货失败
        /// </summary>
        [Description("出货失败")]
        DeliveryFailure = 6
    }
}
