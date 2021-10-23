using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;


namespace KSOAdmin.Core
{
    public class CreateInstance
    {
        public static TService GetService<TService>() where TService : class
        {
            return typeof(TService).GetService() as TService;
        }
         
    }

    public static class ServiceProviderManagerExtension
    {
        public static object GetService(this Type serviceType)
        {
            return  HttpContext.Current.RequestServices.GetService(serviceType);
        }

    }
    public static class HttpContext
    {
        private static IHttpContextAccessor _accessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}
