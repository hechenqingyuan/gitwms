using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Json;
using Git.Storage.Entity.Base;
using Git.Storage.Provider;
using Git.Storage.Provider.Base;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Storage.Controllers
{
    public class MeasureAjaxController : AjaxPage
    {
        /// <summary>
        /// 获得计量单位列表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetMeasure()
        {
            string name = WebUtil.GetFormValue<string>("name",string.Empty);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex",1);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 10);
            PageInfo pageInfo = new PageInfo() { PageIndex=1,PageSize=pageSize };
            MeasureProvider provider = new MeasureProvider();
            MeasureEntity entity = new MeasureEntity();
            entity.MeasureName = name;
            List<MeasureEntity> listResult = provider.GetList(entity, ref pageInfo);
            listResult = listResult.IsNull() ? new List<MeasureEntity>() : listResult;
            string json = ConvertJson.ListToJson<MeasureEntity>(listResult);
            this.ReturnJson.AddProperty("List",json);
            this.ReturnJson.AddProperty("RowCount",pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 新建或者编辑
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Create(string Num, string Name)
        {
            if (Num.IsEmpty())
            {
                MeasureEntity entity = new MeasureEntity();
                entity.MeasureNum = SequenceProvider.GetSequence(typeof(MeasureEntity));
                entity.MeasureName = Name;
                entity.SN = SequenceProvider.GetSequence(typeof(MeasureEntity));
                MeasureProvider provider = new MeasureProvider();
                int line = provider.AddMeasure(entity);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("Key", "1000");
                    this.ReturnJson.AddProperty("Value", "添加成功");
                }
                else
                {
                    this.ReturnJson.AddProperty("Key", "1001");
                    this.ReturnJson.AddProperty("Value", "添加失败");
                }
            }
            else
            {
                MeasureProvider provider = new MeasureProvider();
                int line = provider.EditMeasure(Num,Name);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("Key", "1000");
                    this.ReturnJson.AddProperty("Value", "编辑成功");
                }
                else
                {
                    this.ReturnJson.AddProperty("Key", "1001");
                    this.ReturnJson.AddProperty("Value", "编辑失败");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete(string Num)
        {
            MeasureProvider provider = new MeasureProvider();
            int line = provider.DeleteMeasure(Num);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "删除成功");
            }
            else
            {
                this.ReturnJson.AddProperty("Key", "1001");
                this.ReturnJson.AddProperty("Value", "删除失败");
            }
            return Content(this.ReturnJson.ToString());
        }
    }
}
