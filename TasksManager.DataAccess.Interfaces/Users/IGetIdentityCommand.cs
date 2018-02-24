using System.Security.Claims;
using System.Threading.Tasks;
using TasksManager.ViewModel.Users;

namespace TasksManager.DataAccess.Interfaces.Users
{
    public interface IGetIdentityCommand
    {
        Task<ClaimsIdentity> ExecuteAsync(AuthRequest request);
    }
}
