using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using Git.Storage.Rose.Server;
using Git.Framework.Resource;

namespace Git.Storage.Rose.Server
{
    public class Global : HttpApplication
    {
        public static Hprose.Server.HproseHttpService ExamService = new Hprose.Server.HproseHttpService();

        void Application_Start(object sender, EventArgs e)
        {
            //加载缓存修改文
            ResourceManager.LoadCache();
        }

        void Application_End(object sender, EventArgs e)
        {
            
        }

        void Application_Error(object sender, EventArgs e)
        {
        }
    }
}
