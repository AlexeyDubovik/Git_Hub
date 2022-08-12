using Microsoft.EntityFrameworkCore;

namespace Forum.DAO
{
    public abstract class UserCheck 
    {
        public DbSet<DAL.Entities.User> Users { get; }
        public UserCheck(DbSet<DAL.Entities.User> Users)
        {
            this.Users = Users;
        }
        public abstract bool Check(String str);
    }
}
