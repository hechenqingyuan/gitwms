using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Bad
{
    public class BadAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Bad";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Bad_default",
                "Bad/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
