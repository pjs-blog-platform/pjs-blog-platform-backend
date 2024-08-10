namespace Posts.API.Models.DTO
{
    public class UpdatePostDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Excerpt { get; set; }
        public string? Slug { get; set; }
        public string? FeaturedImageUrl { get; set; }
    }
}
