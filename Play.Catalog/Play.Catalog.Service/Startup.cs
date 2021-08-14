using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Play.Catalog.Service.Contracts.v1.Items;
using Play.Common.Identity;
using Play.Common.MassTransit;
using Play.Common.MongoDb;

namespace Play.Catalog.Service
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMongoOptions()
                .AddMongoRepository("items")
                .AddMassTransitWithRabbitMq()
                .AddJwtBearerAuthentication();

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Policies.Read, policy =>
                {
                    policy.RequireRole("Admin");
                    policy.RequireClaim("scope", "catalog.read-access", "catalog.full-access");
                });

                opt.AddPolicy(Policies.Write, policy =>
                {
                    policy.RequireRole("Admin");
                    policy.RequireClaim("scope", "catalog.write-access", "catalog.full-access");
                });
            });

            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Play.Catalog.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play.Catalog.Api v1"));

                app.UseCors(builder =>
                {
                    builder.WithOrigins(_configuration.GetValue<string>("AllowedOrigin"));
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}