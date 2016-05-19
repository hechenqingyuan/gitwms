using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Provider;
using Git.Storage.Provider.Base;
using Git.Storage.Entity.Base;

namespace Git.Storage.Web.Lib.Filter
{
    public class LoginFilter : BaseAuthorizeAttribute
    {
        private bool ValidateLogin = true;

        private bool ValidateRequest = true;

        public LoginFilter()
            : base()
        {

        }

        public LoginFilter(bool _validateLogin)
            : base()
        {
            this.ValidateLogin = _validateLogin;
        }

        public LoginFilter(bool _validateLogin,bool _validateRequest)
            : base()
        {
            this.ValidateLogin = _validateLogin;
            this.ValidateRequest = _validateRequest;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //AdminEntity LoginUser = filterContext.HttpContext.Session[CacheKey.SESSION_LOGIN_ADMIN] as AdminEntity;
            ////如果是未登陆则跳转到登陆页面
            //if (LoginUser == null)
            //{
            //    string path = GetPath(filterContext);
            //    string url = "/Home/Index";
            //    if (!path.IsEmpty())
            //    {
            //        path = filterContext.HttpContext.Server.UrlEncode(path);
            //        url = "/Home/Index?returnurl=" + path;
            //    }
            //    filterContext.Result = new RedirectResult(url);
            //}

            if (this.ValidateLogin)
            {
                AdminEntity LoginUser = filterContext.HttpContext.Session[CacheKey.SESSION_LOGIN_ADMIN] as AdminEntity;
                string path = filterContext.HttpContext.Request.Path;
                if (LoginUser.IsNull())
                {
                    string url = "/Home/Index";
                    if (!path.IsEmpty())
                    {
                        path = filterContext.HttpContext.Server.UrlEncode(path);
                        url = "/Home/Index?returnurl=" + path;
                    }
                    filterContext.Result = new RedirectResult(url);
                }
                else
                {
                    if (ValidateRequest && path != "/")
                    {
                        PowerProvider provider = new PowerProvider();
                        bool hasPower = provider.HasPower(path,LoginUser.RoleNum);
                        if (!hasPower)
                        {
                            string url = "/Home/Error";
                            filterContext.Result = new RedirectResult(url);
                        }
                    }
                }
            }
        }
    }
}