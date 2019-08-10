namespace todoweb.Server
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using todoweb.Server.Contract;
    using todoweb.Server.Core;
    using todoweb.Server.Core.Contract;
    using todoweb.Server.Models;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddSession(opts => opts.Cookie.IsEssential = true);
            services.AddMvc(o => o.EnableEndpointRouting = false).AddNewtonsoftJson();
            services.AddResponseCompression(opts =>
            {
                var mimeTypes = new List<string> { "application/octet-stream" };
                mimeTypes.AddRange(ResponseCompressionDefaults.MimeTypes);
                opts.MimeTypes = mimeTypes;
            });
            services.AddScoped<IResourceManager<User>, DatabaseResourceManager<User>>();
            services.AddScoped<IResourceManager<Todo>, DatabaseResourceManager<Todo>>();
            services.AddScoped<IAuthorizationPolicy<User>, UserAuthorizationPolicy>();
            services.AddScoped<IAuthorizationPolicy<Todo>, TodoAuthorizationPolicy>();
            services.AddScoped<IHttpSessionManager, HttpSessionManager>();
            services.AddSwaggerDocument();

            services.AddDbContext<DatabaseContext<Todo>>(o => o.UseSqlServer(this.Configuration.GetConnectionString("TodoDatabase")));
            services.AddDbContext<DatabaseContext<User>>(o => o.UseSqlServer(this.Configuration.GetConnectionString("UserDatabase")));
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

            app.UseClientSideBlazorFiles<Client.Blazor.Program>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapFallbackToClientSideBlazor<Client.Blazor.Program>("index.html");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUi3();
        }
    }
}
