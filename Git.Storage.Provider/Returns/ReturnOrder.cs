/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014/4/19 22:27:30
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014/4/19 22:27:30       情缘
*********************************************************************************/

using Git.Framework.Log;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Entity.Return;
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

namespace Git.Storage.Provider.Returns
{
    public partial class ReturnOrder : Bill<ReturnOrderEntity, ReturnDetailEntity>
    {
        private Log log = Log.Instance(typeof(ReturnOrder));

        public ReturnOrder() { }

        /// <summary>
        /// 创建退货单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string Create(ReturnOrderEntity entity, List<ReturnDetailEntity> list)
        {
            int line = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                entity.OrderNum = entity.OrderNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(ReturnOrderEntity)) : entity.OrderNum;
                entity.IncludeAll();
                if (!list.IsNullOrEmpty())
                {
                    list.ForEach(a =>
                    {
                        a.IncludeAll();
                        a.OrderNum = entity.OrderNum;
                    });

                    line = this.ReturnOrder.Add(entity);
                    line += this.ReturnDetail.Add(list);
                }
                ts.Complete();
            }
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 取消退货单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Cancel(ReturnOrderEntity entity)
        {
            ReturnOrderEntity checkOrder = new ReturnOrderEntity();
            entity.Where(a => a.Status == (int)EAudite.Wait).And(a => a.OrderNum == entity.OrderNum);
            if (this.ReturnOrder.GetCount(checkOrder) > 0)
            {
                return EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Pass); //已经审核或者取消的订单不能审核
            }
            entity.Status = (int)EAudite.NotPass;
            entity.IncludeStatus(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.ReturnOrder.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Delete(ReturnOrderEntity entity)
        {
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.ReturnOrder.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Audite(ReturnOrderEntity entity)
        {
            if (entity.Status == (int)EAudite.NotPass)
            {
                entity.IncludeStatus(true).IncludeReason(true).Where(a => a.OrderNum == entity.OrderNum);
                int line = this.ReturnOrder.Update(entity);
                return line > 0 ? "1000" : string.Empty;
            }
            else if (entity.Status == (int)EAudite.Pass)
            {
                Proc_AuditeReturnEntity auditeEntity = new Proc_AuditeReturnEntity();
                auditeEntity.OrderNum = entity.OrderNum;
                auditeEntity.Status = entity.Status;
                auditeEntity.AuditUser = entity.AuditUser;
                auditeEntity.Reason = entity.Reason;
                auditeEntity.OperateType = entity.OperateType;
                auditeEntity.EquipmentNum = entity.EquipmentNum;
                auditeEntity.EquipmentCode = entity.EquipmentCode;
                auditeEntity.Remark = entity.Remark;
                int line = this.Proc_AuditeReturn.ExecuteNonQuery(auditeEntity);
                return auditeEntity.ReturnValue;
            }
            return string.Empty;
        }

        /// <summary>
        /// 打印单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Print(ReturnOrderEntity entity)
        {
            entity.IncludePrintUser(true).IncludePrintTime(true)
                .Where(a => a.OrderNum == entity.OrderNum);
            int line = this.ReturnOrder.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 查询单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override ReturnOrderEntity GetOrder(ReturnOrderEntity entity)
        {
            entity.IncludeAll();
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            entity = this.ReturnOrder.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 获得单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override List<ReturnDetailEntity> GetOrderDetail(ReturnDetailEntity entity)
        {
            ReturnDetailEntity detail = new ReturnDetailEntity();
            detail.IncludeAll();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            List<ReturnDetailEntity> list = this.ReturnDetail.GetList(detail);
            return list;
        }

        /// <summary>
        /// 查询单据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override List<ReturnOrderEntity> GetList(ReturnOrderEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            int rowCount = 0;
            List<ReturnOrderEntity> listResult = this.ReturnOrder.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 查询单据详细数据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override List<ReturnDetailEntity> GetDetailList(ReturnDetailEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            ReturnDetailEntity detail = new ReturnDetailEntity();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            detail.IncludeAll();
            detail.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<ReturnDetailEntity> listResult = this.ReturnDetail.GetList(detail, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            
            return listResult;
        }

        /// <summary>
        /// 编辑单据信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditOrder(ReturnOrderEntity entity)
        {
            entity.Include(a => new { a.ReturnType, a.ProductType, a.ContractOrder, a.Remark });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.ReturnOrder.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditDetail(ReturnDetailEntity entity)
        {
            entity.Where(a => a.ID == entity.ID);
            int line = this.ReturnDetail.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑退货单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string EditOrder(ReturnOrderEntity entity, List<ReturnDetailEntity> list)
        {
            int line = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                entity.Include(a => new { a.ReturnType, a.ProductType, a.ContractOrder, a.Remark, a.CreateUser });
                entity.Where(a => a.OrderNum == entity.OrderNum);
                ReturnDetailEntity detail = new ReturnDetailEntity();
                detail.Where(a => a.OrderNum == entity.OrderNum);
                this.ReturnDetail.Delete(detail);
                foreach (ReturnDetailEntity item in list)
                {
                    item.OrderNum = entity.OrderNum;
                    item.IncludeAll();
                }
                line = this.ReturnOrder.Update(entity);
                this.ReturnDetail.Add(list);
                ts.Complete();
            }

            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 获得打印单据的数据源
        /// </summary>
        /// <param name="argOrderNum"></param>
        /// <returns></returns>
        public override DataSet GetPrint(string argOrderNum)
        {
            DataSet ds = new DataSet();
            ReturnOrderEntity entity = new ReturnOrderEntity();
            entity.OrderNum = argOrderNum;
            entity = GetOrder(entity);
            if (entity != null)
            {
                List<ReturnOrderEntity> list = new List<ReturnOrderEntity>();
                list.Add(entity);
                DataTable tableOrder = list.ToDataTable();
                ds.Tables.Add(tableOrder);

                ReturnDetailEntity detail = new ReturnDetailEntity();
                detail.OrderNum = argOrderNum;
                List<ReturnDetailEntity> listDetail = GetOrderDetail(detail);
                if (!listDetail.IsNullOrEmpty())
                {
                    DataTable tableDetail = listDetail.ToDataTable();
                    ds.Tables.Add(tableDetail);
                }
            }

            return ds;
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int GetCount(ReturnOrderEntity entity)
        {
            return this.ReturnOrder.GetCount(entity);
        }
    }
}
