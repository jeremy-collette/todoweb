namespace todoweb.Server
{
    using Microsoft.AspNetCore.Mvc;

    [Route("", Name = "default")]
    public class TodoController : ResourceController<Client.Models.Todo, Server.Models.Todo>
    {
    }
}
