﻿namespace todoweb.Server.Models
{
    using System;

    public interface IServerResource
    {
        Guid Id { get; set; }

        Guid Owner { get; set; }
    }
}
