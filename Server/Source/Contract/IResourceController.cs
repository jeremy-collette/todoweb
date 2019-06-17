namespace todoweb.Server.Contract
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using todoweb.Client.Models.Contract;
    using todoweb.Server.Models;

    public interface IResourceController<TClientResource> where TClientResource : IClientResource
    {
        ActionResult<IEnumerable<TClientResource>> Get();

        ActionResult<TClientResource> Get(Guid id);

        ActionResult<TClientResource> Create([FromBody]TClientResource resource);

        ActionResult<TClientResource> CreateOrUpdate(Guid id, [FromBody]TClientResource resource);

        ActionResult<bool> Delete(Guid id);
    }
}
