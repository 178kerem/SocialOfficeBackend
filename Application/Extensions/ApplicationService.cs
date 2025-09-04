using Microsoft.Extensions.DependencyInjection;
using SocialOffice.Application.Interfaces.Services.Abstract.M01_User;
using SocialOffice.Application.Interfaces.Services.Concrete.M01_User;

namespace SocialOffice.Application.Extensions
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
