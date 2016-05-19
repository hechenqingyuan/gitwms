using Git.Framework.Controller;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Storage.Web.Lib;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.Json;
using Git.Storage.Provider;
using Git.Storage.Common;
using System.Text;
using Git.Framework.Controller.Mvc;
using Git.Storage.Common.Excel;
using System.Data;
using Storage.Common;
using Git.Storage.Web.Lib.Filter;
using Git.Storage.Entity.Order;
using Git.Storage.Provider.Order;
using Git.Framework.Cache;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.Product.Controllers
{
    public class GoodsAjaxController : AjaxPage
    {
        /// <summary>
        /// 查询产品类别分页
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult CateList()
        {
            string cateName = WebUtil.GetFormValue<string>("cateName", string.Empty);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            ProductCategoryProvider provider = new ProductCategoryProvider();
            ProductCategoryEntity entity = new ProductCategoryEntity();
            if (!cateName.IsEmpty())
            {
                entity.Begin<ProductCategoryEntity>()
                    .Where<ProductCategoryEntity>("CateNum", ECondition.Like, "%" + cateName + "%")
                    .Or<ProductCategoryEntity>("CateName", ECondition.Like, "%" + cateName + "%")
                    .End<ProductCategoryEntity>()
                    ;
            }
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            List<ProductCategoryEntity> list = provider.GetList(entity, ref pageInfo);
            list = list.IsNull() ? new List<ProductCategoryEntity>() : list;
            string json = ConvertJson.ListToJson<ProductCategoryEntity>(list, "data");
            JsonObject jsonObject = new JsonObject(json);
            this.ReturnJson.AddProperty("list", jsonObject);
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除产品类别
        /// </summary>
        /// <param name="cateNum"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DelCate(string cateNum)
        {
            if (!cateNum.IsEmpty())
            {
                ProductCategoryProvider provider = new ProductCategoryProvider();
                int line = provider.Delete(cateNum);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除产品类型
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DelBat([ModelBinder(typeof(JsonBinder<List<string>>))] List<string> List)
        {
            if (!List.IsNullOrEmpty())
            {
                ProductCategoryProvider provider = new ProductCategoryProvider();
                int line = provider.DelBat(List);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 编辑产品类别
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Edit()
        {
            string cateNum = WebUtil.GetFormValue<string>("num", string.Empty);
            string cateName = WebUtil.GetFormValue<string>("name", string.Empty);
            string remark = WebUtil.GetFormValue<string>("remark", string.Empty);
            if (cateNum.IsEmpty())
            {
                ProductCategoryEntity entity = new ProductCategoryEntity();
                entity.CateNum = SequenceProvider.GetSequence(typeof(ProductCategoryEntity));
                entity.CateName = cateName;
                entity.IsDelete = (int)EIsDelete.NotDelete;
                entity.CreateTime = DateTime.Now;
                entity.Remark = remark;
                entity.CreateUser = this.LoginUserCode;
                ProductCategoryProvider provider = new ProductCategoryProvider();
                int line = provider.Add(entity);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            else
            {
                ProductCategoryEntity entity = new ProductCategoryEntity();
                entity.CateNum = cateNum;
                entity.CateName = cateName;
                entity.Remark = remark;
                ProductCategoryProvider provider = new ProductCategoryProvider();
                int line = provider.Update(entity);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            return Content(this.ReturnJson.ToString());
        }


        /***************************************产品管理**********************************************/
        /// <summary>
        /// 查询产品类别分页
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetList()
        {
            string ProductName = WebUtil.GetFormValue<string>("ProductName", string.Empty);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string CateNum = WebUtil.GetFormValue<string>("CateNum", string.Empty);
            ProductProvider provider = new ProductProvider();
            ProductEntity entity = new ProductEntity();
            if (!ProductName.IsEmpty())
            {
                entity.Begin<ProductEntity>()
                 .Where<ProductEntity>("ProductName", ECondition.Like, "%" + ProductName + "%")
                 .Or<ProductEntity>("SnNum", ECondition.Like, "%" + ProductName + "%")
                 .Or<ProductEntity>("BarCode", ECondition.Like, "%" + ProductName + "%")
                 .End<ProductEntity>();
            }
            if (!CateNum.IsEmpty())
            {
                entity.Where<ProductEntity>("CateNum", ECondition.Eth, CateNum);
            }
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            List<ProductEntity> listResult = provider.GetList(entity, ref pageInfo);
            string json = ConvertJson.ListToJson<ProductEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ToExcel()
        {
            PageInfo pageInfo = new Git.Framework.DataTypes.PageInfo() { PageIndex = 1, PageSize = Int32.MaxValue };
            string ProductName = WebUtil.GetFormValue<string>("ProductName", string.Empty);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string CateNum = WebUtil.GetFormValue<string>("CateNum", string.Empty);
            ProductProvider provider = new ProductProvider();
            ProductEntity entity = new ProductEntity();
            if (!ProductName.IsEmpty())
            {
                entity.Where<ProductEntity>("ProductName", ECondition.Like, "%" + ProductName + "%");
            }
            if (!CateNum.IsEmpty())
            {
                entity.Where<ProductEntity>("CateNum", ECondition.Eth, CateNum);
            }
            List<ProductEntity> listResult = provider.GetList(entity, ref pageInfo);

            listResult = listResult.IsNull() ? new List<ProductEntity>() : listResult;
            if (listResult.IsNotNull())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("序号 "));
                dt.Columns.Add(new DataColumn("产品编号"));
                dt.Columns.Add(new DataColumn("条码编号"));
                dt.Columns.Add(new DataColumn("产品名称"));
                dt.Columns.Add(new DataColumn("类别名称"));
                dt.Columns.Add(new DataColumn("库存数"));
                dt.Columns.Add(new DataColumn("预警值下线"));
                dt.Columns.Add(new DataColumn("预警值上线"));
                dt.Columns.Add(new DataColumn("单位"));
                dt.Columns.Add(new DataColumn("平均价格"));
                dt.Columns.Add(new DataColumn("进口价格"));
                dt.Columns.Add(new DataColumn("出口价格"));
                dt.Columns.Add(new DataColumn("净重"));
                dt.Columns.Add(new DataColumn("毛重"));
                dt.Columns.Add(new DataColumn("备注"));
                int count = 1;
                foreach (ProductEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = count;
                    row[1] = t.SnNum;
                    row[2] = t.BarCode;
                    row[3] = t.ProductName;
                    row[4] = t.CateName;
                    row[5] = t.Num;
                    row[6] = t.MinNum;
                    row[7] = t.MaxNum;
                    row[8] = t.UnitName;
                    row[9] = t.AvgPrice;
                    row[10] = t.InPrice;
                    row[11] = t.OutPrice;
                    row[12] = t.NetWeight.ToString();
                    row[13] = t.GrossWeight.ToString();
                    row[14] = t.Remark;
                    dt.Rows.Add(row);
                    count++;
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("产品管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("产品管理", "产品", System.IO.Path.Combine(filePath, filename));
                excel.ToExcel(dt);
                this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());
            }
            else
            {
                this.ReturnJson.AddProperty("d", "无数据导出!");
            }
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 获得库位
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetLocal()
        {
            string StorageNum = WebUtil.GetFormValue<string>("StorageNum", string.Empty);
            List<LocationEntity> listResult = new List<LocationEntity>();
            LocationProvider provider = new LocationProvider();
            if (!StorageNum.IsEmpty())
            {
                listResult = provider.GetList(StorageNum);
            }
            string json = ConvertJson.ListToJson<LocationEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", listResult.Count);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 新增和修改产品信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult EditProduct([ModelBinder(typeof(JsonBinder<ProductEntity>))] ProductEntity entity)
        {
            if (entity.SnNum.IsEmpty())
            {
                entity.SnNum = SequenceProvider.GetSequence(typeof(ProductEntity));
                if (entity.CusNum.IsEmpty())
                {
                    entity.CusName = "";
                }
                entity.Num = 0;
                entity.IsDelete = (int)EIsDelete.NotDelete;
                entity.CreateTime = DateTime.Now;
                entity.CreateUser = this.LoginUserCode;
                int line = new ProductProvider().Add(entity);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            else
            {
                if (entity.CusNum.IsEmpty())
                {
                    entity.CusName = "";
                }
                entity.Num = 0;
                int line = new ProductProvider().Update(entity);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 自动加载产品
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AutoProduct(string productName)
        {
            ProductProvider provider = new ProductProvider();
            List<ProductEntity> list = provider.GetListByCache();
            if (!list.IsNullOrEmpty() && !productName.IsEmpty())
            {
                list = list.Where(a => a.ProductName.Contains(productName) || a.BarCode.Contains(productName) || a.SnNum.Contains(productName)).ToList();
            }
            list = list.IsNull() ? new List<ProductEntity>() : list;
            StringBuilder sb = new StringBuilder();
            JsonObject jsonObject = null;
            foreach (ProductEntity t in list)
            {
                jsonObject = new JsonObject();
                jsonObject.AddProperty("BarCode", t.BarCode);
                jsonObject.AddProperty("ProductName", t.ProductName);
                jsonObject.AddProperty("SnNum", t.SnNum);
                jsonObject.AddProperty("CateNum", t.CateNum);
                jsonObject.AddProperty("CateName", t.CateName);
                jsonObject.AddProperty("InPrice", t.InPrice);
                jsonObject.AddProperty("Unit", t.UnitNum);
                jsonObject.AddProperty("UnitName", t.UnitName);
                jsonObject.AddProperty("Size", t.Size);
                jsonObject.AddProperty("Num", t.Num);
                sb.Append(jsonObject.ToString() + "\n");
            }
            if (sb.Length == 0)
            {
                sb.Append("\n");
            }
            return Content(sb.ToString());
        }


        [LoginAjaxFilter]
        public ActionResult AutoSinProduct(string ProductNum)
        {
            ProductProvider provider = new ProductProvider();
            List<ProductEntity> list = provider.GetListByCache();
            ProductEntity entity = null;
            if (!list.IsNullOrEmpty() && !ProductNum.IsEmpty())
            {
                entity = list.FirstOrDefault(a => a.SnNum == ProductNum);
            }
            entity = entity.IsNull() ? new ProductEntity() : entity;
            List<ProductEntity> listSource = new List<ProductEntity>();
            listSource.Add(entity);
            StringBuilder sb = new StringBuilder();
            JsonObject jsonObject = null;
            foreach (ProductEntity t in listSource)
            {
                jsonObject = new JsonObject();
                jsonObject.AddProperty("BarCode", t.BarCode);
                jsonObject.AddProperty("ProductName", t.ProductName);
                jsonObject.AddProperty("SnNum", t.SnNum);
                jsonObject.AddProperty("CateNum", t.CateNum);
                jsonObject.AddProperty("CateName", t.CateName);
                jsonObject.AddProperty("InPrice", t.InPrice);
                jsonObject.AddProperty("Unit", t.UnitNum);
                jsonObject.AddProperty("UnitName", t.UnitName);
                jsonObject.AddProperty("Size", t.Size.Escape());
                jsonObject.AddProperty("Num", t.Num);
                sb.Append(jsonObject.ToString() + "\n");
            }
            if (sb.Length == 0)
            {
                sb.Append("\n");
            }
            return Content(sb.ToString());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete(string SnNum)
        {
            ProductProvider provider = new ProductProvider();
            int line = provider.Delete(SnNum);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("d", "success");
            }
            else
            {
                this.ReturnJson.AddProperty("d", "");
            }
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult BatchDel(string SnNum)
        {
            ProductProvider provider = new ProductProvider();
            var list = SnNum.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> ls = new List<string>();
            int line = 0;
            foreach (string t in list)
            {
                line += provider.Delete(t);
            }
            if (line > 0)
            {
                this.ReturnJson.AddProperty("d", "success");
            }
            else
            {
                this.ReturnJson.AddProperty("d", "");
            }
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 从文件获取产品分类信息
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="list"></param>
        [LoginAjaxFilter]
        private static void GetProCatInfo(System.Data.DataSet dataset, List<ProductEntity> list)
        {

            var rows = dataset.Tables[0].Rows;
            if (rows.Count > 2)
            {
                ProductCategoryProvider ProductCategory = new ProductCategoryProvider();
                for (var i = 2; i < rows.Count; i++)
                {
                    ProductEntity entity = new ProductEntity();
                    ProductCategoryEntity PCEntity = new ProductCategoryEntity();
                    var row = rows[i];
                    entity.SnNum = row[1].ToString();
                    if (string.IsNullOrEmpty(entity.SnNum)) continue;
                    entity.BarCode = row[2].ToString();
                    entity.ProductName = row[3].ToString();
                    entity.CateName = row[4].ToString();
                    PCEntity = ProductCategory.GetSingleCateName(entity.CateName);
                    if (PCEntity.IsNotNull())
                    {
                        entity.CateNum = PCEntity.CateNum;
                    }
                    else
                    {
                        entity.CateNum = "";
                    }
                    entity.Num = ConvertHelper.ToType<int>(row[5].ToString());
                    entity.MinNum = ConvertHelper.ToType<int>(row[6].ToString());
                    entity.MaxNum = ConvertHelper.ToType<int>(row[7].ToString());
                    entity.UnitName = row[8].ToString();
                    entity.UnitNum = entity.UnitNum;
                    entity.AvgPrice = ConvertHelper.ToType<double>(row[9].ToString());
                    entity.InPrice = ConvertHelper.ToType<double>(row[10].ToString());
                    entity.OutPrice = ConvertHelper.ToType<double>(row[11].ToString());
                    entity.NetWeight = ConvertHelper.ToType<double>(row[12].ToString());
                    entity.GrossWeight = ConvertHelper.ToType<double>(row[13].ToString());
                    entity.Remark = row[14].ToString();

                    entity.IsDelete = (int)EIsDelete.NotDelete;

                    entity.CreateTime = DateTime.Now;
                    list.Add(entity);
                }

            }
        }

    }
}
