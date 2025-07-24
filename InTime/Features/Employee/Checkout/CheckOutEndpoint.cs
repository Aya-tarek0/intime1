using MediatR;

namespace InTime.Features.Employee.Checkout
{
    public static class CheckOutEndpoint
    {
        public static void MapCheckOutEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/attendance/checkout", async (
                CheckOutCommand command,
                ISender sender) =>
            {
                await sender.Send(command);
                return Results.Ok("Checked out successfully.");
            })
            .RequireAuthorization();
        }
    }

}
