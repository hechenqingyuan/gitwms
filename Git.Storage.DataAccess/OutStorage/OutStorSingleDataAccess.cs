/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-08-19 12:03:02
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 12:03:02
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
	public partial class OutStorSingleDataAccess : DbHelper<OutStorSingleEntity>, IOutStorSingle
	{
		public OutStorSingleDataAccess()
		{
		}

	}
}
