using InTime.Data;
using InTime.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InTime.Features.Employee.CheckIn
{
    public class CheckInHandler : IRequestHandler<CheckInCommand, Guid>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckInHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                throw new UnauthorizedAccessException("User not authenticated");

            var today = DateTime.UtcNow.Date;

            var alreadyCheckedIn = await _context.Attendances
                .AnyAsync(a => a.UserId== userId && a.Date == today, cancellationToken);

            if (alreadyCheckedIn)
                throw new InvalidOperationException("Already checked in today.");

            var now = DateTime.UtcNow;
            var status = now.TimeOfDay > TimeSpan.FromHours(9) ? AttendanceStatus.Late : AttendanceStatus.OnTime;

            var attendance = new Attendance
            {
                Id = Guid.NewGuid(),
               UserId= userId,
                Date = today,
                CheckInTime = now,
                Status = status
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync(cancellationToken);

            return attendance.Id;
        }
    }

}
