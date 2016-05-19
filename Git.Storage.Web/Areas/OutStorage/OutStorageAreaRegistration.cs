using System.Web.Mvc;

namespace Git.Storage.Web.Areas.OutStorage
{
    public class OutStorageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "OutStorage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "OutStorage_default",
                "OutStorage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
