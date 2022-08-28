using Forum.Services;
using Microsoft.EntityFrameworkCore;

namespace Forum.DAL.Context
{
    public class IntroContext : DbContext
    {
        public DbSet<Entities.User>? Users { get; set; }
        public DbSet<Entities.Topic>? Topics { get; set; }
        public DbSet<Entities.Article>? Articles { get; set; }
        public DbSet<Entities.DeleteArticleStatus>? SatatusJournal { get; set; }
        public IntroContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Вызывается когда создается модель
            //БД из кода. Здесь можно задать начальные настройки
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }      
    }
}
