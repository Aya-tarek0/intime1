using InTime.Data;
using InTime.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InTime.Features.RegisterCompany
{
    public class RegisterCompanyHandler : IRequestHandler<RegisterCompanyCommand, Guid>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterCompanyHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Guid> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
        {
            // Create Tenant
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = request.CompanyName,
                Email = request.AdminEmail,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Tenants.AddAsync(tenant, cancellationToken);

            // Create Admin User
            var adminUser = new ApplicationUser
            {
                FullName = request.AdminFullName,
                Email = request.AdminEmail,
                UserName = request.AdminEmail,
                TenantId = tenant.Id,
                EmailConfirmed = true 
            };

            var result = await _userManager.CreateAsync(adminUser, request.AdminPassword);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // Assign Role
            await _userManager.AddToRoleAsync(adminUser, "Admin");

            await _context.SaveChangesAsync(cancellationToken);
            return tenant.Id;
        }
    }
    }
