using Forum.DAL.Context;
using Forum.DAO;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using Forum.DAL.Entities;

namespace Forum.Services
{
    public class DAO_Worker_Facade
    {
        public readonly IntroContext _db;
        private readonly Authenticator authenticator;
        private readonly RandomServices random;
        private readonly IHasher _hasher;
        private UserCheck? userCheck;
        public enum GuidType
        {
            All,
            User,
            Topic,
            Article,
        }
        public enum Parameters
        {
            Default,
            Deleted
        }
        public DAO_Worker_Facade(IntroContext db, IHasher hasher, RandomServices randomService)
        {
            _hasher = hasher;
            _db = db;
            random = randomService;
            authenticator = new Authenticator(db.Users!);
        }
        public bool UserDataCheck(string str)
        {
            if (str == null)
                return false;
            if (Regex.IsMatch(str, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                userCheck = new CheckUserEmail(_db.Users!);
            else
                userCheck = new CheckUserLogin(_db.Users!);
            return userCheck.Check(str);
        }
        public Guid? Authenticator(Models.UserModel UserData)
        {
            return authenticator.Authenticate(UserData, _hasher);
        }
        private void DeletePicture(String PictureName, String Path = "./wwwroot/img/")
        {
            File.Delete(Path + PictureName);
        }
        private String? SavePicture(IFormFile Picture, String Path = "./wwwroot/img/")
        {
            if (Picture == null)
                return null;
            int pos = Picture.FileName.LastIndexOf('.');
            string IMG_Format = Picture.FileName.Substring(pos);
            string ImageName = _hasher.Hash(Guid.NewGuid().ToString() + _hasher.Hash(random.Integer.ToString()));
            var file = new FileStream(Path + ImageName + IMG_Format, FileMode.Create);
            Picture.CopyToAsync(file).ContinueWith(f => file.Dispose());
            return ImageName + IMG_Format;
        }
        public void CreateUser(Models.UserModel UserData)
        {
            if (UserData != null && _db.Users != null)
            {
                var user = new DAL.Entities.User();
                user.PassSalt = _hasher.Hash(random.Integer.ToString());
                user.PassHash = _hasher.Hash(UserData.Password1 + user.PassSalt);
                if (UserData.Avatar != null)
                    user.Avatar = SavePicture(UserData.Avatar)!;
                else
                    user.Avatar = "default.png";
                user.Email = UserData.Email!;
                user.RealName = UserData.RealName!;
                user.Login = UserData.Login!;
                user.RegMoment = DateTime.Now;
                _db.Users.Add(user);
                _db.SaveChanges();
            }
        }
        public String ChangeUserLogin(DAL.Entities.User User, String Login)
        {
            if (UserDataCheck(Login))
                return "Login already in use";
            else if (!Regex.IsMatch(Login, @"^[^\s()-]*$") && User != null)
            {
                User.Login = Login;
                _db.Update(User);
                _db.SaveChanges();
                return "Success";
            }
            else
                return "Error";
        }
        public String ChangeUserRealName(DAL.Entities.User User, String NewName)
        {
            if (Regex.IsMatch(NewName, @"^[А-ЯA-Z][а-яa-z]+\s[А-ЯA-Z][а-яa-z]+$") && User != null)
            {
                User.RealName = NewName;
                _db.Update(User);
                _db.SaveChanges();
                return "Success";
            }
            else
                return "Error";
        }
        public String ChangeUserEmail(DAL.Entities.User User, String Email)
        {
            if (UserDataCheck(Email))
                return "Email already in use";
            else if (Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") && User != null)
            {
                User.Email = Email;
                _db.Update(User);
                _db.SaveChanges();
                return "Success";
            }
            else
                return "Error";
        }
        public String ChangeUserPassword(DAL.Entities.User User, Models.PasswordUserModel PassForm)
        {
            if (User == null)
                return "Error User is null";
            if (User.PassHash != _hasher.Hash(PassForm.OldPassword + User.PassSalt))
                return "Invalid Password";
            else if (PassForm.NewPassword2 != null)
            {
                User.PassSalt = _hasher.Hash(random.Integer.ToString());
                User.PassHash = _hasher.Hash(PassForm.NewPassword2 + User.PassSalt);
                _db.Update(User);
                _db.SaveChanges();
                return "Success";
            }
            else
                return "Error";
        }
        public String ChangeUserAvatar(DAL.Entities.User User, IFormFile Avatar)
        {
            if (User == null)
                return "Error User is null";
            else if (Avatar == null)
                return "Error Avatar is null";
            else
                try
                {
                    DeletePicture(User.Avatar);
                    User.Avatar = SavePicture(Avatar)!;
                    _db.Update(User);
                    _db.SaveChanges();
                    return "Success";
                }
                catch
                {
                    return "Error";
                }
        }
        
        public dynamic? GetTopics(GuidType GType, Guid? guid = null)
        {
            if (guid == null && GType != GuidType.All) {
                return null;
            }
            return _db.Topics!
                .Where(t =>
                GType == GuidType.Topic ? t.Id == guid :
                GType == GuidType.User ? t.AuthorId == guid :
                                          t.Id == t.Id)
                .Select(t => new
                {
                    Id = t.Id,
                    Title = t.Title,
                    Descrtiption = t.Descrtiption,
                    Culture = t.Culture,
                    Author = t.Author == null ? null : new
                    {
                        ID = t.Author!.ID,
                        Login = t.Author.Login,
                        RealName = t.Author.RealName,
                        Email = t.Author.Email,
                        Avatar = t.Author.Avatar
                    },
                    ArticlesInfo = t.articles!.Count(a => a.StatusJournal!
                                              .OrderBy(S => S.OperationDate)
                                              .LastOrDefault()!.IsDeleted != true) == 0 ?
                    new
                    {
                        Count = t.articles!.Count(a => a.StatusJournal!
                                           .OrderBy(S => S.OperationDate)
                                           .LastOrDefault()!.IsDeleted != true),
                        CreatedDate = DateTime.Parse("11.11.1111"),
                        RealName = "Empty",
                        Login = "Empty",
                        Email = "Empty",
                    }
                    :
                    new
                    {
                        Count = t.articles!.Count(a => a.StatusJournal!
                                           .OrderBy(S => S.OperationDate)
                                           .LastOrDefault()!.IsDeleted != true),
                        CreatedDate = t.articles!.OrderBy(a => a.CreatedDate).LastOrDefault()!.CreatedDate,
                        RealName = t.articles!.OrderBy(a => a.CreatedDate).LastOrDefault()!.Author!.RealName,
                        Login = t.articles!.OrderBy(a => a.CreatedDate).LastOrDefault()!.Author!.Login,
                        Email = t.articles!.OrderBy(a => a.CreatedDate).LastOrDefault()!.Author!.Email,
                    }
                })
                .ToList()
                .OrderByDescending(topic => topic.ArticlesInfo.CreatedDate);
        }
        public IQueryable? GetArticles(GuidType GType, Guid? guid = null, Parameters param = Parameters.Default)
        {
            if (guid == null && GType != GuidType.All) {
                return null;
            }
            return _db.Articles!
                .Where(a =>
                GType == GuidType.Topic ? a.TopicId == guid :
                GType == GuidType.User ? a.AuthorId == guid :
                GType == GuidType.Article ? a.Id == guid :
                                            a.Id == a.Id)
                .Select(A => new
                {
                    Id = A.Id,
                    Text = A.Text,
                    CreatedDate = A.CreatedDate,
                    ReplyId = A.ReplyId,
                    Topic = new
                    {
                        Id = A.Topic!.Id,
                        Title = A.Topic.Title,
                        Descrtiption = A.Topic.Descrtiption,
                        Culture = A.Topic.Culture,
                        Author = A.Topic.Author == null ? null : new
                        {
                            ID = A.Topic.Author!.ID,
                            Login = A.Topic.Author.Login,
                            RealName = A.Topic.Author.RealName,
                            Avatar = A.Topic.Author.Avatar,
                            Email = A.Topic.Author.Email
                        }
                    },
                    Author = A.Author == null ? null : new
                    {
                        ID = A.Author!.ID,
                        Login = A.Author.Login,
                        RealName = A.Author.RealName,
                        Avatar = A.Author.Avatar,
                        Email = A.Author.Email
                    },
                    Status = A.StatusJournal == null ? null
                        : A.StatusJournal!
                            .OrderByDescending(S => S.OperationDate)
                            .FirstOrDefault()!,
                    Replys = A.Replys! == null ? null : A.Replys!.GetReplys()
                })
                .Where(param == Parameters.Deleted
                       ? (A => (A.Status!.IsDeleted == true))
                       : (A => (A.Status == null || A.Status.IsDeleted == false) && A.ReplyId == null));
        }
    }
}
