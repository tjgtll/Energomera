using Energomera.BLL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;

namespace Energomera.BLL;

public static class DependencyInjection
{
    public static void RegisterBLL(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddScoped<IFieldService, FieldService>();
        services.AddSingleton<GeometryFactory>();
    }
}
