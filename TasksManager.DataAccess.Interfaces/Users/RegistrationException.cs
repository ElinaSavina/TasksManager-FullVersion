using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.DataAccess.Interfaces.Users
{
    public class RegistrationException : Exception
    {
        public RegistrationException()
            : base("Пользователь с таким логином уже существует!") { }
    }
}
