namespace todoweb.Server
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using todoweb.Server.Core;
    using ClientTodo = todoweb.Client.Models.Todo;
    using CoreTodo = todoweb.Server.Models.Todo;

    [Route("", Name = "default")]
    public class TodoController : Controller
    {
        private TodoManager todoManager_ = TodoManager.Instance;
        private IMapper mapper_;

        public TodoController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientTodo, CoreTodo>();
                cfg.CreateMap<CoreTodo, ClientTodo>();
            });
            mapper_ = config.CreateMapper();
        }


        // GET todo/
        [HttpGet]
        public IEnumerable<ClientTodo> Get()
        {
            return mapper_.Map<IEnumerable<ClientTodo>>(todoManager_.GetAll());
        }

        // GET todo/5
        [HttpGet("{id}")]
        public ClientTodo Get(Guid id)
        {
            return mapper_.Map<ClientTodo>(todoManager_.Get(id));
        }

        // POST todo/
        [HttpPost]
        public ClientTodo Create([FromBody]ClientTodo todo)
        {
            var coreTodo = mapper_.Map<CoreTodo>(todo);
            coreTodo.Id = Guid.NewGuid();
            todoManager_.Add(mapper_.Map<CoreTodo>(todo));
            return todo;
        }

        // PUT todo/5
        [HttpPut("{id}")]
        public ClientTodo CreateOrUpdate(Guid id, [FromBody]ClientTodo todo)
        {
            var coreTodo = mapper_.Map<CoreTodo>(todo);
            coreTodo.Id = id;
            todoManager_.Delete(id);
            todoManager_.Add(coreTodo);
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
