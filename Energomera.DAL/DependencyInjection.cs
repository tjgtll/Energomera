using Energomera.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Energomera.DAL;

public static class DependencyInjection
{
    public static void RegisterDAL(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddScoped<IFieldRepository, FieldRepository>();
    }
}
