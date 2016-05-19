/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:43:50
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:43:50
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.Entity.Base;
using Git.Storage.RoseEntity;

namespace Git.Storage.Conversion
{
	public partial class VnProvince_To
	{
		public VnProvince_To()
		{
		}

		public static VnProvinceEntity To(VnProvince_CE item)
		{
			VnProvinceEntity target = new VnProvinceEntity();
			target.ID=item.ID;
			target.Code=item.Code;
			target.PName=item.PName;
			target.PNameEn=item.PNameEn;
			return target;
		}

		public static VnProvince_CE ToCE(VnProvinceEntity item)
		{
			VnProvince_CE target = new VnProvince_CE();
			target.ID=item.ID;
			target.Code=item.Code;
			target.PName=item.PName;
			target.PNameEn=item.PNameEn;
			return target;
		}
	}
}
