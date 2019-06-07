namespace todoweb.Server.Contract
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using todoweb.Client.Models.Contract;
    using todoweb.Server.Models;

    public interface IResourceController<TClientResource> where TClientResource : IClientResource
    {
        IEnumerable<TClientResource> Get();

        TClientResource Get(Guid id);

        TClientResource Create([FromBody]TClientResource resource);

        TClientResource CreateOrUpdate(Guid id, [FromBody]TClientResource resource);

        bool Delete(Guid id);
    }
}
