/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:43:15
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:43:15
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Store;

namespace Git.Storage.Conversion
{
	public partial class CusAddress_To
	{
		public CusAddress_To()
		{
		}

		public static CusAddressEntity To(CusAddress_CE item)
		{
			CusAddressEntity target = new CusAddressEntity();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.CusNum=item.CusNum;
			target.Contact=item.Contact;
			target.Phone=item.Phone;
			target.Address=item.Address;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.Remark=item.Remark;
			return target;
		}

		public static CusAddress_CE ToCE(CusAddressEntity item)
		{
			CusAddress_CE target = new CusAddress_CE();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.CusNum=item.CusNum;
			target.Contact=item.Contact;
			target.Phone=item.Phone;
			target.Address=item.Address;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.Remark=item.Remark;
			return target;
		}
	}
}
