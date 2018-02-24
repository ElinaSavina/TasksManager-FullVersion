using System.Collections.Generic;
using AutoMapper;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.DataAccess.UnitOfWork;
using TasksManager.DataAccess.Interfaces.Users;
using TasksManager.Entities;
using TasksManager.ViewModel.Users;
using Microsoft.AspNetCore.Identity;

namespace TasksManager.DataAccess.DbImplementation.Users
{
    internal class GetIdentityCommand : IGetIdentityCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryableFactory _factory;

        public GetIdentityCommand(IUnitOfWork uow, IMapper mapper, IAsyncQueryableFactory factory)
        {
            _uow = uow;
            _mapper = mapper;
            _factory = factory;
        }
        public async Task<ClaimsIdentity> ExecuteAsync(AuthRequest request)
        {
            var requestUser = _mapper.Map<AuthRequest, User>(request);
            User user = await _factory
                .CreateAsyncQueryable(_uow.Users.Query(u => u.Role))
                .FirstOrDefaultAsync(p => p.Login == requestUser.Login);

            if (user == null)
                throw new AuthenticationException("Неверная пара логин-пароль!");

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, requestUser.Password) == PasswordVerificationResult.Failed)
                throw new AuthenticationException("Неверная пара логин-пароль!");
            
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
