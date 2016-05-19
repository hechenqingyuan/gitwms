/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-08-19 12:00:40
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 12:00:40
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;
using Git.Framework.MsSql;
using Git.Storage.Entity.InStorage;
using Git.Storage.IDataAccess.InStorage;
using Git.Framework.MsSql.DataAccess;
using Git.Storage.Entity.Report;

namespace Git.Storage.DataAccess.InStorage
{
	public partial class InStorageDataAccess : DbHelper<InStorageEntity>, IInStorage
	{
		public InStorageDataAccess()
		{
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
            DataCommand command = DataCommandManager.GetDataCommand("InStorage.InChartReport");
            command.SetParameterValue("@Status", status);
            command.SetParameterValue("@StorageNum", storageNum);
            command.SetParameterValue("@BeginTime", beginTime);
            command.SetParameterValue("@EndTime", endTime);
            List<ReportChart> listResult = command.ExecuteEntityList<ReportChart>();
            return listResult;
        }
    }
}
