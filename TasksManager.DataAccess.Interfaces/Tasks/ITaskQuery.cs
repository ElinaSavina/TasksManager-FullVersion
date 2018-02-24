using System.Threading.Tasks;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.Interfaces.Tasks
{
    public interface ITaskQuery
    {
        Task<TaskResponse> RunAsync(int taskId);
    }
}
