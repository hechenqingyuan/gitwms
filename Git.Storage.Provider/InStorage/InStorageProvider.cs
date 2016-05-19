/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-11-30 9:55:28
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-11-30 9:55:28       情缘
*********************************************************************************/

using Git.Framework.Log;
using Git.Storage.Entity.InStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.DataTypes;
using Git.Framework.ORM;
using Git.Storage.Common;
using Git.Storage.Entity.Store;
using Git.Storage.Entity.OutStorage;
using Git.Framework.MsSql.DataAccess;
using Git.Storage.Entity.Report;

namespace Git.Storage.Provider.InStorage
{
    public partial class InStorageProvider : DataFactory
    {
        private Log log = Log.Instance(typeof(InStorageOrder));

        public InStorageProvider() { }

        /// <summary>
        /// 入库报表
        /// </summary>
        /// <param name="queryTime"></param>
        /// <param name="pageInfo"></param>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public List<Proc_InStorageReportEntity> GetList(int queryTime, ref PageInfo pageInfo, string storageNum)
        {

            Proc_InStorageReportEntity entity = new Proc_InStorageReportEntity();
            entity.StorageNum = storageNum;
            entity.BeginTime = DateTime.Now.AddDays(-queryTime);
            entity.EndTime = DateTime.Now;
            entity.PageIndex = pageInfo.PageIndex;
            entity.PageSize = pageInfo.PageSize;
            List<Proc_InStorageReportEntity> listResult = this.Proc_InStorageReport.ExceuteEntityList<Proc_InStorageReportEntity>(entity);
            pageInfo.RowCount = entity.RecordCount;
            return listResult;
        }

        /// <summary>
        /// 数量排名前十的供应商产品数
        /// </summary>
        /// <param name="queryTime"></param>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public List<InStorageEntity> GetListTOP10(int queryTime, string storageNum)
        {
            DataCommand command = DataCommandManager.GetDataCommand("InStorage.SupplierReport");
            command.SetParameterValue("@Status", (int)EAudite.Pass);
            command.SetParameterValue("@StorageNum", storageNum);
            command.SetParameterValue("@IsDelete", (int)EIsDelete.NotDelete);
            command.SetParameterValue("@BeginTime", DateTime.Now.AddDays(-queryTime));
            command.SetParameterValue("@EndTime", DateTime.Now);
            return command.ExecuteEntityList<InStorageEntity>();
        }

        /// <summary>
        /// 获得某个供应商的所有订购产品数量
        /// </summary>
        /// <param name="supNum"></param>
        /// <param name="queryTime"></param>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public double GetNumBySupNum(string supNum, int queryTime, string storageNum)
        {
            InStorageEntity entity = new InStorageEntity();
            entity.IncludeNum(true);
            entity.Where("CreateTime", ECondition.Between, DateTime.Now.AddDays(-queryTime), DateTime.Now);
            entity.And("StorageNum", ECondition.Eth, storageNum);
            entity.And("SupNum", ECondition.Eth, supNum);
            entity.And("Status", ECondition.Eth, (int)EAudite.Pass);
            entity.And("IsDelete", ECondition.Eth, (int)EIsDelete.NotDelete);
            double allNum = 0;
            try
            {
                allNum = this.InStorage.Sum<double>(entity);
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
            List<ReportChart> listSource = this.InStorage.GetChartTop(status, storageNum, beginTime, endTime);
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
        /// 编辑入库单详细数量
        /// </summary>
        /// <param name="snNum"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int EditInOrderNum(string snNum, double num)
        {
            InStorDetailEntity detail = new InStorDetailEntity();
            detail = new InStorDetailEntity();
            detail.Include(a => new { a.OrderNum, a.Num, a.InPrice });
            detail.Where(a => a.SnNum == snNum);
            detail = this.InStorDetail.GetSingle(detail);

            int line = 0;
            if (detail != null)
            {
                InStorDetailEntity editEntity = new InStorDetailEntity();
                editEntity.IncludeNum(true).IncludeAmount(true);
                editEntity.Num = num;
                editEntity.Amount = num * detail.InPrice;
                editEntity.Where(a => a.SnNum == snNum);
                line = this.InStorDetail.Update(editEntity);

                string orderNum = detail.OrderNum;
                detail = new InStorDetailEntity();
                detail.IncludeNum(true).IncludeAmount(true);
                detail.Where(a => a.OrderNum == orderNum);
                List<InStorDetailEntity> list = this.InStorDetail.GetList(detail);

                double total = list.Sum(a => a.Num);
                double amount = list.Sum(a => a.Amount);
                InStorageEntity entity = new InStorageEntity();
                entity.Num = total;
                entity.Amount=amount;
                entity.IncludeNum(true).IncludeAmount(true);
                entity.Where(a => a.OrderNum == orderNum);

                line += this.InStorage.Update(entity);
            }
            return line;
        }
    }
}
