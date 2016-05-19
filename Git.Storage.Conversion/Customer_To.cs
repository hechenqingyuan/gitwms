/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:58
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:58
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Store;

namespace Git.Storage.Conversion
{
	public partial class Customer_To
	{
		public Customer_To()
		{
		}

		public static CustomerEntity To(Customer_CE item)
		{
			CustomerEntity target = new CustomerEntity();
			target.ID=item.ID;
			target.CusNum=item.CusNum;
			target.CusName=item.CusName;
			target.Phone=item.Phone;
			target.Email=item.Email;
			target.Fax=item.Fax;
			target.Address=item.Address;
			target.CusType=item.CusType;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.Remark=item.Remark;
			return target;
		}

		public static Customer_CE ToCE(CustomerEntity item)
		{
			Customer_CE target = new Customer_CE();
			target.ID=item.ID;
			target.CusNum=item.CusNum;
			target.CusName=item.CusName;
			target.Phone=item.Phone;
			target.Email=item.Email;
			target.Fax=item.Fax;
			target.Address=item.Address;
			target.CusType=item.CusType;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.Remark=item.Remark;
			return target;
		}
	}
}
