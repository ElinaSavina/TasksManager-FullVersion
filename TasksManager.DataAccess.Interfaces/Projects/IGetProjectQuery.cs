using System.Threading.Tasks;
using TasksManager.Entities;

namespace TasksManager.DataAccess.Interfaces.Projects
{
    public interface IGetProjectQuery
    {
        Task<Project> RunAsync(int projectId);
    }
}
