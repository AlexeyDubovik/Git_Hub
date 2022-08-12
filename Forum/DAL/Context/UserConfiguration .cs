using Forum.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.DAL.Context
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Начальная конфигурация построение модели
            //Делигируется из контекста
            // можно задать значение полей,
            // поменять имя таблицы (по умолчанию - имя класса)
            // а также задать начальные данные для таблицы (seed)
            builder.HasData(new User
            {
                ID = Guid.NewGuid(),
                RealName = "Admin",
                Login = "Admin",
                PassHash = "default",
                PassSalt = "default",
                Email = "default",
                RegMoment = DateTime.Now,
                Avatar = "default"
            });
        }
    }
}
