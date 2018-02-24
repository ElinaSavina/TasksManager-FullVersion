using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TasksManager.ViewModel.Users;

namespace TasksManager.DataAccess.Interfaces.Users
{
    public interface IRegistrationCommand
    {
        Task ExecuteAsync(RegistrationRequest request);
    }
}
