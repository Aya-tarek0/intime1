using MediatR;

namespace InTime.Features.Employee.UpdateEmployee
{
    public static class UpdateEmployeeEndpoint
    {
        public static void MapUpdateEmployeeEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/employees/{id}", async (
                string id,
                UpdateEmployeeRequest request,
                ISender sender) =>
            {
                var command = new UpdateEmployeeCommand(
                    Id: id,
                    FullName: request.FullName,
                    Email: request.Email,
                    Password: request.Password
                );

                var result = await sender.Send(command);
                return Results.Ok(result);
            })
            .RequireAuthorization();
        }
    }

    public class UpdateEmployeeRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
