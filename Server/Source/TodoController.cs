namespace todoweb.Server
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using todoweb.Server.Core;
    using todoweb.Server.Models;

    [Route("", Name = "default")]
    public class TodoController : Controller
    {
        private TodoManager todoManager_ = TodoManager.Instance;

        // GET todo/
        [HttpGet]
        public IEnumerable<Todo> Get()
        {
            return todoManager_.GetAll();
        }

        // GET todo/5
        [HttpGet("{id}")]
        public Todo Get(Guid id)
        {
            return todoManager_.Get(id);
        }

        // POST todo/
        [HttpPost]
        public Todo Create([FromBody]Todo todo)
        {
            todo.Id = Guid.NewGuid();
            todoManager_.Add(todo);
            return todo;
        }

        // PUT todo/5
        [HttpPut("{id}")]
        public Todo CreateOrUpdate(Guid id, [FromBody]Todo todo)
        {
            todo.Id = id;
            todoManager_.Delete(id);
            todoManager_.Add(todo);
            return todo;
        }

        // DELETE todo/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            todoManager_.Delete(id);
        }
    }
}
