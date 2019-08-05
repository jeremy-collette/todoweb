namespace todoweb.Server
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using todoweb.Server.Contract;
    using todoweb.Server.Core;

    using Client = Client.Models;
    using Server = Models;

    [Route("/api/user")]
    public class UserController
        : ResourceController<Client.User, Server.User>
    {
        private IResourceManager<Server.User> userManager_;
        private IHttpSessionManager httpSessionManager_;

        public UserController(IResourceManager<Server.User> userManager, IHttpSessionManager httpSessionManager, IAuthorizationPolicy<Server.User> authorizationPolicy)
            : base(userManager, httpSessionManager, authorizationPolicy)
        {
            this.userManager_ = userManager;
            this.httpSessionManager_ = httpSessionManager;
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
                .FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
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