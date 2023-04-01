using BookingApp.Repository.RepUser;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Controllers
{
    public class AuthenticationController : Controller
    {
        IUserRepository userRepository = null;
        public AuthenticationController() => userRepository = new UserRepository();
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Models.User user, string ConfirmPassword)
        {

            if (user.Password != ConfirmPassword)
            {
                ViewBag.Msg = "Password is not match";
                return View();
            }
            if (user.Password.Length < 8)
            {
                ViewBag.Msg = "Password > 8 Character";
                return View();
            }
            ViewBag.Msg = "Register successfully";
            userRepository.Register(user);
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Models.User user)
        {

            Console.WriteLine(user.Username);
            Console.WriteLine(user.Password);
            Models.User userLogined = userRepository.Login(user);
           
            if (userLogined != null)
            {
                if (userLogined.Role == "ADMIN")
                {
                    HttpContext.Session.SetString("Logined", userLogined.Id.ToString());
                    HttpContext.Session.SetString("Roll", userLogined.Role);
                    return RedirectToAction("Index", "Admin");
                }
                HttpContext.Session.SetString("Logined", userLogined.Id.ToString());
                HttpContext.Session.SetString("Roll", userLogined.Role);

                return RedirectToAction("Room", "Room");
            }

            else
            {
                ViewBag.Msg = "Login Failed";
                return View();
            }


        }
    }
}
