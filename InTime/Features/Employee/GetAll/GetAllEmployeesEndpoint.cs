using MediatR;

namespace InTime.Features.Employee.GetAll
{
    public static class GetAllEmployeesEndpoint
    {
        public static void MapGetAllEmployeesEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/employees", async (
                [AsParameters] GetAllEmployeesQuery query,
                ISender sender) =>
            {
                var result = await sender.Send(query);
                return Results.Ok(result);
            })
            .RequireAuthorization();
        }
    }

}
