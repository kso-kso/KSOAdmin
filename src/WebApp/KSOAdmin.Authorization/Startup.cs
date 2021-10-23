
using KSOAdmin.Core.AppsettingHelper;
using KSOAdmin.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using KSOAdmin.Extensions.ApiFilter;
using KSOAdmin.Extensions.AuthorizationUtility;
using KSOAdmin.Extensions.CustomMiddleware;
using Autofac;
using KSOAdmin.Extensions.AuthorizationUtility.CreateToken;
using System.IO;
using System.Linq;

namespace KSOAdmin.Authorization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private IServiceCollection Services { get; set; }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Services = services;
            services.AddSingleton(new Appsettings(Configuration));
            // services.ServiceDependencyInjection();
            #region �м��֧�ֿ������� 
            //services.AddCors(option => option.AddPolicy(Appsettings.app("Startup", "Cors"), _build => _build.AllowAnyOrigin().AllowAnyMethod()));
            #endregion
            services.AddControllers()
            //��ֹ��дСд����
                .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            }); 
            ///ʹ��AutoMapper
            services.AddAutoMapper(typeof(AutoMapperServices));

            #region ȫ��ע��
            services.AddControllers(option =>
            {
                //option.Filters.Add(typeof(ApiExceptionFilterAttribute));
            });
            #endregion
            services.AddSwaggerGen(c =>
            {
                typeof(Config.ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo()
                    {
                        Title = $"{version}:WebApi",
                        Version = version,
                        Description = $"v1�汾�ļ�Ȩ��Ȩ {version} "
                    });
                });
                string basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                string xmlPath = Path.Combine(basePath, "Config/Sewerage.Web.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    typeof(Config.ApiVersions).GetEnumNames().ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{version}");
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            #region ���� 
            // app.UseCors(Appsettings.app("Startup", "Cors"));
           // app.UseCors("CorsPolicy");
            #endregion
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// autofac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            Services.AddModules(builder);
            
        }
    }
}
