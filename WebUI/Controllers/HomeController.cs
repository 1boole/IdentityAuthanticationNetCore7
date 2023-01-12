using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WebUI.Entities;
using WebUI.Helper;
using WebUI.Models;
using Response = WebUI.Entities.Response;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomIdentityUser> _signInManager;

        public HomeController(
            UserManager<CustomIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, 
            SignInManager<CustomIdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(MyProfile));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModelViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {


                var userRoles = await _userManager.GetRolesAsync(user);

                await _signInManager.PasswordSignInAsync(model.Username,model.Password, false, false);

                
                return RedirectToAction(nameof(MyProfile));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(MyProfile));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });


            var imagePath = FileHelper.PhotoSave(model.PhotoUpload);

            CustomIdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                PhotoUrl = imagePath,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                TempData["Message"] = $"errors: {result.Errors.FirstOrDefault().Description}!";
                return View();
            }


            TempData["Message"] = "User created successfully!";
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {

            var userId=HttpContext.User.Claims.Where(x=>x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var user=await _userManager.FindByIdAsync(userId);

            var model = new ProfileViewModel
            {
                Email = user.Email,
                PhotoUrl = user.PhotoUrl,
                Username = user.UserName,
            };

            return View(model);
        }

    }
}
