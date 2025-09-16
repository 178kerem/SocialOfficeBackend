using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialOffice.Application.DTOs.M01_UserManagement;
using SocialOffice.Application.DTOs.M02_UserMovements;
using SocialOffice.Application.Interfaces.Persistence.M01_User;
using SocialOffice.Application.Interfaces.Services.Abstract.M01_User;
using SocialOffice.Domain.Entitites.M01_User;
using SocialOffice.Domain.Utilities.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace SocialOffice.Application.Interfaces.Services.Concrete.M01_User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        IPasswordHasher<User> passwordHasher,
        IConfiguration configuration) // <-- burayı ekle
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); // <-- burayı ekle
        }

        public async Task<IDataResult<UserDto>> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return new DataResult<UserDto>
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("User", "User not found.") },
                    Data = null
                };

            var dto = _mapper.Map<UserDto>(user);
            return new DataResult<UserDto>
            {
                IsSuccess = true,
                Messages = new List<KeyValuePair<string, string>>(),
                Data = dto
            };
        }

        public async Task<IDataResult<UserDto>> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user is null)
                return new DataResult<UserDto>
                {
                    IsSuccess = false,
                    Messages = new List<KeyValuePair<string, string>> { new("User", "User not found.") },
                    Data = null
                };

            var dto = _mapper.Map<UserDto>(user);
            return new DataResult<UserDto>
            {
                IsSuccess = true,
                Messages = new List<KeyValuePair<string, string>>(),
                Data = dto
            };
        }

        public async Task<IDataResult<IEnumerable<UserDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<UserDto>>(users);

            return new DataResult<IEnumerable<UserDto>>
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
        public async Task<IDataResult<UserLoginResultDto>> LoginAsync(UserLoginDto loginDto)
        {
            Console.WriteLine("LoginAsync started");

            if (loginDto == null)
            {
                Console.WriteLine("loginDto is null");
                throw new ArgumentNullException(nameof(loginDto));
            }

            Console.WriteLine($"loginDto.Email: {loginDto.Email}");
            Console.WriteLine($"loginDto.Password: {(string.IsNullOrEmpty(loginDto.Password) ? "empty" : "provided")}");

            if (_userRepository == null)
            {
                Console.WriteLine("_userRepository is null");
                throw new InvalidOperationException("_userRepository is not injected");
            }

            if (_configuration == null)
            {
                Console.WriteLine("_configuration is null");
                throw new InvalidOperationException("_configuration is not injected");
            }

            // Kullanıcıyı e-mail ile al
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                Console.WriteLine($"User not found for email: {loginDto.Email}");
                return new DataResult<UserLoginResultDto>(false, "UserNotFound", "Kullanıcı bulunamadı");
            }

            Console.WriteLine($"Found user: {user.Email}");

            // PasswordHash kontrolü
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                Console.WriteLine($"PasswordHash is null or empty for user: {user.Email}");
                return new DataResult<UserLoginResultDto>(false, "NoPasswordHash", "Kullanıcının şifresi bulunamadı");
            }

            // Şifre kontrolü
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password ?? "", user.PasswordHash))
            {
                Console.WriteLine($"Invalid password for user: {user.Email}");
                return new DataResult<UserLoginResultDto>(false, "InvalidPassword", "Şifre hatalı");
            }

            // JWT key kontrolü
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                Console.WriteLine("JWT key is missing in configuration");
                throw new InvalidOperationException("JWT Key configuration is missing");
            }

            var key = Encoding.UTF8.GetBytes(keyString);

            var expireMinutes = 60;
            var expireStr = _configuration["Jwt:ExpireMinutes"];
            if (!string.IsNullOrEmpty(expireStr) && int.TryParse(expireStr, out var parsed))
                expireMinutes = parsed;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User")
        }),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                Issuer = _configuration["Jwt:Issuer"] ?? "defaultIssuer",
                Audience = _configuration["Jwt:Audience"] ?? "defaultAudience",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var resultDto = new UserLoginResultDto
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Token = tokenString
            };

            Console.WriteLine($"Login successful for user: {user.Email}, Token length: {tokenString.Length}");

            return new DataResult<UserLoginResultDto>(resultDto, true, "Success", "Başarılı giriş");
        }
    }
    }
