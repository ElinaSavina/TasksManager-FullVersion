using System.Threading.Tasks;

namespace TasksManager.DataAccess.Interfaces.Tags
{
    public interface IDeleteTagCommand
    {
        Task ExecuteAsync(string tag);
    }
}
