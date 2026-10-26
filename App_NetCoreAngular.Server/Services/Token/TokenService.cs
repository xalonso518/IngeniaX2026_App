using App_NetCoreAngular.Server.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App_NetCoreAngular.Server.Services.Token
{
    // Servicio de token JWT
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly string _secretKey;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _secretKey = _config["TokenSettings:Secret"]??"";
        }

        // Crea token con datos del usuario
        public string GenerateToken(AuthUsuarioView usuario)
        {
            var claims = new[]
            {
            new Claim("id", usuario.UsuarioId.ToString()),
            new Claim("user", usuario.Username),
            new Claim("guid", usuario.Guid),
            new Claim("rol", usuario.Rol.ToString("D")),
            new Claim("stat",  usuario.Estado.ToString("D")),
            new Claim("nom", usuario.Nombre),
            new Claim("img", usuario.ImagenPerfil != null ? usuario.ImagenPerfil : ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["TokenSettings:Issuer"],
                audience: _config["TokenSettings:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddHours(24 * 5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
