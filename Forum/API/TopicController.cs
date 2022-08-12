using Forum.DAL.Context;
using Forum.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API
{
    [Route("api/Topic")]
    [ApiController]
    public class TopicController : Controller
    {
        private readonly IntroContext _context;
        private readonly HttpRequestGuidCheck _GuidCheck;

        public TopicController(HttpRequestGuidCheck guidCheck, IntroContext context)
        {
            _context = context;
            _GuidCheck = guidCheck;
        }

        [HttpPost]
        public object Create(Models.TopicModel topic)
        {
            Guid UserId = _GuidCheck.Check(HttpContext, HttpContext.Request.Headers["User-Id"].ToString());
            if (UserId == Guid.Empty)
                return new
                {
                    status = "Error",
                    message = "User-Id Header Empty or Invalid"
                };
            if (topic == null)
            {
                return new
                {
                    status = "Error",
                    message = "No Data"
                };
            }
            if (String.IsNullOrEmpty(topic.Title) || String.IsNullOrEmpty(topic.Description))
            {
                return new
                {
                    status = "Error",
                    message = "Empty Title or Description"
                };
            }
            var user = _context.Users!.Find(UserId);
            if (user == null)
            {
                return new
                {
                    status = "Error",
                    message = "Forbidden"
                };
            }
            if (_context.Topics!.Where(t => t.Title == topic.Title).Any())
            {
                return new
                {
                    status = "Error",
                    message = $"Topic {topic.Title} is absent"
                };
            }
            _context.Topics!.Add(new()
            {
                Title = topic.Title,
                Descrtiption = topic.Description,
                AuthorId = UserId
            });
            _context.SaveChanges();
            String Culture = HttpContext.Request.Headers["Culture"].ToString();
            var Default = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            return new
            {
                status = "Ok",
                message = $"Topic {topic.Title} created"
            };
        }
        //[HttpGet]
        //public IEnumerable<DAL.Entities.Topic> Get(Models.TopicModel topic)
        //{
        //    return _context.Topics!;
        //}
        [HttpGet]
        public IEnumerable<DAL.Entities.Topic> Get()
        {
            return _context.Topics!;
        }
    }
}
