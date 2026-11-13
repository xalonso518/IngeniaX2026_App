using App_NetCoreAngular.Server.Models.Productos;
using App_NetCoreAngular.Server.Models.Shared;
using App_NetCoreAngular.Server.Providers.FileProvider;
using App_NetCoreAngular.Server.Repository.ProductoRep;
using App_NetCoreAngular.Server.Services.SignalR;
using App_NetCoreAngular.Server.Utility.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace App_NetCoreAngular.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductoController : ControllerBase
    {
        // ID
        private readonly IProductoRepository _productoRepository;
        private readonly FileLocal _fileLocalRepository;
        private readonly NotificacionSignalR _signalRService;

        public ProductoController(
            IProductoRepository productoRepository,
            FileLocal fileLocalRepository,
            NotificacionSignalR signalRService
            )
        {
            _productoRepository = productoRepository;
            _fileLocalRepository = fileLocalRepository;
            _signalRService = signalRService;
        }

        // Obtenemos valores del claims del usuario autenticado
        private string usuarioId => User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0";
        private string UserName => User.Claims.FirstOrDefault(c => c.Type == "user")?.Value ?? "0";

        ///<summary>
        ///Agrega producto
        ///</summary>
        [HttpPost("agregar")]
        [Authorize(Policy = "UserActive")]
        public async Task<IActionResult> Agregar([FromBody] ProductoForm form)
        {
            ResponseApi<long> res = new ResponseApi<long>() { Success = false };

            // Crea objeto producto
            var nProducto = _productoRepository.Crear(form);

                // Enviamos notificacion de actualizacion a FRONT
                var rSignal = await _signalRService.SendAsync(UserName, "Nuevo producto: " + form.Nombre);
                return Ok(rSignal);
            // // Agrega producto
            // res = await _productoRepository.Agregar(nProducto);
            // if (res.Success)
            // {
            //     // Enviamos notificacion de actualizacion a FRONT
            //     var rSignal = await _signalRService.SendAsync(UserName, "Nuevo producto: " + form.Nombre);

            //     // Regresar valor
            //     return Ok(res);
            // }

            return BadRequest(res);
        }

        ///<summary>
        ///Obtienen lista de los productos
        ///</summary>
        [HttpGet("buscar")]
        [Authorize(Policy = "UserActive")]
        public async Task<IActionResult> Buscar([FromQuery] string page = "1", [FromQuery] string sortBy = "nombre_desc", [FromQuery] string filtro = "")
        {
            //Buscar productos
            var rQuery = await _productoRepository.Buscar(
                page,
                sortBy,
                filtro ?? ""
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
        ///Obtienen producto individual
        ///</summary>
        [HttpGet("obtener/{id}")]
        [Authorize(Policy = "UserActive")]
        public async Task<IActionResult> Obtener(long id)
        {
            var rQuery = await _productoRepository.BuscarPorId(id);
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
        ///Editar producto
        ///</summary>
        [HttpPut("editar/{id}")]
        [Authorize(Policy = "UserActive")]
        public async Task<IActionResult> Editar(long id, [FromBody] ProductoFormEditar form)
        {            
            //Editar
            var rQuery = await _productoRepository.Editar(form);
            if (rQuery.Success)
            {
                return Ok(rQuery);
            }
            else return BadRequest(rQuery);
        }

        ///<summary>
        ///Eliminar producto
        ///</summary>
        [HttpDelete("eliminar/{id}")]
        [Authorize(Policy = "UserActive")]
        public async Task<IActionResult> Eliminar(long id)
        {
            //Editar/Eliminar
            var rQuery = await _productoRepository.Eliminar(id);
            if (rQuery.Success)
            {
                return Ok(rQuery);
            }
            else return BadRequest(rQuery);
        }

        ///<summary>
        ///Editar img producto
        ///</summary>
        [HttpPut("img")]
        [Authorize(Policy = "UserActive")]
        public async Task<IActionResult> ImagenPerfil(ProductoFormImg form)
        {
            ResponseApi<string> res = new ResponseApi<string>() { Success = false };

            // Validamos archivo
            if (FilesValidationUtilities.ValidateImgExtention(form.Archivo) && FilesValidationUtilities.ValidateSize(form.Archivo))
            {
                // Crear objeto File
                var file = FilesUtilities.CreateSingleUploadFile(form.Archivo, "producto", "container", "");

                // Guardar archivo local
                var rFile = await _fileLocalRepository.GuardarFile(FilesUtilities.DIR_IMG_PROD, file);
                if (!rFile.Success) return BadRequest(rFile);

                // Editar imgPerfil producto
                var rEditar = await _productoRepository.EditarImagen(long.Parse(form.ProductoId), file.GetFullName());
                if (!rEditar.Success) return BadRequest(rEditar);

                // Return ok
                return Ok(rEditar);
            }
            else
            {
                res.Error = "Error al obtener el archivo";
            }
            return BadRequest(res);

        }


        ///<summary>
        ///Editar img producto
        ///</summary>
        [HttpPut("imgBlob")]
        [Authorize(Policy = "UserActive")]
        public async Task<IActionResult> ImagenPerfilBlob(ProductoFormImg form)
        {
            ResponseApi<string> res = new ResponseApi<string>() { Success = false };

            // Validamos archivo
            if (FilesValidationUtilities.ValidateImgExtention(form.Archivo) && FilesValidationUtilities.ValidateSize(form.Archivo))
            {
                // Crear objeto File
                var file = FilesUtilities.CreateSingleUploadFile(form.Archivo, "producto", "container", "");
                // String.Concat(nProducto.Imagen, '.', producto.Archivo.Extension)

                //Guarda archivo en blob
                var rFile = await _productoRepository.UploadFileImagen(file);
                if (!rFile.Success) return BadRequest(rFile);

                // Editar imgPerfil producto
                //var rEditar = await _productoRepository.EditarImagen(long.Parse(form.ProductoId), file.GetFullName());
                var rEditar = await _productoRepository.EditarImagen(long.Parse(form.ProductoId), file.GetFullDir());
                if (!rEditar.Success) return BadRequest(rEditar);

                // Return ok
                return Ok(rEditar);
            }
            else
            {
                res.Error = "Error al obtener el archivo";
            }
            return BadRequest(res);

        }
    }
}
