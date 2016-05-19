/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-06-24 21:20:16
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-06-24 21:20:16       情缘
*********************************************************************************/

using Git.Framework.Cache;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.Log;
using Git.Storage.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Storage.Provider.Base
{
    public partial class MeasureProvider : DataFactory
    {
        public MeasureProvider() { }

        /// <summary>
        /// 添加计量单位
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddMeasure(MeasureEntity entity)
        {
            entity.IncludeAll();
            int line = this.Measure.Add(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_MEASURE_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 获得所有的计量单位
        /// </summary>
        /// <returns></returns>
        public List<MeasureEntity> GetList()
        {
            List<MeasureEntity> listResult = CacheHelper.Get(CacheKey.JOOSHOW_MEASURE_CACHE) as List<MeasureEntity>;
            if (!listResult.IsNullOrEmpty())
            {
                return listResult;
            }
            MeasureEntity entity = new MeasureEntity();
            entity.IncludeAll();
            listResult = this.Measure.GetList(entity);
            if (!listResult.IsNullOrEmpty())
            {
                CacheHelper.Insert(CacheKey.JOOSHOW_MEASURE_CACHE,listResult);
            }
            return listResult;
        }

        /// <summary>
        /// 根据计量单位删除
        /// </summary>
        /// <param name="measureNum"></param>
        /// <returns></returns>
        public int DeleteMeasure(string measureNum)
        {
            MeasureEntity entity = new MeasureEntity();
            entity.Where(a => a.MeasureNum == measureNum);
            int line = this.Measure.Delete(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_MEASURE_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 编辑计量单位
        /// </summary>
        /// <param name="measureNum"></param>
        /// <param name="measureName"></param>
        /// <returns></returns>
        public int EditMeasure(string measureNum, string measureName)
        {
            MeasureEntity entity = new MeasureEntity();
            entity.MeasureName = measureName;
            entity.IncludeMeasureName(true);
            entity.Where(a => a.MeasureNum == measureNum);
            int line = this.Measure.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_MEASURE_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 根据计量单位编号获得计量单位
        /// </summary>
        /// <param name="measureNum"></param>
        /// <returns></returns>
        public MeasureEntity GetMeasure(string measureNum)
        {
            List<MeasureEntity> listResult = GetList();
            if (!listResult.IsNullOrEmpty())
            {
                return listResult.FirstOrDefault(a => a.MeasureNum == measureNum);
            }
            return null;
        }

        /// <summary>
        /// 根据名称查询单位信息
        /// </summary>
        /// <param name="measureName"></param>
        /// <returns></returns>
        public MeasureEntity GetMeasureByName(string measureName)
        {
            List<MeasureEntity> listResult = GetList();
            if (!listResult.IsNullOrEmpty())
            {
                return listResult.FirstOrDefault(a => a.MeasureName == measureName);
            }
            return null;
        }

        /// <summary>
        /// 查询分页计量单位
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<MeasureEntity> GetList(MeasureEntity entity, ref PageInfo pageInfo)
        {
            List<MeasureEntity> listResult = GetList();
            if (!listResult.IsNullOrEmpty())
            {
                int rowCount = 0;
                rowCount = listResult.Where(a=>a.MeasureName.Contains(entity.MeasureName)).Count();
                pageInfo.RowCount = rowCount;
                return listResult.Where(a => a.MeasureName.Contains(entity.MeasureName)).Skip((pageInfo.PageIndex - 1) * pageInfo.PageSize).Take(pageInfo.PageSize).ToList();
            }
            return null;
        }


    }
}
