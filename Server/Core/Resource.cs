namespace todoweb.Server.Core
{
    using System;
    using todoweb.Server.Models;

    public abstract class Resource : IServerResource
    {
        public Guid Id { get; set; }
    }
}
