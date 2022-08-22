using Forum.DAL.Context;
using Forum.DAO;
using System.Text.RegularExpressions;
namespace Forum.Services
{
    public class DAO_Worker_Facade
    {
        public readonly IntroContext introContext;
        private readonly Authenticator authenticator;
        private readonly RandomServices randomService;
        private readonly IHasher hasher;
        private UserCheck? userCheck;
        public enum GuidType
        {
            All,
            User,
            Topic,
            Article,
        }
        public DAO_Worker_Facade(IntroContext introContext, IHasher hasher, RandomServices randomService)
        {
            this.hasher = hasher;
            this.introContext = introContext;
            this.randomService = randomService;
            authenticator = new Authenticator(introContext.Users!);
        }
        public bool UserDataCheck(string str)
        {
            if (str == null)
                return false;
            if (Regex.IsMatch(str, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                userCheck = new CheckUserEmail(introContext.Users!);
            else
                userCheck = new CheckUserLogin(introContext.Users!);
            return userCheck.Check(str);
        }
        public Guid? Authenticator(Models.RegUserModel UserData)
        {
            return authenticator.Authenticate(UserData, hasher);
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
            string ImageName = hasher.Hash(Guid.NewGuid().ToString() + hasher.Hash(randomService.Integer.ToString()));
            var file = new FileStream(Path + ImageName + IMG_Format, FileMode.Create);
            Picture.CopyToAsync(file).ContinueWith(f => file.Dispose());
            return ImageName + IMG_Format;
        }
        public void CreateUser(Models.RegUserModel UserData)
        {
            if (UserData != null && introContext.Users != null)
            {
                var user = new DAL.Entities.User();
                user.PassSalt = hasher.Hash(randomService.Integer.ToString());
                user.PassHash = hasher.Hash(UserData.Password1 + user.PassSalt);
                if (UserData.Avatar != null)
                    user.Avatar = SavePicture(UserData.Avatar)!;
                else
                    user.Avatar = "default.png";
                user.Email = UserData.Email!;
                user.RealName = UserData.RealName!;
                user.Login = UserData.Login!;
                user.RegMoment = DateTime.Now;
                introContext.Users.Add(user);
                introContext.SaveChanges();
            }
        }
        public String ChangeUserLogin(DAL.Entities.User User, String Login)
        {
            if (UserDataCheck(Login))
                return "Login already in use";
            else if (!Regex.IsMatch(Login, @"^[^\s()-]*$") && User != null)
            {
                User.Login = Login;
                introContext.Update(User);
                introContext.SaveChanges();
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
                introContext.Update(User);
                introContext.SaveChanges();
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
                introContext.Update(User);
                introContext.SaveChanges();
                return "Success";
            }
            else
                return "Error";
        }
        public String ChangeUserPassword(DAL.Entities.User User, Models.ChUserPasswordModel PassForm)
        {
            if (User == null)
                return "Error User is null";
            if (User.PassHash != hasher.Hash(PassForm.OldPassword + User.PassSalt))
                return "Invalid Password";
            else if (PassForm.NewPassword2 != null)
            {
                User.PassSalt = hasher.Hash(randomService.Integer.ToString());
                User.PassHash = hasher.Hash(PassForm.NewPassword2 + User.PassSalt);
                introContext.Update(User);
                introContext.SaveChanges();
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
                    introContext.Update(User);
                    introContext.SaveChanges();
                    return "Success";
                }
                catch
                {
                    return "Error";
                }
        }
        public IQueryable? GetTopics(GuidType GType, Guid? guid = null)
        {
            if (guid == null && GType != GuidType.All) {
                return null;
            }
            return introContext.Topics!
                .Where(t =>
                GType == GuidType.Topic ? t.Id       == guid :
                GType == GuidType.User  ? t.AuthorId == guid :
                                          t.Id       == t.Id)
                .Select(t => new {
                    Id = t.Id,
                    Title = t.Title,
                    Descrtiption = t.Descrtiption,
                    Culture = t.Culture,
                    Author = t.Author == null ? null : new
                    {
                        ID       = t.Author!.ID,
                        Login    = t.Author.Login,
                        RealName = t.Author.RealName,
                        Email    = t.Author.Email,
                        Avatar   = t.Author.Avatar
                    },
                    ArticlesInfo = t.articles!.Count == 0 ?
                    new
                    {
                        Count       = t.articles!.Count,
                        CreatedDate = DateTime.Parse("11.11.1111"),
                        RealName    = "Empty",
                        Login       = "Empty",
                        Email       = "Empty",
                    }
                    :
                    new
                    {
                        Count       = t.articles!.Count,
                        CreatedDate = t.articles!.OrderBy(a => a.CreatedDate).LastOrDefault()!.CreatedDate,
                        RealName    = t.articles!.OrderBy(a => a.CreatedDate).LastOrDefault()!.Author!.RealName,
                        Login       = t.articles!.OrderBy(a => a.CreatedDate).LastOrDefault()!.Author!.Login,
                        Email       = t.articles!.OrderBy(a => a.CreatedDate).LastOrDefault()!.Author!.Email,
                    }
                });
        }
        public IQueryable? GetArticles(GuidType GType, Guid? guid = null)
        {
            if (guid == null && GType != GuidType.All) {
                return null;
            }
            return introContext.Articles!
                .Where(a =>
                GType == GuidType.Topic   ? a.TopicId  == guid :
                GType == GuidType.User    ? a.AuthorId == guid :
                GType == GuidType.Article ? a.Id       == guid :
                                            a.Id       == a.Id)
                .Select(A => new
                {
                    Id          = A.Id,
                    Text        = A.Text,
                    CreatedDate = A.CreatedDate,
                    Reply       = A.Reply == null ? null : new
                    {
                        Id     = A.Reply.Id,
                        Text   = A.Reply.Text,
                        Author = new
                        {
                            ID       = A.Reply.Author!.ID,
                            Login    = A.Reply.Author.Login,
                            RealName = A.Reply.Author.RealName,
                            Avatar   = A.Reply.Author.Avatar,
                            Email    = A.Reply.Author.Email
                        }
                    },
                    Topic = new
                    {
                        Id           = A.Topic!.Id,
                        Title        = A.Topic.Title,
                        Descrtiption = A.Topic.Descrtiption,
                        Culture      = A.Topic,
                        Author = new
                        {
                            ID       = A.Topic.Author!.ID,
                            Login    = A.Topic.Author.Login,
                            RealName = A.Topic.Author.RealName,
                            Avatar   = A.Topic.Author.Avatar,
                            Email    = A.Topic.Author.Email
                        }
                    },
                    Author = new
                    {
                        ID       = A.Author!.ID,
                        Login    = A.Author.Login,
                        RealName = A.Author.RealName,
                        Avatar   = A.Author.Avatar,
                        Email    = A.Author.Email
                    }
                });
            
        }
    }
}
