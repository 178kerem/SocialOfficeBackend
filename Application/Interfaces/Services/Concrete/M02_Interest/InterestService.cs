using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialOffice.Application.DTOs.M03_Interest;
using SocialOffice.Application.Interfaces.Services.Abstract.M02_Interest;
using SocialOffice.Domain.Entitites.M02_Interest;
using SocialOffice.Domain.Utilities.Results;
using SocialOffice.Application.Interfaces.Persistence.M02_Interest;
namespace SocialOffice.Application.Interfaces.Services.Concrete.M02_Interest
{
    public class InterestService : IInterestService
    {
        private readonly IInterestRepository _interestRepository;
        private readonly IMapper _mapper;

        public InterestService(IInterestRepository interestRepository, IMapper mapper)
        {
            _interestRepository = interestRepository ?? throw new ArgumentNullException(nameof(interestRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IDataResult<IEnumerable<InterestDto>>> GetAllAsync()
        {
            var interests = await _interestRepository.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<InterestDto>>(interests);

            return new DataResult<IEnumerable<InterestDto>>
            {
                IsSuccess = true,
                Messages = new List<KeyValuePair<string, string>>(),
                Data = dtoList
            };
        }

        public async Task<IDataResult<InterestDto>> GetByIdAsync(Guid id)
        {
            var interest = await _interestRepository.GetByIdAsync(id);
            if (interest == null)
                return new DataResult<InterestDto>
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("Interest", "Interest not found.") },
                    Data = null
                };

            var dto = _mapper.Map<InterestDto>(interest);
            return new DataResult<InterestDto>
            {
                IsSuccess = true,
                Messages = new List<KeyValuePair<string, string>>(),
                Data = dto
            };
        }

        public async Task<IResult> CreateAsync(InterestCreateDto dto)
        {
            var interest = _mapper.Map<Interest>(dto);

            try
            {
                await _interestRepository.AddAsync(interest);
                return new Result
                {
                    IsSuccess = true,
                    Messages = new List<KeyValuePair<string, string>> { new("Interest", "Interest created successfully.") }
                };
            }
            catch (DbUpdateException ex)
            {
                return new Result
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("Database", ex.Message) }
                };
            }
        }

        public async Task<IResult> DeleteAsync(InterestDeleteDto dto)
        {
            var interest = await _interestRepository.GetByIdAsync(dto.Id);
            if (interest == null)
                return new Result
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("Interest", "Interest not found.") }
                };

            try
            {
                await _interestRepository.DeleteAsync(interest);
                return new Result
                {
                    IsSuccess = true,
                    Messages = new List<KeyValuePair<string, string>> { new("Interest", "Interest deleted successfully.") }
                };
            }
            catch (DbUpdateException ex)
            {
                return new Result
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("Database", ex.Message) }
                };
            }
        }
    }
}
