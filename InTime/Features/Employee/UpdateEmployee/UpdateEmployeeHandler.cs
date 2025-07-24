using InTime.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InTime.Features.Employee.UpdateEmployee
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateEmployeeHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.intime.com/claims/tenantId");

            if (tenantIdClaim is null)
                throw new UnauthorizedAccessException("TenantId not found in token claims.");

            if (!Guid.TryParse(tenantIdClaim.Value, out var tenantId))
                throw new Exception("Invalid tenantId format in token.");

            var employee = await _userManager.FindByIdAsync(request.Id.ToString());

            if (employee == null || employee.TenantId != tenantId)
                throw new KeyNotFoundException("Employee not found or unauthorized.");

            employee.FullName = request.FullName;
            employee.Email = request.Email;
            employee.UserName = request.Email;

            var updateResult = await _userManager.UpdateAsync(employee);
            if (!updateResult.Succeeded)
                throw new Exception(string.Join(", ", updateResult.Errors.Select(e => e.Description)));

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(employee);
                var passResult = await _userManager.ResetPasswordAsync(employee, token, request.Password);
                if (!passResult.Succeeded)
                    throw new Exception(string.Join(", ", passResult.Errors.Select(e => e.Description)));
            }

            return true;
        }
    }

}
