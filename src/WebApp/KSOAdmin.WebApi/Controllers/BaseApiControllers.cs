using KSOAdmin.Models.Core;
using KSOAdmin.Models.MapperModels;

using Microsoft.AspNetCore.Mvc;

namespace KSOAdmin.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiControllers: ControllerBase
    {
        [NonAction]
        public ResponseModel<TBase> Success<TBase>(TBase data,string Msg ="成功") 
        { 
            return new ResponseModel<TBase>() { 
            Code = 200,
            Data = data,
            IsSuccess = true,
            Msg = Msg
            };
        }
        [NonAction]
        public ResponseModel Success (string Msg = "成功")
        {
            return new ResponseModel ()
            {
                Code = 200,
                IsSuccess = true,
                Msg = Msg
            };
        }

        [NonAction]
        public ResponseModel<string> Fail(string Msg = "失败", int status = 500)
        {
            return new ResponseModel<string>()
            {
                Code = status,
                IsSuccess = true,
                Msg = Msg
            };
        }
        [NonAction]
        public ResponseModel<TBase> Fail<TBase>(string Msg = "失败", int status = 500)
        {
            return new ResponseModel<TBase>()
            {
                Code = status,
                IsSuccess = true,
                Msg = Msg
            };
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="pageModel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [NonAction]
        public ResponseModel<PageGridData<TBase>> SuccessPage<TBase>(PageGridData<TBase> pageModel, string msg = "获取成功")
        {

            return new ResponseModel<PageGridData<TBase>>()
            {
                IsSuccess = true,
                Msg = msg,
                Data = new PageGridData<TBase>()
                {
                    page = pageModel.page,
                    dataCount = pageModel.dataCount,
                    data = pageModel.data,
                    pageCount = pageModel.pageCount,
                }
            };
        }

    }
}
