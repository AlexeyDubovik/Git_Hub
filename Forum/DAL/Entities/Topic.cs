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
        public Guid AuthorId { get; set; }
    }
}
