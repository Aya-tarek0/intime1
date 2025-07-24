using FluentValidation;

namespace InTime.Features.RegisterCompany
{
    public class RegisterCompanyValidator : AbstractValidator<RegisterCompanyCommand>
    {
        public RegisterCompanyValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty();
            RuleFor(x => x.AdminFullName).NotEmpty();
            RuleFor(x => x.AdminEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.AdminPassword).NotEmpty().MinimumLength(6);
        }
    }
}
