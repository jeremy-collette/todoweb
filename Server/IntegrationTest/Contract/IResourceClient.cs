namespace todoweb.Server.IntegrationTest
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using todoweb.Client.Models.Contract;

    public interface IResourceClient<TClientResource>
        where TClientResource : IClientResource
    {
        Task<TClientResource> GetAsync(string id);

        Task<IEnumerable<TClientResource>> GetAllAsync();

        Task<TClientResource> CreateAsync(TClientResource resource);

        Task<TClientResource> CreateOrUpdateAsync(string id, TClientResource resource);

        Task<bool> DeleteAsync(string id);
    }
}
