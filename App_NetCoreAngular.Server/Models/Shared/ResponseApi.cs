namespace App_NetCoreAngular.Server.Models.Shared
{
    public class ResponseApi<T>
    {
        public bool Success { get; set; }
        public T? Payload { get; set; }
        public string? Error { get; set; }
        public ResponsePaginator? Paginator { get; set; }
    }
}
