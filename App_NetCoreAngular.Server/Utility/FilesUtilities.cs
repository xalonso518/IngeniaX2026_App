using App_NetCoreAngular.Server.Models.Files;

namespace App_NetCoreAngular.Server.Utility.Files
{

    public static class FilesUtilities
    {
        public const string DIR_IMG_USER = "img/usuario";
        public const string DIR_IMG_PROD = "img/producto";
        public static string GetExtention(string name)
        {
            string ext = "";

            var division = name.Split('.');
            ext = division.Length > 0 ? division[division.Length - 1] : "txt";

            return ext.ToLower();
        }

        public static string GetFullName(IFormFile file)
        {
            return String.Concat(GetNormalizedName(file.FileName), '.', GetExtention(file.FileName));
        }

        public static string GetNormalizedName(string name)
        {
            name = name.Contains(string.Empty) ? name.Replace("/", string.Empty) : name;

            var division = name.Split('.');
            var nameConPuntos = "";

            if (division.Length > 2)
            {
                for (var i = 0; i < division.Length - 1; i++)
                {

                    nameConPuntos += i < division.Length - 2 ? division[i] + "." : division[i];
                }
            }

            name = nameConPuntos != "" ? nameConPuntos : division.Length > 0 ? division[0] : name.Contains(string.Empty) ? name.Replace("/", string.Empty) : "archivo";



            return name;
            //  return name.Replace("/", string.Empty).Split('.')[0].ToLower();
        }

        public static string GetDirectory(string directory)
        {
            if (directory[0] == '/') directory = directory.Substring(1);
            if (directory[directory.Length - 1] == '/') directory = directory.Substring(0, directory.Length - 1);
            return directory;
        }

        public static string GetFullDirectory(IFormFile file, string directory, string name = "")
        {
            directory = GetDirectory(directory);

            string fullName = "";
            if (String.IsNullOrWhiteSpace(name)) fullName = GetFullName(file);
            else fullName = String.Concat(GetNormalizedName(name), '.', GetExtention(file.FileName));

            return String.Concat(directory, '/', fullName);
        }

        public static SingleUploadFile CreateSingleUploadFile(IFormFile file, string directory, string container, string name = "")
        {
            return new SingleUploadFile
            {
                Container = container,
                Directory = GetDirectory(directory),
                Extension = GetExtention(file.FileName),
                File = file,
                FileName = String.IsNullOrWhiteSpace(name) ? GetNormalizedName(file.FileName) : GetNormalizedName(name)
            };
        }

        public static SingleUploadFileBytes CreateSingleUploadFileBytes(byte[] file, string directory, string container, string name = "")
        {
            return new SingleUploadFileBytes
            {
                Container = container,
                Directory = GetDirectory(directory),
                Extension = GetExtention(name),
                File = file,
                FileName = GetNormalizedName(name)
            };
        }


        public static SingleUploadFilePath CreateSingleUploadFilePath(string path, string directory, string container, string name = "")
        {
            return new SingleUploadFilePath
            {
                Container = container,
                Directory = GetDirectory(directory),
                Extension = GetExtention(name),
                Path = path,
                FileName = GetNormalizedName(name)
            };
        }


    }
}
