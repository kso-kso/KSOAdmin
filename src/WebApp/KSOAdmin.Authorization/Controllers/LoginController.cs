using System;
using System.Threading.Tasks;

using KSOAdmin.Core;
using KSOAdmin.Core.CacheHelper.Interface;
using KSOAdmin.Extensions.AuthorizationUtility;
using KSOAdmin.Extensions.AuthorizationUtility.CreateToken;
using KSOAdmin.IServices.System;
using KSOAdmin.Models.Core;
using KSOAdmin.Models.MapperModels.Token;
using KSOAdmin.Models.ViewModel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KSOAdmin.Authorization.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [ApiController]
    [Route("api/Login")]
    [ApiExplorerSettings(GroupName = "系统")]
    public class LoginController : ControllerBase
    {
        private readonly ICreateVerificationCode _cationCode;
        private readonly ISys_UserServices _services;
        private readonly ICacheService _RedisService;
        private readonly ICreateJWTService _createJWT;

        public LoginController(ICreateVerificationCode cationCode, ISys_UserServices services, ICreateJWTService createJWT, ICacheService RedisStringService)
        {
            this._cationCode = cationCode;
            this._cationCode = cationCode;
            this._services = services;
            this._createJWT = createJWT;
            this._RedisService = RedisStringService;
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("VerificationCode")]
        public async Task<ResponseModel> VerificationCodeAsync(string type)
        {
            string code = _cationCode.RandomText();
            var data = new
            {
                img = _cationCode.CreateBase64Imgage(code),
                uuid = Guid.NewGuid()
            };
            await Task.Run(() =>
            {
                 _RedisService.Add(data.uuid.ObjToString(), code, 60 * 5);
            });
            return ResponseModel.Success("获取成功", data);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="_Sys_User"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("LoginVerification")]
        public async Task<ResponseModel> LoginVerificationAsync([FromBody] View_Sys_User _Sys_User)
        {
            if (_Sys_User is null) return ResponseModel.Fail("请输入用户名密码");
            (ResponseModel, View_Sys_User) values = await _services.LoginVerificationAsync(_Sys_User);
            if (values.Item1.Code!=200)
            {
                return values.Item1;
            }
            TokenOption tokenOption = _createJWT.GetToken(values.Item2);
            _RedisService.AddObject(tokenOption.AccessToken, values.Item2,60*30,true);

            return ResponseModel.Success("登录成功", tokenOption);
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="_Sys_User"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("RefreshToken")]
        public async Task<ResponseModel> RefreshTokenAsync([FromBody] View_Sys_User _Sys_User)
        {
            View_Sys_User vuser = _RedisService.Get<View_Sys_User>(_Sys_User.Token);

            if (vuser is null) return ResponseModel.Fail("Token刷新失败，请重新登录");

            TokenOption option = this._createJWT.GetToken(vuser);
            bool IsSucces = await Task.Run(() =>
            {
                return _RedisService.AddObject(option.RefreshToken, vuser, 60 * 10, true);
            });
            return IsSucces ? ResponseModel.Success("获取成功", new
            {
                Data = option,
                Tag = vuser
            }) : ResponseModel.Fail("验证码获取错误,请重新尝试");

           
        }

    }
}
