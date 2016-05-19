/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 23:00:56
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 23:00:56
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Base;

namespace Git.Storage.Conversion
{
	public partial class Measure_To
	{
		public Measure_To()
		{
		}

		public static MeasureEntity To(Measure_CE item)
		{
			MeasureEntity target = new MeasureEntity();
			target.ID=item.ID;
			target.SN=item.SN;
			target.MeasureNum=item.MeasureNum;
			target.MeasureName=item.MeasureName;
			return target;
		}

		public static Measure_CE ToCE(MeasureEntity item)
		{
			Measure_CE target = new Measure_CE();
			target.ID=item.ID;
			target.SN=item.SN;
			target.MeasureNum=item.MeasureNum;
			target.MeasureName=item.MeasureName;
			return target;
		}
	}
}
