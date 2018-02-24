using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using TasksManager.Entities;
using Task = System.Threading.Tasks.Task;
using System.Linq;

namespace TasksManager.AuthHandlers
{
    public class ProjectAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Project>
    {       
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Project project)
        {
            if (requirement.Name == Operations.Update.Name && context.User.Identity.Name == project?.User.Login)
            {
                context.Succeed(requirement);
            }

            if (requirement.Name == Operations.Delete.Name && context.User.IsInRole("admin"))
            {
                context.Succeed(requirement);
            }

            if (requirement.Name == Operations.CreateTask.Name && context.User.Identity.Name == project?.User.Login)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }  
    }   
}