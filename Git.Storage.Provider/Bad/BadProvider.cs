/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-30 12:45:13
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-30 12:45:13       情缘
*********************************************************************************/

using Git.Framework.Log;
using Git.Framework.ORM;
using Git.Storage.Entity.Bad;
using Git.Storage.Entity.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.DataTypes;
using Git.Storage.Common;

namespace Git.Storage.Provider.Bad
{
    public partial class BadProvider : DataFactory
    {
        private Log log = Log.Instance(typeof(BadProvider));
        public BadProvider() { }

        /// <summary>
        /// 获得库存信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public List<LocalProductEntity> GetList(string barCode)
        {
            LocalProductEntity entity = new LocalProductEntity();
            entity.IncludeAll();
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            entity.Where(a => a.BarCode == barCode).Or(a => a.ProductNum == barCode);

            LocationEntity location = new LocationEntity();
            entity.Left<LocationEntity>(location, new Params<string, string>() { Item1 = "LocalNum", Item2 = "LocalNum" });
            location.Where(a => a.LocalType == (int)ELocalType.Normal);
            List<LocalProductEntity> listResult = this.LocalProduct.GetList(entity);
            return listResult;
        }

        /// <summary>
        /// 报损产品TOP10
        /// </summary>
        /// <param name="queryTime"></param>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public List<Proc_BadTOP10NumEntity> GetListTOP10(int queryTime, string storageNum)
        {
            Proc_BadTOP10NumEntity entity = new Proc_BadTOP10NumEntity();
            List<Proc_BadTOP10NumEntity> list = new List<Proc_BadTOP10NumEntity>();
            entity.ProductNum = string.Empty;
            entity.StorageNum = storageNum;
            entity.BeginTime = DateTime.Now.AddDays(-queryTime);
            entity.EndTime = DateTime.Now;
            entity.Status = (int)EAudite.Pass;
            entity.IsDelete = (int)EIsDelete.NotDelete;
            List<Proc_BadTOP10NumEntity> listResult = this.Proc_BadTOP10Num.ExceuteEntityList<Proc_BadTOP10NumEntity>(entity);
            if (!listResult.IsNullOrEmpty())
            {
                foreach (Proc_BadTOP10NumEntity item in listResult)
                {
                    if (item.TotalNum > 0)
                    {
                        list.Add(item);
                    }
                }
            }
            if (entity.TotalNum > 0)
            {
                entity.ProductName = "其他";
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// 查询单据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public List<BadReportEntity> GetList(BadReportEntity entity, ref PageInfo pageInfo, string storageNum)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.And("Status", ECondition.Eth, (int)EAudite.Pass);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            BadReportDetailEntity badDetail = new BadReportDetailEntity();
            badDetail.Include(a => new { ProductName = a.ProductName, BarCode = a.BarCode, ProductNum = a.ProductNum });
            badDetail.Where(a => a.StorageNum == storageNum);
            entity.Left<BadReportDetailEntity>(badDetail, new Params<string, string>() { Item1 = "OrderNum", Item2 = "OrderNum" });
            int rowCount = 0;
            List<BadReportEntity> listResult = this.BadReport.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            int allNum = GetAllNum();
            if (!listResult.IsNullOrEmpty())
            {
                foreach (BadReportEntity item in listResult)
                {
                    item.NumPCT = (item.Num * 100.00f) / allNum;
                }
            }
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 获得所有报损数量
        /// </summary>
        /// <returns></returns>
        public int GetAllNum()
        {
            BadReportEntity entity = new BadReportEntity();
            entity.IncludeNum(true);
            entity.Where("Status", ECondition.Eth, (int)EAudite.Pass);
            entity.And("IsDelete", ECondition.Eth, (int)EIsDelete.NotDelete);
            int allNum = 0;
            try
            {
                allNum = this.BadReport.Sum<int>(entity);
            }
            catch (Exception e)
            {
                allNum = 0;
                log.Info(e.Message);
            }
            return allNum;
        }
    }
}
