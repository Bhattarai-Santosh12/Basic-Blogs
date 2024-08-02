using Microsoft.AspNetCore.Mvc;

namespace BasicBlogs.Controllers
{
    public class LoginSignUp : Controller
    {

        //registration
        public IActionResult LoginSignup()
        {
            return View();
        }


        //login
        public IActionResult Login()
        {
            return View();
        }
    }
}
