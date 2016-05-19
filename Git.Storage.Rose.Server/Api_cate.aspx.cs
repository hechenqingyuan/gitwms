using Git.Storage.Rose.Server.Api;
using Hprose.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Git.Storage.Rose.Server
{
    public partial class Api_cate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Api_CategoryProvider category = new Api_CategoryProvider();
            HproseHttpMethods methods = new HproseHttpMethods();
            methods.AddInstanceMethods(category, "category");
            Global.ExamService.Handle(methods);
        }
    }
}