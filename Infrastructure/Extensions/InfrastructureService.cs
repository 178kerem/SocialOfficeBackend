using SocialOffice.Application.Interfaces.Persistence.M01_User;
using SocialOffice.Infrastructure.Persistence.Repositories.M01_User;
using Microsoft.Extensions.DependencyInjection;

namespace SocialOffice.Infrastructure.Extensions;

public static class InfrastructureService
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        // Repository DI
        
        services.AddScoped<IUserRepository, UserRepository>();
       
        
        return services;
    }
}
