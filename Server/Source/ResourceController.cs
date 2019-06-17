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
        protected IMapper ModelMapper { get; set; }

        public ResourceController(IResourceManager<TServerResource> resourceManager, IHttpSessionManager httpSessionManager)
        {
            this.resourceManager_ = resourceManager;
            this.httpSessionManager_ = httpSessionManager;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TClientResource, TServerResource>();
                cfg.CreateMap<TServerResource, TClientResource>();
            });

            this.ModelMapper = config.CreateMapper();
        }

        // POST resource/
        [HttpPost]
        public ActionResult<TClientResource> Create([FromBody] TClientResource resource)
        {
            // Make sure we're not already logged in
            //var user = this.httpSessionManager_.GetUserFromRequest(Request);
            //if (user != null)
            //{
            //    return NotFound();
            //}

            // Create new resource for user
            var serverResource = this.ModelMapper.Map<TServerResource>(resource);
            serverResource.Id = Guid.NewGuid();
            //serverResource.UserId = user.Id;
            return this.ModelMapper.Map<TClientResource>(resourceManager_.Add(serverResource));
        }

        // PUT resource/5
        [HttpPut("{id}")]
        public ActionResult<TClientResource> CreateOrUpdate(Guid id, [FromBody] TClientResource resource)
        {
            // Get user from session
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return NotFound();
            }

            // Check if resource exists and if this user owns it
            var foundResource = resourceManager_.Get(id);
            if (foundResource?.UserId != user.UserId)
            {
                return NotFound();
            }

            // Update existing resource / create new
            var serverResource = this.ModelMapper.Map<TServerResource>(resource);
            serverResource.Id = id;
            return this.ModelMapper.Map<TClientResource>(resourceManager_.AddOrUpdate(serverResource));
        }

        // DELETE resource/5
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(Guid id)
        {
            // Get user from session
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return NotFound();
            }

            // Check if resource exists and if this user owns it
            var foundResource = resourceManager_.Get(id);
            if (foundResource?.UserId != user.UserId)
            {
                return NotFound();
            }

            return resourceManager_.Delete(id);
        }

        // GET resource/
        [HttpGet]
        public ActionResult<IEnumerable<TClientResource>> Get()
        {
            // Get user from session
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return NotFound();
            }

            var userResources = resourceManager_.GetAll().Where(r => r.UserId == user.Id);
            return Ok(this.ModelMapper.Map<IEnumerable<TClientResource>>(userResources));
        }

        // GET resource/5
        [HttpGet("{id}")]
        public ActionResult<TClientResource> Get(Guid id)
        {
            // Get user from session
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            if (user == null)
            {
                return NotFound();
            }

            var resource = resourceManager_.Get(id);
            //if (resource?.UserId != user.Id)
            //{
            //    return NotFound();
            //}
            return this.ModelMapper.Map<TClientResource>(resource);
        }
    }
}
