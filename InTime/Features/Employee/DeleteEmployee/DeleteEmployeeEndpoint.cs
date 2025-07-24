using MediatR;

namespace InTime.Features.Employee.DeleteEmployee
{
    public static class DeleteEmployeeEndpoint
    {
        public static void MapDeleteEmployeeEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/employees/{id}", async (
                string id,
                ISender sender) =>
            {
                var result = await sender.Send(new DeleteEmployeeCommand(id));
                return Results.Ok(result);
            })
            .RequireAuthorization();
        }
    }

}
