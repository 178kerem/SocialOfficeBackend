namespace SocialOffice.Application.DTOs.M01_UserManagement;

public record UserCreateDto(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Phone
///Guid RoleId
);
