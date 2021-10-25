using Autofac;
using HttpClientFactoryDemo.Clients;
using HttpClientFactoryDemo.DelegatingHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Reflection;
using TS.Microservices.DependencyInjection;

namespace HttpClientFactoryDemo
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HttpClientFactoryDemo", Version = "v1" });
            });

            services.AddHttpClient();
            services.AddHttpClient("NamedServiceClient", client =>
            {
                client.DefaultRequestHeaders.Add("client-name", "namedclient");
                client.BaseAddress = new Uri("https://localhost:5011");
            }).SetHandlerLifetime(TimeSpan.FromMinutes(20));
          
            services.AddHttpClient<TypedServiceClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5011");
            }).AddHttpMessageHandler(provider => provider.GetService<RequestIdDelegatingHandler>()); 
        }

        /// <summary>
        /// 通过Autofac扩展服务注册功能，在ConfigureServices之后执行
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblies = new[]
                {
                    "HttpClientFactoryDemo"
                }
                .Select(s => Assembly.Load(s))
                .ToArray();
            builder.RegisterDefaultServices(assemblies);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HttpClientFactoryDemo v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
