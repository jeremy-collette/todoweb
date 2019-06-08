namespace todoweb.Server
{
    using Microsoft.AspNetCore.Mvc;
    using todoweb.Server.Core;

    [Route("", Name = "default")]
    public class TodoController
        : ResourceController<Client.Models.Todo, Server.Models.Todo>
    {
        public TodoController(IResourceManager<Server.Models.Todo> resourceManager)
            : base(resourceManager)
        {
        }
    }
}
