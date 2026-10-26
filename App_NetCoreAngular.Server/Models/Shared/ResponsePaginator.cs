namespace App_NetCoreAngular.Server.Models.Shared
{
    public class ResponsePaginator
    {
        private const int MAX_PAGE_SIZE = 25;
        public int Count { get; set; }
        public string? PageIndex { get; set; }

        public int PageSize { get; set; } = MAX_PAGE_SIZE;
        public int Total { get; set; }
    }
    
    public class RequestPaginator
    {
        public RequestPaginator(string page)
        {
            var pInt = 0;
            int.TryParse(page, out pInt);
            Page = pInt == 0 ? 1 : pInt;
            Skip = (Page - 1) * MAX_PAGE_SIZE;
            Take = MAX_PAGE_SIZE;
            Word = "";
            Sort = "";
        }

        public const int MAX_PAGE_SIZE = 25;
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Word { get; set; }
        public int Page { get; set; }
        public string Sort { get; set; }
        public bool Desc { get; set; }
    }
}
