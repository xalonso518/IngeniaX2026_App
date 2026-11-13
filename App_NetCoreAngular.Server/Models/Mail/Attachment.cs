using static App_NetCoreAngular.Server.Enums.EnumSystem;

namespace App_NetCoreAngular.Server.Models.Mail
{
    public class Attachment
    {
        private Dictionary<string, string> TypeDiccionary = new Dictionary<string, string>{
            { "Zip" , "application/zip" },
            { "Rar" , "application/x-rar-compressed" },
            { "Pdf" , "application/pdf" },
            { "Jpg" , "image/jpeg" },
            { "Png" , "image/png" },
            { "Svg" , "image/svg+xml" },
            { "Json" , "application/json" },
            { "Csv" , "text/csv" },
            { "Xml" , "application/xml" },
            { "Excel" , "application/vnd.ms-excel" },
            { "Word" , "application/msword" },
            { "Text" , "text/plain" }
        };

        private string? Content;
        private string? TypeFile;
        private string? FileName;
        private string? Disposition;
        private string? ContentId;

        public void SetContent(byte[] file)
        {
            this.Content = Convert.ToBase64String(file);
        }

        public string GetContent()
        {
            return this.Content??"";
        }

        public void SetType(string type)
        {
            if (TypeDiccionary.ContainsKey(type))
                this.TypeFile = TypeDiccionary[type];
            else this.TypeFile = TypeDiccionary["Text"];
        }

        public string GetTypeFile()
        {
            return this.TypeFile??"";
        }

        public void SetFileName(string name)
        {
            this.FileName = name;
        }

        public string GetFileName()
        {
            return this.FileName??"";
        }

        public void SetDisposition(DispositionAttachment disposition)
        {
            this.Disposition = disposition.ToString("G");
        }

        public string GetDisposition()
        {
            return this.Disposition??"";
        }

        public void SetContentId(string id)
        {
            this.ContentId = id;
        }

        public string GetContentId()
        {
            return this.ContentId??"";
        }

    }
}
