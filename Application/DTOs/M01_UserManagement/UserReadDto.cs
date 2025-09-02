namespace SocialOffice.Application.DTOs.M01_UserManagement;
public record UserReadDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string RoleName,
    DateTime? LastLoginAt
);

