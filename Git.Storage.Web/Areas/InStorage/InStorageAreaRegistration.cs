using System.Web.Mvc;

namespace Git.Storage.Web.Areas.InStorage
{
    public class InStorageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "InStorage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "InStorage_default",
                "InStorage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
