namespace todoweb.Server
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using todoweb.Server.Contract;
    using todoweb.Server.Core;
    using todoweb.Server.Models;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(opts => opts.Cookie.IsEssential = true);
            services.AddMvc(o => o.EnableEndpointRouting = false).AddNewtonsoftJson();
            services.AddResponseCompression(opts =>
            {
                var mimeTypes = new List<string> { "application/octet-stream" };
                mimeTypes.AddRange(ResponseCompressionDefaults.MimeTypes);
                opts.MimeTypes = mimeTypes;
            });
            services.AddSingleton<IResourceManager<Todo>>(new ResourceManager<Todo>());
            var userManager = new ResourceManager<User>();
            services.AddSingleton<IResourceManager<User>>(userManager);
            var sessionManager = new SessionManager(userManager);
            services.AddSingleton<IHttpSessionManager>(new HttpSessionManager(sessionManager));
            services.AddSingleton<IAuthorizationPolicy<User>>(new UserAuthorizationPolicy());
            services.AddSingleton<IAuthorizationPolicy<Todo>>(new TodoAuthorizationPolicy());
            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            app.UseMvc();

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
