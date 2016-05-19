/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:45:42
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:45:42
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Base;

namespace Git.Storage.Conversion
{
	public partial class VnCity_To
	{
		public VnCity_To()
		{
		}

		public static VnCityEntity To(VnCity_CE item)
		{
			VnCityEntity target = new VnCityEntity();
			target.ID=item.ID;
			target.Code=item.Code;
			target.CName=item.CName;
			target.CNameEn=item.CNameEn;
			target.PCode=item.PCode;
			return target;
		}

		public static VnCity_CE ToCE(VnCityEntity item)
		{
			VnCity_CE target = new VnCity_CE();
			target.ID=item.ID;
			target.Code=item.Code;
			target.CName=item.CName;
			target.CNameEn=item.CNameEn;
			target.PCode=item.PCode;
			return target;
		}
	}
}
