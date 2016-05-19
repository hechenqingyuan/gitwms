using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Provider;
using Git.Storage.Entity.Base;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Lib.Filter
{
    public class LoginAjaxFilter : BaseAuthorizeAttribute
    {

        private bool ValidateLogin = true;

        private bool ValidateRequest = true;

        public LoginAjaxFilter()
            : base()
        {

        }

        public LoginAjaxFilter(bool _validateLogin)
            : base()
        {
            this.ValidateLogin = _validateLogin;
        }

        public LoginAjaxFilter(bool _validateLogin, bool _validateRequest)
            : base()
        {
            this.ValidateLogin = _validateLogin;
            this.ValidateRequest = _validateRequest;
        }

        /// <summary>
        /// 1001:用户未登录
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //if (this.ValidateLogin)
            //{
            //    AdminEntity LoginUser = filterContext.HttpContext.Session[CacheKey.SESSION_LOGIN_ADMIN] as AdminEntity;
            //    string path = filterContext.HttpContext.Request.Path;
            //    if (LoginUser.IsNull())
            //    {
            //        filterContext.Result = new ContentResult() { Content = "90001" };
            //    }
            //    else
            //    {
            //        if (ValidateRequest && path != "/")
            //        {
            //            PowerProvider provider = new PowerProvider();
            //            bool hasPower = provider.HasPower(path, LoginUser.RoleNum);
            //            if (!hasPower)
            //            {
            //                filterContext.Result = new ContentResult() { Content = "90002" };
            //            }
            //        }
            //    }
            //}
        }
    }
}