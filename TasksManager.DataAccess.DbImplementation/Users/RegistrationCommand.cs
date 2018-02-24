using System;
using AutoMapper;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Users;
using TasksManager.Entities;
using TasksManager.ViewModel.Users;
using Task = System.Threading.Tasks.Task;
using Microsoft.AspNetCore.Identity;

namespace TasksManager.DataAccess.DbImplementation.Users
{
    internal class RegistrationCommand : IRegistrationCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryableFactory _factory;

        public RegistrationCommand(IUnitOfWork uow, IMapper mapper, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _mapper = mapper;
            _factory = factory;
        }

        public async Task ExecuteAsync(RegistrationRequest request)
        {
            User requestUser = _mapper.Map<RegistrationRequest, User>(request);

            User user = await _factory
                .CreateAsyncQueryable(_uow.Users.Query())
                .FirstOrDefaultAsync(u => u.Login == requestUser.Login);

            if (user != null)
                throw new RegistrationException();
            
            Role role = await _factory
                .CreateAsyncQueryable(_uow.Roles.Query())
                .FirstOrDefaultAsync(u => u.Name == requestUser.Role.Name);

            requestUser.Role = role ?? throw new RoleNotExistException();
            requestUser.Password = new PasswordHasher<User>().HashPassword(requestUser, requestUser.Password);
            
            _uow.Users.Add(requestUser);
            await _uow.CommitAsync();
        }
    }
}
