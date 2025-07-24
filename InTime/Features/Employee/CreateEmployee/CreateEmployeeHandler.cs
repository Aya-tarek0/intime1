using InTime.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InTime.Features.Employee.CreateEmployee
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, Guid>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateEmployeeHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var claims = _httpContextAccessor.HttpContext?.User.Claims;

            if (claims is null)
                throw new UnauthorizedAccessException("No claims found in context.");

            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }

            // Get TenantId from token claims
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
      .FirstOrDefault(c => c.Type == "http://schemas.intime.com/claims/tenantId");




            if (tenantIdClaim is null)
                throw new UnauthorizedAccessException("TenantId not found in token claims.");

            if (!Guid.TryParse(tenantIdClaim.Value, out var tenantId))
                throw new Exception("Invalid tenantId format in token.");

            // Create employee user
            var employee = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
                TenantId = tenantId,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(employee, request.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(employee, "Employee");

            return Guid.Parse(employee.Id);
        }
    }
}