using System.Threading.Tasks;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.Interfaces.Tasks
{
    public interface IUpdateTaskCommand
    {
        Task<TaskResponse> ExecuteAsync(int taskId, UpdateTaskRequest request);
    }
}
