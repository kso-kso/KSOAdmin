using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;

using log4net;
using KSOAdmin.Core.EFDbContext;
using Microsoft.Extensions.DependencyModel;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using KSOAdmin.Extensions.AuthorizationUtility;
using KSOAdmin.Extensions.AuthorizationUtility.CreateToken;
using KSOAdmin.Core.CacheHelper.Service;
using KSOAdmin.Core.CacheHelper.Interface;

namespace KSOAdmin.Extensions.CustomMiddleware
{
    public static class AutofacContainerModuleExtension
    {
        public static IServiceCollection AddModules(this IServiceCollection services, ContainerBuilder builder)
        {
            Type baseType = typeof(IDependency);
            var compilationLibrary = DependencyContext.Default
                .CompileLibraries
                .Where(x => !x.Serviceable
                && x.Type == "project")
                .ToList();
            var count1 = compilationLibrary.Count;
            List<Assembly> assemblyList = new List<Assembly>();

            foreach (var _compilation in compilationLibrary)
            {
                try
                {
                    assemblyList.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(_compilation.Name)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(_compilation.Name + ex.Message);
                }
            }
            builder.RegisterAssemblyTypes(assemblyList.ToArray())
             .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
             .AsSelf().AsImplementedInterfaces()
             .InstancePerLifetimeScope();

            #region Redis
            builder.RegisterType<RedisCacheService>().As<ICacheService>();
            #endregion

            builder.RegisterType<CreateVerificationCode >().As<ICreateVerificationCode>();
            builder.RegisterType<CreateJWTService >().As<ICreateJWTService>();

            string connctionstring = DBConntionString.GetDbString().Connection;
            services.AddDbContextPool<KSOContext>(optionsBuilder => { optionsBuilder.UseMySql(connctionstring); });

            return services;
        }

    }
}
