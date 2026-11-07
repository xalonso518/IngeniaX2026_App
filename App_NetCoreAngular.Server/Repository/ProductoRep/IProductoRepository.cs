
using App_NetCoreAngular.Server.Models.Files;
using App_NetCoreAngular.Server.Models.Productos;
using App_NetCoreAngular.Server.Models.Shared;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Repository.ProductoRep
{
    public interface IProductoRepository
    {
        Producto Crear(ProductoForm producto);
        Task<ResponseApi<long>> Agregar(Producto producto);
        Task<ResponseApi<string>> Editar(ProductoFormEditar form);
        Task<ResponseApi<List<ProductoSimpleView>>> Buscar(string page, string sortBy, string? filtro = "");
        Task<ResponseApi<ProductoView>> BuscarPorId(long productoId);
        Task<ResponseApi<string>> EditarEstado(long productoId, Estado status);
        Task<ResponseApi<string>> EditarImagen(long productoId, string img);
        Task<ResponseApi<string>> Eliminar(long productoId);

        //Blob
        Task<ResponseApi<string>> UploadFileImagen(SingleUploadFile singleFile);
        Task<ResponseApi<byte[]>> GetFileImagen(string container, string fileName);
        Task<ResponseApi<string>> GetFileImagenSas(string container, string fullFileName);
        Task<ResponseApi<bool>> DeleteFileImagen(string container, string fileName);
    }
}
