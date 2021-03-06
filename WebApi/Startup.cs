﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApi.DataLayer;
using WebApi.Utils;

namespace WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(config =>
            {
                //Enable global authorization
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // Add Cors policy
            services.AddCors(options =>
            {
                options.AddPolicy("SMPolicy", builder => builder.AllowAnyOrigin()
                                                                .AllowAnyMethod()
							                                    .AllowAnyHeader()
							                                    .AllowCredentials());
            });


			// Add Configuration Settings
			services.Configure<DataLayer.MongoDB.Data.MongoSettings>(options => Configuration.GetSection("MongoConnection").Bind(options));
			services.Configure<Utils.TokenSettings>(options => Configuration.GetSection("TokenSettings").Bind(options));
            services.Configure<Utils.SocketSettings>(options => Configuration.GetSection("SocketSettings").Bind(options));

            services.AddSingleton<SocketPublisher>();


			TokenSettings settings = new TokenSettings();
			Configuration.GetSection("TokenSettings").Bind(settings);

			services.AddTokenAuthentication(settings);


			// Add Dependencies
			services.AddTransient<IMirrorRepository, DataLayer.MongoDB.MirrorRepository>();
            services.AddTransient<ITicketRepository, DataLayer.MongoDB.TicketRepository>();
            services.AddTransient<IUserRepository, DataLayer.MongoDB.UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            app.UseCors("SMPolicy");

			app.UseAuthentication();

            app.UseMvc();
        }
    }
}
