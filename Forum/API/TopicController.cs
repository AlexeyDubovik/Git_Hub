using Forum.DAL.Context;
using Forum.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Forum.API
{
    [Route("api/Topic")]
    [ApiController]
    public class TopicController : Controller
    {
        private readonly IntroContext _context;
        private readonly HttpRequestGuidCheck _GuidCheck;
        private readonly DAO_Worker_Facade _DAO;

        public TopicController(HttpRequestGuidCheck guidCheck, IntroContext context, DAO_Worker_Facade DAO)
        {
            _context = context;
            _GuidCheck = guidCheck;
            _DAO = DAO;
        }
        [HttpPost]
        public object Create(Models.TopicModel topic)
        {
            Guid UserId = _GuidCheck.Check(HttpContext, HttpContext.Request.Headers["User-Id"].ToString());
            if (UserId == Guid.Empty)
                return new {
                    status = "Error",
                    message = "User-Id Header Empty or Invalid"
                };
            if (topic == null) {
                return new {
                    status = "Error",
                    message = "No Data"
                };
            }
            if (String.IsNullOrEmpty(topic.Title) || String.IsNullOrEmpty(topic.Description)) {
                return new {
                    status = "Error",
                    message = "Empty Title or Description"
                };
            }
            var user = _context.Users!.Find(UserId);
            if (user == null) {
                return new {
                    status = "Error",
                    message = "Forbidden"
                };
            }
            if (_context.Topics!.Where(t => t.Title == topic.Title).Any()) {
                return new {
                    status = "Error",
                    message = $"Topic {topic.Title} is absent"
                };
            }
            String Culture = HttpContext.Request.Headers["Culture"].ToString();
            _context.Topics!.Add(new() {
                Title = topic.Title,
                Descrtiption = topic.Description,
                AuthorId = UserId,
                Culture = Culture
            });
            _context.SaveChanges();
            var Default = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            return new {
                status = "Ok",
                message = $"Topic {topic.Title} created"
            };
        }
        [HttpGet]
        public object Get()
        {
            var Topic = _DAO.GetTopics(DAO_Worker_Facade.GuidType.All);
            if (Topic == null) {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: topic absent";
            }
            return JsonSerializer.Serialize(Topic);
        }
        [HttpGet("{id}")]
        public object Get(String id)
        {
            Guid guid = _GuidCheck.Check(HttpContext, id);
            if (guid == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            var Topic = _DAO.GetTopics(DAO_Worker_Facade.GuidType.Topic, guid);
            if (Topic == null) {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: topic absent";
            }
            return JsonSerializer.Serialize(Topic);
        }
    }
}
