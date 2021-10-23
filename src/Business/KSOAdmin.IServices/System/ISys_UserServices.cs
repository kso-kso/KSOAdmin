
using System.Threading.Tasks;

using KSOAdmin.Models.Core;
using KSOAdmin.Models.DomainModels.System;
using KSOAdmin.Models.ViewModel;

namespace KSOAdmin.IServices.System
{
    public interface ISys_UserServices:IBasicServices<Sys_User>
    {

        Task<(ResponseModel, View_Sys_User)> LoginVerificationAsync(View_Sys_User user);
    }
}
