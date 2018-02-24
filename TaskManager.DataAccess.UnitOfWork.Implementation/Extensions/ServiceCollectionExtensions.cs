using Microsoft.Extensions.DependencyInjection;
using TasksManager.Db;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.DataAccess.UnitOfWork.Implementation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterUnitOfWorkEfCore(this IServiceCollection services,
            string connectionString)
        {
            services
                .AddDbContext<TasksContext>(options => options.UseSqlServer(connectionString))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddTransient<IAsyncQueryableFactory, AsyncQueryableFactory>();

            return services;
        }
    }
}
