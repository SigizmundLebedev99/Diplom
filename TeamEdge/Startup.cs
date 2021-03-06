﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TeamEdge.WebLayer;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.JWT;
using TeamEdge.Mapper;
using TeamEdge.BusinessLogicLayer.History;
using TeamEdge.DAL.Mongo;
using Microsoft.AspNetCore.StaticFiles;

namespace TeamEdge
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
            services.AddDbContext<TeamEdgeDbContext>(c =>
            {
                c.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                c.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CommonProfile>();
            });
            var mapper = config.CreateMapper();

            services.AddSingleton(mapper);

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            });

            services.AddIdentity<User, IdentityRole<int>>().AddEntityFrameworkStores<TeamEdgeDbContext>().AddDefaultTokenProviders();

            services.AddCustomAuthentication(Configuration);

            services.AddSingleton<PathParams>();
            services.AddTransient<IMembershipService, MembershipService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IRepositoryService, RepositoryService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFileWorkService, FileWorkService>();
            services.AddTransient<IValidationService, ValidationService>();
            services.AddSingleton<FileSystemService>();
            services.AddTransient<IWorkItemService, WorkItemService>();
            services.AddTransient<ISprintService, SprintService>();
            services.AddTransient<IHistoryService, HistoryService>();
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<AccountService>();

            services.AddMvc();
            services.AddMemoryCache();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TeamEdgeAPI", Version = "v0.1" });

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "TeamEdge.xml");
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                { "Bearer", Enumerable.Empty<string>() },
            });
            });

            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                c.DocExpansion(DocExpansion.None);
            });
           
            app.UseMvc();
        }
    }
}
