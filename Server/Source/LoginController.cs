namespace todoweb.Server
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using todoweb.Server.Contract;
    using todoweb.Server.Core;
    using todoweb.Server.Models;

    using Client = Client.Models;

    [Route("/api/login")]
    public class LoginController : Controller
    {
        private IResourceManager<User> userManager_;
        private IHttpSessionManager httpSessionManager_;

        public LoginController(IResourceManager<User> userManager, IHttpSessionManager httpSessionManager)
        {
            this.userManager_ = userManager;
            this.httpSessionManager_ = httpSessionManager;
        }

        [HttpPost]
        public ActionResult<Client.User> Login([FromBody] Client.Login login)
        {
            var hasher = SHA256.Create();
            var passwordHash = hasher.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
            var user = userManager_.GetAll().FirstOrDefault(u => u.PasswordHash == passwordHash);
            if (user == null)
            {
                return default;
            }
            this.httpSessionManager_.CreateOrUpdateSession(user, Request);
            return RedirectToAction("Get", "User", user.Id);
        }

        [HttpDelete]
        public bool Logout()
        {
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return false;
            }

            return this.httpSessionManager_.DeleteSession(Request);
        }
    }
}
