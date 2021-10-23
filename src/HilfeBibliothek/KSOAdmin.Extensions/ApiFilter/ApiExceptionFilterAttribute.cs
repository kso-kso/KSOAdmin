
using KSOAdmin.Models.Core;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace KSOAdmin.Extensions.ApiFilter
{
    public class ApiExceptionFilterAttribute : IExceptionFilter
    {
        public ILogger _logger { get; set; }

        public ApiExceptionFilterAttribute(ILogger logger)
        { 
            this._logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                string Msg= context.Exception.Message;
                //日志
                _logger.LogError(Msg);      

                context.Result = new JsonResult(new ResponseModel()
                {
                    Msg = Msg
                });
            }
            context.ExceptionHandled = true;
        }
    }
}
