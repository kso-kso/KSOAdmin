using System.Collections.Generic;
using System.Threading.Tasks;

using KSOAdmin.IServices.System;
using KSOAdmin.Models.DomainModels.System;
using KSOAdmin.Models.ViewModel;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KSOAdmin.WebApi.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [ApiExplorerSettings(GroupName = "系统")]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISys_UserServices _UserServices;
        public HomeController(ILogger<HomeController> logger, ISys_UserServices UserServices)
        {
            _UserServices=UserServices;
            _logger = logger;
        }
        /// <summary>
        /// cs测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Index()
        {
            try
            {
                List<Sys_User> sys_Users = await _UserServices.Query();
                View_Sys_User view  =new View_Sys_User();
                var ss = await _UserServices.LoginVerificationAsync(view);
                return Newtonsoft.Json.JsonConvert.SerializeObject(sys_Users);
            }
            catch (System.Exception ex)
            {

                throw;
            }
           
        }
    }
}
