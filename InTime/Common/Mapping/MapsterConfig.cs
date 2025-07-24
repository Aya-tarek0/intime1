using InTime.Domain.Entities;
using InTime.Features.RegisterCompany;
using Mapster;

namespace InTime.Common.Mapping
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterCompanyCommand, Tenant>()
                  .Map(dest => dest.Name, src => src.CompanyName)
                  .Map(dest => dest.Email, src => src.AdminEmail);

            config.NewConfig<RegisterCompanyCommand, ApplicationUser>()
                  .Map(dest => dest.Email, src => src.AdminEmail)
                  .Map(dest => dest.UserName, src => src.AdminEmail)
                  .Map(dest => dest.FullName, src => src.AdminFullName);
        }
    }
}
