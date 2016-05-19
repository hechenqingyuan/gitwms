/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/03/09 12:57:58
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/03/09 12:57:58
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;
using Git.Framework.MsSql;
using Git.Storage.Entity.OutStorage;
using Git.Storage.IDataAccess.OutStorage;

namespace Git.Storage.DataAccess.OutStorage
{
	public partial class Proc_AuditeOutStorageDataAccess : DbProcHelper<Proc_AuditeOutStorageEntity>, IProc_AuditeOutStorage
	{
		public Proc_AuditeOutStorageDataAccess()
		{
		}

	}
}
