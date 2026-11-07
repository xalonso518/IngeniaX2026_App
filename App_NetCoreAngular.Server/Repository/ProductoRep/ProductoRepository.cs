
using App_NetCoreAngular.Server.Models.Files;
using App_NetCoreAngular.Server.Models.Productos;
using App_NetCoreAngular.Server.Models.Shared;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;
using System.Net;
using App_NetCoreAngular.Server.EntityFramework;
using App_IngeniaX_2026.Server.EntityFramework;
using Microsoft.EntityFrameworkCore;
using App_NetCoreAngular.Server.Services.Storage;

namespace App_NetCoreAngular.Server.Repository.ProductoRep
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDataContext _context;
        private readonly BlobStorage _blobStorage;
        public ProductoRepository(
            AppDataContext context,
            BlobStorage blobStorage)
        {
            _context = context;
            _blobStorage = blobStorage;
        }
        public Producto Crear(ProductoForm producto)
        {
            var nProducto = new Producto
            {
                Nombre = producto.Nombre,
                Codigo = producto.Codigo,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Existencia = producto.Existencia,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Estado = Estado.Activo
            };

            return nProducto;
        }

        public async Task<ResponseApi<long>> Agregar(Producto producto)
        {
            var res = new ResponseApi<long>() { Success = false };
            try
            {
                var rAdd = await _context.Productos.AddAsync(producto);
                var rSaveContext = await _context.SaveChangesAsync();
                if (rSaveContext >= 1)
                {
                    res.Success = true;
                    res.Payload = producto.ProductoId;
                }
                else
                {
                    res.Error = "No se pudo agregar el nuevo producto.";
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }

        public async Task<ResponseApi<List<ProductoSimpleView>>> Buscar(string page, string sortBy, string? filtro = "")
        {
            // Definimos respuesta
            var res = new ResponseApi<List<ProductoSimpleView>>() { Success = false };

            // Iniciamos expresion de query indicando que la tabla y campos a validar
            var expression = _context.Productos
                           .Where(r => r.Estado != Estado.Baja);

            // Si tiene valor en filtro, agregamos una validacion a la expresion
            if (!String.IsNullOrWhiteSpace(filtro))
            {
                expression = expression.Where(r => r.Nombre.Contains(filtro) || r.Codigo.Contains(filtro));
            }

            // Especificamos que campos debe traer
            var query = expression.Select(r => new ProductoSimpleView
            {
                ProductoId = r.ProductoId,
                Nombre = r.Nombre,
                Codigo = r.Codigo,
                Precio = r.Precio,
                Existencia = r.Existencia,
                Imagen = r.Imagen,
                Estado = r.Estado
            });

            // Indicamos la forma de ordenar
            query = sortBy.ToLower() switch
            {
                "codigo_desc" => query.OrderByDescending(r => r.Codigo),
                "codigo" => query.OrderBy(p => p.Codigo),
                "nombre_desc" => query.OrderByDescending(r => r.Nombre),
                "nombre" => query.OrderBy(p => p.Nombre),
                _ => query.OrderBy(p => p.Nombre) // Default sort valor
            };

            // Ejecutamosel query usando un método genérico que se encargará de ejecutar la consulta y obtener el paginador
            res = await PageQuery<ProductoSimpleView>.PageSearch(_context, query, page);
            return res;
        }

        public async Task<ResponseApi<ProductoView>> BuscarPorId(long productoId)
        {
            var res = new ResponseApi<ProductoView>() { Success = false };
            try
            {
                var rQuery = await _context.Productos
                    .Where(r => r.ProductoId == productoId && r.Estado == Estado.Activo)
                    .Select(r => new ProductoView()
                    {
                        ProductoId = productoId,
                        Nombre = r.Nombre,
                        Codigo = r.Codigo,
                        Descripcion = r.Descripcion,
                        Imagen = r.Imagen,
                        Precio = r.Precio,
                        Existencia = r.Existencia,
                        DateUpdated = r.DateUpdated,                        
                        DateCreated = r.DateCreated,
                        Estado = r.Estado
                    }).FirstOrDefaultAsync();

                if (rQuery != null)
                {
                    res.Success = true;
                    res.Payload = rQuery;
                }
                else
                {
                    res.Success = false;
                    res.Error = "No se encontró producto.";
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }

        public async Task<ResponseApi<string>> Editar(ProductoFormEditar form)
        {
            var res = new ResponseApi<string>() { Success = false };
            try
            {
                var result = await _context.Productos.FindAsync(form.ProductoId);
                if (result != null)
                {
                    result.Nombre = form.Nombre;
                    result.Codigo = form.Codigo;
                    result.Descripcion = form.Descripcion;
                    result.Precio = form.Precio;
                    result.Existencia = form.Existencia;
                    result.DateUpdated = DateTime.UtcNow;

                    var rSaveContext = await _context.SaveChangesAsync();
                    if (rSaveContext >= 1)
                    {
                        res.Success = true;
                        res.Payload = form.ProductoId.ToString();
                    }
                    else
                    {
                        res.Error = "Error al editar producto.";
                    }
                }
                else
                {
                    res.Error = "Error al editar producto";
                }
            }
            catch (Exception)
            {
                res.Error = "Error de conexión.";
            }
            return res;
        }

        public async Task<ResponseApi<string>> EditarImagen(long productoId, string img)
        {
            var res = new ResponseApi<string>() { Success = false };

            try
            {
                var response = await _context.Productos
                    .Where(e => e.ProductoId == productoId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Imagen, img));

                if (response != 0)
                {
                    res.Success = true;
                    res.Payload = productoId.ToString();
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

        public async Task<ResponseApi<string>> EditarEstado(long productoId, Estado status)
        {
            var res = new ResponseApi<string>() { Success = false };

            try
            {
                var response = await _context.Productos
                    .Where(e => e.ProductoId == productoId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Estado, status));

                if (response != 0)
                {
                    res.Success = true;
                    res.Payload = productoId.ToString();
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

        public async Task<ResponseApi<string>> Eliminar(long productoId)
        {
            var res = new ResponseApi<string>() { Success = false };

            try
            {
                var response = await _context.Productos
                    .Where(e => e.ProductoId == productoId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.Estado, Estado.Baja));

                if (response != 0)
                {
                    res.Success = true;
                    res.Payload = productoId.ToString();
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

        // BLOB

        public async Task<ResponseApi<string>> UploadFileImagen(SingleUploadFile singleFile)
        {
            ResponseApi<string> res = await _blobStorage.UploadFile(singleFile);
            return res;
        }

        public async Task<ResponseApi<byte[]>> GetFileImagen(string container, string fileName)
        {
            var res = new ResponseApi<byte[]>() { Success = false };
            res = await _blobStorage.GetFileBlob(container, fileName);
            return res;
        }

        public async Task<ResponseApi<string>> GetFileImagenSas(string container, string fullFileName)
        {
            ResponseApi<string> res = await _blobStorage.GetSasFile(container, fullFileName, 1);
            return res;
        }

        public async Task<ResponseApi<bool>> DeleteFileImagen(string container, string fileName)
        {

            ResponseApi<bool> res = await _blobStorage.DeleteFile(container, fileName);

            return res;
        }


    }
}
