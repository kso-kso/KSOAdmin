using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;

namespace KSOAdmin.Core.AppsettingHelper
{
    /// <summary>
    /// 获取配置文件的操作类
    /// </summary>
    public class Appsettings
    {
       private static IConfiguration Configuration { get; set; }

        public Appsettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 获取的字符串
        /// </summary>
        /// <param name="configkey"></param>
        /// <returns></returns>
        public static string app(params string[] configkey)
        {
            try
            {
                if (configkey.Any())
                {
                    return Configuration[string.Join(":", configkey)];
                }
            }
            catch (Exception)
            {
                ConsoleHelper.ConsoleHelper.WriteErrorLine(" 如下节点配置获取错误："+ string.Join(":", configkey));
            }
            return "";
        }

        /// <summary>
        /// 获取的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configkey"></param>
        /// <returns></returns>
        public static T GetConfigClass<T>(params string[] configkey) where T : class, new()
        {
            try
            {
                T t = new();
                if (configkey.Any())
                {
                    Configuration.Bind(string.Join(":", configkey), t);
                    Configuration.GetValue<T>(string.Join(":", configkey), t);
                }
                return t;
            }
            catch (Exception)
            {
                ConsoleHelper.ConsoleHelper.WriteErrorLine(" 如下节点配置获取错误：" + string.Join(":", configkey));
            }
            return default(T);
        }

        /// <summary>
        /// 获取的List集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configkey"></param>
        /// <returns></returns>
        public static List<T> app<T>(params string[] configkey) where T : class, new()
        {
            List<T> list = new List<T>();
            try
            {
                if (configkey.Any())
                {
                    Configuration.Bind(string.Join(":", configkey), list);
                }
            }
            catch (Exception)
            {
                ConsoleHelper.ConsoleHelper.WriteErrorLine(" 如下节点配置获取错误：" + string.Join(":", configkey));
            }
            return list;
        }

    }
}
