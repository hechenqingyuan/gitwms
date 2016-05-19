/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-07-22 14:48:53
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-07-22 14:48:53       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
    public enum EChange
    {
        [Description("入库")]
        In = 1,

        [Description("出库")]
        Out = 2,

        [Description("移库(移除)")]
        MoveOut = 3,

        [Description("移库(移入)")]
        MoveIn = -3,

        [Description("报损(移除)")]
        BadOut = 4,

        [Description("报损(移入)")]
        BadIn = -4,

        [Description("盘盈")]
        InventoryIncome = 5,

        [Description("盘亏")]
        InventoryLoss = 6,

        [Description("退货")]
        Back = 7,
    }
}
