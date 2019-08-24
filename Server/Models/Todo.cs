namespace todoweb.Server.Models
{
    using System;

    public class Todo
        : IServerResource
    {
        public string Id { get; set; }

        public string Owner { get; set; }

        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
