using System.Threading.Tasks;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.Interfaces.Tasks
{
    public interface IAddTagToTaskCommand
    {
        Task<TaskResponse> ExecuteAsync(int taskId, string tag);
    }
}
