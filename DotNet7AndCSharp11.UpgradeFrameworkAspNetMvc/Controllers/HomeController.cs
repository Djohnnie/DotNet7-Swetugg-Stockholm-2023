namespace DotNet7AndCSharp11.UpgradeFrameworkAspNetMvc.Controllers
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View();
        }

        public Microsoft.AspNetCore.Mvc.ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public Microsoft.AspNetCore.Mvc.ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}