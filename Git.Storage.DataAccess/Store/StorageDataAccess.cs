/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-08-19 11:59:54
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 11:59:54
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;
using Git.Framework.MsSql;
using Git.Storage.Entity.Store;
using Git.Storage.IDataAccess.Store;

namespace Git.Storage.DataAccess.Store
{
	public partial class StorageDataAccess : DbHelper<StorageEntity>, IStorage
	{
		public StorageDataAccess()
		{
		}

	}
}
