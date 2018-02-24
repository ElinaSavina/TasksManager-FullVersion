using System.Threading.Tasks;

namespace TasksManager.DataAccess.Interfaces.Tasks
{
    public interface IDeleteTagFromTaskCommand
    {
        Task ExecuteAsync(int taskId, string tag);
    }
}
