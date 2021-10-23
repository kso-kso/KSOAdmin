using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSOAdmin.Models.Base
{
    /// <summary>
    /// 注释表头以及数据操作权限
    /// </summary>
    public class EntityAttribute : Attribute
    {
        /// <summary>
        /// 真实表名(数据库表名，若没有填写默认实体为表名)
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表中文名
        /// </summary>
        public string TableCnName { get; set; }
        /// <summary>
        /// 子表
        /// </summary>
        public Type[] DetailTable { get; set; }
        /// <summary>
        /// 子表中文名
        /// </summary>
        public string DetailTableCnName { get; set; }

        //是否开启数据操作,false=用户只能操作自己(及下级)的数据,true=为全部的数据随意操作
        public bool IsEnity { get; set; } = true;

    }
}
