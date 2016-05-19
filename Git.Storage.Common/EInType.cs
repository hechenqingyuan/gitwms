/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-30 9:03:48
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-30 9:03:48       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
    public enum EInType
    {
        /// <summary>
        /// 购买相应的产品并且入到仓库
        /// </summary>
        [Description("采购收货入库")]
        Purchase=1,

        /// <summary>
        /// 将产品出售给客户然后因为某种原因退回仓库
        /// </summary>
        [Description("销售退货入库")]
        SellToBack=2,

        /// <summary>
        /// 加工生产产品入到仓库
        /// </summary>
        [Description("生产产品入库")]
        Produce = 3,

        /// <summary>
        /// 内部借用某个物品使用完之后还回仓库入库
        /// </summary>
        [Description("领用退还入库")]
        BorrowToBack=4,

        /// <summary>
        /// 从外部借入某个物品入库
        /// </summary>
        [Description("借货入库")]
        BorrowIn=5,

        /// <summary>
        /// 将物品借给其他人然后还回仓库
        /// </summary>
        [Description("借出还入")]
        BorrowOut = 6,
    }
}
