using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;

using Autofac;

using KSOAdmin.Core.AppsettingHelper;
using KSOAdmin.Core.EFDbContext;
using KSOAdmin.Extensions.CustomMiddleware;
using KSOAdmin.Extensions.LogSystem;
using KSOAdmin.IServices;
using KSOAdmin.WebApi.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.WebEncoders;
using Microsoft.OpenApi.Models;

namespace KSOAdmin.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        private IServiceCollection Services { get; set; }
        public void ConfigureServices(IServiceCollection services)
        {
            Services= services;
            services.AddSingleton(new Appsettings(Configuration));
            ///ʹ��AutoMapper
            services.AddAutoMapper(typeof(AutoMapperServices));
            #region GZIP
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            }).Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            }).AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    "text/html; charset=utf-8",
                    "application/xhtml+xml",
                    "application/atom+xml",
                    "image/svg+xml"
                  });
            });
            #endregion

            //������������
             services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

            //��Ȩ��Ȩ��
            services.AututhenticationExtension();

            services.AddAppConfigSetup();
            services.AddControllers();

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo()
                    {
                        Title = $"{version}:Swagger�ĵ�",
                        Version = version,
                        Description = $"Panda.Sewerage :  {version}  "
                    });
                });

                string basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                string xmlPath = Path.Combine(basePath, "Sewerage.Web.xml");
                c.IncludeXmlComments(xmlPath, true);
            });
            #endregion

            #region �м��֧�ֿ������� 
           services.AddCors(option => option.AddPolicy(Appsettings.app("Startup", "Cors"), _build => _build.AllowAnyOrigin().AllowAnyMethod()));
            #endregion

            #region ͨ�����ü������ļ��ϴ��Ĵ�С����
            services.Configure<FormOptions>(o =>
            {
                o.BufferBodyLengthLimit = long.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = long.MaxValue;
                o.MultipartBoundaryLengthLimit = int.MaxValue;
                o.MultipartHeadersCountLimit = int.MaxValue;
                o.MultipartHeadersLengthLimit = int.MaxValue;
            }); 
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    //if (env.IsDevelopment())
                    //{
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{version}");
                    //}
                    //else
                    //{
                    //    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{version}");
                    //}
                });
            });
            #endregion
            app.UseHsts();
            #region ���� 
            app.UseCors(Appsettings.app("Startup", "Cors"));
            #endregion
            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
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
