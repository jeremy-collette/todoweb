namespace todoweb.Server
{
    using Microsoft.AspNetCore.Mvc;

    using todoweb.Server.Contract;
    using todoweb.Server.Core;

    [Route("/api/user")]
    public class UserController
        : ResourceController<Client.Models.User, Server.Models.User>
    {
        public UserController(IResourceManager<Server.Models.User> resourceManager, IHttpSessionManager httpSessionManager)
            : base(resourceManager, httpSessionManager)
        {
        }
    }
}