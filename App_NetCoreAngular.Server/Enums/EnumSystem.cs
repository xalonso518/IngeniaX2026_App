namespace App_NetCoreAngular.Server.Enums
{
    public class EnumSystem
    {
        public enum TypeAttachment
        {
            Zip = 1,
            Rar = 2,
            Pdf = 3,
            Jpg = 4,
            Png = 5,
            Svg = 6,
            Json = 7,
            Csv = 8,
            Xml = 9,
            Excel = 10,
            Word = 11
        }

        public enum DispositionAttachment
        {
            inline = 1,
            attachment = 2
        }

        public enum TypeTemplate
        {
            Bienvenida = 1,
            ResetPassword = 2,
            CorreoPrueba = 666
        }
    }
}
