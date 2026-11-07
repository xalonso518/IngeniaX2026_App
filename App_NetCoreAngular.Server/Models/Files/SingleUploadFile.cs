using System.Net.NetworkInformation;

namespace App_NetCoreAngular.Server.Models.Files
{
    public class SingleUploadFile
    {
        public required string Container { get; set; }
        public required string Directory { get; set; }
        public required string FileName { get; set; }
        public required string Extension { get; set; }
        public IFormFile? File { get; set; }

        public string GetFullDir()
        {
            return String.Concat(Directory, '/', FileName, '.', Extension);
        }

        public string GetFullName()
        {
            return String.Concat(FileName, '.', Extension);
        }
        public short GetSize()
        {
            short size = 0;
            if(File != null)
            {
                size = (short)(File.Length / 1024);
            }
            return size;
        }
    }
}