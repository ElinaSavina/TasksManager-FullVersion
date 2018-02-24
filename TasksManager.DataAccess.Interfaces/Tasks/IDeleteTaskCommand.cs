using System.Threading.Tasks;

namespace TasksManager.DataAccess.Interfaces.Tasks
{
    public interface IDeleteTaskCommand
    {
        Task ExecuteAsync(int taskId);
    }
}
