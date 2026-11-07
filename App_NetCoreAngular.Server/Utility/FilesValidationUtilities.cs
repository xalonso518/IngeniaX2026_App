namespace App_NetCoreAngular.Server.Utility.Files
{

    public static class FilesValidationUtilities
    {
        private static readonly List<string> ALLOW_EXT_FILES = new List<string>(){ "pdf", "xml", "rar", "zip",
            "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "cert", "key" };

        private static readonly List<string> ALLOW_EXT_IMG = new List<string>() { "png", "jpg", "jpeg", "svg", "gif" };

        private const int MAX_SIZE = 1024 * 1024 * 5;

        public static bool ValidateSize(IFormFile file)
        {
            if (file.Length > MAX_SIZE) return false;
            return true;
        }

        public static bool ValidateImgExtention(IFormFile file)
        {
            if (ALLOW_EXT_IMG.Contains(FilesUtilities.GetExtention(file.FileName).ToLower())) return true;
            return false;
        }

        public static bool ValidateXmlExtention(IFormFile file)
        {
            if ("xml" == FilesUtilities.GetExtention(file.FileName).ToLower()) return true;
            return false;
        }

        public static bool ValidateImgExtention(string ext)
        {
            if (ALLOW_EXT_IMG.Contains(ext.ToLower())) return true;
            return false;
        }

        public static bool ValidateAllExtention(string ext)
        {
            if (ALLOW_EXT_FILES.Contains(ext.ToLower())) return true;
            if (ALLOW_EXT_IMG.Contains(ext.ToLower())) return true;
            return false;
        }

        public static bool ValidateFileImg(IFormFile file)
        {
            return ValidateImgExtention(file) && ValidateSize(file);
        }

        public static bool ValidateSize(List<IFormFile> files)
        {
            var flag = true;
            files.ForEach(file =>
            {
                if (file.Length > MAX_SIZE)
                {
                    flag = false;
                    return;
                }
            }
            );
            return flag;
        }

        public static bool ValidateSize(int size)
        {
            var flag = true;
            if (size > MAX_SIZE) flag = false;
            return flag;
        }

        public static bool ValidateImgExtention(List<IFormFile> files)
        {
            var flag = true;
            files.ForEach(file =>
            {
                var ext = FilesUtilities.GetExtention(file.FileName).ToLower();
                if (!ALLOW_EXT_IMG.Contains(ext))
                {
                    flag = false;
                    return;
                }
            }
            );
            return flag;
        }

        public static bool ValidateFileExtention(List<IFormFile> files)
        {
            var flag = true;
            files.ForEach(file =>
            {
                var ext = FilesUtilities.GetExtention(file.FileName).ToLower();
                if (!ALLOW_EXT_IMG.Contains(ext))
                {
                    flag = false;
                    return;
                }
            }
            );
            return flag;
        }


        // Ticket 5117 [Bookkeep Pro] Archivos adjuntos/No es posible seleccionar varios archivos
        // Se pide quitar validación de extensiones, puede ser cualquiera 28/05/2026
        // Se comentó que podría tener riesgo quitar la validación dado que  no se sabe todo su alcance en el sistema
        public static bool ValidateAllExtention(IFormFile file)
        {
            var flag = true;
            var ext = FilesUtilities.GetExtention(file.FileName).ToLower();
            if (!ALLOW_EXT_IMG.Contains(ext) && !ALLOW_EXT_FILES.Contains(ext))
            {
                //flag = false;

            }
            return flag;
        }

        // El siguiente método era  el  original 28/05/2026
        public static bool ValidateAllExtentionOriginal(IFormFile file)
        {
            var flag = true;
            var ext = FilesUtilities.GetExtention(file.FileName).ToLower();
            if (!ALLOW_EXT_IMG.Contains(ext) && !ALLOW_EXT_FILES.Contains(ext))
            {
                flag = false;
            }
            return flag;
        }

        public static bool ValidateAllExtention(List<IFormFile> files)
        {
            var flag = true;
            files.ForEach(file =>
            {
                var ext = FilesUtilities.GetExtention(file.FileName).ToLower();
                if (!ALLOW_EXT_IMG.Contains(ext) && !ALLOW_EXT_FILES.Contains(ext))
                {
                    flag = false;
                    return;
                }
            }
            );
            return flag;
        }
    }
}
