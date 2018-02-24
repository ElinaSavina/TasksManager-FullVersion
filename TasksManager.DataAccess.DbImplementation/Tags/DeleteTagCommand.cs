using System.Threading.Tasks;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Tags;

namespace TasksManager.DataAccess.DbImplementation.Tags
{
    public class DeleteTagCommand : IDeleteTagCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncQueryableFactory _factory;

        public DeleteTagCommand(IUnitOfWork uow, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _factory = factory;
        }

        public async Task ExecuteAsync(string tagName)
        {
            var tag = await _factory
                .CreateAsyncQueryable(_uow.Tags.Query())
                .FirstOrDefaultAsync(t => t.Name == tagName);

            if (tag != null)
            {
                _uow.Tags.Remove(tag);
                await _uow.CommitAsync();
            }
        }
    }
}
