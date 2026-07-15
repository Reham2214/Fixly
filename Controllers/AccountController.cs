using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Fixly.Data;
using Fixly.Models;
using System.Threading.Tasks;

namespace Fixly.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
         private readonly UserManager<ApplicationUser> _userManager;
         private readonly AppDbContext _context;
         private readonly IWebHostEnvironment _environment;
         public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AppDbContext cn, IWebHostEnvironment environment) 
        {
            _signInManager = signInManager;
            _userManager = userManager; 
            _context =cn;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel { };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);
                return RedirectToRoleHome(roles.FirstOrDefault());
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model.Role == "Service Provider")
            {
                if (string.IsNullOrWhiteSpace(model.ServiceCategory))
                    ModelState.AddModelError(nameof(model.ServiceCategory),
                        "التخصص مطلوب.");

                if (!model.YearsExperience.HasValue)
                    ModelState.AddModelError(nameof(model.YearsExperience),
                        "عدد سنوات الخبرة مطلوب.");

                if (string.IsNullOrWhiteSpace(model.About))
                    ModelState.AddModelError(nameof(model.About),
                        "النبذة مطلوبة.");
            }

            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                City = model.City,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            if (model.Role == "Service Provider")
            {
                var providerProfile = new ServiceProviderProfile
                {
                    UserId = user.Id,
                    ServiceCategory = model.ServiceCategory,
                    YearsExperience = model.YearsExperience!.Value,
                    About = model.About
                };

                _context.ServiceProviderProfiles.Add(providerProfile);

                await _context.SaveChangesAsync();

                if (model.WorkImages != null && model.WorkImages.Any())
                {
                    string uploadFolder = Path.Combine(
                        _environment.WebRootPath,
                        "images");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    foreach (var image in model.WorkImages)
                    {
                        if (image.Length > 0)
                        {
                            string fileName =
                                Guid.NewGuid().ToString() +
                                Path.GetExtension(image.FileName);

                            string filePath =
                                Path.Combine(uploadFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            _context.WorkImages.Add(new WorkImage
                            {
                                ServiceProviderProfileId = providerProfile.Id,
                                ImagePath = "images/" + fileName
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToRoleHome(model.Role);
        }

        private IActionResult RedirectToRoleHome(string role)
        {
            switch (role)
            {
                case "Customer":
                    return RedirectToAction("Index", "Customer");

                case "Service Provider":
                    return RedirectToAction("Index", "ServiceProviders");

                default:
                    return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // تنظيف جلسة الدخول وحذف ملف تعريف الارتباط (Cookie)
            await _signInManager.SignOutAsync();
            
            // توجيه المستخدم إلى الصفحة الرئيسية بعد تسجيل الخروج
            return RedirectToAction("Login", "Account");
        }
           
    }
}