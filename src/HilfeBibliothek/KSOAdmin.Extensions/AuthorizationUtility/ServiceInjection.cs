using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KSOAdmin.Core.AppsettingHelper;
using KSOAdmin.Core.EFDbContext;
using KSOAdmin.Extensions.AuthorizationUtility.CreateToken;
using KSOAdmin.IRepository.System;
using KSOAdmin.IServices;
using KSOAdmin.IServices.System;
using KSOAdmin.Repository.System;
using KSOAdmin.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KSOAdmin.Extensions.AuthorizationUtility
{
    /// <summary>
    /// 在权限注入里面，只需要用到个别的service，切使用方式相对固定,无需全部批量注入，减少性能损耗
    /// </summary>
    public static class ServiceInjection
    {
        /// <summary>
        /// 增加注入
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ServiceDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<ICreateVerificationCode, CreateVerificationCode>();
            services.AddTransient<ICreateJWTService, CreateJWTService>();
            //services.AddTransient<ISys_UserRepository, Sys_UserRepository>();
            //services.AddTransient<ISys_UserServices, ISys_UserServices>();
            return services;
        }

    }
}
