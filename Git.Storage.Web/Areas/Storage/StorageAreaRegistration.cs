using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Storage
{
    public class StorageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Storage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Storage_default",
                "Storage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
