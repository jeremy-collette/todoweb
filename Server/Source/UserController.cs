namespace todoweb.Server
{
    using System.Linq;

    using AutoMapper.Configuration;
    using Microsoft.AspNetCore.Mvc;

    using todoweb.Server.Contract;
    using todoweb.Server.Core.Contract;

    using Client = Client.Models;
    using Server = Models;

    [Route("/api/user")]
    public class UserController
        : ResourceController<Client.User, Server.User>
    {
        private IResourceManager<Server.User> userManager_;
        private IHttpSessionManager httpSessionManager_;
        private IPasswordHasher passwordHasher_;

        private static MapperConfigurationExpression GetMapperConfigurationExpression(IPasswordHasher passwordHasher)
        {
            var mapperConfigurationExpression = new MapperConfigurationExpression();
            mapperConfigurationExpression.CreateMap<Client.User, Server.User>()
                .ForMember(dest => dest.PasswordHash, opts => opts.MapFrom(
                    src => passwordHasher.GetHash(src.Password)));
            mapperConfigurationExpression.CreateMap<Server.User, Client.User>();
            return mapperConfigurationExpression;
        }

        public UserController(IResourceManager<Server.User> userManager, IHttpSessionManager httpSessionManager, IAuthorizationPolicy<Server.User> authorizationPolicy, IPasswordHasher passwordHasher)
            : base(userManager, httpSessionManager, authorizationPolicy, keyGenerator: u => u.Email, modelValidator: new UserValidator(), modelMapperConfigurationExpression: UserController.GetMapperConfigurationExpression(passwordHasher))
        {
            this.userManager_ = userManager;
            this.httpSessionManager_ = httpSessionManager;
            this.passwordHasher_ = passwordHasher;
        }

        [Route("login")]
        [HttpGet]
        public ActionResult<Client.User> GetCurrent()
        {
            var serverUser = this.httpSessionManager_.GetUserFromRequest(Request);
            if (serverUser == null)
            {
                return NotFound();
            }

            return Ok(this.ModelMapper.Map<Client.User>(serverUser));
        }

        [Route("login")]
        [HttpPost]
        public ActionResult<Client.User> Login([FromBody] Client.User user)
        {
            var serverUser = userManager_.GetAll()
                .FirstOrDefault(u => u.Email == user.Email && this.passwordHasher_.Verify(user.Password, u.PasswordHash));
            if (serverUser == null)
            {
                return NotFound();
            }

            this.httpSessionManager_.CreateOrUpdateSession(serverUser, Request);
            return Ok(this.ModelMapper.Map<Client.User>(serverUser));
        }

        [Route("logout")]
        [HttpPost]
        public ActionResult<bool> Logout()
        {
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(this.httpSessionManager_.DeleteSession(Request));
        }
    }
}