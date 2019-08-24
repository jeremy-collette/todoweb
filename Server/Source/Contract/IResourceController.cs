namespace todoweb.Server.Contract
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using todoweb.Client.Models.Contract;

    public interface IResourceController<TClientResource> where TClientResource : IClientResource
    {
        ActionResult<IEnumerable<TClientResource>> Get();

        ActionResult<TClientResource> Get(string id);

        ActionResult<TClientResource> Create([FromBody]TClientResource resource);

        ActionResult<TClientResource> CreateOrUpdate(string id, [FromBody]TClientResource resource);

        ActionResult<bool> Delete(string id);
    }
}
