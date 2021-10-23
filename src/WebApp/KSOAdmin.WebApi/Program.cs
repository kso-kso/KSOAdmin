using System.IO;

using Autofac.Extensions.DependencyInjection;

using KSOAdmin.Extensions.LogSystem;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KSOAdmin.WebApi
{
    /*
 *                        _oo0oo_
 *                       o8888888o
 *                       88" . "88
 *                       (| -_- |)
 *                       0\  =  /0
 *                     ___/`---'\___
 *                   .' \\|     |// '.
 *                  / \\|||  :  |||// \
 *                 / _||||| -:- |||||- \
 *                |   | \\\  - /// |   |
 *                | \_|  ''\---/''  |_/ |
 *                \  .-\__  '-'  ___/-. /
 *              ___'. .'  /--.--\  `. .'___
 *           ."" '<  `.___\_<|>_/___.' >' "".
 *          | | :  `- \`.;`\ _ /`;.`/ - ` : | |
 *          \  \ `_.   \_ __\ /__ _/   .-` /  /
 *      =====`-.____`.___ \_____/___.-`___.-'=====
 *                        `=---='
 * 
 * 
 *      ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * 
 *            ���汣��       ����崻�     ����BUG
 */
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              //֧��autofac
              .UseServiceProviderFactory(new AutofacServiceProviderFactory())

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //����urls
                    webBuilder.UseUrls("https://*:8101");
                    webBuilder.UseKestrel().UseUrls("https://*:8101");

                    //֧��iis
                    webBuilder.UseIIS();
                    webBuilder.UseStartup<Startup>()

                    //���������־
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.Sources.Clear();
                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                        //����Apollo��������
                        config.AddConfigurationApollo("appsettings.apollo.json");
                    })

                    //���� Log4net
                     .ConfigureLogging((webhost, logging) =>
                    {
                        //�޸�Ĭ����־����
                        logging.AddFilter("System", LogLevel.Error);
                        logging.AddFilter("Microsoft", LogLevel.Error);
                        // 3.ͳһ����
                        logging.SetMinimumLevel(LogLevel.Error);
                        // Ĭ��log4net.confg
                        logging.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "ConfigFile/Log4net.config"));
                    });
                });
    }
}
