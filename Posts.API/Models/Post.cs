namespace Posts.API.Models
{
    public class Post
    {
        public int Id {  get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Excerpt { get; set; }
        public string Slug {  get; set; }
        public string? FeaturedImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
