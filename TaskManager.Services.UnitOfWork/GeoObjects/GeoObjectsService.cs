using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess.UnitOfWork;
using TaskManager.Services.GeoObjects;
using TasksManager.Entities;
using TasksManager.ViewModel.GeoObjects;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Services.UnitOfWork.GeoObjects
{
    public class GeoObjectsService : IGeoObjectsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;
        private readonly IMapper _mapper;

        public GeoObjectsService(IUnitOfWork uow, IAsyncQueryableFactory factory, IMapper mapper)
        {
            _uow = uow;
            _factory = factory;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GeoObjectResponse>> GetChildren(int parentId)
        {
            var query = _uow.GeoObjects.Query().Where(g => g.ParentId == parentId);
            var geoObject =  await _factory.CreateAsyncQueryable(query).ToListAsync();
            var children = new List<GeoObjectResponse>();
            foreach (var obj in geoObject)
            {
                children.Add(_mapper.Map<GeoObject, GeoObjectResponse>(obj));
            }
            return children;
        }

        public async Task<IEnumerable<GeoObjectResponse>> GetRoot(int level)
        {
            var geoObjects = await _factory.CreateAsyncQueryable(
                _uow.GeoObjects.Query().Where(g => g.Level <= level)).ToListAsync();
            var roots = geoObjects.Where(g => g.Parent == null).ToList();
            var dict = geoObjects
                .Where(g => g.Parent != null)
                .GroupBy(g => g.ParentId)
                .ToDictionary(group => group.Key.Value, group => group.ToList());
            var rootsResponse = new List<GeoObjectResponse>();
            foreach (var root in roots)
            {
                root.Children = LoadChildren(dict, root.Id, level);
                rootsResponse.Add(_mapper.Map<GeoObject, GeoObjectResponse>(root));
            }
            return rootsResponse;
        }
        private ICollection<GeoObject> LoadChildren(IDictionary<int, List<GeoObject>> dict, int parentId, int level)
        {
            dict.TryGetValue(parentId, out var children);
            if (children == null) return null;
            if (level > 1)
            {
                foreach (var ch in children)
                {
                    ch.Children = LoadChildren(dict, ch.Id, level - 1);
                }
            }
            return children;
        }

        public async Task Delete(int id)
        {
            await DeleteNode(id);
            await _uow.CommitAsync();
        }

        private async Task DeleteNode(int id)
        {
            var element = await _factory.CreateAsyncQueryable(_uow.GeoObjects.Query(c => c.Children))
                .FirstOrDefaultAsync(g => g.Id == id);
            foreach (var ch in element.Children)
            {
                await DeleteNode(ch.Id);
            }
            _uow.GeoObjects.Remove(element);
        }

        public async Task<GeoObjectResponse> AddNode(AddGeoObjectRequest obj)
        {
            if (obj.ParentId != null)
            {
                var parent = await _factory.CreateAsyncQueryable(_uow.GeoObjects.Query())
                    .FirstOrDefaultAsync(p => p.Id == obj.ParentId);
                if (parent == null)
                    throw new InvalidDataException("Ошибка при добавлении объекта. Родителя не существует.");
            }
            var geoObj = _mapper.Map<AddGeoObjectRequest, GeoObject>(obj);
            _uow.GeoObjects.Add(geoObj);
            await _uow.CommitAsync();
            return _mapper.Map<GeoObject, GeoObjectResponse>(geoObj);
        }
    }
}
