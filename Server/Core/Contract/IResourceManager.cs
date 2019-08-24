namespace todoweb.Server.Core.Contract
{
    using System.Collections.Generic;

    using todoweb.Server.Models;

    public interface IResourceManager<TResource> where TResource : IServerResource
    {
        TResource Get(string id);

        IEnumerable<TResource> GetAll();

        TResource Add(TResource resource);

        TResource AddOrUpdate(TResource resource);

        bool Delete(string id);
    }
}
