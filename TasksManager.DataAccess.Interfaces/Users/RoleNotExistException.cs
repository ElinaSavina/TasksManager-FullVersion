using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.DataAccess.Interfaces.Users
{
    public class RoleNotExistException : Exception
    {
        public RoleNotExistException()
            : base("Невозможно создать учетную запись с такой ролью") { }
    }
}
