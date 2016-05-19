/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-30 12:35:03
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-30 12:35:03       情缘
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
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using System.Data;
using Git.Storage.Provider.Base;


namespace Git.Storage.Provider.Move
{
    public partial class MoveOrder : Bill<MoveOrderEntity, MoveOrderDetailEntity>
    {
        public MoveOrder() { }


        /// <summary>
        /// 创建单据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string Create(MoveOrderEntity entity, List<MoveOrderDetailEntity> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                entity.OrderNum = entity.OrderNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(MoveOrderEntity)) : entity.OrderNum;
                entity.IncludeAll();

                if (!list.IsNullOrEmpty())
                {
                    list.ForEach(a =>
                    {
                        a.IncludeAll();
                        a.OrderNum = entity.OrderNum;
                    });
                    line = this.MoveOrder.Add(entity);
                    line += this.MoveOrderDetail.Add(list);
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
        public override string Cancel(MoveOrderEntity entity)
        {
            //只有待审核状态的单据才能取消，已经成功的订单不能取消
            MoveOrderEntity checkOrder = new MoveOrderEntity();
            entity.Where(a => a.Status == (int)EAudite.Wait).And(a => a.OrderNum == entity.OrderNum);
            if (this.MoveOrder.GetCount(checkOrder) > 0)
            {
                return EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Pass); //已经审核或者取消的订单不能审核
            }
            entity.Status = (int)EAudite.NotPass;
            entity.IncludeStatus(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.MoveOrder.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Delete(MoveOrderEntity entity)
        {
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.MoveOrder.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Audite(MoveOrderEntity entity)
        {
            if (entity.Status == (int)EAudite.NotPass)
            {
                entity.IncludeStatus(true).IncludeReason(true).Where(a => a.OrderNum == entity.OrderNum);
                int line = this.MoveOrder.Update(entity);
                return line > 0 ? "1000" : string.Empty;
            }
            else if (entity.Status == (int)EAudite.Pass)
            {
                Proc_AuditeMoveEntity auditeEntity = new Proc_AuditeMoveEntity();
                auditeEntity.OrderNum = entity.OrderNum;
                auditeEntity.Status = entity.Status;
                auditeEntity.AuditUser = entity.AuditUser;
                auditeEntity.Reason = entity.Reason;
                auditeEntity.OperateType = entity.OperateType;
                auditeEntity.EquipmentNum = entity.EquipmentNum;
                auditeEntity.EquipmentCode = entity.EquipmentCode;
                auditeEntity.Remark = entity.Remark;
                int line = this.Proc_AuditeMove.ExecuteNonQuery(auditeEntity);
                return auditeEntity.ReturnValue;
            }
            return string.Empty;
        }

        /// <summary>
        /// 打印单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Print(MoveOrderEntity entity)
        {
            entity.IncludePrintUser(true).IncludePrintTime(true)
                .Where(a => a.OrderNum == entity.OrderNum);
            int line = this.MoveOrder.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 查询单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override MoveOrderEntity GetOrder(MoveOrderEntity entity)
        {
            entity.IncludeAll();
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            entity = this.MoveOrder.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 获得单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override List<MoveOrderDetailEntity> GetOrderDetail(MoveOrderDetailEntity entity)
        {
            MoveOrderDetailEntity detail = new MoveOrderDetailEntity();
            detail.IncludeAll();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            List<MoveOrderDetailEntity> list = this.MoveOrderDetail.GetList(detail);
            if (!list.IsNullOrEmpty())
            {
                List<LocationEntity> listLocation = new LocationProvider().GetList();
                listLocation = listLocation == null ? new List<LocationEntity>() : listLocation;
                foreach (MoveOrderDetailEntity item in list)
                {
                    LocationEntity location = listLocation.FirstOrDefault(a => a.LocalNum == item.FromLocalNum);
                    item.FromLocalName = location == null ? "" : location.LocalName;
                    location = listLocation.FirstOrDefault(a => a.LocalNum == item.ToLocalNum);
                    item.ToLocalName = location == null ? "" : location.LocalName;
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
        public override List<MoveOrderEntity> GetList(MoveOrderEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            int rowCount = 0;
            List<MoveOrderEntity> listResult = this.MoveOrder.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 查询单据详细数据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override List<MoveOrderDetailEntity> GetDetailList(MoveOrderDetailEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            MoveOrderDetailEntity detail = new MoveOrderDetailEntity();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            detail.IncludeAll();
            detail.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<MoveOrderDetailEntity> listResult = this.MoveOrderDetail.GetList(detail, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            if (!listResult.IsNullOrEmpty())
            {
                List<LocationEntity> listLocation = new LocationProvider().GetList();
                listLocation = listLocation == null ? new List<LocationEntity>() : listLocation;
                foreach (MoveOrderDetailEntity item in listResult)
                {
                    LocationEntity location = listLocation.FirstOrDefault(a => a.LocalNum == item.FromLocalNum);
                    item.FromLocalName = location == null ? "" : location.LocalName;
                    location = listLocation.FirstOrDefault(a => a.LocalNum == item.ToLocalNum);
                    item.ToLocalName = location == null ? "" : location.LocalName;
                }
            }
            return listResult;
        }

        /// <summary>
        /// 编辑单据信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditOrder(MoveOrderEntity entity)
        {
            entity.Include(a => new { a.MoveType, a.ProductType, a.ContractOrder, a.Remark, a.Amout, a.Num });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.MoveOrder.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditDetail(MoveOrderDetailEntity entity)
        {
            entity.Where(a => a.SnNum == entity.SnNum);
            int line = this.MoveOrderDetail.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int GetCount(MoveOrderEntity entity)
        {
            return this.MoveOrder.GetCount(entity);
        }

        /// <summary>
        /// 编辑移库单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string EditOrder(MoveOrderEntity entity, List<MoveOrderDetailEntity> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                entity.Include(a => new { a.MoveType, a.ProductType, a.ContractOrder, a.Remark, a.Amout, a.Num });
                entity.Where(a => a.OrderNum == entity.OrderNum);
                MoveOrderDetailEntity detail = new MoveOrderDetailEntity();
                detail.Where(a => a.OrderNum == entity.OrderNum);
                this.MoveOrderDetail.Delete(detail);
                foreach (MoveOrderDetailEntity item in list)
                {
                    item.OrderNum = entity.OrderNum;
                    item.IncludeAll();
                }
                entity.Num = list.Sum(a => a.Num);
                entity.Amout = list.Sum(a => a.Amout);
                line = this.MoveOrder.Update(entity);
                this.MoveOrderDetail.Add(list);
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
            MoveOrderEntity entity = new MoveOrderEntity();
            entity.OrderNum = argOrderNum;
            entity = GetOrder(entity);
            if (entity != null)
            {
                List<MoveOrderEntity> list = new List<MoveOrderEntity>();
                list.Add(entity);
                DataTable tableOrder = list.ToDataTable();
                ds.Tables.Add(tableOrder);

                MoveOrderDetailEntity detail = new MoveOrderDetailEntity();
                detail.OrderNum = argOrderNum;
                List<MoveOrderDetailEntity> listDetail = GetOrderDetail(detail);
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
