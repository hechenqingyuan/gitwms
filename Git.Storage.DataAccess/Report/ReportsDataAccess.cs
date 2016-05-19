/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2015/09/05 13:07:30
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2015/09/05 13:07:30
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;
using Git.Framework.MsSql;
using Git.Storage.Entity.Report;
using Git.Storage.IDataAccess.Report;
using Git.Framework.MsSql.DataAccess;
using Git.Storage.Common;

namespace Git.Storage.DataAccess.Report
{
	public partial class ReportsDataAccess : DbHelper<ReportsEntity>, IReports
	{
		public ReportsDataAccess()
		{
		}

        /// <summary>
        /// 根据存储过程名称查询元数据
        /// </summary>
        /// <param name="argProceName"></param>
        /// <returns></returns>
        public List<ProceMetadata> GetMetadataList(string argProceName)
        {
            DataCommand command = DataCommandManager.GetDataCommand("Common.GetProceParam");
            command.SetParameterValue("@SPECIFIC_NAME", argProceName);
            List<ProceMetadata> list = command.ExecuteEntityList<ProceMetadata>();
            
            return list;
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public DataSet GetDataSource(ReportsEntity entity, List<ReportParamsEntity> list)
        {
            DataCommand command = null;
            DataSet ds = null;
            if (entity.DsType == (int)EDataSourceType.SQL)
            {
                command = DataCommandManager.CreateCustomDataCommand("JooWMS", CommandType.Text, entity.DataSource);
            }
            else
            {
                command = DataCommandManager.CreateCustomDataCommand("JooWMS", CommandType.StoredProcedure, entity.DataSource);
            }
            if (list != null)
            {
                foreach (ReportParamsEntity item in list)
                {
                    DbType dbType = DbType.String;
                    if (item.ParamType == "datetime" || item.ParamType=="date")
                    {
                        dbType = DbType.DateTime;
                        item.DefaultValue = string.IsNullOrEmpty(item.DefaultValue) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.DefaultValue;
                    }
                    else if (item.ParamType == "int")
                    {
                        dbType = DbType.Int32;
                        item.DefaultValue = string.IsNullOrEmpty(item.DefaultValue) ? "0" : item.DefaultValue;
                    }
                    else
                    {
                        item.DefaultValue = string.IsNullOrEmpty(item.DefaultValue) ? "" : item.DefaultValue;
                    }
                    command.AddParameterValue(item.ParamName, item.DefaultValue, dbType);
                }
            }
            ds = command.ExecuteDataSet();

            return ds;
        }
	}
}
