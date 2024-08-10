using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Posts.API.Models.DTO;
using Posts.API.Services;

namespace Posts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly IPostService _postService;

        public PostController(ILogger<PostController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<PostDTO> posts = (await _postService.GetAsync()).ToList();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound(); // Return 404 if post is not found
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDTO createPost)
        {
            if (createPost == null)
            {
                return BadRequest("Post data cannot be null.");
            }

            try
            {
                var createdPost = await _postService.CreateAsync(createPost);
                return CreatedAtAction(nameof(GetById), new { id = createdPost.Id }, createdPost);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Return 400 if there are validation issues
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the post.");
                return StatusCode(500, "An error occurred while creating the post.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePostDTO updatePost)
        {
            if (updatePost == null)
            {
                return BadRequest("Post data cannot be null.");
            }

            try
            {
                var result = await _postService.UpdateAsync(id, updatePost);
                if (!result)
                {
                    return NotFound(); // Return 404 if post to update is not found
                }

                return NoContent(); // Return 204 when the update is successful
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Return 400 if there are validation issues
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the post.");
                return StatusCode(500, "An error occurred while updating the post.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _postService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(); // Return 404 if post to delete is not found
                }

                return NoContent(); // Return 204 when the delete is successful
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the post.");
                return StatusCode(500, "An error occurred while deleting the post.");
            }
        }
    }
}
