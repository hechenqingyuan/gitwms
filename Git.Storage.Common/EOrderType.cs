/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-08-11 17:18:58
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-08-11 17:18:58       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
    public enum EOrderType
    {
        /// <summary>
        /// 实际订单
        /// </summary>
        [Description("实际订单")]
        Really = 1,

        /// <summary>
        /// 虚拟订单
        /// </summary>
        [Description("虚拟订单")]
        Invented = 2
    }
}
