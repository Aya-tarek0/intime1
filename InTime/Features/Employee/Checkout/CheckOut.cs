using System.Security.Claims;
using InTime.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InTime.Features.Employee.Checkout
{
    public class CheckOutCommand : IRequest<Unit>
    {
    }

    public class CheckOutHandler : IRequestHandler<CheckOutCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CheckOutHandler(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(CheckOutCommand request, CancellationToken cancellationToken )
        {
            var today = DateTime.UtcNow.Date;
            var UserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);


            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.UserId == UserId&& a.Date == today, cancellationToken);

            if (attendance == null)
                throw new Exception("No check-in record found for today.");

            if (attendance.CheckOutTime.HasValue)
                throw new Exception("Already checked out for today.");

            attendance.CheckOutTime = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
