using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGC.Infrastructure.Repositories;
//using Microsoft.EntityFrameworkCore;

namespace SGC.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITableRepository, SqlServerTableRepository>();
        }
    }
}