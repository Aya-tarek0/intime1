using MediatR;

namespace InTime.Features.Employee.DeleteEmployee
{
    public record DeleteEmployeeCommand(string Id) : IRequest<bool>;

}
