using Forum.Services;
using Microsoft.EntityFrameworkCore;

namespace Forum.DAO
{
    public class Authenticator
    {
        private DbSet<DAL.Entities.User> Users { get; }
        public Authenticator(DbSet<DAL.Entities.User> Users)
        {
            this.Users = Users;
        }
        public Guid? Authenticate(Models.UserModel UserData, IHasher _hasher)
        {
            if (UserData == null)
                return null;
            var UserFromLogin = Users
                .Where(
                u => u.Login.Equals(UserData.Login)
                ).FirstOrDefault();
            if (UserFromLogin != null)
            {
                var Pass = _hasher.Hash(UserData.Password1 + UserFromLogin.PassSalt);
                var UserFromPass = Users.Where(
                    u => u.Login.Equals(UserData.Login) &&
                    u.PassHash.Equals(Pass)).FirstOrDefault();
                if (UserFromPass != null)
                    return UserFromPass.ID;
                return null;
            }
            return null;
        }
    }
}
