/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-30 9:13:48
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-30 9:13:48       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
    public enum EOutType
    {
        /// <summary>
        /// 从外部采购物品入库
        /// </summary>
        [Description("采购退货出库")]
        BuyToBack=1,

        /// <summary>
        /// 销售出产品从仓库出货
        /// </summary>
        [Description("销售提货出库")]
        Sell=2,

        /// <summary>
        /// 需要某种材料或者物品出库
        /// </summary>
        [Description("领用出库")]
        Use =3,

        /// <summary>
        /// 从仓库借出某物品出库
        /// </summary>
        [Description("借货出库")]
        Borrow = 4,

        /// <summary>
        /// 之前借入某个物品还出出库
        /// </summary>
        [Description("借入还出")]
        ToBack = 5,
    }
}
