using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Extensions.Selectors
{
    public static class CollectionSort
    {
        public static IQueryable<Collection> ApplySort(this IQueryable<Collection> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Collection, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Name;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            return query;
        }
    }
}
