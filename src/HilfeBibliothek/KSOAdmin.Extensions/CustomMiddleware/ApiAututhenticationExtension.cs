using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

using KSOAdmin.Core.AppsettingHelper;
using KSOAdmin.Extensions.AuthorizationUtility.CreateToken;
using KSOAdmin.Models.Core;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

namespace KSOAdmin.Extensions.CustomMiddleware
{
    /// <summary>
    /// 用于每次请求都验证Token
    /// </summary>
    public static class ApiAututhenticationExtension
    {
        public static IServiceCollection AututhenticationExtension(this IServiceCollection services)
        {
            string KeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.public.json");
            if (!File.Exists(KeyPath))
            {
                throw new Exception("没有找到公钥");
            }
            string key = File.ReadAllText(KeyPath);
            Console.WriteLine($"KeyPath:{KeyPath}");
            var keyParams = JsonConvert.DeserializeObject<RSAParameters>(key);
            SigningCredentials credentials = new SigningCredentials(new RsaSecurityKey(keyParams), SecurityAlgorithms.RsaSha256Signature);
            JWTTokenOptions tokenOptions = Appsettings.GetConfigClass<JWTTokenOptions>("JWTTokenOptions");
            services.AddAuthentication(Appsettings.app("Startup","IdentityServer4", "ApiName")) // 获取配置文件
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidIssuer = tokenOptions.Issuer,
                    ValidateAudience = true,//是否验证Audience
                    ValidAudience = tokenOptions.Audience,//Audience 
                    ValidateLifetime = true,//是否验证失效时间 
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey 
                    IssuerSigningKey = new RsaSecurityKey(keyParams),
                };

                //如果验证不通过，可以给一个时间注册一个动作，这动作就是指定返回的结果；
                options.Events = new JwtBearerEvents
                {
                    //此处为权限验证失败后触发的事件
                    OnChallenge = context =>
                    {
                        //此处代码为终止.Net Core默认的返回类型和数据结果 
                        context.HandleResponse();
                        //自定义返回的数据类型
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        //没有权限
                        context.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseModel()
                        {
                            Msg = "请首先登录",
                            Code = 401
                        }));
                        return Task.FromResult(0);
                    }
                };
            });
            return services;
        }
    }
}
