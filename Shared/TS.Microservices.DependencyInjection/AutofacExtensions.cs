using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using System;
using System.Linq;
using System.Reflection;
using TS.Microservices.DependencyInjection.IServices;

namespace TS.Microservices.DependencyInjection
{
    public static class AutofacExtensions
    {
        /// <summary>
        /// 注册实现了ISingletonService、IScopedService、ITransientService的子类
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies">待扫描的程序集</param>
        public static void RegisterDefaultServices(this ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterSingletonServices(assemblies);
            builder.RegisterScopedServices(assemblies);
            builder.RegisterTransientServices(assemblies);
        }

        /// <summary>
        /// 扫描指定的程序集，批量注册继承ISingletonService的类
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies">待扫描的程序集</param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterSingletonServices(this ContainerBuilder builder, Assembly[] assemblies)
        {
            return builder.RegisterAssemblyServices<ISingletonService>(assemblies).SingleInstance();
        }

        /// <summary>
        /// 扫描指定的程序集，批量注册继承IScopedService的类
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies">待扫描的程序集</param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterScopedServices(this ContainerBuilder builder, Assembly[] assemblies)
        {
            return builder.RegisterAssemblyServices<IScopedService>(assemblies).InstancePerLifetimeScope();
        }

        /// <summary>
        /// 扫描指定的程序集，批量注册继承ITransientService的类
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies">待扫描的程序集</param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterTransientServices(this ContainerBuilder builder, Assembly[] assemblies)
        {
            return builder.RegisterAssemblyServices<ITransientService>(assemblies).InstancePerDependency();
        }

        /// <summary>
        /// 通过指定的程序集进行批量注册
        /// </summary>
        /// <typeparam name="T">注册实现了T类型的非虚类。如果T是接口，会同时注册到该接口类型中</typeparam>
        /// <param name="builder"></param>
        /// <param name="assemblies">待扫描的程序集</param>
        /// <param name="serviceNameMapping">可以指定注册的映射关系。将类和指定key关联，以便能根据key获取指定类实例</param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAssemblyServices<T>(this ContainerBuilder builder, Assembly[] assemblies, Func<Type, string> serviceNameMapping = null) where T : class
        {
            if (assemblies == null || !assemblies.Any())
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var registrationBuilder = (from t in builder.RegisterAssemblyTypes(assemblies)
                                       where typeof(T).IsAssignableFrom(t) && !t.IsAbstract
                                       select t).AsSelf();
            if (typeof(T).IsInterface)
            {
                registrationBuilder = registrationBuilder.AsImplementedInterfaces();
            }

            if (serviceNameMapping != null)
            {
                registrationBuilder = registrationBuilder.Named<T>(serviceNameMapping);
            }

            return registrationBuilder;
        }
    }
}
