using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.DAL.Entities
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; } = "default";
        public string Descrtiption { get; set; } = "default";
        public string Culture { get; set; } = "en-US";
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }
        public List<Article>? articles { get; set; }
    }
}
