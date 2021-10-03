using AdFormAssignment.DAL;
using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Implementations;
using AdFormsAssignment.BLL;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.BLL.Implementations;
using AdFormsAssignment.CustomMiddlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AdFormsAssignment.Security;
using CorrelationId.CorrelationIdWork;

namespace AdFormsAssignment
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
            services.AddControllers().AddNewtonsoftJson();

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
              //  options.OperationFilter<AddParametersToSwagger>();
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Swagger API",
                    Description = "API for swagger",
                    Version = "v1"
                });

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
            services.AddScoped<ITodoListDAL, TodoListDAL>();
            services.AddScoped<ITodoItemDAL, TodoItemDAL>();
            services.AddScoped<ILabelDAL, LabelDAL>();
            services.AddScoped<IUserDAL, UserDAL>();

            services.AddScoped<ITodoListService, ToDoListService>();
            services.AddScoped<ITodoItemService, TodoItemService>();
            services.AddScoped<ILabelService, LabelService>();
            services.AddScoped<IUserService, UserService>();

            //auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //var path = Directory.GetCurrentDirectory();
            //loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRequestResponseLogging();
            app.UseMiddleware<CorrelationMiddleware>();
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
