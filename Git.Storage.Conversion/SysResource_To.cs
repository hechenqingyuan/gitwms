/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:45
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:45
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Base;

namespace Git.Storage.Conversion
{
	public partial class SysResource_To
	{
		public SysResource_To()
		{
		}

		public static SysResourceEntity To(SysResource_CE item)
		{
			SysResourceEntity target = new SysResourceEntity();
			target.ID=item.ID;
			target.ResNum=item.ResNum;
			target.ResName=item.ResName;
			target.ParentNum=item.ParentNum;
			target.Depth=item.Depth;
			target.ParentPath=item.ParentPath;
			target.ChildCount=item.ChildCount;
			target.Sort=item.Sort;
			target.IsHide=item.IsHide;
			target.IsDelete=item.IsDelete;
			target.Url=item.Url;
			target.CssName=item.CssName;
			target.CreateTime=item.CreateTime;
			target.Depart=item.Depart;
			target.ResType=item.ResType;
			target.UpdateTime=item.UpdateTime;
			target.CreateUser=item.CreateUser;
			target.UpdateUser=item.UpdateUser;
			target.CreateIp=item.CreateIp;
			target.UpdateIp=item.UpdateIp;
			target.Remark=item.Remark;
			return target;
		}

		public static SysResource_CE ToCE(SysResourceEntity item)
		{
			SysResource_CE target = new SysResource_CE();
			target.ID=item.ID;
			target.ResNum=item.ResNum;
			target.ResName=item.ResName;
			target.ParentNum=item.ParentNum;
			target.Depth=item.Depth;
			target.ParentPath=item.ParentPath;
			target.ChildCount=item.ChildCount;
			target.Sort=item.Sort;
			target.IsHide=item.IsHide;
			target.IsDelete=item.IsDelete;
			target.Url=item.Url;
			target.CssName=item.CssName;
			target.CreateTime=item.CreateTime;
			target.Depart=item.Depart;
			target.ResType=item.ResType;
			target.UpdateTime=item.UpdateTime;
			target.CreateUser=item.CreateUser;
			target.UpdateUser=item.UpdateUser;
			target.CreateIp=item.CreateIp;
			target.UpdateIp=item.UpdateIp;
			target.Remark=item.Remark;
			return target;
		}
	}
}
