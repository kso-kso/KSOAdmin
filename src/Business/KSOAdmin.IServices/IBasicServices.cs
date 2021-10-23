using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

using KSOAdmin.Models.Core;
using KSOAdmin.Models.MapperModels;

using Microsoft.AspNetCore.Http;

namespace KSOAdmin.IServices
{
    public interface IBasicServices<T> where T  : class ,new()
    {
        Task<List<T>> Query();
        Task<List<T>> Query(Expression<Func<T, bool>> whereExpression, Expression<Func<T, bool>> orderby=null);
        Task<List<TResult>> QueryOther<TResult>(Expression<Func<TResult, bool>> expression ) where TResult : class;

        Task<PageGridData<T>> GetPageData(PageGridData<T> pageData);

        Task<ResponseModel<T>> Upload(List<T> files);

        Task<ResponseModel<T>> Upload( T t);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity">保存的实体</param>
        /// <param name="validationEntity">是否对实体进行校验</param>
        /// <returns></returns>
        Task<ResponseModel<T>> AddEntity(T entity, bool validationEntity = true);

        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="list">保存的明细</param>
        /// <param name="validationEntity">是否对实体进行校验</param>
        /// <returns></returns>
        Task<ResponseModel<T>> Add(List<T> list = null, bool validationEntity = true);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keys">删除的主键</param>
        /// <param name="delList">是否删除对应明细(默认会删除明细)</param>
        /// <returns></returns>
        Task<ResponseModel<T>> Del(object[] keys, bool delList = true);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keys">删除的主键</param>
        /// <param name="delList">是否删除对应明细(默认会删除明细)</param>
        /// <returns></returns>
        Task<ResponseModel<T>> Del(T t, bool delList = true);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keys">删除的主键</param>
        /// <param name="delList">是否删除对应明细(默认会删除明细)</param>
        /// <returns></returns>
        Task<ResponseModel<T>> Del(List<T> list, bool delList = true);
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<T>> DownLoadTemplate();
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        Task<ResponseModel<T>> Import(List<IFormFile> files);
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        Task<ResponseModel<T>> Export(PageDataOptions pageData);

        (string, T, bool) ApiValidate(string bizContent, Expression<Func<T, object>> expression = null);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="bizContent"></param>
        /// <param name="expression">对指属性验证格式如：x=>new { x.UserName,x.Value }</param>
        /// <returns>(string,TInput, bool) string:返回验证消息,TInput：bizContent序列化后的对象,bool:验证是否通过</returns>
        (string, TInput, bool) ApiValidateInput<TInput>(string bizContent, Expression<Func<TInput, object>> expression);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="bizContent"></param>
        /// <param name="expression">对指属性验证格式如：x=>new { x.UserName,x.Value }</param>
        /// <param name="validateExpression">对指定的字段只做合法性判断比如长度是是否超长</param>
        /// <returns>(string,TInput, bool) string:返回验证消息,TInput：bizContent序列化后的对象,bool:验证是否通过</returns>
        (string, TInput, bool) ApiValidateInput<TInput>(string bizContent, Expression<Func<TInput, object>> expression, Expression<Func<TInput, object>> validateExpression);

        /// <summary>
        /// 将一个实体的赋到另一个实体上,应用场景：
        /// 两个实体，a a1= new a();b b1= new b();  a1.P=b1.P; a1.Name=b1.Name;
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="result"></param>
        /// <param name="expression">指定对需要的字段赋值,格式x=>new {x.Name,x.P},返回的结果只会对Name与P赋值</param>
        void MapValueToEntity<TSource, TResult>(TSource source, TResult result, Expression<Func<TResult, object>> expression = null) where TResult : class;

    }
}
