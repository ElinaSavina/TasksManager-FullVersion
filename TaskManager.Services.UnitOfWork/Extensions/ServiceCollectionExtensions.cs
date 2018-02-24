using Microsoft.Extensions.DependencyInjection;
using TaskManager.Services.GeoObjects;
using TaskManager.Services.Groups;
using TaskManager.Services.UnitOfWork.GeoObjects;
using TaskManager.Services.UnitOfWork.Group;

namespace TaskManager.Services.UnitOfWork.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServicesUnitOfWork(this IServiceCollection services)
        {
            services
                .AddScoped<IGroupService, GroupService>()
                .AddScoped<IGeoObjectsService, GeoObjectsService>();
            return services;
        }
    }
}
