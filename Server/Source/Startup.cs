namespace todoweb.Server
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NJsonSchema;
    using NSwag.AspNetCore;

    using todoweb.Server.Core;
    using todoweb.Server.Models;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            services.AddResponseCompression(opts =>
            {
                var mimeTypes = new List<string> { "application/octet-stream" };
                mimeTypes.AddRange(ResponseCompressionDefaults.MimeTypes);
                opts.MimeTypes = mimeTypes;
            });
            var todoManager = new ResourceManager<Todo>();
            services.AddSingleton<IResourceManager<Todo>>(todoManager);
            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            app.Map("/bzr", child => { child.UseBlazor<Client.Blazor.Program>(); });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUi3();
        }
    }
}
