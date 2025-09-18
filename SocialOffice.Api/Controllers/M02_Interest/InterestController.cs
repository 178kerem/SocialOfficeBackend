using Microsoft.AspNetCore.Mvc;
using SocialOffice.Application.DTOs.M03_Interest;
using SocialOffice.Application.Interfaces.Services.Abstract.M02_Interest;

namespace SocialOffice.Api.Controllers.M02_Interest
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterestController : ControllerBase
    {
        private readonly IInterestService _interestService;

        public InterestController(IInterestService interestService)
        {
            _interestService = interestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _interestService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _interestService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Messages);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InterestCreateDto dto)
        {
            var result = await _interestService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Messages);

            return Ok(result.Messages);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dto = new InterestDeleteDto { Id = id };
            var result = await _interestService.DeleteAsync(dto);
            if (!result.IsSuccess)
                return NotFound(result.Messages);

            return Ok(result.Messages);
        }
    }
}
