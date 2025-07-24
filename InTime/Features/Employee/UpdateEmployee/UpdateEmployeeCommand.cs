using MediatR;

namespace InTime.Features.Employee.UpdateEmployee
{
    public record UpdateEmployeeCommand(
    string Id,
     string FullName,
     string Email,
     string Password
 ) : IRequest<bool>;

}
