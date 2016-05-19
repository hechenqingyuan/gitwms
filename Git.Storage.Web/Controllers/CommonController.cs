using Git.Framework.Controller;
using Git.Storage.Common.EnumJson;
using Git.Storage.Provider;
using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Controllers
{
    public class CommonController : Controller
    {
        /// <summary>
        /// 输出枚举转化JS
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 30 * 1000)]
        public ActionResult Js()
        {
            string js = EnumToJsonHelper.GetJs();
            js += EnumToJsonHelper.GetMenuObject();
            return Content(js);
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            bool success = false;
            if (Request.Files.Count > 0)
            {
                string ext = System.IO.Path.GetExtension(Request.Files[0].FileName).ToLower();
                if (Request.Files[0].ContentLength <= 5 * 1024 * 1024)
                {
                    success = true;
                    string fileName = Guid.NewGuid().ToString("N") + ext;
                    string dir = Server.MapPath("~/UploadFile/");
                    if (!System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }
                    Request.Files[0].SaveAs(System.IO.Path.Combine(dir, fileName));
                    return Content("{ success: true, fileUrl:'/UploadFile/" + fileName + "' }");
                }
            }
            if (!success)
            {
                return Content("{ success: false, fileUrl:'' }");
            }
            return Content(string.Empty);
        }

        /// <summary>
        /// 设置菜单显示的状态
        /// </summary>
        /// <returns></returns>
        public ActionResult SetMenuStatus()
        {
            string status = WebUtil.GetFormValue<string>("MenuStatus", "open");
            Session[CacheKey.SESSION_MENU_STATUS] = status;
            return Content(string.Empty);
        }


        /// <summary>
        /// 输出验证码
        /// </summary>
        public void Val()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            VerifyCode verify = new VerifyCode();
            string code = verify.CreateVerifyCode();
            SessionHelper.Add("ValCode",code,3);
            Session.Add("ValCode", code);
            Bitmap image = verify.CreateImageCode(code);
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            Response.ClearContent();
            Response.ContentType = "image/gif";
            Response.BinaryWrite(ms.GetBuffer());
            ms.Close();
            ms = null;
            image.Dispose();
            image = null;
        }
    }
}
