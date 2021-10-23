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
                    //���� Log4net
                     .ConfigureLogging((webhost, logging) =>
                     {
                         //�޸�Ĭ����־����
                         logging.AddFilter("System", LogLevel.Error);
                         logging.AddFilter("Microsoft", LogLevel.Error);
                         // 3.ͳһ����
                         logging.SetMinimumLevel(LogLevel.Error);
                         // Ĭ��log4net.confg
                         logging.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Config/Log4net.config"));
                     }); ;
                      
                });
    }
}
