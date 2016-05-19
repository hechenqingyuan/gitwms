/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-29 23:42:29
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-29 23:42:29       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
    public enum EMoveType
    {
        [Description("移库上架")]
        ToRack=1,

        [Description("仓库移库")]
        RackToRack=2,

        [Description("报损移库")]
        MoveToBad = 3
    }
}
