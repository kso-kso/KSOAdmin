using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using KSOAdmin.Models.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;

namespace KSOAdmin.Core.EFDbContext
{
    public  class KSOContext : DbContext, IDependency
    {
        /// <summary>
        /// 连接名字
        /// </summary>

        public KSOContext()
        {
        }

        public KSOContext(DbContextOptions<KSOContext> options) : base(options)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            return base.Set<TEntity>();
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                //加日志处理
                throw (ex.InnerException as Exception ?? ex);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = DBConntionString.GetDbString();
            if ("MySql" == connectionString?.ConnId.ToString())
            {
                optionsBuilder.UseMySql(connectionString.Connection);
            }
            else
            {
                optionsBuilder.UseSqlServer(connectionString.Connection);
            }
            //默认禁用实体跟踪
            optionsBuilder = optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
            ///通过日志输出Sql语句
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Type type = null;
            try
            {
                var compilationLibrary = DependencyContext
                   .Default
                   .CompileLibraries
                   .Where(x => !x.Serviceable && x.Type != "package" && x.Type == "project");
                foreach (var _compilation in compilationLibrary)
                {
                    //加载指定类
                    AssemblyLoadContext.Default
                    .LoadFromAssemblyName(new AssemblyName(_compilation.Name))
                    .GetTypes()
                    .Where(x =>
                        x.GetTypeInfo().BaseType != null
                        && x.BaseType == (typeof(BaseModel)))
                        .ToList().ForEach(t =>
                        {
                            modelBuilder.Entity(t);
                        });
                }
                base.OnModelCreating(modelBuilder);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
