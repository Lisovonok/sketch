using System.Web.Mvc;
using System.Web.Security;
using MVCBlog.Website.Models.InputModels.Login;

namespace MVCBlog.Website.Controllers
{
    public partial class LoginController : Controller
    {
        public virtual ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction(MVC.Blog.Index());
            }
            else
            {
                return View();
            }
        }
    
        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual ActionResult Index(LoginFormInput loginFormInput, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!FormsAuthentication.Authenticate(loginFormInput.Username, loginFormInput.Password))
            {
                ModelState.AddModelError("login", Properties.Common.LoginFailure);
                return View();
            }

            FormsAuthentication.SetAuthCookie(loginFormInput.Username, loginFormInput.RememberMe);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(MVC.Login.Index());
            }
        }

        public virtual ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction(MVC.Login.Index());
        }
    }
}
