using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.DataAccess.Interfaces.Tasks
{
    public interface IGetTaskQuery
    {
        Task<Entities.Task> RunAsync(int taskId);
    }
}
