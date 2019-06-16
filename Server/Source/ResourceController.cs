namespace todoweb.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private IHttpSessionManager httpSessionManager_;
        private IMapper modelMapper_;

        public ResourceController(IResourceManager<TServerResource> resourceManager, IHttpSessionManager httpSessionManager)
        {
            this.resourceManager_ = resourceManager;
            this.httpSessionManager_ = httpSessionManager;
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
            // Make sure we're not already logged in
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user != null)
            {
                return default;
            }

            // Create new resource for user
            var serverResource = modelMapper_.Map<TServerResource>(resource);
            serverResource.Id = Guid.NewGuid();
            serverResource.UserId = user.Id;
            return modelMapper_.Map<TClientResource>(resourceManager_.Add(serverResource));
        }

        // PUT resource/5
        [HttpPut("{id}")]
        public TClientResource CreateOrUpdate(Guid id, [FromBody] TClientResource resource)
        {
            // Get user from session
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return default;
            }

            // Check if resource exists and if this user owns it
            var foundResource = resourceManager_.Get(id);
            if (foundResource?.UserId != user.UserId)
            {
                return default;
            }

            // Update existing resource / create new
            var serverResource = modelMapper_.Map<TServerResource>(resource);
            serverResource.Id = id;
            return modelMapper_.Map<TClientResource>(resourceManager_.AddOrUpdate(serverResource));
        }

        // DELETE resource/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            // Get user from session
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return false;
            }

            // Check if resource exists and if this user owns it
            var foundResource = resourceManager_.Get(id);
            if (foundResource?.UserId != user.UserId)
            {
                return default;
            }

            return resourceManager_.Delete(id);
        }

        // GET resource/
        [HttpGet]
        public IEnumerable<TClientResource> Get()
        {
            // Get user from session
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return default;
            }

            var userResources = resourceManager_.GetAll().Where(r => r.UserId == user.Id);
            return modelMapper_.Map<IEnumerable<TClientResource>>(userResources);
        }

        // GET resource/5
        [HttpGet("{id}")]
        public TClientResource Get(Guid id)
        {
            // Get user from session
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return default;
            }

            var resource = resourceManager_.Get(id);
            if (resource?.UserId != user.Id)
            {
                return default;
            }
            return modelMapper_.Map<TClientResource>(resource);
        }
    }
}
