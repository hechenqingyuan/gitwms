using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Framework.Log;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.DataTypes;
using Git.Storage.Entity.Store;
using Git.Storage.Entity.InStorage;
using Git.Storage.Common;
using Git.Framework.Cache;

namespace Git.Storage.Provider.Store
{
    public partial class LocalProductProvider : DataFactory
    {
        private readonly Log log = Log.Instance(typeof(LocalProductProvider));

        public LocalProductProvider() { }

        /// <summary>
        /// 根据产品条码或者产品编号或者产品名称搜索库存产品数量以及库存位置
        /// 主要用于选择产品的界面中搜索功能。在搜索到相应产品之后查询库存信息
        /// </summary>
        /// <returns></returns>
        public List<LocalProductEntity> GetList(string barCode)
        {
            LocalProductEntity entity = new LocalProductEntity();
            entity.IncludeAll();
            ProductEntity ProEntity = new ProductEntity();
            ProEntity.Include(a => new { Size = a.Size, InPrice = a.InPrice });
            entity.Left<ProductEntity>(ProEntity, new Params<string, string>() { Item1 = "ProductNum", Item2 = "SnNum" });
            entity.Where(a => a.BarCode == barCode);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            List<LocalProductEntity> listResult = this.LocalProduct.GetList(entity);
            return listResult;
        }

        /// <summary>
        /// 根据库存唯一编码号获取该位置的库存信息以及产品信息
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public LocalProductEntity GetLocalProductBySn(string sn, string productNum = null)
        {
            LocalProductEntity entity = new LocalProductEntity();
            entity.IncludeAll();
            ProductEntity ProEntity = new ProductEntity();
            ProEntity.Include(a => new { Size = a.Size, InPrice = a.InPrice });
            entity.Left<ProductEntity>(ProEntity, new Params<string, string>() { Item1 = "ProductNum", Item2 = "SnNum" });
            entity.Where(a => a.Sn == sn);
            if (!productNum.IsEmpty())
            {
                entity.Where(a => a.ProductNum == productNum);
            }
            entity = this.LocalProduct.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 在线库存报表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<LocalProductEntity> GetList(LocalProductEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            ProductEntity ProEntity = new ProductEntity();
            ProEntity.Include(a => new { Size = a.Size, AvgPrice = a.AvgPrice,CateNum=a.CateNum, CateName = a.CateName, MinNum = a.MinNum, MaxNum = a.MaxNum });
            entity.Left<ProductEntity>(ProEntity, new Params<string, string>() { Item1 = "ProductNum", Item2 = "SnNum" });
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<LocalProductEntity> listResult = this.LocalProduct.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;

            List<ProductCategoryEntity> listCates = new ProductCategoryProvider().GetList();
            listCates = listCates.IsNull() ? new List<ProductCategoryEntity>() : listCates;
            if (!listResult.IsNullOrEmpty())
            {
                foreach (LocalProductEntity item in listResult)
                {
                    if (item.CateName.IsEmpty())
                    {
                        ProductCategoryEntity cate = listCates.FirstOrDefault(a => a.CateNum == item.CateNum);
                        item.CateName = cate == null ? "" : cate.CateName;
                    }
                }
            }
            return listResult;
        }

        /// <summary>
        /// 获得满足条件的所有产品的库存
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="localType"></param>
        /// <param name="searchKey"></param>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public int GetAllNum(string localName, string localType, string searchKey, string storageNum)
        {
            LocalProductEntity entity = new LocalProductEntity();
            if (!storageNum.IsEmpty())
            {
                entity.Where("StorageNum", ECondition.Eth, storageNum);
            }
            if (!localType.IsEmpty())
            {
                entity.Where("LocalType", ECondition.Eth, localType);
            }
            if (!localName.IsEmpty())
            {
                entity.Where("LocalName", ECondition.Like, "%" + localName + "%");
                entity.Or("LocalNum", ECondition.Like, "%" + localName + "%");
            }
            if (!searchKey.IsEmpty())
            {
                entity.Begin<LocalProductEntity>()
                 .Where<LocalProductEntity>("ProductName", ECondition.Like, "%" + searchKey + "%")
                 .Or<LocalProductEntity>("ProductNum", ECondition.Like, "%" + searchKey + "%")
                 .Or<LocalProductEntity>("BarCode", ECondition.Like, "%" + searchKey + "%")
                 .End<LocalProductEntity>();
            }
            entity.IncludeNum(true);
            int allNum = 0;
            try
            {
                allNum = this.LocalProduct.Sum<int>(entity);
            }
            catch (Exception e)
            {
                allNum = 0;
                log.Info(e.Message);
            }
            return allNum;
        }

        /// <summary>
        /// 获得满足条件的所有产品的总价格
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="localType"></param>
        /// <param name="searchKey"></param>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public double GetAllTotalPrice(string localName, string localType, string searchKey, string storageNum)
        {
            LocalProductEntity entity = new LocalProductEntity();
            double allTotalPrice = 0;
            if (!storageNum.IsEmpty())
            {
                entity.Where("StorageNum", ECondition.Eth, storageNum);
            }
            if (!localType.IsEmpty())
            {
                entity.Where("LocalType", ECondition.Eth, localType);
            }
            if (!localName.IsEmpty())
            {
                entity.Where("LocalName", ECondition.Like, "%" + localName + "%");
                entity.Or("LocalNum", ECondition.Like, "%" + localName + "%");
            }

            if (!searchKey.IsEmpty())
            {
                entity.Begin<LocalProductEntity>()
                 .Where<LocalProductEntity>("ProductName", ECondition.Like, "%" + searchKey + "%")
                 .Or<LocalProductEntity>("ProductNum", ECondition.Like, "%" + searchKey + "%")
                 .Or<LocalProductEntity>("BarCode", ECondition.Like, "%" + searchKey + "%")
                 .End<LocalProductEntity>();
            }
            entity.IncludeNum(true);

            ProductEntity ProEntity = new ProductEntity();
            ProEntity.Include(a => new { AvgPrice = a.AvgPrice });
            entity.Left<ProductEntity>(ProEntity, new Params<string, string>() { Item1 = "ProductNum", Item2 = "SnNum" });
            entity.OrderBy(a => a.ID, EOrderBy.DESC);

            List<LocalProductEntity> listResult = this.LocalProduct.GetList(entity);
            if (!listResult.IsNullOrEmpty())
            {
                listResult.ForEach(a =>
                {
                    a.TotalPrice = a.Num * a.AvgPrice;
                    allTotalPrice += a.TotalPrice;
                });
            }
            return allTotalPrice;
        }

        /// <summary>
        /// 获得产品的当前库存
        /// </summary>
        /// <param name="storageNum"></param>
        /// <param name="productNum"></param>
        /// <returns></returns>
        public double GetLocalNum(string storageNum, string productNum)
        {
            LocalProductEntity entity = new LocalProductEntity();
            entity.IncludeNum(true);
            entity.Where(a => a.StorageNum == storageNum).And(a => a.ProductNum == productNum)
                .And(a => a.LocalType == (int)ELocalType.Normal);
            try
            {
                double sum = this.LocalProduct.Sum<double>(entity);
                return sum;
            }
            catch (Exception e)
            {
                log.Info(e.Message);
            }
            return 0;
        }

        /// <summary>
        /// 获得产品的当前库存信息
        /// </summary>
        /// <param name="localNum"></param>
        /// <param name="productNum"></param>
        /// <returns></returns>
        public LocalProductEntity GetSingle(string localNum, string productNum)
        {
            LocalProductEntity entity = new LocalProductEntity();
            entity.IncludeAll();
            entity.Where(a => a.LocalNum == localNum).And(a => a.ProductNum == productNum)
                .And(a => a.LocalType == (int)ELocalType.Normal);
            entity = this.LocalProduct.GetSingle(entity);
            return entity;
        }

    }
}
