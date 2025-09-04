using SocialOffice.Application.DTOs.M01_UserManagement;

using SocialOffice.Domain.Utilities.Results;
namespace SocialOffice.Application.Interfaces.Services.Abstract.M01_User
{
    public interface IUserService
    {

        Task<IDataResult<UserReadDto>> GetByIdAsync(Guid id);
        Task<IDataResult<UserReadDto>> GetByEmailAsync(string email);
        Task<IDataResult<IEnumerable<UserReadDto>>> GetAllAsync();
        Task<IResult> CreateAsync(UserCreateDto dto);
        Task<IResult> UpdateAsync(UserUpdateDto dto);
        Task<IResult> DeleteAsync(Guid id);

    }
}

