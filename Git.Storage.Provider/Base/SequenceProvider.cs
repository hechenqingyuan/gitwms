/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2015/10/8 13:02:25
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2015/10/8 13:02:25       情缘
*********************************************************************************/

using Git.Storage.Entity.Base;
using Git.Framework.ORM;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Storage.Common;

namespace Git.Storage.Provider.Base
{
    public partial class SequenceProvider:DataFactory
    {
        public SequenceProvider() { }

        /// <summary>
        /// 新增序号管理
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Init()
        {
            DataTable table = this.Sequence.GetTables();
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    string TabName = row["name"] != null ? row["name"].ToString() : string.Empty;
                    if (!TabName.IsEmpty())
                    {
                        SequenceEntity entity = new SequenceEntity();
                        entity.Where(a => a.TabName == TabName);
                        if (this.Sequence.GetCount(entity) == 0)
                        {
                            entity = new SequenceEntity();
                            entity.SN = TNumProivder.CreateGUID();
                            entity.TabName = TabName;
                            entity.FirstType = (int)ESequence.Sequence;
                            entity.FirstRule = "";
                            entity.FirstLength = 6;
                            entity.JoinChar = "";
                            entity.IncludeAll();
                            this.Sequence.Add(entity);
                        }
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 修改序号
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(SequenceEntity entity)
        {
            entity.Include(a => new { a.FirstType,a.FirstRule,a.FirstLength,a.SecondType,a.SecondRule,a.SecondLength,a.ThirdType,a.ThirdRule,a.ThirdLength,a.FourType,a.FourRule,a.FourLength,a.JoinChar,a.Sample,a.CurrentValue,a.Remark });
            entity.Where(a => a.SN == entity.SN);
            int line = this.Sequence.Update(entity);
            return line;
        }

        /// <summary>
        /// 查询序列分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<SequenceEntity> GetList(SequenceEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.OrderBy(a => a.ID, EOrderBy.ASC);
            int rowCount = 0;
            List<SequenceEntity> listResult = this.Sequence.GetList(entity,pageInfo.PageSize,pageInfo.PageIndex,out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 根据表名序列
        /// </summary>
        /// <param name="TabName"></param>
        /// <returns></returns>
        public SequenceEntity GetSingle(string TabName)
        {
            SequenceEntity entity = new SequenceEntity();
            entity.IncludeAll();
            entity.Where(a => a.TabName == TabName);
            entity = this.Sequence.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 根据SN号获得序列
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public SequenceEntity Get(string SN)
        {
            SequenceEntity entity = new SequenceEntity();
            entity.IncludeAll();
            entity.Where(a => a.SN == SN);
            entity = this.Sequence.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 获得表的序列号
        /// </summary>
        /// <returns></returns>
        public static string GetSequence(Type type)
        {
            Func<int, string, int, string> Func = (int SequenceType, string SequenceRule, int SequenceLength) =>
            {
                string value = string.Empty;
                if (SequenceType > 0)
                {
                    if (SequenceType == (int)ESequence.Constant)
                    {
                        value = SequenceRule;
                    }
                    else if (SequenceType == (int)ESequence.Guid)
                    {
                        value = TNumProivder.CreateGUID();
                    }
                    else if (SequenceType == (int)ESequence.CustomerTime)
                    {
                        value = DateTime.Now.ToString(SequenceRule);
                    }
                    else if (SequenceType == (int)ESequence.Sequence)
                    {
                        if (SequenceLength == 0)
                        {
                            value = new TNumProivder().GetSwiftNum(type);
                        }
                        else
                        {
                            value = new TNumProivder().GetSwiftNum(type, SequenceLength);
                        }
                    }
                    else if (SequenceType == (int)ESequence.SequenceOfDay)
                    {
                        if (SequenceLength == 0)
                        {
                            value = new TNumProivder().GetSwiftNumByDay(type);
                        }
                        else
                        {
                            value = new TNumProivder().GetSwiftNumByDay(type, SequenceLength);
                        }
                    }
                }
                return value;
            };

            TableInfo tableInfo = EntityTypeCache.Get(type);
            string TabName = tableInfo.Table.Name;
            SequenceProvider provider = new SequenceProvider();
            SequenceEntity entity = provider.GetSingle(TabName);

            string result = string.Empty;
            if (entity != null)
            {
                List<string> list = new List<string>();
                if (entity.FirstType > 0)
                {
                    list.Add(Func(entity.FirstType, entity.FirstRule, entity.FirstLength));
                }
                if (entity.SecondType > 0)
                {
                    list.Add(Func(entity.SecondType, entity.SecondRule, entity.SecondLength));
                }
                if (entity.ThirdType > 0)
                {
                    list.Add(Func(entity.ThirdType, entity.ThirdRule, entity.ThirdLength));
                }
                if (entity.FourType > 0)
                {
                    list.Add(Func(entity.FourType, entity.FourRule, entity.FourLength));
                }
                result = string.Join(entity.JoinChar, list.ToArray());
            }
            else
            {
                result = TNumProivder.CreateGUID();
            }
            return result;
        }


    }
}
