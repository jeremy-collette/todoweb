using System;
using System.Collections.Generic;
using System.Linq;
using todoweb.Server.Models;

namespace todoweb.Server.Core
{
    public class ResourceManager<TResource> : IResourceManager<TResource> where TResource : IServerResource
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

        public bool Delete(Guid id)
        {
            var resource = this.resources_.FirstOrDefault(r => r.Id == id);
            if (resource == null)
            {
                return false;
            }
            this.resources_.Remove(resource);
            return true;
        }

        public TResource Get(Guid id)
        {
            return this.resources_.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<TResource> GetAll()
        {
            return this.resources_;
        }
    }
}
