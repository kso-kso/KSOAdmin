using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSOAdmin.Models.ViewModel
{
    public class View_Sys_User
    {
        /// <summary>
        /// 支持多种方式登录，用户名，邮箱 ，手机号
        /// 传入的时候UserName 并不单单的是name 也有可能 用户名，邮箱 ，手机号
        /// </summary>
        public string UserName { get; set; }

        public string UserPwd { get; set; }
        public string UserVerification { get; set; }
        //唯一ID
        public int User_Id { get; set; }
        //手机号
        public string PhoneNo { get; set; }
        //邮箱
        public string Email { get; set; }
        //Token
        public string Token { get; set; }
        
        public int Role_Id { get; set; }

        public string UUID { get; set; }
    }
}
