/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014/4/21 21:06:55
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014/4/21 21:06:55       情缘
*********************************************************************************/

using Git.Framework.DataTypes;
using Git.Framework.Log;
using Git.Storage.Entity.Return;
using Git.Framework.ORM;
using Git.Framework.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Storage.Common;

namespace Git.Storage.Provider.Returns
{
    public partial class ReturnProvider : DataFactory
    {
        private Log log = Log.Instance(typeof(ReturnProvider));

        public ReturnProvider() { }

        /// <summary>
        /// 根据关联单号查询已经退货的的情况
        /// </summary>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public List<ReturnDetailEntity> GetDetailByOrder(string orderNum)
        {
            ReturnDetailEntity entity = new ReturnDetailEntity();
            entity.IncludeAll();
            ReturnOrderEntity order = new ReturnOrderEntity();
            entity.Left<ReturnOrderEntity>(order, new Params<string, string>() { Item1 = "OrderNum", Item2 = "OrderNum" });
            order.Where(a => a.Status == (int)EAudite.Pass);
            order.Where(a => a.ContractOrder == orderNum);
            List<ReturnDetailEntity> list = this.ReturnDetail.GetList(entity);
            return list;
        }

        /// <summary>
        /// 用于判断该订单是否已经正在出库
        /// </summary>
        /// <returns></returns>
        public ReturnOrderEntity CheckOrder(string orderNum)
        {
            ReturnDetailEntity entity = new ReturnDetailEntity() { ContractOrder = orderNum };
            ReturnOrderEntity order = new ReturnOrderEntity();
            order.Where(a => a.Status == (int)EAudite.Wait);
            order.IncludeAll();
            entity.Left<ReturnOrderEntity>(order, new Params<string, string>() { Item1 = "OrderNum", Item2 = "OrderNum" });
            order = this.ReturnOrder.GetSingle(order);
            return order;
        }

        /// <summary>
        /// 退货产品TOP10
        /// </summary>
        /// <param name="queryTime"></param>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public List<Proc_ReturnTOP10NumEntity> GetListTOP10(int queryTime, string storageNum)
        {
            Proc_ReturnTOP10NumEntity entity = new Proc_ReturnTOP10NumEntity();
            List<Proc_ReturnTOP10NumEntity> list = new List<Proc_ReturnTOP10NumEntity>();
            entity.StorageNum = storageNum;
            entity.ProductNum = string.Empty;
            entity.BeginTime = DateTime.Now.AddDays(-queryTime);
            entity.EndTime = DateTime.Now;
            entity.Status = (int)EAudite.Pass;
            entity.IsDelete = (int)EIsDelete.NotDelete;
            List<Proc_ReturnTOP10NumEntity> listResult = this.Proc_ReturnTOP10Num.ExceuteEntityList<Proc_ReturnTOP10NumEntity>(entity);
            if (!listResult.IsNullOrEmpty())
            {
                foreach (Proc_ReturnTOP10NumEntity item in listResult)
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
        /// <returns></returns>
        public List<ReturnOrderEntity> GetList(ReturnOrderEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.And("Status", ECondition.Eth, (int)EAudite.Pass);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            ReturnDetailEntity detail = new ReturnDetailEntity();
            detail.Include(a => new { ProductName = a.ProductName, BarCode = a.BarCode, ProductNum = a.ProductNum });
            entity.Left<ReturnDetailEntity>(detail, new Params<string, string>() { Item1 = "OrderNum", Item2 = "OrderNum" });
            int rowCount = 0;
            List<ReturnOrderEntity> listResult = this.ReturnOrder.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            int allNum = GetAllNum();
            if (!listResult.IsNullOrEmpty())
            {
                foreach (ReturnOrderEntity item in listResult)
                {
                    item.NumPCT = ((float)item.Num * 100.00f) / allNum;
                }
            }
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 获得所有退货数量
        /// </summary>
        /// <returns></returns>
        public int GetAllNum()
        {
            ReturnOrderEntity entity = new ReturnOrderEntity();
            entity.IncludeNum(true);
            entity.Where("Status", ECondition.Eth, (int)EAudite.Pass);
            entity.And("IsDelete", ECondition.Eth, (int)EIsDelete.NotDelete);
            int allNum = 0;
            try
            {
                allNum = this.ReturnOrder.Sum<int>(entity);
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
