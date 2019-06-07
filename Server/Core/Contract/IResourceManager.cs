namespace todoweb.Server.Core
{
    using System;
    using System.Collections.Generic;
    using todoweb.Server.Models;

    public interface IResourceManager<TResource> where TResource : IServerResource
    {
        TResource Get(Guid id);

        IEnumerable<TResource> GetAll();

        TResource Add(TResource resource);

        TResource AddOrUpdate(TResource resource);

        bool Delete(Guid id);
    }
}
