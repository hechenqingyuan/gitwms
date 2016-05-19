using Git.Storage.Common;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Git.Storage.Web.Lib
{
    public class LocalHelper
    {
        /// <summary>
        /// 仓库
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        public static string GetStorageNumList(string storeNum)
        {
            StorageProvider provider = new StorageProvider();
            List<StorageEntity> list = provider.GetList();
            StringBuilder sb = new StringBuilder();
            string storeTemplate = "<option value='{0}' {1}>{2}</option>";
            sb.AppendFormat(storeTemplate, "", "", "请选择仓库");
            if (!list.IsNullOrEmpty())
            {
                foreach (StorageEntity store in list)
                {
                    sb.AppendFormat(storeTemplate, store.StorageNum, store.StorageNum == storeNum ? "selected='selected'" : string.Empty, store.StorageName);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 库位
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        public static string GetLocalNumList(string storeNum, string localNum)
        {
            LocationProvider provider = new LocationProvider();
            List<LocationEntity> list = provider.GetList();
            list = list.Where(a => a.StorageNum == storeNum).ToList();
            StringBuilder sb = new StringBuilder();
            string storeTemplate = "<option value='{0}' {1}>{2}</option>";
            if (!list.IsNullOrEmpty())
            {
                foreach (LocationEntity store in list)
                {
                    sb.AppendFormat(storeTemplate, store.LocalNum, store.LocalNum == localNum ? "selected='selected'" : string.Empty, store.LocalName);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获得某个仓库中某种类型的仓库列表
        /// </summary>
        /// <param name="storeNum">仓库编号</param>
        /// <param name="localType">库位类型</param>
        /// <param name="localNum">选中库位编号</param>
        /// <returns></returns>
        public static string GetLocalNum(string storeNum, ELocalType localType, string localNum)
        {
            LocationProvider provider = new LocationProvider();
            List<LocationEntity> list = provider.GetList();
            StringBuilder sb = new StringBuilder();
            if (!list.IsNullOrEmpty())
            {
                string storeTemplate = "<option value='{0}' {1}>{2}</option>";
                sb.AppendFormat(storeTemplate,"","","请选择库位");
                foreach (LocationEntity entity in list.Where(a=>a.StorageNum==storeNum && a.LocalType==(int)localType))
                {
                    sb.AppendFormat(storeTemplate, entity.LocalNum, entity.LocalNum == localNum ? "selected='selected'" : string.Empty, entity.LocalName);
                }
            }
            return sb.ToString();
        }

    }
}