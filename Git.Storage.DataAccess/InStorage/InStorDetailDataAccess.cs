/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-08-19 12:00:41
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 12:00:41
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

namespace Git.Storage.DataAccess.InStorage
{
	public partial class InStorDetailDataAccess : DbHelper<InStorDetailEntity>, IInStorDetail
	{
		public InStorDetailDataAccess()
		{
		}

	}
}
