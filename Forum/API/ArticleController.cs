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
            if (_context.Topics!.Find(guid) == null)
            {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: topic absent";
            }
            var Articles = _DAO.GetArticles(DAO_Worker_Facade.GuidType.Topic, guid);
            if (Articles == null)
            {
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
            if (_context.Users!.Find(authorId) == null)
            {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: user with this id does not exist";
            }
            if (_context.Topics!.Find(articleModel.TopicId) == null)
            {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: topic absent";
            }
            if (articleModel.Text != null)
            {
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
        [HttpDelete("{ArticleID}")]
        public object Delete(String ArticleID, [FromHeader] String UserID)
        {
            Guid articleID = _GuidCheck.Check(HttpContext, ArticleID);
            Guid userID = _GuidCheck.Check(HttpContext, UserID);
            if (articleID == Guid.Empty || userID == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            var user = _context.Users!.Find(userID);
            var article = _context.Articles!.Find(articleID);
            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: user with this id does not exist";
            }
            if (article == null)
            {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: Article absent";
            }
            if (article.StatusJournal != null && article.StatusJournal
                .OrderByDescending(S => S.OperationDate)
                .FirstOrDefault()!.IsDeleted == true)
            {
                HttpContext.Response.StatusCode = 406;
                return "Not Acceptable : Article already deleted";
            }
            if (article.ReplyId != null)
            {
                HttpContext.Response.StatusCode = 406;
                return "Not Acceptable : Article has reply";
            }
            if (article.AuthorId != user.ID)
            {
                HttpContext.Response.StatusCode = 403;
                return "Forbidden : You have no rights";
            }
            var Status = new DAL.Entities.DeleteArticleStatus
            {
                ID = Guid.NewGuid(),
                ArticleID = articleID,
                UserID = userID,
                IsDeleted = true,
                OperationDate = DateTime.Now
            };
            _context.Add(Status);
            _context.SaveChanges();
            return JsonSerializer.Serialize(Status);
        }

    }
}
