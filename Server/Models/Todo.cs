namespace todoweb.Server.Models
{
    using System;

    public class Todo : IServerResource
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
