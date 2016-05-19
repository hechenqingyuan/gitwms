/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-30 12:40:49
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-30 12:40:49       情缘
*********************************************************************************/

using Git.Storage.Entity.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Framework.ORM;

namespace Git.Storage.Provider.Move
{
    public partial class MoveProvider:DataFactory
    {
        public MoveProvider() { }

        /// <summary>
        /// 移库管理获得库存信息
        /// 
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public List<LocalProductEntity> GetList(string barCode)
        {
            LocalProductEntity entity = new LocalProductEntity();
            entity.IncludeAll();
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            entity.Where(a => a.BarCode == barCode).Or(a => a.ProductNum == barCode);
            List<LocalProductEntity> listResult = this.LocalProduct.GetList(entity);
            return listResult;
        }
    }
}
