using System.Threading.Tasks;
using TasksManager.ViewModel.Projects;

namespace TasksManager.DataAccess.Interfaces.Projects
{
    public interface IProjectQuery
    {
        Task<ProjectResponse> RunAsync(int projectId);
    }
}
