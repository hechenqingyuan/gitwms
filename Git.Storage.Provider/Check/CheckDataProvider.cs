/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014/4/16 9:09:18
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014/4/16 9:09:18       情缘
*********************************************************************************/

using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Entity.Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Storage.Provider.Check
{
    public partial class CheckDataProvider:DataFactory
    {
        public CheckDataProvider() { }

        /// <summary>
        /// 获得盘点盘差信息
        /// </summary>
        /// <param name="orderNum"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<CheckDataEntity> GetCheckDifList(string orderNum, ref PageInfo pageInfo)
        {
            CheckDataEntity entity = new CheckDataEntity();
            entity.IncludeAll();
            entity.Where(a => a.OrderNum == orderNum);
            entity.OrderBy(a => a.ID, EOrderBy.ASC);
            int rowCount = 0;
            List<CheckDataEntity> listResult = this.CheckData.GetList(entity,pageInfo.PageSize,pageInfo.PageIndex,out rowCount);
            pageInfo.PageCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 修改盘点差异表中盘点的数据
        /// </summary>
        /// <returns></returns>
        public int UpdateCheckInfoNum(CheckDataEntity entity)
        {
            entity.IncludeFirstQty(true);
            entity.Where(a => a.OrderNum == entity.OrderNum)
                .And(a => a.LocalNum == entity.LocalNum)
                .And(a => a.StorageNum == entity.StorageNum)
                .And(a => a.ProductNum == entity.ProductNum)
                .And(a => a.BarCode == entity.BarCode)
                .And(a=>a.BatchNum==entity.BatchNum)
                ;
            int line = this.CheckData.Update(entity);
            return line;
        }

        /// <summary>
        /// 完成盘点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int CompleteCheck(CheckStockEntity entity)
        {
            entity.IncludeIsComplete(true).Where(a => a.OrderNum == entity.OrderNum);
            int line = this.CheckStock.Update(entity);
            return line;
        }

        /// <summary>
        /// 获取盘点单据Excel数据
        /// </summary>
        /// <returns></returns>
        public List<CheckDataEntity> GetCheckOrder(string orderNum)
        {
            CheckDataEntity entity = new CheckDataEntity();
            entity.IncludeAll();
            entity.Where(a => a.OrderNum == orderNum);
            List<CheckDataEntity> listResult = this.CheckData.GetList(entity);
            return listResult;
        }

        /// <summary>
        /// 新增盘点数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddCheckData(CheckDataEntity entity)
        {
            entity.IncludeAll();
            int line = this.CheckData.Add(entity);
            return line;
        }

        /// <summary>
        /// 删除盘点数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteCheckData(CheckDataEntity entity)
        {
            int line = this.CheckData.Delete(entity);
            return line;
        }
    }
}
