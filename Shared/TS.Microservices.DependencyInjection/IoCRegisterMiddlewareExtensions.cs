using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace TS.Microservices.DependencyInjection
{
    /// <summary>
    /// 配置默认IoC容器
    /// </summary>
    public static class IoCRegisterMiddlewareExtensions
    {
        /// <summary>
        /// 配置IoC容器Factory
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseAppServiceProviderFactory(this IHostBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }

    }
}
