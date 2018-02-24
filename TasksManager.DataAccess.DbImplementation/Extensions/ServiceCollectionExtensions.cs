using Microsoft.Extensions.DependencyInjection;
using TasksManager.DataAccess.DbImplementation.Projects;
using TasksManager.DataAccess.DbImplementation.Tags;
using TasksManager.DataAccess.DbImplementation.Tasks;
using TasksManager.DataAccess.DbImplementation.Users;
using TasksManager.DataAccess.Interfaces;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.DataAccess.Interfaces.Tags;
using TasksManager.DataAccess.Interfaces.Tasks;
using TasksManager.DataAccess.Interfaces.Users;

namespace TasksManager.DataAccess.DbImplementation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterQueriesAndCommands(this IServiceCollection services)
        {
            services
                .AddScoped<IProjectQuery, ProjectQuery>()
                .AddScoped<IProjectsListQuery, ProjectsListQuery>()
                .AddScoped<IGetProjectQuery, GetProjectQuery>()

                .AddScoped<ITaskQuery, TaskQuery>()
                .AddScoped<ITaskListQuery, TaskListQuery>()

                .AddScoped<ITagsListQuery, TagsListQuery>()

                .AddScoped<ICreateProjectCommand, CreateProjectCommand>()
                .AddScoped<IUpdateProjectCommand, UpdateProjectCommand>()
                .AddScoped<IDeleteProjectCommand, DeleteProjectCommand>()

                .AddScoped<ICreateTaskCommand, CreateTaskCommand>()
                .AddScoped<IUpdateTaskCommand, UpdateTaskCommand>()
                .AddScoped<IDeleteTaskCommand, DeleteTaskCommand>()
                .AddScoped<IGetTaskQuery, GetTaskQuery>()
                .AddScoped<IAddTagToTaskCommand, AddTagToTaskCommand>()
                .AddScoped<IDeleteTagFromTaskCommand, DeleteTagFromTaskCommand>()

                .AddScoped<ILockProjectCommand, LockProjectCommand>()

                .AddScoped<IDeleteTagCommand, DeleteTagCommand>()

                .AddScoped<ILockProjectCommand, LockProjectCommand>()
                .AddScoped<IUnlockProjectCommand, UnlockProjectCommand>()

                .AddScoped<IGetIdentityCommand, GetIdentityCommand>()
                .AddScoped<IRegistrationCommand, RegistrationCommand>();

            return services;
        }
    }
}
