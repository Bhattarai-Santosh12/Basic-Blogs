using BasicBlogs.Entities;

using BasicBlogs.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BasicBlogs.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly AppDbContext _context;

        public object CookieAuthenticationDefault { get; private set; }

        public UserAccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public IActionResult Index()
        {
            return View(_context.UserAccounts.ToList());
        }

        //registration
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(UserAccountVM model)
        {
            if (ModelState.IsValid)
            {
                UserAccountVM account = new UserAccountVM();
                account.UserName = model.UserName;
                account.Email = model.Email;
                account.Password = model.Password;
                account.Address = model.Address;
                account.Phone = model.Phone;
                
            

                try
                {
                    _context.UserAccounts.Add(account);
                    _context.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = $"{account.UserName} Registration Successfull, Please Login";
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                // Check if the user is an admin (from UserLogins table)
                var adminUser = _context.UserLogins.FirstOrDefault(x =>
                    x.UserNameOrEmail == model.UserNameOrEmail && x.Password == model.Password);

                if (adminUser != null)
                {
                    // Create claims for the admin user
                    var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, adminUser.UserNameOrEmail),
                new Claim(ClaimTypes.Role, adminUser.Role),
            };

                    var adminIdentity = new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(adminIdentity));

                    // Redirect to admin page
                    return RedirectToAction("ReadBlogs", "MyBlog");
                }

                // Check if the user is a regular user (from UserAccounts table)
                var regularUser = _context.UserAccounts.FirstOrDefault(x =>
                    x.Email == model.UserNameOrEmail && x.Password == model.Password);

                if (regularUser != null)
                {
                    // Create claims for the regular user
                    var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, regularUser.Email),
                new Claim(ClaimTypes.Role, "User"), // You may set default "User" role
            };

                    var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity));

                    // Redirect to user panel
                 //   return RedirectToAction("UserPannel", "MyBlogsUserPannel");
                    return RedirectToAction("Index", "MyBlogsUserPannel", new { id = regularUser.Id });

                }

                // If no user is found
                ModelState.AddModelError("", "UserName/Email or Password is not correct");
            }

            return View(model);
        }


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
    }
}