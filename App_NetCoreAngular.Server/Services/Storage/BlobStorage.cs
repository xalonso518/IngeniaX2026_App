using App_NetCoreAngular.Server.Models.Files;
using App_NetCoreAngular.Server.Models.Shared;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace App_NetCoreAngular.Server.Services.Storage
{

    public class BlobStorage
    {
        private readonly IConfiguration _configuration;
        private readonly string apiKey;
        private readonly CloudStorageAccount _storageAccount;

        public BlobStorage(
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            apiKey = _configuration["BlobApi"];
            _storageAccount = CloudStorageAccount.Parse(apiKey);
        }

        public Task<bool> ContainerExist(string name)
        {
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(name);
            return container.ExistsAsync();
        }

        private Task<bool> ContainerExist(string name, CloudBlobClient blobClient)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(name);
            return container.ExistsAsync();
        }

        private async Task<bool> CreateContainer(CloudBlobClient blobClient, string name)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(name);
            await container.CreateAsync();
            return await container.ExistsAsync();
        }

        public async Task<ResponseApi<string>> UploadFileLite(SingleUploadFile file)
        {
            var response = new ResponseApi<string>() { Success = false };
            try
            {
                CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(file.Container);
                CloudBlockBlob blob = container.GetBlockBlobReference(file.GetFullDir());

                using (var fileStream = file.File.OpenReadStream())
                {
                    await blob.UploadFromStreamAsync(fileStream);
                    response.Success = true;
                    response.Payload = blob.Uri.AbsolutePath;
                }
            }
            catch (Exception e)
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }


        public async Task<ResponseApi<string>> UploadFileLite(SingleUploadFilePath file)
        {
            var response = new ResponseApi<string>() { Success = false };
            try
            {
                CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(file.Container);
                CloudBlockBlob blob = container.GetBlockBlobReference(file.GetFullDir());

                await blob.UploadFromFileAsync(file.Path);
                response.Success = true;
                response.Payload = blob.Uri.AbsolutePath;
            }
            catch (Exception e)
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }

        public async Task<ResponseApi<string>> UploadFileLite(SingleUploadFileBytes file)
        {
            var response = new ResponseApi<string>() { Success = false };
            try
            {
                CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(file.Container);
                CloudBlockBlob blob = container.GetBlockBlobReference(file.GetFullDir());

                await blob.UploadFromByteArrayAsync(file.File, 0, file.File.Length);
                response.Success = true;
                response.Payload = blob.Uri.AbsolutePath;
            }
            catch (Exception e)
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }

        public async Task<ResponseApi<bool>> UploadFilesLite(List<SingleUploadFile> files)
        {
            var response = new ResponseApi<bool>() { Success = false };
            try
            {
                CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
                foreach (var file in files)
                {
                    CloudBlobContainer container = blobClient.GetContainerReference(file.Container);
                    CloudBlockBlob blob = container.GetBlockBlobReference(file.GetFullDir());

                    using (var fileStream = file.File.OpenReadStream())
                    {
                        await blob.UploadFromStreamAsync(fileStream);
                        response.Success = true;
                        response.Payload = true;
                    }
                }
            }
            catch (Exception e)
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }

        public async Task<ResponseApi<bool>> UploadFilesLite(List<SingleUploadFileBytes> files)
        {
            var response = new ResponseApi<bool>() { Success = false };
            try
            {
                CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
                foreach (var file in files)
                {
                    CloudBlobContainer container = blobClient.GetContainerReference(file.Container);
                    CloudBlockBlob blob = container.GetBlockBlobReference(file.GetFullDir());

                    await blob.UploadFromByteArrayAsync(file.File, 0, file.File.Length);
                    response.Success = true;
                    response.Payload = true;
                }
            }
            catch (Exception e)
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }

        public async Task<ResponseApi<string>> UploadFile(SingleUploadFile file)
        {
            var response = new ResponseApi<string>() { Success = false };

            try
            {
                if (file != null && file.File != null && !String.IsNullOrWhiteSpace(file.Container))
                {
                    CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(file.Container);
                    await container.CreateIfNotExistsAsync();

                    CloudBlockBlob blob = container.GetBlockBlobReference(file.GetFullDir());
                    if (!await blob.ExistsAsync())
                    {
                        using (var fileStream = file.File.OpenReadStream())
                        {
                            await blob.UploadFromStreamAsync(fileStream);
                            response.Success = true;
                            response.Payload = blob.Uri.AbsolutePath;
                        }
                    }
                    else
                    {
                        response.Error = "Error en blob storage";
                    }

                }
                else
                {
                    response.Error = "Error en blob storage";
                }
            }
            catch (StorageException e)
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }

        public Task<ResponseApi<bool>> UploadFiles(List<SingleUploadFile> files)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseApi<bool>> DeleteFile(string container, string fileName)
        {
            var response = new ResponseApi<bool>() { Success = false };
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer containerBlob = blobClient.GetContainerReference(container);
            var existContainer = await containerBlob.ExistsAsync();
            if (existContainer)
            {
                CloudBlockBlob blob = containerBlob.GetBlockBlobReference(fileName);
                await blob.DeleteAsync();
                response.Success = true;
                response.Payload = true;
            }
            else
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }



        public Task<ResponseApi<bool>> ExistFile(SingleUploadFile file)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseApi<string>> GetDirectionFile(SingleUploadFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseApi<string>> GetSasFile(string container, string fileName, double hours)
        {
            var response = new ResponseApi<string>() { Success = false };
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer containerBlob = blobClient.GetContainerReference(container);
            var existContainer = await containerBlob.ExistsAsync();
            if (existContainer)
            {
                CloudBlockBlob blob = containerBlob.GetBlockBlobReference(fileName);
                SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy
                {
                    SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5),
                    SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(hours),
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write
                };
                string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);
                response.Success = true;
                response.Payload = blob.Uri + sasBlobToken;
            }
            else
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }

        public async Task<ResponseApi<string>> GetSasFile(string fullFileName, double hours)
        {
            var arreglo = fullFileName.Split('/');
            var container = arreglo[0];
            var fileName = "";

            for (var i = 1; i < arreglo.Length; i++)
            {
                Console.WriteLine(arreglo[i]);
                fileName += arreglo[i];
                if (i != arreglo.Length - 1) fileName += "/";
            }

            var response = new ResponseApi<string>() { Success = false };
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer containerBlob = blobClient.GetContainerReference(container);
            var existContainer = await containerBlob.ExistsAsync();
            if (existContainer)
            {
                CloudBlockBlob blob = containerBlob.GetBlockBlobReference(fileName);
                SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy
                {
                    SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5),
                    SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(hours),
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write
                };
                string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);
                response.Success = true;
                response.Payload = blob.Uri + sasBlobToken;
            }
            else
            {
                response.Error = "Error en blob storage";
            }
            Console.WriteLine(response.Payload);
            return response;
        }


        public async Task<ResponseApi<string>> GetReference(string container, string fileName)
        {
            var response = new ResponseApi<string>() { Success = false };
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer containerBlob = blobClient.GetContainerReference(container);
            var existContainer = await containerBlob.ExistsAsync();
            if (existContainer)
            {
                CloudBlockBlob blob = containerBlob.GetBlockBlobReference(fileName);
                response.Success = true;
                response.Payload = blob.Uri.ToString();
            }
            else
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }

        public async Task<ResponseApi<bool>> CreateContainer(string container)
        {
            var response = new ResponseApi<bool>() { Success = false };
            try
            {
                CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
                CloudBlobContainer containerBlob = blobClient.GetContainerReference(container);
                response.Success = await containerBlob.CreateIfNotExistsAsync();
            }
            catch (StorageException e)
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }

        public async Task<ResponseApi<byte[]>> GetFileBlob(string container, string fileName)
        {
            var response = new ResponseApi<byte[]>() { Success = false };
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer containerBlob = blobClient.GetContainerReference(container);
            var existContainer = await containerBlob.ExistsAsync();
            if (existContainer)
            {
                byte[] file;
                CloudBlockBlob blobFile = containerBlob.GetBlockBlobReference(fileName);
                var ss = await blobFile.ExistsAsync();
                using (var stream = new MemoryStream())
                {
                    await blobFile.DownloadToStreamAsync(stream);
                    file = stream.ToArray();
                }
                if (file != null && file.Length > 0)
                {
                    response.Success = true;
                    response.Payload = file;
                    return response;
                }
            }
            response.Error = "Error en blob storage";
            return response;
        }

        public async Task<ResponseApi<byte[]>> GetFileBlob(string fullFileName)
        {
            var arreglo = fullFileName.Split('/');
            var container = arreglo[0];
            var fileName = "";

            for (var i = 1; i < arreglo.Length; i++)
            {
                //  Console.WriteLine(arreglo[i]);
                fileName += arreglo[i];
                if (i != arreglo.Length - 1) fileName += "/";
            }

            var response = new ResponseApi<byte[]>() { Success = false };
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer containerBlob = blobClient.GetContainerReference(container);
            var existContainer = await containerBlob.ExistsAsync();
            if (existContainer)
            {
                byte[] file;
                CloudBlockBlob blobFile = containerBlob.GetBlockBlobReference(fileName);

                using (var stream = new MemoryStream())
                {
                    await blobFile.DownloadToStreamAsync(stream);
                    file = stream.ToArray();
                }
                if (file != null && file.Length > 0)
                {
                    response.Success = true;
                    response.Payload = file;
                    return response;
                }
            }
            response.Error = "Error en blob storage";
            return response;
        }

        public async Task<ResponseApi<byte[]>> GetFileBlobToBackUp(string fullFileName)
        {
            var arreglo = fullFileName.Split('/');
            var container = arreglo[0];
            var fileName = "";

            for (var i = 1; i < arreglo.Length; i++)
            {
                // Console.WriteLine(arreglo[i]);
                fileName += arreglo[i];
                if (i != arreglo.Length - 1) fileName += "/";
            }

            var response = new ResponseApi<byte[]>() { Success = false };
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer containerBlob = blobClient.GetContainerReference(container);
            var existContainer = await containerBlob.ExistsAsync();
            if (existContainer)
            {
                var listaBytes = new List<byte>();
                byte[] file = listaBytes.ToArray();
                CloudBlockBlob blobFile = containerBlob.GetBlockBlobReference(fileName);


                var existeBlob = await blobFile.ExistsAsync();


                if (existeBlob)
                    using (var stream = new MemoryStream())
                    {
                        await blobFile.DownloadToStreamAsync(stream);
                        file = stream.ToArray();
                    }

                if (file != null && file.Length >= 0)
                {
                    response.Success = true;
                    response.Payload = file;
                    return response;
                }
            }
            response.Error = "Error en blob storage";
            return response;
        }

        public async Task<ResponseApi<bool>> DeleteContainer(string container)
        {
            var response = new ResponseApi<bool>() { Success = false };
            try
            {
                CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
                CloudBlobContainer containerBlob = blobClient.GetContainerReference(container);
                response.Success = await containerBlob.DeleteIfExistsAsync();
            }
            catch (StorageException e)
            {
                response.Error = "Error en blob storage";
            }
            return response;
        }
    }
}
