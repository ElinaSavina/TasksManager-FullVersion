using System.Threading.Tasks;
using TasksManager.Entities;
using TasksManager.ViewModel.Group;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Services.Groups
{
    public interface IGroupService
    {
        Task<GroupResponse> AddGroup(CreateGroupRequest request);
        Task<GroupResponse> GetGroup(int id);
        Task<GroupResponse> UpdateGroup(int id, UpdateGroupRequest request);
        Task RemoveGroup(int id);
    }
}
