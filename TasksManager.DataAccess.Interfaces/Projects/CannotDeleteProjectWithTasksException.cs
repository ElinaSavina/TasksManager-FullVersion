﻿using System;

namespace TasksManager.DataAccess.Interfaces.Projects
{
    public class CannotDeleteProjectWithTasksException : Exception
    {
        public CannotDeleteProjectWithTasksException()
            : base("It is not possible to delete project if it has tasks.") { }
    }
}
