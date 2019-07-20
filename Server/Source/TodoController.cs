namespace todoweb.Server
{
    using Microsoft.AspNetCore.Mvc;

    using todoweb.Server.Contract;
    using todoweb.Server.Core;
    using todoweb.Server.Core.Contract;

    using Client = Client.Models;
    using Server = Server.Models;

    [Route("/api/todo")]
    public class TodoController
        : ResourceController<Client.Todo, Server.Todo>
    {
        // TODO(@jez): Use interface
        public TodoController(DatabaseContext<Server.Todo> databaseContext, IHttpSessionManager httpSessionManager, IAuthorizationPolicy<Server.Todo> authorizationPolicy)
            : base(new DatabaseResourceManager<Server.Todo>(databaseContext), httpSessionManager, authorizationPolicy)
        {
        }
    }
}
