using MediatR;

namespace InTime.Features.Employee.GetById
{
    public record GetEmployeeByIdQuery(string Id) : IRequest<EmployeeDto>;

}
