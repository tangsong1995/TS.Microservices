using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using TS.Microservices.Infrastructure;
using TS.Microservices.WebApi.Extensions;

namespace TS.Microservices.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        internal static bool Ready { get; set; } = true;
        internal static bool Live { get; set; } = true;


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            services.AddHealthChecks()
                .AddMySql(Configuration.GetValue<string>("Mysql"), "mysql", tags: new string[] { "mysql", "live", "all" })
                .AddRabbitMQ(s =>
                {
                    var connectionFactory = new RabbitMQ.Client.ConnectionFactory();
                    Configuration.GetSection("RabbitMQ").Bind(connectionFactory);
                    return connectionFactory;
                }, "rabbitmq", tags: new string[] { "rabbitmq", "live", "all" })
                .AddCheck("liveChecker", () =>
                {
                    return Live ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
                }, new string[] { "live", "all" })
                .AddCheck("readyChecker", () =>
                {
                    return Ready ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
                }, new string[] { "ready", "all" });


            var secrityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]));
            services.AddSingleton(secrityKey);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(30),//失效时间的偏离时间，在失败后的指定时间内有效
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = "localhost",//Audience
                    ValidIssuer = "localhost",//Issuer
                    IssuerSigningKey = secrityKey//拿到SecurityKey
                };
            });

            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TS.Microservices.WebApi", Version = "v1" });
            });


            services.AddMediatRServices();
            services.AddMySqlDomainContext(Configuration.GetValue<string>("Mysql"));
            services.AddRepositories();
            services.AddEventBus(Configuration);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (Configuration.GetValue("USE_PathBase", false))
            {
                app.Use((context, next) =>
                {
                    //context.Request.PathBase = new PathString("/mobile");
                    return next();
                });
            }

            if (Configuration.GetValue("USE_Forwarded_Headers", false))
            {
                app.UseForwardedHeaders();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<OrderingContext>();
                context.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TS.Microservices.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/live", new HealthCheckOptions { Predicate = registration => registration.Tags.Contains("live") });
                endpoints.MapHealthChecks("/ready", new HealthCheckOptions { Predicate = registration => registration.Tags.Contains("ready") });
                endpoints.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapGrpcService<Grpc.OrderServiceImpl>();
                endpoints.MapControllers();
            });
        }
    }
}
