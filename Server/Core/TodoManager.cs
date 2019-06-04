namespace todoweb.Server.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using todoweb.Server.Models;

    public class TodoManager
    {
        public static TodoManager Instance { get; } = new TodoManager();

        private readonly List<Todo> todos_ = new List<Todo>();

        public IEnumerable<Todo> GetAll()
        {
            return todos_;
        }

        public Todo Get(Guid id)
        {
            return todos_.FirstOrDefault(t => t.Id == id);
        }

        public void Delete(Guid id)
        {
            todos_.Remove(this.Get(id));
        }

        public void Add(Todo t)
        {
            todos_.Add(t);
        }
    }
}
