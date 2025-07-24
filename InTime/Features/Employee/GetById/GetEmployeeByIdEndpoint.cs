using MediatR;

namespace InTime.Features.Employee.GetById
{
    public static class GetEmployeeByIdEndpoint
    {
        public static IEndpointRouteBuilder MapGetEmployeeByIdEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/employees/{id}", async (string id, ISender sender) =>
            {
                var result = await sender.Send(new GetEmployeeByIdQuery(id));
                return Results.Ok(result);
            })
            .RequireAuthorization() 
            .WithName("GetEmployeeById")
            .WithTags("Employees");

            return app;
        }
    }

}
