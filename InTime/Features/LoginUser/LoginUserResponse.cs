namespace InTime.Features.LoginUser
{
    public class LoginUserResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Guid TenantId { get; set; }
    }
}
