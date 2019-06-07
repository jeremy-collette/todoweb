using System;
using System.Collections.Generic;
using System.Text;

namespace todoweb.Client.Models
{
    public class User
    {
        public Guid Id { get; }

        public string Email { get; set; }
    }
}
