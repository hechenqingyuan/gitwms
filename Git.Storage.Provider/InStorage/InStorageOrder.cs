/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-30 9:36:46
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-30 9:36:46       情缘
*********************************************************************************/

using Git.Framework.DataTypes;
using Git.Storage.Entity.InStorage;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Git.Storage.Common;
using Git.Storage.Entity.Base;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Framework.Cache;
using System.Data;
using Git.Storage.Provider.Base;

namespace Git.Storage.Provider.InStorage
{
    public partial class InStorageOrder : Bill<InStorageEntity, InStorDetailEntity>
    {
        public InStorageOrder() { }

        /// <summary>
        /// 创建单据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string Create(InStorageEntity entity, List<InStorDetailEntity> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                entity.OrderNum = entity.OrderNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(InStorageEntity)) : entity.OrderNum;
                entity.IncludeAll();

                if (!list.IsNullOrEmpty())
                {
                    list.ForEach(a =>
                    {
                        a.IncludeAll();
                        a.OrderNum = entity.OrderNum;
                    });
                    entity.Num = list.Sum(q => q.Num);
                    entity.Amount = list.Sum(a => a.Amount);
                    line = this.InStorage.Add(entity);
                    line += this.InStorDetail.Add(list);
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
        public override string Cancel(InStorageEntity entity)
        {
            //只有待审核状态的单据才能取消，已经成功的订单不能取消
            InStorageEntity checkOrder = new InStorageEntity();
            entity.Where(a => a.Status == (int)EAudite.Wait).And(a => a.OrderNum == entity.OrderNum);
            if (this.InStorage.GetCount(checkOrder) > 0)
            {
                return EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Pass); //已经审核或者取消的订单不能审核
            }
            entity.Status = (int)EAudite.NotPass;
            entity.IncludeStatus(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.InStorage.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Delete(InStorageEntity entity)
        {
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.InStorage.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Audite(InStorageEntity entity)
        {
            if (entity.Status == (int)EAudite.NotPass)
            {
                entity.IncludeStatus(true).IncludeReason(true).Where(a => a.OrderNum == entity.OrderNum);
                int line = this.InStorage.Update(entity);
                return line > 0 ? "1000" : string.Empty;
            }
            else if (entity.Status == (int)EAudite.Pass)
            {
                Proc_AuditeInStorageEntity auditeEntity = new Proc_AuditeInStorageEntity();
                auditeEntity.OrderNum = entity.OrderNum;
                auditeEntity.Status = entity.Status;
                auditeEntity.AuditUser = entity.AuditUser;
                auditeEntity.Reason = entity.Reason;
                auditeEntity.OperateType = entity.OperateType;
                auditeEntity.EquipmentNum = entity.EquipmentNum;
                auditeEntity.EquipmentCode = entity.EquipmentCode;
                auditeEntity.Remark = entity.Remark;
                int line = this.Proc_AuditeInStorage.ExecuteNonQuery(auditeEntity);
                return auditeEntity.ReturnValue;
            }
            return string.Empty;
        }

        /// <summary>
        /// 打印单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Print(InStorageEntity entity)
        {
            entity.IncludePrintUser(true).IncludePrintTime(true)
                .Where(a => a.OrderNum == entity.OrderNum);
            int line = this.InStorage.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 查询单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override InStorageEntity GetOrder(InStorageEntity entity)
        {
            entity.IncludeAll();
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            entity = this.InStorage.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 获得单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override List<InStorDetailEntity> GetOrderDetail(InStorDetailEntity entity)
        {
            InStorDetailEntity detail = new InStorDetailEntity();
            detail.IncludeAll();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            List<InStorDetailEntity> list = this.InStorDetail.GetList(detail);
            if (!list.IsNullOrEmpty())
            {
                List<LocationEntity> listLocation = new LocationProvider().GetList();
                listLocation = listLocation == null ? new List<LocationEntity>() : listLocation;
                List<ProductEntity> listProduct = new ProductProvider().GetListByCache();
                listProduct = listProduct == null ? new List<ProductEntity>() : listProduct;
                foreach (InStorDetailEntity item in list)
                {
                    LocationEntity location = listLocation.FirstOrDefault(a => a.LocalNum == item.LocalNum);
                    item.LocalName = location == null ? "" : location.LocalName;
                    if (item.Amount == 0)
                    {
                        item.Amount = item.InPrice * item.Num;
                    }
                    ProductEntity product = listProduct.FirstOrDefault(a => a.SnNum == item.ProductNum);
                    item.Size = product.IsNull() ? string.Empty : product.Size;
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
        public override List<InStorageEntity> GetList(InStorageEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            int rowCount = 0;
            List<InStorageEntity> listResult = this.InStorage.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 查询单据详细数据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override List<InStorDetailEntity> GetDetailList(InStorDetailEntity entity, ref PageInfo pageInfo)
        {
            InStorDetailEntity detail = new InStorDetailEntity();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            detail.IncludeAll();
            detail.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<InStorDetailEntity> list = this.InStorDetail.Top(detail, 12);

            List<InStorDetailEntity> listResult = this.InStorDetail.GetList(detail, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            if (!listResult.IsNullOrEmpty())
            {
                List<LocationEntity> listLocation = new LocationProvider().GetList();
                listLocation = listLocation == null ? new List<LocationEntity>() : listLocation;
                foreach (InStorDetailEntity item in listResult)
                {
                    LocationEntity location = listLocation.FirstOrDefault(a => a.LocalNum == item.LocalNum);
                    item.LocalName = location == null ? "" : location.LocalName;
                    if (item.Amount == 0)
                    {
                        item.Amount = item.InPrice * item.Num;
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
        public override string EditOrder(InStorageEntity entity)
        {
            entity.Include(a => new { a.InType, a.ProductType, a.ContractOrder, a.SupNum, a.SupName, a.ContactName, a.Phone, a.Remark, a.Amount, a.Num });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.InStorage.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditDetail(InStorDetailEntity entity)
        {
            entity.Where(a => a.SnNum == entity.SnNum);
            int line = this.InStorDetail.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑入库单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string EditOrder(InStorageEntity entity, List<InStorDetailEntity> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                entity.Include(a => new { a.InType, a.ProductType, a.ContractOrder, a.SupNum, a.SupName, a.ContactName, a.Phone, a.Remark, a.Num, a.Amount });
                entity.Where(a => a.OrderNum == entity.OrderNum);
                InStorDetailEntity detail = new InStorDetailEntity();
                detail.Where(a => a.OrderNum == entity.OrderNum);
                this.InStorDetail.Delete(detail);
                foreach (InStorDetailEntity item in list)
                {
                    item.OrderNum = entity.OrderNum;
                    item.IncludeAll();
                }
                entity.Num = list.Sum(a => a.Num);
                entity.Amount = list.Sum(a => a.Amount);
                line = this.InStorage.Update(entity);
                this.InStorDetail.Add(list);
                ts.Complete();
                return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
            }
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int GetCount(InStorageEntity entity)
        {
            return this.InStorage.GetCount(entity);
        }

        /// <summary>
        /// 获得打印单据的数据源
        /// </summary>
        /// <param name="argOrderNum"></param>
        /// <returns></returns>
        public override DataSet GetPrint(string argOrderNum)
        {
            DataSet ds = new DataSet();
            InStorageEntity entity = new InStorageEntity();
            entity.OrderNum = argOrderNum;
            entity = GetOrder(entity);
            if (entity != null)
            {
                List<InStorageEntity> list = new List<InStorageEntity>();
                list.Add(entity);
                DataTable tableOrder = list.ToDataTable();
                ds.Tables.Add(tableOrder);

                InStorDetailEntity detail = new InStorDetailEntity();
                detail.OrderNum = argOrderNum;
                List<InStorDetailEntity> listDetail = GetOrderDetail(detail);
                listDetail = listDetail.IsNull() ? new List<InStorDetailEntity>() : listDetail;
                DataTable tableDetail = listDetail.ToDataTable();
                ds.Tables.Add(tableDetail);
            }
            else
            {
                List<InStorageEntity> list = new List<InStorageEntity>();
                List<InStorDetailEntity> listDetail = new List<InStorDetailEntity>();
                DataTable tableOrder = list.ToDataTable();
                ds.Tables.Add(tableOrder);

                DataTable tableDetail = listDetail.ToDataTable();
                ds.Tables.Add(tableDetail);
            }
            return ds;
        }
    }
}
