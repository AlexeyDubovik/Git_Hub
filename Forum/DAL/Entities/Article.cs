using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.DAL.Entities
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public Topic? Topic { get; set; }
        public string Text { get; set; } = "default";
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }
        public Guid? ReplyId { get; set; }
        public Article? Reply { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
