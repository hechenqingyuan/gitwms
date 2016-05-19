using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Returns
{
    public class ReturnsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Returns";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Returns_default",
                "Returns/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
