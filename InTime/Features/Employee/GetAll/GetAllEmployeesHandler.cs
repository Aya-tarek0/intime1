using System.Linq;
using InTime.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InTime.Features.Employee.GetAll
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    // Handler
    public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, PaginatedResult<EmployeeDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetAllEmployeesHandler> _logger;

        public GetAllEmployeesHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<GetAllEmployeesHandler> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<PaginatedResult<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.intime.com/claims/tenantId");

            if (tenantIdClaim is null || !Guid.TryParse(tenantIdClaim.Value, out var tenantId))
                throw new UnauthorizedAccessException("TenantId missing or invalid");

            var query = _context.Users
                .AsNoTracking()
                .Where(u => u.TenantId == tenantId);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(search) || u.Email.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(u => u.FullName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(u => new EmployeeDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email!
                })
                .ToListAsync(cancellationToken);


            return new PaginatedResult<EmployeeDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }

}
