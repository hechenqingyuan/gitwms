/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-30 12:41:16
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-30 12:41:16       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Framework.Log;
using Git.Storage.Entity.OutStorage;
using Git.Framework.ORM;
using Git.Framework.DataTypes;
using Git.Framework.MsSql.DataAccess;
using Git.Storage.Common;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Report;

namespace Git.Storage.Provider.OutStorage
{
    public partial class OutStorageProvider : DataFactory
    {
        private readonly Log log = Log.Instance(typeof(OutStorageProvider));

        public OutStorageProvider() { }

        /// <summary>
        /// 出库报表
        /// </summary>
        /// <param name="queryTime"></param>
        /// <param name="pageInfo"></param>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public List<Proc_OutStorageReportEntity> GetList(int queryTime, ref PageInfo pageInfo, string storageNum)
        {

            Proc_OutStorageReportEntity entity = new Proc_OutStorageReportEntity();
            entity.StorageNum = storageNum;
            entity.BeginTime = DateTime.Now.AddDays(-queryTime);
            entity.EndTime = DateTime.Now;
            entity.PageIndex = pageInfo.PageIndex;
            entity.PageSize = pageInfo.PageSize;
            List<Proc_OutStorageReportEntity> listResult = this.Proc_OutStorageReport.ExceuteEntityList<Proc_OutStorageReportEntity>(entity);
            pageInfo.RowCount = entity.RecordCount;
            return listResult;
        }

        /// <summary>
        /// 数量排名前十的客户产品数
        /// </summary>
        /// <param name="queryTime"></param>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public List<OutStorageEntity> GetListTOP10(int queryTime, string storageNum)
        {
            DataCommand command = DataCommandManager.GetDataCommand("OutStorage.CustomerReport");
            command.SetParameterValue("@StorageNum", storageNum);
            command.SetParameterValue("@Status", (int)EAudite.Pass);
            command.SetParameterValue("@IsDelete", (int)EIsDelete.NotDelete);
            command.SetParameterValue("@BeginTime", DateTime.Now.AddDays(-queryTime));
            command.SetParameterValue("@EndTime", DateTime.Now);
            return command.ExecuteEntityList<OutStorageEntity>();
        }

        /// <summary>
        /// 获得某个客户的所有订购产品数量
        /// </summary>
        /// <param name="cusNum"></param>
        /// <param name="queryTime"></param>
        /// <returns></returns>
        public double GetNumByCusNum(string cusNum, int queryTime, string storageNum)
        {
            OutStorageEntity entity = new OutStorageEntity();
            entity.IncludeNum(true);
            entity.Where("CreateTime", ECondition.Between, DateTime.Now.AddDays(-queryTime), DateTime.Now);
            entity.And("StorageNum", ECondition.Eth, storageNum);
            entity.And("CusNum", ECondition.Eth, cusNum);
            entity.And("Status", ECondition.Eth, (int)EAudite.Pass);
            entity.And("IsDelete", ECondition.Eth, (int)EIsDelete.NotDelete);
            double allNum = 0;
            try
            {
                allNum = this.OutStorage.Sum<double>(entity);
            }
            catch (Exception e)
            {
                allNum = 0;
                log.Info(e.Message);
            }
            return allNum;
        }


        /// <summary>
        /// 查询指定时间段范围内各个产品的数量
        /// </summary>
        /// <param name="status"></param>
        /// <param name="storageNum"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<ReportChart> GetChartTop(int status, string storageNum, DateTime beginTime, DateTime endTime)
        {
            List<ReportChart> listSource = this.OutStorage.GetChartTop(status, storageNum, beginTime, endTime);
            List<ReportChart> listResult = new List<ReportChart>();
            if (!listSource.IsNullOrEmpty())
            {
                listSource = listSource.OrderByDescending(a => a.Num).ToList();
                if (listSource.Count > 9)
                {
                    listResult = listSource.Take(9).ToList();
                    ReportChart chart = new ReportChart();
                    chart.ProductName = "其他";
                    chart.Num = listResult.Skip(9).Sum(a => a.Num);
                    listResult.Add(chart);
                }
                else
                {
                    listResult = listSource.Take(9).ToList();
                }
            }
            return listResult;
        }

        /// <summary>
        /// 修改出库单的数量
        /// </summary>
        /// <param name="snNum"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int EditInOrderNum(string snNum, double num)
        {
            OutStoDetailEntity detail = new OutStoDetailEntity();
            detail = new OutStoDetailEntity();
            detail.Include(a => new { a.OrderNum, a.Num,a.OutPrice,a.Amount });
            detail.Where(a => a.SnNum == snNum);
            detail = this.OutStoDetail.GetSingle(detail);

            int line = 0;
            if (detail != null)
            {
                string orderNum = detail.OrderNum;
                OutStoDetailEntity editDetail = new OutStoDetailEntity();
                editDetail.Num = num;
                editDetail.Amount = editDetail.Num * editDetail.OutPrice;
                editDetail.IncludeNum(true).IncludeAmount(true);
                editDetail.Where(a => a.SnNum == snNum);
                line = this.OutStoDetail.Update(editDetail);

                OutStoDetailEntity orderDetail = new OutStoDetailEntity();
                orderDetail.Include(a => new { a.Num, a.Amount });
                orderDetail.Where(a => a.OrderNum == orderNum);
                List<OutStoDetailEntity> list = this.OutStoDetail.GetList(orderDetail);

                OutStorageEntity entity = new OutStorageEntity();
                entity.Num = list.Sum(a=>a.Num);
                entity.Amount = list.Sum(a => a.Amount);
                entity.IncludeNum(true).IncludeAmount(true);
                entity.Where(a => a.OrderNum == orderNum);
                line += this.OutStorage.Update(entity);
            }
            return line;
        }

        /// <summary>
        /// 获得订单详细查询
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<OutStoDetailEntity> GetOrderDetail(OutStoDetailEntity entity)
        {
            entity.IncludeAll();
            List<OutStoDetailEntity> list = this.OutStoDetail.GetList(entity);
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

    }
}
