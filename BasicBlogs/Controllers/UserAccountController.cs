using BasicBlogs.Entities;
using BasicBlogs.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BasicBlogs.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly AppDbContext _context;

        public UserAccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // Index to list users
        public IActionResult Index()
        {
            return View(_context.UserAccounts.ToList());
        }

        // Registration action
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(UserAccountVM model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.UserAccounts.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser == null)
                {
                    var account = new UserAccountVM
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Password = model.Password, // Make sure to hash passwords in a real application
                        Address = model.Address,
                        Phone = model.Phone
                    };

                    try
                    {
                        _context.UserAccounts.Add(account);
                        _context.SaveChanges();
                        ModelState.Clear();
                        ViewBag.Message = $"{account.UserName} Registration Successful, Please Login";
                        return RedirectToAction("Login");
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError("", "Please enter a valid email address");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists");
                }
            }

            return View(model);
        }

        // Login action
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var adminUser = _context.UserLogins.FirstOrDefault(x =>
                    x.UserNameOrEmail == model.UserNameOrEmail && x.Password == model.Password);

                if (adminUser != null)
                {
                    var adminClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, adminUser.UserNameOrEmail),
                        new Claim(ClaimTypes.Role, adminUser.Role),
                    };

                    var adminIdentity = new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(adminIdentity));

                    return RedirectToAction("ReadBlogs", "MyBlog");
                }

                var regularUser = _context.UserAccounts.FirstOrDefault(x =>
                    x.Email == model.UserNameOrEmail && x.Password == model.Password);

                if (regularUser != null)
                {
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, regularUser.Email),
                        new Claim(ClaimTypes.Role, "User"),
                    };

                    var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity));

                    return RedirectToAction("Index", "MyBlogsUserPannel");
                }

                ModelState.AddModelError("", "UserName/Email or Password is not correct");
            }

            return View(model);
        }

        // Google External Login
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "UserAccount", new { ReturnUrl = returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        // Google External Login Callback
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result?.Principal == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var externalClaims = result.Principal.Claims.ToList();
            var userEmail = externalClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var userName = externalClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var user = _context.UserAccounts.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                user = new UserAccountVM
                {
                    Email = userEmail,
                    UserName = userName,
                    Password = "ExternalLoginProvided",
                    Phone = null,
                    Address = null
                };
                _context.UserAccounts.Add(user);
                await _context.SaveChangesAsync();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            // return LocalRedirect(returnUrl);
            return RedirectToAction("Index", "MyBlogsUserPannel");
        }

        // Logout action
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
