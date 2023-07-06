using CinemaManagementSystem.BUSINESS.Abstract;
using CinemaManagementSystem.WEBUI.EmailServices;
using CinemaManagementSystem.WEBUI.Extensions;
using CinemaManagementSystem.WEBUI.Identity;
using CinemaManagementSystem.WEBUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagementSystem.WEBUI.Controllers
{
    //[AutoValidateAntiforgeryToken]
    public class AccountController: Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        private IBookingCartService _bookingcartService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, IBookingCartService bookingcartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _bookingcartService = bookingcartService;

        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        //[ValidateAntiForgeryToken]
        [HttpPost]
        
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user==null)
            {
                ModelState.AddModelError("", "No account has been created with this username before.");
                return View(model);
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Please confirm your membership with the link sent to your account..");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true , false);
            if(result.Succeeded)
            {
                return Redirect("/Admin/MovieList");
            }
            ModelState.AddModelError("", "The entered username or password was entered incorrectly.");
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                //user bilgisi veriliyor, oluşturulan token bilgisi dbye kaydediliyor.
                //url ve token eşleşmesi gerekiyor.
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new { 
                    userId = user.Id,
                    token = code
                });
                await _emailSender.SendEmailAsync(model.Email, "Please confirm your account.", $"Please <a href='https://localhost:7058{url}'>click</a> the link to confirm your email account.");
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "An unknown error occurred, please try again.");
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            TempData.Put("message", new MessageInfo()
            {
                Title = "Session closed.",
                Message = "Your account has been securely closed.",
                AlertType = "danger"
            });
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(userId == null || token == null)
            {
                TempData.Put("message", new MessageInfo()
                {
                    Title = "Inavlid token.",
                    Message = "Invalid token.",
                    AlertType = "danger"
                });
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user!=null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if(result.Succeeded)
                {
                    //hesabım onaylandığı anda cart objesini oluşturuyorum.
                    _bookingcartService.InitializeCart(user.Id);
                    TempData.Put("message", new MessageInfo()
                    {
                        Title = "Your account has been confirmed.",
                        Message = "Your account has been confirmed.",
                        AlertType = "success"
                    });
                    return View();
                }
            }
            TempData.Put("message", new MessageInfo()
            {
                Title = "Your account has not been confirmed.",
                Message = "Your account has not been confirmed.",
                AlertType = "warning"
            });
            return View();
        }
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return View();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
           
            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = code
            });
            await _emailSender.SendEmailAsync(Email,"Reset Password" , $"Please <a href='https://localhost:7058{url}'>click</a> the link to reset your password.");
            return View();
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            if(userId==null || token==null)
            {
                return RedirectToAction("/Home/Index");
            }
            var model = new ResetPasswordModel { Token = token };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if(ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return RedirectToAction("/Home/Index");
            }
            var result = await _userManager.ResetPasswordAsync(user,model.Token,model.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("/Account/Login");
            }
                return View(model); 
        }
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
   
}
