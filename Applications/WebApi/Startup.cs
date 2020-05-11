using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Loader;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FinanceApi.Repositories;
using FinanceApi.Repositories.Base;
using FinanceApi.Services;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using WebApi.Authorization;
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
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        /// <summary>
        /// configuration of web site
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Autofac container
        /// </summary>
        public ILifetimeScope AutofacContainer { get; private set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">web site service</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllers();

            services.AddHangfire(x =>
                {
                    x.UseMemoryStorage();
                });
            services.Configure<ConnectionSetting>(Configuration.GetSection("ConnectionStrings"));
        }

        /// <summary>
        /// register by autofac
        /// </summary>
        /// <param name="builder">autofac container builder</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac, like:
            builder.RegisterAssemblyTypes(typeof(StockService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(StockRepo).Assembly)
                .Where(t => t.Name.EndsWith("Repo"))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = 1
            });

            app.UseOwin();

            app.UseHangfireDashboard("/hangfire",
                new DashboardOptions
                {
                    Authorization = new[] { new NeedlessDashboardAuthorizationFilter() }
                });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            DbProviderFactories.RegisterFactory("System.Data.MySql", MySqlClientFactory.Instance);

            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            GlobalConfiguration.Configuration.UseAutofacActivator(this.AutofacContainer);

            RecurringJob.AddOrUpdate<ExchangeGrabSchedule>(x => x.Grab(), Cron.Daily(16));
            RecurringJob.AddOrUpdate<StcokInfoGrabSchedule>(x => x.Grab(), Cron.Daily(16));
        }
    }
}