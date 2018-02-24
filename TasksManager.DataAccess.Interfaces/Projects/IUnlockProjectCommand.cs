using System.Threading.Tasks;

namespace TasksManager.DataAccess.Interfaces.Projects
{
    public interface IUnlockProjectCommand
    {
        Task<bool> ExecuteAsync(int projectId, string user);
    }
}
