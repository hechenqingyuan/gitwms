using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Git.Storage.Entity.Store
{


    [TableAttribute(DbName = "JooWMS", Name = "Proc_ProductReport", IsInternal = false, MapType = MapType.Proc)]
    public partial class Proc_ProductReportEntity:BaseEntity
	{
		public Proc_ProductReportEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32, Length = 4000,ColumnType=ColumnType.InPut)]
		public Int32 ID { get;  set; }

		[DataMapping(ColumnName = "IsTime", DbType = DbType.Int32, Length = 4000,ColumnType=ColumnType.InPut)]
		public Int32 IsTime { get;  set; }

		[DataMapping(ColumnName = "SearchKey", DbType = DbType.String, Length = 50,ColumnType=ColumnType.InPut)]
		public string SearchKey { get;  set; }

		[DataMapping(ColumnName = "ProductNum", DbType = DbType.String, Length = 50,ColumnType=ColumnType.InPut)]
		public string ProductNum { get;  set; }

		[DataMapping(ColumnName = "LocalProductNum", DbType = DbType.Double, Length = 4000,ColumnType=ColumnType.InOutPut)]
		public double LocalProductNum { get;  set; }

		[DataMapping(ColumnName = "InStorageNum", DbType = DbType.Double, Length = 4000,ColumnType=ColumnType.InOutPut)]
		public double InStorageNum { get;  set; }

		[DataMapping(ColumnName = "OutStorageNum", DbType = DbType.Double, Length = 4000,ColumnType=ColumnType.InOutPut)]
		public double OutStorageNum { get;  set; }

		[DataMapping(ColumnName = "BadReportNum", DbType = DbType.Double, Length = 4000,ColumnType=ColumnType.InOutPut)]
		public double BadReportNum { get;  set; }

		[DataMapping(ColumnName = "TotalLocalProductNum", DbType = DbType.Double, Length = 4000,ColumnType=ColumnType.InOutPut)]
		public double TotalLocalProductNum { get;  set; }

		[DataMapping(ColumnName = "TotalInStorageNum", DbType = DbType.Double, Length = 4000,ColumnType=ColumnType.InOutPut)]
		public double TotalInStorageNum { get;  set; }

		[DataMapping(ColumnName = "TotalOutStorageNum", DbType = DbType.Double, Length = 4000,ColumnType=ColumnType.InOutPut)]
		public double TotalOutStorageNum { get;  set; }

		[DataMapping(ColumnName = "TotalBadReportNum", DbType = DbType.Double, Length = 4000,ColumnType=ColumnType.InOutPut)]
		public double TotalBadReportNum { get;  set; }

		[DataMapping(ColumnName = "BeginTime", DbType = DbType.DateTime, Length = 4000,ColumnType=ColumnType.InPut)]
		public DateTime BeginTime { get;  set; }

		[DataMapping(ColumnName = "EndTime", DbType = DbType.DateTime, Length = 4000,ColumnType=ColumnType.InPut)]
		public DateTime EndTime { get;  set; }

		[DataMapping(ColumnName = "Status", DbType = DbType.Int32, Length = 4000,ColumnType=ColumnType.InPut)]
		public Int32 Status { get;  set; }

		[DataMapping(ColumnName = "IsDelete", DbType = DbType.Int32, Length = 4000,ColumnType=ColumnType.InPut)]
		public Int32 IsDelete { get;  set; }

	}
}
