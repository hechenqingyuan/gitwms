using Git.Framework.Log;
using Git.Storage.Entity.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Entity.Store;
using Git.Framework.DataTypes;
using Git.Framework.Json;
using Git.Storage.Common;
using System.Net.Http;
using Git.Framework.Resource;
using System.Transactions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Git.Storage.Provider.Order
{
    public partial class OrderProvider : DataFactory
    {
        private readonly Log log = Log.Instance(typeof(OrderProvider));

        public OrderProvider() { }

        /// <summary>
        /// 根据订单详细唯一编码号获取该订单详细信息以及产品信息
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public OrderDetailEntity GetOrderDetailBySnNum(string snNum)
        {
            OrderDetailEntity entity = new OrderDetailEntity();
            entity.IncludeAll();
            ProductEntity ProEntity = new ProductEntity();
            ProEntity.Include(a => new { Size = a.Size});
            entity.Left<ProductEntity>(ProEntity, new Params<string, string>() { Item1 = "ProductNum", Item2 = "SnNum" });
            entity.Where("SnNum", ECondition.Eth, snNum);
            entity = this.OrderDetail.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 查询订单详细
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OrderDetailEntity GetOrderDetail(OrderDetailEntity entity)
        {
            entity.IncludeAll();
            entity = this.OrderDetail.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 根据订单号查询所有订单相关联的的排产单号
        /// </summary>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public List<string> GetOrderPlan(string orderNum)
        {
            OrdersEntity entity = new OrdersEntity();
            entity.Include(a => new { a.OrderNum, a.ContractOrder });
            entity.Where("OrderNum", ECondition.Like, "%" + orderNum + "%");
            List<OrdersEntity> list = this.Orders.GetList(entity);
            if (!list.IsNullOrEmpty())
            {
                List<string> items = list.Select(a => a.ContractOrder).ToList();
                return items;
            }
            return null;
        }
    }
}
