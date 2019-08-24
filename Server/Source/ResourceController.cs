namespace todoweb.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.Configuration;
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
        private IAuthorizationPolicy<TServerResource> authorizationPolicy_;
        private Func<TClientResource, string> keyGenerator_;
        private IModelValidator<TServerResource> modelValidator_;

        protected IMapper ModelMapper { get; set; }

        public ResourceController(IResourceManager<TServerResource> resourceManager, IHttpSessionManager httpSessionManager, IAuthorizationPolicy<TServerResource> authorizationPolicy, Func<TClientResource, string> keyGenerator, IModelValidator<TServerResource> modelValidator, MapperConfigurationExpression modelMapperConfigurationExpression)
        {
            this.resourceManager_ = resourceManager;
            this.httpSessionManager_ = httpSessionManager;
            this.authorizationPolicy_ = authorizationPolicy;
            this.keyGenerator_ = keyGenerator;
            this.modelValidator_ = modelValidator;

            var config = new MapperConfiguration(modelMapperConfigurationExpression);
            this.ModelMapper = config.CreateMapper();
        }

        public ResourceController(IResourceManager<TServerResource> resourceManager, IHttpSessionManager httpSessionManager, IAuthorizationPolicy<TServerResource> authorizationPolicy, Func<TClientResource, string> keyGenerator, IModelValidator<TServerResource> modelValidator)
        {
            this.resourceManager_ = resourceManager;
            this.httpSessionManager_ = httpSessionManager;
            this.authorizationPolicy_ = authorizationPolicy;
            this.keyGenerator_ = keyGenerator;
            this.modelValidator_ = modelValidator;

            var modelMapperConfigurationExpression = new MapperConfigurationExpression();
            modelMapperConfigurationExpression.CreateMap<TClientResource, TServerResource>();
            modelMapperConfigurationExpression.CreateMap<TServerResource, TClientResource>();

            var config = new MapperConfiguration(modelMapperConfigurationExpression);
            this.ModelMapper = config.CreateMapper();

        }

        // POST resource/
        [HttpPost]
        public ActionResult<TClientResource> Create([FromBody] TClientResource resource)
        {
            return this.CreateOrUpdate(this.keyGenerator_(resource), resource);
        }

        // PUT resource/5
        [HttpPut("{id}")]
        public ActionResult<TClientResource> CreateOrUpdate(string id, [FromBody] TClientResource resource)
        {
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            var serverResource = this.resourceManager_.Get(id);

            if (serverResource != null)
            {
                // Update path
                if (!this.authorizationPolicy_.CanWrite(user, serverResource))
                {
                    return Unauthorized();
                }

                this.ModelMapper.Map(resource, serverResource);
            }
            else
            {
                // Create path
                if (!this.authorizationPolicy_.CanCreate(user))
                {
                    return Unauthorized();
                }

                serverResource = this.ModelMapper.Map<TServerResource>(resource);
                serverResource.Id = id;
                serverResource.Owner = user != null ? user.Id : serverResource.Id;
            }

            if (!this.modelValidator_.Validate(serverResource))
            {
                return BadRequest("Invalid model");
            }

            return Ok(this.ModelMapper.Map<TClientResource>(resourceManager_.AddOrUpdate(serverResource)));
        }

        // DELETE resource/5
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(string id)
        {
            var user = this.httpSessionManager_.GetUserFromRequest(Request);

            var serverResource = resourceManager_.Get(id);
            if (serverResource == null)
            {
                return NotFound();
            }

            if (!this.authorizationPolicy_.CanDelete(user, serverResource))
            {
                return Unauthorized();
            }

            return Ok(resourceManager_.Delete(id));
        }

        // GET resource/
        [HttpGet]
        public ActionResult<IEnumerable<TClientResource>> Get()
        {
            var user = this.httpSessionManager_.GetUserFromRequest(Request);
            var resources = resourceManager_.GetAll().ToList();
            var authorizedResources = resources.Where(resource => this.authorizationPolicy_.CanRead(user, resource)).ToList();

            return Ok(this.ModelMapper.Map<IEnumerable<TClientResource>>(authorizedResources));
        }

        // GET resource/5
        [HttpGet("{id}")]
        public ActionResult<TClientResource> Get(string id)
        {
            var user = this.httpSessionManager_.GetUserFromRequest(Request);

            var serverResource = resourceManager_.Get(id);
            if (serverResource == null)
            {
                return NotFound();
            }

            if (!this.authorizationPolicy_.CanRead(user, serverResource))
            {
                return Unauthorized();
            }

            return Ok(this.ModelMapper.Map<TClientResource>(serverResource));
        }
    }
}
