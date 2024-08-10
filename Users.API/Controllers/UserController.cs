using Microsoft.AspNetCore.Mvc;
using Users.API.Models.DTO;
using Users.API.Services;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserDTO> users = (await _userService.GetAsync()).ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Return 404 if user is not found
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO createUser)
        {
            if (createUser == null)
            {
                return BadRequest("User data cannot be null.");
            }

            try
            {
                var createdUser = await _userService.CreateAsync(createUser);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Return 400 if there are validation issues
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the user.");
                return StatusCode(500, "An error occurred while creating the user.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO updateUser)
        {
            if (updateUser == null)
            {
                return BadRequest("User data cannot be null.");
            }

            try
            {
                var result = await _userService.UpdateAsync(id, updateUser);
                if (!result)
                {
                    return NotFound(); // Return 404 if user to update is not found
                }

                return NoContent(); // Return 204 when the update is successful
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Return 400 if there are validation issues
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the user.");
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(); // Return 404 if user to delete is not found
                }

                return NoContent(); // Return 204 when the delete is successful
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the user.");
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }


    }
}
