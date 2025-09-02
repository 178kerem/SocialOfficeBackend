namespace SocialOffice.Application.DTOs.M01_UserManagement;
public record UserUpdateDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Phone
///Guid RoleId
);
