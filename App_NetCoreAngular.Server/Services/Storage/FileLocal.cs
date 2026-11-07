using App_NetCoreAngular.Server.Models.Files;
using App_NetCoreAngular.Server.Models.Shared;

namespace App_NetCoreAngular.Server.Providers.FileProvider
{
    // Clase para manejar los archivos de forma local, wwwroot
    public class FileLocal
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string localDir;
        private readonly string webRootPath = "";

        public FileLocal(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            localDir = _configuration["FileServer:Local"] ?? "";
            webRootPath = _webHostEnvironment.WebRootPath;
        }

        public async Task<ResponseApi<string>> GuardarFile(string directory, SingleUploadFile file)
        {
            var res = new ResponseApi<string>() { Success = false };
            try
            {
                // Obtenemos path del file
                string filePath = Path.Combine(webRootPath, localDir, directory);

                // Validamos existencia de directorio
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                // Generamos nuevo nombre
                var filePathGuid = Path.Combine(filePath, file.GetFullName());

                // Creamos stream
                using (var stream = new FileStream(filePathGuid, FileMode.Create))
                {
                    // Copiamos archivo
                    if (file.File != null)
                        await file.File.CopyToAsync(stream);
                }

                res.Success = true;
                res.Payload = filePathGuid;
            }
            catch (Exception e)
            {
                res.Error = "Error al guardar el archivo.";
            }

            return res;
        }
        public async Task<ResponseApi<string>> EliminarFile(string directory, string fileName)
        {
            var res = new ResponseApi<string>() { Success = false };
            try
            {
                // Obtenemos path del file
                string filePath = Path.Combine(webRootPath, localDir, directory);

                // Validamos existencia de file
                if (File.Exists(filePath))
                    File.Delete(filePath);

                res.Success = true;
                res.Payload = filePath;
            }
            catch (Exception e)
            {
                res.Error = "Error al eliminar el archivo.";
            }

            return res;
        }
        public async Task<ResponseApi<FileStream>> ObtenerFile(string directory, string fileName)
        {
            var res = new ResponseApi<FileStream>() { Success = false };
            try
            {
                // Obtenemos path del file
                string filePath = Path.Combine(webRootPath, localDir, directory, fileName);

                if (File.Exists(filePath))
                {

                    // Obtenemos stream de datos
                    FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    // byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    //string contentType = "application/pdf";
                    //string fileName = "downloaded_document.pdf";
                    //return File(fileStream, contentType, fileName);
                    //return Results.File(fileStream, contentType, fileName);

                    res.Success = true;
                    res.Payload = fileStream;
                }
                else
                {
                    res.Error = "Error al obtener el archivo.";
                }
            }
            catch (Exception e)
            {
                res.Error = "Error al obtener el archivo.";
            }

            return res;
        }

        public async Task<ResponseApi<string>> CrearDirectorio(string directory)
        {
            var res = new ResponseApi<string>() { Success = false };
            try
            {
                // Obtenemos path del direcotrio
                string filePath = Path.Combine(webRootPath, localDir, directory);

                // Validamos existencia de directorio
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                res.Success = true;
                res.Payload = filePath;
            }
            catch (Exception e)
            {
                res.Error = "Error al crear el directorio.";
            }

            return res;
        }

        public async Task<ResponseApi<string>> EliminarDirectorio(string directory, bool recursive)
        {
            var res = new ResponseApi<string>() { Success = false };
            try
            {
                // Obtenemos path del direcotrio
                string filePath = Path.Combine(webRootPath, localDir, directory);

                // Validamos existencia de directorio
                if (Directory.Exists(filePath))
                    Directory.Delete(filePath, recursive);

                res.Success = true;
                res.Payload = filePath;
            }
            catch (Exception e)
            {
                res.Error = "Error al eliminar el directorio.";
            }

            return res;
        }
    }
}
