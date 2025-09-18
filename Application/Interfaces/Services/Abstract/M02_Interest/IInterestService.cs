using SocialOffice.Application.DTOs.M03_Interest;
using SocialOffice.Domain.Utilities.Results;

namespace SocialOffice.Application.Interfaces.Services.Abstract.M02_Interest
{
    public interface IInterestService
    {
        Task<IDataResult<IEnumerable<InterestDto>>> GetAllAsync();
        Task<IDataResult<InterestDto>> GetByIdAsync(Guid id);
        Task<IResult> CreateAsync(InterestCreateDto dto);
        Task<IResult> DeleteAsync(InterestDeleteDto dto);
    }
}