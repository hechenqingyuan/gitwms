using Git.Framework.DataTypes;
using Git.Framework.Log;
using Git.Framework.ORM;
using Git.Storage.Entity.Base;
using Git.Storage.Entity.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Storage.Provider.Store
{
    public partial class InventoryProvider : DataFactory
    {
        private readonly Log log = Log.Instance(typeof(InventoryProvider));

        public InventoryProvider() { }

        /// <summary>
        /// 台帐报表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<InventoryBookEntity> GetList(InventoryBookEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.OrderBy(a => a.ProductNum, EOrderBy.ASC);
            AdminEntity adminEntity = new AdminEntity();
            adminEntity.Include(a => new { UserName = a.UserName });
            entity.Left<AdminEntity>(adminEntity, new Params<string, string>() { Item1 = "CreateUser", Item2 = "UserCode" });
            int rowCount = 0;
            List<InventoryBookEntity> listResult = this.InventoryBook.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

    }
}
