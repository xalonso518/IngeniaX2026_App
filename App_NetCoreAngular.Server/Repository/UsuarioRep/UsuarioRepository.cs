using App_IngeniaX_2026.Server.EntityFramework;
using App_NetCoreAngular.Server.EntityFramework;
using App_NetCoreAngular.Server.Models.Shared;
using App_NetCoreAngular.Server.Models.Usuarios;
using App_NetCoreAngular.Server.Repository.UsuariorRep;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Repository.UsuarioRep
{
    public class UsuarioRepository: IUsuarioRepository
    {
        private readonly AppDataContext _context;
        public UsuarioRepository(
            AppDataContext context)
        {
            _context = context;
        }
        public Usuario Crear(UsuarioForm usuario)
        {
            var nUsuario = new Usuario
            {
                Guid = Guid.NewGuid().ToString(),
                Username = usuario.Username,
                Password = usuario.Password,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.RolUsuario,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Estado = Estado.Activo
            };

            return nUsuario;
        }
        public async Task<ResponseApi<long>> Agregar(Usuario usuario)
        {
            var res = new ResponseApi<long>() { Success = false };
            try
            {
                var rAdd = await _context.Usuarios.AddAsync(usuario);
                var rSaveContext = await _context.SaveChangesAsync();
                if (rSaveContext >= 1)
                {
                    res.Success = true;
                    res.Payload = usuario.UsuarioId;
                }
                else
                {
                    res.Error = "No se pudo agregar el nuevo usuario.";
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }

        public async Task<ResponseApi<bool>> ValidarExistencia(string username)
        {
            var res = new ResponseApi<bool>() { Success = false };
            try
            {
                var rUser = await _context.Usuarios
                    .Where(r => r.Username == username)
                    .Select(r => r.UsuarioId).FirstOrDefaultAsync();

                // Si el usuario existe, rUser tendrá un valor distinto de 0, de lo contrario será 0
                if (rUser != 0)
                {
                    res.Success = true;
                    res.Payload = true;
                }
                else
                {
                    res.Success = true;
                    res.Payload = false;
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }

        public async Task<ResponseApi<List<UsuarioSimpleView>>> Buscar(string page, string sortBy, string rol, string? filtro = null)
        {
            // Definimos respuesta
            var res = new ResponseApi<List<UsuarioSimpleView>>() { Success = false };

            // Iniciamos expresion de query indicando que la tabla y campos a validar
            var expression = _context.Usuarios
                           .Where(r => r.Estado != Estado.Baja);

            // Si tiene valor en filtro, agregamos una validacion a la expresion
            if (!String.IsNullOrWhiteSpace(filtro))
            {
                expression = expression.Where(r => r.Nombre.Contains(filtro) || r.Username.Contains(filtro));
            }

            // Si tiene valor en rol, agregamos una validacion a la expresion
            if (!String.IsNullOrWhiteSpace(rol) && rol != "0")
            {
                if (!Int32.TryParse(rol, out int rolInt)) rolInt = 0;
                expression = expression.Where(r => r.Rol == (RolUsuario)rolInt);
            }

            // Especificamos que campos debe traer
            var query = expression.Select(r => new UsuarioSimpleView
            {
                UsuarioId = r.UsuarioId,
                Username = r.Username,
                Guid = r.Guid,
                Nombre = r.Nombre,
                ImagenPerfil = r.ImagenPerfil,
                Rol = r.Rol,
                Email = r.Email,
                Telefono = r.Telefono,
                DateCreated = r.DateCreated,
                Estado = r.Estado
            });

            // Indicamos la forma de ordenar
            query = sortBy.ToLower() switch
            {
                "user_desc" => query.OrderByDescending(r => r.Username),
                "user" => query.OrderBy(p => p.Username),
                "nombre_desc" => query.OrderByDescending(r => r.Nombre),
                "nombre" => query.OrderBy(p => p.Nombre),
                "fecha_desc" => query.OrderByDescending(p => p.DateCreated),
                _ => query.OrderBy(p => p.DateCreated) // Default sort valor
            };

            // Ejecutamosel query usando un método genérico que se encargará de ejecutar la consulta y obtener el paginador
            res = await PageQuery<UsuarioSimpleView>.PageSearch(_context, query, page);
            return res;
        }

        public async Task<ResponseApi<UsuarioView>> BuscarPorGuid(string guid)
        {
            var res = new ResponseApi<UsuarioView>() { Success = false };
            try
            {
                var rUser = await _context.Usuarios
                    .Where(r => r.Guid == guid && r.Estado == Estado.Activo)
                    .Select(r => new UsuarioView()
                    {
                        UsuarioId = r.UsuarioId,
                        Guid = r.Guid,
                        Nombre = r.Nombre,
                        Rol = r.Rol,
                        Estado = r.Estado,
                        Username = r.Username,
                        ImagenPerfil = r.ImagenPerfil,
                        Email = r.Email,
                        Telefono = r.Telefono,
                        DateUpdaated = r.DateUpdated,
                        DateCreated = r.DateCreated
                    }).FirstOrDefaultAsync();

                if (rUser != null)
                {
                    res.Success = true;
                    res.Payload = rUser;
                }
                else
                {
                    res.Success = false;
                    res.Error = "No se encontró usuario.";
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }

        public async Task<ResponseApi<string>> EditarEstado(long usuarioId, Estado status)
        {
            var res = new ResponseApi<string>() { Success = false };

            try
            {
                var response = await _context.Usuarios
                    .Where(e => e.UsuarioId == usuarioId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Estado, status));

                if (response != 0)
                {
                    res.Success = true;
                    res.Payload = usuarioId.ToString();
                }
                else
                {
                    res.Success = false;
                    res.Payload = "";
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }
        public async Task<ResponseApi<string>> Editar(UsuarioFormEditar form)
        {
            var res = new ResponseApi<string>() { Success = false };
            try
            {
                var result = await _context.Usuarios.FindAsync(form.UsuarioId);
                if (result != null)
                {
                    result.Nombre = form.Nombre;
                    result.Rol = form.Rol;
                    result.Email = form.Email;
                    result.Telefono = form.Telefono;
                    result.DateUpdated = DateTime.UtcNow;

                    var rSaveContext = await _context.SaveChangesAsync();
                    if (rSaveContext >= 1)
                    {
                        res.Success = true;
                        res.Payload = form.UsuarioId.ToString();

                    }
                    else
                    {
                        res.Error = "Error al editar usuario.";
                    }
                }
                else
                {
                    res.Error = "Error al editar usuario";
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }
        public async Task<ResponseApi<string>> EditarPassword(long usuarioId, string password)
        {
            var res = new ResponseApi<string>() { Success = false };

            try
            {
                var response = await _context.Usuarios
                    .Where(e => e.UsuarioId == usuarioId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Password, password));

                if (response != 0)
                {
                    res.Success = true;
                    res.Payload = usuarioId.ToString();
                }
                else
                {
                    res.Success = false;
                    res.Payload = "";
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }
        public async Task<ResponseApi<string>> EditarImagenPerfil(long usuarioId, string img)
        {
            var res = new ResponseApi<string>() { Success = false };

            try
            {
                var response = await _context.Usuarios
                    .Where(e => e.UsuarioId == usuarioId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.ImagenPerfil, img));

                if (response != 0)
                {
                    res.Success = true;
                    res.Payload = usuarioId.ToString();
                }
                else
                {
                    res.Success = false;
                    res.Payload = "";
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }
        public async Task<ResponseApi<string>> EliminarUsuario(string guid)
        {
            var res = new ResponseApi<string>() { Success = false };

            try
            {
                var response = await _context.Usuarios
                    .Where(e => e.Guid == guid)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Estado, Estado.Baja));

                if (response != 0)
                {
                    res.Success = true;
                    res.Payload = guid;
                }
                else
                {
                    res.Success = false;
                    res.Payload = "";
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }

    }
}
