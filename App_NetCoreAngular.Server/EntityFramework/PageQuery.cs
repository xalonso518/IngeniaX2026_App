using Microsoft.EntityFrameworkCore;
using App_NetCoreAngular.Server.Models.Shared;

namespace App_IngeniaX_2026.Server.EntityFramework
{
    // Método estático para el paginador
    public static class PageQuery<T>
    {
        public static async Task<ResponseApi<List<T>>> PageSearch(
            DbContext context,
            IQueryable<T> query, string page)
        {
            var res = new ResponseApi<List<T>>() { Success = false };
            var requestPaginator = new RequestPaginator(page);
            try
            {
                using (context)
                {
                    var total = await query.CountAsync();
                    var rQuery = await query.Skip(requestPaginator.Skip).Take(requestPaginator.Take).ToListAsync();
                    if (rQuery != null)
                    {
                        res.Success = true;
                        res.Payload = rQuery;
                        res.Paginator = new ResponsePaginator()
                        {
                            PageIndex = requestPaginator.Skip.ToString(),
                            PageSize = requestPaginator.Take,
                            Total = total
                        };
                    }
                    else
                    {
                        res.Success = true;
                    }
                }
            }
            catch (Exception e)
            {
                res.Error = "Error de conexión";
            }
            return res;
        }
    }
}
