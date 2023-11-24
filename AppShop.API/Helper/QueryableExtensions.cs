using AppShop.Share.DTOs;

namespace AppShop.API.Helper
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable,
            PaginationDTO pagination)
        {
            if (pagination.RecordsNumber is -1)
                return queryable;

            return queryable
                    .Skip((pagination.Page - 1) * pagination.RecordsNumber)
                    .Take(pagination.RecordsNumber);
        }
    }
}

