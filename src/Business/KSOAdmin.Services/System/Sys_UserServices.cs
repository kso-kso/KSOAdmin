using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using KSOAdmin.Core.CacheHelper.Interface;
using KSOAdmin.Core.EFDbContext;
using KSOAdmin.Core.MD5Helper;
using KSOAdmin.IRepository;
using KSOAdmin.IRepository.System;
using KSOAdmin.IServices.System;
using KSOAdmin.Models.Core;
using KSOAdmin.Models.DomainModels.System;
using KSOAdmin.Models.ViewModel;

namespace KSOAdmin.Services.System
{
    public class SysUserServices : BasicServices<Sys_User>, ISys_UserServices, IDependency
    {
        private readonly ICacheService _RedisService;

        private readonly IMapper _iMapper;
        private readonly ISys_UserRepository _pository;
        private readonly IBasicRepository<Sys_User> _BaseRepository;
        public SysUserServices(ISys_UserRepository pository, IBasicRepository<Sys_User> BaseRepository, IMapper iMapper, ICacheService RedisService)
        {
            this._iMapper = iMapper;
            this._pository = pository;
            this.repository = BaseRepository;//父类
            this._BaseRepository = BaseRepository;
            this._RedisService = RedisService;
        }
        /// <summary>
        /// 登录的查询
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<(ResponseModel, View_Sys_User)> LoginVerificationAsync(View_Sys_User user)
        {
            ResponseModel response = new() { Code = 500 };
            if (string.IsNullOrWhiteSpace( user.UserPwd) )
            {
                response.Msg = "密码输入格式不正确";
                return (response, user);
            }
            if (user.UserName?.Trim().Length < 4)
            {
                response.Msg = "用户名输入格式不正确";
                return (response, user);
            }
            if (user.UserVerification?.Trim().Length != 4)
            {
                response.Msg = "验证码输入格式不正确";
                return (response, user);
            }
            else
            {
                string code = _RedisService.Get(user.UUID);
                if (code is null || code.ToLower() != user.UserVerification.ToLower())
                {
                    response.Msg = "验证码输入错误";
                    return (response, user);
                }
            }
            user.UserPwd = MD5.GetMD5Hash(user.UserPwd);
            Sys_User sys_User = await _pository.FindFirstAsync(w => (w.UserName == user.UserName || w.Email == user.UserName || w.PhoneNo == user.UserName) && w.UserPwd == user.UserPwd && w.Enable == 1);
            if (sys_User is null)
            {
                response.Code = 401;
                response.Msg = "找不到此用户，请先注册";
                return (response, user);
            }
            else
            {
                response.Code = 200;
                response.Msg = "执行成功";
            }
            return (response, _iMapper.Map<Sys_User, View_Sys_User>(sys_User));
        }

        protected override object GetDetailSummary<Detail>(IQueryable<Detail> queryeable)
        {
            throw new NotImplementedException();
        }

    }
}
