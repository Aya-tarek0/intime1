using MediatR;

namespace InTime.Features.RegisterCompany
{
    public static class RegisterCompanyEndpoint
    {
        public static void MapRegisterCompanyEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/companies/register", async (
                RegisterCompanyCommand command,
                ISender mediator) =>
            {
                var id = await mediator.Send(command);
                return Results.Ok(new { CompanyId = id });
            });
        }
    }
}
