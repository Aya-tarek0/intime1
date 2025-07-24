using InTime.Data;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace InTime.Features.Employee.GetById
{
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GetEmployeeByIdHandler> _logger;

        public GetEmployeeByIdHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IMemoryCache cache, ILogger<GetEmployeeByIdHandler> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
.FirstOrDefault(c => c.Type == "http://schemas.intime.com/claims/tenantId");

            if (string.IsNullOrEmpty(tenantIdClaim.Value) || !Guid.TryParse(tenantIdClaim.Value, out var tenantId))
                throw new UnauthorizedAccessException("TenantId not found in token.");

            string cacheKey = $"employee_{request.Id}_{tenantId}";

            if (_cache.TryGetValue(cacheKey, out EmployeeDto cachedEmployee))
            {
                _logger.LogInformation("Returned employee {Id} from cache", request.Id);
                return cachedEmployee;
            }

            var employee = await _context.Users.AsNoTracking()
               
                .Where(x => x.Id == request.Id && x.TenantId == tenantId)
                .FirstOrDefaultAsync(cancellationToken);

            if (employee is null)
            {
                _logger.LogWarning("Employee {Id} not found for tenant {TenantId}", request.Id, tenantId);
                throw new KeyNotFoundException("Employee not found");
            }

            var dto = _mapper.Map<EmployeeDto>(employee);

            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(5));

            _logger.LogInformation("Fetched employee {Id} for tenant {TenantId}", request.Id, tenantId);

            return dto;
        }
    }

}
