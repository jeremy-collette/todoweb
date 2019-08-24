namespace todoweb.Client.Models
{
    using System;

    using todoweb.Client.Models.Contract;

    public class Todo
        : IClientResource
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
