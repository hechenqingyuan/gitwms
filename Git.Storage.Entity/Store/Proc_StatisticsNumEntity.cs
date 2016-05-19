using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Git.Storage.Entity.Store
{

    [TableAttribute(DbName = "JooShowGit", Name = "Proc_StatisticsNum", IsInternal = false, MapType = MapType.Proc)]
    public partial class Proc_StatisticsNumEntity : BaseEntity
    {
        /// <summary>
        /// 统计产品库存数，进货总数，出货总数，报损总数实体
        /// </summary>
        public Proc_StatisticsNumEntity()
        {
        }

        /// <summary>
        /// ID
        /// </summary>
        [DataMapping(ColumnName = "ID", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InPut)]
        public Int32 ID { get; set; }

        /// <summary>
        /// 搜索关键词
        /// </summary>
        [DataMapping(ColumnName = "SearchKey", DbType = DbType.String, Length = 50, ColumnType = ColumnType.InPut)]
        public string SearchKey { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [DataMapping(ColumnName = "ProductNum", DbType = DbType.String, Length = 50, ColumnType = ColumnType.InPut)]
        public string ProductNum { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        [DataMapping(ColumnName = "LocalProductNum", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public Int32 LocalProductNum { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        [DataMapping(ColumnName = "InStorageNum", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public Int32 InStorageNum { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        [DataMapping(ColumnName = "OutStorageNum", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public Int32 OutStorageNum { get; set; }

        /// <summary>
        /// 报损数量
        /// </summary>
        [DataMapping(ColumnName = "BadReportNum", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public Int32 BadReportNum { get; set; }

        /// <summary>
        /// 总计库存数量
        /// </summary>
        [DataMapping(ColumnName = "TotalLocalProductNum", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public Int32 TotalLocalProductNum { get; set; }

        /// <summary>
        /// 总计入库数量
        /// </summary>
        [DataMapping(ColumnName = "TotalInStorageNum", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public Int32 TotalInStorageNum { get; set; }

        /// <summary>
        /// 总计出库数量
        /// </summary>
        [DataMapping(ColumnName = "TotalOutStorageNum", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public Int32 TotalOutStorageNum { get; set; }

        /// <summary>
        /// 总计报损数量
        /// </summary>
        [DataMapping(ColumnName = "TotalBadReportNum", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public Int32 TotalBadReportNum { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [DataMapping(ColumnName = "Status", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InPut)]
        public Int32 Status { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMapping(ColumnName = "IsDelete", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InPut)]
        public Int32 IsDelete { get; set; }



        [DataMapping(ColumnName = "BeginTime", DbType = DbType.DateTime, Length = 8, ColumnType = ColumnType.InPut)]
        public DateTime BeginTime { get; set; }


        [DataMapping(ColumnName = "EndTime", DbType = DbType.DateTime, Length = 8, ColumnType = ColumnType.InPut)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 是否算上时间条件
        /// </summary>
        [DataMapping(ColumnName = "IsTime", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InPut)]
        public Int32 IsTime { get; set; }
    }
}
