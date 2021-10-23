using System;

using KSOAdmin.Models.Core;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KSOAdmin.WebApi.Filter
{
    /// <summary>
    /// 结果处理
    /// </summary>
    public class ApiResultFilterAttribute : ResultFilterAttribute
    {
        /// <summary>
        /// 是否需要重写返回处理  true 需要  false 不需要
        /// </summary> 
        public bool IsCustomRespone { get; set; }

        public ApiResultFilterAttribute(bool CustomRespone= true) 
        { 
            IsCustomRespone = CustomRespone;
        }
        /// <summary>
        /// 返回后
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        /// <summary>
        /// 返回前
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //不执行
            if (!IsCustomRespone)
            {
                base.OnResultExecuting(context);
            }
            //自定义
            var result = context.Result as ObjectResult;
            context.Result = new JsonResult(ResponseModel.Success("成功",result.Value));
            return;

        }
    }
}
