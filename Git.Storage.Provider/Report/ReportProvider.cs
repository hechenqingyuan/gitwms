using Git.Storage.Entity.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Common;
using Git.Framework.ORM;
using System.Data;
using Git.Storage.Entity.InStorage;
using Git.Storage.Provider.InStorage;
using Git.Storage.Entity.OutStorage;
using Git.Storage.Provider.OutStorage;
using Git.Storage.Provider.Base;

namespace Git.Storage.Provider.Report
{
    public partial class ReportProvider:DataFactory
    {
        public ReportProvider() { }

        /// <summary>
        /// 创建报表文件格式
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public int Create(ReportsEntity entity,List<ReportParamsEntity> list)
        {
            if (!entity.ReportNum.IsEmpty())
            {
                return Update(entity,list);
            }
            int line = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                entity.ReportNum = entity.ReportNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(ReportsEntity)) : entity.ReportNum;
                entity.IncludeAll();
                line += this.Reports.Add(entity);

                if (!list.IsNullOrEmpty())
                {
                    foreach (ReportParamsEntity item in list)
                    {
                        item.ParamNum = item.ParamNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(ReportParamsEntity)) : item.ParamNum;
                        item.ReportNum = entity.ReportNum;
                        item.IncludeAll();
                    }
                    this.ReportParams.Add(list);
                }
                ts.Complete();
            }

            return line;
        }

        /// <summary>
        /// 修改报表格式
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public int Update(ReportsEntity entity, List<ReportParamsEntity> list)
        {
            int line = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                ReportParamsEntity param = new ReportParamsEntity();
                param.Where(a => a.ReportNum == entity.ReportNum);
                line += this.ReportParams.Delete(param);

                entity.Include(a => new { a.ReportName, a.ReportType, a.Remark, a.DataSource, a.DsType, a.FileName });
                entity.Where(a => a.ReportNum == entity.ReportNum);
                line += this.Reports.Update(entity);

                if (!list.IsNullOrEmpty())
                {
                    foreach (ReportParamsEntity item in list)
                    {
                        item.ParamNum = item.ParamNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(ReportParamsEntity)) : item.ParamNum;
                        item.ReportNum = entity.ReportNum;
                        item.IncludeAll();
                    }
                    this.ReportParams.Add(list);
                }
                ts.Complete();
            }

            return line;
        }

        /// <summary>
        /// 查询报表
        /// </summary>
        /// <param name="argReportNum"></param>
        /// <returns></returns>
        public ReportsEntity GetReport(string argReportNum)
        {
            ReportsEntity entity = new ReportsEntity();
            entity.IncludeAll();
            entity.Where(a => a.ReportNum == argReportNum)
                .And(a=>a.IsDelete==(int)EIsDelete.NotDelete)
                ;
            entity = this.Reports.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 根据报表格式编号删除报表
        /// </summary>
        /// <param name="reportNum"></param>
        /// <returns></returns>
        public int Delete(string reportNum)
        {
            ReportsEntity entity = new ReportsEntity();
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.ReportNum == reportNum);
            int line = this.Reports.Update(entity);

            return line;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int Delete(List<string> list)
        {
            if (!list.IsNullOrEmpty())
            {
                ReportsEntity entity = new ReportsEntity();
                entity.IsDelete = (int)EIsDelete.Deleted;
                entity.IncludeIsDelete(true);
                entity.Where("ReportNum", ECondition.In, list.ToArray());
                int line = this.Reports.Update(entity);
                return line;
            }
            return 0;
        }

        /// <summary>
        /// 报表查询分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<ReportsEntity> GetList(ReportsEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID,EOrderBy.ASC);
            int rowCount = 0;
            List<ReportsEntity> listResult = this.Reports.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 根据报表格式编号查询参数信息
        /// </summary>
        /// <param name="reportNum"></param>
        /// <returns></returns>
        public List<ReportParamsEntity> GetParams(string reportNum)
        {
            ReportParamsEntity entity = new ReportParamsEntity();
            entity.IncludeAll();
            entity.Where(a => a.ReportNum == reportNum);
            List<ReportParamsEntity> list = this.ReportParams.GetList(entity);
            return list;
        }

        /// <summary>
        /// 根据存储过程名称查询元数据信息
        /// </summary>
        /// <param name="argProceName"></param>
        /// <returns></returns>
        public List<ReportParamsEntity> GetProceMetadata(string argProceName)
        {
            List<ProceMetadata> list = this.Reports.GetMetadataList(argProceName);
            if (!list.IsNullOrEmpty())
            {
                List<ReportParamsEntity> listResult = new List<ReportParamsEntity>();
                foreach (ProceMetadata item in list)
                {
                    ReportParamsEntity entity = new ReportParamsEntity();
                    entity.ReportNum = string.Empty;
                    entity.ParamNum = SequenceProvider.GetSequence(typeof(ReportParamsEntity));
                    entity.InputNo = item.ORDINAL_POSITION.ToString();
                    entity.ParamName = item.PARAMETER_NAME;
                    entity.ShowName = string.Empty;
                    entity.ParamType = item.DATA_TYPE;
                    entity.ParamData = "";
                    entity.DefaultValue = "";
                    listResult.Add(entity);
                }
                return listResult;
            }
            return null;
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public DataSet GetDataSource(ReportsEntity entity, List<ReportParamsEntity> list,int orderType,string orderNum)
        {
            DataSet ds = null;
            if (entity.ReportType == (int)EReportType.Report)
            {
                ds = this.Reports.GetDataSource(entity, list);
            }
            else
            {
                if (orderType == (int)EOrder.InOrder)
                {
                    Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
                    ds = bill.GetPrint(orderNum);
                }
                else if (orderType == (int)EOrder.OutOrder)
                {
                    Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
                    ds = bill.GetPrint(orderNum);
                }
            }
            return ds;
        }
    }
}
