using MediatR;

namespace InTime.Features.Employee.GetAll
{
    public record GetAllEmployeesQuery(string? Search, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<EmployeeDto>>;

}
