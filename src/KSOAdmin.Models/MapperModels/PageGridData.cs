using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSOAdmin.Models.MapperModels
{
    /// <summary>
    /// 正常的分页返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageGridData<T>
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int page { get; set; } = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount { get; set; } = 6;
        /// <summary>
        /// 数据总数
        /// </summary>
        public int dataCount { get; set; } = 0;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> data { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<WhereModel> Orderby { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public bool IsOrderby { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<WhereModel> Where { get; set; }
    }
}
