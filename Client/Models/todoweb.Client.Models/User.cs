﻿namespace todoweb.Client.Models
{
    using System;

    using todoweb.Client.Models.Contract;

    public class User
        : IClientResource
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
