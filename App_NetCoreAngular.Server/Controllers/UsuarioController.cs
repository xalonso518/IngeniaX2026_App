using App_NetCoreAngular.Server.Models.Shared;
using App_NetCoreAngular.Server.Models.Usuarios;
using App_NetCoreAngular.Server.Repository.UsuariorRep;
using App_NetCoreAngular.Server.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_IngeniaX_2026.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsuarioController : ControllerBase
    {
        // ID
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<AuthController> _logger;

        public UsuarioController(
            IUsuarioRepository usuarioRepository,
            ILogger<AuthController> logger
            )
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        // Obtenemos valores del claims del usuario autenticado
        private string UsuarioId => User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0";
        private string UserName => User.Claims.FirstOrDefault(c => c.Type == "user")?.Value ?? "0";

        ///<summary>
        ///Agrega usuario
        ///</summary>/usuario/agregar - POST
        [HttpPost("agregar")]
        public async Task<IActionResult> Agregar([FromBody] UsuarioForm form)
        {
            ResponseApi<long> res = new ResponseApi<long>() { Success = false };

            //Validar existencia usuario
            var rExiste = await _usuarioRepository.ValidarExistencia(form.Username);
            if (!rExiste.Success) return BadRequest(rExiste);
            if (rExiste.Success && rExiste.Payload == false)
            {
                // Hash password
                form.Password = HashUtilities.HashPassword(form.Password);

                // Crea objeto usuario
                var nuevoUsuario = _usuarioRepository.Crear(form);

                // Agrega usuario
                res = await _usuarioRepository.Agregar(nuevoUsuario);
                if (res.Success)
                {
                    //Crear correo bienvenida
                    //var rCorreo = await _usuarioRepository.EnviarCorreoBienvenida(form.Username);

                    // Regresar valor
                    return Ok(res);
                }
            }
            else
            {
                res.Error = "Error desconocido.";
                return BadRequest(res);
            }
            return BadRequest(res);
        }

        ///<summary>
        ///Obtienen lista de los usuario
        ///</summary>
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string page = "1", [FromQuery] string sortBy = "user_desc", [FromQuery] string rol = "0", [FromQuery] string filtro = "")
        {
            //Buscar usuarios
            var rQuery = await _usuarioRepository.Buscar(
                page,
                sortBy,
                rol,
                filtro??""
                );
            if (rQuery.Success)
            {
                return Ok(rQuery);
            }
            else
            {
                return BadRequest(rQuery);
            }
        }

        ///<summary>
        ///Obtienen usuario individual
        ///</summary>
        [HttpGet("obtener/{guid}")]
        public async Task<IActionResult> Obtener(string guid)
        {
            var rQuery = await _usuarioRepository.BuscarPorGuid(guid);
            if (rQuery.Success)
            {
                return Ok(rQuery);
            }
            else
            {
                return BadRequest(rQuery);
            }
        }

        ///<summary>
        ///Editar usuario
        ///</summary>
        [HttpPut("editar/{guid}")]
        public async Task<IActionResult> Editar(string guid, [FromBody] UsuarioFormEditar form)
        {
            var res = new ResponseApi<string>() { Success = false };

            //Editar
            res = await _usuarioRepository.Editar(form);
            if (res.Success)
            {
                return Ok(res);
            }
            else return BadRequest(res);
        }

        ///<summary>
        ///Cambiar estado activo/inactivo usuario
        ///</summary>
        [HttpPut("estado")]
        public async Task<IActionResult> CambiarEstado([FromBody] UsuarioFormSimple form)
        {
            //Validar status
            form.Estado = form.Estado == Estado.Activo ? Estado.Activo : Estado.Inactivo; 

            //Modificar status
            var rQuery = await _usuarioRepository.EditarEstado(form.UsuarioId, form.Estado);
            if (rQuery.Success)
            {
                return Ok(rQuery);
            }
            else
            {
                return BadRequest(rQuery);
            }
        }

        ///<summary>
        ///Editar password
        ///</summary>
        [HttpPut("password/{guid}")]
        public async Task<IActionResult> Password(string guid, [FromBody] UsuarioFormEditarPassword form)
        {
            var res = new ResponseApi<string>() { Success = false };

            // Hash password
            form.Password = HashUtilities.HashPassword(form.Password);

            //Editar
            res = await _usuarioRepository.EditarPassword(form.UsuarioId, form.Password);
            if (res.Success)
            {
                return Ok(res);
            }
            else return BadRequest(res);
        }

        ///<summary>
        ///Eliminar usaurio
        ///</summary>
        [HttpDelete("eliminar/{guid}")]
        public async Task<IActionResult> Eliminar(string guid)
        {
            var res = new ResponseApi<string>() { Success = false };
            //Editar/Eliminar
            res = await _usuarioRepository.EliminarUsuario(guid);
            if (res.Success)
            {
                return Ok(res);
            }
            else return BadRequest(res);
        }
    }
}
