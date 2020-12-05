using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Shared.Messages.Queries
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> PagedAsync<T>(this IQueryable<T> source, int page, int pageSize, string orderBy, string sortOrder)
        {
            var totalResults = await source.CountAsync();
            var totalPages = (int)Math.Ceiling(totalResults / (double)pageSize);
            var result = source;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                result = result.OrderBy(sortOrder == "desc" ? $"{orderBy} desc" : orderBy);
            }

            var paged = await result
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return PagedResult<T>.Create(paged, page, pageSize, totalPages, totalResults);
        }
    }

}