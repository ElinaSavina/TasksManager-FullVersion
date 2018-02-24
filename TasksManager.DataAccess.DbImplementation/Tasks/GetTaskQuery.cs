using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Tasks;
using TasksManager.Entities;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class GetTaskQuery : IGetTaskQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public GetTaskQuery(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }

        public async Task<Entities.Task> RunAsync(int taskId)
        {
            return await _factory
                .CreateAsyncQueryable(_uow.Tasks.Query(t => t.Project.User))
                .FirstOrDefaultAsync(p => p.Id == taskId);
        }
    }
}
