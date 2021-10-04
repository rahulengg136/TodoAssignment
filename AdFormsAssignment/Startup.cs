using AdFormAssignment.DAL;
using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Implementations;
using AdFormsAssignment.BLL;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.BLL.Implementations;
using AdFormsAssignment.Configuration;
using AdFormsAssignment.CustomMiddlewares;
using AdFormsAssignment.GraphQL;
using AdFormsAssignment.Security;
using CorrelationId.CorrelationIdWork;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace AdFormsAssignment
{
    /// <summary>
    /// Start up class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Start up constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
      
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                options =>
                {
                    options.RespectBrowserAcceptHeader = true;

                }
                ).AddNewtonsoftJson();

            //authentication work
            var key = "todo-assignment-token-private-key";
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                };
            });

            services.AddSingleton<IJwtAuth>(new JwtAuth(key));

            // swagger work
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<AddParametersToSwagger>();
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Swagger API",
                    Description = "API for swagger",
                    Version = "v1"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter token here",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                        Reference=new OpenApiReference{
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                    },
                        new string[]{ }
                    }
                });

            });

            var ConnectionString = Configuration.GetConnectionString("MyConn");

            //Entity Framework  
            services.AddDbContext<MyProjectContext>(options => options.UseSqlServer(ConnectionString));

            // registering services
            services.AddScoped<ITodoListDal, TodoListDal>();
            services.AddScoped<ITodoItemDal, TodoItemDal>();
            services.AddScoped<ILabelDAL, LabelDal>();
            services.AddScoped<IUserDal, UserDal>();

            services.AddScoped<ITodoListService, ToDoListService>();
            services.AddScoped<ITodoItemService, TodoItemService>();
            services.AddScoped<ILabelService, LabelService>();
            services.AddScoped<IUserService, UserService>();

            services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());
            //auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // graphQL
            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(
               s.GetRequiredService
               ));
            services.AddScoped<GraphQLSchema>();
            services.AddGraphQL(o => { o.ExposeExceptions = true; })
                .AddGraphTypes(ServiceLifetime.Scoped)
                ;

        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"> Application builder</param>
        /// <param name="env"> Host environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseGraphQL<GraphQLSchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

            app.UseRequestResponseLogging();
            app.UseMiddleware<CorrelationMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger API");
            });

        }
    }
}
