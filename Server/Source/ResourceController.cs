namespace todoweb.Server
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using todoweb.Client.Models.Contract;
    using todoweb.Server.Contract;
    using todoweb.Server.Core;
    using todoweb.Server.Models;

    public class ResourceController<TClientResource, TServerResource>
        : Controller,
        IResourceController<TClientResource>
        where TClientResource : IClientResource
        where TServerResource : IServerResource
    {
        private IResourceManager<TServerResource> resourceManager_;
        private IMapper modelMapper_;

        public ResourceController(IResourceManager<TServerResource> resourceManager)
        {
            this.resourceManager_ = resourceManager;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TClientResource, TServerResource>();
                cfg.CreateMap<TServerResource, TClientResource>();
            });
            modelMapper_ = config.CreateMapper();
        }

        // POST resource/
        [HttpPost]
        public TClientResource Create([FromBody] TClientResource resource)
        {
            var serverResource = modelMapper_.Map<TServerResource>(resource);
            serverResource.Id = Guid.NewGuid();
            return modelMapper_.Map<TClientResource>(resourceManager_.Add(serverResource));
        }

        // PUT resource/5
        [HttpPut("{id}")]
        public TClientResource CreateOrUpdate(Guid id, [FromBody] TClientResource resource)
        {
            var serverResource = modelMapper_.Map<TServerResource>(resource);
            serverResource.Id = id;
            return modelMapper_.Map<TClientResource>(resourceManager_.AddOrUpdate(serverResource));
        }

        // DELETE resource/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            return resourceManager_.Delete(id);
        }

        // GET resource/
        [HttpGet]
        public IEnumerable<TClientResource> Get()
        {
            return modelMapper_.Map<IEnumerable<TClientResource>>(resourceManager_.GetAll());
        }

        // GET resource/5
        [HttpGet("{id}")]
        public TClientResource Get(Guid id)
        {
            var resource = resourceManager_.Get(id);
            if (resource == null)
            {
                return default;
            }
            return modelMapper_.Map<TClientResource>(resource);
        }
    }
}
