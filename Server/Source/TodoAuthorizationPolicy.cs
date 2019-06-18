﻿
using Microsoft.AspNetCore.Http;
using todoweb.Server.Contract;
using todoweb.Server.Models;

namespace todoweb.Server
{
    public class TodoAuthorizationPolicy : IAuthorizationPolicy<Todo>
    {
        public bool CanCreate(User creator)
        {
            return creator != null;
        }

        public bool CanDelete(User accessor, Todo resource)
        {
            return accessor?.Id == resource.Owner;
        }

        public bool CanRead(User accessor, Todo resource)
        {
            return accessor?.Id == resource.Owner;
        }

        public bool CanWrite(User accessor, Todo resource)
        {
            return accessor?.Id == resource.Owner;
        }
    }
}
