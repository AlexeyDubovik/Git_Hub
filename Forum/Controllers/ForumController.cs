using Forum.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace Forum.Controllers
{
    public class ForumController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthService _authService;
        private readonly IStringLocalizer<ForumController> _localizer;
        private readonly DAL.Context.IntroContext _introContext;

        public ForumController(IStringLocalizer<ForumController> localizer, 
            ILogger<HomeController> logger, 
            IAuthService authService, 
            DAL.Context.IntroContext introContext)
        {
            _localizer = localizer;
            _logger = logger;
            _authService = authService;
            _introContext = introContext;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cultureFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            ViewData["Locale"] = cultureFeature?.RequestCulture.UICulture.ToString();
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
            return View();
        }
        public IActionResult Topic(string id)
        {
            Guid guid = Guid.Empty;
            try
            {
                guid = Guid.Parse(id);
                ViewData["Topic-id"] = id;
                ViewData["Title"] = _introContext.Topics!.Find(guid)!.Title;
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
