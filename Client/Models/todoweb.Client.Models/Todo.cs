using System;
using todoweb.Client.Models.Contract;

namespace todoweb.Client.Models
{
    public class Todo : IClientResource
    {
        public Guid Id { get; }

        public string Title { get; set; }
    }
}
