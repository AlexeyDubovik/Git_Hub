using Microsoft.EntityFrameworkCore;

namespace Forum.DAO
{
    public class CheckUserLogin : UserCheck
    {
        public CheckUserLogin(DbSet<DAL.Entities.User> Users) : base(Users) { }
        public override bool Check(String login)
        {
            var UserforLogin = Users.Where(u => u.Login == login).FirstOrDefault();
            if (UserforLogin != null)
                return true;
            return false;
        }
    }
}
