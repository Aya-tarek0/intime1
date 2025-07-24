using MediatR;

namespace InTime.Features.Employee.CheckIn
{
    public static class CheckInEndpoint
    {
        public static void MapCheckInEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/attendance/checkin", async (ISender sender) =>
            {
                var result = await sender.Send(new CheckInCommand());
                return Results.Ok(new { Message = "Checked in successfully", AttendanceId = result });
            })
            .RequireAuthorization(); 
        }
    }

}
