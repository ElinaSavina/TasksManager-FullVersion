using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.DataAccess.Interfaces.Projects
{
    public class ProjectLockedException : Exception
    {
        public ProjectLockedException()
            : base("Project is locked.") { }
    }
}
