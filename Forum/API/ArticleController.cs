using Forum.DAL.Context;
using Forum.Models;
using Forum.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Forum.API
{
    [Route("api/Article")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly IntroContext _context;
        private readonly HttpRequestGuidCheck _GuidCheck;
        public ArticleController(IntroContext context, HttpRequestGuidCheck GuidCheck)
        {
            _context = context;
            _GuidCheck = GuidCheck;
        }
        [HttpGet("{TopicID}")]
        public object Get(String TopicID)
        {
            Guid guid = _GuidCheck.Check(HttpContext, TopicID);
            if (guid == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            var Topic = _context.Topics!.Find(guid);
            if (Topic == null) {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: topic absent";
            }
            var Articles = _context.Articles!.Select(A => A).Where(A=> A.TopicId == guid).ToList();
            if (Articles == null) {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: Topic empty";
            }
            return JsonSerializer.Serialize(Articles);
        }
        [HttpPost]
        public object Post([FromHeader] String AuthorId, [FromForm] ArticleModel articleModel)
        {
            Guid authorId = _GuidCheck.Check(HttpContext, AuthorId);
            if (authorId == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            if(articleModel.Text != null)
            {
                _context.Articles?.Add(new DAL.Entities.Article
                {
                    Id = new Guid(),
                    AuthorId = authorId,
                    TopicId = articleModel.TopicId,
                    Text = articleModel.Text,
                    CreatedDate = DateTime.Now,
                    ReplyId = null
                    //PictureFile = "default"
                });
                _context.SaveChanges();
                return $"POST Success {AuthorId} - {articleModel.TopicId} {articleModel.Text}";
            }
            HttpContext.Response.StatusCode = 418;
            return $" I’m a teapot";
        }
    }
}
