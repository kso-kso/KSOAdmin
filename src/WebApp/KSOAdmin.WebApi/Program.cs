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
 *            佛祖保佑       永不宕机     永无BUG
 */
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              //支持autofac
              .UseServiceProviderFactory(new AutofacServiceProviderFactory())

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //配置urls
                    webBuilder.UseUrls("https://*:8101");
                    webBuilder.UseKestrel().UseUrls("https://*:8101");

                    //支持iis
                    webBuilder.UseIIS();
                    webBuilder.UseStartup<Startup>()

                    //输出配置日志
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.Sources.Clear();
                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                        //接入Apollo配置中心
                        config.AddConfigurationApollo("appsettings.apollo.json");
                    })

                    //配置 Log4net
                     .ConfigureLogging((webhost, logging) =>
                    {
                        //修改默认日志级别
                        logging.AddFilter("System", LogLevel.Error);
                        logging.AddFilter("Microsoft", LogLevel.Error);
                        // 3.统一设置
                        logging.SetMinimumLevel(LogLevel.Error);
                        // 默认log4net.confg
                        logging.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "ConfigFile/Log4net.config"));
                    });
                });
    }
}
