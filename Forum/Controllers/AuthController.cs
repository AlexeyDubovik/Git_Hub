using Forum.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Forum.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly DAO_Worker_Facade _DAO;
        // Внедрение зависимостей через службы 
        public AuthController(ILogger<AuthController> logger, IAuthService authService, DAO_Worker_Facade DAO)
        {
            _authService = authService;
            _logger = logger;
            _DAO = DAO;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cultureFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            ViewData["Locale"] = cultureFeature?.RequestCulture.UICulture.ToString();
            
            if (_authService.User != null)
            {
                HttpContext.Session.Remove("AuthError");
                HttpContext.Session.Remove("AuthData");
                ViewData["authUser"] = _authService.User;
                ViewData["Interval"] = HttpContext.Session.GetString("Interval");
            }
            base.OnActionExecuting(context);
            if (context.RouteData.Values["culture"]!.ToString() != ViewData["Locale"]!.ToString())
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    culture = cultureFeature?.RequestCulture.UICulture.ToString(),
                    controller = context.RouteData.Values["controller"],
                    action = context.RouteData.Values["action"]!.ToString()
                }));
            }
        }
        public IActionResult Index()
        {
            String? err      = HttpContext.Session.GetString("AuthError");
            String? AuthData = HttpContext.Session.GetString("AuthData");
            if (_authService.User == null)
            {
                if (err != null)
                {
                    ViewData["AuthError"] = JsonSerializer.Deserialize<String[]>(err);
                    HttpContext.Session.Remove("AuthError");
                }
                if (AuthData != null)
                {
                    ViewData["AuthData"] = JsonSerializer.Deserialize<String[]>(AuthData);
                    HttpContext.Session.Remove("AuthData");
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult AuthUser(Models.RegUserModel UserData)
        {
            String[] err = new String[3];
            String[] data = new String[1];
            int errorCount = 0;
            if (UserData == null)
            {
                err[0] = "Incorrect request(no data)";
                errorCount++;
            }
            else
            {
                if (String.IsNullOrEmpty(UserData.Login))
                {
                    err[1] = "Error Field is Empty";
                    errorCount++;
                }
                else
                {
                    data[0] = UserData.Login;
                }
                if (String.IsNullOrEmpty(UserData.Password1))
                {
                    err[2] = "Error Field is Empty";
                    errorCount++;
                }
            }
            if (errorCount == 0)
            {
                var UserID = _DAO.Authenticator(UserData!);
                if (UserID == null)
                    err[0] = "Invalid Login or Password";
                else
                {
                    HttpContext.Session.SetString("UserID", UserID.ToString()!);
                    HttpContext.Session.SetString("authMoment", DateTime.Now.Ticks.ToString());
                    return Redirect("/");
                }
            }
            HttpContext.Session.SetString("AuthData", JsonSerializer.Serialize(data));
            HttpContext.Session.SetString("AuthError", JsonSerializer.Serialize(err));
            return RedirectToAction("Index");
        }
        public IActionResult Registration()
        {
            String? err       = HttpContext.Session.GetString("RegError");
            String? dataField = HttpContext.Session.GetString("dataField");
            if (err != null)
            {
                ViewData["err"] = JsonSerializer.Deserialize<String[]>(err);
                HttpContext.Session.Remove("RegError");
            }
            if (dataField != null)
            {
                ViewData["dataField"] = JsonSerializer.Deserialize<String[]>(dataField);
                HttpContext.Session.Remove("dataField");
            }
            return View();
        }
        [HttpPost]
        public IActionResult RegUser(Models.RegUserModel UserData)
        {
            String[] err = new String[7];
            String[] dataField = new String[4];
            int errorCount = 0;
            if (UserData == null) {
                err[0] = "Incorrect request(no data)";
                errorCount++;
            }
            else
            {
                //RealName
                if (String.IsNullOrEmpty(UserData.RealName)) {
                    err[1] = "Error Field is Empty";
                    errorCount++;
                }
                else if (!Regex.IsMatch(UserData.RealName!, @"^[А-ЯA-Z][а-яa-z]+\s[А-ЯA-Z][а-яa-z]+$")) {
                    err[1] = "Invalid RealName (Name Surname)";
                    errorCount++;
                }
                else
                    dataField[0] = UserData.RealName;
                //Login
                if (String.IsNullOrEmpty(UserData.Login)) {
                    err[2] = "Error Field is Empty";
                    errorCount++;
                }
                else if (_DAO.UserDataCheck(UserData.Login)) {
                    err[2] = "Login is already use";
                    errorCount++;
                }
                else
                    dataField[1] = UserData.Login;
                //Password
                if (String.IsNullOrEmpty(UserData.Password1)) {
                    err[3] = "Error Field is Empty";
                    errorCount++;
                }
                if (String.IsNullOrEmpty(UserData.Password2)) {
                    err[4] = "Error Field is Empty";
                    errorCount++;
                }
                if (0 > String.Compare(UserData.Password1, UserData.Password2)) {
                    err[5] = "Password is not Valid";
                    errorCount++;
                }
                //Email
                if (String.IsNullOrEmpty(UserData.Email)) {
                    err[6] = "Error Field is Empty";
                    errorCount++;
                }
                else if (!Regex.IsMatch(UserData.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")) { 
                    err[6] = "Invalid Email";
                    errorCount++;
                }
                else if (_DAO.UserDataCheck(UserData.Email)){
                    err[6] = "Invalid Email (this Email already use)";
                    errorCount++;
                }
                else
                    dataField[2] = UserData.Email;
                //Avatar
                //проверка на размер аватарки
                //
                //
                //
            }
            //Создаем пользователя
            if (errorCount == 0) {
                _DAO.CreateUser(UserData!);
                return RedirectToAction("Index");
            }
            //Отправляем ошибку
            HttpContext.Session.SetString("dataField", JsonSerializer.Serialize(dataField));
            HttpContext.Session.SetString("RegError", JsonSerializer.Serialize(err));
            return RedirectToAction("Register");
        }
        public IActionResult Profile()
        {
            if (_authService.User == null)
                return Redirect("/Auth/Index");
            String? err  = HttpContext.Session.GetString("PassError");
            String? Inf = HttpContext.Session.GetString("Info");
            if (err != null){
                ViewData["PassError"] = JsonSerializer.Deserialize<String[]>(err);               
                HttpContext.Session.Remove("PassError");                
            }
            if (Inf != null){
                ViewData["Info"] = JsonSerializer.Deserialize<String>(Inf);
                HttpContext.Session.Remove("Info");
            }
            return View();
        }
        public String ChangeLogin(String Login)
        {
            if (_authService.User != null)
                return _DAO.ChangeUserLogin(_authService.User!, Login);
            else
                return "User is null";
        }
        public String ChangeRealName(String NewName)
        {
            if (_authService.User != null)
                return _DAO.ChangeUserRealName(_authService.User!, NewName);
            else
                return "User is null";
        }
        public String ChangeEmail(String Email)
        {
            if (_authService.User != null)
                return _DAO.ChangeUserEmail(_authService.User, Email);
            else
                return "User is null";
        }
        [HttpPost]
        public String ChangeAvatar(IFormFile Avatar)
        {
            if (_authService.User != null)
                return _DAO.ChangeUserAvatar(_authService.User, Avatar);
            else
                return "User is null";
        }
        [HttpPost]
        public IActionResult ChangeUserPassword(Models.ChUserPasswordModel PassForm)
        {
            if (_authService.User == null)
                return Redirect("/Home/Index");
            String[] err = new String[3];
            int errorCount = 0;
            if (String.IsNullOrEmpty(PassForm.OldPassword)) {
                err[0] = "Error Field is Empty";
                errorCount++;
            }
            if (String.IsNullOrEmpty(PassForm.NewPassword1)) {
                err[1] = "Error Field is Empty";
                errorCount++;
            }
            if (String.IsNullOrEmpty(PassForm.NewPassword2)) {
                err[2] = "Error Field is Empty";
                errorCount++;
            }
            if (errorCount == 0) {
                String result = _DAO.ChangeUserPassword(_authService.User, PassForm);
                if (result != "Success")
                    err[0] = result;
                if (PassForm.NewPassword1 != PassForm.NewPassword2)
                    err[2] = "Password is not Valid";
                else if (result == "Success") {
                    String confirm = result;
                    HttpContext.Session.SetString("Info", JsonSerializer.Serialize(confirm));
                    return RedirectToAction("Profile");
                }
            }
            HttpContext.Session.SetString("PassError", JsonSerializer.Serialize(err));
            return RedirectToAction("Profile");
        }
    }
}
