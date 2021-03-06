﻿namespace todoweb.Server.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using todoweb.Server.Core.Contract;
    using todoweb.Server.Models;

    public class ListResourceManager<TResource>
        : IResourceManager<TResource>
        where TResource : IServerResource
    {
        private ICollection<TResource> resources_ = new List<TResource>();

        public TResource Add(TResource resource)
        {
            this.resources_.Add(resource);
            return resource;
        }

        public TResource AddOrUpdate(TResource resource)
        {
            this.Delete(resource.Id);
            return this.Add(resource);
        }

        public bool Delete(string id)
        {
            var resource = this.resources_.FirstOrDefault(r => r.Id == id);
            if (resource == null)
            {
                return false;
            }
            this.resources_.Remove(resource);
            return true;
        }

        public TResource Get(string id)
        {
            return this.resources_.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<TResource> GetAll()
        {
            return this.resources_;
        }
    }
}
