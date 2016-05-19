/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-29 23:39:49
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-29 23:39:49       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
    public enum EAudite
    {
        [Description("等待审核")]
        Wait=1,

        [Description("审核成功")]
        Pass=2,

        [Description("审核失败")]
        NotPass=3
    }
}
