using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSOAdmin.Extensions.AuthorizationUtility
{
    public interface  ICreateVerificationCode
    {
        string RandomText();
        string CreateBase64Imgage(string code);
    }
}
