using SocialOffice.Application.DTOs.M01_UserManagement;
using SocialOffice.Application.DTOs.M02_UserMovements;
using SocialOffice.Domain.Utilities.Results;
namespace SocialOffice.Application.Interfaces.Services.Abstract.M01_User
{
    public interface IUserService
    {

        Task<IDataResult<UserDto>> GetByIdAsync(Guid id);
        Task<IDataResult<UserDto>> GetByEmailAsync(string email);
        Task<IDataResult<IEnumerable<UserDto>>> GetAllAsync();
        Task<IResult> CreateAsync(UserCreateDto dto);
        Task<IResult> UpdateAsync(UserUpdateDto dto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<UserLoginResultDto>> LoginAsync(UserLoginDto loginDto);
    }
}

