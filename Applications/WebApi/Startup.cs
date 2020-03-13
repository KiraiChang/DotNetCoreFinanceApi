using System;
using System.Data;
using System.Runtime.Loader;
using Autofac;
using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using WebApi.Schedules;

namespace FinanceApi
{
    /// <summary>
    /// web site startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="env">web hosting environment</param>
        public Startup(IWebHostEnvironment env)
        {
            // In ASP.NET Core 3.0 `env` will be an IWebHostingEnvironment, not IHostingEnvironment.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        /// <summary>
        /// configuration of web site
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">web site service</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllers();

            services.AddHangfire(x => x.UseStorage(new MySqlStorage(Configuration.GetConnectionString("Hangfire"), new MySqlStorageOptions() { TablePrefix = "Hangfire", PrepareSchemaIfNecessary = true })));
        }

        /// <summary>
        /// register by autofac
        /// </summary>
        /// <param name="builder">autofac container builder</param>
        /// <returns>build-ed container</returns>
        public IContainer ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac, like:
            builder
                .Register<IDbConnection>(c => new MySqlConnection(Configuration.GetConnectionString("Default")))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(AssemblyLoadContext.Default.LoadFromAssemblyPath(AppDomain.CurrentDomain.BaseDirectory + "FinanceApi.Services.Dll"))
                .Where(t => t.Name.EndsWith("Service"))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(AssemblyLoadContext.Default.LoadFromAssemblyPath(AppDomain.CurrentDomain.BaseDirectory + "FinanceApi.Repositories.Dll"))
                .Where(t => t.Name.EndsWith("Repo"))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

            var container = builder.Build();
            GlobalConfiguration.Configuration.UseAutofacActivator(container);
            return container;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">web application</param>
        /// <param name="env">environment setting</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = 1
            });

            app.UseOwin();

            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            RecurringJob.AddOrUpdate<StcokGrabSchedule>(x => x.Grab("0050"), Cron.Daily(16));
        }
    }
}