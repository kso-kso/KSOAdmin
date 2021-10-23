using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSOAdmin.Models.MapperModels.Appsetting
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public class DBString
    {
        public string ConnId { get; set; }
        public int? DBType { get; set; }
        public bool IsEnable { get; set; }
        public string Connection { get; set; }
    }
}
