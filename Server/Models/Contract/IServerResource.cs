namespace todoweb.Server.Models
{
    using System;

    public interface IServerResource
    {
        string Id { get; set; }

        string Owner { get; set; }
    }
}
