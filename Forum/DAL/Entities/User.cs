using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.DAL.Entities
{
    public record User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public string RealName { get; set; } = "default";
        public string Login { get; set; } = "default";
        public string PassHash { get; set; } = "default";
        public string PassSalt { get; set; } = "default";
        public string Email { get; set; } = "default";
        public string Avatar { get; set; } = "default";
        public DateTime RegMoment { get; set; } = DateTime.Now;
        public DateTime? LogMoment { get; set; }
        public List<Article>? articles { get; set; }
    }
}
