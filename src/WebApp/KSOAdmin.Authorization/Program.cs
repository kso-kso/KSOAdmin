using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;

namespace KSOAdmin.Authorization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:8102");
                    webBuilder.UseStartup<Startup>() 
                    //配置 Log4net
                     .ConfigureLogging((webhost, logging) =>
                     {
                         //修改默认日志级别
                         logging.AddFilter("System", LogLevel.Error);
                         logging.AddFilter("Microsoft", LogLevel.Error);
                         // 3.统一设置
                         logging.SetMinimumLevel(LogLevel.Error);
                         // 默认log4net.confg
                         logging.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Config/Log4net.config"));
                     }); ;
                      
                });
    }
}
