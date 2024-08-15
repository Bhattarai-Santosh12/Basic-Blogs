using Microsoft.AspNetCore.Mvc;

namespace BasicBlogs.Controllers
{
    public class MyBlogsUserPannelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
