using MediatR;

namespace InTime.Features.Employee.CheckIn
{
    public record CheckInCommand : IRequest<Guid>;

}
