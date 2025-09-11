using Microsoft.AspNetCore.Mvc;
using SocialOffice.Application.DTOs.M01_UserManagement;
using SocialOffice.Application.DTOs.M02_UserMovements;
using SocialOffice.Application.Interfaces.Services.Abstract.M01_User;
namespace SocialOffice.Api.Controllers.M01_User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id">User GUID</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Messages);

            return Ok(result.Data);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _userService.GetByEmailAsync(email);
            if (!result.IsSuccess)
                return NotFound(result.Messages);

            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result.Data); // IEnumerable<UserListDto>
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var result = await _userService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Messages);

            return Ok(result.Messages);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserUpdateDto dto)
        {
            var result = await _userService.UpdateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Messages);

            return Ok(result.Messages);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Messages);

            return Ok(result.Messages);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                return BadRequest(new { message = "Email ve şifre gerekli" });

            var result = await _userService.LoginAsync(dto);

            if (!result.IsSuccess)
            {
                // result.Messages array veya string olabilir
                return Unauthorized(new { message = result.Messages });
            }

            // result.Data UserLoginResultDto tipinde
            return Ok(result.Data);
        }

    }
}