/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-26 13:43:56
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-26 13:43:56       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
    public enum ELocalType
    {
        /// <summary>
        /// 正式库区
        /// </summary>
        [Description("正式库区")]
        Normal=1,

        /// <summary>
        /// 待入库区
        /// </summary>
        [Description("待入库区")]
        WaitIn=2,

        /// <summary>
        /// 待检库区
        /// </summary>
        [Description("待检库区")]
        WaitCheck = 3,

        /// <summary>
        /// 待出库区
        /// </summary>
        [Description("待出库区")]
        WaitOut = 4,

        /// <summary>
        /// 报损库区
        /// </summary>
        [Description("报损库区")]
        Bad = 5,
    }
}
