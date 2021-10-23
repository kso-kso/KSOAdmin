
using System.Collections.Generic;
using System.Linq;

using KSOAdmin.Core.AppsettingHelper;
using KSOAdmin.Models.MapperModels.Appsetting;

namespace KSOAdmin.Core.EFDbContext
{
    /// <summary>
    /// 获取连接字符串
    /// </summary>
    public class DBConntionString
    {
        /// <summary>
        /// 默认库 为true
        /// </summary>
        /// <returns></returns>
        public static DBString GetDbString() 
        {
            var dBString = Appsettings.app<DBString>("DBString");
            var dBString1 = dBString?.Where(w => w.IsEnable == true).FirstOrDefault();
            return dBString1??null;
        }

        /// <summary>
        /// 指定库别
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static DBString GetDbString(DBTypes types)
        {
            if (types == DBTypes.SqlServer)
            {
                var dBString = Appsettings.app<DBString>("DBString");
                var dBString1 = dBString?.Where(w => w.ConnId == "SqlServer").FirstOrDefault();
                return dBString1 ?? null;
            }
            else if (types == DBTypes.MySql)
            {
                var dBString = Appsettings.app<DBString>("DBString");
                var dBString1 = dBString?.Where(w => w.ConnId == "MySql").FirstOrDefault();
                return dBString1 ?? null;
            }
            return default;
        }
        /// <summary>
        /// 主从库
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static List<DBString> GetDbList(DBTypes types)
        {
            if (types == DBTypes.SqlServer)
            {
                var dBString = Appsettings.app<DBString>("DBString");
                var dBString1 = dBString?.Where(w => w.ConnId == "SqlServer").ToList();
                return dBString1 ?? null;
            }
            else if (types == DBTypes.MySql)
            {
                var dBString = Appsettings.app<DBString>("DBString");
                var dBString1 = dBString?.Where(w => w.ConnId == "MySql").ToList();
                return dBString1 ?? null;
            }
            return default;
        }

       


    }
   
}
