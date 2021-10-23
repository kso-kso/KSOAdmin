using System;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using KSOAdmin.Models.MapperModels.Token;
using KSOAdmin.Models.ViewModel;
using KSOAdmin.Core.AppsettingHelper;

namespace KSOAdmin.Extensions.AuthorizationUtility.CreateToken
{
    public class CreateJWTService : ICreateJWTService
    {
        #region Option注入
        private static JWTTokenOptions _JWTTokenOptions;
        public CreateJWTService( )
        {
            if (_JWTTokenOptions==null)
            {
             _JWTTokenOptions = Appsettings.GetConfigClass<JWTTokenOptions>("JWTTokenOptions");
            }
        }
        #endregion
        public TokenOption GetToken(View_Sys_User cSUser)
        {
            Claim[] claims = new[]
            {
               new Claim(ClaimTypes.Name, cSUser.UserName),
               new Claim(ClaimTypes.Sid,cSUser.User_Id.ToString()),
               new Claim(ClaimTypes.Email,cSUser.Email),
               new Claim(ClaimTypes.HomePhone,cSUser.PhoneNo),
               new Claim(ClaimTypes.Role,cSUser.Role_Id.ToString()),
            };

            string keyDir = Directory.GetCurrentDirectory();
            if (RSAHelper.TryGetKeyParameters(keyDir, true, out RSAParameters keyParams) == false)
            {
                keyParams = RSAHelper.GenerateAndSaveKey(keyDir);
            }

            SigningCredentials credentials = new SigningCredentials(new RsaSecurityKey(keyParams), SecurityAlgorithms.RsaSha256Signature);

            JwtSecurityToken AccesstokenJwt = new JwtSecurityToken(
               issuer: _JWTTokenOptions.Issuer,
               audience: _JWTTokenOptions.Audience,
               claims: claims,
               expires: DateTime.Now.AddMinutes(10),//10分钟有效期
               signingCredentials: credentials
               );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string accesstoekn = handler.WriteToken(AccesstokenJwt);

            JwtSecurityToken RefreshtokenJwt = new JwtSecurityToken(
               issuer: _JWTTokenOptions.Issuer,
               audience: _JWTTokenOptions.Audience,
               claims: claims,
               expires: DateTime.Now.AddMinutes(30),//30分钟有效期
               signingCredentials: credentials);
            string refreshtoken = handler.WriteToken(RefreshtokenJwt);
            //等待执行完成后，Token写入数据库中


            return new TokenOption()
            {
                AccessToken = accesstoekn,
                RefreshToken = refreshtoken
            };
        }
    }
}
