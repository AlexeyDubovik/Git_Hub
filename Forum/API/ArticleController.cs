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
        private readonly DAO_Worker_Facade _DAO;
        public ArticleController(IntroContext context, HttpRequestGuidCheck GuidCheck, DAO_Worker_Facade DAO)
        {
            _context = context;
            _GuidCheck = GuidCheck;
            _DAO = DAO;
        }
        [HttpGet("{TopicID}")]
        public object Get(String TopicID)
        {
            Guid guid = _GuidCheck.Check(HttpContext, TopicID);
            if (guid == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            if (_context.Topics!.Find(guid) == null) {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: topic absent";
            }
            var Articles = _DAO.GetArticles(DAO_Worker_Facade.GuidType.Topic, guid);
            if (Articles == null) {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: Topic empty";
            }
            return Articles;
        }
        [HttpPost]
        public object Post([FromHeader] String AuthorId, [FromForm] ArticleModel articleModel)
        {
            Guid authorId = _GuidCheck.Check(HttpContext, AuthorId);
            if (authorId == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            if (_context.Users!.Find(authorId) == null) {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: user with this id does not exist";
            }
            if (_context.Topics!.Find(articleModel.TopicId) == null) {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: topic absent";
            }
            if (articleModel.Text != null) {
                var NewArticle = new DAL.Entities.Article
                {
                    Id = new Guid(),
                    AuthorId = authorId,
                    TopicId = articleModel.TopicId,
                    Text = articleModel.Text,
                    CreatedDate = DateTime.Now,
                    ReplyId = articleModel.ReplyId
                };
                _context.Articles?.Add(NewArticle);
                _context.SaveChanges();
                return JsonSerializer.Serialize($"Success: Article {NewArticle.Id} created");
            }
            HttpContext.Response.StatusCode = 418;
            return $" I’m a teapot";
        }
    }
}
