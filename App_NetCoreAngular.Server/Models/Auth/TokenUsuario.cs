namespace App_NetCoreAngular.Server.Models.Auth
{
    public class TokenUsuario
    {
        public required string Id { get; set; }
        public required string Usuario { get; set; }
        public required string Token { get; set; }
    }
    public class TokenReturn
    {
        public required string Token { get; set; }
    }
}
