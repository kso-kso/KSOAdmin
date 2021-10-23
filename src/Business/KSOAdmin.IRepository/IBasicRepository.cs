using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using KSOAdmin.Core.EFDbContext;
using KSOAdmin.Models.Core;

using Microsoft.Data.SqlClient;

namespace KSOAdmin.IRepository
{
    /// <summary>
    /// 底层父类
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IBasicRepository<TModel> where TModel : class, new()
    {
        /// <summary>
        /// 获取操作的DB实体
        /// </summary>
        KSOContext DbContext { get; }

        /// <summary>
        /// 事务的支持
        /// </summary>
        /// <param name="responses"></param>
        /// <returns></returns>
        Task<ResponseModel> DbContextTransaction(Func<ResponseModel> responses);

        #region 查询
        /// <summary>
        /// 异步单个查询
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<TModel> FindFirstAsync(Expression<Func<TModel, bool>> expression);

        /// <summary>
        /// 同步单个查询
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        TModel FindFirst(Expression<Func<TModel, bool>> expression);

        /// <summary>
        /// 同步查询
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        List<TModel> FindList(Expression<Func<TModel, bool>> expression);

        /// <summary>
        /// 异步查询
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<List<TModel>> FindListAsync(Expression<Func<TModel, bool>> expression);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <param name="orderBy">排序字段 </param>
        /// <returns></returns>
        IQueryable<TModel> FindAsIQueryable(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel,bool>> orderBy = null,bool IsDesc=true);

        /// <summary>
        /// 通过条件查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">返回类型如:Find(x => x.UserName == loginInfo.userName, p => new { uname = p.UserName });</param>
        /// <returns></returns>
        List<T> FindOtherList<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// 通过条件查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Task<List<T>> FindOtherListAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// 通过条件查询数据其他的表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        List<T> FindOtherFirst<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// 异步条件查询数据其他的表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Task<List<T>> FindOtherFirstAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// 通过条件查询数据其他的表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns>返回IQueryable</returns>
        IQueryable<T> FindOtherIQueryable<T>(Expression<Func<T, bool>> predicate) where T : class;
        #endregion

        #region 分页
        
        IQueryable<TFind> IQueryablePage<TFind>(out int rowcount, Expression<Func<TFind, bool>> predicate, Expression<Func<TFind, bool>> orderBy, int pageIndex = 1, int pagesize = 20, bool IsOrderby = true) where TFind : class;

        IQueryable<TModel> IQueryablePage( out int rowcount, Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, bool>> orderBy, int pageIndex = 1, int pagesize = 20, bool IsOrderby = true);

        List<TModel> IQueryablePage(IQueryable<TModel> queryable,  out int rowcount, Expression<Func<TModel, bool>> orderBy, int pageIndex = 1, int pagesize = 20, bool IsOrderby = true);

        #endregion

        #region 删除
        int Delete(TModel model, bool saveChanges = true);

        Task<int> DeleteAsync(TModel model, bool saveChanges = true);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="delList">是否将子表的数据也删除</param>
        /// <returns></returns>
        int DeleteWithKeys(object[] keys, bool delList = true);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        int DeleteRange(Expression<Func<TModel,bool>> expression );
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(Expression<Func<TModel, bool>> expression);
        Task<int> DeleteRangeAsync(List<TModel> list);
        #endregion

        #region 修改

        int Update(TModel entity, bool saveChanges = true);

        Task<int> UpdateAsync(TModel entity , bool saveChanges = true);
        /// <summary>
        /// 修改异步
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties">指定更新字段:x=>new {x.Name,x.Enable}</param>
        /// <param name="saveChanges">是否保存</param>
        /// <returns></returns>
        int Update<T>(T entity , bool saveChanges = true) where T : class;

        Task<int> UpdateAsync<T>(T entity , bool saveChanges = true) where T : class;
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="entities"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        int UpdateRange<TSource>(IEnumerable<TSource> entities, bool saveChanges = true) where TSource : class;
        Task<int> UpdateRangeAsync(IEnumerable<TModel> entities, bool saveChanges = true);
        

        #endregion

        #region 添加

        void Add(TModel entities, bool SaveChanges = true);
        void AddRange(IEnumerable<TModel> entities, bool SaveChanges = true);

        Task AddAsync(TModel entities);
        Task AddRangeAsync(IEnumerable<TModel> entities);

        #endregion

        int SaveChanges();

        Task<int> SaveChangesAsync();

        #region 执行原生sql
         /// <summary>
         /// 修改删除 插入
         /// </summary>
         /// <param name="sql"></param>
         /// <param name="sqlParameters"></param>
         /// <returns></returns>
        int ExecuteSqlCommand(string sql, params SqlParameter[] sqlParameters);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        List<TModel> FromSql(string sql, params SqlParameter[] sqlParameters);
 
        #endregion


    }
}

