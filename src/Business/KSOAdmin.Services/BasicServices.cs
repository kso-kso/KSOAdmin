using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using KSOAdmin.Core;
using KSOAdmin.Core.EFDbContext;
using KSOAdmin.Core.Expressions;
using KSOAdmin.IRepository;
using KSOAdmin.IServices;
using KSOAdmin.Models.Core;
using KSOAdmin.Models.MapperModels;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KSOAdmin.Services
{
    public abstract class BasicServices<T> : ServiceFunFilter<T>,IBasicServices<T>
         where T : class,new()
    {
        public BasicServices()
        {
            Response = new ResponseModel<T>();
        }
        public Microsoft.AspNetCore.Http.HttpContext Context
        {
            get
            {
                return Core.HttpContext.Current;
            }
        }
        private ResponseModel<T> Response { get; set; }

        protected IBasicRepository<T> repository { get; set; }

        public virtual async Task<List<T>> Query()
        {
            return await repository.FindListAsync(w => true);
        }

        public virtual async Task<List<T>> Query(Expression<Func<T, bool>> whereExpression, Expression<Func<T, bool>> orderby = null)
        {
            return await repository.FindAsIQueryable(whereExpression,orderby).ToListAsync();
        }

        public virtual async Task<List<TResult>> QueryOther<TResult>(Expression<Func<TResult, bool>> expression ) where TResult :class  
        {
            return await repository.FindOtherListAsync<TResult>(expression);
        }

        public async Task<PageGridData<T>> GetPageData(PageGridData<T> pageData)
        {
            if (pageData == null) throw new Exception("不允许为null");
            Expression<Func<T, bool>> expressionWhere = LambdaExtensions.CreateExpressionList<T>(pageData.Where);
            Expression<Func<T, bool>> expressionOrderby = LambdaExtensions.CreateExpressionList<T>(pageData.Orderby);
            var stu= repository.IQueryablePage<T>(out int pageCount, expressionWhere, expressionOrderby, pageData.page, pageData.PageSize, pageData.IsOrderby);
            pageData.pageCount = pageCount;
            pageData.data =await stu.ToListAsync();
            return pageData;
        }

        public async Task<ResponseModel<T>> Upload(List<T> files)
        {
            ResponseModel<T> response = new ResponseModel<T>();
           int row= await repository.UpdateRangeAsync(files);
            if (row>0)
            {
                response.Msg = "修改成功";
                response.Code = 200;
            }
            else
            {
                response.Msg = "修改失败";
                response.Code = -1;
            }
            return response;
        }

        public async Task<ResponseModel<T>> Upload(T t)
        {
            ResponseModel<T> response = new ResponseModel<T>();
            int row = await repository.UpdateAsync(t);
            if (row > 0)
            {
                response.Msg = "修改成功";
                response.Code = 200;
            }
            else
            {
                response.Msg = "修改失败";
                response.Code = -1;
            }
            return response;
        }

        public async Task<ResponseModel<T>> AddEntity(T entity, bool validationEntity = true)
        {
            ResponseModel<T> response = new ResponseModel<T>();
            if (validationEntity)
            {

            }
            await  repository.AddAsync(entity);
            response.Msg = "添加成功";
            return response;
        }

        public async Task<ResponseModel<T>> Add(List<T> list = null, bool validationEntity = true)
        {
            ResponseModel<T> response = new ResponseModel<T>();
            await repository.AddRangeAsync(list);
            
            response.Msg = "添加成功";
            return response;
        }

        public async Task<ResponseModel<T>> Del(object[] keys, bool delList = true)
        {
           int row= repository.DeleteWithKeys(keys, delList);
            return row > 0 ? ResponseModel<T>.Success() : ResponseModel<T>.Fail("执行失败");
        }

        public async Task<ResponseModel<T>> Del(T t, bool delList = true)
        {
            int row= await repository.DeleteAsync(t, delList);
            return row > 0 ? ResponseModel<T>.Success() : ResponseModel<T>.Fail("执行失败");
        }

        public async Task<ResponseModel<T>> Del(List<T> list, bool delList = true)
        {
            int row = await repository.DeleteRangeAsync(list);
            return row > 0 ? ResponseModel<T>.Success() : ResponseModel<T>.Fail("执行失败");
        }

        public Task<ResponseModel<T>> DownLoadTemplate()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<T>> Import(List<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<T>> Export(PageDataOptions pageData)
        {
            throw new NotImplementedException();
        }

        public (string, T, bool) ApiValidate(string bizContent, Expression<Func<T, object>> expression = null)
        {
            throw new NotImplementedException();
        }

        public (string, TInput, bool) ApiValidateInput<TInput>(string bizContent, Expression<Func<TInput, object>> expression)
        {
            throw new NotImplementedException();
        }

        public (string, TInput, bool) ApiValidateInput<TInput>(string bizContent, Expression<Func<TInput, object>> expression, Expression<Func<TInput, object>> validateExpression)
        {
            throw new NotImplementedException();
        }

        public void MapValueToEntity<TSource, TResult>(TSource source, TResult result, Expression<Func<TResult, object>> expression = null) where TResult : class
        {
            throw new NotImplementedException();
        }
    }
}
