using Lego_api_data;
using Lego_api_data.Helpers;
using Lego_api_data.Models;
using Lego_api_data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lego_api.Extensions
{
    public static class ORMExtensions
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LegoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("LegoDatabase")));

            services.AddScoped<IRepository<Theme>, ThemesRepository>();
            services.AddScoped<IRepository<Set>, SetsRepository>();
        }
    }
}
