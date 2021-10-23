using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using KSOAdmin.Core.EFDbContext;
using KSOAdmin.IRepository;
using KSOAdmin.Models.Base;
using KSOAdmin.Models.Core;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace KSOAdmin.Repository
{
    /// <summary>
    /// 父类的实现
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class BasicRepository<TModel> :IDependency, IBasicRepository<TModel> where TModel : class, new() 
    {
        private KSOContext DefaultDbContext { get; set; }
       

        public BasicRepository() { }

        public BasicRepository(KSOContext _DbContext) 
        {
            DefaultDbContext = _DbContext??throw new Exception("请实例化DbContext!");
        }

        public virtual KSOContext DbContext
        {
            get { return DefaultDbContext; }
        }

        private DbSet<TModel> DBSet
        {
            get { return DefaultDbContext.Set<TModel>(); }
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="responses"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseModel> DbContextTransaction(Func<ResponseModel> responses)
        {
            ResponseModel respon = new ResponseModel();
            using (var tran=await DefaultDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    respon = responses();
                    if (respon.IsSuccess)
                    {
                        await tran.CommitAsync();
                        respon.Code = 200;
                    }
                    else
                    {
                        await tran.RollbackAsync();
                        respon.Code = -1;
                    }
                    return respon;
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        #region 添加
        public void Add(TModel entities, bool SaveChanges = true)
        {
            AddRange(new List<TModel>() { entities },SaveChanges);
        }
        public void AddRange(IEnumerable<TModel> entities, bool SaveChanges = true)
        {
            DBSet.AddRange(entities);
            if (SaveChanges) DbContext.SaveChanges();
        }
        public async Task AddAsync(TModel entities)
        {
            await DBSet.AddRangeAsync(entities);
        }

        public async Task AddRangeAsync(IEnumerable<TModel> entities)
        {
           await DBSet.AddRangeAsync(entities);
        }
        #endregion


        #region 删除

        public int Delete(TModel model, bool saveChanges = true)
        {
            DBSet.Remove(model);
            if (saveChanges) return  DefaultDbContext.SaveChanges();
            return 0;
        }
        public async Task<int> DeleteAsync(TModel model, bool saveChanges = true)
        {
            DefaultDbContext.Entry<TModel>(model).State = EntityState.Deleted;
            if (saveChanges) return await DefaultDbContext.SaveChangesAsync();
            return 0;
        }
        /// <summary>
        /// 批量删除自己
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int DeleteRange(Expression<Func<TModel, bool>> expression)
        {
            Type entityType = typeof(TModel);
            List<TModel> models = DBSet.Where(expression).ToList();
            DefaultDbContext.RemoveRange(models);
            return models.Count();
        }
        /// <summary>
        /// 批量删除自己
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(Expression<Func<TModel, bool>> expression)
        {
            Type entityType = typeof(TModel);
            List<TModel> models =await DBSet.Where(expression).ToListAsync();
            foreach (var item in models)
            {
                DefaultDbContext.Entry<TModel>(item).State = EntityState.Deleted;
            }
            return await DefaultDbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 批量删除自己
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(List<TModel> list)
        {
            try
            {
                DefaultDbContext.RemoveRange(list);
                return list.Count();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 主从删除 借鉴 VOL
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="delList"></param>
        /// <returns></returns>
        public int DeleteWithKeys(object[] keys, bool delList = true)
        {
            Type entityType = typeof(TModel);
            string tKey = entityType.GetEntityTableKey();
            string joinKeys =  $"'{string.Join("','", keys)}'";
            string sql = $"DELETE FROM {entityType.GetEntityTableName() } where {tKey} in ({joinKeys});";
            if (delList)
            {
                Type detailType = entityType.GetCustomAttribute<EntityAttribute>().DetailTable?[0];
                if (detailType != null)
                    sql = sql + $"DELETE FROM {detailType.GetEntityTableName()} where {tKey} in ({joinKeys});";
            }
            return ExecuteSqlCommand(sql);
        }

        #endregion

        #region 查询
        /// <summary>
        /// 查询排序
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="IsDesc"></param>
        /// <returns></returns>
        public IQueryable<TModel> FindAsIQueryable(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, bool>> orderBy = null, bool IsDesc = true)
        {
            IQueryable<TModel> models = DBSet.AsNoTracking().Where(predicate);
            if (orderBy != null)
            {
                if (IsDesc)
                {
                    models.OrderBy(orderBy);
                }
                else
                {
                    models.OrderByDescending(orderBy);
                }
            }
            return models;
        }

        /// <summary>
        /// 获取第一个
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TModel FindFirst(Expression<Func<TModel, bool>> expression)
        {
            return DBSet.AsNoTracking().Where(expression).FirstOrDefault();
        }
       /// <summary>
       /// 异步获取第一个
       /// </summary>
       /// <param name="expression"></param>
       /// <returns></returns>
       /// <exception cref="NotImplementedException"></exception>
        public async Task<TModel> FindFirstAsync(Expression<Func<TModel, bool>> expression)=>await DBSet.Where(expression).FirstOrDefaultAsync();

        public List<TModel> FindList(Expression<Func<TModel, bool>> expression) => DBSet.Where(expression).ToList();

        public async Task<List<TModel>> FindListAsync(Expression<Func<TModel, bool>> expression) 
        {
          return  await DBSet.Where(expression).ToListAsync(); 
        }

        public List<T> FindOtherFirst<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DefaultDbContext.Set<T>().AsNoTracking().Where(predicate).ToList();
        }

        public async Task<List<T>> FindOtherFirstAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await DefaultDbContext.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public IQueryable<T> FindOtherIQueryable<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DefaultDbContext.Set<T>().AsNoTracking().Where(predicate);
        }

        public List<T> FindOtherList<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DefaultDbContext.Set<T>().AsNoTracking().Where(predicate).ToList();
        }

        public Task<List<T>> FindOtherListAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DefaultDbContext.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }
        #endregion

        #region sql
       
        public virtual int ExecuteSqlCommand(string sql, params SqlParameter[] sqlParameters)
        {
            return DbContext.Database.ExecuteSqlRaw(sql, sqlParameters);
        }
        
        public virtual List<TModel> FromSql(string sql, params SqlParameter[] sqlParameters)
        {
            return DBSet.FromSqlRaw(sql, sqlParameters).ToList();
        }
        /// <summary>
        /// 执行sql
        /// 使用方式 FormattableString sql=$"select * from xx where name ={xx} and pwd={xx1} "，
        /// FromSqlInterpolated内部处理sql注入的问题，直接在{xx}写对应的值即可
        /// 注意：sql必须 select * 返回所有TEntity字段，
        /// </summary>
        /// <param name="formattableString"></param>
        /// <returns></returns>
        public virtual IQueryable<TModel> FromSqlInterpolated([NotNull] FormattableString sql)
        {
            return DBSet.FromSqlInterpolated(sql);
        }
        
        #endregion

        #region 分页
        public IQueryable<TFind> IQueryablePage<TFind>(out int rowcount, Expression<Func<TFind, bool>> predicate, Expression<Func<TFind, bool>> orderBy, int pageIndex = 1, int pagesize = 20, bool IsOrderby = true) where TFind : class
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pagesize = pagesize <= 0 ? 10 : pagesize;
            if (predicate == null)
            {
                predicate = x => true;
            }
            var _db = DbContext.Set<TFind>();
            rowcount = _db.Count(predicate);
            return DbContext.Set<TFind>().Where(predicate)
                .OrderIsDesc(orderBy,IsOrderby)
                .Skip((pageIndex - 1) * pagesize)
                .Take(pagesize);
        }

        public IQueryable<TModel> IQueryablePage(out int rowcount, Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, bool>> orderBy, int pageIndex = 1, int pagesize = 20, bool IsOrderby = true)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pagesize = pagesize <= 0 ? 10 : pagesize;
            if (predicate == null)
            {
                predicate = x => true;
            }
            rowcount = DBSet.Count(predicate);
            return DBSet.Where(predicate)
                .OrderIsDesc(orderBy, IsOrderby)
                .Skip((pageIndex - 1) * pagesize)
                .Take(pagesize);
        }

        public List<TModel>  IQueryablePage(IQueryable<TModel> queryable, out int rowcount, Expression<Func<TModel, bool>> orderBy, int pageIndex = 1, int pagesize = 20, bool IsOrderby = true)
        {
            if (queryable is null) throw new Exception("Queryable 不能为空");
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pagesize = pagesize <= 0 ? 10 : pagesize;
            rowcount = queryable.ToList().Count();
            return  queryable
                .OrderIsDesc(orderBy, IsOrderby)
                .Skip((pageIndex - 1) * pagesize)
                .Take(pagesize).ToList();
        }
        #endregion

        #region 修改
        public int Update(TModel entity,  bool saveChanges = true)
        {
            DBSet.Update(entity);
            return 1;
        }

        public async Task<int> UpdateAsync(TModel entity, bool saveChanges = true)
        {
            DefaultDbContext.Entry<TModel>(entity).State = EntityState.Modified;
            if (saveChanges) return await DefaultDbContext.SaveChangesAsync();
            return 0;
        }

        public int Update<T>(T entity,  bool saveChanges = true) where T : class
        {
            DefaultDbContext.Set<T>().Update(entity);
            return 1;
        }

        public async Task<int> UpdateAsync<T>(T entity,  bool saveChanges = true) where T : class
        {
            DefaultDbContext.Entry<T>(entity).State = EntityState.Modified;
            if (saveChanges) return await DefaultDbContext.SaveChangesAsync();
            return 0;
        }
        public int UpdateRange<T>(IEnumerable<T> entities, bool saveChanges = true) where T : class
        {
            foreach (var item in entities)
            {
            DefaultDbContext.Entry<T>(item).State = EntityState.Modified;
            }
            if (saveChanges) return  DefaultDbContext.SaveChanges();
            return 0;
        }
        public async Task<int> UpdateRangeAsync(IEnumerable<TModel> entities, bool saveChanges = true)
        {
            foreach (var item in entities)
            {
                DefaultDbContext.Entry<TModel>(item).State = EntityState.Modified;
            }
            if (saveChanges) return await DefaultDbContext.SaveChangesAsync();
            return 0;
        }

        #endregion

        /// <summary>
        /// 自定义的保存
        /// </summary>
        /// <returns></returns>
        public virtual int SaveChanges()
        {
            return  DbContext.SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return  DbContext.SaveChangesAsync();
        }

     
    }
}
