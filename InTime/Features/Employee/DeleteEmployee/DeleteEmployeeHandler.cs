using InTime.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InTime.Features.Employee.DeleteEmployee
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteEmployeeHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.intime.com/claims/tenantId");

            if (tenantIdClaim is null)
                throw new UnauthorizedAccessException("TenantId not found in token claims.");

            if (!Guid.TryParse(tenantIdClaim.Value, out var tenantId))
                throw new Exception("Invalid tenantId format in token.");

            var employee = await _userManager.FindByIdAsync(request.Id.ToString());

            if (employee is null || employee.TenantId != tenantId)
                throw new KeyNotFoundException("Employee not found or unauthorized.");

            var result = await _userManager.DeleteAsync(employee);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return true;
        }
    }

}
