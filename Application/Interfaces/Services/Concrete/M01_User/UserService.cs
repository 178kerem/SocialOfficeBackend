using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialOffice.Application.DTOs.M01_UserManagement;
using SocialOffice.Application.Interfaces.Persistence.M01_User;
using SocialOffice.Application.Interfaces.Services.Abstract.M01_User;
using SocialOffice.Domain.Entitites.M01_User;
using SocialOffice.Domain.Utilities.Results;
namespace SocialOffice.Application.Interfaces.Services.Concrete.M01_User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<UserReadDto>> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return new DataResult<UserReadDto>
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("User", "User not found.") },
                    Data = null
                };

            var dto = _mapper.Map<UserReadDto>(user);
            return new DataResult<UserReadDto>
            {
                IsSuccess = true,
                Messages = new List<KeyValuePair<string, string>>(),
                Data = dto
            };
        }

        public async Task<IDataResult<UserReadDto>> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user is null)
                return new DataResult<UserReadDto>
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("User", "User not found.") },
                    Data = null
                };

            var dto = _mapper.Map<UserReadDto>(user);
            return new DataResult<UserReadDto>
            {
                IsSuccess = true,
                Messages = new List<KeyValuePair<string, string>>(),
                Data = dto
            };
        }

        public async Task<IDataResult<IEnumerable<UserReadDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<UserReadDto>>(users);

            return new DataResult<IEnumerable<UserReadDto>>
            {
                IsSuccess = true,
                Messages = new List<KeyValuePair<string, string>>(),
                Data = dtoList
            };
        }

        public async Task<IResult> CreateAsync(UserCreateDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                return new Result
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("Email", "This email address is already in use.") }
                };

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            try
            {
                await _userRepository.AddAsync(user);
                return new Result
                {
                    IsSuccess = true,
                    Messages = new List<KeyValuePair<string, string>> { new("User", "User created successfully.") }
                };
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;

                var errors = new List<KeyValuePair<string, string>>();

                if (message.Contains("IX_Users_Email"))
                    errors.Add(new("Email", "This email address is already registered."));
                else if (message.Contains("FK_Users_Role"))
                    errors.Add(new("Role", "Invalid role."));
                else if (message.Contains("violates not-null constraint"))
                    errors.Add(new("General", "Missing required fields."));
                else if (message.Contains("violates foreign key constraint"))
                    errors.Add(new("General", "Referenced entity does not exist."));
                else
                    errors.Add(new("General", "A database error occurred while creating user."));

                return new Result
                {
                    IsSuccess = false,
                    Messages = errors
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("General", ex.Message) }
                };
            }
        }

        public async Task<IResult> UpdateAsync(UserUpdateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.Id);
            if (user is null)
                return new Result
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("User", "User not found.") }
                };

            if (!string.Equals(user.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                    return new Result
                    {
                        IsSuccess = false,
                        Messages = new List<KeyValuePair<string, string>> { new("Email", "This email address is already in use by another user.") }
                    };
            }

            _mapper.Map(dto, user);

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            try
            {
                await _userRepository.UpdateAsync(user);
                return new Result
                {
                    IsSuccess = true,
                    Messages = new List<KeyValuePair<string, string>> { new("User", "User updated successfully.") }
                };
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;

                var errors = new List<KeyValuePair<string, string>>();

                if (message.Contains("IX_Users_Email"))
                    errors.Add(new("Email", "This email address is already registered."));
                else if (message.Contains("FK_Users_Role"))
                    errors.Add(new("Role", "Invalid role."));
                else if (message.Contains("violates not-null constraint"))
                    errors.Add(new("General", "Missing required fields."));
                else if (message.Contains("violates foreign key constraint"))
                    errors.Add(new("General", "Referenced entity does not exist."));
                else
                    errors.Add(new("General", "A database error occurred while updating user."));

                return new Result
                {
                    IsSuccess = false,
                    Messages = errors
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("General", ex.Message) }
                };
            }
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return new Result
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("User", "User not found.") }
                };

            try
            {
                await _userRepository.DeleteAsync(user);
                return new Result
                {
                    IsSuccess = true,
                    Messages = new List<KeyValuePair<string, string>> { new("User", "User deleted successfully.") }
                };
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;

                var errors = new List<KeyValuePair<string, string>>();

                if (message.Contains("violates foreign key constraint"))
                    errors.Add(new("General", "Cannot delete this user because it is referenced by other records."));
                else
                    errors.Add(new("General", "A database error occurred while deleting user."));

                return new Result
                {
                    IsSuccess = false,
                    Messages = errors
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("General", ex.Message) }
                };
            }
        }
    }
}
