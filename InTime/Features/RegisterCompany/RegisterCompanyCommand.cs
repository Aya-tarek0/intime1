using MediatR;

namespace InTime.Features.RegisterCompany
{
    public class RegisterCompanyCommand : IRequest<Guid>
    {
        public string CompanyName { get; set; } = string.Empty;
        public string AdminFullName { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
    }
}
