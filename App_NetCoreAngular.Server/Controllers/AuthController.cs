using App_NetCoreAngular.Server.Models.Auth;
using App_NetCoreAngular.Server.Models.Shared;
using App_NetCoreAngular.Server.Repository.AuthRep;
using App_NetCoreAngular.Server.Services.Token;
using App_NetCoreAngular.Server.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace App_IngeniaX_2026.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        // ID        
        private readonly IAuthRepository _authRepository;
        private readonly TokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(
            IAuthRepository authRepository,
            TokenService tokenService,
            ILogger<AuthController> logger
            )
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _logger = logger;

        }

        ///<summary>
        ///Login de usuario
        ///</summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginForm form)
        {
            // Response
            var res = new ResponseApi<TokenUsuario> { Success = false };

            // Hash password
            form.Password = HashUtilities.HashPassword(form.Password);

            // Validar password y usuario
            var rUsuario = await _authRepository.GetUsuarioPassword(form);
            if (!rUsuario.Success) return BadRequest(rUsuario);
            if (rUsuario.Payload != null)
            {
                // Generar token
                var token = _tokenService.GenerateToken(rUsuario.Payload);
                res.Success = true;
                res.Payload = new TokenUsuario()
                {
                    Id = rUsuario.Payload.UsuarioId.ToString(),
                    Usuario = rUsuario.Payload.Username,
                    Token = token
                };
                return Ok(res);
            }
            else
            {
                _logger.LogError("Intento de login para el usuario: {Email}", form.Email);
                res.Error = "Datos del usuario incorrectos.";
                return BadRequest(res);
            }
        }
    }
}
