using Microsoft.Extensions.DependencyInjection;
using SocialOffice.Application.Interfaces.Persistence.M01_User;
using SocialOffice.Application.Interfaces.Persistence.M02_Interest;
using SocialOffice.Application.Interfaces.Services.Abstract.M02_Interest;
using SocialOffice.Application.Interfaces.Services.Concrete.M02_Interest;
using SocialOffice.Infrastructure.Persistence.Repositories.M01_User;
using SocialOffice.Infrastructure.Persistence.Repositories.M02_Interest;

namespace SocialOffice.Infrastructure.Extensions;

public static class InfrastructureService
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        // Repository DI
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IInterestRepository, InterestRepository>();

        return services;
    }
}
