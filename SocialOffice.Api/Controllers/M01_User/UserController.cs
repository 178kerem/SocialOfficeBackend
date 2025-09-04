using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using SocialOffice.Application.DTOs.M01_UserManagement;
using SocialOffice.Application.Interfaces.Services.Abstract.M01_User;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
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

        

    }
}