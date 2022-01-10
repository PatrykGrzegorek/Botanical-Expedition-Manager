using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Extensions.Selectors
{
    public static class ObservationSort
    {
        public static IQueryable<Observation> ApplySort(this IQueryable<Observation> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Observation, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.ObservationId;
                    break;
                case 2:
                    orderSelector = d => d.Date;
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
