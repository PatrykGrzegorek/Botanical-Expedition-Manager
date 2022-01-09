using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Extensions.Selectors
{
    public static class MuseumSort
    {
        public static IQueryable<Museum> ApplySort(this IQueryable<Museum> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Museum, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.MuseumId;
                    break;
                case 2:
                    orderSelector = d => d.Name;
                    break;
                case 3:
                    orderSelector = d => d.Country;
                    break;
                case 4:
                    orderSelector = d => d.City;
                    break;
                case 5:
                    orderSelector = d => d.StreetName;
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
