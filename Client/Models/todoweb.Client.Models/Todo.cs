namespace todoweb.Client.Models
{
    using System;
    using todoweb.Client.Models.Contract;

    public class Todo
        : IClientResource
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
    }
}
