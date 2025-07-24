using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace InTime.Features.Employee.CreateEmployee
{
    public static class CreateEmployeeEndpoint
    {

        public static void MapCreateEmployeeEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/employees/create", async (
                CreateEmployeeCommand command,
                ISender mediator) =>
            {
                var id = await mediator.Send(command);
                return Results.Ok(new { EmployeeId = id });
            }).RequireAuthorization();
            
        }
    }
}

