﻿namespace todoweb.Server
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    using todoweb.Server.Contract;
    using todoweb.Server.Core.Contract;

    using Client = Client.Models;
    using Server = Server.Models;

    [Route("/api/todo")]
    public class TodoController
        : ResourceController<Client.Todo, Server.Todo>
    {
        private IResourceManager<Server.Todo> todoManager_;
        private IHttpSessionManager httpSessionManager_;
        private IAuthorizationPolicy<Server.Todo> authorizationPolicy_;

        public TodoController(IResourceManager<Server.Todo> todoManager, IHttpSessionManager httpSessionManager, IAuthorizationPolicy<Server.Todo> authorizationPolicy)
            : base(todoManager, httpSessionManager, authorizationPolicy, keyGenerator: t => Guid.NewGuid().ToString(), modelValidator: new TodoValidator())
        {
            this.todoManager_ = todoManager;
            this.httpSessionManager_ = httpSessionManager;
            this.authorizationPolicy_ = authorizationPolicy;
        }

        [Route("complete/{id}")]
        [HttpPost]
        public ActionResult Complete(string id)
        {
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return Unauthorized();
            }

            var todo = this.todoManager_.Get(id);
            if (todo == null)
            {
                return NotFound();
            }

            if (!authorizationPolicy_.CanWrite(user, todo))
            {
                return Unauthorized();
            }

            todo.Done = true;
            this.todoManager_.AddOrUpdate(todo);
            return Ok();
        }
    }
}
