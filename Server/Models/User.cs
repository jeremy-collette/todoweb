using System;
using System.Collections.Generic;
using System.Text;

namespace todoweb.Server.Models
{
    public class User
        : IServerResource
    {
        public string Id { get; set; }

        public string Owner { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
    }
}
