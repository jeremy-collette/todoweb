﻿namespace todoweb.Server
{
    using System.Collections.Generic;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using todoweb.Server.Core;

    [Route("/home")]
    public class HomeController : Controller
    {
        private IResourceManager<Server.Models.Todo> resourceManager_;
        private IMapper modelMapper_;

        public HomeController(IResourceManager<Server.Models.Todo> resourceManager)
        {
            this.resourceManager_ = resourceManager;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Server.Models.Todo, Client.Models.Todo>();
            });
            modelMapper_ = config.CreateMapper();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return new OkObjectResult("Hello");
        }
    }
}