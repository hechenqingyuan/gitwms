using Git.Framework.Email;
using Git.Framework.Resource;
using Git.Storage.Common;
using Git.Storage.Common.EnumJson;
using Git.Storage.Provider.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Git.Storage.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);



            //加载缓存修改文
            ResourceManager.LoadCache();
            //注册枚举值
            EnumToJsonHelper.Reg(typeof(EAudite),
                typeof(EBadType),
                typeof(ECusType),
                typeof(EInType),
                typeof(EEquipmentStatus),
                typeof(EIsDelete),
                typeof(ELocalType),
                typeof(EMoveType),
                typeof(EOutType),
                typeof(EProductType),
                typeof(EReturnStatus),
                typeof(EStorageType),
                typeof(EOpType),
                typeof(ECheckType),
                typeof(EReturnType),
                typeof(EChange),
                typeof(EOrderStatus),
                typeof(EOrderType),
                typeof(ESupType),
                typeof(EDataSourceType),
                typeof(EReportType),
                typeof(EElementType),
                typeof(ESequence),
                typeof(EBool));

            SetVersion();

            //初始化规则
            SequenceProvider sequenceProvider = new SequenceProvider();
            sequenceProvider.Init();
        }


        /// <summary>
        /// 得到系统编译的版本
        /// </summary>
        private void SetVersion()
        {
            System.Reflection.AssemblyName assName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            string version = "V" + assName.Version.ToString();
            version = version.Substring(0, version.LastIndexOf("."));
            Git.Framework.Cache.CacheHelper.Add(Git.Storage.Provider.CacheKey.CACHE_SYS_VERSION,version);
        }
    }
}