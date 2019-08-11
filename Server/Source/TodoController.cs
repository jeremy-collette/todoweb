namespace todoweb.Server
{
    using Microsoft.AspNetCore.Mvc;

    using todoweb.Server.Contract;
    using todoweb.Server.Core;

    using Client = Client.Models;
    using Server = Server.Models;

    [Route("/api/todo")]
    public class TodoController
        : ResourceController<Client.Todo, Server.Todo>
    {
        public TodoController(IResourceManager<Server.Todo> userManager, IHttpSessionManager httpSessionManager, IAuthorizationPolicy<Server.Todo> authorizationPolicy)
            : base(userManager, httpSessionManager, authorizationPolicy)
        {
        }
    }
}
