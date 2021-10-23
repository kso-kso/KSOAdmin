using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KSOAdmin.Models.MapperModels.Token;
using KSOAdmin.Models.ViewModel;

namespace KSOAdmin.Extensions.AuthorizationUtility.CreateToken
{
    public interface ICreateJWTService
    {
        TokenOption GetToken(View_Sys_User cSUser);
    }
}
