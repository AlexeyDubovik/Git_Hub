using Microsoft.EntityFrameworkCore;

namespace Forum.DAO
{
    public class CheckUserEmail: UserCheck
    {
        public CheckUserEmail(DbSet<DAL.Entities.User> Users) : base(Users) { }
        public override bool Check(String email)
        {
            var UserforLogin = Users.Where(u => u.Email == email).FirstOrDefault();
            if (UserforLogin != null)
                return true;
            return false;
        }
    }
}
