using MediatR;

namespace InTime.Features.Employee.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
