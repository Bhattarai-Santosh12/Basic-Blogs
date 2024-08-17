using BasicBlogs.Entities;


using BasicBlogs.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BasicBlogs.Controllers
{
    [Authorize]
    public class UserAccountController : Controller
    {
        private  readonly AppDbContext _context;

        public object CookieAuthenticationDefault { get; private set; }

        public UserAccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        [Authorize]
        public IActionResult Index ()
        { 
        return View(_context.UserAccounts.ToList());
        }



        //registration
        [AllowAnonymous]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(UserAccountVM model)
        {
            if (ModelState.IsValid)
            {
                UserAccountVM account= new UserAccountVM();
                account.Email = model.Email;
                account.Address = model.Address;
                account.Password = model.Password;
                account.Phone = model.Phone;
                account.UserName = model.UserName;

                try
                {
                    _context.UserAccounts.Add(account);
                    _context.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = $"{account.UserName} Login Successfull, Please Login";
                    return RedirectToAction("Login");
                }
                catch (DbUpdateException ex)
                {

                    ModelState.AddModelError("", "Please enter a valid email address");
                    return View(model);
                }
                return View();
            }

            return View(model);
        }

        //login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //public IActionResult Login(LoginVM model)
        public async Task<IActionResult> Login(LoginVM model)
        {
            if(ModelState.IsValid)
            {
                var User = _context.UserAccounts.Where(x => (x.UserName == model.UserNameOrEmail || x.Email==model.UserNameOrEmail )&& x.Password == model.Password).FirstOrDefault();
                if(User != null)
                {
                    //Success, Create Cookie

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, User.Email),
                        new Claim("Name", User.UserName),
                        new Claim(ClaimTypes.Role,"User"),
                    };
                    var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ClaimsIdentity));

                   
                    return RedirectToAction("ReadBlogs", "MyBlog");
                }
                else
                {
                    ModelState.AddModelError("", "UserName/Email or Password is not correct");
                }

            }

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "UserAccount"); 
        }


        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
