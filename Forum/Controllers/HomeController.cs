using Forum.Models;
using Forum.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITimeManage _time;
        private readonly IAuthService _authService;
        private readonly DAO_Worker_Facade _DAO;
        private readonly RandomServices _randomService;
        private readonly IStringLocalizer<HomeController> _localizer;
        public HomeController(                  // Внедрение зависимостей через службы 
            ILogger<HomeController> logger,
            ITimeManage time,
            IAuthService authService,
            DAO_Worker_Facade DAO,
            RandomServices randomServices,
            IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _time = time;
            _DAO = DAO;
            _authService = authService;
            _randomService = randomServices;
            //_localizer = factory.Create("ForController", this.GetType().Assembly.GetName().Name!);
            _localizer = localizer;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cultureFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            ViewData["Locale"] = cultureFeature?.RequestCulture.UICulture.ToString();
            //Достаю ЮЗЕРА через сервис авторизации4
            if (_authService.User != null)
            {
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
            ViewData["time"] = _time.Time;
            ViewData["Date"] = _time.Date;
            ViewData["Users"] = _DAO._db.Users == null ? "Empty" : _DAO._db.Users?.Count();
            ViewData["fromAuthMiddleware"] = HttpContext.Items["fromAuthMiddleware"];
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            var model = new About
            {
                Data = _localizer["AboutData"]
            };
            return View(model);
        }
        public IActionResult Contacts()
        {
            var model = new Contacts
            {
                Address = " 1981 Landings Drive,  Building K,   Mountain View, CA 94043 - 0801, ",
                Phone = "+38067 388 3330",
                Name = "Mozilla Foundation"
            };
            return View(model);
        }
        public IActionResult Auth()
        {
            return View();
        }
        public ViewResult Register()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Exit()
        {
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("authMoment");
            return RedirectToAction("Index");
        }
        public String? GetInterval()
        {
            if (_authService.User == null)
                return null;
            var value = HttpContext.Items["fromAuthMiddleware"];
            return value?.ToString();
        }
    }
}