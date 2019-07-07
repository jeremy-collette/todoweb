namespace todoweb.Server.Models
{
    using System;

    public class Todo
        : IServerResource
    {
        public Guid Id { get; set; }

        public Guid Owner { get; set; }

        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
