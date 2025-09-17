namespace SocialOffice.Application.DTOs.M02_UserMovements;
public class UserLoginResultDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty; // JWT eklenecek
    public string Type { get; set; } = string.Empty;
}

