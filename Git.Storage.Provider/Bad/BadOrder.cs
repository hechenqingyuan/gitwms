/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-30 12:42:01
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-30 12:42:01       情缘
*********************************************************************************/

using Git.Storage.Entity.Bad;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Log;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Git.Storage.Common;
using Git.Storage.Entity.Base;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Store;
using System.Data;
using Git.Storage.Provider.Base;

namespace Git.Storage.Provider.Bad
{
    public partial class BadOrder : Bill<BadReportEntity, BadReportDetailEntity>
    {
        public BadOrder() { }


        /// <summary>
        /// 创建单据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string Create(BadReportEntity entity, List<BadReportDetailEntity> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                entity.OrderNum = entity.OrderNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(BadReportEntity)) : entity.OrderNum;
                entity.IncludeAll();
                if (!list.IsNullOrEmpty())
                {
                    list.ForEach(a =>
                    {
                        a.IncludeAll();
                        a.OrderNum = entity.OrderNum;
                    });
                    entity.Amount = list.Sum(q => q.Num);
                    entity.Amount = list.Sum(a => a.Amount);
                    line = this.BadReport.Add(entity);
                    line += this.BadReportDetail.Add(list);
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
        public override string Cancel(BadReportEntity entity)
        {
            //只有待审核状态的单据才能取消，已经成功的订单不能取消
            BadReportEntity checkOrder = new BadReportEntity();
            entity.Where(a => a.Status == (int)EAudite.Wait).And(a => a.OrderNum == entity.OrderNum);
            if (this.BadReport.GetCount(checkOrder) > 0)
            {
                return EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Pass); //已经审核或者取消的订单不能审核
            }
            entity.Status = (int)EAudite.NotPass;
            entity.IncludeStatus(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.BadReport.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Delete(BadReportEntity entity)
        {
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.BadReport.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Audite(BadReportEntity entity)
        {
            if (entity.Status == (int)EAudite.NotPass)
            {
                entity.IncludeStatus(true).IncludeReason(true).IncludeAuditUser(true).Where(a => a.OrderNum == entity.OrderNum);
                int line = this.BadReport.Update(entity);
                return line > 0 ? "1000" : string.Empty;
            }
            else if (entity.Status == (int)EAudite.Pass)
            {
                Proc_AuditeBadReportEntity auditeEntity = new Proc_AuditeBadReportEntity();
                auditeEntity.OrderNum = entity.OrderNum;
                auditeEntity.Status = entity.Status;
                auditeEntity.AuditUser = entity.AuditUser;
                auditeEntity.Reason = entity.Reason;
                auditeEntity.OperateType = entity.OperateType;
                auditeEntity.EquipmentNum = entity.EquipmentNum;
                auditeEntity.EquipmentCode = entity.EquipmentCode;
                int line = this.Proc_AuditeBadReport.ExecuteNonQuery(auditeEntity);
                return auditeEntity.ReturnValue;
            }
            return string.Empty;
        }

        /// <summary>
        /// 打印单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string Print(BadReportEntity entity)
        {
            entity.IncludePrintUser(true).IncludePrintTime(true)
                .Where(a => a.OrderNum == entity.OrderNum);
            int line = this.BadReport.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 查询单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override BadReportEntity GetOrder(BadReportEntity entity)
        {
            entity.IncludeAll();
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            entity = this.BadReport.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 获得单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override List<BadReportDetailEntity> GetOrderDetail(BadReportDetailEntity entity)
        {
            BadReportDetailEntity detail = new BadReportDetailEntity();
            detail.IncludeAll();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            List<BadReportDetailEntity> list = this.BadReportDetail.GetList(detail);
            if (!list.IsNullOrEmpty())
            {
                List<LocationEntity> listLocation = new LocationProvider().GetList();
                listLocation = listLocation == null ? new List<LocationEntity>() : listLocation;
                foreach (BadReportDetailEntity item in list)
                {
                    LocationEntity location = listLocation.FirstOrDefault(a => a.LocalNum == item.FromLocalNum);
                    item.FromLocalName = location == null ? "" : location.LocalName;

                    location = listLocation.FirstOrDefault(a => a.LocalNum == item.ToLocalNum);
                    item.ToLocalName = location == null ? "" : location.LocalName;
                    if (item.Amount == 0)
                    {
                        item.Amount = item.InPrice * item.Num;
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
        public override List<BadReportEntity> GetList(BadReportEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            AdminEntity admin = new AdminEntity();
            admin.Include(a => new { CreateUserName = a.UserName });
            entity.Left<AdminEntity>(admin, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            int rowCount = 0;
            List<BadReportEntity> listResult = this.BadReport.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 查询单据详细数据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override List<BadReportDetailEntity> GetDetailList(BadReportDetailEntity entity, ref Framework.DataTypes.PageInfo pageInfo)
        {
            BadReportDetailEntity detail = new BadReportDetailEntity();
            detail.Where(a => a.OrderNum == entity.OrderNum);
            detail.IncludeAll();
            detail.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<BadReportDetailEntity> listResult = this.BadReportDetail.GetList(detail, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            if (!listResult.IsNullOrEmpty())
            {
                List<LocationEntity> listLocation = new LocationProvider().GetList();
                listLocation = listLocation == null ? new List<LocationEntity>() : listLocation;
                foreach (BadReportDetailEntity item in listResult)
                {
                    LocationEntity location = listLocation.FirstOrDefault(a => a.LocalNum == item.FromLocalNum);
                    item.FromLocalName = location == null ? "" : location.LocalName;

                    location = listLocation.FirstOrDefault(a => a.LocalNum == item.ToLocalNum);
                    item.ToLocalName = location == null ? "" : location.LocalName;
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
        public override string EditOrder(BadReportEntity entity)
        {
            entity.Include(a => new { a.BadType, a.ProductType, a.ContractOrder, a.Remark, a.Amount, a.Num });
            entity.Where(a => a.OrderNum == entity.OrderNum);
            int line = this.BadReport.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 编辑单据详细信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string EditDetail(BadReportDetailEntity entity)
        {
            entity.Where(a => a.SnNum == entity.SnNum);
            int line = this.BadReportDetail.Update(entity);
            return line > 0 ? EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) : string.Empty;
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int GetCount(BadReportEntity entity)
        {
            return this.BadReport.GetCount(entity);
        }

        /// <summary>
        /// 编辑报损单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override string EditOrder(BadReportEntity entity, List<BadReportDetailEntity> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                entity.Include(a => new { a.BadType, a.ProductType, a.ContractOrder, a.Remark, a.Amount, a.Num });
                entity.Where(a => a.OrderNum == entity.OrderNum);
                BadReportDetailEntity detail = new BadReportDetailEntity();
                detail.Where(a => a.OrderNum == entity.OrderNum);
                this.BadReportDetail.Delete(detail);
                foreach (BadReportDetailEntity item in list)
                {
                    item.OrderNum = entity.OrderNum;
                    item.IncludeAll();
                }
                entity.Num = list.Sum(a => a.Num);
                entity.Amount = list.Sum(a => a.Amount);
                line = this.BadReport.Update(entity);
                this.BadReportDetail.Add(list);
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
            BadReportEntity entity = new BadReportEntity();
            entity.OrderNum = argOrderNum;
            entity = GetOrder(entity);
            if (entity != null)
            {
                List<BadReportEntity> list = new List<BadReportEntity>();
                list.Add(entity);
                DataTable tableOrder = list.ToDataTable();
                ds.Tables.Add(tableOrder);

                BadReportDetailEntity detail = new BadReportDetailEntity();
                detail.OrderNum = argOrderNum;
                List<BadReportDetailEntity> listDetail = GetOrderDetail(detail);
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
