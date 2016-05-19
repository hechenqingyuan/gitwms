/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-30 9:56:32
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-30 9:56:32       情缘
*********************************************************************************/

using Git.Storage.Entity.OutStorage;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Git.Storage.Common;
using Git.Storage.Entity.Base;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Order;
using System.Data;

namespace Git.Storage.Provider.OutStorage
{
    public partial class OutStorageOrder : Bill<OutStorageEntity, OutStoDetailEntity>
    {
        public OutStorageOrder() { }

        /// <summary>
        /// 创建单据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string Create(OutStorageEntity entity, List<OutStoDetailEntity> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                list.ForEach(a =>
                {
                    a.OrderNum = a.OrderNum.IsEmpty() ? entity.OrderNum : a.OrderNum;
                    a.Amount = a.Amount == 0 ? a.OutPrice * a.Num : a.Amount;
                    a.IncludeAll();
                });
                entity.Num = list.Sum(a => a.Num);
                entity.Amount = list.Sum(a => a.Amount);
                entity.IncludeAll();
                int line = this.OutStorage.Add(entity);
                line += this.OutStoDetail.Add(list);
                ts.Complete();
                return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
            }
        }

        /// <summary>
        /// 取消单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Cancel(OutStorageEntity entity)
        {
            OutStorageEntity checkOrder = new OutStorageEntity();
            entity.Where(a => a.Status == (int)EAudite.Wait).And(a => a.OrderNum == entity.OrderNum);
            if (this.OutStorage.GetCount(checkOrder) > 0)
            {
                return EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Pass); //已经审核或者取消的订单不能审核
            }
            entity.Status = (int)EAudite.NotPass;
            entity.IncludeStatus(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.OutStorage.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Delete(OutStorageEntity entity)
        {
            OutStorageEntity item = new OutStorageEntity();
            item.IsDelete = (int)EIsDelete.Deleted;
            item.IncludeIsDelete(true);
            item.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.OutStorage.Update(item);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Audite(OutStorageEntity entity)
        {
            if (entity.Status == (int)EAudite.NotPass)
            {
                entity.IncludeStatus(true).IncludeReason(true).Where(a => a.OrderNum == entity.OrderNum);
                int line = this.OutStorage.Update(entity);
                return line > 0 ? "1000" : string.Empty;
            }
            else if (entity.Status == (int)EAudite.Pass)
            {
                Proc_AuditeOutStorageEntity auditeEntity = new Proc_AuditeOutStorageEntity();
                auditeEntity.OrderNum = entity.OrderNum;
                auditeEntity.Status = entity.Status;
                auditeEntity.AuditUser = entity.AuditUser;
                auditeEntity.Reason = entity.Reason;
                auditeEntity.OperateType = entity.OperateType;
                auditeEntity.EquipmentNum = entity.EquipmentNum;
                auditeEntity.EquipmentCode = entity.EquipmentCode;
                auditeEntity.Remark = entity.Remark;
                int line = this.Proc_AuditeOutStorage.ExecuteNonQuery(auditeEntity);

                /***
                 * 如果是销售订单则需要更改订单中的发货数量以及相应的状态
                 * 1. 查询出库单，判断出库单的类型
                 * 2. 查询出库单所有的内容详细，判断存在哪些订单
                 * 3. 统计相应的订单的出库总数量
                 * 4. 修改订单状态
                 * */
                OutStorageEntity outEntity = new OutStorageEntity();
                outEntity.IncludeAll();
                outEntity.Where(a => a.OrderNum == entity.OrderNum);
                outEntity = this.OutStorage.GetSingle(outEntity);
                OutStoDetailEntity detail = new OutStoDetailEntity();
                detail.Where(a => a.OrderNum == entity.OrderNum);
                detail.IncludeAll();
                List<OutStoDetailEntity> listDetail = this.OutStoDetail.GetList(detail);
                if (outEntity != null && !listDetail.IsNullOrEmpty())
                {
                    if (outEntity.OutType == (int)EOutType.Sell)
                    {
                        OutStorageProvider outProvider = new OutStorageProvider();
                        foreach (var item in listDetail.Where(a => !a.ContractOrder.IsEmpty() && !a.ContractSn.IsEmpty()).GroupBy(a => new { a.ContractOrder, a.ContractSn }))
                        {
                            OutStoDetailEntity tempOutDetail = new OutStoDetailEntity();
                            tempOutDetail.Where(a => a.ContractOrder == item.Key.ContractOrder).And(a => a.ContractSn == item.Key.ContractSn);
                            List<OutStoDetailEntity> list = outProvider.GetOrderDetail(tempOutDetail);
                            double value = list.Sum(a => a.Num);

                            OrderDetailEntity orderDetail = new OrderDetailEntity();
                            orderDetail.RealNum = value;
                            orderDetail.IncludeRealNum(true);
                            orderDetail.Where(a => a.SnNum == item.Key.ContractSn).And(a => a.OrderNum == item.Key.ContractOrder);
                            this.OrderDetail.Update(orderDetail);

                            orderDetail = new OrderDetailEntity();
                            orderDetail.IncludeAll();
                            orderDetail.And(a => a.OrderNum == item.Key.ContractOrder);
                            List<OrderDetailEntity> listOrderDetail = this.OrderDetail.GetList(orderDetail);
                            bool flag = true;
                            foreach (OrderDetailEntity detailItem in listOrderDetail)
                            {
                                if (detailItem.RealNum < detailItem.Num)
                                {
                                    flag = false;
                                }
                            }

                            OrdersEntity order = new OrdersEntity();
                            if (flag)
                            {
                                order.Status = (int)EOrderStatus.AllDelivery;
                            }
                            else
                            {
                                order.Status = (int)EOrderStatus.PartialDelivery;
                            }
                            order.IncludeStatus(true);
                            order.Where(a => a.OrderNum == item.Key.ContractOrder);
                            this.Orders.Update(order);
                        }
                    }
                }
                return auditeEntity.ReturnValue;

            }
            return string.Empty;
        }

        /// <summary>
        /// 打印单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Print(OutStorageEntity entity)
        {
            entity.IncludePrintUser(true).IncludePrintTime(true)
                .Where(a => a.OrderNum == entity.OrderNum);
            int line = this.OutStorage.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 查询单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override OutStorageEntity GetOrder(OutStorageEntity entity)
        {
            entity.IncludeAll();
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            entity = this.OutStorage.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 获得单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override List<OutStoDetailEntity> GetOrderDetail(OutStoDetailEntity entity)
        {
            OutStoDetailEntity detail = new OutStoDetailEntity();
            detail.IncludeAll();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            List<OutStoDetailEntity> list = this.OutStoDetail.GetList(detail);
            if (!list.IsNullOrEmpty())
            {
                List<LocationEntity> listLocation = new LocationProvider().GetList();
                listLocation = listLocation == null ? new List<LocationEntity>() : listLocation;
                foreach (OutStoDetailEntity item in list)
                {
                    LocationEntity location = listLocation.FirstOrDefault(a => a.LocalNum == item.LocalNum);
                    item.LocalName = location == null ? "" : location.LocalName;
                    if (item.Amount == 0)
                    {
                        item.Amount = item.OutPrice * item.Num;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 查询单据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override List<OutStorageEntity> GetList(OutStorageEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            int rowCount = 0;
            List<OutStorageEntity> listResult = this.OutStorage.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            if (!listResult.IsNullOrEmpty())
            {
                foreach (OutStorageEntity item in listResult)
                {
                    OutStoDetailEntity detail = new OutStoDetailEntity();
                    detail.IncludeAll();
                    detail.Where(a => a.OrderNum == item.OrderNum);
                    List<OutStoDetailEntity> listDetail = this.OutStoDetail.GetList(detail);
                    if (!listDetail.IsNullOrEmpty())
                    {
                        foreach (OutStoDetailEntity detailItem in listDetail)
                        {
                            item.ProductNames += string.Format("[{0}]", detailItem.ProductName);
                        }
                    }
                }

            }
            return listResult;
        }

        /// <summary>
        /// 查询单据详细数据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override List<OutStoDetailEntity> GetDetailList(OutStoDetailEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            OutStoDetailEntity detail = new OutStoDetailEntity();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            detail.IncludeAll();
            detail.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<OutStoDetailEntity> listResult = this.OutStoDetail.GetList(detail, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            if (!listResult.IsNullOrEmpty())
            {
                List<LocationEntity> listLocation = new LocationProvider().GetList();
                listLocation = listLocation == null ? new List<LocationEntity>() : listLocation;
                foreach (OutStoDetailEntity item in listResult)
                {
                    LocationEntity location = listLocation.FirstOrDefault(a => a.LocalNum == item.LocalNum);
                    item.LocalName = location == null ? "" : location.LocalName;
                    if (item.Amount == 0)
                    {
                        item.Amount = item.OutPrice * item.Num;
                    }
                }
            }
            return listResult;
        }

        /// <summary>
        /// 编辑单据信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditOrder(OutStorageEntity entity)
        {
            entity.Include(a => new { a.OutType, a.ProductType, a.ContractOrder, a.CusNum, a.CusName, a.Contact, a.Address, a.Phone, a.Remark, a.Amount, a.Num });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.OutStorage.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditDetail(OutStoDetailEntity entity)
        {
            entity.Where(a => a.SnNum == entity.SnNum);
            int line = this.OutStoDetail.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int GetCount(OutStorageEntity entity)
        {
            return this.OutStorage.GetCount(entity);
        }

        /// <summary>
        /// 编辑出库单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string EditOrder(OutStorageEntity entity, List<OutStoDetailEntity> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                entity.Include(a => new { a.OutType, a.ProductType, a.ContractOrder, a.CusNum, a.CusName, a.Contact, a.Address, a.Phone, a.Remark, a.Amount, a.Num });
                entity.Where(a => a.OrderNum == entity.OrderNum);
                OutStoDetailEntity detail = new OutStoDetailEntity();
                detail.Where(a => a.OrderNum == entity.OrderNum);
                this.OutStoDetail.Delete(detail);
                foreach (OutStoDetailEntity item in list)
                {
                    item.OrderNum = entity.OrderNum;
                    item.IncludeAll();
                }
                entity.Num = list.Sum(a => a.Num);
                entity.Amount = list.Sum(a => a.Amount);
                line = this.OutStorage.Update(entity);
                this.OutStoDetail.Add(list);
                ts.Complete();
                return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
            }
        }

        /// <summary>
        /// 获得打印单据的数据源
        /// </summary>
        /// <param name="argOrderNum"></param>
        /// <returns></returns>
        public override DataSet GetPrint(string argOrderNum)
        {
            DataSet ds = new DataSet();
            OutStorageEntity entity = new OutStorageEntity();
            entity.OrderNum = argOrderNum;
            entity = GetOrder(entity);
            if (entity != null)
            {
                List<OutStorageEntity> list = new List<OutStorageEntity>();
                list.Add(entity);
                DataTable tableOrder = list.ToDataTable();
                ds.Tables.Add(tableOrder);

                OutStoDetailEntity detail = new OutStoDetailEntity();
                detail.OrderNum = argOrderNum;
                List<OutStoDetailEntity> listDetail = GetOrderDetail(detail);
                if (!listDetail.IsNullOrEmpty())
                {
                    DataTable tableDetail = listDetail.ToDataTable();
                    ds.Tables.Add(tableDetail);
                }
            }
            else
            {
                List<OutStorageEntity> list = new List<OutStorageEntity>();
                DataTable tableOrder = list.ToDataTable();
                ds.Tables.Add(tableOrder);
                List<OutStoDetailEntity> listDetail = new List<OutStoDetailEntity>();
                DataTable tableDetail = listDetail.ToDataTable();
                ds.Tables.Add(tableDetail);
            }
            return ds;
        }
    }
}
