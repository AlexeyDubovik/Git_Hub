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
        private readonly IntroContext _db;
        private readonly HttpRequestGuidCheck _GuidCheck;
        private readonly DAO_Worker_Facade _DAO;
        private readonly IAuthService _auth;
        public ArticleController(IntroContext db, HttpRequestGuidCheck GuidCheck, DAO_Worker_Facade DAO, IAuthService auth)
        {
            _db = db;
            _GuidCheck = GuidCheck;
            _DAO = DAO;
            _auth = auth;
        }
        [HttpGet("{ID}")]
        public object Get(String ID)
        {
            Guid guid = _GuidCheck.Check(HttpContext, ID);
            DAO_Worker_Facade.GuidType GT = DAO_Worker_Facade.GuidType.All;
            DAO_Worker_Facade.Parameters DP = DAO_Worker_Facade.Parameters.Default;
            String ParamSearch = HttpContext.Request.Query["ParamSearch"];
            String GuidType = HttpContext.Request.Query["GuidType"];
            if (guid == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            if (GuidType == "User")
            {
                if (_db.Users!.Find(guid) == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    return "Not Found: User absent";
                }
                GT = DAO_Worker_Facade.GuidType.User;
            }
            if (GuidType == "Article")
            {
                if (_db.Articles!.Find(guid) == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    return "Not Found: Article absent";
                }
                GT = DAO_Worker_Facade.GuidType.Article;
            }
            else if (GuidType == null || GuidType == "Topic")
            {
                if (_db.Topics!.Find(guid) == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    return "Not Found: topic absent";
                }
                GT = DAO_Worker_Facade.GuidType.Topic;
            }
            if (ParamSearch == "Deleted")
            {
                DP = DAO_Worker_Facade.Parameters.Deleted;
            }
            var Articles = _DAO.GetArticles(GT, guid, DP);
            if (Articles == null)
            {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: Articles empty";
            }
            return Articles;
        }
        [HttpPost]
        public object Post([FromHeader] String AuthorId, [FromForm] ArticleModel articleModel)
        {
            Guid authorId = _GuidCheck.Check(HttpContext, AuthorId);
            if (authorId == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            if (_db.Users!.Find(authorId) == null)
            {
                HttpContext.Response.StatusCode = 404;
                return "Not Found: user with this id does not exist";
            }
            if (_db.Topics!.Find(articleModel.TopicId) == null)
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
                _db.Articles?.Add(NewArticle);
                _db.SaveChanges();
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
            var user = _db.Users!.Find(userID);
            var article = _db.Articles!.Find(articleID);
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
            _db.Add(Status);
            _db.SaveChanges();
            return JsonSerializer.Serialize(Status);
        }
        public object Default([FromQuery]string id)
        {
            switch (HttpContext.Request.Method)
            {
                case "RESTORE":
                    return Restore(id);
                default:
                    break;
            }
            return new { method = HttpContext.Request.Method, ID = id };
        }
        private object Restore(string id)
        {
            Guid ArticleID = _GuidCheck.Check(HttpContext, HttpContext.Request.Query["ArticleID"]);
            Guid UserID = _GuidCheck.Check(HttpContext, id);
            if (ArticleID == Guid.Empty || UserID == Guid.Empty)
                return "Conflict: invalid id format (GUID required)";
            if(_auth.User!.ID != UserID)
            {
                HttpContext.Response.StatusCode = 403;
                return "Forbidden : You have no rights";
            }
            var SJ = _db.SatatusJournal!.Where(R=>R.ArticleID == ArticleID).FirstOrDefault();
            if (SJ != null)
            {
                SJ.IsDeleted = false;
                SJ.OperationDate = DateTime.Now;
                SJ.UserID = UserID;
                _db.SaveChanges();
            }
            return JsonSerializer.Serialize(SJ);
        }
    }
}
