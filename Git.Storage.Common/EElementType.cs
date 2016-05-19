/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2015/9/6 10:35:24
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2015/9/6 10:35:24       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
    public enum EElementType
    {
        [Description("文本框")]
        TextBox=1,

        [Description("文本域")]
        TextArea=2,

        [Description("下拉框")]
        Select=3,

        [Description("时间框")]
        DateTime=4,

        [Description("日期框")]
        Date=5
    }
}
