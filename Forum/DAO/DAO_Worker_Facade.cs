using Forum.DAL.Context;
using Forum.DAO;
using System.Text.RegularExpressions;

namespace Forum.Services
{
    public class DAO_Worker_Facade
    {
        public  readonly IntroContext introContext;
        private readonly Authenticator authenticator;
        private readonly RandomServices randomService;
        private readonly IHasher hasher;
        private UserCheck? userCheck;
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
        private void DeleteUserAvatar(String AvatarFileName)
        {
            File.Delete("./wwwroot/img/" + AvatarFileName);
        }
        private String? SaveUserAvatar(IFormFile Avatar)
        {
            if (Avatar == null)
                return null;
            int pos = Avatar.FileName.LastIndexOf('.');
            string IMG_Format = Avatar.FileName.Substring(pos);
            string ImageName = hasher.Hash(Guid.NewGuid().ToString() + hasher.Hash(randomService.Integer.ToString()));
            var file = new FileStream("./wwwroot/img/" + ImageName + IMG_Format, FileMode.Create);
            Avatar.CopyToAsync(file).ContinueWith(f => file.Dispose());
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
                    user.Avatar = SaveUserAvatar(UserData.Avatar)!;
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
            else if (!Regex.IsMatch(Login, @"^[^\s()-]*$") && User != null) {
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
            if (Regex.IsMatch(NewName, @"^[А-ЯA-Z][а-яa-z]+\s[А-ЯA-Z][а-яa-z]+$") && User != null) {
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
            else if (Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") && User != null) {
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
            else if (PassForm.NewPassword2 != null) {
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
                try {
                    DeleteUserAvatar(User.Avatar);
                    User.Avatar = SaveUserAvatar(Avatar)!;
                    introContext.Update(User);
                    introContext.SaveChanges();
                    return "Success";
                }
                catch {
                    return "Error";
                }
        }
    }
}
