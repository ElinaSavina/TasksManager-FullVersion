using System.Threading.Tasks;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.Interfaces.Tasks
{
    public interface ITaskListQuery
    {
        Task<ListResponse<TaskResponse>> RunAsync(TaskFilter filter, ListOptions options);
    }
}
