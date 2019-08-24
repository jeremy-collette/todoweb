namespace todoweb.Server.Core.Contract
{
    using System;
    using System.Collections.Generic;

    using todoweb.Server.Models;

    public class DatabaseResourceManager<TResource>
        : IResourceManager<TResource>
        where TResource : class, IServerResource
    {
        private DatabaseContext<TResource> context_;

        public DatabaseResourceManager(DatabaseContext<TResource> context)
        {
            this.context_ = context;
            this.context_.Database.EnsureCreated();
        }

        public TResource Add(TResource resource)
        {
            this.context_.Resources.Add(resource);
            this.context_.SaveChanges();
            return resource;
        }

        public TResource AddOrUpdate(TResource resource)
        {
            var found = this.Get(resource.Id);
            if (found != null)
            {
                this.Delete(resource.Id);
            }
            return this.Add(resource);
        }

        public bool Delete(string id)
        {
            var found = this.context_.Resources.Find(id);
            if (found == null)
            {
                return false;
            }
            this.context_.Resources.Remove(found);
            this.context_.SaveChanges();
            return true;
        }

        public TResource Get(string id)
        {
            return this.context_.Resources.Find(id);
        }

        public IEnumerable<TResource> GetAll()
        {
            return this.context_.Resources;
        }

    }
}
