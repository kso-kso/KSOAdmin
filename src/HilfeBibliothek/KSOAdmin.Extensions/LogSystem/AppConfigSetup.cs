using KSOAdmin.Core;
using KSOAdmin.Core.AppsettingHelper;
using KSOAdmin.Core.ConsoleHelper;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Text;

namespace KSOAdmin.Extensions.LogSystem
{
    public static class AppConfigSetup
    {
        public static void AddAppConfigSetup(this IServiceCollection services )
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
         
            if (Appsettings.app(new string[] { "Startup", "AppConfigAlert" }).ObjToBool())
            {

                 ConsoleHelper.WriteSuccessLine(@"                                           
                        ....     ...           ...........             .........
                        ....   ...            ....       ...       ....        ....
                        .... ...             ....                 ....           ....
                        .......                ....              ....             ....  
                        ......                   ....           ....               ....    
                        .......                     ....        ....              ....     
                        ....  ...                     ....      ....              ....
                        ....    ...                    ....     ....             ....
                        ....      ...                  ....      ....           ....
                        ....       ...        ...     ....         .....       ....
                        ....         ...       .........               .........                  ");
                // Redis缓存 
                if (!Appsettings.app(new string[] { "AppSettings", "Redis" }).ObjToBool())
                {
                    Console.WriteLine($"KSOAdmin Redis Start : False  ");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"KSOAdmin Redis Start: True");
                }

                // 日志系统
                if (!Appsettings.app(new string[] { "AppSettings", "LogAOP"}).ObjToBool())
                {
                    Console.WriteLine($"Log4net  Start : False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Log4net  Start : True");
                }

                 
                Console.WriteLine();
            }
        }
    }
}
