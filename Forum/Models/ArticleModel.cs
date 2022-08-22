namespace Forum.Models
{
    public class ArticleModel
    {
        public string? Text { get; set; }
        public Guid TopicId { get; set; }
        public Guid? ReplyId { get; set; }
        //public IFormFile? PictureFile { get; set; }
    }
}
