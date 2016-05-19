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

using Git.Storage.Entity.Check;
using Git.Framework.DataTypes;
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
using System.Data;
using Git.Storage.Provider.Base;

namespace Git.Storage.Provider.Check
{
    public partial class CheckOrder : Bill<CheckStockEntity, CheckStockInfoEntity>
    {
        public CheckOrder() { }

        /// <summary>
        /// 创建盘点单据
        /// 1. 将单据信息数据和单据详细信息数据保存到数据库
        /// 2. 统计盘点当期的账面数据
        /// 3. 备份当前的库存数据
        /// 4. 生成盘差准备表数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string Create(CheckStockEntity entity, List<CheckStockInfoEntity> list)
        {
            int line = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                entity.OrderNum = entity.OrderNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(CheckStockEntity)) : entity.OrderNum;
                entity.IncludeAll();
                if (!list.IsNullOrEmpty())
                {
                    list.ForEach(a =>
                    {
                        a.IncludeAll();
                        a.OrderNum = entity.OrderNum;
                    });

                    line = this.CheckStock.Add(entity);
                    line += this.CheckStockInfo.Add(list);
                }
                ts.Complete();
            }
            //调用存储过程备份数据 生成盘点差异表
            Proc_CreateCheckEntity checkEntity = new Proc_CreateCheckEntity();
            checkEntity.OrderNum = entity.OrderNum;
            checkEntity.CreateUser = entity.CreateUser;
            checkEntity.CreateName = "";
            this.Proc_CreateCheck.ExecuteNonQuery(checkEntity);
            return line > 0 && checkEntity.ReturnValue == "1000" ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 取消盘点单
        /// 1. 修改盘点单的状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Cancel(CheckStockEntity entity)
        {
            //只有待审核状态的单据才能取消，已经成功的订单不能取消
            CheckStockEntity checkOrder = new CheckStockEntity();
            entity.Where(a => a.Status == (int)EAudite.Wait).And(a => a.OrderNum == entity.OrderNum);
            if (this.CheckStock.GetCount(checkOrder) > 0)
            {
                return EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Pass); //已经审核或者取消的订单不能审核
            }
            entity.Status = (int)EAudite.NotPass;
            entity.IncludeStatus(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.CheckStock.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Delete(CheckStockEntity entity)
        {
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.CheckStock.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Audite(CheckStockEntity entity)
        {
            if (entity.Status == (int)EAudite.NotPass)
            {
                entity.IncludeStatus(true).IncludeReason(true).Where(a => a.OrderNum == entity.OrderNum);
                int line = this.CheckStock.Update(entity);
                return line > 0 ? "1000" : string.Empty;
            }
            else if (entity.Status == (int)EAudite.Pass)
            {
                Proc_AuditeCheckEntity audite = new Proc_AuditeCheckEntity();
                audite.OrderNum = entity.OrderNum;
                audite.Status = entity.Status;
                audite.AuditUser = entity.AuditUser;
                audite.Reason = entity.Reason;
                audite.OperateType = entity.OperateType;
                audite.EquipmentCode = entity.EquipmentCode;
                audite.EquipmentNum = entity.EquipmentNum;
                int line = this.Proc_AuditeCheck.ExecuteNonQuery(audite);
                return line > 0 ? "1000" : string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// 打印单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Print(CheckStockEntity entity)
        {
            entity.IncludePrintUser(true).IncludePrintTime(true)
                .Where(a => a.OrderNum == entity.OrderNum);
            int line = this.CheckStock.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 查询单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override CheckStockEntity GetOrder(CheckStockEntity entity)
        {
            entity.IncludeAll();
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            entity = this.CheckStock.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 获得单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override List<CheckStockInfoEntity> GetOrderDetail(CheckStockInfoEntity entity)
        {
            CheckStockInfoEntity detail = new CheckStockInfoEntity();
            detail.IncludeAll();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            List<CheckStockInfoEntity> list = this.CheckStockInfo.GetList(detail);
            if (!list.IsNullOrEmpty())
            {
                List<LocationEntity> listLocation = new LocationProvider().GetList();
                listLocation = listLocation == null ? new List<LocationEntity>() : listLocation;
                foreach (CheckStockInfoEntity item in list)
                {
                    LocationEntity location = listLocation.FirstOrDefault(a => a.LocalNum == item.TargetNum);
                    item.LocalName = location == null ? "" : location.LocalName;
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
        public override List<CheckStockEntity> GetList(CheckStockEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            int rowCount = 0;
            List<CheckStockEntity> listResult = this.CheckStock.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 查询单据详细数据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override List<CheckStockInfoEntity> GetDetailList(CheckStockInfoEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            CheckStockInfoEntity detail = new CheckStockInfoEntity();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            detail.IncludeAll();
            detail.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<CheckStockInfoEntity> listResult = this.CheckStockInfo.GetList(detail, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            if (!listResult.IsNullOrEmpty())
            {
                List<LocationEntity> listLocation = new LocationProvider().GetList();
                listLocation = listLocation == null ? new List<LocationEntity>() : listLocation;
                foreach (CheckStockInfoEntity item in listResult)
                {
                    LocationEntity location = listLocation.FirstOrDefault(a => a.LocalNum == item.TargetNum);
                    item.LocalName = location == null ? "" : location.LocalName;
                }
            }
            return listResult;
        }

        /// <summary>
        /// 编辑单据信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditOrder(CheckStockEntity entity)
        {
            entity.Include(a => new { a.Type, a.ProductType, a.ContractOrder, a.Remark });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.CheckStock.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditDetail(CheckStockInfoEntity entity)
        {
            entity.Where(a => a.ID == entity.ID);
            int line = this.CheckStockInfo.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑入库单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string EditOrder(CheckStockEntity entity, List<CheckStockInfoEntity> list)
        {
            int line = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                entity.Include(a => new { a.Type, a.ProductType, a.ContractOrder, a.Remark, a.CreateUser });
                entity.Where(a => a.OrderNum == entity.OrderNum);
                CheckStockInfoEntity detail = new CheckStockInfoEntity();
                detail.Where(a => a.OrderNum == entity.OrderNum);
                this.CheckStockInfo.Delete(detail);
                foreach (CheckStockInfoEntity item in list)
                {
                    item.OrderNum = entity.OrderNum;
                    item.IncludeAll();
                }
                line = this.CheckStock.Update(entity);
                this.CheckStockInfo.Add(list);

                ts.Complete();
            }

            CheckDataEntity checkData = new CheckDataEntity();
            checkData.Where(a => a.OrderNum == entity.OrderNum);
            this.CheckData.Delete(checkData);

            //调用存储过程备份数据 生成盘点差异表
            Proc_CreateCheckEntity checkEntity = new Proc_CreateCheckEntity();
            checkEntity.OrderNum = entity.OrderNum;
            checkEntity.CreateUser = entity.CreateUser;
            checkEntity.CreateName = "";
            line+=this.Proc_CreateCheck.ExecuteNonQuery(checkEntity);
            return line > 0 && checkEntity.ReturnValue == "1000" ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int GetCount(CheckStockEntity entity)
        {
            return this.CheckStock.GetCount(entity);
        }


        /// <summary>
        /// 获得打印单据的数据源
        /// </summary>
        /// <param name="argOrderNum"></param>
        /// <returns></returns>
        public override DataSet GetPrint(string argOrderNum)
        {
            DataSet ds = new DataSet();
            CheckStockEntity entity = new CheckStockEntity();
            entity.OrderNum = argOrderNum;
            entity = GetOrder(entity);
            if (entity != null)
            {
                List<CheckStockEntity> list = new List<CheckStockEntity>();
                list.Add(entity);
                DataTable tableOrder = list.ToDataTable();
                ds.Tables.Add(tableOrder);

                CheckStockInfoEntity detail = new CheckStockInfoEntity();
                detail.OrderNum = argOrderNum;
                List<CheckStockInfoEntity> listDetail = GetOrderDetail(detail);
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
