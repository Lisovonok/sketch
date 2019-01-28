using System.Net;
using System.Web.Mvc;

namespace MVCBlog.Website.Controllers
{
    public partial class ErrorController : Controller
    {
        public virtual ActionResult Index()
        {
            return View(MVC.Shared.Views.Error);
        }

        public virtual ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;

            return View();
        }
    }
}
