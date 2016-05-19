/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-08-06 20:16:12
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-08-06 20:16:12       情缘
*********************************************************************************/

using Git.Storage.Entity.Move;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.DataTypes;
using Git.Storage.Common;
using Git.Framework.ORM;
using Git.Storage.Entity.Base;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Order;
using Git.Storage.Entity.Store;
using Git.Framework.Resource;
using Git.Framework.Json;
using System.Net.Http;
using System.Data;
using Git.Storage.Provider.Base;

namespace Git.Storage.Provider.Order
{
    public partial class OrderBill : Bill<OrdersEntity, OrderDetailEntity>
    {
        /// <summary>
        /// 创建单据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string Create(OrdersEntity entity, List<OrderDetailEntity> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                entity.OrderNum = entity.OrderNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(OrdersEntity)) : entity.OrderNum;
                entity.IncludeAll();

                if (!list.IsNullOrEmpty())
                {
                    list.ForEach(a =>
                    {
                        a.OrderNum = entity.OrderNum;
                        a.SendTime = entity.SendDate;
                        a.IncludeAll();
                    });
                    line = this.Orders.Add(entity);
                    line += this.OrderDetail.Add(list);
                }
                ts.Complete();
                return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
            }
        }

        /// <summary>
        /// 取消单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Cancel(OrdersEntity entity)
        {
            //只有待审核状态的单据才能取消，已经成功的订单不能取消
            OrdersEntity checkOrder = new OrdersEntity();
            entity.Where(a => a.AuditeStatus == (int)EAudite.Wait).And(a => a.OrderNum == entity.OrderNum);
            if (this.Orders.GetCount(checkOrder) > 0)
            {
                return EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Pass); //已经审核或者取消的订单不能审核
            }
            entity.AuditeStatus = (int)EAudite.NotPass;
            entity.IncludeAuditeStatus(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.Orders.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Delete(OrdersEntity entity)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                entity.IsDelete = (int)EIsDelete.Deleted;
                entity.IncludeIsDelete(true);
                entity.Where(a => a.OrderNum == entity.OrderNum);
                int line = this.Orders.Update(entity);

                OrderDetailEntity detail = new OrderDetailEntity();
                detail.Where(a => a.OrderNum == entity.OrderNum);

                line += this.OrderDetail.Delete(detail);
                ts.Complete();
                return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
            }
        }

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Audite(OrdersEntity entity)
        {
            string orderNum = entity.OrderNum;
            if (entity.AuditeStatus == (int)EAudite.NotPass)
            {
                entity.IncludeAuditeStatus(true).IncludeReason(true).Where(a => a.OrderNum == entity.OrderNum);
                int line = this.Orders.Update(entity);
                return line > 0 ? "1000" : string.Empty;
            }
            else if (entity.AuditeStatus == (int)EAudite.Pass)
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    entity.IncludeAuditeStatus(true).IncludeReason(true).Where(a => a.OrderNum == orderNum);
                    int line = this.Orders.Update(entity);
                    ts.Complete();
                    return line > 0 ? "1000" : string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 打印单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Print(OrdersEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override OrdersEntity GetOrder(OrdersEntity entity)
        {
            entity.IncludeAll();
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            entity.Where(a => a.OrderNum == entity.OrderNum).And(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity = this.Orders.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 获得单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override List<OrderDetailEntity> GetOrderDetail(OrderDetailEntity entity)
        {
            OrderDetailEntity detail = new OrderDetailEntity();
            detail.IncludeAll();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            ProductEntity product = new ProductEntity();
            product.Include(a => new { Size = a.Size, UnitName = a.UnitName });
            detail.Left<ProductEntity>(product, new Params<string, string>() { Item1 = "ProductNum", Item2 = "SnNum" });
            List<OrderDetailEntity> list = this.OrderDetail.GetList(detail);

            return list;
        }

        /// <summary>
        /// 查询单据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override List<OrdersEntity> GetList(OrdersEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            OrderDetailEntity detail = new OrderDetailEntity();
            detail.Include(a => new { BarCode = a.BarCode, ProductNum = a.ProductNum, ProductName = a.ProductName });
            entity.Left<OrderDetailEntity>(detail, new Params<string, string>() { Item1 = "SnNum", Item2 = "OrderSnNum" });
            int rowCount = 0;
            List<OrdersEntity> listResult = this.Orders.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 查询单据详细数据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override List<OrderDetailEntity> GetDetailList(OrderDetailEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            OrderDetailEntity detail = new OrderDetailEntity();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            detail.IncludeAll();
            detail.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<OrderDetailEntity> listResult = this.OrderDetail.GetList(detail, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 编辑单据信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditOrder(OrdersEntity entity)
        {
            entity.Include(a => new { a.OrderType, a.ContractOrder, a.Remark, a.Amount, a.Num });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.Orders.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditDetail(OrderDetailEntity entity)
        {
            entity.Where(a => a.SnNum == entity.SnNum);
            int line = this.OrderDetail.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑订单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string EditOrder(OrdersEntity entity, List<OrderDetailEntity> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                entity.Include(a => new { a.OrderType, a.ContractOrder, a.Remark, a.Amount, a.Num });
                entity.Where(a => a.OrderNum == entity.OrderNum);
                OrderDetailEntity detail = new OrderDetailEntity();
                detail.Where(a => a.OrderNum == entity.OrderNum);
                this.MoveOrderDetail.Delete(detail);
                foreach (OrderDetailEntity item in list)
                {
                    item.OrderNum = entity.OrderNum;
                    item.IncludeAll();
                }
                entity.Num = list.Sum(a => a.Num);
                entity.Amount = list.Sum(a => a.Amount);
                line = this.Orders.Update(entity);
                this.OrderDetail.Add(list);
                ts.Complete();
                return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
            }
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int GetCount(OrdersEntity entity)
        {
            return this.Orders.GetCount(entity);
        }


        /// <summary>
        /// 获得打印单据的数据源
        /// </summary>
        /// <param name="argOrderNum"></param>
        /// <returns></returns>
        public override DataSet GetPrint(string argOrderNum)
        {
            DataSet ds = new DataSet();
            OrdersEntity entity = new OrdersEntity();
            entity.OrderNum = argOrderNum;
            entity = GetOrder(entity);
            if (entity != null)
            {
                List<OrdersEntity> list = new List<OrdersEntity>();
                list.Add(entity);
                DataTable tableOrder = list.ToDataTable();
                ds.Tables.Add(tableOrder);

                OrderDetailEntity detail = new OrderDetailEntity();
                detail.OrderNum = argOrderNum;
                List<OrderDetailEntity> listDetail = GetOrderDetail(detail);
                if (!listDetail.IsNullOrEmpty())
                {
                    DataTable tableDetail = listDetail.ToDataTable();
                    ds.Tables.Add(tableDetail);
                }
            }

            return ds;
        }
    }
}
