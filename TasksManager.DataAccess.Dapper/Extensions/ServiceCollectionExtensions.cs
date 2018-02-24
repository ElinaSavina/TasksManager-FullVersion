using Microsoft.Extensions.DependencyInjection;
using TasksManager.DataAccess.Dapper.Projects;
using TasksManager.DataAccess.Dapper.Tasks;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.DataAccess.Interfaces.Tasks;

namespace TasksManager.DataAccess.Dapper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDapperDataAccess(this IServiceCollection services, string connectionString)
        {
            services
                .AddScoped<IConnectionFactory, SqlConnectionFactory>(
                provider => new SqlConnectionFactory(connectionString))

                .AddScoped<IProjectQuery, ProjectQuery>()
                .AddScoped<IProjectsListQuery, ProjectsListQuery>()
                .AddScoped<ICreateProjectCommand, CreateProjectCommand>()
                .AddScoped<IUpdateProjectCommand, UpdateProjectCommand>()
                .AddScoped<IDeleteProjectCommand, DeleteProjectCommand>()

                .AddScoped<ICreateTaskCommand, CreateTaskCommand>()
                .AddScoped<IDeleteTaskCommand, DeleteTaskCommand>();

            return services;
        }
    }
}
