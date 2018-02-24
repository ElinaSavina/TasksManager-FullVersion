using System.Threading.Tasks;

namespace TasksManager.DataAccess.Interfaces.Projects
{
    public interface IDeleteProjectCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// <exception cref="CannotDeleteProjectWithTasksException"></exception>
        Task ExecuteAsync(int projectId, string user);
    }
}
