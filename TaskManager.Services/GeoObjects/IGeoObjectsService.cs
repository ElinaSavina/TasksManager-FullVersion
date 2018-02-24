using System.Collections.Generic;
using System.Threading.Tasks;
using TasksManager.ViewModel.GeoObjects;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Services.GeoObjects
{
    public interface IGeoObjectsService
    {
        Task<IEnumerable<GeoObjectResponse>> GetChildren(int parentId);

        Task<IEnumerable<GeoObjectResponse>> GetRoot(int level);

        Task Delete(int id);

        Task<GeoObjectResponse> AddNode(AddGeoObjectRequest obj);
    }
}
