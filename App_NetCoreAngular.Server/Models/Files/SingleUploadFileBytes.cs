namespace App_NetCoreAngular.Server.Models.Files
{
    public class SingleUploadFileBytes
    {
        public required string Container { get; set; }
        public required string Directory { get; set; }
        public required string FileName { get; set; }
        public required string Extension { get; set; }
        public byte[]? File { get; set; }

        public string GetFullDir()
        {
            return String.Concat(Directory, '/', FileName, '.', Extension);
        }

        public string GetFullName()
        {
            return String.Concat(FileName, '.', Extension);
        }
    }
}
