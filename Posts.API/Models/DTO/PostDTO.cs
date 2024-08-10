namespace Posts.API.Models.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Excerpt { get; set; }
        public string Slug { get; set; }
        public string? FeaturedImageUrl { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
