using CodeDungeon.Services.Abstract;
using CodeDungeon.Services.Concrete;

namespace CodeDungeon.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
           
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

           
        }
    }
}