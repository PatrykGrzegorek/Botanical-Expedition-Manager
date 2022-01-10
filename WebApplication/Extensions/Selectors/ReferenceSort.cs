using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Extensions.Selectors
{
    public static class ReferenceSort
    {
        public static IQueryable<Reference> ApplySort(this IQueryable<Reference> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Reference, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.ReferenceId;
                    break;
                case 2:
                    orderSelector = d => d.Title;
                    break;
                case 3:
                    orderSelector = d => d.Year;
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
