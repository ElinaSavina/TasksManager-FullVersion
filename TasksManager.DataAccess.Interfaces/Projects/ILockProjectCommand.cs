using System.Threading.Tasks;

namespace TasksManager.DataAccess.Interfaces.Projects
{
    public interface ILockProjectCommand
    {
        Task<bool> ExecuteAsync(int projectId, string user);
    }
}
