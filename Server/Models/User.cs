﻿using System;
using System.Collections.Generic;
using System.Text;

namespace todoweb.Server.Models
{
    public class User
        : IServerResource
    {
        public Guid Id { get; set; }

        public Guid Owner { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}