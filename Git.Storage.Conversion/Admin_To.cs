/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:43:19
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:43:19
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Base;

namespace Git.Storage.Conversion
{
	public partial class Admin_To
	{
		public Admin_To()
		{
		}

		public static AdminEntity To(Admin_CE item)
		{
			AdminEntity target = new AdminEntity();
			target.ID=item.ID;
			target.UserName=item.UserName;
			target.PassWord=item.PassWord;
			target.UserCode=item.UserCode;
			target.RealName=item.RealName;
			target.Email=item.Email;
			target.Mobile=item.Mobile;
			target.Phone=item.Phone;
			target.CreateTime=item.CreateTime;
			target.CreateIp=item.CreateIp;
			target.CreateUser=item.CreateUser;
			target.LoginCount=item.LoginCount;
			target.Picture=item.Picture;
			target.UpdateTime=item.UpdateTime;
			target.IsDelete=item.IsDelete;
			target.Status=item.Status;
			target.DepartNum=item.DepartNum;
			target.ParentCode=item.ParentCode;
			target.RoleNum=item.RoleNum;
			target.Remark=item.Remark;
			return target;
		}

		public static Admin_CE ToCE(AdminEntity item)
		{
			Admin_CE target = new Admin_CE();
			target.ID=item.ID;
			target.UserName=item.UserName;
			target.PassWord=item.PassWord;
			target.UserCode=item.UserCode;
			target.RealName=item.RealName;
			target.Email=item.Email;
			target.Mobile=item.Mobile;
			target.Phone=item.Phone;
			target.CreateTime=item.CreateTime;
			target.CreateIp=item.CreateIp;
			target.CreateUser=item.CreateUser;
			target.LoginCount=item.LoginCount;
			target.Picture=item.Picture;
			target.UpdateTime=item.UpdateTime;
			target.IsDelete=item.IsDelete;
			target.Status=item.Status;
			target.DepartNum=item.DepartNum;
			target.ParentCode=item.ParentCode;
			target.RoleNum=item.RoleNum;
			target.Remark=item.Remark;
			return target;
		}
	}
}
