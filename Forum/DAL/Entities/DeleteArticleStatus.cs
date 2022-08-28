using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.DAL.Entities
{
    public record DeleteArticleStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid ArticleID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime OperationDate { get; set; }
    }
}
