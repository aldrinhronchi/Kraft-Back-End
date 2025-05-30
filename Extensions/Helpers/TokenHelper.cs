﻿using Kraft_Back_CS.Extensions.Middlewares;
using Kraft_Back_CS.Models.Usuario;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kraft_Back_CS.Extensions.Helpers
{
    /// <summary>
    /// Helper para efetuar as ações relacionadas ao Token JWT
    /// </summary>
    public class TokenHelper
    {
        /// <summary>
        /// Gera o Token JWT daquele usuario
        /// </summary>
        /// <param name="usuario">O usuario a ser efetuado a autenticação</param>
        /// <returns>O Token JWT</returns>
        public static String GerarToken(Usuario usuario)
        {
            IConfiguration appsettings = ServiceLocator.Current.BuscarServico<IConfiguration>();
            JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(appsettings["Jwt:Key"]);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.Name, usuario.Nome),
                        new Claim(ClaimTypes.Email, usuario.Email),
                        new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString())
                    }),
                Issuer = appsettings["Jwt:Issuer"],
                Audience = appsettings["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = Handler.CreateToken(tokenDescriptor);
            return Handler.WriteToken(token);
        }

        /// <summary>
        /// Busca o ID do Usuario de dentro do Token
        /// </summary>
        /// <param name="token">Token a ter o seu ID extraido</param>
        /// <returns>o ID de Usuario</returns>
        public static Int32 ObterIDUsuarioDeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var ClaimID = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            Int32 IDUsuario = 0;
            if (!Int32.TryParse(ClaimID, out IDUsuario))
            {
                throw new UnauthorizedAccessException("Usuário não encontrado no token!");
            }
            return IDUsuario;
        }
    }
}