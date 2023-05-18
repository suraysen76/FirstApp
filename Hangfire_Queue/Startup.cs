using Hangfire;
using Hangfire.MySql;
using HangfireQueueJobs.Interface;
using HangfireQueueJobs.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Transactions;

namespace HangfireTest1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Hangfire Queue API",
                    Description = "Hangfire Queue API Swagger",
                    Version = "v1"
                });
            });

            services.AddHangfire(configuration =>
            configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(new MySqlStorage(
                    Configuration["Database:hangfireConnection"],
                    new MySqlStorageOptions
                    {
                 
                        TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 50000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        TablesPrefix = "hangfire_"
                    })));




            //services.AddHangfireServer(options =>
            //{
            //    options.Queues = new[] { "a_queue","b_queue", "default" };
            //    options.WorkerCount = Environment.ProcessorCount * 5;
            //});

  

            ////default Queue
            services.AddHangfireServer(options =>
            {
                options.ServerName = $"{Environment.MachineName}:a";
                options.Queues = new[] { "secondary_queue" };
                options.WorkerCount = 5;
            });

            services.AddHangfireServer(options =>
            {
                options.ServerName = $"{Environment.MachineName}:b";
                options.Queues = new[] { "default" };
                options.WorkerCount = 15;// Environment.ProcessorCount * 5-5;

            });
            //services.AddHangfireServer(options => options.WorkerCount = Environment.ProcessorCount * 5 / numOfQueues);

            services.AddScoped<IHangfireQueueJobsService, HangfireQueueJobsService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            int numOfQueues = 2;
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hangfire Test API");
                options.RoutePrefix = "";
            });
            app.UseHangfireDashboard("/hangfire");
            
           
        }
    }
}
