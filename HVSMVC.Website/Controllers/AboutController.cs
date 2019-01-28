using System.Web.Mvc;

namespace MVCBlog.Website.Controllers
{
    public partial class AboutController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
