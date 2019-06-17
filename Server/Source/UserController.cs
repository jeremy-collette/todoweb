namespace todoweb.Server
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
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

        public UserController(IResourceManager<Server.User> userManager, IHttpSessionManager httpSessionManager)
            : base(userManager, httpSessionManager)
        {
            this.userManager_ = userManager;
            this.httpSessionManager_ = httpSessionManager;
        }

        [Route("login")]
        [HttpPost]
        public ActionResult<Client.User> Login([FromBody] Client.User user)
        {
            var serverUser = userManager_.GetAll().FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            if (serverUser == null)
            {
                return NotFound();
            }

            this.httpSessionManager_.CreateOrUpdateSession(serverUser, Request);
            return Get(serverUser.Id);
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