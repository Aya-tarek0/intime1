using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InTime.Features.LoginUser
{
    public static class LoginUserEndpoint
    {
        public static void MapLoginUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/login", async (
                LoginUserCommand command,
                [FromServices] ISender mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            });
        }
    }
}
